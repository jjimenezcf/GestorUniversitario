using Gestor.Elementos.Universitario.ModeloBd;
using Gestor.Elementos.Universitario;
using Gestor.Elementos.Universitario.ModeloIu;
using System.Reflection;
using AutoMapper;

namespace Gestor.Elementos.Universitario
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
