using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Gestor.Elementos;

namespace Gestor.Elementos.Entorno
{
    public class ConstructorDelContexto : IDesignTimeDbContextFactory<CtoEntorno>
    {
        public CtoEntorno CreateDbContext(string[] arg)
        {

            var datosDeConexion = ContextoDeElementos.ObtenerDatosDeConexion();

            var opciones = new DbContextOptionsBuilder<CtoEntorno>();
            opciones.UseSqlServer(datosDeConexion.CadenaConexion);
            object[] parametros = { opciones.Options, datosDeConexion.Configuracion };

            return (CtoEntorno)Activator.CreateInstance(typeof(CtoEntorno), parametros);
        }
    }

    public class CtoEntorno : ContextoDeElementos
    {

        public static CtoEntorno CrearContexto()
        {
            return (CtoEntorno)ObtenerContexto(nameof(CtoEntorno), () => new ConstructorDelContexto().CreateDbContext(new string[] { }));
        }

        #region dbSets del contexto de seguridad
        public DbSet<MenuDtm> Menus { get; set; }
        public DbSet<ArbolDeMenuDtm> MenuSe { get; set; }
        public DbSet<VistaMvcDtm> VistasMvc { get; set; }
        public DbSet<VariableDtm> Variables { get; set; }
        public DbSet<UsuarioDtm> Usuarios { get; set; }
        public DbSet<UsuPermisoDtm> UsuPermisos { get; set; }

        #endregion

        public CtoEntorno(DbContextOptions<CtoEntorno> options, IConfiguration configuracion) :
        base(options, configuracion)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            TablaUsuario.Definir(modelBuilder);
            TablaVariable.Definir(modelBuilder);
            TablaVistaMvc.Definir(modelBuilder);
            TablaMenu.Definir(modelBuilder);
            VistaMenuSe.Definir(modelBuilder);
            VistaUsuarioPermiso.Definir(modelBuilder);
        }

        public static void NuevaVersion(CtoEntorno cnx, string nuevaVersion)
        {
            var version = cnx.Variables.SingleOrDefault(v => v.Nombre == Variable.Version);
            if (version == null)
            {
                cnx.Variables.Add(new VariableDtm { Nombre = Variable.Version, Descripcion = "Versión del producto", Valor = "0.0.1" });
                cnx.SaveChanges();
            }
            else
            {
                if (version.Valor != nuevaVersion)
                {
                    version.Valor = nuevaVersion;
                    cnx.Variables.Update(version);
                    cnx.SaveChanges();
                }
            }
        }

        public static void InicializarMaestros(CtoEntorno contexto, GestorDeMenus gestorDeMenus, GestorDeVistasMvc gestorDeVistasMvc)
        {
            if (!contexto.Usuarios.Any())
                IniciarEntorno.CrearDatosIniciales(contexto);

        }


    }
}
