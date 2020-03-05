using AutoMapper;

namespace Gestor.Elementos.Usuario
{
    public class GestorDeInscripciones : GestorDeElementos<ContextoUniversitario, RegistroDeInscripcion, ElementoInscripcionesDeUnEstudiante>
    {

        public class MapeoRegistroInscripcion : Profile
        {
            public MapeoRegistroInscripcion()
            {
                CreateMap<RegistroDeInscripcion, ElementoInscripcionesDeUnEstudiante>();
            }
        }

        public GestorDeInscripciones(ContextoUniversitario contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
        }

        
        protected override RegistroDeInscripcion LeerConDetalle(int Id)

        {
            return null;
        }
        
    }
}
