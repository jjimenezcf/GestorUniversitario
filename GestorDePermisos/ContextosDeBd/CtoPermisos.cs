using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Gestor.Elementos.Seguridad
{

    public class CtoPermisos : ContextoDeElementos
    {
        public DbSet<PermisoReg> Permisos { get; set; }
        public DbSet<RolReg> Roles { get; set; }
        public DbSet<PuestoReg> Puestos { get; set; }
        public DbSet<UsuarioView> Usuarios { get; set; }
        public DbSet<RolPermisoReg> PermisosDeUnRol { get; set; }
        public DbSet<RolPuestoReg> PuestosDeUnRol { get; set; }
        public DbSet<UsuPuestoReg> PuestosDeUnUsuario { get; set; }

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
