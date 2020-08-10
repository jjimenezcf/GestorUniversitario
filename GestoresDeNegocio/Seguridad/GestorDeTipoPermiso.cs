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

        internal TipoPermisoDtm Crear(enumTipoDePermiso acceso)
        {
            var registro = new TipoPermisoDtm();
            registro.Nombre = acceso.ToString();
            PersistirRegistro(registro, new ParametrosDeNegocio(TipoOperacion.Insertar));
            return registro;
        }
    }
}
