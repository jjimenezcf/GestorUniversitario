
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;

namespace ServicioDeDatos.TrabajosSometidos
{
    public class TrabajoErrorDtm : Registro
    {
        public int idTrabajo { get; set; }
        public string Error { get; set; }
        public virtual TrabajoSometidoDtm Trabajo { get; set; }
    }

    public static class TablaTrabajoError
    {
        public static void Definir(ModelBuilder mb)
        {
            mb.Entity<TrabajoErrorDtm>().ToTable("ERROR", "TRABAJO");
            mb.Entity<TrabajoErrorDtm>().Property(p => p.idTrabajo).HasColumnName("ID_TRABAJO").IsRequired(true).HasColumnType("INT");
            mb.Entity<TrabajoErrorDtm>().Property(p => p.Error).HasColumnName("ERROR").IsRequired(true).HasColumnType("VARCHAR(MAX)");
            mb.Entity<TrabajoErrorDtm>().HasOne(x => x.Trabajo).WithMany().HasForeignKey(x => x.idTrabajo).HasConstraintName("FK_ERROR_DE_TRABAJO_ID_TRABAJO").OnDelete(DeleteBehavior.Restrict);
        }
    }


}