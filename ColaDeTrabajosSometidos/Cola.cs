using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GestorDeElementos;
using GestoresDeNegocio.TrabajosSometidos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServicioDeDatos;
using ServicioDeDatos.TrabajosSometidos;

namespace ColaDeTrabajosSometidos
{
    public class Cola : BackgroundService
    {
        private readonly ILogger<Cola> _logger;
        private IServiceProvider _servicios;


        public Cola(IServiceProvider services, ILogger<Cola> logger)
        {
            _logger = logger;
            _servicios = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                var filtros = new List<ClausulaDeFiltrado>();

                var noBloqueado = new ClausulaDeFiltrado();
                noBloqueado.Clausula = nameof(TrabajoDeUsuarioDtm.Estado);
                noBloqueado.Criterio = ModeloDeDto.CriteriosDeFiltrado.diferente;
                noBloqueado.Valor = enumEstadosDeUnTrabajo.Bloqueado.ToDtm();
                filtros.Add(noBloqueado);

                var listo = new ClausulaDeFiltrado();
                listo.Clausula = nameof(TrabajoDeUsuarioDtm.Estado);
                listo.Criterio = ModeloDeDto.CriteriosDeFiltrado.igual;
                listo.Valor = enumEstadosDeUnTrabajo.Pendiente.ToDtm();
                filtros.Add(listo);

                var orden = new List<ClausulaDeOrdenacion>();
                var fechaPlanificacion = new ClausulaDeOrdenacion();
                fechaPlanificacion.Modo = ModoDeOrdenancion.ascendente;
                fechaPlanificacion.OrdenarPor = nameof(TrabajoDeUsuarioDtm.Planificado);

                var scope = _servicios.CreateScope();
                var gestor = scope.ServiceProvider.GetRequiredService<GestorDeTrabajosDeUsuario>();

                var trabajosPorEjecutar = gestor.LeerRegistros(0, -1, filtros, orden);

                _logger.LogInformation("Trabajos pendientes {pedientes}", trabajosPorEjecutar.Count());
                if (trabajosPorEjecutar.Count()>0)
                {
                    _logger.LogInformation("Ejecutando el trabajo {trabajo} del usuario {usuario}", trabajosPorEjecutar[0].Trabajo.Nombre, trabajosPorEjecutar[0].Ejecutor.Login);
                }

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
