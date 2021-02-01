using System;
using System.Collections.Generic;
using Enumerados;
using ModeloDeDto;
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

        public void CambiarDimension(Posicion posicion)
        {
            if (posicion.fila >= Filas)
                Filas = posicion.fila + 1;

            if (posicion.columna >= Columnas)
                Columnas = posicion.columna + 1;
        }

        internal void NumeroDeFilas(int fila)
        {
            Filas = fila;
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
            Tipo = enumTipoControl.TablaBloque;
            Dimension = dimension;
            Controles = controles;
        }

        public override string RenderControl()
        {
            var htmlTabla = $@" 
                <!--  ***************** tabla de filtrado: {Padre.Etiqueta} ******************* -->
                <table id=¨{IdHtml}¨ class=¨{Css.Render(enumCssMnt.MntTablaDeFiltro)}¨>
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
            //double porColumnas = 100 / Dimension.Columnas;
            //double porColEtiq = (porColumnas * 30) / (100);
            //double porColCtrl = (porColumnas * 70) / (100);

            double porColumnas = 100 / Dimension.Columnas;
            double porColEtiq = (double) 7.5;
            double porColCtrl = porColumnas - porColEtiq;

            var idColumna = $"{idFila}_{j}";
            var htmlColumnaEtiqueta = $@"<td id=¨{idColumna}_e¨ style=¨width:{porColEtiq.ToString().Replace(',','.')}%¨>
                                            etiqueta
                                         </td>";
            var htmlColumnaControl = $@"<td id=¨{idColumna}_c¨ style=¨width:{porColCtrl.ToString().Replace(',', '.')}%¨>
                                           control
                                        </td>";
            var htmlControl = "";
            var htmlEtiqueta = "";
            foreach (ControlHtml c in Controles)
            {
                if (c.Posicion == null)
                    continue;

                if (c.Posicion.fila >= Dimension.Filas)
                    Gestor.Errores.GestorDeErrores.Emitir($"El control {c.Propiedad} no puede ser renderizado en la fila indicada {c.Posicion.fila}, solo hay {Dimension.Filas} filas");

                if (c.Posicion.columna >= Dimension.Columnas)
                    Gestor.Errores.GestorDeErrores.Emitir($"El control {c.Propiedad} no puede ser renderizado en la columna indicada {c.Posicion.columna}, solo hay {Dimension.Columnas} columnas");

                if (c.Posicion.fila == i && c.Posicion.columna == j)
                    htmlEtiqueta = $"{(c.Tipo == enumTipoControl.Check ? "" : c.RenderLabel(c.IdHtml))}";

                if (c.Posicion.fila == i && c.Posicion.columna == j)
                    htmlControl = $"{c.RenderControl()}";
            }


            return htmlColumnaEtiqueta.Replace("etiqueta", htmlEtiqueta) +
                   Environment.NewLine +
                   htmlColumnaControl.Replace("control", htmlControl);
        }

    }
}
