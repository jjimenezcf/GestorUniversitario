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
        VistaMvc,
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

        private static List<enumNegocio> _negociosConSeguridad = new List<enumNegocio>
        {
           enumNegocio.Usuario,
           enumNegocio.VistaMvc,
           enumNegocio.Menu,
           enumNegocio.Variable,
           enumNegocio.Negocio,
           enumNegocio.Permiso,
           enumNegocio.Puesto,
           enumNegocio.Rol
        };

        public static bool UsaSeguridad(enumNegocio negocio)
        {
            return _negociosConSeguridad.Contains(negocio);
        }

        public static string ToString(enumNegocio negocio)
        {
            switch (negocio)
            {
                case enumNegocio.Usuario: return "Usuarios de SE";
                case enumNegocio.VistaMvc: return "Vistas";
                case enumNegocio.Variable: return "Variables";
                case enumNegocio.Menu: return "Menus";
                case enumNegocio.Puesto: return "Puestos";
                case enumNegocio.UsuariosDeUnPuesto: return "Usuarios de un puesto";
                case enumNegocio.Negocio: return "Negocios";
                case enumNegocio.Permiso: return "Permisos";
                case enumNegocio.Rol: return "Roles";
                case enumNegocio.PermisosDeUnRol: return "Permisos de un rol";
            }
            throw new Exception($"El negocio {negocio} no está definido, no se puede parsear");
        }

        public static enumNegocio ParsearDto(string registroDto)
        {
            switch (registroDto)
            {
                case nameof(UsuarioDto): return enumNegocio.Usuario;
                case nameof(VistaMvcDto): return enumNegocio.VistaMvc;
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

            throw new Exception($"No está definido el dto '{registroDto}', no se puede parsear");
        }


        public static enumNegocio ParsearNegocio(string negocio)
        {
            switch (negocio)
            {
                case "Usuario": return enumNegocio.Usuario;
                case "VistaMvc": return enumNegocio.VistaMvc;
                case "Permiso": return enumNegocio.Permiso;
                case "Menu": return enumNegocio.Menu;
                case "PermisosDeUnUsuario": return enumNegocio.PermisosDeUnUsuario;
                case "Variable": return enumNegocio.Variable;
                case "Negocio": return enumNegocio.Negocio;
                case "ClasePermiso": return enumNegocio.ClasePermiso;
                case "PermisosDeUnPuesto": return enumNegocio.PermisosDeUnPuesto;
                case "PermisosDeUnRol": return enumNegocio.PermisosDeUnRol;
                case "Puesto": return enumNegocio.Puesto;
                case "PuestosDeUnRol": return enumNegocio.PuestosDeUnRol;
                case "PuestosDeUnUsuario": return enumNegocio.PuestosDeUnUsuario;
                case "Rol": return enumNegocio.Rol;
                case "RolesDeUnPermiso": return enumNegocio.RolesDeUnPermiso;
                case "RolesDeUnPuesto": return enumNegocio.RolesDeUnPuesto;
                case "TipoPermiso": return enumNegocio.TipoPermiso;
                case "UsuariosDeUnPuesto": return enumNegocio.UsuariosDeUnPuesto;
                case "Archivos": return enumNegocio.Archivos;
            }

            throw new Exception($"No está definido el negocio '{negocio}', no se puede parsear");
        }
    }
}
