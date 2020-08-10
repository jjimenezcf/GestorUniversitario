using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioDeDatos.Seguridad
{

    [Table("ROL", Schema = "SEGURIDAD")]
    public class RolDtm : Registro
    {
        [Column("DESCRIPCION", TypeName = "VARCHAR(MAX)")]
        public string Descripcion { get; set; }

        public ICollection<RolesDeUnPermisoDtm> Permisos { get; set; }
        public ICollection<RolesDeUnPuestoDtm> Puestos { get; set; }
    }

    public static class TablaRol
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolDtm>().Property(p => p.Nombre).HasColumnName("NOMBRE").HasColumnType("VARCHAR(250)").IsRequired();

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
