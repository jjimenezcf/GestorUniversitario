using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Seguridad
{

    /*
    create table SEGURIDAD.ROL_PERMISO (
       ID                   int                  identity,
       IDROL                int                  not null,
       IDPERMISO            int                  not null
    )
    go
    
    alter table SEGURIDAD.ROL_PERMISO
       add constraint PK_ROL_PERMISO primary key (ID)
    go
    
    alter table SEGURIDAD.ROL_PERMISO
       add constraint AK_ROL_PERMISO unique (IDROL, IDPERMISO)
    go
    
    alter table SEGURIDAD.ROL_PERMISO
       add constraint FK_ROL_PERMISO_IDPERMISO foreign key (IDPERMISO)
          references SEGURIDAD.PERMISO (ID)
    go
    
    alter table SEGURIDAD.ROL_PERMISO
       add constraint FK_ROL_PERMISO_IDROL foreign key (IDROL)
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

    public static class TablaRolPermiso
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolPermisoReg>()
                .HasAlternateKey(p => new { p.IdRol, p.IdPermiso })
                .HasName("AK_ROL_PERMISO");

            modelBuilder.Entity<RolPermisoReg>()
                .HasOne(x => x.Rol)
                .WithMany(r => r.Permisos)
                .HasForeignKey(x => x.IdRol)
                .HasConstraintName("FK_ROL_PERMISO_IDROL");

            modelBuilder.Entity<RolPermisoReg>()
                .HasOne(x => x.Permiso)
                .WithMany(p => p.Roles)
                .HasForeignKey(x => x.IdPermiso)
                .HasConstraintName("FK_ROL_PERMISO_IDPERMISO");
        }
    }
}

