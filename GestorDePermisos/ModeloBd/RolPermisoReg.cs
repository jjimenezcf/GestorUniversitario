using Gestor.Elementos.ModeloBd;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Permiso
{
    [Table("ROL_PERMISO", Schema = "PERMISO")]
    public class RolPermisoReg : Registro
    {
        public int IdRol { get; set; }
        public int IdPermiso { get; set; }

        public PermisoReg Curso { get; set; }

    }


}
