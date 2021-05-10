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


        public static readonly string Cola_Activa = nameof(Cola_Activa);
        public static readonly string Cola_Trazar = nameof(Cola_Trazar);
        public static readonly string Cola_Ejecutor = nameof(Cola_Ejecutor);
        public static readonly string Cola_Emisor = nameof(Cola_Emisor);
        public static readonly string Cola_Receptor = nameof(Cola_Receptor);
    }


    public class CacheDeVariable
    {
        private static ConcurrentDictionary<string, object> cacheVariables = ServicioDeCaches.Obtener(typeof(VariableDtm).FullName);

        public static string ServidorDeArchivos
        {
            get
            {
                if (!cacheVariables.ContainsKey(Variable.CFG_Servidor_Archivos))
                    cacheVariables[Variable.CFG_Servidor_Archivos] = LeerValorDeVariable(Variable.CFG_Servidor_Archivos);
                return cacheVariables[Variable.CFG_Servidor_Archivos].ToString();
            }
        }

        public static string ServidorDeCorreo
        {
            get
            {
                if (!cacheVariables.ContainsKey(Variable.CFG_Servidor_De_Correo))
                    cacheVariables[Variable.CFG_Servidor_De_Correo] = LeerCrear(Variable.CFG_Servidor_De_Correo, "Define el nombre de la sección del fichero appsetting.json donde se encuentran las características del servidor de correo a utilizar", "CorreoDeGmail");
                return cacheVariables[Variable.CFG_Servidor_De_Correo].ToString();
            }
        }

        public static bool HayQueDebuggar
        {
            get
            {
                if (!cacheVariables.ContainsKey(Variable.CFG_Debugar_Sqls))
                    cacheVariables[Variable.CFG_Debugar_Sqls] = LeerValorDeVariable(Variable.CFG_Debugar_Sqls);
                return cacheVariables[Variable.CFG_Debugar_Sqls].ToString() == "S";
            }
        }

        public static string Version
        {
            get
            {
                if (!cacheVariables.ContainsKey(Variable.CFG_Version))
                    cacheVariables[Variable.CFG_Version] = LeerValorDeVariable(Variable.CFG_Version);
                return cacheVariables[Variable.CFG_Version].ToString();
            }
        }

        public static string UrlBase
        {
            get
            {
                if (!cacheVariables.ContainsKey(Variable.CFG_UrlBase))
                    cacheVariables[Variable.CFG_UrlBase] = LeerCrear(Variable.CFG_UrlBase, "Indica el sitio WEB donde está ubicado", "https://localhost:44396/");
                return cacheVariables[Variable.CFG_UrlBase].ToString();
            }
        }

        public static string Cola_Receptor
        {
            get
            {
                if (!cacheVariables.ContainsKey(Variable.Cola_Receptor))
                    cacheVariables[Variable.Cola_Receptor] = LeerCrear(Variable.Cola_Receptor, "Indica el mail del receptor de mensajes de soporte de la cola de trabajos sometidos", "juan.jimenez@emuasa.es");
                return cacheVariables[Variable.Cola_Receptor].ToString();
            }
        }

        public static string Cola_Emisor
        {
            get
            {
                if (!cacheVariables.ContainsKey(Variable.Cola_Emisor))
                    cacheVariables[Variable.Cola_Emisor] = LeerCrear(Variable.Cola_Emisor, "Indica el mail del emisor de mensajes de la cola de trabajos sometidos", "back.ground.cola@gmail.com");
                return cacheVariables[Variable.Cola_Emisor].ToString();
            }
        }

        public static string Cola_LoginDeEjecutor
        {
            get
            {
                if (!cacheVariables.ContainsKey(Variable.Cola_Ejecutor))
                    cacheVariables[Variable.Cola_Ejecutor] = LeerCrear(Variable.Cola_Ejecutor, "Indica el login con el que se ejecuta la cola de trabajos sometidos", "raul.miras");
                return cacheVariables[Variable.Cola_Ejecutor].ToString();
            }
        }


        public static bool Cola_Trazar
        {
            get
            {
                if (!cacheVariables.ContainsKey(Variable.Cola_Trazar))
                    cacheVariables[Variable.Cola_Trazar] = LeerCrear(Variable.Cola_Trazar, "Indica si se trazan las consultas SQL del contexto de la cola", "S");
                return cacheVariables[Variable.Cola_Trazar].ToString() == "S";
            }
        }

        public static bool Cola_Activa
        {
            get
            {
                if (!cacheVariables.ContainsKey(Variable.Cola_Activa))
                    cacheVariables[Variable.Cola_Activa] = LeerCrear(Variable.Cola_Activa, "Indica si la cola de trabajos sometidos está activa", "S");
                return cacheVariables[Variable.Cola_Activa].ToString() == "S";
            }
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
            var consulta = new ConsultaSql<VariableDtm>(VariableSqls.CrearVariable);
            var valores = new Dictionary<string, object> { { $"@{nameof(variable)}", variable }, { $"@{nameof(descripcion)}", descripcion }, { $"@{nameof(valor)}", valor } };
            consulta.EjecutarConsulta(new DynamicParameters(valores));
            return valor;
        }


        public static void BorrarCache(string variable)
        {
            ServicioDeCaches.EliminarElemento(typeof(VariableDtm).FullName, variable);
        }
    }
}
