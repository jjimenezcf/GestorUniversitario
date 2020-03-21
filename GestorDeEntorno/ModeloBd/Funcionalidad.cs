using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gestor.Elementos.ModeloBd;

namespace Gestor.Elementos.Entorno
{
    [Table("FUN_ACCION", Schema = "ENTORNO")]
    public class RegAccion : Registro
    {
        [Required]
        [Column("CONTROLADOR", Order = 1, TypeName = "VARCHAR(250)")]
        public string Controlador { get; set; }

        [Required]
        [Column("ACCION", Order = 2, TypeName = "VARCHAR(250)")]
        public string Accion { get; set; }


        [Required]
        [Column("PARAMETROS", Order = 3, TypeName = "VARCHAR(250)")]
        public string Parametros { get; set; }
    }

    [Table("FUN_ELEMENTO", Schema = "ENTORNO")]
    public class RegFuncion : Registro
    {
        [Required]
        [Column("IDPADRE", Order = 2, TypeName = "INT")]
        public decimal IdPadre { get; set; }

        [Required]
        [Column("NOMBRE", Order = 3, TypeName = "VARCHAR(250)")]
        public string Nombre { get; set; }

        [Column("DESCRIPCION", Order = 4, TypeName = "VARCHAR(MAX)")]
        public string Descripcion { get; set; }

        [Required]
        [Column("ACTIVO", Order = 5, TypeName = "CHAR(1)")]
        public char Activo { get; set; }

        [ForeignKey("IDACCION")]
        public virtual RegAccion Accion { get; set; }
    }


}
