using System.Collections.Concurrent;
using System.Collections.Generic;
using ServicioDeDatos.Elemento;
using Gestor.Errores;
using ServicioDeDatos.Entorno;
using Utilidades;

namespace ServicioDeDatos
{


    public class Variable
    {
        public static readonly string Version = $"CFG_{nameof(Version)}";
        public static readonly string Debugar_Sqls = $"CFG_{nameof(Debugar_Sqls)}";
        public static readonly string Servidor_Archivos = $"CFG_{nameof(Servidor_Archivos)}";
        public static readonly string Binarios = $"CFG_{nameof(Binarios)}";
    }


    public class CacheDeVariable 
    {
        private static ConcurrentDictionary<string, object> cacheVariables = ServicioDeCaches.Obtener(nameof(CacheDeVariable));

        public static string ServidorDeArchivos
        {
            get
            {
                if (!cacheVariables.ContainsKey(Variable.Servidor_Archivos))
                    cacheVariables[Variable.Servidor_Archivos] = LeerValor(Variable.Servidor_Archivos);
                return cacheVariables[Variable.Servidor_Archivos].ToString();
            }
        }

        public static bool HayQueDebuggar
        {
            get
            {
                if (!cacheVariables.ContainsKey(Variable.Debugar_Sqls))
                    cacheVariables[Variable.Debugar_Sqls] = LeerValor(Variable.Debugar_Sqls);
                return cacheVariables[Variable.Debugar_Sqls].ToString() =="S";
            }
        }

        public static string Version
        {
            get
            {
                if (!cacheVariables.ContainsKey(Variable.Version))
                    cacheVariables[Variable.Version] = LeerValor(Variable.Version);
                return cacheVariables[Variable.Version].ToString();
            }
        }

        private static string LeerValor(string variable)
        {
            var sentencia = $"Select Id, Nombre, Descripcion, Valor  from ENTORNO.Variable where NOMBRE like '{variable}'";
            var consulta = new ConsultaSql<VariableDtm>(sentencia);
            var resultado = consulta.LanzarConsulta();

            if (resultado.Count == 0)
                GestorDeErrores.Emitir($"No se localiza la variable {variable}");

            if (resultado.Count > 1)
                GestorDeErrores.Emitir($"Hay más de un registros para la {variable}");

            return resultado[0].Valor;
        }


        public static void BorrarCache(string variable)
        {
            ServicioDeCaches.EliminarElemento(nameof(CacheDeVariable), variable);
        }
    }
}
