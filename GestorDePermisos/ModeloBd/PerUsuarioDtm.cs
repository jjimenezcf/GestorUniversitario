using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Seguridad
{
    public class PerUsuarioDtm: Registro
    {
        [Required]
        [Column("IDUSUA", TypeName = "INT")]
        public int IdUsua { get; set; }

        [Required]
        [Column("IDPERMISO", TypeName = "INT")]
        public int IdPermiso { get; set; }

        public virtual PermisoDtm Permiso { get; set; }
    }
    public static class VistaUsuarioPermiso
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PerUsuarioDtm>()
                .ToTable("USU_PERMISO", "ENTORNO")
                .HasKey(x => new { x.Id });

            modelBuilder.Entity<PerUsuarioDtm>()
                .HasOne(x => x.Permiso)
                .WithMany(x => x.Usuarios)
                .HasForeignKey(x => x.IdPermiso);
        }
    }
}
