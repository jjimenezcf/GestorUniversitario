using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Seguridad
{

    [Table("CLASE_PERMISO", Schema = "SEGURIDAD")]
    public class ClasePermisoDtm : Registro
    {
        [Required]
        [Column("NOMBRE", TypeName = "VARCHAR(30)")]
        public string Nombre { get; set; }

        public virtual ICollection<PermisoDtm> Permisos { get; set; }
    }

    public static class TablaClasePermiso
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClasePermisoDtm>()
                        .HasIndex(cp => cp.Nombre)
                        .HasName("I_CLASE_PERMISO_NOMBRE")
                        .IsUnique();

            modelBuilder.Entity<ClasePermisoDtm>()
                .HasMany(cp => cp.Permisos)
                .WithOne(p => p.Clase);

        }
    }
}
