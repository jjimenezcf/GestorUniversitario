using Microsoft.EntityFrameworkCore;
using Gestor.Elementos.ModeloBd;
using System;
using System.Linq;
using GestorDeElementos.Utilidades;
using Extensiones;
using System.Data.Common;
using Z.EntityFramework.Extensions;

namespace Gestor.Elementos
{
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

        public TrazaSql Traza { get; private set; }
        private InterceptadorDeConsultas _interceptadorDeConsultas;

        public ContextoDeElementos(DbContextOptions options) :
        base(options)
        {

            _interceptadorDeConsultas = new InterceptadorDeConsultas();
            DbInterception.Add(_interceptadorDeConsultas);

            //dbContextOptionsBuilder.AddInterceptors(new LogSql());

            DatosDeConexion = new DatosDeConexion();
            DatosDeConexion.ServidorWeb = Environment.MachineName;
            DatosDeConexion.ServidorBd = Database.GetDbConnection().DataSource;
            DatosDeConexion.Bd = Database.GetDbConnection().Database;
            DatosDeConexion.Version = new ExisteTabla(this, Literal.Tabla.Variable).Existe ?
             ObtenerVersion() :
             "0.0.0.";
            DatosDeConexion.Usuario = Literal.usuario;
        }

        private string ObtenerVersion()
        {
            var registro = Variables.SingleOrDefault(v => v.Nombre == Literal.version);
            return registro == null ? "0.0.0" : registro.Valor;
        }

        public DbSet<CatalogoDelSe> CatalogoDelSe { get; set; }
        public DbSet<RegistroDeVariable> Variables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CatalogoDelSe>().ToView(Literal.Vista.Catalogo);
            modelBuilder.Entity<RegistroDeVariable>().ToTable(Literal.Tabla.Variable);
        }

        public void IniciarTraza()
        {
            if (Traza == null)
                CrearTraza(NivelDeTraza.Siempre, @"c:\Temp\Trazas", $"traza_{DateTime.Now}.txt");
            else
            if (!Traza.EstaAbierta)
                Traza.Abrir(true);
        }

        public void CerrarTraza()
        {
            if (Traza != null)
            {
                if (!Traza.EstaAbierta)
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

        public InterceptadorDeConsultas()
        {
        }

        public override void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            base.NonQueryExecuted(command, interceptionContext);
            RegistrarTraza("InterceptadorDeConsultas.NonQueryExecuted", interceptionContext.Result.ToString(), command.CommandText);
        }

        public override void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            base.NonQueryExecuting(command, interceptionContext);
            RegistrarTraza("InterceptadorDeConsultas.NonQueryExecuting", interceptionContext.EventData.ToString(), command.CommandText);
        }

        public override void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            base.ReaderExecuted(command, interceptionContext);
            RegistrarTraza("InterceptadorDeConsultas.ReaderExecuted", interceptionContext.Result.ToString(), command.CommandText);
        }

        public override void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            base.ReaderExecuting(command, interceptionContext);
            RegistrarTraza("InterceptadorDeConsultas.ReaderExecuting", interceptionContext.EventData.ToString(), command.CommandText);
        }

        public override void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            base.ScalarExecuted(command, interceptionContext);
            RegistrarTraza("InterceptadorDeConsultas.ScalarExecuted", interceptionContext.Result.ToString(), command.CommandText);
        }

        public override void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            base.ScalarExecuting(command, interceptionContext);
            RegistrarTraza("InterceptadorDeConsultas.ScalarExecuting", interceptionContext.EventData.ToString(), command.CommandText);
        }

        public override void NonQueryError(DbCommand command, DbCommandInterceptionContext<int> interceptionContext, Exception exception)
        {
            base.NonQueryError(command, interceptionContext, exception);
            RegistrarError("InterceptadorDeConsultas.NonQueryError", interceptionContext.EventData.ToString(), command.CommandText, exception.Message);
        }

        public override void ReaderError(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext, Exception exception)
        {
            base.ReaderError(command, interceptionContext, exception);
            RegistrarError("InterceptadorDeConsultas.NonQueryError", interceptionContext.EventData.ToString(), command.CommandText, exception.Message);
        }

        public override void ScalarError(DbCommand command, DbCommandInterceptionContext<object> interceptionContext, Exception exception)
        {
            base.ScalarError(command, interceptionContext, exception);
            RegistrarError("InterceptadorDeConsultas.NonQueryError", interceptionContext.EventData.ToString(), command.CommandText, exception.Message);
        }

        private void RegistrarTraza(string method, string command, string commandText)
        {
            if (Traza != null)
              Traza.AnotarTrazaSql($"Intercepted on: {method} \n {command} \n {commandText}");
        }

        private void RegistrarError(string method, string command, string commandText, string exception)
        {
            Console.WriteLine("Intercepted on: {0} \n {1} \n {2} \n {3}", method, command, commandText, exception);
        }
    }
}


