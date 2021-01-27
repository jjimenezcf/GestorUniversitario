using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;

namespace ServicioDeDatos.Entorno
{
    [Table("VARIABLE", Schema = "ENTORNO")]
    public class VariableDtm : Registro, INombre
    {
        [Column("DESCRIPCION", Order = 4, TypeName = "VARCHAR(MAX)")]
        public string Descripcion { get; set; }

        [Required]
        [Column("VALOR", Order = 3, TypeName = "VARCHAR(250)")]
        public string Valor { get; set; }
    }

    public static class TablaVariable
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VariableDtm>().Property(p => p.Nombre).HasColumnName("NOMBRE").HasColumnType("VARCHAR(50)").IsRequired();
            modelBuilder.Entity<VariableDtm>().Property(v => v.Descripcion).IsRequired(false);
            modelBuilder.Entity<VistaMvcDtm>()
            .HasIndex(v => new { v.Nombre })
            .IsUnique(true)
            .HasDatabaseName("IX_VARIABLE");

        }
    }

}
