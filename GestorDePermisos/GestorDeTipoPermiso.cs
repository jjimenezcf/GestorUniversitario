using AutoMapper;
using Gestor.Elementos;
using ModeloDeDto.Seguridad;

namespace ServicioDeDatos.Seguridad
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


        internal static GestorDeTipoPermiso Gestor(IMapper mapeador)
        {
            var contexto = ContextoSe.ObtenerContexto();
            return (GestorDeTipoPermiso)CrearGestor<GestorDeTipoPermiso>(() => new GestorDeTipoPermiso(contexto, mapeador));
        }
    }
}
