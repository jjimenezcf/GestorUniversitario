using System.ComponentModel.DataAnnotations.Schema;
using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;

namespace Gestor.Elementos.Seguridad
{
    /*
    create table SEGURIDAD.ROL_PUESTO (
       ID                   int                  identity,
       IDROL                int                  not null,
       IDPUESTO             int                  not null
    )
    go
    
    alter table SEGURIDAD.ROL_PUESTO
       add constraint PK_ROL_PUESTO primary key (ID)
    go
    
    alter table SEGURIDAD.ROL_PUESTO
       add constraint AK_ROL_PUESTO unique (IDROL, IDPUESTO)
    go
    
    alter table SEGURIDAD.ROL_PUESTO
       add constraint FK_ROL_PUESTO_IDPUESTO foreign key (IDPUESTO)
          references SEGURIDAD.PUESTO (ID)
    go
    
    alter table SEGURIDAD.ROL_PUESTO
       add constraint FK_ROL_PUESTO_IDROL foreign key (IDROL)
          references SEGURIDAD.ROL (ID)
    go
    */

    [Table("ROL_PUESTO", Schema = "SEGURIDAD")]
    public class RolPuestoReg: Registro
    {
        [Column("IDROL", TypeName = "INT")]
        public int IdRol { get; set; }

        [Column("IDPUESTO", TypeName = "INT")]
        public int idPuesto { get; set; }

        public RolReg Rol { get; set; }
        public PuestoReg Puesto { get; set; }
    }

    public static class TablaRolPuesto
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolPuestoReg>()
                .HasAlternateKey(p => new { p.IdRol, p.idPuesto })
                .HasName("AK_ROL_PUESTO");

            modelBuilder.Entity<RolPuestoReg>()
                .HasOne(x => x.Rol)
                .WithMany(r => r.Puestos)
                .HasForeignKey(x => x.IdRol)
                .HasConstraintName("FK_ROL_PUESTO_IDROL");

            modelBuilder.Entity<RolPuestoReg>()
                .HasOne(x => x.Puesto)
                .WithMany(p => p.Roles)
                .HasForeignKey(x => x.idPuesto)
                .HasConstraintName("FK_ROL_PUESTO_IDPUESTO");
        }
    }
}
