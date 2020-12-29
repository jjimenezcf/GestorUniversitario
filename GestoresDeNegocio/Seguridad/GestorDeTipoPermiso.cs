using System;
using AutoMapper;
using GestorDeElementos;
using ModeloDeDto.Seguridad;
using ServicioDeDatos;
using ServicioDeDatos.Seguridad;

namespace GestoresDeNegocio.Seguridad
{
    class GestorDeTipoPermiso : GestorDeElementos<ContextoSe, TipoPermisoDtm, TipoPermisoDto>
    {
        public class MapearTipoPermiso : Profile
        {
            public MapearTipoPermiso()
            {
                CreateMap<TipoPermisoDtm, TipoPermisoDto>();
            }
        }

        public GestorDeTipoPermiso(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {
        }


        internal static GestorDeTipoPermiso Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeTipoPermiso(contexto, mapeador);
        }

        internal TipoPermisoDtm CrearTipoPermisoDeDatos(enumModoDeAccesoDeDatos acceso)
        {
            var registro = new TipoPermisoDtm();
            registro.Nombre = ModoDeAcceso.ToString(acceso);
            PersistirRegistro(registro, new ParametrosDeNegocio(TipoOperacion.Insertar));
            return registro;
        }

        internal TipoPermisoDtm CrearTipoPermisoFuncional(enumModoDeAccesoFuncional acceso)
        {
            var registro = new TipoPermisoDtm();
            registro.Nombre = ModoDeAcceso.ToString(acceso);
            PersistirRegistro(registro, new ParametrosDeNegocio(TipoOperacion.Insertar));
            return registro;
        }
    }
}
