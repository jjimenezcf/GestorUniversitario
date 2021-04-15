using Utilidades;
using System;
using System.Data.Common;
using Utilidades;

namespace ServicioDeDatos.Utilidades
{

    public class TrazaSql : Traza
    {
        private double duraciAcomulada = 0;
        private int sentenciasEjecutadas = 0;

        public TrazaSql(NivelDeTraza nivel, string ruta, string fichero, string mensajeInicial)
            : base(nivel, ruta, fichero)
        {
            Cronometro.Start();
            Abrir(true);
            AnotarMensaje("Inicio", mensajeInicial);
        }

        public string CerrarTraza(string mensaje)
        {
            Cronometro.Stop();
            mensaje = $"Petición finalizada" + Environment.NewLine +
                      $"Total SQLs:     {duraciAcomulada,9:0.000}" + Environment.NewLine +
                      $"Total petición: {Cronometro.ElapsedMilliseconds,9:0.000}" + Environment.NewLine +
                      Environment.NewLine +
                      Environment.NewLine +
                      $"Total Sentencias:{sentenciasEjecutadas}" + Environment.NewLine +
                      mensaje;
            Registrar(mensaje);
            Cerrar();
            return mensaje;
        }

        public void AnotarTrazaSql(string setenciaSql, DbParameterCollection dbParametros, double duracion)
        {
            if (Abierta)
            {
                var parametros = dbParametros.ParsearParametros();
                duraciAcomulada += duracion;
                string logTraza = $"Sentencia SQL:{Environment.NewLine}{setenciaSql}{Environment.NewLine}" +
                                  $"Parámetros:      {Environment.NewLine}{parametros}" +
                                  $"Duracion SQL:    {duracion,9:0.000}" + Environment.NewLine +
                                  $"Total SQLs:      {duraciAcomulada,9:0.000}" + Environment.NewLine +
                                  $"Tiempo petición: {Cronometro.ElapsedMilliseconds,9:0.000}" + Environment.NewLine;
                Registrar(logTraza);
                Separador();
                sentenciasEjecutadas++;
            }
        }
        

        public void AnotarExcepcion(Exception exc)
        {
            if (Abierta)
            {
                string logTraza = $"Excepcioón genrada: {Environment.NewLine}{exc.MensajeCompleto(true)}{Environment.NewLine}";
                logTraza = $"{logTraza}";
                Registrar(logTraza);
                Separador();
            }
        }

        public void AnotarMensaje(string asunto, string mensaje)
        {
            if (Abierta)
            {
                string logTraza = $"{asunto}{Environment.NewLine}{mensaje}{Environment.NewLine}";
                Registrar(logTraza);
                Separador();
            }
        }
    }


}
