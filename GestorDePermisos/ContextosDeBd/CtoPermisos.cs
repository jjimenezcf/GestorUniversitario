using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Gestor.Elementos.Seguridad
{

    public class CtoPermisos : ContextoDeElementos
    {
        public DbSet<RegPermiso> Permisos { get; set; }
        public DbSet<RegRol> Roles { get; set; }
        public DbSet<RegPuesto> Puestos { get; set; }
        public DbSet<VisUsuario> Usuarios { get; set; }
        public DbSet<RegRolPermisos> PermisosDeUnRol { get; set; }
        public DbSet<RegRolPuesto> PuestosDeUnRol { get; set; }
        public DbSet<RegUsuPuesto> PuestosDeUnUsuario { get; set; }

        public CtoPermisos(DbContextOptions<CtoPermisos> options, IConfiguration configuracion) :
        base(options, configuracion)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            TablaRol.Definir(modelBuilder);

            TablaPermiso.Definir(modelBuilder);

            TablaPuesto.Definir(modelBuilder);

            VistaUsuario.Definir(modelBuilder);

            TablaRolPermiso.Definir(modelBuilder);

            TablaRolPuesto.Definir(modelBuilder);

            TablaUsuPuesto.Definir(modelBuilder);
        }


    }
}
