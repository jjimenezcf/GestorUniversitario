using Gestor.Elementos.Usuario.ModeloBd;
using Gestor.Elementos.Usuario;
using Gestor.Elementos.Usuario.ModeloIu;
using System.Reflection;
using AutoMapper;

namespace Gestor.Elementos.Usuario
{
    public class GestorDeInscripciones : GestorDeElementos<ContextoUniversitario, RegistroDeInscripcion, ElementoInscripcionesDeUnEstudiante>
    {

        public class MapeoRegistroInscripcion : Profile
        {
            public MapeoRegistroInscripcion()
            {
                CreateMap<RegistroDeInscripcion, ModeloIu.ElementoInscripcionesDeUnEstudiante>();
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
