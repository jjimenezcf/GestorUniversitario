using Extensiones.String;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Extensiones
{

    public enum NivelDeTraza
    {
        Debug,
        Info,
        Advertencia,
        Error,
        Siempre,
        Off
    }
    public class NivelLog
    {
        public static NivelDeTraza Nivel(string nivel)
        {
            if (nivel.ToUpper() == NivelDeTraza.Debug.ToString().ToUpper())
                return NivelDeTraza.Debug;

            if (nivel.ToUpper() == NivelDeTraza.Error.ToString().ToUpper())
                return NivelDeTraza.Error;

            if (nivel.ToUpper() == NivelDeTraza.Info.ToString().ToUpper())
                return NivelDeTraza.Info;

            if (nivel.ToUpper() == NivelDeTraza.Advertencia.ToString().ToUpper())
                return NivelDeTraza.Advertencia;

            if (nivel.ToUpper() == NivelDeTraza.Siempre.ToString().ToUpper())
                return NivelDeTraza.Siempre;

            return NivelDeTraza.Off;
        }
    }

    public class Traza
    {

        private StreamWriter _sw;
        private readonly string _fichero;
        private readonly string _ruta;
        private readonly NivelDeTraza _nivel;
        private bool abierto { get; set; }
        private bool escribirNivel => _nivel != NivelDeTraza.Siempre;
        public bool LaTrazaEstaAbierta => abierto;
        public Stopwatch Cronometro = new Stopwatch();


        /// <summary>
        /// Constructor sin parametros.
        /// </summary>
        public Traza(NivelDeTraza nivel, string ruta, string fichero)
        {
            _fichero = fichero.RemplazarCaracteres("_");
            _ruta = ruta;
            _nivel = nivel;
        }


        /// <summary>
        /// Constructor sin parametros.
        /// </summary>
        public void Abrir(bool anadir)
        {

            if (_nivel == NivelDeTraza.Off)
                return;

            if (abierto)
                return;

            if (!Directory.Exists(_ruta))
            {
                Directory.CreateDirectory(_ruta);
            }

            var fichero = Path.Combine(_ruta, _fichero);
            var i = 1;
            while (File.Exists(fichero))
            {
                fichero = Path.Combine(_ruta, $"{Path.GetFileNameWithoutExtension(fichero)}_{i}{Path.GetExtension(fichero)}");
                i++;
            }

            try
            {
                _sw = new StreamWriter(fichero, anadir);
                abierto = true;
            }
            catch
            {
                abierto = false;
            }
        }

        public void Cerrar()
        {
            if (!abierto)
                return;
            try
            {
                _sw.Close();
            }
            finally
            {
                abierto = false;
            }
        }

        private void Escribir(NivelDeTraza tipoNivel, string mensaje, bool registrarHora = true)
        {
            var log = $"{ (registrarHora ? $"{DateTime.Now} -" : "")}{(escribirNivel ? $" {tipoNivel.ToString()} : " : " ")}{mensaje}";

            Escribir(log);
        }

        private void Escribir(string log)
        {
            if (abierto)
                _sw.WriteLine(log);
        }

        public void Log(NivelDeTraza tipoNivel, string mensaje)
        {
            if (!abierto)
                return;

            if (tipoNivel == NivelDeTraza.Siempre)
                Escribir(tipoNivel, mensaje);
            else
            {
                if (_nivel != NivelDeTraza.Off)
                    return;

                if (tipoNivel == NivelDeTraza.Debug)
                    Escribir(tipoNivel, mensaje);
                else
                if (tipoNivel == NivelDeTraza.Info && (_nivel == NivelDeTraza.Info || _nivel == NivelDeTraza.Advertencia || _nivel == NivelDeTraza.Error))
                    Escribir(tipoNivel, mensaje);
                else
                if (tipoNivel == NivelDeTraza.Advertencia && (_nivel == NivelDeTraza.Advertencia || _nivel == NivelDeTraza.Error))
                    Escribir(tipoNivel, mensaje);
                else
                if (tipoNivel == NivelDeTraza.Error && _nivel == NivelDeTraza.Error)
                    Escribir(tipoNivel, mensaje);
            }

        }

        public void Debug(object mensaje)
        {
            if (mensaje != null) Log(NivelDeTraza.Debug, mensaje.ToString());
        }

        public void Info(object mensaje)
        {
            if (mensaje != null) Log(NivelDeTraza.Info, mensaje.ToString());
        }
        public void Error(object mensaje)
        {
            if (mensaje != null) Log(NivelDeTraza.Error, mensaje.ToString());
        }
        public void Advertencia(object mensaje)
        {
            if (mensaje != null) Log(NivelDeTraza.Advertencia, mensaje.ToString());
        }
        public void Registrar(object mensaje)
        {
            if (mensaje != null) Log(NivelDeTraza.Siempre, mensaje.ToString());
        }

        public void Separador()
        {
            Escribir("---------------------------------------" + Environment.NewLine);
        }
    }
}
