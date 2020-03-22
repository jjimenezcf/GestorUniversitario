using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Seguridad
{

    [Table("PERMISO", Schema = "SEGURIDAD")]
    public class rPermiso : Registro
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

        public ICollection<rRolPermiso> Roles { get; set; }
    }

    public static class TablaPermiso
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<rPermiso>()
                        .HasAlternateKey(p => p.Nombre)
                        .HasName("AK_PERMISO_NOMBRE");

            modelBuilder.Entity<rPermiso>()
                        .HasAlternateKey(p => new { p.Clase, p.Permiso})
                        .HasName("AK_PERMISO_PERMISO");
        }
    }
}
