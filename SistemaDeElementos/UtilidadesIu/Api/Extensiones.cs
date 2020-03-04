using Gestor.Elementos;
using System.Collections.Generic;
using Utilidades;

namespace UniversidadDeMurcia.UtilidadesIu
{
    public static class parametrosMvc
    {
        public static Dictionary<string, Ordenacion> ParsearOrdenacion(this string orden)
        {
            var ordenParseado = new Dictionary<string, Ordenacion>();

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
                        if (i + 1 == ordenes.Length && !ordenes[i].IsNullOrEmpty())
                        {
                            ordenParseado[ordenes[i]] = Ordenacion.Ascendente;
                            break;
                        }

                        if (ordenes[i + 1].IsNullOrEmpty())
                        {
                            ordenParseado[ordenes[i]] = Ordenacion.Ascendente;
                            break;
                        }

                        if (ordenes[i + 1] == Ordenacion.Ascendente.ToString())
                        {
                            ordenParseado[ordenes[i]] = Ordenacion.Ascendente;
                            break;
                        }

                        if (ordenes[i + 1] == Ordenacion.Descendente.ToString())
                        {
                            ordenParseado[ordenes[i]] = Ordenacion.Descendente;
                            break;
                        }

                        i = i + 2;
                    }

                }
            }

            return ordenParseado;
        }

    }
}
