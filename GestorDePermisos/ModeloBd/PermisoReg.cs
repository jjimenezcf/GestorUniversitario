using Gestor.Elementos.ModeloBd;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Permiso
{
    enum Clase
    {
        Tipo = 1,
        Estado = 2,
        Transicion = 3,
        CentroGestor = 4,
        Negocio = 5,
        Elemento = 6
    }
    enum Permiso
    {
        Gestor = 1,
        Consultor = 2,
        Creador = 3,
        Administrador = 4,
        Acceso = 5
    }

    [Table("PERMISO", Schema = "PERMISO")]
    public class PermisoReg : Registro
    {
        [Required]
        [Column("NOMBRE", Order = 1, TypeName = "VARCHAR(250)")]
        public string Nombre { get; set; }

        [Required]
        [Column("CLASE", Order = 3, TypeName = "decimal(2,0)")]
        [DefaultValue(0)]
        public int Clase { get; set; }


        [Required]
        [Column("PERMISO", Order = 4, TypeName = "decimal(2,0)")]
        [DefaultValue(0)]
        public int Permiso { get; set; }

        public ICollection<RolPermisoReg> Roles { get; set; }
    }
}
