﻿using System;
using GestorUniversitario.ContextosDeBd;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
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
            try
            {
                var contexto = services.GetRequiredService<ContextoUniversitario>();
                InicializadorBD.Inicializar(contexto);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Error al inicializar la BD.");
                Gestor.Errores.Errores.EnviaError("Error al inicializar la BD.", ex);
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
