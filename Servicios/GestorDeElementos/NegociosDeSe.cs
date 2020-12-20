using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModeloDeDto.Archivos;
using ModeloDeDto.Entorno;
using ModeloDeDto.Negocio;
using ModeloDeDto.Seguridad;

namespace GestorDeElementos
{
    public enum enumNegocio
    {
        No_Definido,
        Usuario,
        VistasMvc,
        Permisos,
        Menu,
        PermisosDeUnUsuario,
        Variable,
        Negocio,
        ClasePermiso,
        Permiso,
        PermisosDeUnPuesto,
        PermisosDeUnRol,
        Puesto,
        PuestosDeUnRol,
        PuestosDeUnUsuario,
        Rol,
        RolesDeUnPermiso,
        RolesDeUnPuesto,
        TipoPermiso,
        UsuariosDeUnPuesto,
        Archivos
    }

    public static class NegociosDeSe
    {
        public static string ToString(enumNegocio negocio)
        {
            switch (negocio)
            {
                case enumNegocio.Usuario: return "Usuarios de SE";
                case enumNegocio.VistasMvc: return "Vistas";
                case enumNegocio.Variable: return "Variables";
                case enumNegocio.Menu: return "Menus";
                case enumNegocio.Puesto: return "Puestos";
            }
            throw new Exception($"El negocio {negocio} no está definido, no se puede parsear");
        }

        public static enumNegocio ParsearDto(string negocio)
        {
            switch (negocio)
            {
                case nameof(UsuarioDto): return enumNegocio.Usuario;
                case nameof(VistaMvcDto): return enumNegocio.VistasMvc;
                case nameof(PermisoDto): return enumNegocio.Permiso;
                case nameof(MenuDto): return enumNegocio.Menu;
                case nameof(PermisosDeUnUsuarioDto): return enumNegocio.PermisosDeUnUsuario;
                case nameof(VariableDto): return enumNegocio.Variable;
                case nameof(NegocioDto): return enumNegocio.Negocio;
                case nameof(ClasePermisoDto): return enumNegocio.ClasePermiso;
                case nameof(PermisosDeUnPuestoDto): return enumNegocio.PermisosDeUnPuesto;
                case nameof(PermisosDeUnRolDto): return enumNegocio.PermisosDeUnRol;
                case nameof(PuestoDto): return enumNegocio.Puesto;
                case nameof(PuestosDeUnRolDto): return enumNegocio.PuestosDeUnRol;
                case nameof(PuestosDeUnUsuarioDto): return enumNegocio.PuestosDeUnUsuario;
                case nameof(RolDto): return enumNegocio.Rol;
                case nameof(RolesDeUnPermisoDto): return enumNegocio.RolesDeUnPermiso;
                case nameof(RolesDeUnPuestoDto): return enumNegocio.RolesDeUnPuesto;
                case nameof(TipoPermisoDto): return enumNegocio.TipoPermiso;
                case nameof(UsuariosDeUnPuestoDto): return enumNegocio.UsuariosDeUnPuesto;
                case nameof(ArchivosDto): return enumNegocio.Archivos;
            }

            throw new Exception($"No está definido el dto {negocio}, no se puede parsear");
        }
    }
}
