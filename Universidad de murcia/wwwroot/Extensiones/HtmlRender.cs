using System;
using System.Collections.Generic;
using System.Text;

namespace Extensiones
{
    public enum Aliniacion  {no_definida, izquierda, centrada, derecha, justificada};

    public class ColumnaDelGrid
    {
        private Aliniacion _alineada;
        private string _titulo;

        public string Id { get; set; }
        public string Nombre { get; set; }
        public string Titulo { get { return _titulo == null ? Nombre : _titulo; } set { _titulo = value; } }
        public Type Tipo { get; set; } = typeof(string);
        public int Ancho { get; set; } = 0;
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

        public string AlineacionCss
        {
            get
            {
                switch (Alineada)
                {
                    case Aliniacion.izquierda:
                        return "text-left";
                    case Aliniacion.derecha:
                        return "text-right";
                    case Aliniacion.centrada:
                        return "text-center";
                    case Aliniacion.justificada:
                        return "text-justify";
                    default:
                        return "text-left";
                }
            }
        }

        public string Ruta { get; set; }
        public string Accion { get; set; }

        public static string ToString(ColumnaDelGrid columna)
        {
            return columna.Titulo;
        }

    }

    public class CeldaDelGrid
    {

        private ColumnaDelGrid _columna;
        public string IdCabecera => _columna.Id;
        public string Id { get; set; }
        public object Valor { get; set; }        
        public Type Tipo => _columna.Tipo;
        public string AlineacionCss => _columna.AlineacionCss;
        public bool Visible => _columna.Visible;
        public bool Editable => _columna.Editable;

        public string AlEntrar { get; set; }
        public string AlSalir { get; set; }
        public string AlCambiar { get; set; }

        public CeldaDelGrid(ColumnaDelGrid columna)
        {
            _columna = columna;
        }
        
        public static string ToString(CeldaDelGrid columna)
        {
            return columna.Valor.ToString();
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


        private static string RenderCeldaCheck(string idGrid, string idCelda)
        {
            var check = $"<input type=¨checkbox¨ id=¨chx_{idGrid}_{idCelda}¨ name=¨chx_{idGrid}¨ aria-label=¨Marcar para seleccionar¨>";
            var celdaDelCheck = $@"<td>{Environment.NewLine}{check}{Environment.NewLine}</td>";
            return celdaDelCheck.Render();
        }

        public static string RenderColumnaCabecera(ColumnaDelGrid columna)
        {
            var visible = columna.Visible ? "" : "¨display:{none};¨";
            var ancho = columna.Ancho == 0 ? "" : $"height: {columna.Ancho}%;";
            var estilo = visible + ancho == "" ? "" : $"style =¨{ancho} {visible}¨";
            return $"{Environment.NewLine}<th id= ¨{columna.Id}¨ class=¨{columna.AlineacionCss}¨ {estilo}>{columna.Titulo}</th>".Render();
            //<a href=¨/ruta/accion?orden=ordenPor¨>
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
        }

        public static string RenderCelda(CeldaDelGrid celda)
        {
            return $"<td id=¨{celda.Id}¨>{celda.Valor}</td>".Render();
        }

        private static string RenderFila(int numFil, FilaDelGrid filaDelGrid)
        {
            var fila = new StringBuilder();
            foreach (var celda in filaDelGrid.Celdas)
            {
                celda.Id = $"{celda.IdCabecera}_{numFil}";                
                fila.AppendLine(RenderCelda(celda));
            }
            return $@"{fila.ToString()}";
        }
        
        private static string RenderFilaSeleccionable(string idGrid, int numFil, FilaDelGrid filaDelGrid)
        {
            string filaHtml = RenderFila(numFil, filaDelGrid);
            string celdaDelCheck = RenderCeldaCheck($"{idGrid}", $"{filaDelGrid.Celdas.Count}_{numFil}");
            return $"<tr>{Environment.NewLine}{filaHtml}{celdaDelCheck}{Environment.NewLine}</tr>{Environment.NewLine}";
        }

        private static string RenderCabecera(string idGrid, IEnumerable<ColumnaDelGrid> columnasGrid)
        {
            var cabeceraHtml = new StringBuilder();
            var numCol = 0;
            foreach (var columna in columnasGrid)
            {
                columna.Id = $"{idGrid}_{numCol}";
                cabeceraHtml.Append(RenderColumnaCabecera(columna));
                numCol++;
            }
            return $@"<thead>{Environment.NewLine}<tr>{cabeceraHtml.ToString()}{Environment.NewLine}</tr>{Environment.NewLine}</thead>";
        }

        private static string RenderDetalleGrid(string idGrid, IEnumerable<FilaDelGrid> filas)
        {
            var htmlDetalleGrid = new StringBuilder();
            int i = 0;
            foreach (var fila in filas)
            {
                htmlDetalleGrid.Append(RenderFilaSeleccionable(idGrid, i, fila));
                i = i + 1;
            }
            return htmlDetalleGrid.ToString();
        }

        public static string RenderizarTabla(string idGrid, List<ColumnaDelGrid> columnasDelGrid, List<FilaDelGrid> filasDelGrid, bool incluirCheck)
        {
            var htmlTabla = $@"<table id=¨{idGrid}¨ class=¨table¨>{Environment.NewLine}{RenderCabecera(idGrid, columnasDelGrid)}{Environment.NewLine}{RenderDetalleGrid(idGrid, filasDelGrid)}</table>";
            return htmlTabla;
        }
    }
}
