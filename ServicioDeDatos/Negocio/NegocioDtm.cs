using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Seguridad;

namespace ServicioDeDatos.Negocio
{
    [Table("NEGOCIO", Schema = "NEGOCIO")]
    public class NegocioDtm : Registro, INombre
    {
        [Required]
        [Column("ELEMENTO", TypeName = "VARCHAR(250)")]
        public string Elemento { get; set; }

        [Column("ICONO", TypeName = "VARCHAR(250)")]
        public string Icono { get; set; }

        [Required]
        [Column("ACTIVO", TypeName = "BIT")]
        public bool Activo { get; set; }

        [Column("IDPERMISO_GESTOR", TypeName = "INT")]
        public int IdPermisoDeGestor { get; set; }

        [Column("IDPERMISO_CONSULTOR", TypeName = "INT")]
        public int IdPermisoDeConsultor { get; set; }

        [Column("IDPERMISO_ADMINISTRADOR", TypeName = "INT")]
        public int IdPermisoDeAdministrador { get; set; }

        public PermisoDtm PermisoDeGestor { get; set; }
        public PermisoDtm PermisoDeConsultor { get; set; }
        public PermisoDtm PermisoDeAdministrador { get; set; }
    }


    public static class TablaNegocio
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NegocioDtm>().Property(p => p.Nombre).HasColumnName("NOMBRE").HasColumnType("VARCHAR(250)").IsRequired();

            modelBuilder.Entity<NegocioDtm>().Property(p => p.Elemento).IsRequired(true);

            modelBuilder.Entity<NegocioDtm>().Property(p => p.Activo).IsRequired(true).HasDefaultValue(true);

            modelBuilder.Entity<NegocioDtm>()
            .HasOne(p => p.PermisoDeGestor)
            .WithMany()
            .IsRequired(true)
            .HasForeignKey(p => p.IdPermisoDeGestor)
            .HasConstraintName("FK_NEGOCIO_IDPERMISO_GESTOR")
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NegocioDtm>()
            .HasOne(p => p.PermisoDeConsultor)
            .WithMany()
            .IsRequired(true)
            .HasForeignKey(p => p.IdPermisoDeConsultor)
            .HasConstraintName("FK_NEGOCIO_IDPERMISO_CONSULTOR")
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NegocioDtm>()
            .HasOne(p => p.PermisoDeAdministrador)
            .WithMany()
            .IsRequired(true)
            .HasForeignKey(p => p.IdPermisoDeAdministrador)
            .HasConstraintName("FK_NEGOCIO_IDPERMISO_ADMINISTRADOR")
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NegocioDtm>()
               .HasIndex(p => new { p.IdPermisoDeAdministrador })
               .IsUnique(true)
               .HasDatabaseName("IX_NEGOCIO_IDPERMISO_ADMINISTRADOR");

            modelBuilder.Entity<NegocioDtm>()
               .HasIndex(p => new { p.IdPermisoDeConsultor })
               .IsUnique(true)
               .HasDatabaseName("IX_NEGOCIO_IDPERMISO_CONSULTOR");

            modelBuilder.Entity<NegocioDtm>()
               .HasIndex(p => new { p.IdPermisoDeGestor })
               .IsUnique(true)
               .HasDatabaseName("IX_NEGOCIO_IDPERMISO_GESTOR");

            modelBuilder.Entity<NegocioDtm>().HasIndex(x => x.Nombre)
               .IsUnique(true)
               .HasDatabaseName("IX_NEGOCIO_NOMBRE");

        }
    }

}
