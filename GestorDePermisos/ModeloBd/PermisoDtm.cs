using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Seguridad
{

    [Table("PERMISO", Schema = "SEGURIDAD")]
    public class PermisoDtm : Registro
    {
        [Required]
        [Column("NOMBRE", TypeName = "VARCHAR(250)")]
        public string Nombre { get; set; }

        [Required]
        [Column("CLASE", TypeName = "VARCHAR(30)")]
        [DefaultValue(0)]
        public int Clase { get; set; }


        [Required]
        [Column("PERMISO", TypeName = "VARCHAR(30)")]
        [DefaultValue(0)]
        public int Permiso { get; set; }

        public ICollection<rRolPermiso> Roles { get; set; }
        public ICollection<PerUsuarioDtm> Usuarios { get; set; }
    }

    public static class TablaPermiso
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermisoDtm>()
                        .HasAlternateKey(p => p.Nombre)
                        .HasName("AK_PERMISO_NOMBRE");

            modelBuilder.Entity<PermisoDtm>()
                        .HasAlternateKey(p => new { p.Clase, p.Permiso})
                        .HasName("AK_PERMISO_PERMISO");
        }
    }
}
