using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Entorno;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioDeDatos.Seguridad
{

    [Table("PERMISO", Schema = "SEGURIDAD")]
    public class PermisoDtm : Registro
    {
        //[Required]
        //[Column("NOMBRE", TypeName = "VARCHAR(250)")]
        //public string Nombre { get; set; }

        public int IdClase { get; set; }
        public virtual ClasePermisoDtm Clase { get; set; }
        
        public int IdTipo { get; set; }

        public virtual TipoPermisoDtm Tipo { get; set; }

        public ICollection<PermisosDeUnRolDtm> Roles { get; set; }
        public ICollection<PermisosDeUnUsuarioDtm> Usuarios { get; set; }
        public ICollection<PermisosDeUnPuestoDtm> Puestos { get; set; }
    }

    public static class TablaPermiso
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermisoDtm>().Property(p => p.Nombre).HasColumnName("NOMBRE").HasColumnType("VARCHAR(250)").IsRequired();
            modelBuilder.Entity<PermisoDtm>()
                        .HasIndex(p => p.Nombre)
                        .HasName("I_PERMISO_NOMBRE")
                        .IsUnique();

            modelBuilder.Entity<PermisoDtm>().Property(p => p.IdTipo).IsRequired();
            modelBuilder.Entity<PermisoDtm>().Property(p => p.IdClase).IsRequired();

            modelBuilder.Entity<PermisoDtm>().Property(p => p.IdTipo).HasColumnName("IDTIPO");
            modelBuilder.Entity<PermisoDtm>().Property(p => p.IdClase).HasColumnName("IDCLASE");

            modelBuilder.Entity<PermisoDtm>()
                        .HasIndex(p => new {p.IdClase, p.IdTipo})
                        .HasName("I_PERMISO_IDCLASE_IDTIPO");

            modelBuilder.Entity<PermisoDtm>()
                        .HasOne(p => p.Clase)
                        .WithMany(cp => cp.Permisos)
                        .HasForeignKey(p => p.IdClase)
                        .HasConstraintName("FK_PERMISO_IDCLASE")
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PermisoDtm>()
                        .HasOne(p => p.Tipo)
                        .WithMany(tp => tp.Permisos)
                        .HasForeignKey(p => p.IdTipo)
                        .HasConstraintName("FK_PERMISO_IDTIPO")
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PermisoDtm>()
                .HasMany(p => p.Roles)
                .WithOne(p => p.Permiso)
                .HasForeignKey(p => p.IdRol);

            modelBuilder.Entity<PermisoDtm>()
                .HasMany(p => p.Usuarios)
                .WithOne(p => p.Permiso)
                .HasForeignKey(p => p.IdPermiso);
        }
    }
}
