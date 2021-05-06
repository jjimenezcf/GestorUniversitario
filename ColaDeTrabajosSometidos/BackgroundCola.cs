using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Gestor.Errores;
using GestorDeElementos;
using GestoresDeNegocio.Entorno;
using GestoresDeNegocio.TrabajosSometidos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServicioDeDatos;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.TrabajosSometidos;

namespace ColaDeTrabajosSometidos
{
    internal class Literal
    {
        internal static readonly string EntornoDeEjecucion = nameof(BackgroundCola);
        internal static readonly string ColaActiva = nameof(ColaActiva);
        internal static readonly string EmisorDeCorreos = nameof(EmisorDeCorreos);
        internal static readonly string ReceptorDeCorreos = nameof(ReceptorDeCorreos);
        internal static readonly string EjecutorDeLaCola = nameof(EjecutorDeLaCola);
        internal static readonly string TrazarConsultas = nameof(TrazarConsultas);
        internal static readonly string CadenaDeConexion = nameof(CadenaDeConexion);
    }

    public class BackgroundCola : BackgroundService
    {
        private readonly ILogger<BackgroundCola> _logger;
        private IServiceProvider _servicios;

        public IConfiguration Configuracion { get; }


        public UsuarioDtm Usuario { get; private set; }

        public BackgroundCola(IServiceProvider services, ILogger<BackgroundCola> logger, IConfiguration configuracion)
        {
            _logger = logger;
            _servicios = services;
            Configuracion = configuracion;
            ObtenerUsuarioEjecutor();
        }

        public void ObtenerUsuarioEjecutor()
        {
            var scope = _servicios.CreateScope();
            using (var gestor = scope.ServiceProvider.GetRequiredService<GestorDeUsuarios>())
            {
                Usuario = gestor.LeerRegistroCacheado(nameof(UsuarioDtm.Login), CacheDeVariable.Cola_Ejecutor);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            if (!CacheDeVariable.Cola_Activa)
                return;

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
                using (var gestor = scope.ServiceProvider.GetRequiredService<GestorDeTrabajosDeUsuario>())
                {
                    try
                    {
                        CumplimentarDatosDeConexion(gestor);

                        if (CacheDeVariable.Cola_Trazar)
                            gestor.Contexto.IniciarTraza(nameof(BackgroundCola));


                        var trabajosPorEjecutar = gestor.LeerRegistros(0, -1, filtros, orden);

                        _logger.LogInformation("Trabajos pendientes {pedientes}", trabajosPorEjecutar.Count());
                        if (trabajosPorEjecutar.Count() > 0)
                        {
                            var entorno = new EntornoDeTrabajo(gestor, trabajosPorEjecutar[0]);
                            _logger.LogInformation("Ejecutando el trabajo {trabajo} del usuario {usuario}", trabajosPorEjecutar[0].Trabajo.Nombre, trabajosPorEjecutar[0].Ejecutor.Login);
                        }


                        GestorDeCorreos.EnviarCorreoDe(gestor.Contexto, CacheDeVariable.Cola_Emisor, new List<string> { CacheDeVariable.Cola_Receptor }, "Cola ejecutada", $"Se ha ejecutado la cola y había pendientes {trabajosPorEjecutar.Count()} trabajos", null, null);
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

                        await Task.Delay(10000, stoppingToken);
                    }
                }

            }
        }

        private void CumplimentarDatosDeConexion(GestorDeTrabajosDeUsuario gestor)
        {
            gestor.Contexto.DatosDeConexion.IdUsuario = Usuario.Id;
            gestor.Contexto.DatosDeConexion.EsAdministrador = Usuario.EsAdministrador;
            gestor.Contexto.DatosDeConexion.Login = Usuario.Login;

        }
    }
}
