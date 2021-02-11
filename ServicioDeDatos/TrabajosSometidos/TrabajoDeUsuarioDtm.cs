using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Entorno;



namespace ServicioDeDatos.TrabajosSometidos
{
    public enum enumEstadosDeUnTrabajo {pendiente, iniciado, terminado, conErrores, Error}

    public static class enumEstadosDeUnTrabajoExtension { 
        public static string ToBd(this enumEstadosDeUnTrabajo estado)
        {
            switch(estado)
            {
                case enumEstadosDeUnTrabajo.conErrores: return "TR";
                case enumEstadosDeUnTrabajo.pendiente: return "PT";
                case enumEstadosDeUnTrabajo.iniciado: return "EJ";
                case enumEstadosDeUnTrabajo.terminado: return "OK";
                case enumEstadosDeUnTrabajo.Error: return "ER";
            }

            throw new Exception($"El estado de un trabajo {estado} no está definido en la BD");
        }
    }

    [Table("USUARIO", Schema = "TRABAJO")]
    public class TrabajoDeUsuarioDtm : Registro
    {
        public int IdTrabajo { get; set; }
        public int IdEjecutor { get; set; }
        public int IdSometedor { get; set; }
        public DateTime Entrada { get; set; }
        public DateTime Panificado { get; set; }
        public DateTime? Iniciado { get; set; }
        public DateTime? Terminado { get; set; }
        public enumEstadosDeUnTrabajo Estado { get; set; }
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
            mb.Entity<TrabajoDeUsuarioDtm>().Property(p => p.Entrada).HasColumnName("ENTRADA").IsRequired(true).HasColumnType("DATETIME");
            mb.Entity<TrabajoDeUsuarioDtm>().Property(p => p.Panificado).HasColumnName("PLANIFICADO").IsRequired(true).HasColumnType("DATETIME");
            mb.Entity<TrabajoDeUsuarioDtm>().Property(p => p.Iniciado).HasColumnName("INICIADO").IsRequired(false).HasColumnType("DATETIME");
            mb.Entity<TrabajoDeUsuarioDtm>().Property(p => p.Terminado).HasColumnName("TERMINADO").IsRequired(false).HasColumnType("DATETIME");
            mb.Entity<TrabajoDeUsuarioDtm>().Property(p => p.Estado).HasColumnName("ESTADO").IsRequired(true).HasColumnType("CHAR(2)");
            mb.Entity<TrabajoDeUsuarioDtm>().Property(p => p.Parametros).HasColumnName("PARAMETROS").IsRequired(false).HasColumnType("VARCHAR(2000)");
            mb.Entity<TrabajoDeUsuarioDtm>().Property(p => p.Periodicidad).HasColumnName("PERIODICIDAD").IsRequired(true).HasColumnType("INT");

            mb.Entity<TrabajoDeUsuarioDtm>().HasOne(x => x.Trabajo).WithMany().HasForeignKey(x=>x.IdTrabajo).HasConstraintName("FK_TRABAJO_DE_USUARIO_ID_TRABAJO").OnDelete(DeleteBehavior.Restrict);
            mb.Entity<TrabajoDeUsuarioDtm>().HasOne(x => x.Ejecutor).WithMany().HasForeignKey(x => x.IdEjecutor).HasConstraintName("FK_TRABAJO_DE_USUARIO_ID_EJECUTOR").OnDelete(DeleteBehavior.Restrict);
            mb.Entity<TrabajoDeUsuarioDtm>().HasOne(x => x.Sometedor).WithMany().HasForeignKey(x => x.IdSometedor).HasConstraintName("FK_TRABAJO_DE_USUARIO_ID_SOMETEDOR").OnDelete(DeleteBehavior.Restrict);

     

        }
    }
}
