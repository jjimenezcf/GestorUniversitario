using Gestor.Elementos;
using System.Collections.Generic;
using Utilidades;

namespace MVCSistemaDeElementos.UtilidadesIu
{
    public static class parametrosMvc
    {
        public static List<ClausulaOrdenacion> ParsearOrdenacion(this string orden)
        {
            var ordenParseado = new List<ClausulaOrdenacion>();

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
                        var clausula = new ClausulaOrdenacion();
                        clausula.Propiedad = ordenes[i];
                        clausula.modo = ModoDeOrdenancion.ascendente;

                        if (i + 1 < ordenes.Length && ordenes[i + 1] == ModoDeOrdenancion.descendente.ToString())
                            clausula.modo = ModoDeOrdenancion.descendente;

                        ordenParseado.Add(clausula);
                        i = i + 2;
                    }

                }
            }

            return ordenParseado;
        }

    }
}
