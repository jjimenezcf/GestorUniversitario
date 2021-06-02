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
using ModeloDeDto;
using ServicioDeDatos.Elemento;

namespace GestoresDeNegocio.Negocio
{

    public class GestorDeNegocios : GestorDeElementos<ContextoSe, NegocioDtm, NegocioDto>
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

        public GestorDeNegocios(ContextoSe contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        public static GestorDeNegocios Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeNegocios(contexto, mapeador);
        }

        public static enumModoDeAccesoDeDatos LeerModoDeAcceso(ContextoSe contexto, enumNegocio negocio)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            return gestor.LeerModoDeAccesoAlNegocio(contexto.DatosDeConexion.IdUsuario, negocio);
        }

        public static enumModoDeAccesoDeDatos LeerModoDeAccesoAlElemento(ContextoSe contexto, enumNegocio negocio, int id)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            return gestor.LeerModoDeAccesoAlElemento(contexto.DatosDeConexion.IdUsuario, negocio, id);
        }

        public static NegocioDtm LeerNegocio(ContextoSe contexto, enumNegocio negocio)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            return gestor.LeerNegocio(negocio);
        }

        public static NegocioDtm LeerNegocio(ContextoSe contexto, int idNegocio)
        {
            var gestor = Gestor(contexto, contexto.Mapeador);
            return gestor.LeerRegistroPorId(idNegocio, true, false, false);
        }

        internal static string ComponerUrl(TipoDtoElmento elemento)
        {
            var url = ExtensionesDto.UrlParaMostrarUnDto(elemento.ClaseDto());
            var refHtml = $@"<a href='{CacheDeVariable.Cfg_UrlBase}{url}?id={elemento.IdElemento}' target='_blank' idelemento='2041'>{elemento.Referencia}</a>";
            return refHtml;
        }

        public NegocioDtm LeerNegocio(enumNegocio negocio, bool errorSiNoHay = true)
        {
            var negocioDtm = LeerRegistro(nameof(NegocioDtm.Enumerado), negocio.ToString(), errorSiNoHay, true, false, false);
            return negocioDtm;
        }

        public NegocioDtm LeerNegocioParaModificar(enumNegocio negocio, bool errorSiNoHay = true)
        {
            var negocioDtm = LeerRegistro(nameof(NegocioDtm.Enumerado), negocio.ToString(), errorSiNoHay, true, true, false);
            return negocioDtm;
        }

        public bool NegocioActivo(enumNegocio negocio)
        {
            if (negocio == enumNegocio.No_Definido)
                return true;

            if (!NegociosDeSe.UsaSeguridad(negocio))
                return true;

            var registro = LeerRegistroCacheado(nameof(NegocioDtm.Nombre), negocio.ToNombre(), false, true);
            if (registro == null)
                GestorDeErrores.Emitir($"El negocio de {NegociosDeSe.ToNombre(negocio)} no está definido, y se ha indicado por programa que usa seguridad, defínalo como negocio");
            return registro.Activo;
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

            if (negocio == enumNegocio.Variable)
            {
                switch (permisosNecesarios)
                {
                    case enumModoDeAccesoDeDatos.Consultor: return true;
                    case enumModoDeAccesoDeDatos.Administrador: return Contexto.DatosDeConexion.EsAdministrador;
                    case enumModoDeAccesoDeDatos.Gestor: return Contexto.DatosDeConexion.EsAdministrador;
                    default:
                        throw new Exception($"Al elemto variable no se le puede acceder con el tipo de permiso: '{permisosNecesarios}'");
                }
            }

            var negocioDtm = LeerRegistroCacheado(nameof(NegocioDtm.Nombre), negocio.ToNombre());
            var cache = ServicioDeCaches.Obtener($"{nameof(GestorDeNegocios)}.{nameof(TienePermisos)}");
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

        internal static TipoDtoElmento ValidarElementoDto(TipoDtoElmento elemento)
        {
            if (elemento.IdElemento <= 0)
                GestorDeErrores.Emitir("El elemento dto a validar no tiene indicado el Id");

            if (elemento.TipoDto.IsNullOrEmpty())
                GestorDeErrores.Emitir("El TipoDto a validar no puede ser nulo");

            if (elemento.Referencia.IsNullOrEmpty())
                GestorDeErrores.Emitir("La referencia del elemnto ser nula");

            try
            {
                elemento.ClaseDto();
            }
            catch(Exception e)
            {
                GestorDeErrores.Emitir($"Error al obtener la clase del TipoDto {elemento.TipoDto}",e);
            }

            return elemento;
        }

        protected override IQueryable<NegocioDtm> AplicarFiltros(IQueryable<NegocioDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId)
                return registros;

            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == nameof(NegocioDtm.ElementoDtm).ToLower())
                {
                    if (filtro.Criterio == CriteriosDeFiltrado.igual)
                        return registros.Where(x => x.ElementoDtm == filtro.Valor);

                    if (filtro.Criterio == CriteriosDeFiltrado.contiene)
                        return registros.Where(x => x.ElementoDtm.Contains(filtro.Valor));

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

        protected override void AntesDePersistir(NegocioDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistir(registro, parametros);

            if (parametros.Operacion == enumTipoOperacion.Insertar)
            {
                registro.IdPermisoDeAdministrador = GestorDePermisos.CrearObtener(Contexto, Mapeador, registro.Nombre, enumClaseDePermiso.Negocio, enumModoDeAccesoDeDatos.Administrador).Id;
                registro.IdPermisoDeGestor = GestorDePermisos.CrearObtener(Contexto, Mapeador, registro.Nombre, enumClaseDePermiso.Negocio, enumModoDeAccesoDeDatos.Gestor).Id;
                registro.IdPermisoDeConsultor = GestorDePermisos.CrearObtener(Contexto, Mapeador, registro.Nombre, enumClaseDePermiso.Negocio, enumModoDeAccesoDeDatos.Consultor).Id;
            }

            if (parametros.Operacion == enumTipoOperacion.Modificar && (!parametros.Parametros.ContainsKey(NegociosDeSe.ActualizarSeguridad) || (bool)parametros.Parametros[NegociosDeSe.ActualizarSeguridad]))
            {
                var registroEnBD = registro;
                registro.IdPermisoDeAdministrador = GestorDePermisos.ModificarPermisoDeDatos(Contexto, Mapeador, registroEnBD.PermisoDeAdministrador, registro.Nombre, enumClaseDePermiso.Negocio, enumModoDeAccesoDeDatos.Administrador).Id;
                registro.IdPermisoDeGestor = GestorDePermisos.ModificarPermisoDeDatos(Contexto, Mapeador, registroEnBD.PermisoDeGestor, registro.Nombre, enumClaseDePermiso.Negocio, enumModoDeAccesoDeDatos.Gestor).Id;
                registro.IdPermisoDeConsultor = GestorDePermisos.ModificarPermisoDeDatos(Contexto, Mapeador, registroEnBD.PermisoDeConsultor, registro.Nombre, enumClaseDePermiso.Negocio, enumModoDeAccesoDeDatos.Consultor).Id;
            }
        }

        protected override void DespuesDePersistir(NegocioDtm registro, ParametrosDeNegocio parametros)
        {
            base.DespuesDePersistir(registro, parametros);

            if (parametros.Operacion == enumTipoOperacion.Modificar || parametros.Operacion == enumTipoOperacion.Eliminar)
            {
                var cache = $"{nameof(GestorDeElementos)}.{nameof(LeerModoDeAccesoAlNegocio)}";
                var patron = $"Negocio:registro.Nombre";
                ServicioDeCaches.EliminarElementos(cache, patron);

                cache = $"{nameof(NegociosDeSe)}.{nameof(NegociosDeSe.LeerNegocioPorEnumerado)}";
                var indice = $"{nameof(enumNegocio)}-{registro.Enumerado}";
                ServicioDeCaches.EliminarElemento(cache, indice);

                cache = $"{nameof(NegociosDeSe)}.{nameof(NegociosDeSe.LeerNegocioPorNombre)}";
                indice = $"{nameof(INombre)}-{registro.Nombre}";
                ServicioDeCaches.EliminarElemento(cache, indice);

                cache = $"{nameof(NegociosDeSe)}.{nameof(NegociosDeSe.LeerNegocioPorDto)}";
                indice = $"{nameof(NegociosDeSe.Dto)}-{registro.ElementoDto}";
                ServicioDeCaches.EliminarElemento(cache, indice);

                cache = $"{nameof(NegociosDeSe)}.{nameof(NegociosDeSe.LeerNegocioPorDtm)}";
                indice = $"{nameof(NegociosDeSe.Dtm)}-{registro.ElementoDtm}";
                ServicioDeCaches.EliminarElemento(cache, indice);
            }
        }

        protected override void AntesDePersistirValidarRegistro(NegocioDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistirValidarRegistro(registro, parametros);
            if (registro.ElementoDtm.IsNullOrEmpty())
                GestorDeErrores.Emitir($"Ha de indicar la clase del objeto {registro.Nombre} es obligatorio");

            var encontrado = false;
            var ensamblado = Assembly.Load(nameof(ServicioDeDatos));
            foreach (var clase in ensamblado.DefinedTypes)
            {
                if (clase.FullName == registro.ElementoDtm)
                {
                    encontrado = true;
                    break;
                }
            }

            if (!encontrado)
                GestorDeErrores.Emitir($"La clase del elemento {registro.ElementoDtm} del negocio {registro.Nombre} debe existir");

        }

    }
}
