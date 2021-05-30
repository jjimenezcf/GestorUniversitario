using System;
using ServicioDeDatos;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GestoresDeNegocio.Negocio;
using ServicioDeDatos.Negocio;
using ModeloDeDto.Negocio;
using GestorDeElementos;
using ModeloDeDto.Callejero;
using ServicioDeDatos.Callejero;

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
        }

        private static void InicializarNegocios(IServiceProvider services)
        {
            var scope = services.CreateScope();
            using (var gestor = scope.ServiceProvider.GetRequiredService<GestorDeNegocios>())
            {
                gestor.Contexto.IniciarTraza(nameof(InicializarNegocios));
                try
                {
                    gestor.Contexto.DatosDeConexion.CreandoModelo = true;
                    GestorDeNegocios.CrearNegocioSiNoExiste(gestor.Contexto, enumNegocio.Municipio, typeof(MunicipioDtm), typeof(MunicipioDto), "");
                }
                finally
                {
                    gestor.Contexto.DatosDeConexion.CreandoModelo = false;
                }
            }

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>();
    }
}
