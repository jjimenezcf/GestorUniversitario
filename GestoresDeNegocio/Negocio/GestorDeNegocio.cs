using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ServicioDeDatos.Negocio;
using ServicioDeDatos;
using ModeloDeDto.Negocio;
using GestorDeElementos;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.Seguridad;
using GestoresDeNegocio.Entorno;
using GestoresDeNegocio.Seguridad;
using Microsoft.EntityFrameworkCore;
using Utilidades;
using Gestor.Errores;
using System;
using System.Reflection;

namespace GestoresDeNegocio.Negocio
{

    public class GestorDeNegocio : GestorDeElementos<ContextoSe, NegocioDtm, NegocioDto>
    {

        public class MapearNegocio : Profile
        {
            public MapearNegocio()
            {
                CreateMap<NegocioDtm, NegocioDto>()
                .ForMember(dto => dto.PermisoDeGestor, dtm => dtm.MapFrom(x => x.PermisoDeGestor.Nombre))
                .ForMember(dto => dto.PermisoDeAdministrador, dtm => dtm.MapFrom(x => x.PermisoDeAdministrador.Nombre))
                .ForMember(dto => dto.PermisoDeConsultor, dtm => dtm.MapFrom(x => x.PermisoDeConsultor.Nombre));

                CreateMap<NegocioDto, NegocioDtm>()
                .ForMember(dtm => dtm.PermisoDeGestor, dto => dto.Ignore())
                .ForMember(dtm => dtm.PermisoDeAdministrador, dto => dto.Ignore())
                .ForMember(dtm => dtm.PermisoDeConsultor, dto => dto.Ignore());
            }
        }

        public GestorDeNegocio(ContextoSe contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }
        public static GestorDeNegocio Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeNegocio(contexto, mapeador);
        }


        protected override IQueryable<NegocioDtm> AplicarFiltros(IQueryable<NegocioDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId)
                return registros;

            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(NegocioDtm.Elemento).ToLower())
                {
                    if (filtro.Criterio == CriteriosDeFiltrado.igual)
                        return registros.Where(x => x.Elemento == filtro.Valor);

                    if (filtro.Criterio == CriteriosDeFiltrado.contiene)
                        return registros.Where(x => x.Elemento.Contains(filtro.Valor));

                    if (filtro.Criterio == CriteriosDeFiltrado.esAlgunoDe)
                    {
                        var ids = filtro.Valor.Split(',');
                        int[] lista = Array.Empty<int>();
                        int i = 0;
                        foreach (string s in ids)
                        {
                            lista[i] = s.Entero();
                            i++;
                        }

                        return registros.Where(x => lista.Contains(x.Id));
                    }
                }
            }

            return registros;
        }

        protected override IQueryable<NegocioDtm> AplicarJoins(IQueryable<NegocioDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(p => p.PermisoDeAdministrador);
            registros = registros.Include(p => p.PermisoDeConsultor);
            registros = registros.Include(p => p.PermisoDeGestor);
            return registros;
        }


        public bool TienePermisos(UsuarioDtm usuarioConectado, enumModoDeAccesoDeDatos permisosNecesarios, enumNegocio negocio)
        {
            if (!NegociosDeSe.UsaSeguridad(negocio))
                return true;

            var estaActivo = NegocioActivo(negocio);

            if (!estaActivo && (permisosNecesarios == enumModoDeAccesoDeDatos.Administrador || permisosNecesarios == enumModoDeAccesoDeDatos.Gestor))
                return false;

            if (usuarioConectado.EsAdministrador)
                return true;

            var negocioDtm = LeerRegistroCacheado(nameof(NegocioDtm.Nombre), NegociosDeSe.ToString(negocio));
            var cache = ServicioDeCaches.Obtener($"{nameof(GestorDeNegocio)}.{nameof(TienePermisos)}");
            var indice = $"{usuarioConectado.Id}.{negocioDtm.Id}.{permisosNecesarios}";

            if (!cache.ContainsKey(indice))
            {
                var gestor = GestorDePermisosDeUnUsuario.Gestor(Contexto, Mapeador);

                var filtros = new List<ClausulaDeFiltrado>
                {
                    new ClausulaDeFiltrado { Clausula = nameof(PermisosDeUnUsuarioDtm.IdUsuario), Criterio = CriteriosDeFiltrado.igual, Valor = usuarioConectado.Id.ToString()}
                };

                if (permisosNecesarios == enumModoDeAccesoDeDatos.Administrador)
                    filtros.Add(new ClausulaDeFiltrado { Clausula = nameof(PermisosDeUnUsuarioDtm.IdPermiso), Criterio = CriteriosDeFiltrado.igual, Valor = negocioDtm.IdPermisoDeAdministrador.ToString() });

                if (permisosNecesarios == enumModoDeAccesoDeDatos.Gestor)
                    filtros.Add(new ClausulaDeFiltrado { Clausula = nameof(PermisosDeUnUsuarioDtm.IdPermiso), Criterio = CriteriosDeFiltrado.esAlgunoDe, Valor = $"{negocioDtm.IdPermisoDeGestor},{negocioDtm.IdPermisoDeAdministrador}" });

                if (permisosNecesarios == enumModoDeAccesoDeDatos.Consultor)
                    filtros.Add(new ClausulaDeFiltrado { Clausula = nameof(PermisosDeUnUsuarioDtm.IdPermiso), Criterio = CriteriosDeFiltrado.esAlgunoDe, Valor = $"{negocioDtm.IdPermisoDeConsultor},{negocioDtm.IdPermisoDeGestor},{negocioDtm.IdPermisoDeAdministrador}" });

                cache[indice] = gestor.Contar(filtros) > 0;
            }
            return (bool)cache[indice];
        }

        protected override void AntesDePersistir(NegocioDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistir(registro, parametros);

            if (parametros.Operacion == TipoOperacion.Insertar)
            {
                registro.IdPermisoDeAdministrador = GestorDePermisos.CrearObtener(Contexto, Mapeador, registro.Nombre, enumClaseDePermiso.Negocio, enumModoDeAccesoDeDatos.Administrador).Id;
                registro.IdPermisoDeGestor = GestorDePermisos.CrearObtener(Contexto, Mapeador, registro.Nombre, enumClaseDePermiso.Negocio, enumModoDeAccesoDeDatos.Gestor).Id;
                registro.IdPermisoDeConsultor = GestorDePermisos.CrearObtener(Contexto, Mapeador, registro.Nombre, enumClaseDePermiso.Negocio, enumModoDeAccesoDeDatos.Consultor).Id;
            }

            if (parametros.Operacion == TipoOperacion.Modificar)
            {
                registro.IdPermisoDeAdministrador = GestorDePermisos.ModificarPermisoDeDatos(Contexto, Mapeador, RegistroEnBD.PermisoDeAdministrador, registro.Nombre, enumClaseDePermiso.Negocio, enumModoDeAccesoDeDatos.Administrador).Id;
                registro.IdPermisoDeGestor = GestorDePermisos.ModificarPermisoDeDatos(Contexto, Mapeador, RegistroEnBD.PermisoDeGestor, registro.Nombre, enumClaseDePermiso.Negocio, enumModoDeAccesoDeDatos.Gestor).Id;
                registro.IdPermisoDeConsultor = GestorDePermisos.ModificarPermisoDeDatos(Contexto, Mapeador, RegistroEnBD.PermisoDeConsultor, registro.Nombre, enumClaseDePermiso.Negocio, enumModoDeAccesoDeDatos.Consultor).Id;
            }

        }

        protected override void AntesDePersistirValidarRegistro(NegocioDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistirValidarRegistro(registro, parametros);
            if (registro.Elemento.IsNullOrEmpty())
                GestorDeErrores.Emitir($"Ha de indicar la clase del objeto {registro.Nombre} es obligatorio");

            var encontrado = false;
            var ensamblado = Assembly.Load(nameof(ServicioDeDatos));
            foreach (var clase in ensamblado.DefinedTypes)
            {
                if (clase.Name == registro.Elemento)
                {
                    encontrado = true;
                    break;
                }
            }

            if (!encontrado)
                GestorDeErrores.Emitir($"La clase del elemento {registro.Elemento} del negocio {registro.Nombre} debe existir");

        }

        public bool NegocioActivo(enumNegocio negocio)
        {
            if (!NegociosDeSe.UsaSeguridad(negocio))
                return true;

            var registro = LeerRegistroCacheado(nameof(NegocioDtm.Nombre), NegociosDeSe.ToString(negocio));
            return registro.Activo;
        }
    }
}

/*  Recorrer todos los assembly del dominio de la aplicación por reflexión y ver si hay una clase
 *
            //AppDomain currentDomain = AppDomain.CurrentDomain;
            //var a = currentDomain.GetAssemblies();
            //foreach (var ensamblado in a)
            //{
            //    foreach (var t in ensamblado.GetTypes())
            //    {
            //        if (t.Name == registro.Elemento)
            //        {
            //            encontrado = true;
            //            break;
            //        }
            //    }
            //    if (encontrado)
            //        break;
            //}
 * */