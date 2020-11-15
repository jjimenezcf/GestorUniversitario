using GestorDeElementos;
using System.Collections.Generic;
using Utilidades;

namespace MVCSistemaDeElementos.UtilidadesIu
{
    public static class parametrosMvc
    {
        public static List<ClausulaDeOrdenacion> ParsearOrdenacion(this string orden)
        {
            var ordenParseado = new List<ClausulaDeOrdenacion>();

            if (!orden.IsNullOrEmpty())
            {
                var ordenes = orden.Split(';');
                var i = 0;
                while (i < ordenes.Length)
                {
                    if (ordenes[i].IsNullOrEmpty())
                        break;
                    else
                    {
                        var clausula = new ClausulaDeOrdenacion();
                        clausula.Criterio = ordenes[i];
                        clausula.Modo = ModoDeOrdenancion.ascendente;

                        if (i + 1 < ordenes.Length && ordenes[i + 1] == ModoDeOrdenancion.descendente.ToString())
                            clausula.Modo = ModoDeOrdenancion.descendente;

                        ordenParseado.Add(clausula);
                        i = i + 2;
                    }

                }
            }

            return ordenParseado;
        }

    }
}
