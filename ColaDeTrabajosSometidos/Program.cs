using System;
using GestorDeElementos;
using GestoresDeNegocio.TrabajosSometidos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServicioDeDatos;



namespace ColaDeTrabajosSometidos
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args);
            var hostConstruido = host.Build();
            hostConstruido.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<ContextoSe>(options => options.UseSqlServer("Server=DESARROLLO2;Database=SistemaDeElementos;uid=admin;Password=kadmon;MultipleActiveResultSets=true"));
                    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                    services.AddScoped<GestorDeTrabajosSometido>();
                    services.AddHostedService<Cola>();
                });
    }
}
