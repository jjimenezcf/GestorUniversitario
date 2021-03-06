﻿using ModeloDeDto;

namespace UtilidadesParaIu
{

    public static class HtmlRender
    {
        public static string Render(this string cadena)
        {
            while (cadena.IndexOf("< ") >= 0)
                cadena = cadena.Replace("< ", "<");


            while (cadena.IndexOf("  ") >= 0)
                cadena = cadena.Replace("  ", " ");

            return cadena.Replace("¨", "\"");
        }

        public static string AlineacionCss(Aliniacion alineacion)
        {
            switch (alineacion)
            {
                case Aliniacion.izquierda:
                    return "left";
                case Aliniacion.derecha:
                    return "right";
                case Aliniacion.centrada:
                    return "center";
                case Aliniacion.justificada:
                    return "justify";
                default:
                    return "left";
            }
        }

        public static string AlineacionTabulator(Aliniacion alineada)
        {
            switch (alineada)
            {
                case Aliniacion.izquierda:
                    return "left";
                case Aliniacion.derecha:
                    return "right";
                case Aliniacion.centrada:
                    return "center";
                case Aliniacion.justificada:
                    return "justify";
                default:
                    return "left";
            }
        }
    }
}
