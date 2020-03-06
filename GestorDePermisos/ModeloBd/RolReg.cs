using Gestor.Elementos.ModeloBd;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Permiso
{
    [Table("ROL", Schema = "PERMISO")]
    public class RolReg : Registro
    {
        [Required]
        [Column("NOMBRE", Order = 1, TypeName = "VARCHAR(250)")]
        public string Nombre { get; set; }

        public ICollection<RolPermisoReg> Permisos { get; set; }
    }
}
