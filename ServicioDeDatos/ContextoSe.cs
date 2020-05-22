using Microsoft.EntityFrameworkCore;
using System;
using Utilidades.Traza;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Concurrent;
using ServicioDeDatos.Utilidades;
using Z.EntityFramework.Extensions;
using ServicioDeDatos.Elemento;
using Microsoft.EntityFrameworkCore.Design;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.Seguridad;
using ServicioDeDatos.Archivos;
using Microsoft.EntityFrameworkCore.Storage;

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
        internal class Tabla
        {
            internal static string Variable = "ENTORNO.Variable";
        }
    }
    public class DatosDeConexion
    {
        public string ServidorWeb { get; set; }
        public string ServidorBd { get; set; }
        public string Bd { get; set; }
        public string Usuario { get; set; }
        public int IdUsuario { get; set; }
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

        internal  IDbContextTransaction Transaccion { get; set; }

        public bool Debuggar => new CacheDeVariable(this).HayQueDebuggar;

        private string ObtenerVersion => new CacheDeVariable(this).Version;


        public TrazaSql Traza { get; private set; }

        private InterceptadorDeConsultas _interceptadorDeConsultas;

        public static ContextoSe ObtenerContexto()
        {
            //return ObtenerContexto(nameof(ContextoSe), () => new ConstructorDelContexto().CreateDbContext(new string[] { }));
            return new ConstructorDelContexto().CreateDbContext(new string[] { });
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

        /// <summary>
        /// No se puede cachear la conexión, ya que si no compartirían la transacción a BD, el usuario de donexión etc..
        /// </summary>
        /// <param name="options"></param>
        /// <param name="configuracion"></param>
        //protected static ContextoSe ObtenerContexto(string nombreContexto, Func<ContextoSe> crearContexto)
        //{
        //    if (!_CacheDeContextos.ContainsKey(nombreContexto))
        //    {
        //        var contexto = crearContexto();
        //        _CacheDeContextos[nombreContexto] = contexto;
        //    }

        //    return _CacheDeContextos[nombreContexto];
        //}

        public ContextoSe(DbContextOptions options, IConfiguration configuracion) :
        base(options)
        {
            Configuracion = configuracion;

            _interceptadorDeConsultas = new InterceptadorDeConsultas();
            DbInterception.Add(_interceptadorDeConsultas);

            //No se puede cachear el contexto por los datos que este compartiría (transacción, usuario, etc...)
            //if (_CacheDeContextos == null)
            //    _CacheDeContextos = new ConcurrentDictionary<string, ContextoSe>();

            InicializarDatosDeConexion();
        }

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
            DatosDeConexion.Usuario = Literal.usuario;
            DatosDeConexion.IdUsuario = 11;
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
            Traza = new TrazaSql(nivel, ruta, fichero, $"Traza iniciada por {DatosDeConexion.Usuario}");
            _interceptadorDeConsultas.Traza = Traza;
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


