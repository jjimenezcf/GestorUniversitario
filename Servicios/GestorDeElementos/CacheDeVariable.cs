﻿using System;
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

        public bool HayQueDebuggar
        {
            get
            {
                if (!cacheVariables.ContainsKey(Variable.Debugar_Sqls))
                    cacheVariables[Variable.Debugar_Sqls] = Consultar(Variable.Debugar_Sqls);
                return cacheVariables[Variable.Debugar_Sqls]=="S";
            }
        }

        public string Version
        {
            get
            {
                if (!cacheVariables.ContainsKey(Variable.Version))
                    cacheVariables[Variable.Version] = Consultar(Variable.Version);
                return cacheVariables[Variable.Version];
            }
        }

        private string Consultar(string variable)
        {
            var sentencia = $"Select * from {Literal.Tabla.Variable} where NOMBRE like '{variable}'";
            var consulta = new ConsultaSql(Contexto, sentencia);
            consulta.Ejecutar();

            if (consulta == null)
                Errores.GestorDeErrores.Emitir($"No se ha ejecutado la consulta {sentencia}");

            if (consulta.Registros.Count == 0)
                Errores.GestorDeErrores.Emitir($"No se han localizado registros para la {sentencia}");

            if (consulta.Registros.Count == 1)
                Errores.GestorDeErrores.Emitir($"Hay más de un registros para la {sentencia}");

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
