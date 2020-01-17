using Extensiones;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestorDeElementos.Utilidades
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
                      $"Total SQLs:     {duraciAcomulada.ToString("0.000").PadLeft(9, ' ')}" + Environment.NewLine +
                      $"Total petición: {Cronometro.ElapsedMilliseconds.ToString("0.000").PadLeft(9, ' ')}" + Environment.NewLine +
                      Environment.NewLine +
                      Environment.NewLine +
                      $"Total Sentencias:{sentenciasEjecutadas}" + Environment.NewLine +
                      mensaje;
            Registrar(mensaje);
            Cerrar();
            return mensaje;
        }

        public void AnotarTrazaSql(string setenciaSql, string parametros, double duracion)
        {
            if (EstaAbierta)
            {
                duraciAcomulada += duracion;
                string logTraza = $"Sentencia SQL:{Environment.NewLine}{setenciaSql}{Environment.NewLine}" +
                                  $"Parámetros:      {parametros}." + Environment.NewLine +
                                  $"Duracion SQL:    {duracion.ToString("0.000").PadLeft(9, ' ')}" + Environment.NewLine +
                                  $"Total SQLs:      {duraciAcomulada.ToString("0.000").PadLeft(9, ' ')}" + Environment.NewLine +
                                  $"Tiempo petición: {Cronometro.ElapsedMilliseconds.ToString("0.000").PadLeft(9, ' ')}" + Environment.NewLine;
                Registrar(logTraza);
                Separador();
                sentenciasEjecutadas++;
            }
        }

        public void AnotarTrazaSql(string traza)
        {
            if (EstaAbierta)
            {
                Registrar(traza);
                Separador();
                sentenciasEjecutadas++;
            }
        }

        public void AnotarTrazaSql(string setenciaSql, string parametros)
        {
            if (EstaAbierta)
            {
                string logTraza = $"Sentencia SQL:{Environment.NewLine}{setenciaSql}{Environment.NewLine}" +
                                  $"Parámetros:   {parametros}." + Environment.NewLine;
                Registrar(logTraza);
                Separador();
                sentenciasEjecutadas++;
            }
        }

        public void AnotarExcepcion(Exception exc)
        {
            if (EstaAbierta)
            {
                string logTraza = $"Excepcioón genrada: {Environment.NewLine}{exc.MensajeCompleto(true)}{Environment.NewLine}";
                logTraza = $"{logTraza}";
                Registrar(logTraza);
                Separador();
            }
        }

        public void AnotarMensaje(string asunto, string mensaje)
        {
            if (EstaAbierta)
            {
                string logTraza = $"{asunto}{Environment.NewLine}{mensaje}{Environment.NewLine}";
                Registrar(logTraza);
                Separador();
            }
        }
    }


}
