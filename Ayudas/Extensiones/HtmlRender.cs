using System;
using System.Collections.Generic;
using System.Text;

namespace Extensiones
{
    public static class HtmlRender
    {
        public static string Render(this string cadena)
        {
            return cadena.Replace("¨", "\"");
        }

        private static string componerFilaHtml(string id, int numeroDeFila, List<string> valores)
        {
            var fila = new StringBuilder();
            var input = "<input type=¨text¨ id=¨celda_id_i_j¨ name=¨columna_id_j¨ value=¨valor¨></input>";
            var j = 0;
            foreach (var valor in valores)
            {
                var celda = input
                    .Replace("celda_id_i_j", $"celda_{id}_{numeroDeFila}_{j}")
                    .Replace("columna_id_j", $"columna_{id}_{j}")
                    .Replace("valor", valor)
                    .Render();
                j = j + 1;
                fila.AppendLine($"<td>{celda}</td>");
            }
            return $@"<tr>{fila.ToString()}</tr>";
        }

        private static string componerFilaSeleccionableHtml(string id, int numeroDeFila, List<string> valores)
        {
            var j = valores.Count - 1;
            var check = "<input type=¨checkbox¨ id=¨celda_id_i_j¨ name=¨columna_id_j¨ aria-label=¨Checkbox for following text input¨>"
                    .Replace("celda_id_i_j", $"celda_{id}_{numeroDeFila}_{j}")
                    .Replace("columna_id_j", $"columna_{id}_{j}")
                    .Render();

            var celda = $@"<td>{check}</td>";
            return componerFilaHtml(id, numeroDeFila, valores).Replace("</tr>", $"{celda}</tr>");
        }

        private static string ComponerTablaHtml(List<string> columnas, StringBuilder filas)
        {
            var cabecera = new StringBuilder();
            foreach (var valor in columnas)
            {
                cabecera.AppendLine($"<th><a>{valor}</a></th>");
            }
            return $@"
                <table class=¨table¨>
                    <thead>
                        <tr>
                          {cabecera.ToString()}
                        <th></th>
                        </tr>
                    </thead>
                    <tbody>
                      {filas.ToString()}
                    </tbody>
                </table>";
        }

        public static string RenderizarTabla(string id, List<string> cabecera, List<List<string>> filas, bool incluirCheck)
        {
            var htmlFilas = new StringBuilder();
            int i = 0;
            foreach (var fila in filas)
            {
                htmlFilas.Append(incluirCheck 
                                 ? componerFilaSeleccionableHtml(id, i, fila) 
                                 : componerFilaHtml(id, i, fila));
            }

            var htmlTabla = ComponerTablaHtml(cabecera, htmlFilas);

            return htmlTabla;
        }
    }
}
