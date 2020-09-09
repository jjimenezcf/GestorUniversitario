using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace ServicioDeDatos.Seguridad
{


    [Table("ROL_PERMISO", Schema = "SEGURIDAD")]
    public class PermisosDeUnRolDtm : RegistroDeRelacion
    {
        [Column("IDROL", TypeName = "INT")]
        public int IdRol { get; set; }

        [Column("IDPERMISO", TypeName = "INT")]
        public int IdPermiso { get; set; }

        public RolDtm Rol { get; set; }
        public PermisoDtm Permiso { get; set; }

        public PermisosDeUnRolDtm()
        {
            NombreDeLaPropiedadDelIdElemento1 = nameof(IdRol);
            NombreDeLaPropiedadDelIdElemento2 = nameof(IdPermiso);
        }

    }


    //[Table("PERMISO_ROL", Schema = "SEGURIDAD")]
    //public class RolesDeUnPermisoDtm : RegistroDeRelacion
    //{
    //    [Column("IDROL", TypeName = "INT")]
    //    public int IdRol { get; set; }

    //    [Column("IDPERMISO", TypeName = "INT")]
    //    public int IdPermiso { get; set; }

    //    public RolDtm Rol { get; set; }

    //    public PermisoDtm Permiso { get; set; }

    //    public RolesDeUnPermisoDtm()
    //    {
    //        NombreDeLaPropiedadDelIdElemento1 = nameof(IdRol);
    //        NombreDeLaPropiedadDelIdElemento2 = nameof(IdPermiso);
    //    }
    //}

    public static class TablaRolPermiso
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PermisosDeUnRolDtm>()
                .HasAlternateKey(p => new { p.IdRol, p.IdPermiso })
                .HasName("AK_ROL_PERMISO");

            modelBuilder.Entity<PermisosDeUnRolDtm>()
                .HasOne(x => x.Rol)
                .WithMany(r => r.Permisos)
                .HasForeignKey(x => x.IdRol)
                .HasConstraintName("FK_ROL_PERMISO_IDROL")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PermisosDeUnRolDtm>()
                .HasOne(x => x.Permiso)
                .WithMany(p => p.Roles)
                .HasForeignKey(x => x.IdPermiso)
                .HasConstraintName("FK_ROL_PERMISO_IDPERMISO")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }

    //public static class TablaPermisoRol
    //{
    //    public static void Definir(ModelBuilder modelBuilder)
    //    {
    //        modelBuilder.Entity<RolesDeUnPermisoDtm>()
    //            .HasAlternateKey(p => new { p.IdRol, p.IdPermiso })
    //            .HasName("AK_ROL_PERMISO");

    //        modelBuilder.Entity<RolesDeUnPermisoDtm>()
    //            .HasOne(x => x.Rol)
    //            .WithMany()
    //            .HasForeignKey(x => x.IdRol)
    //            .HasConstraintName("FK_ROL_PERMISO_IDROL")
    //            .OnDelete(DeleteBehavior.Restrict);

    //        modelBuilder.Entity<RolesDeUnPermisoDtm>()
    //            .HasOne(x => x.Permiso)
    //            .WithMany()
    //            .HasForeignKey(x => x.IdPermiso)
    //            .HasConstraintName("FK_ROL_PERMISO_IDPERMISO")
    //            .OnDelete(DeleteBehavior.Restrict);
    //    }
    //}
}

