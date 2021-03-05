using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace ServicioDeDatos.Utilidades
{
    public static class Parametros
    {

        public static string ParsearParametros(this DbParameterCollection dbParametros)
        {
            var parametros = "";
            if (dbParametros == null)
                return "";

            foreach (DbParameter dbParametro in dbParametros)
            {
                parametros = $"{parametros}{dbParametro.ParameterName}={dbParametro.ParsearValorParametro()}{Environment.NewLine}";
            }

            return parametros;
        }

        private static string ParsearValorParametro(this DbParameter dbParametro)
        {

            if (dbParametro.Value == DBNull.Value)
                return "is null";

            if (dbParametro.EsDelTipo(new List<DbType>() { DbType.String, DbType.Guid }))
                return $"'{dbParametro.Value}'";

            return $"{dbParametro.Value}";

        }
               
        private static bool EsDelTipo(this DbParameter parametro, List<DbType> listaDeTipos)
        {
            if (parametro == null)
                return true;

            return listaDeTipos.Contains(parametro.DbType);
        }

    }


}
