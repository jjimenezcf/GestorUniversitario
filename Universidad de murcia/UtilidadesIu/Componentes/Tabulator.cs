using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilidadesParaIu
{
    public class Tabulator
    {
        public static string RenderizarGrid(Grid grid)
        {
            var tabulator = $@"
            <div id=¨{grid.Id}¨>
            </div>";

            var seleccionable = $"selectable: {grid.ConSeleccion.ToString().ToLower()}";

            var crearTabulator = @"
            <script>
                var table = new Tabulator(¨#{grid.Id}¨, {
                    layout:¨fitColumns¨,
                    {seleccionable},
                    columns: [{columnas}],
                });
            </script>
            "
            .Replace("{grid.Id}", grid.Id)
            .Replace("{seleccionable}", seleccionable)
            .Replace("{columnas}", TabuladorCabecera(grid.Id, grid.columnas));

            return $"{tabulator}{Environment.NewLine}{crearTabulator}";
        }


        private static string TabuladorCabecera(string idGrid, IEnumerable<ColumnaDelGrid> columnasGrid)
        {
            var tabuladorCabecera = new StringBuilder();
            foreach (var columna in columnasGrid)
            {
                tabuladorCabecera.Append(TabuladorColumnaCabecera(columna));
            }

            return $@"{tabuladorCabecera.ToString()}{Environment.NewLine}";
        }


        private static string TabuladorColumnaCabecera(ColumnaDelGrid columna)
        {
            var ancho = columna.Ancho > 0 ? $", width: {columna.Ancho} " : "";
            var visible = $", visible: {columna.Visible.ToString().ToLower()} ";
            var editable = $", editor: {columna.Editable.ToString().ToLower()} ";
            var sorter = columna.Ordenar ? $", sorter: ¨{columna.Tipo.Name.ToLower()}¨ " : "";
            var alineacion = $", align:¨{columna.AlineacionTabulator}¨";

            var descriptor = "{title: ¨Titulo¨, field: ¨Nombre¨{sorter}{ancho}{visible}{editable}{alineacion}}";
            descriptor = descriptor
               .Replace("Titulo", columna.Titulo)
               .Replace("Nombre", columna.Nombre)
               .Replace("{sorter}", sorter)
               .Replace("{ancho}", ancho)
               .Replace("{visible}", visible)
               .Replace("{editable}", editable)
               .Replace("{alineacion}", alineacion);

            return $"{Environment.NewLine}{descriptor},";
        }

    }
}
