using Gestor.Elementos.ModeloBd;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.Seguridad
{

       
    [Table("ROL_PERMISO", Schema = "SEGURIDAD")]
    public class rRolPermiso : Registro
    {
        [Column("IDROL", TypeName = "INT")]
        public int IdRol { get; set; }

        [Column("IDPERMISO", TypeName = "INT")]
        public int IdPermiso { get; set; }

        public rRol Rol { get; set; }
        public PermisoDtm Permiso { get; set; }

    }

    public static class TablaRolPermiso
    {
        public static void Definir(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<rRolPermiso>()
                .HasAlternateKey(p => new { p.IdRol, p.IdPermiso })
                .HasName("AK_ROL_PERMISO");

            modelBuilder.Entity<rRolPermiso>()
                .HasOne(x => x.Rol)
                .WithMany(r => r.Permisos)
                .HasForeignKey(x => x.IdRol)
                .HasConstraintName("FK_ROL_PERMISO_IDROL");

            modelBuilder.Entity<rRolPermiso>()
                .HasOne(x => x.Permiso)
                .WithMany(p => p.Roles)
                .HasForeignKey(x => x.IdPermiso)
                .HasConstraintName("FK_ROL_PERMISO_IDPERMISO");
        }
    }
}

