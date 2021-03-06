﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            if (cadena.StartsWith("|") && cadena.Length > 1)
            {
                cadena = cadena.Substring(1);
                if (str.Length < cadena.Length)
                    return false;

                return str.StartsWith(cadena);
            }

            if (cadena.EndsWith("|") && cadena.Length > 1)
            {
                cadena = cadena.Substring(0, cadena.Length - 1);
                if (str.Length < cadena.Length)
                    return false;

                return str.EndsWith(cadena);
            }

            return str.Contains(cadena);
        }

        public static int Incluir(this List<int> lista, string cadena, string separador = ";", bool quitarCeros = true)
        {
            var elementos = lista.Count;
            if (cadena.IsNullOrEmpty())
                return 0;
            lista.AddRange(cadena.ToLista<int>(separador,quitarCeros));
            return lista.Count - elementos;
        }

        public static string ToString(this List<string> lista, string separador)
        {
            var retorno = "";
            foreach(var l in lista)
            {
                if (l.IsNullOrEmpty())
                    continue;
                retorno = $"{retorno}{separador}{l}";
            }
            return retorno.IsNullOrEmpty() ? retorno: retorno.Substring(1);
        }

        public static List<T> ToLista<T>(this string str, string separador = ";", bool quitarNulos = true)
        {
            var l = new List<T>();
            if (str.IsNullOrEmpty())
                return l;

            var cadenas = str.Split(separador);
            foreach (string c in cadenas)
            {
                if (typeof(T) == typeof(int))
                {
                    var i = c.Entero();
                    if (i == 0 && quitarNulos)
                        continue;

                    if (i >= 0)
                        l.Add((T)(object)i);
                    continue;
                }

                if (typeof(T) == typeof(string))
                {
                    if (c.IsNullOrEmpty() && quitarNulos)
                        continue;
                    l.Add((T)(object)c);
                    continue;
                }

                throw new Exception($"No se ha definido como se pasa a una lista el tipo {typeof(T)}");
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

        public static bool EsFecha(this string fecha)
        {
            try
            {
                DateTime.Parse(fecha);
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static DateTime? Fecha(this string fecha, bool finDelDia = false)
        {
            DateTime? f = null;
            if (!fecha.IsNullOrEmpty() && fecha.EsFecha())
            {
                f = DateTime.Parse(fecha);
                if (finDelDia && f.HasValue && ((DateTime)f).Hour == 0 && ((DateTime)f).Minute == 0 && ((DateTime)f).Second == 0)
                {
                    f = ((DateTime)f).AddHours(23);
                    f = ((DateTime)f).AddMinutes(59);
                    f = ((DateTime)f).AddSeconds(59);
                }
            }
            return f;
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

    public static class EnumExtensions
    {
        public static string ToDescription<T>(this T valorEnumerado)
        {
            var type = valorEnumerado.GetType();
            if (!type.IsEnum)
            {
                throw new Exception($"{nameof(valorEnumerado)} debe ser un valor de enumerado");
            }
            var memberInfo = type.GetMember(valorEnumerado.ToString());
            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return valorEnumerado.ToString();
        }
    }

}
