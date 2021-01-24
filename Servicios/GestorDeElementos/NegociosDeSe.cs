using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModeloDeDto.Archivos;
using ModeloDeDto.Entorno;
using ModeloDeDto.Negocio;
using ModeloDeDto.Seguridad;
using ServicioDeDatos.Archivos;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.Negocio;
using ServicioDeDatos.Seguridad;

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

        private static List<enumNegocio> _negociosDeParametrizacion = new List<enumNegocio>
        {
           enumNegocio.VistaMvc,
           enumNegocio.Menu,
           enumNegocio.Variable,
           enumNegocio.Negocio,
           enumNegocio.Permiso
        };

        public static bool UsaSeguridad(enumNegocio negocio)
        {
            return _negociosConSeguridad.Contains(negocio);
        }

        public static bool EsDeParametrizacion(enumNegocio negocio)
        {
            return _negociosDeParametrizacion.Contains(negocio);
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
                case enumNegocio.Negocio: return "Negocios";
                case enumNegocio.Permiso: return "Permisos";
                case enumNegocio.Rol: return "Roles";
            }
            throw new Exception($"El negocio {negocio} no está definido, no se puede parsear");
        }

        public static enumNegocio ParsearDto(string registroDto)
        {
            switch (registroDto)
            {
                case nameof(UsuarioDto): return enumNegocio.Usuario;
                case nameof(MenuDto): return enumNegocio.Menu;
                case nameof(VistaMvcDto): return enumNegocio.VistaMvc;
                case nameof(VariableDto): return enumNegocio.Variable;
                case nameof(NegocioDto): return enumNegocio.Negocio;
                case nameof(PermisoDto): return enumNegocio.Permiso;
                case nameof(RolDto): return enumNegocio.Rol;
                case nameof(PuestoDto): return enumNegocio.Puesto;
            }

            return enumNegocio.No_Definido;
        }

        public static enumNegocio ParsearDtm(string registroDtm)
        {
            switch (registroDtm)
            {
                case nameof(UsuarioDtm): return enumNegocio.Usuario;
                case nameof(VistaMvcDtm): return enumNegocio.VistaMvc;
                case nameof(MenuDtm): return enumNegocio.Menu;
                case nameof(VariableDtm): return enumNegocio.Variable;
                case nameof(NegocioDtm): return enumNegocio.Negocio;
                case nameof(PermisoDtm): return enumNegocio.Permiso;
                case nameof(RolDtm): return enumNegocio.Rol;
                case nameof(PuestoDtm): return enumNegocio.Puesto;
            }

            return enumNegocio.No_Definido;
        }


        public static enumNegocio ParsearNegocio(string negocio)
        {
            switch (negocio)
            {
                case "Usuario": return enumNegocio.Usuario;
                case "VistaMvc": return enumNegocio.VistaMvc;
                case "Permiso": return enumNegocio.Permiso;
                case "Menu": return enumNegocio.Menu;
                case "Variable": return enumNegocio.Variable;
                case "Negocio": return enumNegocio.Negocio;
                case "Puesto": return enumNegocio.Puesto;
                case "Rol": return enumNegocio.Rol;
            }
            return enumNegocio.No_Definido;
        }
    }
}
