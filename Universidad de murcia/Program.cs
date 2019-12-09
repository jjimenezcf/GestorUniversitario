using System;
using System.Linq;
using Gestor.Elementos.ModeloBd;
using Gestor.Elementos.Universitario.ContextosDeBd;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
namespace UniversidadDeMurcia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            CreateDbIfNotExists(host);
            host.Run();
        }

        private static void CreateDbIfNotExists(IWebHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();
            var contexto = services.GetRequiredService<ContextoUniversitario>();
            try
            {
                InicializadorBD.Inicializar(contexto);
                var resultado = contexto.CatalogoDelSe
                    .FromSqlRaw($"SELECT * FROM dbo.CatalogoDelSe WHERE tabla = '__EFMigrationsHistory'")
                    .FirstOrDefault();

                //var consulta = contexto.Se
                //    .FromSqlRaw($"select top(1) ProductVersion from dbo.__EFMigrationsHistory order by MigrationId desc")
                //    .FirstOrDefault();

                //contexto.DatosDeConexion.ServidorWeb = Environment.MachineName;
                //contexto.DatosDeConexion.ServidorBd = contexto.Database.GetDbConnection().DataSource;
                //contexto.DatosDeConexion.Bd = contexto.Database.GetDbConnection().Database;
                //contexto.DatosDeConexion.Version = "1.1.1";
                //contexto.DatosDeConexion.Usuario = "jjimenezcf@gmail.com";

                logger.LogInformation($"{Environment.NewLine}Objeto leido: {resultado}." +
                                      $"{Environment.NewLine}Id: {resultado.Id}" +
                                      $"{Environment.NewLine}BD: {resultado.Catalogo}" +
                                      $"{Environment.NewLine}Esquema: {resultado.Esquema}" +
                                      $"{Environment.NewLine}Tabla: {resultado.Tabla}");
                
                logger.LogInformation($"{Environment.NewLine}Contexto {contexto.GetType().Name} inicializado.{Environment.NewLine}BD: {contexto.Database.GetDbConnection().Database}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"{Environment.NewLine}Error al inicializar la BD.{Environment.NewLine}");
                Gestor.Errores.GestorDeErrores.EnviaError("Error al inicializar la BD.", ex);
                throw new Exception($"Error al conectarse al contexto {contexto.GetType().Name}",ex);
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
