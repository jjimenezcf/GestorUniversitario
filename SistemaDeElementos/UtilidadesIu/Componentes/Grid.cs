using System;
using System.Text;
using System.Collections.Generic;
using MVCSistemaDeElementos.Descriptores;
using Utilidades;
using ModeloDeDto;
using System.Reflection.Metadata.Ecma335;

namespace UtilidadesParaIu
{
    public class Grid<TElemento> where TElemento : ElementoDto
    {
        public string Id { get; private set; }

        public ZonaDeDatos<TElemento> ZonaDeDatos { get; private set; }

        public string IdHtml => Id.ToLower();

        public string IdHtmlCabeceraDeTabla => $"{IdHtml}_cabecera";

        public string IdHtmlFilaCabecera => $"{IdHtml}_c_tr_0";

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
            return RenderizarGrid(this);
        }

        public string NavegadorToHtml()
        {
           return RenderNavegadorGrid(this) ;
        }

        private static string RenderColumnaCabecera(ColumnaDelGrid<TElemento> columna)
        {
            var porcentaje = columna.ZonaDeDatos.Mnt.Crud.Modo == ModoDescriptor.SeleccionarParaFiltrar
                          || columna.ZonaDeDatos.Mnt.Crud.Modo == ModoDescriptor.Relacion
                          || columna.ZonaDeDatos.Mnt.Crud.Modo == ModoDescriptor.Consulta
                ? columna.PorAnchoSel
                : columna.PorAnchoMnt;

            var atributosDelEstilo = $"text-align: {columna.AlineacionCss};";
            atributosDelEstilo = $"width: {porcentaje}%; {atributosDelEstilo}";
            string htmlRef = columna.ConOrdenacion 
                ? RenderAccionOrdenar(columna) 
                : columna.Titulo;

            string claseCss = columna.Visible ? Css.Render(enumCssGrid.ColumnaCabecera) : Css.Render(enumCssGrid.ColumnaOculta);
            string claseCssAlineacion = columna.Alineada == Aliniacion.derecha ? $"class=¨{Css.Render(enumCssGrid.ColumnaAlineadaDerecha)}¨" : "";

            var htmlTh = $@"{Environment.NewLine}
                          <th scope=¨col¨
                              id = ¨{columna.IdHtml}¨ 
                              class=¨{claseCss}¨ 
                              propiedad = ¨{columna.Propiedad.ToLower()}¨
                              modo-ordenacion=¨{(columna.cssOrdenacion == enumCssOrdenacion.SinOrden ? $"{enumModoOrdenacion.sinOrden.Render()}": $"{enumModoOrdenacion.ascendente.Render()}")}¨ 
                              style = ¨{atributosDelEstilo}¨
                              alineacion=¨{columna.AlineacionCss}¨
                              ordenar-por = ¨{columna.OrdenarPor}¨>
                              <div {claseCssAlineacion}>
                                {htmlRef}
                              </div>
                          </th>";
            return htmlTh;
        }

        private static string RenderAccionOrdenar(ColumnaDelGrid<TElemento> columna)
        {

            var gestorDeEventos = RenderGestorDeEventos(columna.ZonaDeDatos.Mnt.Crud.Modo);

            var parametros = $"{columna.IdHtml}";
            if (columna.ZonaDeDatos.Mnt.Crud.Modo == ModoDescriptor.SeleccionarParaFiltrar ||
                columna.ZonaDeDatos.Mnt.Crud.Modo == ModoDescriptor.Relacion ||
                columna.ZonaDeDatos.Mnt.Crud.Modo == ModoDescriptor.Consulta)
            {
                parametros = $"{columna.ZonaDeDatos.IdHtmlModal}#{parametros}";
            }

            string htmlRef = $"href =¨javascript: Crud.{gestorDeEventos}('ordenar-por', '{parametros}')¨";

            return $@"<a {htmlRef} class=¨{Css.Render(columna.cssOrdenacion)}¨>{columna.Titulo}</a>";
        }

        private static string RenderEventoPuslsa(CeldaDelGrid<TElemento> celda, string idControlHtml)
        {
            var getorDeEventos = RenderGestorDeEventos(celda.Fila.Datos.Mnt.Crud.Modo);

            var parametros = $"{celda.Fila.idHtmlCheckDeSeleccion}#{idControlHtml}";
            if (celda.Fila.Datos.Mnt.Crud.Modo == ModoDescriptor.SeleccionarParaFiltrar ||
                celda.Fila.Datos.Mnt.Crud.Modo == ModoDescriptor.Relacion ||
                celda.Fila.Datos.Mnt.Crud.Modo == ModoDescriptor.Consulta)
            {
                parametros = $"{celda.Fila.Datos.IdHtmlModal}#{parametros}";
            }

            return $"Crud.{getorDeEventos}('{TipoDeAccionDeMnt.FilaPulsada}', '{parametros}');";
        }

        private static string RenderGestorDeEventos(ModoDescriptor modo)
        {
            var getorDeEventos = $"{GestorDeEventos.EventosDelMantenimiento}"; 
            if (modo == ModoDescriptor.SeleccionarParaFiltrar)
            {
                getorDeEventos = $"{GestorDeEventos.EventosModalDeSeleccion}";
            }
            if (modo == ModoDescriptor.ParaSeleccionar)
            {
                getorDeEventos = $"{GestorDeEventos.EventosModalParaSeleccionar}";
            }
            if (modo == ModoDescriptor.Relacion)
            {
                getorDeEventos = $"{GestorDeEventos.EventosModalDeCrearRelaciones}";
            }
            if (modo == ModoDescriptor.Consulta)
            {
                getorDeEventos = $"{GestorDeEventos.EventosModalDeConsultaDeRelaciones}";
            }
            return getorDeEventos;
        }


        private static string RenderTd(CeldaDelGrid<TElemento> celda)
        {

            var nombreTd = $"td.{celda.Propiedad}.{celda.Fila.Datos.IdHtml}".ToLower();
            string pulsarCheck = RenderEventoPuslsa(celda, celda.idHtmlTd);

            var onclickTd = $"onclick=¨{pulsarCheck}¨";
            var ocultar = celda.Visible ? "" : "hidden";

            var tdHtml = $@"<td id=¨{celda.idHtmlTd}¨ 
                                name=¨{nombreTd}¨ 
                                style=¨text-align: {celda.AlineacionCss()};¨ 
                                propiedad=¨{celda.Propiedad}¨ 
                                {onclickTd} 
                                {ocultar} >
                                {RenderCeldaDelTd(celda)}
                           </td>";
            return tdHtml;
        }

        private static string RenderCeldaDelTd(CeldaDelGrid<TElemento> celda)
        {
            var idDelInput = $"{celda.idHtml}";
            string pulsarCheck = RenderEventoPuslsa(celda, idDelInput);

            var tipoHtml = celda.Tipo == typeof(bool) ? "type =¨checkbox¨" : "type =¨text¨";
            var onclick = celda.Tipo == typeof(bool)
                  ? $"onclick=¨{pulsarCheck}¨"
                  : "";


            var editable = !celda.Editable ? "readonly" : "";

            var nombreInput = $"{celda.Propiedad}.{celda.Fila.Datos.IdHtml}".ToLower();

            var input = $" <input {tipoHtml} id=¨{idDelInput}¨ " +
            $"        name=¨{nombreInput}¨ " +
            $"        style=¨width:100%; border:0; text-align: {celda.AlineacionCss()};¨ " +
            $"        propiedad=¨{celda.Propiedad}¨ " +
            $"        style=¨width:100%; border:0¨ " +
            $"        {editable} " +
            $"        {onclick} " +
            $"        value=¨{celda.Valor}¨ />";

            return input;
        }


        private static string RenderFila(FilaDelGrid<TElemento> fila)
        {
            var filaHtml = new StringBuilder();
            var numCol = 0;
            for (var j = 0; j < fila.NumeroDeCeldas; j++)
            {
                var celda = fila.ObtenerCelda(j);
                filaHtml.AppendLine(RenderTd(celda));
                numCol++;
            }
            return $@"{filaHtml}";
        }

        private static string RenderFilaSeleccionable(FilaDelGrid<TElemento> fila)
        {
            string celdaDelCheck = ""; // RenderCeldaCheck(fila.Datos.IdHtml, fila.IdHtml, fila.NumeroDeCeldas);
            string filaHtml = RenderFila(fila);

            return $"<tr id='{fila.IdHtml}'>{Environment.NewLine}" +
                   $"   {celdaDelCheck}{filaHtml}{Environment.NewLine}" +
                   $"</tr>{Environment.NewLine}";
        }

        private static string RenderCabecera(Grid<TElemento> grid)
        {
            var cabeceraHtml = new StringBuilder();
            foreach (var columna in grid.columnas)
            {
                cabeceraHtml.Append(RenderColumnaCabecera(columna));
            }

            return $@"<thead id='{grid.IdHtmlCabeceraDeTabla}' class=¨{Css.Render(enumCssCuerpo.CuerpoDatosGridThead)}¨ >{Environment.NewLine}
                         <tr id=¨{grid.IdHtmlFilaCabecera}¨>
                            {cabeceraHtml}
                         </tr>
                      </thead>";

        }

        private static string AplicarCss(bool esModal,enumCssNavegadorEnModal claseCss)
        {
            if (esModal)
            {
                return Css.Render(claseCss);
            }
            else
            {
                switch (claseCss)
                {
                    case enumCssNavegadorEnModal.InfoGrid: return Css.Render(enumCssNavegadorEnMnt.InfoGrid);
                    case enumCssNavegadorEnModal.Mensaje: return Css.Render(enumCssNavegadorEnMnt.Mensaje);
                    case enumCssNavegadorEnModal.Cantidad: return Css.Render(enumCssNavegadorEnMnt.Cantidad);
                    case enumCssNavegadorEnModal.Opcion: return Css.Render(enumCssNavegadorEnMnt.Opcion);
                    case enumCssNavegadorEnModal.Navegador: return Css.Render(enumCssNavegadorEnMnt.Navegador);
                }

            }
            throw new Exception($"No se ha definido la clase a aplicar a para {claseCss} del enumerado del navegador");
        }

        private static string RenderNavegadorGrid(Grid<TElemento> grid)
        {
            var gestorDeEventos = RenderGestorDeEventos(grid.ZonaDeDatos.Mnt.Crud.Modo);
            var parametros = grid.ZonaDeDatos.Mnt.Crud.Modo == ModoDescriptor.Mantenimiento
                ? ""
                : $"{grid.ZonaDeDatos.IdHtmlModal}";

            var accionUltimos = $"Crud.{gestorDeEventos}('obtener-ultimos','{parametros}')";
            var accionBuscar = $"Crud.{gestorDeEventos}('buscar-elementos','{parametros}')";
            var accionAnterior = $"Crud.{gestorDeEventos}('obtener-anteriores','{parametros}')";
            var accionSiguiente = $"Crud.{gestorDeEventos}('obtener-siguientes','{parametros}')";
            var esModal = grid.ZonaDeDatos.Mnt.Crud.EsModal;

            var htmlContenedorNavegador = esModal
                ?$@"
                   <!-- ***************** Navegador del grid ****************** -->
                   <div id= ¨{grid.IdHtml}_pie¨ class=¨{Css.Render(enumCssNavegadorEnModal.Contenedor)}¨>
                     htmlNavegadorGrid
                   </div>
                 "
                 :"htmlNavegadorGrid" ;


            var htmlNavegadorGrid = $@"
            <div id=¨{grid.IdHtmlNavegador}¨ class=¨{AplicarCss(esModal,enumCssNavegadorEnModal.Navegador)}¨>
                 <div id=¨{grid.IdHtmlNavegador_1}¨ data-type=¨img¨>
                        <img src=¨/images/paginaInicial.png¨ alt=¨Primera página¨ title=¨Ir al primer registro¨ onclick=¨{accionBuscar}¨>
                 </div>
                 <div id=¨{grid.IdHtmlNavegador_2}¨>
                        <input type=¨number¨ 
                               id=¨{grid.IdHtmlPorLeer}¨ 
                               class = ¨{AplicarCss(esModal, enumCssNavegadorEnModal.Cantidad)}¨
                               value=¨{grid.CantidadPorLeer}¨ 
                               min=¨1¨ step=¨1¨ max=¨999¨ 
                               pagina=¨1¨  
                               posicion=¨{grid.Ultimo_Leido}¨  
                               controlador=¨{grid.Controlador}¨  
                               total-en-bd=¨{grid.TotalEnBd}¨ 
                               title=¨Pagina: 1 de un total de {Math.Ceiling((decimal)(grid.TotalEnBd / grid.CantidadPorLeer))}¨ />
                 </div>
                 <div id=¨id=¨{grid.IdHtmlNavegador_3}¨ data-type=¨img¨ >
                        <img src=¨/images/paginaAnterior.png¨ alt=¨Primera página¨ title=¨Página anterior¨ onclick=¨{accionAnterior}¨>
                        <img src=¨/images/paginaSiguiente.png¨ alt=¨Siguiente página¨ title=¨Página siguiente¨ onclick=¨{accionSiguiente}¨>
                        <img src=¨/images/paginaUltima.png¨ alt=¨Última página¨ title=¨Última página¨ onclick=¨{accionUltimos}¨>
                 </div>
            </div>
            <div id = ¨div.seleccion.{grid.IdHtml}¨ class=¨{AplicarCss(esModal, enumCssNavegadorEnModal.Opcion)}¨>     
              {RenderOpcionesGrid(grid.IdHtml, gestorDeEventos, parametros)}
            </div>
            <div id= ¨{grid.IdHtml}_mensaje¨ class=¨{AplicarCss(esModal, enumCssNavegadorEnModal.Mensaje)}¨>
               Seleccionadas: 0 de {grid.TotalEnBd}
            </div>
            <div id= ¨{grid.IdHtml}_info¨ class=¨{AplicarCss(esModal, enumCssNavegadorEnModal.InfoGrid)}¨>
               Pagina: 1 de un total de {Math.Ceiling((decimal)(grid.TotalEnBd / grid.CantidadPorLeer))}
            </div>
            ";
            return htmlContenedorNavegador.Replace("htmlNavegadorGrid", htmlNavegadorGrid);
        }

        private static string RenderOpcionesGrid(string IdHtmlGrid, string gestorDeEventos, string parametros)
        {
            var htmlOpcionesGrid = @$"             

            <nav class='menu-del-grid'>
                <ul>
                    <li class='menu-del-grid-li'>
                        <center><a href='#'>Opciones</a></center>
                        <ul>
                            <li class='menu-del-grid-li-li' style='padding-bottom: 0px;'><a href='#'>Buscar</a></li>
                            <li class='menu-del-grid-li-li' style='padding-bottom: 0px;'>
                                <a href='#'>Seleccionar</a>
                                <ul>
                                    <li class='menu-del-grid-li-li' style='padding-bottom: 0px;'>
                                        <a href=¨javascript:Crud.{GestorDeEventos.EventosMenuDelGrid}('{TipoDeAccionDeMnt.SeleccionarTodo}', '{IdHtmlGrid}');¨>seleccionar todo</a>
                                    </li>
                                    <li class='menu-del-grid-li-li' style='padding-bottom: 0px;'>
                                        <a href=¨javascript:Crud.{GestorDeEventos.EventosMenuDelGrid}('{TipoDeAccionDeMnt.AnularSeleccion}', '{IdHtmlGrid}');¨>anular selección</a>
                                    </li>
                                    <li class='menu-del-grid-li-li'>
                                        <a id='div.seleccion.{IdHtmlGrid}.ref' href=¨javascript:Crud.{gestorDeEventos}('{TipoDeAccionDeMnt.MostrarSoloSeleccionadas}', '{parametros}');¨>Seleccionadas</a>
                                        <input id=¨div.seleccion.{IdHtmlGrid}.input¨ type=¨hidden¨ value=¨0¨ > 
                                    </li>
                                </ul>
                            <li class='menu-del-grid-li-li'>
                                <a href='#'>Ordenación</a>
                                <ul>
                                    <li class='menu-del-grid-li-li' style='padding-bottom: 0px;'>
                                        <a href=¨javascript:Crud.{GestorDeEventos.EventosMenuDelGrid}('{TipoDeAccionDeMnt.AnularOrden}', '{IdHtmlGrid}');¨>anular orden</a>
                                    </li>
                                    <li class='menu-del-grid-li-li'>
                                        <a href=¨javascript:Crud.{GestorDeEventos.EventosMenuDelGrid}('{TipoDeAccionDeMnt.AplicarOrdenInicial}', '{IdHtmlGrid}');¨>orden inicial</a>
                                    </li>
                                </ul>
                            <li>
                        </ul>
                    </li>
                </ul>
            </nav>

            ";

            /*
             * 
             *        
              <a id = ¨div.seleccion.{IdHtmlGrid}.ref¨ href=¨javascript:Crud.{gestorDeEventos}('{TipoDeAccionDeMnt.MostrarSoloSeleccionadas}', '{parametros}');¨>Seleccionadas</a>
              <input id=¨div.seleccion.{IdHtmlGrid}.input¨ type=¨hidden¨ value=¨0¨ >  
             * 
             * */

            return htmlOpcionesGrid;
        }

        private static string RenderizarGrid(Grid<TElemento> grid)
        {
            var htmlTabla = $@"<table id=¨{grid.IdHtmlTabla}¨ class=¨cuerpo-datos-tabla table table-striped¨ style=¨margin-bottom: 0px;¨>
                                    {RenderCabecera(grid)}
                               </table>
                             ";

            var htmlGrid = grid.ZonaDeDatos.Mnt.Crud.EsModal ? htmlTabla + RenderNavegadorGrid(grid) : htmlTabla;

            return htmlGrid;
        }


    }
}