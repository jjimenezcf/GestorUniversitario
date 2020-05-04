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
        [Column("IDCLASE", TypeName = "INT")]
        [DefaultValue(0)]
        public int IdClase { get; set; }
        public virtual ClasePermisoDtm Clase { get; set; }

        [Column("PERMISO", TypeName = "VARCHAR(30)")]
        public string Permiso { get; set; }

        public ICollection<rRolPermiso> Roles { get; set; }
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

            modelBuilder.Entity<PermisoDtm>()
                        .HasIndex(p => new {p.IdClase})
                        .HasName("I_PERMISO_IDCLASE");

            modelBuilder.Entity<PermisoDtm>()
                        .HasOne(p => p.Clase)
                        .WithMany(cp => cp.Permisos)
                        .HasForeignKey(p => p.IdClase)
                        .HasConstraintName("FK_PERMISO_IDCLASE")
                        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
