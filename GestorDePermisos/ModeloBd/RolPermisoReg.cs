using Gestor.Elementos.ModeloBd;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Permiso
{

    /*
     create table SEGURIDAD.ROL_PERMISO (
        ID                   int                  identity,
        IDROL                int                  not null,
        IDPERMISO            int                  not null
     )
     go
     
     alter table SEGURIDAD.ROL_PERMISO
        add constraint fk_rol_permiso_idpermiso foreign key (IDPERMISO)
           references SEGURIDAD.PERMISO (ID)
     go
     
     alter table SEGURIDAD.ROL_PERMISO
        add constraint fk_rol_permiso_idrol foreign key (IDROL)
           references SEGURIDAD.ROL (ID)
     go     
    
    */



    [Table("ROL_PERMISO", Schema = "SEGURIDAD")]
    public class RolPermisoReg : Registro
    {
        [Column("IDROL", TypeName = "INT")]
        public int IdRol { get; set; }

        [Column("IDPERMISO", TypeName = "INT")]
        public int IdPermiso { get; set; }

        public RolReg Rol { get; set; }
        public PermisoReg Permiso { get; set; }

    }


}
