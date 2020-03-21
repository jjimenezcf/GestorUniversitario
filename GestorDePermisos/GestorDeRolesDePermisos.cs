using AutoMapper;

namespace Gestor.Elementos.Seguridad
{
    public class GestorDeRolesDePermisos : GestorDeElementos<CtoPermisos, RegRolPermisos, RolPermisoDto>
    {

        public class MapearRolPermiso : Profile
        {
            public MapearRolPermiso()
            {
                CreateMap<RegRolPermisos, RolPermisoDto>();
            }
        }

        public GestorDeRolesDePermisos(CtoPermisos contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
        }

        
        protected override RegRolPermisos LeerConDetalle(int Id)

        {
            return null;
        }
        
    }
}
