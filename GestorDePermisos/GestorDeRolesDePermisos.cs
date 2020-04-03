using AutoMapper;

namespace Gestor.Elementos.Seguridad
{
    public class GestorDeRolesDePermisos : GestorDeElementos<CtoSeguridad, rRolPermiso, RolPermisoDto>
    {

        public class MapearRolPermiso : Profile
        {
            public MapearRolPermiso()
            {
                CreateMap<rRolPermiso, RolPermisoDto>();
            }
        }

        public GestorDeRolesDePermisos(CtoSeguridad contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
        }

        
        protected override rRolPermiso LeerConDetalle(int Id)

        {
            return null;
        }
        
    }
}
