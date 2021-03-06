﻿using System;
using System.Diagnostics;
using System.IO;

namespace Utilidades
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
        private string _fichero;
        private string _ruta;
        private NivelDeTraza _nivel;
        private bool _Abierta { get; set; }
        private bool escribirNivel => _nivel != NivelDeTraza.Siempre;

        protected Stopwatch Cronometro = new Stopwatch();
        public bool Abierta => _Abierta;


        /// <summary>
        /// Constructor sin parametros.
        /// </summary>
        public Traza(NivelDeTraza nivel, string ruta, string fichero)
        {
            InicializarTraza(nivel, ruta, fichero);
        }


        public void NuevaTraza(string fichero)
        {

            if (_nivel == NivelDeTraza.Off)
                return;

            if (_Abierta && _fichero != fichero.RemplazarCaracteres("_"))
            {
                Cerrar();
                InicializarTraza(_nivel, _ruta, fichero);
                Abrir(true);
            }


        }

        private void InicializarTraza(NivelDeTraza nivel, string ruta, string fichero)
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

            if (_Abierta)
                return;

            if (!Directory.Exists(_ruta))
            {
                Directory.CreateDirectory(_ruta);
            }

            var nombre = $"{Path.GetFileNameWithoutExtension(_fichero)}";
            var extension = Path.GetExtension(_fichero);
            var rutaFichero = Path.Combine(_ruta, $"{nombre}_{0.ToString().PadLeft(3, '0')}{extension}");
            var i = 0;
            while (File.Exists(rutaFichero))
            {
                i++;
                rutaFichero = Path.Combine(_ruta, $"{nombre}_{i.ToString().PadLeft(3,'0')}{extension}");
            }

            try
            {
                _sw = new StreamWriter(rutaFichero, anadir);
                _Abierta = true;
            }
            catch
            {
                _Abierta = false;
            }
        }

        public void Cerrar()
        {
            if (!_Abierta)
                return;
            try
            {
                _sw.Close();
            }
            finally
            {
                _Abierta = false;
            }
        }

        private void Escribir(NivelDeTraza tipoNivel, string mensaje, bool registrarHora = true)
        {
            var log = $"{ (registrarHora ? $"{DateTime.Now} -" : "")}{(escribirNivel ? $" {tipoNivel.ToString()} : " : " ")}{mensaje}";

            Escribir(log);
        }

        private void Escribir(string log)
        {
            if (_Abierta)
                try
                {
                    _sw.WriteLine(log);
                }
                catch 
                { }
        }

        private void Log(NivelDeTraza tipoNivel, string mensaje)
        {
            if (!_Abierta)
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

        private void Debug(string mensaje)
        {
            if (!mensaje.IsNullOrEmpty())
                Log(NivelDeTraza.Debug, mensaje.ToString());
        }

        private void Info(string mensaje)
        {
            if (!mensaje.IsNullOrEmpty())
                Log(NivelDeTraza.Info, mensaje.ToString());
        }
        private void Error(string mensaje)
        {
            if (!mensaje.IsNullOrEmpty())
                Log(NivelDeTraza.Error, mensaje.ToString());
        }
        private void Advertencia(string mensaje)
        {
            if (!mensaje.IsNullOrEmpty())
                Log(NivelDeTraza.Advertencia, mensaje.ToString());
        }
        protected void Registrar(string mensaje)
        {
            if (!mensaje.IsNullOrEmpty()) 
                Log(NivelDeTraza.Siempre, mensaje.ToString());
        }

        protected void Separador()
        {
            Escribir("---------------------------------------" + Environment.NewLine);
        }
    }
}
