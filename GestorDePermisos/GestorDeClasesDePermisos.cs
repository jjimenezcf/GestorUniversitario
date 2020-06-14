using AutoMapper;
using ServicioDeDatos;
using ServicioDeDatos.Seguridad;

namespace Gestor.Elementos.Seguridad
{


    public class GestorDeClaseDePermisos : GestorDeElementos<ContextoSe, ClasePermisoDtm, ClasePermisoDto>
    {

        public class MapearClasePermiso : Profile
        {
            public MapearClasePermiso()
            {
                CreateMap<ClasePermisoDtm, ClasePermisoDto>();
                CreateMap<ClasePermisoDto, ClasePermisoDtm>();
            }
        }

        public GestorDeClaseDePermisos(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {


        }


        internal static GestorDeClaseDePermisos Gestor(IMapper mapeador)
        {
            var contexto = ContextoSe.ObtenerContexto();
            return (GestorDeClaseDePermisos)CrearGestor<GestorDeClaseDePermisos>(() => new GestorDeClaseDePermisos(contexto, mapeador));
        }

    }
}
