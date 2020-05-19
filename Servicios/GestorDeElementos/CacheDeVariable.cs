using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using Gestor.Elementos.ModeloBd;

namespace Gestor.Elementos
{


    public class Variable
    {
        public static readonly string Version = $"CFG_{nameof(Version)}";
        public static readonly string Debugar_Sqls = $"CFG_{nameof(Debugar_Sqls)}";
        public static readonly string Servidor_Archivos = $"CFG_{nameof(Servidor_Archivos)}";
    }


    public class CacheDeVariable 
    {
        public ContextoDeElementos Contexto { get; private set; }

        private static ConcurrentDictionary<string, string> cacheVariables;

        public string ServidorDeArchivos
        {
            get
            {
                if (!cacheVariables.ContainsKey(Variable.Servidor_Archivos))
                    cacheVariables[Variable.Servidor_Archivos] = Consultar(Variable.Servidor_Archivos);
                return cacheVariables[Variable.Servidor_Archivos];
            }
        } 

        private string Consultar(string variable)
        {
            var consulta = new ConsultaSql(Contexto, $"Select * from {Literal.Tabla.Variable} where NOMBRE like '{variable}'");
            consulta.Ejecutar();
            return (string)consulta.Registros[0][3];
        }

        public CacheDeVariable(ContextoDeElementos contexto)
        {
            Contexto = contexto;
            if (cacheVariables == null)
                cacheVariables = new ConcurrentDictionary<string, string>();
        }

        public string BorrarCache(string variable)
        {
            var valor = "";
            if (cacheVariables.ContainsKey(variable))
                cacheVariables.Remove(variable, out valor);
            return valor;
        }
    }
}
