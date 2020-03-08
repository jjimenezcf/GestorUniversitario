using Gestor.Elementos.ModeloBd;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Permiso
{
    /*
     * 
     * 
     create table SEGURIDAD.ROL (
             ID                   int                  identity,
             NOMBRE               varchar(250)         not null,
             DESCRIPCION          varchar(MAX)         null
          )
          alter table SEGURIDAD.ROL
              add constraint PK_ROL primary key (ID)
          go
          
          alter table SEGURIDAD.ROL
             add constraint AK_ROL_NOMBRE unique (NOMBRE)
          go
     */


    [Table("ROL", Schema = "SEGURIDAD")]
    public class RolReg : Registro
    {
        [Required]
        [Column("NOMBRE", TypeName = "VARCHAR(250)")]
        public string Nombre { get; set; }

        [Required]
        [Column("DESCRIPCION", TypeName = "VARCHAR(MAX)")]
        public string Descripcion { get; set; }

        public ICollection<RolPermisoReg> Permisos { get; set; }
    }
}
