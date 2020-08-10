using AutoMapper;
using Gestor.Elementos;
using ModeloDeDto.Seguridad;

namespace ServicioDeDatos.Seguridad
{

    public class GestorDeRoles : GestorDeElementos<ContextoSe, RolDtm, RolDto>
    {
        public class MapearPuestoDeTrabajo : Profile
        {
            public MapearPuestoDeTrabajo()
            {
                CreateMap<RolDtm, RolDto>();
                CreateMap<RolDto, RolDtm>();
            }
        }

        public GestorDeRoles(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {
        }


        internal static GestorDeRoles Gestor(IMapper mapeador)
        {
            var contexto = ContextoSe.ObtenerContexto();
            return (GestorDeRoles)CrearGestor<GestorDeRoles>(() => new GestorDeRoles(contexto, mapeador));
        }

    }
}
