using System;
using System.Linq;
using GestorDeElementos.BdModelo;
using GestorUniversitario.ContextosDeBd;
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
                var resultado = contexto.CatalogoDeBd
                    .FromSqlRaw($"SELECT convert(int, ROW_NUMBER() OVER(ORDER BY Table_Name ASC)) as Id, TABLE_CATALOG as Catalogo, TABLE_SCHEMA as Esquema, TABLE_NAME as Tabla FROM information_schema.tables WHERE  table_name = '__EFMigrationsHistory'")
                    .FirstOrDefault();
                
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
                Gestor.Errores.Errores.EnviaError("Error al inicializar la BD.", ex);
                throw new Exception($"Error al conectarse al contexto {contexto.GetType().Name}",ex);
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
