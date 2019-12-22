using System;
using System.Collections.Generic;
using System.Text;

namespace Extensiones
{

    public class ColumnaDelGrid
    {
        public string Nombre { get; set; }
        public bool Ordenar { get; set; }
        public string OrdenPor => $"ordenoPor{Nombre}";
        public string Sentido = "Asc";

        public string Ruta { get; set; }
        public string Accion { get; set; }

    }
    
    public class FilaDelGrid
    {
        public List<string> Valores = new List<string>();
    }


    public static class HtmlRender
    {
        public static string Render(this string cadena)
        {
            return cadena.Replace("¨", "\"");
        }

        private static string ComponerFilaHtml(string idGrid, int numFil, FilaDelGrid filaDelGrid)
        {
            var fila = new StringBuilder();
            var numCol = 0;
            foreach (var valor in filaDelGrid.Valores)
            {
                var celda = $"<input type=¨text¨ id=¨{idGrid}_{numFil}_{numCol}¨ name=¨txt_{idGrid}_{numCol}¨ value=¨{valor}¨></input>"
                    .Render();
                numCol = numCol + 1;
                fila.AppendLine($"<td>{Environment.NewLine}{celda}{Environment.NewLine}</td>");
            }
            return $@"<tr>{Environment.NewLine}{fila.ToString()}{Environment.NewLine}</tr>";
        }

        private static string ComponerFilaSeleccionableHtml(string idGrid, int numFil, FilaDelGrid filaDelGrid)
        {
            var numCol = filaDelGrid.Valores.Count;
            var check = $"<input type=¨checkbox¨ id=¨{idGrid}_{numFil}_{numCol}¨ name=¨chx_{idGrid}_{numCol}¨ aria-label=¨Marcar para seleccionar¨>"
                        .Render();
            
            var celdaDelCheck = $@"<td>{Environment.NewLine}{check}{Environment.NewLine}</td>";
            return ComponerFilaHtml(idGrid, numFil, filaDelGrid).Replace("</tr>", $"{celdaDelCheck}{Environment.NewLine}</tr>");
        }

        private static string RenderCabeceraGrid(string idGrid, IEnumerable<ColumnaDelGrid> columnasGrid)
        {

            var htmlCabeceraGrid = $@"
                                    <table id=¨{idGrid}¨ class=¨table¨>
                                        <thead>
                                            <tr>
                                            renderColunasCabecera
                                            </tr>
                                        </thead>
                                    	renderizarCuerpo
                                    </table>                                    
                                   ";
            var htmlColumnaCabecera = @" <th>
                                           <a href=¨/ruta/accion?orden=ordenPor¨>Columna.Nombre</a>
                                         </th>
                                       ";
            var htmlColumnasCabecera = new StringBuilder();
            foreach (var columna in columnasGrid)
            {
                var html = htmlColumnaCabecera;
                if (columna.Ordenar)
                {
                    html = html.Replace("ruta", columna.Ruta)
                        .Replace("accion", columna.Accion)
                        .Replace("ordenPor", $"{columna.OrdenPor}{columna.Sentido}");
                }
                else
                {
                    html = html.Replace(" href=¨/ruta/accion?orden=ordenPor¨", "");
                }
                html = html.Replace("Columna.Nombre", columna.Nombre).Render();
                htmlColumnasCabecera.AppendLine(html);
            }

            return htmlCabeceraGrid.Replace("renderColunasCabecera", htmlColumnasCabecera.ToString()).Render();
        }

        private static string RenderDetalleGrid(string idGrid, IEnumerable<FilaDelGrid> filas)
        {
            var htmlDetalleGrid = new StringBuilder();
            int i = 0;
            foreach (var fila in filas)
            {
                htmlDetalleGrid.Append(ComponerFilaSeleccionableHtml(idGrid, i, fila));
                i = i + 1;
            }

            return htmlDetalleGrid.ToString().Render();
        }

        public static string RenderizarTabla(string idGrid, List<ColumnaDelGrid> columnasDelGrid, List<FilaDelGrid> filasDelGrid, bool incluirCheck)
        {

            var htmlTabla = RenderCabeceraGrid(idGrid, columnasDelGrid);
            htmlTabla = htmlTabla.Replace("renderizarCuerpo", RenderDetalleGrid(idGrid, filasDelGrid));

            return htmlTabla;
        }
    }
}
