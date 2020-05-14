using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace GestorDeElementos.Utilidades
{
    public static class Parametros
    {

        public static string ParsearParametros(this DbParameterCollection dbParametros)
        {
            var parametros = "";
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

    //public static class IQueryable
    //{
    //    //public static IQueryable<TRegistro> LogSql<TRegistro>(this IQueryable<TRegistro> consulta, ContextoDeElementos contexto, Dictionary<string, string> filtros) where TRegistro : class
    //    //{
    //    //    var a = consulta.ToSql();
    //    //    contexto.Traza.AnotarTrazaSql(a, "--");
    //    //    return consulta;
    //    //}

    //    //public static string ToSql<TRegistro>(this IQueryable<TRegistro> query) where TRegistro : class
    //    //{
    //    //    var enumerator = query.Provider.Execute<IEnumerable<TRegistro>>(query.Expression).GetEnumerator();
    //    //    var enumeratorType = enumerator.GetType();
    //    //    var selectFieldInfo = enumeratorType.GetField("_selectExpression", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException($"cannot find field _selectExpression on type {enumeratorType.Name}");
    //    //    var sqlGeneratorFieldInfo = enumeratorType.GetField("_querySqlGeneratorFactory", BindingFlags.NonPublic | BindingFlags.Instance) ?? throw new InvalidOperationException($"cannot find field _querySqlGeneratorFactory on type {enumeratorType.Name}");
    //    //    var selectExpression = selectFieldInfo.GetValue(enumerator) as SelectExpression ?? throw new InvalidOperationException($"could not get SelectExpression");
    //    //    var factory = sqlGeneratorFieldInfo.GetValue(enumerator) as IQuerySqlGeneratorFactory ?? throw new InvalidOperationException($"could not get IQuerySqlGeneratorFactory");
    //    //    var sqlGenerator = factory.Create();
    //    //    var command = sqlGenerator.GetCommand(selectExpression);
    //    //    var sql = command.CommandText;
    //    //    return sql;
    //    //}

    //    //public static int Count<TRegistro>(this IQueryable<TRegistro> source, ContextoDeElementos contexto, Dictionary<string, string> filtros) where TRegistro : class
    //    //{
    //    //    source.LogSql(contexto, filtros);
    //    //    return source.Count();
    //    //}

    //    //public static List<TRegistro> ToList<TRegistro>(this IQueryable<TRegistro> source, ContextoDeElementos contexto, Dictionary<string, string> filtros) where TRegistro : class
    //    //{
    //    //    source.LogSql(contexto, filtros);
    //    //    return source.ToList();
    //    //}
    //}
}
