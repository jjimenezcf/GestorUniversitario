using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gestor.Elementos.ModeloBd;

namespace Gestor.Elementos.Entorno
{
    [Table("VAR_ELEMENTO", Schema = "ENTORNO")]
    public class RegVariable : Registro
    {
        [Required]
        [Column("NOMBRE", Order = 3, TypeName = "VARCHAR(50)")]
        public string Nombre { get; set; }

        [Column("DESCRIPCION", Order = 4, TypeName = "VARCHAR(MAX)")]
        public string Descripcion { get; set; }

        [Required]
        [Column("VALOR", Order = 3, TypeName = "VARCHAR(50)")]
        public string Valor { get; set; }
    }
}
