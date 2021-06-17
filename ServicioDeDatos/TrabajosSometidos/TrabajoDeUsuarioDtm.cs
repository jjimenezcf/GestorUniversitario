using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Entorno;



namespace ServicioDeDatos.TrabajosSometidos
{
    public enum enumEstadosDeUnTrabajo {Pendiente, iniciado, Terminado, conErrores, Error, Bloqueado}

    public static class TrabajoSometido { 
        public static string ToDtm(this enumEstadosDeUnTrabajo estado)
        {
            switch(estado)
            {
                case enumEstadosDeUnTrabajo.conErrores: return "TR";
                case enumEstadosDeUnTrabajo.Pendiente: return "PT";
                case enumEstadosDeUnTrabajo.Bloqueado: return "BL";
                case enumEstadosDeUnTrabajo.iniciado: return "EJ";
                case enumEstadosDeUnTrabajo.Terminado: return "OK";
                case enumEstadosDeUnTrabajo.Error: return "ER";
            }

            throw new Exception($"El estado de un trabajo {estado} no está definido en la BD");
        }
        public static string ToDto(this enumEstadosDeUnTrabajo estado)
        {
            switch (estado)
            {
                case enumEstadosDeUnTrabajo.conErrores: return "Con errores" ;
                case enumEstadosDeUnTrabajo.Pendiente:  return "Pendiente";
                case enumEstadosDeUnTrabajo.Bloqueado:  return "Bloqueado";
                case enumEstadosDeUnTrabajo.iniciado:   return "Iniciado";
                case enumEstadosDeUnTrabajo.Terminado:  return "Terminado";
                case enumEstadosDeUnTrabajo.Error:      return "Erroneo";
            }

            throw new Exception($"El estado de un trabajo {estado} no está definido en la BD");
        }
        public static string ToDto(string estadoDtm)
        {
            switch (estadoDtm)
            {
                case "TR": return "Con errores" ;
                case "PT": return "Pendiente";
                case "BL": return "Bloqueado";
                case "EJ": return "Iniciado";
                case "OK": return "Terminado";
                case "ER": return "Erroneo";
            }

            throw new Exception($"El estado de un trabajo {estadoDtm} no está definido en la BD");
        }
        public static string ToDtm(string estadoDto)
        {
            switch (estadoDto)
            {
                case "Con errores" : return "TR";
                case "Pendiente"   : return "PT";
                case "Bloqueado"   : return "BL";
                case "Iniciado"    : return "EJ";
                case "Terminado"   : return "OK";
                case "Erroneo"     : return "ER";
            }

            throw new Exception($"El estado de un trabajo '{estadoDto}' no es válido");
        }
    }


    [Table("USUARIO", Schema = "TRABAJO")]
    public class TrabajoDeUsuarioDtm : Registro
    {
        public int IdTrabajo { get; set; }
        public int IdEjecutor { get; set; }
        public int IdSometedor { get; set; }
        public DateTime Encolado { get; set; }
        public DateTime Planificado { get; set; }
        public DateTime? Iniciado { get; set; }
        public DateTime? Terminado { get; set; }

        public string Estado { get; set; }
        public string Parametros { get; set; }
        public int Periodicidad { get; set; }
        public virtual TrabajoSometidoDtm Trabajo { get; set; }
        public virtual UsuarioDtm Sometedor { get; set; }
        public virtual UsuarioDtm Ejecutor { get; set; }
    }

    public static class TablaTrabajoDeUsuario
    {
        public static void Definir(ModelBuilder mb)
        {
            mb.Entity<TrabajoDeUsuarioDtm>().Property(p => p.IdTrabajo).HasColumnName("ID_TRABAJO").IsRequired(true).HasColumnType("INT");
            mb.Entity<TrabajoDeUsuarioDtm>().Property(p => p.IdEjecutor).HasColumnName("ID_EJECUTOR").IsRequired(true).HasColumnType("INT");
            mb.Entity<TrabajoDeUsuarioDtm>().Property(p => p.IdSometedor).HasColumnName("ID_SOMETEDOR").IsRequired(true).HasColumnType("INT");
            mb.Entity<TrabajoDeUsuarioDtm>().Property(p => p.Encolado).HasColumnName("ENTRADA").IsRequired(true).HasColumnType("DATETIME2(7)");
            mb.Entity<TrabajoDeUsuarioDtm>().Property(p => p.Planificado).HasColumnName("PLANIFICADO").IsRequired(true).HasColumnType("DATETIME2(7)");
            mb.Entity<TrabajoDeUsuarioDtm>().Property(p => p.Iniciado).HasColumnName("INICIADO").IsRequired(false).HasColumnType("DATETIME2(7)");
            mb.Entity<TrabajoDeUsuarioDtm>().Property(p => p.Terminado).HasColumnName("TERMINADO").IsRequired(false).HasColumnType("DATETIME2(7)");
            mb.Entity<TrabajoDeUsuarioDtm>().Property(p => p.Estado).HasColumnName("ESTADO").IsRequired(true).HasColumnType("CHAR(2)");
            mb.Entity<TrabajoDeUsuarioDtm>().Property(p => p.Parametros).HasColumnName("PARAMETROS").IsRequired(false).HasColumnType("VARCHAR(2000)");
            mb.Entity<TrabajoDeUsuarioDtm>().Property(p => p.Periodicidad).HasColumnName("PERIODICIDAD").IsRequired(true).HasColumnType("INT");

            mb.Entity<TrabajoDeUsuarioDtm>().HasOne(x => x.Trabajo).WithMany().HasForeignKey(x=>x.IdTrabajo).HasConstraintName("FK_TRABAJO_DE_USUARIO_ID_TRABAJO").OnDelete(DeleteBehavior.Restrict);
            mb.Entity<TrabajoDeUsuarioDtm>().HasOne(x => x.Ejecutor).WithMany().HasForeignKey(x => x.IdEjecutor).HasConstraintName("FK_TRABAJO_DE_USUARIO_ID_EJECUTOR").OnDelete(DeleteBehavior.Restrict);
            mb.Entity<TrabajoDeUsuarioDtm>().HasOne(x => x.Sometedor).WithMany().HasForeignKey(x => x.IdSometedor).HasConstraintName("FK_TRABAJO_DE_USUARIO_ID_SOMETEDOR").OnDelete(DeleteBehavior.Restrict);

     

        }
    }
    public class TrabajoDeUsuarioDapper: IRegistro
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

    }

    public static class TrabajosDeUsuarioSql
    {
        public static string LeerTrabajoPendiente = @"
SELECT top(1)  T1.ID as Id
, T2.NOMBRE as Nombre
FROM TRABAJO.USUARIO T1
INNER JOIN TRABAJO.TRABAJO T2 ON T2.ID = T1.ID_TRABAJO
WHERE ESTADO = 'PT'
 AND (INICIADO = '0001-01-01T00:00:00.0000000' OR INICIADO IS NULL)
 and t1.ID not in (select ID_TRABAJO from TRABAJO.SEMAFORO with(nolock))
 and t1.PLANIFICADO < GETDATE()
 ORDER BY PLANIFICADO
";
    }
}
