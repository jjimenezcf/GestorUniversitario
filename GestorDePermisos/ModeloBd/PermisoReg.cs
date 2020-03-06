using Gestor.Elementos.ModeloBd;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Permiso
{
    [Table("PERMISO", Schema = "PERMISO")]
    public class PermisoReg : Registro
    {
        [Required]
        [Column("NOMBRE", Order = 1, TypeName = "VARCHAR(250)")]
        public string Nombre { get; set; }

        [Required]
        [Column("CLASE", Order = 1, TypeName = "INT")]
        [DefaultValue(0)]
        public int Clase { get; set; }

        [Required]
        [Column("TIENE", Order = 1, TypeName = "BIT")]
        [DefaultValue(0)]
        public bool Tiene { get; set; }

        [Required]
        [Column("PERMISO", Order = 1, TypeName = "BIT")]
        [DefaultValue(0)]
        public bool Permiso { get; set; }

        public ICollection<RolPermisoReg> Roles { get; set; }
    }
}
