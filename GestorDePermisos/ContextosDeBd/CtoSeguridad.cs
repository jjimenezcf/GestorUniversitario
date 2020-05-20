using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Gestor.Elementos.Seguridad
{
    public class ConstructorDelContexto : IDesignTimeDbContextFactory<CtoSeguridad>
    {
        public CtoSeguridad CreateDbContext(string[] arg)
        {

            var datosDeConexion = ContextoDeElementos.ObtenerDatosDeConexion();

            var opciones = new DbContextOptionsBuilder<CtoSeguridad>();
            opciones.UseSqlServer(datosDeConexion.CadenaConexion);
            object[] parametros = { opciones.Options, datosDeConexion.Configuracion };

            return (CtoSeguridad)Activator.CreateInstance(typeof(CtoSeguridad), parametros);
        }
    }

    public class CtoSeguridad : ContextoDeElementos
    {
        public static CtoSeguridad CrearContexto()
        {
            return (CtoSeguridad)ObtenerContexto(nameof(CtoSeguridad), () => new ConstructorDelContexto().CreateDbContext(new string[] { }));
        }


        #region dbSets del contexto de seguridad

        public DbSet<TipoPermisoDtm> TiposDePermisos { get; set; }
        public DbSet<ClasePermisoDtm> ClasesDePermisos { get; set; }
        public DbSet<PermisoDtm> Permisos { get; set; }
        public DbSet<RolDtm> Roles { get; set; }
        public DbSet<rPuesto> Puestos { get; set; }
        public DbSet<vUsuario> Usuarios { get; set; }
        public DbSet<RolPermisoDtm> PermisosDeUnRol { get; set; }
        public DbSet<rRolPuesto> PuestosDeUnRol { get; set; }
        public DbSet<RegUsuPuesto> PuestosDeUnUsuario { get; set; }

        #endregion

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
