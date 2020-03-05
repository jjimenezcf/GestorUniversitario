using System;
using Gestor.Elementos.Entorno;
using Gestor.Elementos.Usuario;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace MVCSistemaDeElementos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            CrearBdSiNoExiste(host);
            host.Run();
        }

        private static void CrearBdSiNoExiste(IWebHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            IniciarContextoDeEntorno(services);
            IniciarContextoUniversitario(services);
        }

        private static void IniciarContextoUniversitario(IServiceProvider services)
        {
            var cnxUniv = services.GetRequiredService<ContextoUniversitario>();
            try
            {
                cnxUniv.Database.Migrate();
                cnxUniv.IniciarTraza();
                ContextoUniversitario.InicializarMaestros(cnxUniv);
            }
            catch (Exception ex)
            {
                Gestor.Errores.GestorDeErrores.EnviaError("Error al inicializar la BD.", ex);
                throw new Exception($"Error al conectarse al contexto {cnxUniv.GetType().Name}", ex);
            }
            finally
            {
                if (cnxUniv != null)
                    cnxUniv.CerrarTraza();
            }
        }

        private static void IniciarContextoDeEntorno(IServiceProvider services)
        {
            var cnxEntorno = services.GetRequiredService<ContextoEntorno>();
            try
            {
                cnxEntorno.Database.Migrate();
                cnxEntorno.IniciarTraza();
                ContextoEntorno.NuevaVersion(cnxEntorno);
            }
            catch (Exception ex)
            {
                Gestor.Errores.GestorDeErrores.EnviaError("Error al inicializar la BD.", ex);
                throw new Exception($"Error al conectarse al contexto {cnxEntorno.GetType().Name}", ex);
            }
            finally
            {
                if (cnxEntorno != null)
                    cnxEntorno.CerrarTraza();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>();
    }
}
