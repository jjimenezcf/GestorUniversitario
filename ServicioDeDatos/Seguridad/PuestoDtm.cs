using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioDeDatos.Seguridad
{

    [Table("PUESTO", Schema = "SEGURIDAD")]
    public class PuestoDtm : RegistroConNombre
    {
        [Column("DESCRIPCION", TypeName = "VARCHAR(MAX)")]
        public string Descripcion { get; set; }
        public string RolesDeUnPuesto { get; }

        public virtual ICollection<RolesDeUnPuestoDtm> Roles { get; set; }
        public virtual ICollection<PuestosDeUnUsuarioDtm> Usuarios { get; set; }

        public virtual ICollection<PermisosDeUnPuestoDtm> Permisos { get; set; }
    }

    public static class TablaPuesto
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PuestoDtm>().Property(p => p.Nombre).HasColumnName("NOMBRE").HasColumnType("VARCHAR(250)").IsRequired();
            modelBuilder.Entity<PuestoDtm>()
            .HasIndex(p => p.Nombre)
            .HasDatabaseName("I_PUESTO_NOMBRE")
            .IsUnique();

            modelBuilder.Entity<PuestoDtm>().Property(p => p.RolesDeUnPuesto).HasColumnName("ROLES").HasColumnType("VARCHAR(MAX)").HasComputedColumnSql("SEGURIDAD.OBTENER_ROLES_DE_UN_PUESTO(id)");

            modelBuilder.Entity<PuestoDtm>()
                    .HasMany(p => p.Usuarios)
                    .WithOne(p => p.Puesto)
                    .HasForeignKey(p => p.IdUsuario);

            modelBuilder.Entity<PuestoDtm>()
                    .HasMany(p => p.Roles)
                    .WithOne(p => p.Puesto)
                    .HasForeignKey(p => p.IdRol);

        }
    }
}
