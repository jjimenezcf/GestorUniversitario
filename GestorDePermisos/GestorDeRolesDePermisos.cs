using AutoMapper;

namespace Gestor.Elementos.Seguridad
{
    public class GestorDeRolesDePermisos : GestorDeElementos<CtoPermisos, RolPermisoReg, RolPermisoDto>
    {

        public class MapearRolPermiso : Profile
        {
            public MapearRolPermiso()
            {
                CreateMap<RolPermisoReg, RolPermisoDto>();
            }
        }

        public GestorDeRolesDePermisos(CtoPermisos contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
        }

        
        protected override RolPermisoReg LeerConDetalle(int Id)

        {
            return null;
        }
        
    }
}
