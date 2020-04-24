using System;
using System.Text;
using System.Collections.Generic;
using System.Text.Json;
using MVCSistemaDeElementos.Descriptores;
using Gestor.Elementos.ModeloIu;

namespace UtilidadesParaIu
{
    public class Grid<TElemento>
    {
        public string Id { get; private set; }

        public ZonaDeDatos<TElemento> ZonaDeDatos { get; set; }

        public string IdHtml => Id.ToLower();

        public string IdHtmlCabecera => $"{IdHtml}_c_tr_0";

        public string IdHtmlTabla => $"{IdHtml}_table";
        public string IdHtmlNavegador => $"{IdHtml}_nav";
        public string IdHtmlNavegador_1 => $"{IdHtmlNavegador}_1";
        public string IdHtmlNavegador_2 => $"{IdHtmlNavegador}_2";
        public string IdHtmlNavegador_3 => $"{IdHtmlNavegador}_3";
        public string IdHtmlPorLeer => $"{IdHtmlNavegador_2}_reg";

        public string Controlador => ZonaDeDatos.Mnt.Crud.Controlador;
        public List<ColumnaDelGrid<TElemento>> columnas { get; private set; } = new List<ColumnaDelGrid<TElemento>>();
        public List<FilaDelGrid<TElemento>> filas { get; private set; } = new List<FilaDelGrid<TElemento>>();

        public int NumeroDeFilas => filas.Count;

        public int TotalEnBd => ZonaDeDatos.TotalEnBd;
        private int PosicionInicial => ZonaDeDatos.PosicionInicial;
        private int CantidadPorLeer => ZonaDeDatos.CantidadPorLeer;
        public int Seleccionables { get; set; }
        public int Ultimo_Leido => PosicionInicial + filas.Count;

        public bool ConSeleccion { get; set; } = true;
        public bool ConNavegador { get; set; } = true;
        public ModeloGrid Modelo { get; private set; } = ModeloGrid.Propio;

        public Grid(ZonaDeDatos<TElemento> zonaDeDatos)
        {
            ZonaDeDatos = zonaDeDatos;
            Id = ZonaDeDatos.Id;
            Seleccionables = 2;
        }

        public FilaDelGrid<TElemento> ObtenerFila(int i)
        {
            return filas[i];
        }


        public string ToHtml()
        {
            ZonaDeDatos.CalcularAnchosColumnas();
            return RenderizarGrid(this).Render();
        }

        private static string RenderColumnaCabecera(ColumnaDelGrid<TElemento> columna)
        {
            var visible = columna.Visible ? "visibility: visible;" : "visibility: hidden";
            var ancho = columna.PorAncho == 0 ? "" : $"width:{columna.PorAncho}%";
            var estilo =  $"style=¨{ancho}¨;"; 

            columna.descriptor.visible = visible;
            columna.descriptor.alineada = columna.AlineacionCss;

            var descriptor = $"descriptor={JsonSerializer.Serialize(columna.descriptor)}";

            var htmlRef = columna.Ordenar ? $@"<a href=¨javascript:Crud.EjecutarMenuMnt('ordenarpor','{columna.IdHtml}')¨  
                                                 class=¨ordenada-sin-orden¨>{columna.Titulo} 
                                                </a>"
                    : $"<a>{columna.Titulo}</a>";

            var htmlTh = $@"{Environment.NewLine}<th id = ¨{columna.IdHtml}¨ 
                                               class=¨columna-cabecera {columna.AlineacionCss}¨ 
                                               propiedad = ¨{columna.Propiedad.ToLower()}¨
                                               modo-ordenacion=¨sin-orden¨ 
                                               {estilo} 
                                               {visible}
                                               {descriptor}>
                                               {htmlRef}
                                           </td>";
            return htmlTh;
        }

        private static string RenderColumnaDeSeleccion(string idGrid)
        {
            var visible = "";
            var ancho = "";
            var estilo = visible + ancho == "" ? "" : $"{ancho} {visible}";
            var columna = new ColumnaDelGrid<TElemento>();
            columna.Propiedad = idGrid + "_chk_sel";
            columna.Titulo = " ";
            columna.descriptor.visible = visible;
            columna.descriptor.alineada = HtmlRender.AlineacionCss(Aliniacion.centrada);
            columna.descriptor.valor = "CrearCheck";

            var descriptor = $"descriptor={JsonSerializer.Serialize(columna.descriptor)}";

            return $"{Environment.NewLine}<th scope=¨col¨ id= ¨{columna.descriptor.id}¨ class=¨{columna.AlineacionCss}¨ {estilo} {descriptor}>{columna.Titulo}</th>";
        }

        private static string RenderCeldaCheck(string idGrid, string idFila, int numCol)
        {

            var idDelTd = $"{idFila}.{numCol}";
            var nombreTd = $"td.chksel.{idGrid}";

            var idDelCheck = $"{idFila}.chksel";
            var nombreCheck = $"chksel.{idGrid}";

            var check = $@"<input type=¨checkbox¨ 
                                  id=¨{idDelCheck}¨ 
                                  name=¨{nombreCheck}¨ 
                                  class=¨text-center¨ 
                                  aria-label=¨Marcar para seleccionar¨
                                  onclick=¨Crud.AlPulsarUnCheckDeSeleccion('{idGrid}','{idDelCheck}');¨ /> ";



            var tdDelCheck = $@"<td id=¨{idDelTd}¨ 
                                       name=¨{nombreTd}¨ 
                                       class=¨{HtmlRender.AlineacionCss(Aliniacion.centrada)}¨>{Environment.NewLine}" +
                             $@"  {check}{Environment.NewLine}" +
                             $@"</td>";

            return tdDelCheck;
        }

        private static string RenderCeldaInput(CeldaDelGrid<TElemento> celda)
        {
            var editable = !celda.Editable ? "readonly" : "";


            var idDelTd = $"{celda.idTdHtml}";
            var nombreTd = $"td.{celda.Propiedad}.{celda.Fila.Datos.IdHtml}".ToLower(); // idGrid}"

            var idDelInput = $"{celda.idHtml}";
            var nombreInput = $"{celda.Propiedad}.{celda.Fila.Datos.IdHtml}".ToLower(); // idGrid}"


            var input = $" <input id=¨{idDelInput}¨ " +
                        $"        name=¨{nombreInput}¨ " +
                        $"        class=¨{celda.AlineacionCss()}¨ " +
                        $"        style=¨width:100%; border:0¨ " +
                        $"        {editable} " +
                        $"        value=¨{celda.Valor}¨/>";


            var ocultar = celda.Visible ? "" : "hidden";

            return $@"<td id=¨{idDelTd}¨ 
                          name=¨{nombreTd}¨ 
                          class=¨{celda.AlineacionCss()}¨ {ocultar}>" +
                   $"   {input}" +
                   $" </td>";
        }

        private static string RenderFila(FilaDelGrid<TElemento> fila)
        {
            var filaHtml = new StringBuilder();
            var numCol = 0;
            for(var j= 0; j < fila.NumeroDeCeldas; j++)
            {
                var celda = fila.ObtenerCelda(j);
                filaHtml.AppendLine(RenderCeldaInput(celda));
                numCol++;
            }
            return $@"{filaHtml}";
        }

        private static string RenderFilaSeleccionable(FilaDelGrid<TElemento> fila)
        {
            string filaHtml = RenderFila(fila);
            string celdaDelCheck = RenderCeldaCheck(fila.Datos.IdHtml, fila.IdHtml, fila.NumeroDeCeldas);

            return $"<tr id='{fila.IdHtml}'>{Environment.NewLine}" +
                   $"   {filaHtml}{celdaDelCheck}{Environment.NewLine}" +
                   $"</tr>{Environment.NewLine}";
        }

        private static string RenderCabecera(Grid<TElemento> grid)
        {
            var cabeceraHtml = new StringBuilder();
            foreach (var columna in grid.columnas)
            {
                cabeceraHtml.Append(RenderColumnaCabecera(columna));
            }
            cabeceraHtml.Append(RenderColumnaDeSeleccion(grid.IdHtml)); ; //RenderCeldaCheck($"{idGrid}", $"chk");
            return $@"<thead id='{grid.IdHtml}_cabecera'>{Environment.NewLine}
                         <tr id=¨{grid.IdHtmlCabecera}¨>
                            {cabeceraHtml}{Environment.NewLine}
                         </tr>{Environment.NewLine}
                      </thead>";
        }

        private static string RenderDetalleGrid(Grid<TElemento> grid)
        {
            var htmlDetalleGrid = new StringBuilder();
            for(var i= 0; i< grid.NumeroDeFilas; i++)
            {
                var fila = grid.ObtenerFila(i);
                htmlDetalleGrid.Append(RenderFilaSeleccionable(fila));
            }
            return $@"<tbody id='{grid.IdHtml}_detalle'>
                         {htmlDetalleGrid}
                      </tbody>";
        }

        private static string RenderNavegadorGrid(Grid<TElemento> grid)
        {

            var accionSiguiente = grid.ZonaDeDatos.Mnt.Crud.Modo == ModoDescriptor.Seleccion
                ? $"LeerSiguientes('{grid.IdHtml}')"
                : $"Crud.EjecutarMenuMnt('obtenersiguientes')";

            var accionBuscar = grid.ZonaDeDatos.Mnt.Crud.Modo == ModoDescriptor.Seleccion
                ? $"Leer('{grid.IdHtml}')"
                : $"Crud.EjecutarMenuMnt('buscar')";

            var accionAnterior = grid.ZonaDeDatos.Mnt.Crud.Modo == ModoDescriptor.Seleccion
                ? $"LeerAnteriores('{grid.IdHtml}')"
                : $"Crud.EjecutarMenuMnt('obteneranteriores')";

            var accionUltimos = grid.ZonaDeDatos.Mnt.Crud.Modo == ModoDescriptor.Seleccion
                ? $"LeerUltimos('{grid.IdHtml}')"
                : $"Crud.EjecutarMenuMnt('obtenerultimos')";

            var htmlNavegadorGrid = $@"
            <div id= ¨{grid.IdHtml}_pie¨ class=¨pie-grid¨>
                <div id=¨{grid.IdHtmlNavegador}¨ class = ¨navegador-grid¨>
                    <div id=¨{grid.IdHtmlNavegador_1}¨ data-type=¨img¨>
                        <img src=¨/images/paginaInicial.png¨ alt=¨Primera página¨ title=¨Ir al primer registro¨ onclick=¨{accionBuscar}¨>
                    </div>
                    <div id=¨{grid.IdHtmlNavegador_2}¨>
                        <input type=¨number¨ 
                               id=¨{grid.IdHtmlPorLeer}¨ 
                               class = ¨cantidad-grid¨
                               value=¨{grid.CantidadPorLeer}¨ 
                               min=¨1¨ step=¨1¨ max=¨999¨ 
                               posicion=¨{grid.Ultimo_Leido}¨  
                               controlador=¨{grid.Controlador}¨  
                               totalEnBd=¨{grid.TotalEnBd}¨ 
                               title=¨leidos {grid.filas.Count} desde la posición {grid.PosicionInicial}¨ />
                    </div>
                    <div id=¨id=¨{grid.IdHtmlNavegador_3}¨ data-type=¨img¨ >
                        <img src=¨/images/paginaAnterior.png¨ alt=¨Primera página¨ title=¨Página anterior¨ onclick=¨{accionAnterior}¨>
                        <img src=¨/images/paginaSiguiente.png¨ alt=¨Siguiente página¨ title=¨Página siguiente¨ onclick=¨{accionSiguiente}¨>
                        <img src=¨/images/paginaUltima.png¨ alt=¨Última página¨ title=¨Última página¨ onclick=¨{accionUltimos}¨>
                    </div>
                </div>
                <div id= ¨{grid.IdHtml}_info¨ class=¨info-grid¨>
                   {grid.filas.Count} desde la posición {grid.PosicionInicial}
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

        private static string RenderizarGrid(Grid<TElemento> grid)
        {
            var htmlTabla = $@"<table id=¨{grid.IdHtmlTabla}¨ 
                                      class=¨tabla-grid¨ 
                                      width=¨100%¨>{Environment.NewLine}" +
                            $"   {RenderCabecera(grid)}{Environment.NewLine}" +
                            $"   {RenderDetalleGrid(grid)}" +
                            $"</table>";
            var htmlNavegador = grid.ConNavegador ? RenderNavegadorGrid(grid) : "";
            return (htmlTabla + htmlNavegador + RenderOpcionesGrid());
        }

    }
}