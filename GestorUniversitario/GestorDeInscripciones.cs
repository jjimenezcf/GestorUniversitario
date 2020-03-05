using AutoMapper;

namespace Gestor.Elementos.Usuario
{
    public class GestorDeInscripciones : GestorDeElementos<ContextoUsuario, RegistroDeInscripcion, ElementoInscripcionesDeUnEstudiante>
    {

        public class MapeoRegistroInscripcion : Profile
        {
            public MapeoRegistroInscripcion()
            {
                CreateMap<RegistroDeInscripcion, ElementoInscripcionesDeUnEstudiante>();
            }
        }

        public GestorDeInscripciones(ContextoUsuario contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
        }

        
        protected override RegistroDeInscripcion LeerConDetalle(int Id)

        {
            return null;
        }
        
    }
}
