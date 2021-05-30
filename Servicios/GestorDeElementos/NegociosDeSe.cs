using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using Gestor.Errores;
using ModeloDeDto;
using ModeloDeDto.Archivos;
using ModeloDeDto.Callejero;
using ModeloDeDto.Entorno;
using ModeloDeDto.Negocio;
using ModeloDeDto.Seguridad;
using ModeloDeDto.TrabajosSometidos;
using Newtonsoft.Json;
using ServicioDeDatos;
using ServicioDeDatos.Archivos;
using ServicioDeDatos.Callejero;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.Negocio;
using ServicioDeDatos.Seguridad;
using ServicioDeDatos.TrabajosSometidos;
using Utilidades;

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
        Provincia,
        Municipio,
        Correo
    }

    public class NegocioAttribute : Attribute
    {
        public enumNegocio Negocio { get; set; } = enumNegocio.No_Definido;
    }


    public static class NegociosDeSe
    {
        public  static readonly string ValidarSeguridad = nameof(ValidarSeguridad);

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
           enumNegocio.Provincia,
           enumNegocio.Municipio
        };

        private static List<enumNegocio> _negociosDeParametrizacion = new List<enumNegocio>
        {
           enumNegocio.VistaMvc,
           enumNegocio.Menu,
           enumNegocio.Variable,
           enumNegocio.Negocio,
           enumNegocio.Permiso,
           enumNegocio.Usuario,
           enumNegocio.Correo
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
            return !EsUnNegocio(negocio);
        }

        public static bool EsUnNegocio(string negocio)
        {
            return Negocio(negocio) != enumNegocio.No_Definido;
        }

        public static bool EsUnNegocio(this enumNegocio negocio)
        {
            return negocio != enumNegocio.No_Definido;
        }


        public static string ToJson(this List<TipoDtoElmento> e)
        {
            if (e == null)
                e = new List<TipoDtoElmento>();

            return JsonConvert.SerializeObject(e);
        }

        public static string Nombre(this enumNegocio negocio)
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
                case enumNegocio.Municipio: return "Municipios";
                case enumNegocio.Correo: return "Correos";
            }
            throw new Exception($"El negocio {negocio} no está definido, no se puede obtener su nombre");
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
                case "Correos": return enumNegocio.Correo;
            }

            return enumNegocio.No_Definido;
        }

        public static enumNegocio NegocioDeUnDto(string elementoDto)
        {
            switch (elementoDto)
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
                case nameof(MunicipioDto): return enumNegocio.Municipio;
                case nameof(CorreoDto): return enumNegocio.Correo;
            }

            return enumNegocio.No_Definido;
        }

        public static enumNegocio NegocioDeUnDtm(string registroDtm)
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
                case nameof(MunicipioDtm): return enumNegocio.Municipio;
                case nameof(CorreoDtm): return enumNegocio.Correo;
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
                case enumNegocio.Municipio: return typeof(MunicipioDtm);
                case enumNegocio.Correo: return typeof(CorreoDtm);
            }
            throw new Exception($"El negocio {negocio} no está definido, no se puede obtener su tipo Dtm");
        }

        public static string UrlParaMostrarUnNegocio(enumNegocio negocio)
        {
            var cache = ServicioDeCaches.Obtener(typeof(NegocioDtm).FullName);
            var nombreNegocio = negocio.Nombre();
            var indice = $"{nameof(INombre.Nombre)}-{nombreNegocio}";
            if (!cache.ContainsKey(indice))
            {
                var consulta = new ConsultaSql<NegocioDtm>(NegocioSqls.LeerNegocioPorNombre);
                var valores = new Dictionary<string, object> { { $"@{nameof(INombre.Nombre)}", nombreNegocio } };
                var negocios = consulta.LanzarConsulta(new DynamicParameters(valores));
                if (negocios.Count != 1) GestorDeErrores.Emitir($"No se ha localizado de forma unívoca el negocio {nombreNegocio}");
                cache[indice] = negocios[0];
            }
            var elementoDto = ((NegocioDtm)cache[indice]).ElementoDto;
            if (elementoDto.IsNullOrEmpty()) GestorDeErrores.Emitir($"No se ha definido el elementoDto para el negocio {nombreNegocio}");

            var tipoDto = ExtensionesDto.ObtenerTypoDto(elementoDto);
            return ExtensionesDto.UrlParaMostrarUnDto(tipoDto);
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
                case enumNegocio.Municipio: return typeof(MunicipioDto);
                case enumNegocio.Correo: return typeof(CorreoDto);
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
                case enumNegocio.Municipio: return new MunicipioDtm();
                case enumNegocio.Correo: return new CorreoDtm();
            }
            throw new Exception($"El negocio {negocio} no está definido, no se puede obtener un objeto dtm");
        }

        public static IGestor CrearGestor(ContextoSe contexto, string dtm, string dto)
        {
            var cache = ServicioDeCaches.Obtener(nameof(CrearGestor));
            var clave = $"{dtm}-{dto}";
            if (!cache.ContainsKey(clave))
            {
                var assembly = Assembly.LoadFile($@"{Ensamblados.RutaDeBinarios()}\GestoresDeNegocio.dll");
                var clases = assembly.GetExportedTypes();
                for (int i = 0; i < clases.Length; i++)
                {
                    var clase = clases[i];
                    if (clase.BaseType.Name.Contains(nameof(GestorDeElementos)) && clase.BaseType.GenericTypeArguments[1].Name == dtm && clase.BaseType.GenericTypeArguments[2].Name == dto)
                    {
                        var parametros = new Type[] { typeof(ContextoSe), typeof(IMapper) };

                        cache[clave] = clase.GetConstructor(new Type[] { typeof(ContextoSe), typeof(IMapper) });
                        if (cache[clave] == null)
                            throw new Exception($"No se ha definido el constructor Gestor para la clase {clase.Name} con los parámetros de contexto y mapeador.");

                        break;
                    }
                }
            }

            if (!cache.ContainsKey(clave))
                throw new Exception($"No se ha localizado un gestor de elementods para los tipos {dtm}, {dto}.");

            return (IGestor)((ConstructorInfo)cache[clave]).Invoke(new object[] { contexto, contexto.Mapeador });
        }

        public static object ValorDelAtributo(Type clase, string nombreAtributo, bool obligatorio = true)
        {
            Attribute[] atributosDelGestor = Attribute.GetCustomAttributes(clase);

            if (atributosDelGestor == null || atributosDelGestor.Length == 0)
                GestorDeErrores.Emitir($"No hay definido atributos {nameof(NegocioAttribute)} para el gestor {clase.Name}");

            foreach (Attribute propiedad in atributosDelGestor)
            {
                if (propiedad is NegocioAttribute)
                {
                    NegocioAttribute a = (NegocioAttribute)propiedad;
                    switch (nombreAtributo)
                    {
                        case nameof(NegocioAttribute.Negocio):
                            return a.Negocio;
                    }
                    if (obligatorio)
                        throw new Exception($"Se ha solicitado el atributo {nameof(NegocioAttribute)}.{nombreAtributo} de la clase {clase.Name} y no está definido");
                }
            }

            return null;
        }

    }
}
