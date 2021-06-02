using System.Collections.Concurrent;
using System.Collections.Generic;
using ServicioDeDatos.Elemento;
using Gestor.Errores;
using ServicioDeDatos.Entorno;
using Utilidades;
using Dapper;

namespace ServicioDeDatos
{


    public class Variable
    {
        public static readonly string CFG_Version = nameof(CFG_Version);
        public static readonly string CFG_Debugar_Sqls = nameof(CFG_Debugar_Sqls);
        public static readonly string CFG_Servidor_Archivos = nameof(CFG_Servidor_Archivos);
        public static readonly string CFG_Ruta_De_Binarios = nameof(CFG_Ruta_De_Binarios);
        public static readonly string CFG_Ruta_De_Exportaciones = nameof(CFG_Ruta_De_Exportaciones);
        public static readonly string CFG_Servidor_De_Correo = nameof(CFG_Servidor_De_Correo);
        public static readonly string CFG_UrlBase = nameof(CFG_UrlBase);
        public static readonly string CFG_Crear_Registros_De_Entorno = nameof(CFG_Crear_Registros_De_Entorno);
        public static readonly string CFG_Ruta_De_Descarga = nameof(CFG_Ruta_De_Descarga);


        public static readonly string Cola_Activa = nameof(Cola_Activa);
        public static readonly string Cola_Trazar = nameof(Cola_Trazar);
        public static readonly string Cola_Ejecutor = nameof(Cola_Ejecutor);
        public static readonly string Cola_Emisor = nameof(Cola_Emisor);
        public static readonly string Cola_Receptor = nameof(Cola_Receptor);
    }


    public class CacheDeVariable
    {
        private static ConcurrentDictionary<string, object> cacheVariables = ServicioDeCaches.Obtener(typeof(VariableDtm).FullName);

        public static string Cfg_ServidorDeArchivos => ObtenerVariable(Variable.CFG_Servidor_Archivos, "Define la ruta donde se localiza el servidor de archivos", @"c:\temp\SistemaDocumental");

        public static string Cfg_ServidorDeCorreo => ObtenerVariable(Variable.CFG_Servidor_De_Correo, "Define el nombre de la sección del fichero appsetting.json donde se encuentran las características del servidor de correo a utilizar", "CorreoDeGmail");

        public static bool Cfg_HayQueDebuggar => ObtenerVariable(Variable.CFG_Debugar_Sqls, "Indica si hay que debugar", "S") == "S";

        public static string Cfg_Version => ObtenerVariable(Variable.CFG_Version, "Versión del sistema de elementos", "0.1");

        public static string Cfg_UrlBase => ObtenerVariable(Variable.CFG_UrlBase, "Indica el sitio WEB donde está ubicado", "https://localhost:44396/");

        public static string Cfg_RutaDeDescarga => ObtenerVariable(Variable.CFG_Ruta_De_Descarga, "Indica la dirección absoluta de donde se dejan los ficheros temporales que se descargan", @"C:\temp\descargas");

        public static bool Cfg_CrearRegistrosDeEntorno => ObtenerVariable(Variable.CFG_Crear_Registros_De_Entorno, "Indica si al iniciar el sitio web se han de crear registros en el entorno", "N") == "S";

        public static string Cola_Receptor => ObtenerVariable(Variable.Cola_Receptor, "Indica el mail del receptor de mensajes de soporte de la cola de trabajos sometidos", "juan.jimenez@emuasa.es");

        public static string Cola_Emisor => ObtenerVariable(Variable.Cola_Emisor, "Indica el mail del emisor de mensajes de la cola de trabajos sometidos", "back.ground.cola@gmail.com");

        public static string Cola_LoginDeEjecutor => ObtenerVariable(Variable.Cola_Ejecutor, "Indica el login con el que se ejecuta la cola de trabajos sometidos", "raul.miras");

        public static bool Cola_Trazar => ObtenerVariable(Variable.Cola_Trazar, "Indica si se trazan las consultas SQL del contexto de la cola", "S") == "S";

        public static bool Cola_Activa => ObtenerVariable(Variable.Cola_Activa, "Indica si la cola de trabajos sometidos está activa", "S") == "S";

        private static string ObtenerVariable(string variable, string descripcion, string valor)
        {
            if (!cacheVariables.ContainsKey(variable))
                cacheVariables[variable] = LeerCrear(variable, descripcion, valor);
            return cacheVariables[variable].ToString();
        }

        private static string LeerCrear(string variable, string descripcion, string valorInicial)
        {
            var valor = LeerValorDeVariable(variable, emitirError: false);
            if (valor == Literal.VariableNoDefinida)
            {
                valor = CrearVariable(variable, descripcion, valorInicial);
            }
            return valor;
        }

        private static string LeerValorDeVariable(string variable, bool emitirError = true)
        {
            var consulta = new ConsultaSql<VariableDtm>(VariableSqls.LeerValorDeVariable);
            var valores = new Dictionary<string, object> { { $"@{nameof(variable)}", variable } };
            var resultado = consulta.LanzarConsulta(new DynamicParameters(valores));


            if (resultado.Count == 0)
            {
                if (emitirError)
                    GestorDeErrores.Emitir($"No se localiza la variable {variable}");
                else
                    return Literal.VariableNoDefinida;
            }

            if (resultado.Count > 1)
                GestorDeErrores.Emitir($"Hay más de un registros para la {variable}");

            return resultado[0].Valor;
        }

        private static string CrearVariable(string variable, string descripcion, string valor)
        {
            var sentencia = new ConsultaSql<VariableDtm>(VariableSqls.CrearVariable);
            var valores = new Dictionary<string, object> { { $"@{nameof(variable)}", variable }, { $"@{nameof(descripcion)}", descripcion }, { $"@{nameof(valor)}", valor } };
            if (Cfg_HayQueDebuggar)
                sentencia.DebuggarSentencia($"{variable}.txt", new DynamicParameters(valores));
            else
                sentencia.EjecutarSentencia(new DynamicParameters(valores));
            return valor;
        }


        public static void BorrarCache(string variable)
        {
            ServicioDeCaches.EliminarElemento(typeof(VariableDtm).FullName, variable);
        }

        public static void Modificar(string variable, string valor)
        {
            var sentencia = new ConsultaSql<VariableDtm>(VariableSqls.ModificarVariable);
            var valores = new Dictionary<string, object> { { $"@{nameof(valor)}", valor }, { $"@{nameof(variable)}", variable } };
            if (Cfg_HayQueDebuggar)
                sentencia.DebuggarSentencia($"{variable}.txt", new DynamicParameters(valores));
            else
                sentencia.EjecutarSentencia(new DynamicParameters(valores));

            BorrarCache(variable);
        }
    }
}
