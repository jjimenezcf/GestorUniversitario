using AutoMapper;
using GestorDeElementos;
using ModeloDeDto.Seguridad;

namespace ServicioDeDatos.Seguridad
{

    public class GestorDePuestosDeTrabajo : GestorDeElementos<ContextoSe, PuestoDtm, PuestoDto>
    {
        public class MapearPuestoDeTrabajo : Profile
        {
            public MapearPuestoDeTrabajo()
            {
                CreateMap<PuestoDtm, PuestoDto>();
                CreateMap<PuestoDto, PuestoDtm>();
            }
        }

        public GestorDePuestosDeTrabajo(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {
        }


        internal static GestorDePuestosDeTrabajo Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDePuestosDeTrabajo(contexto, mapeador);
        }

    }
}
