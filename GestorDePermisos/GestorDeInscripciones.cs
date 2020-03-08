using AutoMapper;

namespace Gestor.Elementos.Seguridad
{
    public class GestorDeInscripciones : GestorDeElementos<CtoPermisos, RolPermisoReg, ElementoInscripcionesDeUnEstudiante>
    {

        public class MapeoRegistroInscripcion : Profile
        {
            public MapeoRegistroInscripcion()
            {
                CreateMap<RolPermisoReg, ElementoInscripcionesDeUnEstudiante>();
            }
        }

        public GestorDeInscripciones(CtoPermisos contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
        }

        
        protected override RolPermisoReg LeerConDetalle(int Id)

        {
            return null;
        }
        
    }
}
