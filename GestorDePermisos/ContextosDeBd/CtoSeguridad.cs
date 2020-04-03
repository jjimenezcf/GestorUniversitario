using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Gestor.Elementos.Seguridad
{

    public class CtoSeguridad : ContextoDeElementos
    {
        public DbSet<PermisoDtm> Permisos { get; set; }
        public DbSet<rRol> Roles { get; set; }
        public DbSet<rPuesto> Puestos { get; set; }
        public DbSet<vUsuario> Usuarios { get; set; }
        public DbSet<rRolPermiso> PermisosDeUnRol { get; set; }
        public DbSet<rRolPuesto> PuestosDeUnRol { get; set; }
        public DbSet<RegUsuPuesto> PuestosDeUnUsuario { get; set; }

        public CtoSeguridad(DbContextOptions<CtoSeguridad> options, IConfiguration configuracion) :
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
