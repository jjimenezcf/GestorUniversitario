using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Utilidades
{
    public static class Cadenas
    {
        public static bool IsNullOrEmpty(this string str, bool quitarBlancos = true)
        {
            if (str == null)
                return true;

            return string.IsNullOrEmpty(quitarBlancos ? str.Trim() : str);
        }

        public static bool Evaluar(this string str, string cadena)
        {
            if (str.IsNullOrEmpty() || cadena.IsNullOrEmpty())
                return false;

            if (cadena.StartsWith("=") && cadena.Length > 1)
            {
                cadena = cadena.Substring(0, cadena.Length - 1);
                if (str.Length < cadena.Length)
                    return false;

                return str.Equals(cadena);
            }

            if (cadena.StartsWith("|") && cadena.Length>1)
            {
                cadena = cadena.Substring(1);
                if (str.Length < cadena.Length)
                    return false;

                return str.StartsWith(cadena);
            }

            if (cadena.EndsWith("|") && cadena.Length > 1)
            {
                cadena = cadena.Substring(0, cadena.Length -1);
                if (str.Length < cadena.Length)
                    return false;

                return str.EndsWith(cadena);
            }

            return str.Contains(cadena);
        }

        public static List<int> ListaEnteros(this string str, string separador=";",  bool quitarCeros = true)
        {
            var l = new List<int>();
            if (str.IsNullOrEmpty())
                return l;

            var numeros = str.Split(separador);
            foreach(string n in numeros)
            {
                var i = n.Entero();
                if (i == 0 && quitarCeros)
                    continue;

                if (i >= 0)
                    l.Add(i);
            }

            return l;
        }


        
        public static string RemplazarCaracteres(this string str, string caracterDeRemplazo = "")
        {
            return str.RemplazarCaracteres(@"[^\w\.@-_]", caracterDeRemplazo);
        }

        public static string RemplazarCaracteres(this string str, string caracteresNoValidos = @"[^\w\.@-_]", string caracterDeRemplazo = "")
        {
            return Regex.Replace(str, caracteresNoValidos, caracterDeRemplazo, RegexOptions.None);
        }

    }

    public static class Numeros
    {
        public static int Entero(this string str)
        {
            int numero = 0;
            if (str.IsNullOrEmpty())
                return numero;

            int.TryParse(str, out numero);
            return numero;
        }

        public static bool EsEntero(this string str)
        {
            bool result = int.TryParse(str, out _);
            return result;
        }

    }

    
    public static class Excepciones
    {
        public static string MensajeCompleto(this Exception exc, bool mostrarPila = false)
        {
            var result = "";
            var exOrigen = exc;
            while (exc != null)
            {
                var mensaje = exc.Message;

                if (!result.Contains(mensaje))
                    result += mensaje + Environment.NewLine;
                exc = exc.InnerException;
            }

            if (mostrarPila)
                result += exOrigen.StackTrace;

            return result;
        }

    }

}
