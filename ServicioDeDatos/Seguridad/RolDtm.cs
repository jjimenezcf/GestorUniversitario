using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioDeDatos.Seguridad
{

    [Table("ROL", Schema = "SEGURIDAD")]
    public class RolDtm : Registro
    {
        [Required]
        [Column("NOMBRE", TypeName = "VARCHAR(250)")]
        public string Nombre { get; set; }

        [Column("DESCRIPCION", TypeName = "VARCHAR(MAX)")]
        public string Descripcion { get; set; }

        public ICollection<RolesDeUnPermiso> Permisos { get; set; }
        public ICollection<RolesDeUnPuestoDtm> Puestos { get; set; }
    }

    public static class TablaRol
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolDtm>()
                        .HasIndex(p => p.Nombre)
                        .HasName("I_ROL_NOMBRE")
                        .IsUnique();

            modelBuilder.Entity<RolDtm>()
                    .HasMany(p => p.Permisos)
                    .WithOne(p => p.Rol)
                    .HasForeignKey(p => p.IdPermiso);

            modelBuilder.Entity<RolDtm>()
                    .HasMany(p => p.Puestos)
                    .WithOne(p => p.Rol)
                    .HasForeignKey(p => p.IdRol);
        }
    }
}
