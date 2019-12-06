using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Gestor.Elementos
{
    public static class GestorDeConsultas
    {
        public static void Seleccionar(ContextoDeElementos contexto, ConsultaSql consultaSql, params object[] parameters)
        {            
            var sentenciaSql = contexto.Database.GetDbConnection().CreateCommand();
            sentenciaSql.CommandText = consultaSql.Select;
            contexto.Database.OpenConnection();
            var resultadoSql = sentenciaSql.ExecuteReader();
            
            var registrosLeidos = 0;
            while (resultadoSql.HasRows)
            {
                for(int i = 0; i < resultadoSql.FieldCount; i++)
                   consultaSql.Columnas.Add(resultadoSql.GetName(i));
                
                resultadoSql.Read();
                var registro = new List<object>();
                for (int i = 0; i < resultadoSql.FieldCount; i++)
                {
                    registro.Add(resultadoSql.GetValue(i));
                }
                consultaSql.Registros.Add(registrosLeidos,registro);
                
                registrosLeidos++;
                resultadoSql.NextResult();
            }
            consultaSql.Leidos = registrosLeidos;
        }

        //public static async Task<RelationalDataReader> ExecuteSqlCommandAsync(this DatabaseFacade databaseFacade,
        //                                                     string sql,
        //                                                     CancellationToken cancellationToken = default(CancellationToken),
        //                                                     params object[] parameters)
        //{

        //    var concurrencyDetector = databaseFacade.GetService<IConcurrencyDetector>();

        //    using (concurrencyDetector.EnterCriticalSection())
        //    {
        //        var rawSqlCommand = databaseFacade
        //            .GetService<IRawSqlCommandBuilder>()
        //            .Build(sql, parameters);

        //        return await rawSqlCommand
        //            .RelationalCommand
        //            .ExecuteReaderAsync(
        //                databaseFacade.GetService<IRelationalConnection>(),
        //                parameterValues: rawSqlCommand.ParameterValues,
        //                cancellationToken: cancellationToken);
        //    }
        //}
    }
}
