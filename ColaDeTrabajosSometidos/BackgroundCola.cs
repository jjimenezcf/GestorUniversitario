using System;
using System.Threading;
using System.Threading.Tasks;
using GestoresDeNegocio.Entorno;
using GestoresDeNegocio.TrabajosSometidos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServicioDeDatos;
using ServicioDeDatos.Entorno;

namespace ColaDeTrabajosSometidos
{
    public class BackgroundCola : BackgroundService
    {
        private IServiceProvider _servicios;

        public UsuarioDtm Usuario { get; private set; }

        public BackgroundCola(IServiceProvider services)
        {
            _servicios = services;
            ObtenerUsuarioEjecutor();
        }

        public void ObtenerUsuarioEjecutor()
        {
            var scope = _servicios.CreateScope();
            using (var gestor = scope.ServiceProvider.GetRequiredService<GestorDeUsuarios>())
            {
                Usuario = gestor.LeerRegistroCacheado(nameof(UsuarioDtm.Login), CacheDeVariable.Cola_LoginDeEjecutor);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (CacheDeVariable.Cola_Activa)
                {
                    var scope = _servicios.CreateScope();
                    using (var gestor = scope.ServiceProvider.GetRequiredService<GestorDeTrabajosDeUsuario>())
                    {
                        if (CacheDeVariable.Cola_Trazar)
                            gestor.Contexto.IniciarTraza(nameof(BackgroundCola));

                        try
                        {
                            await gestor.ProcesarCola(Usuario);
                        }
                        catch (Exception e)
                        {
                            if (CacheDeVariable.Cola_Trazar)
                                gestor.Contexto.AnotarExcepcion(e);
                        }
                        finally
                        {
                            if (CacheDeVariable.Cola_Trazar)
                                gestor.Contexto.CerrarTraza();
                        }
                    }
                }
                await Task.Delay(10000, stoppingToken);
            }
        }


    }

}
