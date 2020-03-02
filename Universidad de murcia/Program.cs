using System;
using System.Linq;
using Gestor.Elementos;
using Gestor.Elementos.Entorno;
using Gestor.Elementos.ModeloBd;
using Gestor.Elementos.Universitario;
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
            var cnxSe = services.GetRequiredService<ContextoEntorno>();
            try
            {
                cnxSe.IniciarTraza();
                ContextoEntorno.NuevaVersion(cnxSe);
            }
            catch (Exception ex)
            {
                Gestor.Errores.GestorDeErrores.EnviaError("Error al inicializar la BD.", ex);
                throw new Exception($"Error al conectarse al contexto {cnxSe.GetType().Name}", ex);
            }
            finally
            {
                if (cnxSe != null)
                    cnxSe.CerrarTraza();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>();
    }
}
