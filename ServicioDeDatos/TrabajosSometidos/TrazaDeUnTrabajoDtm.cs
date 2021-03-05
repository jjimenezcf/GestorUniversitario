using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;

namespace ServicioDeDatos.TrabajosSometidos
{
   public class TrazaDeUnTrabajoDtm : Registro
    {
        public int IdTrabajoDeUsuario { get; set; }
        public string Log { get; set; }
        public virtual TrabajoDeUsuarioDtm TrabajoDeUsuario { get; set; }
    }

    public static class TablaDeLatrazaDeUnTrabajo
    {
        public static void Definir(ModelBuilder mb)
        {
            mb.Entity<TrazaDeUnTrabajoDtm>().ToTable("LOG", "TRABAJO");
            mb.Entity<TrazaDeUnTrabajoDtm>().Property(p => p.IdTrabajoDeUsuario).HasColumnName("ID_TRABAJO_USUARIO").IsRequired(true).HasColumnType("INT");
            mb.Entity<TrazaDeUnTrabajoDtm>().Property(p => p.Log).HasColumnName("LOG").IsRequired(true).HasColumnType("VARCHAR(MAX)");
            mb.Entity<TrazaDeUnTrabajoDtm>().HasOne(x => x.TrabajoDeUsuario).WithMany().HasForeignKey(x => x.Id).HasConstraintName("FK_LOG_DE_TRABAJO_ID_TRABAJO_USUARIO").OnDelete(DeleteBehavior.Restrict);
        }
    }
}