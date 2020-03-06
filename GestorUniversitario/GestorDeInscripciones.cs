using AutoMapper;

namespace Gestor.Elementos.Permiso
{
    public class GestorDeInscripciones : GestorDeElementos<CtoPermisos, RegistroDeInscripcion, ElementoInscripcionesDeUnEstudiante>
    {

        public class MapeoRegistroInscripcion : Profile
        {
            public MapeoRegistroInscripcion()
            {
                CreateMap<RegistroDeInscripcion, ElementoInscripcionesDeUnEstudiante>();
            }
        }

        public GestorDeInscripciones(CtoPermisos contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
        }

        
        protected override RegistroDeInscripcion LeerConDetalle(int Id)

        {
            return null;
        }
        
    }
}
