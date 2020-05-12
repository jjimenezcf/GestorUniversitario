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

        [Column("IDCLASE", TypeName = "INT")]
        public int IdClase { get; set; }
        public virtual ClasePermisoDtm Clase { get; set; }
        
        [Required]
        [Column("IDTIPO", TypeName = "INT")]
        public int IdTipo { get; set; }

        public virtual TipoPermisoDtm Tipo { get; set; }

        public ICollection<RolPermisoDtm> Roles { get; set; }
        public ICollection<PerUsuarioDtm> Usuarios { get; set; }
    }

    public static class TablaPermiso
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermisoDtm>()
                        .HasIndex(p => p.Nombre)
                        .HasName("I_PERMISO_NOMBRE")
                        .IsUnique();

            modelBuilder.Entity<PermisoDtm>().Property(p => p.IdTipo).IsRequired();
            modelBuilder.Entity<PermisoDtm>().Property(p => p.IdClase).IsRequired();


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
        }
    }
}
