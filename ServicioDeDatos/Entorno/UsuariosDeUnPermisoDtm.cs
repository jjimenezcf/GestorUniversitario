using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Seguridad;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioDeDatos.Entorno
{
    public class UsuariosDeUnPermisoDtm : Registro
    {
        [Required]
        [Column("IDUSUA", TypeName = "INT")]
        public int IdUsua { get; set; }

        [Required]
        [Column("IDPERMISO", TypeName = "INT")]
        public int IdPermiso { get; set; }

        public virtual UsuarioDtm Usuario { get; set; }

        public virtual PermisoDtm Permiso { get; set; }
    }

    public static class VistaUsuarioPermiso
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuariosDeUnPermisoDtm>()
                .ToTable("USU_PERMISO", "ENTORNO")
                .HasKey(x => new { x.Id });

            modelBuilder.Entity<UsuariosDeUnPermisoDtm>()
                .HasOne(x => x.Usuario)
                .WithMany(x => x.Permisos)
                .HasForeignKey(x => x.IdUsua);

            modelBuilder.Entity<UsuariosDeUnPermisoDtm>()
                .HasOne(x => x.Permiso)
                .WithMany(x => x.Usuarios)
                .HasForeignKey(x => x.IdUsua);
        }
    }
}
