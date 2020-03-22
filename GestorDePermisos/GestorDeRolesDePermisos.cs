using AutoMapper;

namespace Gestor.Elementos.Seguridad
{
    public class GestorDeRolesDePermisos : GestorDeElementos<CtoPermisos, rRolPermiso, RolPermisoDto>
    {

        public class MapearRolPermiso : Profile
        {
            public MapearRolPermiso()
            {
                CreateMap<rRolPermiso, RolPermisoDto>();
            }
        }

        public GestorDeRolesDePermisos(CtoPermisos contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
        }

        
        protected override rRolPermiso LeerConDetalle(int Id)

        {
            return null;
        }
        
    }
}
