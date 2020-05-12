using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Gestor.Elementos.Seguridad
{


    public class CtoSeguridad : ContextoDeElementos
    {
        public class ConstructorDelContexto : IDesignTimeDbContextFactory<CtoSeguridad>
        {
            public CtoSeguridad CreateDbContext(string[] arg)
            {
                var generador = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json");
                var configuaracion = generador.Build();
                var cadenaDeConexion = configuaracion.GetConnectionString(Gestor.Elementos.Literal.CadenaDeConexion);

                var opciones = new DbContextOptionsBuilder<CtoSeguridad>();
                opciones.UseSqlServer(cadenaDeConexion);
                object[] parametros = { opciones.Options, configuaracion };

                return (CtoSeguridad)Activator.CreateInstance(typeof(CtoSeguridad), parametros);
            }
        }


        public DbSet<TipoPermisoDtm> TiposDePermisos { get; set; }
        public DbSet<ClasePermisoDtm> ClasesDePermisos { get; set; }
        public DbSet<PermisoDtm> Permisos { get; set; }
        public DbSet<RolDtm> Roles { get; set; }
        public DbSet<rPuesto> Puestos { get; set; }
        public DbSet<vUsuario> Usuarios { get; set; }
        public DbSet<RolPermisoDtm> PermisosDeUnRol { get; set; }
        public DbSet<rRolPuesto> PuestosDeUnRol { get; set; }
        public DbSet<RegUsuPuesto> PuestosDeUnUsuario { get; set; }

        public CtoSeguridad(DbContextOptions<CtoSeguridad> options, IConfiguration configuracion) :
        base(options, configuracion)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            TablaClasePermiso.Definir(modelBuilder);

            TablaPermisoTipo.Definir(modelBuilder);

            TablaRol.Definir(modelBuilder);

            TablaPermiso.Definir(modelBuilder);

            TablaPuesto.Definir(modelBuilder);

            VistaUsuario.Definir(modelBuilder);

            TablaRolPermiso.Definir(modelBuilder);

            TablaRolPuesto.Definir(modelBuilder);

            TablaUsuPuesto.Definir(modelBuilder);

            VistaUsuarioPermiso.Definir(modelBuilder);
        }


    }
}
