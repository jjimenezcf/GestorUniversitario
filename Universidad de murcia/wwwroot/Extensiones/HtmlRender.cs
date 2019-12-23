using System;
using System.Collections.Generic;
using System.Text;

namespace Extensiones
{
    public enum Aliniacion  {no_definida, izquierda, centrada, derecha};

    public class ColumnaDelGrid
    { 
        private Aliniacion _alineada;
        private string _titulo;

        public string Nombre { get; set; }
        public string Titulo { get { return _titulo == null ? Nombre : _titulo; } set { _titulo = value; } }
        public Type Tipo { get; set; } = typeof(string);
        public bool Ordenar { get; set; } = false;
        public string OrdenPor => $"ordenoPor{Nombre}";
        public string Sentido = "Asc";
        public bool Visible { get; set; } = true;
        public bool Editable { get; set; } = false;
        public IFormatProvider Mascara { get; set; } = null;
        public Aliniacion Alineada
        {
            get
            {
                return _alineada == Aliniacion.no_definida
                       ? (Tipo == typeof(int) || (Tipo == typeof(decimal)) || (Tipo == typeof(DateTime))
                          ? Aliniacion.derecha
                          : Aliniacion.izquierda)
                       : _alineada;
            }
            set { _alineada = value; }
        }

        public string Ruta { get; set; }
        public string Accion { get; set; }

    }

    public class CeldaDelGrid
    {

        private ColumnaDelGrid _columna;
        private string _valor;
        public string Valor { get { return _columna.Mascara == null ? _valor : _valor.ToString(_columna.Mascara); } set { _valor = value; } }
        public Type Tipo { get { return _columna.Tipo; } }
        public Aliniacion Alineada { get { return _columna.Alineada; } }
        public bool Visible { get { return _columna.Visible; } }
        public bool Editable { get { return _columna.Editable; } }

        public string AlEntrar { get; set; }
        public string AlSalir { get; set; }
        public string AlCambiar { get; set; }

        public CeldaDelGrid(ColumnaDelGrid columna)
        {
            _columna = columna;
        }
    }

    public class FilaDelGrid
    {
        public List<CeldaDelGrid> Celdas = new List<CeldaDelGrid>();
    }


    public static class HtmlRender
    {
        public static string Render(this string cadena)
        {
            return cadena.Replace("¨", "\"");
        }
        //visibility: hidden
        private static string ComponerFilaHtml(string idGrid, int numFil, FilaDelGrid filaDelGrid)
        {
            var fila = new StringBuilder();
            var numCol = 0;
            foreach (var celda in filaDelGrid.Celdas)
            {
                var estilo = "style =¨display:{visible};¨"
                    .Replace("{visible}", celda.Visible ? "inline" : "none");

                var control = $"<input type=¨text¨ id=¨{idGrid}_{numFil}_{numCol}¨ name=¨txt_{idGrid}_{numCol}¨ {estilo} value=¨{celda.Valor}¨/>"
                     .Render();
                
                numCol = numCol + 1;

                fila.AppendLine($"<td {estilo}>{Environment.NewLine}{control}{Environment.NewLine}</td>");
            }
            return $@"<tr>{Environment.NewLine}{fila.ToString()}{Environment.NewLine}</tr>";
        }

        private static string ComponerFilaSeleccionableHtml(string idGrid, int numFil, FilaDelGrid filaDelGrid)
        {
            var numCol = filaDelGrid.Celdas.Count;
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

            var htmlColumnaCabecera = @" <th style=¨display:{visible};¨>{Columna.Nombre}</th>
                                       ";
            //<a href=¨/ruta/accion?orden=ordenPor¨>
            var htmlColumnasCabecera = new StringBuilder();
            foreach (var columna in columnasGrid)
            {
                var html = htmlColumnaCabecera
                    .Replace("{visible}", columna.Visible ? "inline" : "none")
                    .Replace("{Columna.Nombre}", columna.Nombre);

                //if (columna.Ordenar)
                //{
                //    html = html.Replace("ruta", columna.Ruta)
                //        .Replace("accion", columna.Accion)
                //        .Replace("ordenPor", $"{columna.OrdenPor}{columna.Sentido}");
                //}
                //else
                //{
                //    html = html.Replace(" href=¨/ruta/accion?orden=ordenPor¨", "");
                //}

                html = html.Render();
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
