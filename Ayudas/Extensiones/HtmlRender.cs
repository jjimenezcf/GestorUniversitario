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

        private static string ComponerFilaHtml(string id, int numeroDeFila, FilaDelGrid filaDelGrid)
        {
            var fila = new StringBuilder();
            var input = "<input type=¨text¨ id=¨celda_id_i_j¨ name=¨columna_id_j¨ value=¨valor¨></input>";
            var j = 0;
            foreach (var valor in filaDelGrid.Valores)
            {
                var celda = input
                    .Replace("celda_id_i_j", $"celda_{id}_{numeroDeFila}_{j}")
                    .Replace("columna_id_j", $"columna_{id}_{j}")
                    .Replace("valor", valor)
                    .Render();
                j = j + 1;
                fila.AppendLine($"<td>{Environment.NewLine}{celda}{Environment.NewLine}</td>");
            }
            return $@"<tr>{Environment.NewLine}{fila.ToString()}{Environment.NewLine}</tr>";
        }

        private static string ComponerFilaSeleccionableHtml(string id, int numeroDeFila, FilaDelGrid filaDelGrid)
        {
            var j = filaDelGrid.Valores.Count - 1;
            var check = "<input type=¨checkbox¨ id=¨celda_id_i_j¨ name=¨columna_id_j¨ aria-label=¨Checkbox for following text input¨>"
                    .Replace("celda_id_i_j", $"celda_{id}_{numeroDeFila}_{j}")
                    .Replace("columna_id_j", $"columna_{id}_{j}")
                    .Render();

            var celda = $@"<td>{Environment.NewLine}{check}{Environment.NewLine}</td>";
            return ComponerFilaHtml(id, numeroDeFila, filaDelGrid).Replace("</tr>", $"{celda}{Environment.NewLine}</tr>");
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
            }

            return htmlDetalleGrid.ToString().Render();
        }

        public static string RenderizarTabla(string idTabla, List<ColumnaDelGrid> columnasDelGrid, List<FilaDelGrid> filasDelGrid, bool incluirCheck)
        {

            var htmlTabla = RenderCabeceraGrid(idTabla, columnasDelGrid);
            htmlTabla = htmlTabla.Replace("renderizarCuerpo", RenderDetalleGrid(idTabla, filasDelGrid));

            return htmlTabla;
        }
    }
}
