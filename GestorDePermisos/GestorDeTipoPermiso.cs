using AutoMapper;
using Gestor.Elementos;
using Gestor.Elementos.Seguridad;

namespace ServicioDeDatos.Seguridad
{
    class GestorDeTipoPermiso : GestorDeElementos<ContextoDeElementos, TipoPermisoDtm, TipoPermisoDto>
    {
        public class MapearTipoPermiso : Profile
        {
            public MapearTipoPermiso()
            {
                CreateMap<TipoPermisoDtm, TipoPermisoDto>();
            }
        }

        public GestorDeTipoPermiso(ContextoDeElementos contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {


        }
    }
}
