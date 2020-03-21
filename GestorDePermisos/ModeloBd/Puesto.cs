using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Seguridad
{
    /*
     * 
     * 
       create table SEGURIDAD.PUESTO (
          ID                   int                  identity,
          NOMBRE               varchar(250)         not null,
          DESCRIPCION          varchar(MAX)         null
       )
       go
       
       alter table SEGURIDAD.PUESTO
          add constraint PK_PUESTO primary key (ID)
       go
       
       alter table SEGURIDAD.PUESTO
          add constraint AK_PUESTO_NOMBRE unique (NOMBRE)
       go
       
     */


    [Table("PUESTO", Schema = "SEGURIDAD")]
    public class RegPuesto : Registro
    {
        [Required]
        [Column("NOMBRE", TypeName = "VARCHAR(250)")]
        public string Nombre { get; set; }

        [Column("DESCRIPCION", TypeName = "VARCHAR(MAX)")]
        public string Descripcion { get; set; }

        public ICollection<RegRolPuesto> Roles { get; set; }
        public ICollection<RegUsuPuesto> Usuarios { get; set; }
    }

    public static class TablaPuesto
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegPuesto>()
            .HasAlternateKey(p => p.Nombre)
            .HasName("AK_PUESTO_NOMBRE");
        }
    }
}
