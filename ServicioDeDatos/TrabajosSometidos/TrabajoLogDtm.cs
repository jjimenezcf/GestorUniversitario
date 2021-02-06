using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;

namespace ServicioDeDatos.TrabajosSometidos
{
   public class TrabajoLogDtm : Registro
    {
        public int idTrabajo { get; set; }
        public string Log { get; set; }
        public virtual TrabajoSometidoDtm Trabajo { get; set; }
    }

    public static class TablaTrabajoLog
    {
        public static void Definir(ModelBuilder mb)
        {
            mb.Entity<TrabajoLogDtm>().ToTable("LOG", "TRABAJO");
            mb.Entity<TrabajoLogDtm>().Property(p => p.idTrabajo).HasColumnName("ID_TRABAJO").IsRequired(true).HasColumnType("INT");
            mb.Entity<TrabajoLogDtm>().Property(p => p.Log).HasColumnName("LOG").IsRequired(true).HasColumnType("VARCHAR(MAX)");
            mb.Entity<TrabajoLogDtm>().HasOne(x => x.Trabajo).WithMany().HasForeignKey(x => x.idTrabajo).HasConstraintName("FK_LOG_DE_TRABAJO_ID_TRABAJO").OnDelete(DeleteBehavior.Restrict);
        }
    }
}