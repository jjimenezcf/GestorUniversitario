using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gestor.Errores;
using ServicioDeDatos.Elemento;

namespace ServicioDeDatos.Entorno
{
    public class ProcedimientoAlmacenadoDtm : Registro, INombre
    {
      public string Esquema { get; set; }
    }

    //Antigua forma, antes de usar Dapper
    //public class ExistePa : ConsultaSql
    //{
    //    public bool Existe => Leidos == 0 ? false : (int)Registros[0][0] == 1;

    //    public static string sentencia = @"
    //       SELECT cast(t2.Id as int) as Id , t1.specific_schema as Esquema, t1.specific_name as Nombre 
    //       FROM INFORMATION_SCHEMA.ROUTINES t1
    //       inner join sysobjects t2 on t2.name = t1.specific_name
    //       where t1.ROUTINE_TYPE = 'PROCEDURE' 
    //         and t2.type = 'P'
    //         and t1.specific_schema like '[Esquema]'
    //         and t1.specific_name like '[Pa]'";

    //    public ExistePa(ContextoSe contexto, string pa, string esquema)
    //    : base(contexto, sentencia.Replace("[Pa]", pa).Replace("[Esquema]", esquema))
    //    {
    //        Ejecutar();
    //    }
    //}

    public static class GestorDePa
    {
        private static string Sentencia = @"
           SELECT cast(t2.Id as int) as Id , t1.specific_schema as Esquema, t1.specific_name as Nombre 
           FROM INFORMATION_SCHEMA.ROUTINES t1
           inner join sysobjects t2 on t2.name = t1.specific_name
           where t1.ROUTINE_TYPE = 'PROCEDURE' 
             and t2.type = 'P'
             and t1.specific_schema like '[Esquema]'
             and t1.specific_name like '[Pa]'";

        public static List<ProcedimientoAlmacenadoDtm> Leer(string nombrePa, string esquema)
        {
            var consulta = new ConsultaSql<ProcedimientoAlmacenadoDtm>(Sentencia.Replace("[Pa]", nombrePa).Replace("[Esquema]", esquema));
            return consulta.Ejecutar();
        }

        public static void ValidarExistePa(string nombrePa, string esquema)
        {
            var pas = Leer(nombrePa, esquema);
            if (pas.Count == 0)
                GestorDeErrores.Emitir($"El {esquema}.{nombrePa} indicado no existe en la BD");
        }


    }
}
