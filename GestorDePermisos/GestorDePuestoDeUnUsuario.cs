using AutoMapper;
using GestorDeSeguridad.ModeloIu;
using ServicioDeDatos;
using ServicioDeDatos.Seguridad;

namespace Gestor.Elementos.Seguridad
{
    public class GestorDePuestoDeUnUsuario : GestorDeElementos<ContextoSe, PuestosDeUsuarioDtm, PuestoDeUnUsuarioDto>
    {

        public class MapearClasePermiso : Profile
        {
            public MapearClasePermiso()
            {
                CreateMap<PuestosDeUsuarioDtm, PuestoDeUnUsuarioDto>();
                CreateMap<PuestoDeUnUsuarioDto, PuestosDeUsuarioDtm>();
            }
        }

        public GestorDePuestoDeUnUsuario(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {


        }


        internal static GestorDePuestoDeUnUsuario Gestor(IMapper mapeador)
        {
            var contexto = ContextoSe.ObtenerContexto();
            return (GestorDePuestoDeUnUsuario)CrearGestor<GestorDePuestoDeUnUsuario>(() => new GestorDePuestoDeUnUsuario(contexto, mapeador));
        }

    }
}

