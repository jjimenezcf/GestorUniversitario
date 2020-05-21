using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicioDeDatos.Seguridad
{


    [Table("ROL_PERMISO", Schema = "SEGURIDAD")]
    public class RolesDeUnPermiso : Registro
    {
        [Column("IDROL", TypeName = "INT")]
        public int IdRol { get; set; }

        [Column("IDPERMISO", TypeName = "INT")]
        public int IdPermiso { get; set; }

        public RolDtm Rol { get; set; }
        public PermisoDtm Permiso { get; set; }

    }

    public static class TablaRolPermiso
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolesDeUnPermiso>()
                .HasAlternateKey(p => new { p.IdRol, p.IdPermiso })
                .HasName("AK_ROL_PERMISO");

            modelBuilder.Entity<RolesDeUnPermiso>()
                .HasOne(x => x.Rol)
                .WithMany(r => r.Permisos)
                .HasForeignKey(x => x.IdRol)
                .HasConstraintName("FK_ROL_PERMISO_IDROL");

            modelBuilder.Entity<RolesDeUnPermiso>()
                .HasOne(x => x.Permiso)
                .WithMany(p => p.Roles)
                .HasForeignKey(x => x.IdPermiso)
                .HasConstraintName("FK_ROL_PERMISO_IDPERMISO");
        }
    }
}

