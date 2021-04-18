using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModeloDeDto.Archivos;
using ModeloDeDto.Callejero;
using ModeloDeDto.Entorno;
using ModeloDeDto.Negocio;
using ModeloDeDto.Seguridad;
using ServicioDeDatos.Archivos;
using ServicioDeDatos.Callejero;
using ServicioDeDatos.Elemento;
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
        Archivos,
        Pais,
        Provincia
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
           enumNegocio.Rol,
           enumNegocio.Pais,
           enumNegocio.Provincia
        };

        private static List<enumNegocio> _negociosDeParametrizacion = new List<enumNegocio>
        {
           enumNegocio.VistaMvc,
           enumNegocio.Menu,
           enumNegocio.Variable,
           enumNegocio.Negocio,
           enumNegocio.Permiso
        };


        private static List<string> _Registros = new List<string>
        {
           nameof(PermisosDeUnPuestoDtm).Replace("Dtm",""),
           nameof(PermisosDeUnRolDtm).Replace("Dtm","")
        };

        public static bool EsDeParametrizacion(enumNegocio negocio)
        {
            return _negociosDeParametrizacion.Contains(negocio);
        } 

        public static bool UsaSeguridad(enumNegocio negocio)
        {
            return _negociosConSeguridad.Contains(negocio);
        }

        public static bool EsUnRegistro(string negocio)
        {
            return _Registros.Contains(negocio);
        }

        public static bool EsUnRegistro(enumNegocio negocio)
        {
            return _Registros.Contains(negocio.ToString());
        }

        public static string ToString(enumNegocio negocio)
        {
            if (negocio == enumNegocio.No_Definido)
                return enumNegocio.No_Definido.ToString();

            switch (negocio)
            {
                case enumNegocio.Usuario: return "Usuarios";
                case enumNegocio.VistaMvc: return "Vistas";
                case enumNegocio.Variable: return "Variables";
                case enumNegocio.Menu: return "Menus";
                case enumNegocio.Puesto: return "Puestos";
                case enumNegocio.Negocio: return "Negocios";
                case enumNegocio.Permiso: return "Permisos";
                case enumNegocio.Rol: return "Roles";
                case enumNegocio.Pais: return "Paises";
                case enumNegocio.Provincia: return "Provincias";
            }
            throw new Exception($"El negocio {negocio} no está definido, no se puede parsear");
        }

        public static enumNegocio Negocio(string negocio, bool nullValido = false)
        {
            if (negocio == null && nullValido)
                return enumNegocio.No_Definido;

            if (negocio == enumNegocio.No_Definido.ToString())
                return enumNegocio.No_Definido;

            switch (negocio)
            {
                case "Usuarios": return enumNegocio.Usuario;
                case "Vistas": return enumNegocio.VistaMvc;
                case "Permisos": return enumNegocio.Permiso;
                case "Menus": return enumNegocio.Menu;
                case "Variables": return enumNegocio.Variable;
                case "Negocios": return enumNegocio.Negocio;
                case "Puestos": return enumNegocio.Puesto;
                case "Roles": return enumNegocio.Rol;
                case "Paises": return enumNegocio.Pais;
                case "Provincias": return enumNegocio.Provincia;
            }

            if (EsUnRegistro(negocio))
                return enumNegocio.No_Definido;

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
                case nameof(PaisDto): return enumNegocio.Pais;
                case nameof(ProvinciaDto): return enumNegocio.Provincia;
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
                case nameof(PaisDtm): return enumNegocio.Pais;
                case nameof(ProvinciaDtm): return enumNegocio.Provincia;
            }

            return enumNegocio.No_Definido;
        }

        internal static Type TipoDtm(this enumNegocio negocio)
        {
            switch (negocio)
            {
                case enumNegocio.Usuario: return typeof(UsuarioDtm);
                case enumNegocio.VistaMvc: return typeof(VistaMvcDtm);
                case enumNegocio.Variable: return typeof(VariableDtm);
                case enumNegocio.Menu: return typeof(MenuDtm);
                case enumNegocio.Puesto: return typeof(PuestoDtm);
                case enumNegocio.Negocio: return typeof(NegocioDtm);
                case enumNegocio.Permiso: return typeof(PermisoDtm);
                case enumNegocio.Rol: return typeof(RolDtm);
                case enumNegocio.Pais: return typeof(PaisDtm);
                case enumNegocio.Provincia: return typeof(ProvinciaDtm);
            }
            throw new Exception($"El negocio {negocio} no está definido, no se puede obtener su tipo Dtm");
        }

        internal static Type TipoDto(this enumNegocio negocio)
        {
            switch (negocio)
            {
                case enumNegocio.Usuario: return typeof(UsuarioDto);
                case enumNegocio.VistaMvc: return typeof(VistaMvcDto);
                case enumNegocio.Variable: return typeof(VariableDto);
                case enumNegocio.Menu: return typeof(MenuDto);
                case enumNegocio.Puesto: return typeof(PuestoDto);
                case enumNegocio.Negocio: return typeof(NegocioDto);
                case enumNegocio.Permiso: return typeof(PermisoDto);
                case enumNegocio.Rol: return typeof(RolDto);
                case enumNegocio.Pais: return typeof(PaisDto);
                case enumNegocio.Provincia: return typeof(ProvinciaDto);
            }
            throw new Exception($"El negocio {negocio} no está definido, no se puede obtener su tipo Dto");
        }
        internal static IRegistro ObjetoDtm(this enumNegocio negocio)
        {
            switch (negocio)
            {
                case enumNegocio.Usuario: return new UsuarioDtm();
                case enumNegocio.VistaMvc: return new VistaMvcDtm();
                case enumNegocio.Variable: return new VariableDtm();
                case enumNegocio.Menu: return new MenuDtm();
                case enumNegocio.Puesto: return new PuestoDtm();
                case enumNegocio.Negocio: return new NegocioDtm();
                case enumNegocio.Permiso: return new PermisoDtm();
                case enumNegocio.Rol: return new RolDtm();
                case enumNegocio.Pais: return new PaisDtm();
                case enumNegocio.Provincia: return new ProvinciaDtm();
            }
            throw new Exception($"El negocio {negocio} no está definido, no se puede obtener un objeto dtm");
        }
    }
}
