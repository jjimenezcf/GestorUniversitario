using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;

namespace ServicioDeDatos.Callejero
{
    [Table("PAIS", Schema = "CALLEJERO")]
    public class PaisDtm : ElementoDtm
    {
        [Required]
        [Column("CODIGO", Order = 3, TypeName = "VARCHAR(3)")]
        public string Codigo { get; set; }
    }

    public static class TablaPais
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            GeneradorMd.DefinirElementoDto<PaisDtm>(modelBuilder);
            modelBuilder.Entity<PaisDtm>().Property(v => v.Codigo).IsRequired(true);
        }
    }

}
