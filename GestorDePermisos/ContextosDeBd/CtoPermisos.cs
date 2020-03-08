using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Gestor.Elementos.Permiso
{

    public class CtoPermisos : ContextoDeElementos
    {
        public DbSet<PermisoReg> Permisos { get; set; }
        public DbSet<RolReg> Roles { get; set; }
        public DbSet<RolPermisoReg> PermisosDeUnRol { get; set; }

        public CtoPermisos(DbContextOptions<CtoPermisos> options, IConfiguration configuracion) :
        base(options, configuracion)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
            modelBuilder.Entity<PermisoReg>()
                .HasAlternateKey(p => p.Nombre)
                .HasName("AK_PERMISO_NOMBRE");

            modelBuilder.Entity<RolReg>()
                .HasAlternateKey(p => p.Nombre)
                .HasName("AK_ROL_NOMBRE");

            modelBuilder.Entity<RolPermisoReg>()
                .HasOne(x => x.Rol)
                .WithMany(r => r.Permisos)
                .HasForeignKey(x => x.IdRol)
                .HasConstraintName("FK_ROL_PERMISO_IDROL");

            modelBuilder.Entity<RolPermisoReg>()
                .HasOne(x => x.Permiso)
                .WithMany(p => p.Roles)
                .HasForeignKey(x => x.IdPermiso)
                .HasConstraintName("FK_ROL_PERMISO_IDPERMISO");
        }


    }
}
