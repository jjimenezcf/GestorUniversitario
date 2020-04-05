using System;
using System.Collections.Generic;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{

    public class Dimension
    {
        public int Filas { get; private set; }
        public int Columnas { get; private set; }

        public Dimension(int filas, int columnas)
        {
            Filas = filas;
            Columnas = columnas;
        }
    }

    public class TablaFiltro: ControlFiltroHtml
    {
        public Dimension Dimension { get; private set; }
        public ICollection<ControlFiltroHtml> Controles { get; set; }

        public TablaFiltro(ControlFiltroHtml padre, Dimension dimension, ICollection<ControlFiltroHtml> controles)
        : base(
          padre: padre,
          id: $"{padre.Id}_Tabla",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.TablaBloque;
            Dimension = dimension;
            Controles = controles;
        }

        public override string RenderControl()
        {
            return RenderTabla();
        }

        private string RenderTabla()
        {

            var htmlTabla = $@"<table id=¨{IdHtml}¨ width=¨100%¨>
                                  filas
                               </table>";
            var htmlFilas = "";
            for (var i = 0; i < Dimension.Filas; i++)
                htmlFilas = $"{htmlFilas}{(htmlFilas.IsNullOrEmpty() ? "" : Environment.NewLine)}{RenderFila(i)}";

            return htmlTabla.Replace("filas", htmlFilas);
        }

        private string RenderFila(int i)
        {
            var idFila = $"{IdHtml}_{i}";
            var htmlFila = $@"<tr id=¨{idFila}¨>
                                 columnas
                              </tr>";
            var htmlColumnas = "";
            for (var j = 0; j < Dimension.Columnas; j++)
                htmlColumnas = $"{htmlColumnas}{(htmlColumnas.IsNullOrEmpty() ? "" : Environment.NewLine)}{RenderColumnasControl(idFila, i, j)}";


            return htmlFila.Replace("columnas", htmlColumnas);
        }

        private string RenderColumnasControl(string idFila, int i, int j)
        {
            var idColumna = $"{idFila}_{j}";
            var htmlColumnaEtiqueta = $@"<td id=¨{idColumna}_e¨ style=¨width:15%¨>
                                            etiqueta
                                         </td>";
            var htmlColumnaControl = $@"<td id=¨{idColumna}_c¨ style=¨width:35%¨>
                                           control
                                        </td>";
            var htmlControl = "";
            var htmlEtiqueta = "";
            foreach (ControlHtml c in Controles)
            {
                if (c.Posicion == null)
                    continue;

                if (c.Posicion.fila == i && c.Posicion.columna == j)
                    htmlEtiqueta = $"{c.RenderLabel()}";

                if (c.Posicion.fila == i && c.Posicion.columna == j)
                    htmlControl = $"{c.RenderControl()}";
            }


            return htmlColumnaEtiqueta.Replace("etiqueta", htmlEtiqueta) +
                   Environment.NewLine +
                   htmlColumnaControl.Replace("control", htmlControl);
        }

    }
}
