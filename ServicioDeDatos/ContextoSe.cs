using Microsoft.EntityFrameworkCore;
using System;
using Utilidades;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.IO;
using ServicioDeDatos.Utilidades;
using Z.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using AutoMapper;

namespace ServicioDeDatos
{
    public class Literal
    {
        internal static readonly string usuario = "jjimenezcf@gmail.com";
        internal static readonly string Version_0 = "0.0.0";
        public static readonly string CadenaDeConexion = nameof(CadenaDeConexion);

        public class Vista
        {
            internal static string Catalogo = "CatalogoDelSe";
        }
    }
    public class DatosDeConexion
    {
        public string ServidorWeb { get; set; }
        public string ServidorBd { get; set; }
        public string Bd { get; set; }
        public string Login { get; set; }
        public int IdUsuario { get; set; }
        public bool EsAdministrador { get; set; }
        public string Version { get; set; }
        public string Menu { get; set; }
    }

    public class ConstructorDelContexto : IDesignTimeDbContextFactory<ContextoSe>
    {
        public ContextoSe CreateDbContext(string[] arg)
        {

            var datosDeConexion = ContextoSe.ObtenerDatosDeConexion();

            var opciones = new DbContextOptionsBuilder<ContextoSe>();
            opciones.UseSqlServer(datosDeConexion.CadenaConexion);
            object[] parametros = { opciones.Options, datosDeConexion.Configuracion };

            return (ContextoSe)Activator.CreateInstance(typeof(ContextoSe), parametros);
        }
    }

    public static class Transaccion
    {
        
        public static bool IniciarTransaccion(this ContextoSe contexto)
        {
            if (contexto.Database.CurrentTransaction == null)
            {
                contexto.Transaccion = contexto.Database.BeginTransaction();
                return true;
            }
            return false;
        }

        public static void Commit(this ContextoSe contexto, bool transaccion)
        {
            if (transaccion)
            {
                contexto.Transaccion.Commit();
                contexto.Transaccion.Dispose();
            }
        }
        public static void Rollback(this ContextoSe contexto, bool transaccion)
        {
            if (transaccion)
            {
                contexto.Transaccion.Rollback();
                contexto.Transaccion.Dispose();
            }
        }
    }

    public partial class ContextoSe : DbContext
    {

        //private static ConcurrentDictionary<string, ContextoSe> _CacheDeContextos { get; set; }
        public DatosDeConexion DatosDeConexion { get; private set; }
        public IConfiguration Configuracion { get; private set; }

        public DbContextOptions OpcionesDelContexto { get; private set; }

        public IMapper Mapeador { get; set; }

        internal  IDbContextTransaction Transaccion { get; set; }

        public bool HayTransaccion => Transaccion != null;

        public bool Debuggar => CacheDeVariable.HayQueDebuggar;

        private string ObtenerVersion => CacheDeVariable.Version;


        public TrazaSql Traza { get; private set; }

        private InterceptadorDeConsultas Interceptor;

        public static ContextoSe ObtenerContexto(ContextoSe contexto)
        {
           // return ObtenerContexto(nameof(ContextoSe), () => new ConstructorDelContexto().CreateDbContext(new string[] { }));
            //return new ConstructorDelContexto().CreateDbContext(new string[] { });
            var opciones = new DbContextOptionsBuilder<ContextoSe>();
            var datosDeConexion = ObtenerDatosDeConexion();
            opciones.UseSqlServer(datosDeConexion.CadenaConexion);
            var c = new ContextoSe(opciones.Options, datosDeConexion.Configuracion);
            c.Mapeador = contexto.Mapeador;
            c.DatosDeConexion = contexto.DatosDeConexion;
            c.Traza = contexto.Traza;
            c.Interceptor = contexto.Interceptor;
            return c;
        }

        public static (IConfigurationRoot Configuracion, string CadenaConexion) ObtenerDatosDeConexion()
        {
            var generador = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json");

            var configuracion = generador.Build();
            var cadenaDeConexion = configuracion.GetConnectionString(Literal.CadenaDeConexion);
            return (configuracion, cadenaDeConexion);
        }

        public ContextoSe(DbContextOptions opcionesDelContexto, IConfiguration configuracion) :
        base(opcionesDelContexto)
        {
            Configuracion = configuracion;
            OpcionesDelContexto = opcionesDelContexto;

            Interceptor = new InterceptadorDeConsultas();
            DbInterception.Add(Interceptor);

            InicializarDatosDeConexion();
        }

        /// <summary>
        /// NECESARIO PARA EJECUTAR LAS MIGRACIONES EN UN PROYECTO APARTE
        /// </summary>
        /// <param name="options"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var conexion = Configuracion.GetConnectionString(Literal.CadenaDeConexion);
            options.UseSqlServer(conexion, x => x.MigrationsHistoryTable("__Migraciones", "ENTORNO"))
                   .UseSqlServer(conexion, x => x.MigrationsAssembly("Migraciones"));
        }

        public void InicializarDatosDeConexion()
        {
            DatosDeConexion = new DatosDeConexion();
            DatosDeConexion.ServidorWeb = Environment.MachineName;
            DatosDeConexion.ServidorBd = Database.GetDbConnection().DataSource;
            DatosDeConexion.Bd = Database.GetDbConnection().Database;
            DatosDeConexion.Login = null;
            DatosDeConexion.IdUsuario = 0;
            DatosDeConexion.Version = ObtenerVersion;
        }

        public void IniciarTraza()
        {
            if (!Debuggar)
                return;

            if (Traza == null)
                CrearTraza(NivelDeTraza.Siempre, @"c:\Temp\Trazas", $"traza_{DateTime.Now}.txt");
            else
            if (!Traza.Abierta)
                Traza.Abrir(true);
        }

        public void CerrarTraza()
        {
            if (Traza != null)
            {
                if (!Traza.Abierta)
                    Traza.Abrir(true);

                Traza.CerrarTraza("Conexión cerrada");
            }
        }

        private void CrearTraza(NivelDeTraza nivel, string ruta, string fichero)
        {
            Traza = new TrazaSql(nivel, ruta, fichero, $"Traza iniciada por {DatosDeConexion.Login}");
            Interceptor.Traza = Traza;
        }


    }
    public class InterceptadorDeConsultas : DbCommandInterceptor
    {
        public TrazaSql Traza { get; set; }
        private DbCommand _sentenciaSql { get; set; }
        private Stopwatch _cronoSql;


        public override void NonQueryExecuting(DbCommand sentenciaSql, DbCommandInterceptionContext<int> interceptionContext)
        {
            base.NonQueryExecuting(sentenciaSql, interceptionContext);
            _sentenciaSql = sentenciaSql;
            _cronoSql = new Stopwatch();
            _cronoSql.Start();
        }
        public override void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            base.NonQueryExecuted(command, interceptionContext);
            RegistrarTraza();
        }
        public override void NonQueryError(DbCommand sentenciaSql, DbCommandInterceptionContext<int> interceptionContext, Exception exception)
        {
            base.NonQueryError(sentenciaSql, interceptionContext, exception);
            RegistrarError(exception);
        }


        public override void ReaderExecuting(DbCommand sentenciaSql, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            base.ReaderExecuting(sentenciaSql, interceptionContext);
            _sentenciaSql = sentenciaSql;
            _cronoSql = new Stopwatch();
            _cronoSql.Start();
        }
        public override void ReaderExecuted(DbCommand sentenciaSql, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            base.ReaderExecuted(sentenciaSql, interceptionContext);
            RegistrarTraza();
        }
        public override void ReaderError(DbCommand sentenciaSql, DbCommandInterceptionContext<DbDataReader> interceptionContext, Exception exception)
        {
            base.ReaderError(sentenciaSql, interceptionContext, exception);
            RegistrarError(exception);
        }



        public override void ScalarExecuting(DbCommand sentenciaSql, DbCommandInterceptionContext<object> interceptionContext)
        {
            base.ScalarExecuting(sentenciaSql, interceptionContext);
            RegistrarTraza();
        }
        public override void ScalarExecuted(DbCommand sentenciaSql, DbCommandInterceptionContext<object> interceptionContext)
        {
            base.ScalarExecuted(sentenciaSql, interceptionContext);
            RegistrarTraza();
        }
        public override void ScalarError(DbCommand sentenciaSql, DbCommandInterceptionContext<object> interceptionContext, Exception exception)
        {
            base.ScalarError(sentenciaSql, interceptionContext, exception);
            RegistrarError(exception);
        }

        private void RegistrarTraza()
        {
            if (_cronoSql != null)
            {
                _cronoSql.Stop();

                if (Traza != null)
                    Traza.AnotarTrazaSql(_sentenciaSql.CommandText, _sentenciaSql.Parameters, _cronoSql.ElapsedMilliseconds);
            }
        }

        private void RegistrarError(Exception excepcion)
        {
            if (_cronoSql != null)
            {
                _cronoSql.Stop();

                if (Traza != null)
                    Traza.AnotarExcepcion(excepcion);
            }
        }
    }

}


