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
            modelBuilder.Entity<PermisoReg>();
            modelBuilder.Entity<RolReg>();
            modelBuilder.Entity<RolPermisoReg>();

        }


    }
}
