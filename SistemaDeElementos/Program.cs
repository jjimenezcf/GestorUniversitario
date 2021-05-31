using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using GestoresDeNegocio.Negocio;
using GestoresDeNegocio.Entorno;

namespace MVCSistemaDeElementos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var servidorWeb = CreateWebHostBuilder(args).Build();
            InicializarBD(servidorWeb);
            servidorWeb.Run();
        }

        private static void InicializarBD(IWebHost sevidorWeb)
        {
            var scope = sevidorWeb.Services.CreateScope();
            var services = scope.ServiceProvider;
            InicializarNegocios(services);
            InicializarVistas(services);
        }

        private static void InicializarNegocios(IServiceProvider services)
        {
            var scope = services.CreateScope();
            using (var gestor = scope.ServiceProvider.GetRequiredService<GestorDeNegocios>())
            {
                PersistenciaDeNegocios.PersistirNegocios(gestor);
            }
        }

        private static void InicializarVistas(IServiceProvider services)
        {
            var scope = services.CreateScope();
            using (var gestor = scope.ServiceProvider.GetRequiredService<GestorDeVistaMvc>())
            {
                PersistenciaDeVistas.PersistirVista(gestor);
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>();
    }
}
