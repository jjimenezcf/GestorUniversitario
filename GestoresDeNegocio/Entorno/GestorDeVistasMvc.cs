﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Utilidades;
using ModeloDeDto.Entorno;
using ServicioDeDatos;
using ServicioDeDatos.Entorno;
using Gestor.Errores;
using ServicioDeDatos.Seguridad;
using GestorDeElementos;
using GestoresDeNegocio.Seguridad;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Linq.Dynamic.Core;
using ModeloDeDto;

namespace GestoresDeNegocio.Entorno
{

    public class GestorDeVistaMvc : GestorDeElementos<ContextoSe, VistaMvcDtm, VistaMvcDto>
    {
        public class MapearVistaMvc : Profile
        {
            public MapearVistaMvc()
            {
                CreateMap<VistaMvcDtm, VistaMvcDto>()
                .ForMember(dto => dto.Menus, dtm => dtm.MapFrom(x => x.Menus))
                .ForMember(dto => dto.Permiso, dtm => dtm.MapFrom(x => x.Permiso.Nombre));

                CreateMap<VistaMvcDto, VistaMvcDtm>()
                .ForMember(dtm => dtm.Permiso, dto => dto.Ignore())
                ;
            }
        }

        public GestorDeVistaMvc(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {

        }


        public static GestorDeVistaMvc Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeVistaMvc(contexto, mapeador);
        }

        public VistaMvcDtm CrearVistaMvc(string nombre, string controlador, string accion, bool mostrarEnModal, string elementoDto)
        {
            var v = new VistaMvcDtm();
            v.Nombre = nombre;
            v.Controlador = controlador;
            v.Accion = accion;
            v.MostrarEnModal = mostrarEnModal;
            v.ElementoDto = elementoDto;
            v = PersistirRegistro(v, new ParametrosDeNegocio(enumTipoOperacion.Insertar));
            return v;
        }

        protected override IQueryable<VistaMvcDtm> AplicarJoins(IQueryable<VistaMvcDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(p => p.Permiso);
            return registros;
        }


        protected override IQueryable<VistaMvcDtm> AplicarFiltros(IQueryable<VistaMvcDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId)
                return registros;

            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(VistaMvcDtm.Controlador).ToLower())
                {
                    if (filtro.Criterio == CriteriosDeFiltrado.igual)
                        registros = registros.Where(x => x.Controlador == filtro.Valor);

                    if (filtro.Criterio == CriteriosDeFiltrado.contiene)
                        registros = registros.Where(x => x.Controlador.Contains(filtro.Valor));
                }
                if (filtro.Clausula.ToLower() == nameof(VistaMvcDtm.Accion).ToLower())
                {
                    if (filtro.Criterio == CriteriosDeFiltrado.igual)
                        registros = registros.Where(x => x.Accion == filtro.Valor);
                }
            }

            return registros;
        }

        public bool TienePermisos(UsuarioDtm usuarioConectado, string vista)
        {
            if (usuarioConectado.EsAdministrador)
                return true;

            var vistaDtm = LeerVistaMvc(vista);
            var cache = ServicioDeCaches.Obtener($"{nameof(GestorDeVistaMvc)}.{nameof(TienePermisos)}");
            var indice = $"Usuario:{usuarioConectado.Id} Permiso:{vistaDtm.IdPermiso}";

            if (!cache.ContainsKey(indice))
            {
                var gestor = GestorDePermisosDeUnUsuario.Gestor(Contexto, Mapeador);

                var filtros = new List<ClausulaDeFiltrado>
                {
                    new ClausulaDeFiltrado { Clausula = nameof(PermisosDeUnUsuarioDtm.IdUsuario), Criterio = CriteriosDeFiltrado.igual, Valor = usuarioConectado.Id.ToString()},
                    new ClausulaDeFiltrado { Clausula = nameof(PermisosDeUnUsuarioDtm.IdPermiso), Criterio = CriteriosDeFiltrado.igual, Valor = vistaDtm.IdPermiso.ToString() }
                };

                cache[indice] = gestor.Contar(filtros) > 0;
            }
            return (bool)cache[indice];

        }


        public List<VistaMvcDto> LeerVistas(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros)
        {            
            var registros = LeerRegistrosPorNombre(posicion, cantidad, filtros);
            return MapearElementos(registros).ToList();
        }

        public VistaMvcDtm LeerVistaMvc(string vistaMvc)
        {
            var vista = ValidarParametroAntesDeLeerVistaMvc(vistaMvc);

            var cache = ServicioDeCaches.Obtener(nameof(LeerVistaMvc));
            if (!cache.ContainsKey(vista))
            {
                var filtros = new List<ClausulaDeFiltrado>
                {
                    new ClausulaDeFiltrado { Clausula = nameof(VistaMvcDtm.Controlador), Criterio = CriteriosDeFiltrado.igual, Valor = vista.Split(".")[0]},
                    new ClausulaDeFiltrado { Clausula = nameof(VistaMvcDtm.Accion), Criterio = CriteriosDeFiltrado.igual, Valor = vista.Split(".")[1] }
                };
                var vistas = LeerRegistrosPorNombre(0, -1, filtros);
                if (vistas.Count != 1)
                {
                    if (vistas.Count == 0)
                        GestorDeErrores.Emitir($"No se ha localizado la vistaMvc {vista}");
                    else
                        GestorDeErrores.Emitir($"Se han localizado {vistas.Count} vistasMvc para {vista}");
                }

                if (vistas == null)
                {
                    GestorDeErrores.Emitir($"Defina la vista {vista} en BD");
                }

                cache[$"{vista}"] = vistas[0];
            }

            return (VistaMvcDtm)cache[$"{vista}"];
        }

        private static string ValidarParametroAntesDeLeerVistaMvc(string vistaMvc)
        {
            if (vistaMvc.IsNullOrEmpty())
                GestorDeErrores.Emitir($"Debe indicar el nombre del controlador y vista a buscar");

            var partes = vistaMvc.Split(".");

            if (partes.Length != 2)
                GestorDeErrores.Emitir($"El valor proporcionado {vistaMvc} no es válido, ha de seguir el patrón Controlador.Vista");

            var nombreDelControlador = partes[0];
            var nombreDeLaVista = partes[1];
            if (nombreDelControlador.IsNullOrEmpty() || nombreDeLaVista.IsNullOrEmpty())
                GestorDeErrores.Emitir($"falta información del controlador o la vista a buscar, usted ha proporcionado, controlado: {nombreDelControlador}, vista: {nombreDeLaVista}");

            return $"{nombreDelControlador}.{nombreDeLaVista}";
        }

        protected override void AntesDePersistir(VistaMvcDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistir(registro, parametros);

            if (!registro.ElementoDto.IsNullOrEmpty())
               ExtensionesDto.ObtenerTypoDto(registro.ElementoDto);

            if (parametros.Operacion == enumTipoOperacion.Insertar)
            {
                var permiso = GestorDePermisos.CrearObtener(Contexto, Mapeador, registro.Nombre, enumClaseDePermiso.Vista);
                registro.IdPermiso = permiso.Id;
            }
            if (parametros.Operacion == enumTipoOperacion.Modificar)
            {
                registro.IdPermiso = ((VistaMvcDtm)parametros.registroEnBd).IdPermiso;
            }
        }

        protected override void DespuesDePersistir(VistaMvcDtm registro, ParametrosDeNegocio parametros)
        {
            base.DespuesDePersistir(registro, parametros);
            var RegistroEnBD = ((VistaMvcDtm)parametros.registroEnBd);

            if (parametros.Operacion == enumTipoOperacion.Modificar && RegistroEnBD.Nombre != registro.Nombre)
                GestorDePermisos.ModificarPermisoFuncional(Contexto, Mapeador, RegistroEnBD.Permiso, registro.Nombre, enumClaseDePermiso.Vista);

            if (parametros.Operacion == enumTipoOperacion.Eliminar)
                GestorDePermisos.Eliminar(Contexto, Mapeador, RegistroEnBD.Permiso);

            if (parametros.Operacion == enumTipoOperacion.Eliminar || parametros.Operacion == enumTipoOperacion.Modificar)
            {
                ServicioDeCaches.EliminarElemento(nameof(LeerVistaMvc), $"{registro.Controlador}.{ registro.Accion}");
                ServicioDeCaches.EliminarElemento(nameof(ExtensionesDto.UrlParaMostrarUnDto), registro.ElementoDto);
            }

            ServicioDeCaches.EliminarCache(nameof(GestorDeArbolDeMenu.LeerArbolDeMenu));
        }

        protected override void DespuesDeMapearElemento(VistaMvcDtm registro, VistaMvcDto elemento, ParametrosDeMapeo parametros)
        {
            base.DespuesDeMapearElemento(registro, elemento, parametros);
        }



    }

}

