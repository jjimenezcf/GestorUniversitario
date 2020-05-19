using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;

namespace Gestor.Elementos.Entorno
{
    [Table("VARIABLE", Schema = "ENTORNO")]
    public class VariableDtm : Registro
    {
        [Required]
        [Column("NOMBRE", Order = 3, TypeName = "VARCHAR(50)")]
        public string Nombre { get; set; }

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
            modelBuilder.Entity<VariableDtm>().Property(v => v.Descripcion).IsRequired(false);
            modelBuilder.Entity<VistaMvcDtm>()
            .HasIndex(v => new { v.Nombre })
            .IsUnique(true)
            .HasName("IX_VARIABLE");

        }
    }

}
