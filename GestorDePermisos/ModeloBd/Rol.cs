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
    public class RegRol : Registro
    {
        [Required]
        [Column("NOMBRE", TypeName = "VARCHAR(250)")]
        public string Nombre { get; set; }

        [Column("DESCRIPCION", TypeName = "VARCHAR(MAX)")]
        public string Descripcion { get; set; }

        public ICollection<RegRolPermisos> Permisos { get; set; }
        public ICollection<RegRolPuesto> Puestos { get; set; }
    }

    public static class TablaRol
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RegRol>()
                        .HasAlternateKey(p => p.Nombre)
                        .HasName("AK_ROL_NOMBRE");
        }
    }
}
