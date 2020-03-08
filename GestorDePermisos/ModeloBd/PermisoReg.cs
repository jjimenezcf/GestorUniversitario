using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Seguridad
{

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

       alter table SEGURIDAD.PERMISO
       add constraint AK_PERMISO_PERMISO unique (CLASE, PERMISO)
       go 
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

    public static class TablaPermiso
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermisoReg>()
                        .HasAlternateKey(p => p.Nombre)
                        .HasName("AK_PERMISO_NOMBRE");

            modelBuilder.Entity<PermisoReg>()
                        .HasAlternateKey(p => new { p.Clase, p.Permiso})
                        .HasName("AK_PERMISO_PERMISO");
        }
    }
}
