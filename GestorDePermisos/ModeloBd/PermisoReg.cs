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


    /*
     * 
       create table SEGURIDAD.PERMISO (
          ID                   int                  identity,
          NOMBRE               varchar(250)         not null,
          CLASE                decimal(2,0)         not null,
          PERMISO              decimal(2,0)         not null
       )
       go
       
       alter table SEGURIDAD.PERMISO
          add constraint PK_PERMISO primary key (ID)
       go
       
       alter table SEGURIDAD.PERMISO
          add constraint AK_PERMISO_NOMBRE unique (NOMBRE)
       go
     * 
     */

    [Table("PERMISO", Schema = "SEGURIDAD")]
    public class PermisoReg : Registro
    {
        [Required]
        [Column("NOMBRE", TypeName = "VARCHAR(250)")]
        public string Nombre { get; set; }

        [Required]
        [Column("CLASE", TypeName = "decimal(2,0)")]
        [DefaultValue(0)]
        public int Clase { get; set; }


        [Required]
        [Column("PERMISO", TypeName = "decimal(2,0)")]
        [DefaultValue(0)]
        public int Permiso { get; set; }

        public ICollection<RolPermisoReg> Roles { get; set; }
    }
}
