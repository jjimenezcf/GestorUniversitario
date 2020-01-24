using System;
using System.Text;
using System.Collections.Generic;
using System.Text.Json;
using System.Web;

namespace UtilidadesParaIu
{
    public class Grid
    {
        string _id;
        public string Id { get { return _id.ToLower(); } private set { _id = value; } }
        public string Ruta { get; set; }
        public List<ColumnaDelGrid> columnas { get; private set; }
        public List<FilaDelGrid> filas { get; private set; }

        public int TotalEnBd { get; set; }
        private int _PosicionInicial { get; set; }
        private int _CantidadPorLeer { get; set; }
        public int Seleccionables { get; set; }
        public int Ultimo_Leido => _PosicionInicial + filas.Count;
        
        public bool ConSeleccion { get; set; } = true;
        public bool ConNavegador { get; set; } = true;
        public ModeloGrid Modelo { get; private set; } = ModeloGrid.Propio;

        public Grid(string idGrid, List<ColumnaDelGrid> columnasGrid, List<FilaDelGrid> filasDelGrid, int posicionInicial, int cantidadPorLeer)
        {
            IniciarClase(idGrid, columnasGrid, filasDelGrid, posicionInicial, cantidadPorLeer);
        }

        private void IniciarClase(string idGrid, List<ColumnaDelGrid> columnasGrid, List<FilaDelGrid> filasDelGrid, int posicionInicial, int cantidadPorLeer)
        {
            Id = idGrid;
            columnas = columnasGrid;
            filas = filasDelGrid;
            _PosicionInicial = posicionInicial;
            _CantidadPorLeer = cantidadPorLeer;
            Seleccionables = 1;
        }

        public string ToHtml()
        {
            return (Modelo == ModeloGrid.Tabulator 
                              ? Tabulator.RenderizarGrid(this) 
                              : RenderizarGrid(this));
        }


        private static string RenderColumnaCabecera(ColumnaDelGrid columna)
        {
            var visible = columna.Visible ? "" : "hidden";
            var ancho = columna.Ancho == 0 ? "" : $"width: {columna.Ancho}%;";
            var estilo = visible + ancho == "" ? "" : $"{ancho} {visible}";

            columna.descriptor.visible = visible;
            columna.descriptor.alineada = columna.AlineacionCss;

            var descriptor = $"descriptor={JsonSerializer.Serialize(columna.descriptor)}";

            return $"{Environment.NewLine}<th scope=¨col¨ id= ¨{columna.descriptor.id}¨ class=¨{columna.AlineacionCss}¨ {estilo} {descriptor}>{columna.Titulo}</th>";
        }

        private static string RenderColumnaDeSeleccion(string idGrid)
        {
            var visible = "";
            var ancho = "";
            var estilo = visible + ancho == "" ? "" : $"{ancho} {visible}";
            var columna = new ColumnaDelGrid();
            columna.Nombre = idGrid + "_chk_sel";
            columna.Id = $"{idGrid}_{columna.Propiedad}";
            columna.Titulo = " ";
            columna.descriptor.visible = visible;
            columna.descriptor.alineada = HtmlRender.AlineacionCss(Aliniacion.centrada);
            columna.descriptor.valor = "CrearCheck";

            var descriptor = $"descriptor={JsonSerializer.Serialize(columna.descriptor)}";

            return $"{Environment.NewLine}<th scope=¨col¨ id= ¨{columna.descriptor.id}¨ class=¨{columna.AlineacionCss}¨ {estilo} {descriptor}>{columna.Titulo}</th>";
        }


        private static string RenderCeldaCheck(string idGrid, string idCelda, int numFil, int numCol)
        {
            var check = $"<input type=¨checkbox¨ id=¨c_{idGrid}_{idCelda}¨ " +
                        $"                       name=¨chk_{idGrid}¨ " +
                        $"                       class=¨text-center¨ " +
                        $"                       aria-label=¨Marcar para seleccionar¨" +
                        $"                       onclick=¨MarcarParaSeleccionar('{idGrid}','c_{idGrid}_{idCelda}');¨ /> ";

            var celdaDelCheck = $@"<td id=¨{idGrid}_{numFil}_{numCol}¨ 
                                       name=¨{idGrid}_chk_sel¨ 
                                       class=¨{HtmlRender.AlineacionCss(Aliniacion.centrada)}¨>{Environment.NewLine}" +
                                $@"  {check}{Environment.NewLine}" +
                                $@"</td>";

            return celdaDelCheck;
        }

        private static string RenderCeldaInput(CeldaDelGrid celda)
        {
            var editable = !celda.Editable ? "readonly" : "";
            var input = $" <input id=¨i_{celda.Id}¨ " +
                        $"        name=¨i_{celda.IdCabecera}¨ " +
                        $"        class=¨{celda.AlineacionCss()}¨ " +
                        $"        style=¨width:100%; border:0¨ " +
                        $"        {editable} " +
                        $"        value=¨{celda.Valor}¨/>";

            var ocultar = celda.Visible ? "" : "hidden";
            return $"<td id=¨{celda.Id}¨ name=¨{celda.IdCabecera}¨ class=¨{celda.AlineacionCss()}¨ {ocultar}>" +
                   $"   {input}" +
                   $"</td>";
        }

        private static string RenderFila(int numFil, FilaDelGrid fila)
        {
            var filaHtml = new StringBuilder();
            foreach (var celda in fila.Celdas)
            {
                celda.Id = $"{celda.IdCabecera}_{numFil}";
                filaHtml.AppendLine(RenderCeldaInput(celda));
            }
            return $@"{filaHtml.ToString()}";
        }

        private static string RenderFilaSeleccionable(string idGrid, int numFil, FilaDelGrid fila)
        {
            string filaHtml = RenderFila(numFil, fila);
            string celdaDelCheck = RenderCeldaCheck($"{idGrid}", $"chk_{numFil}", numFil, fila.Celdas.Count);
            return $"<tr id='{idGrid}_f{numFil}'>{Environment.NewLine}" +
                   $"   {filaHtml}{celdaDelCheck}{Environment.NewLine}" +
                   $"</tr>{Environment.NewLine}";
        }

        private static string RenderCabecera(string idGrid, IEnumerable<ColumnaDelGrid> columnasGrid)
        {
            var cabeceraHtml = new StringBuilder();
            foreach (var columna in columnasGrid)
            {
                columna.Id = $"{idGrid}_{columna.Propiedad}";
                cabeceraHtml.Append(RenderColumnaCabecera(columna));
            }
            cabeceraHtml.Append(RenderColumnaDeSeleccion(idGrid)); ; //RenderCeldaCheck($"{idGrid}", $"chk");
            return $@"<thead id='{idGrid}_cab'>{Environment.NewLine}<tr id=¨{idGrid}_c0¨>{cabeceraHtml.ToString()}{Environment.NewLine}</tr>{Environment.NewLine}</thead>";
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
            return $@"<tbody id='{idGrid}_body'>{htmlDetalleGrid.ToString()}</tbody>";
        }

        private static string RenderNavegadorGrid(Grid grid)
        {
            var htmlNavegadorGrid = $@"
            <div class=¨text-center¨>
                <div id=¨{grid.Id}.Nav¨ style=¨float: left¨>
                    <div id=¨{grid.Id}.Nav.1¨ data-type=¨img¨ style=¨display:inline-block¨>
                        <img src=¨/images/paginaInicial.png¨ alt=¨Primera página¨ title=¨Ir al primer registro¨ width=¨22¨ height=¨22¨ onclick=¨Leer('{grid.Id}','{grid.Ruta}')¨>
                    </div>
                    <div id=¨{grid.Id}.Nav.2¨ class=¨mx-sm-3¨ style=¨display:inline-block¨>
                        <input type=¨number¨ id=¨{grid.Id}.Nav.2.Reg¨ value=¨{grid._CantidadPorLeer}¨ 
                                             min=¨1¨ step=¨1¨ max=¨999¨ 
                                             posicion=¨{grid.Ultimo_Leido}¨  
                                             totalEnBd=¨{grid.TotalEnBd}¨ title=¨leidos {grid.filas.Count} de {grid.TotalEnBd} desde la posición {grid._PosicionInicial}¨ 
                                             style=¨width: 50px;margin-top: 5px;align-content:center; border-radius: 10px¨>
                    </div>
                    <div id=¨id=¨{grid.Id}.Nav.3¨ data-type=¨img¨ style=¨display:inline-block¨>
                        <img src=¨/images/paginaAnterior.png¨ alt=¨Primera página¨ title=¨Página anterior¨ width=¨22¨ height=¨22¨ onclick=¨LeerAnteriores('{grid.Id}','{grid.Ruta}')¨>
                        <img src=¨/images/paginaSiguiente.png¨ alt=¨Siguiente página¨ title=¨Página siguiente¨ width=¨22¨ height=¨22¨ onclick=¨LeerSiguientes('{grid.Id}','{grid.Ruta}')¨>
                        <img src=¨/images/paginaUltima.png¨ alt=¨Última página¨ title=¨Última página¨ width=¨22¨ height=¨22¨ onclick=¨LeerUltimos('{grid.Id}','{grid.Ruta}')¨>
                    </div>
                </div>
                <div  style=¨float: right¨>
                   {grid.filas.Count} de {grid.TotalEnBd}
                </div>
            </div>
            ";
            return htmlNavegadorGrid;
        }

        private static string RenderOpcionesGrid()
        {
            var htmlOpcionesGrid = "";
            return htmlOpcionesGrid;
        }

        private static string RenderizarGrid(Grid grid)
        {
            var htmlTabla = $@"<table id=¨{grid.Id}¨ 
                                      class=¨table table-striped table-hover¨ 
                                      width=¨100%¨
                                      seleccionables ={ grid.Seleccionables}
                                      seleccionados =¨¨>{Environment.NewLine}" +
                            $"   {RenderCabecera(grid.Id, grid.columnas)}{Environment.NewLine}" +
                            $"   {RenderDetalleGrid(grid.Id, grid.filas)}" +
                            $"</table>";
            var htmlNavegador = grid.ConNavegador ? RenderNavegadorGrid(grid) : "";
            return (htmlTabla + htmlNavegador + RenderOpcionesGrid());
        }

    }
}