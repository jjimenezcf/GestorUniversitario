using Microsoft.EntityFrameworkCore;
using Gestor.Elementos.ModeloBd;
using System;
using System.Linq;
using GestorDeElementos.Utilidades;
using Utilidades.Traza;
using System.Data.Common;
using Z.EntityFramework.Extensions;
using System.Diagnostics;

namespace Gestor.Elementos
{

    class Literal
    {
        internal static readonly string usuario = "jjimenezcf@gmail.com";
        internal static readonly string esquemaBd = "dbo";
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
        public string Usuario { get; set; }
        public string Version { get; set; }
    }

    public class ContextoDeElementos : DbContext
    {
        public DatosDeConexion DatosDeConexion { get; set; }

        public bool Debuggar { get; set; }

        public TrazaSql Traza { get; private set; }
        private InterceptadorDeConsultas _interceptadorDeConsultas;

        public ContextoDeElementos(DbContextOptions options) :
        base(options)
        {

            _interceptadorDeConsultas = new InterceptadorDeConsultas();
            DbInterception.Add(_interceptadorDeConsultas);

            InicializarDatosContexto();
        }

        public void InicializarDatosContexto()
        {
            DatosDeConexion = new DatosDeConexion();
            DatosDeConexion.ServidorWeb = Environment.MachineName;
            DatosDeConexion.ServidorBd = Database.GetDbConnection().DataSource;
            DatosDeConexion.Bd = Database.GetDbConnection().Database;
            DatosDeConexion.Usuario = Literal.usuario;
        }
        

        public DbSet<CatalogoDelSe> CatalogoDelSe { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CatalogoDelSe>().ToView(Literal.Vista.Catalogo);
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


