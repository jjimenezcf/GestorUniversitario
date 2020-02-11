
using System;
using System.Collections.Generic;
using Gestor.Elementos.ModeloIu;
using UniversidadDeMurcia.Descriptores;
using Utilidades;

namespace UtilidadesParaIu
{
    public enum Aliniacion { no_definida, izquierda, centrada, derecha, justificada };

    public enum ModeloGrid { Tabulator, Propio };

    public static class HtmlRender
    {
        public static string Render(this string cadena)
        {
            while (cadena.IndexOf("< ") >= 0)
                cadena = cadena.Replace("< ", "<");

            return cadena.Replace("¨", "\"");
        }

        public static string AlineacionCss(Aliniacion alineacion)
        {
            switch (alineacion)
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

        public static string AlineacionTabulator(Aliniacion alineada)
        {
            switch (alineada)
            {
                case Aliniacion.izquierda:
                    return "left";
                case Aliniacion.derecha:
                    return "right";
                case Aliniacion.centrada:
                    return "center";
                case Aliniacion.justificada:
                    return "justify";
                default:
                    return "left";
            }
        }

        public static string RenderCrud(DescriptorDeCrud crud)
        {
            var htmlCrud =
                   RenderTitulo(crud) + Environment.NewLine +
                   RenderOpcionesMenu(crud.Menu) + Environment.NewLine +
                   RenderFiltro(crud.Filtro) + Environment.NewLine +
                   RenderModalesFiltro(crud.Filtro) + Environment.NewLine +
                   RenderGrid(crud.Grid) + Environment.NewLine +
                   RenderPie();

            return htmlCrud.Render();                   
        }

        private static string RenderTitulo(DescriptorDeCrud crud)
        {
            var htmlCabecera = $"<h2>{crud.Titulo}</h2>";
            return htmlCabecera;
        }

        public static string RenderModalesFiltro(ZonaDeFiltro filtro)
        {
            var htmlModalesEnFiltro = "";
            foreach (Bloque b in filtro.Bloques)
                htmlModalesEnFiltro = $"{htmlModalesEnFiltro}{(htmlModalesEnFiltro.IsNullOrEmpty() ? "" : Environment.NewLine)}{RenderModalesBloque(b)}";

            return htmlModalesEnFiltro;
        }

        private static object RenderModalesBloque(Bloque b)
        {
            var htmlModalesEnBloque = "";
            foreach (Control c in b.Controles)
            {
                if (c.Tipo == TipoControl.Selector)
                    htmlModalesEnBloque = $"{htmlModalesEnBloque}{(htmlModalesEnBloque.IsNullOrEmpty() ? "" : Environment.NewLine)}{RenderModal((Selector)c)}";

            }
            return htmlModalesEnBloque;
        }
        private static string RenderSelector(Selector s)
        {
            return $@"<div class=¨input-group mb-3¨>
                       <input id=¨{s.Id}¨ type = ¨text¨ class=¨form-control¨ placeholder=¨{s.Ayuda}¨>
                       <div class=¨input-group-append¨>
                            <button class=¨btn btn-outline-secondary¨ type=¨button¨ data-toggle=¨modal¨ data-target=¨#{s.Modal.Id}¨ >Seleccionar</button>
                       </div>
                    </div>
                  ";
        }
        private static object RenderModal(Selector s)
        {

            const string _htmlModalSelector =
            @"
             <div class=¨modal fade¨ id=¨idModal¨ tabindex=¨-1¨ role=¨dialog¨ aria-labelledby=¨exampleModalLabel¨ aria-hidden=¨true¨>
               <div class=¨modal-dialog¨ role=¨document¨>
                 <div class=¨modal-content¨>
                   <div class=¨modal-header¨>
                     <h5 class=¨modal-title¨ id=¨exampleModalLabel¨>titulo</h5>
                   </div>
                   <div id=¨{idContenedor}¨ class=¨modal-body¨>
                     {gridDeElementos}
                   </div>
                   <div class=¨modal-footer¨>
                     <button type = ¨button¨ class=¨btn btn-secondary¨ data-dismiss=¨modal¨>Cerrar</button>
                     <button type = ¨button¨ class=¨btn btn-primary¨ data-dismiss=¨modal¨ onclick=¨AlSeleccionar('{idSelector}', '{idGrid}', '{referenciaChecks}')¨>Seleccionar</button>
                   </div>
                 </div>
               </div>
             </div>
             <script>
               AlAbrirLaModal
               AlCerrarLaModal
             </script>
             ";


            var nombreCheckDeSeleccion = $"chksel.{s.Id}";

            return _htmlModalSelector
                    .Replace("idModal", s.Modal.Id)
                    .Replace("titulo", s.Modal.Ayuda)
                    .Replace("{idSelector}", s.Id)
                    //.Replace("{idGrid}", IdGrid)
                    .Replace("{referenciaChecks}", $"{nombreCheckDeSeleccion}")
                    .Replace("{columnaId}", s.propiedadParaFiltrar)
                    .Replace("{columnaMostrar}", s.propiedadParaMostrar)
                    .Replace("{idContenedor}", $"contenedor.{s.Modal.Id}")
                    .Replace("{gridDeElementos}", "")
                    .Replace("AlAbrirLaModal", "")
                    .Replace("AlCerrarLaModal","")
                    .Render();
        }


        private static string RenderPie()
        {
            return "";
        }

        private static string RenderGrid(ZonaDeGrid grid)
        {
            return "";
        }

        private static string RenderFiltro(ZonaDeFiltro filtro)
        {
            var htmlFiltro = $@"<div id = ¨{filtro.Id}¨ style=¨width:100%¨>     
                                     bloques 
                                </div>";

            var htmlBloques = "";
            foreach (Bloque b in filtro.Bloques)
                htmlBloques = $"{htmlBloques}{(htmlBloques.IsNullOrEmpty() ? "" : Environment.NewLine)}{RenderBloque(b)}";

            return htmlFiltro.Replace("bloques", htmlBloques);
        }

        private static string RenderBloque(Bloque bloque)
        {
            string htmlBloque = $@"<div id = ¨{bloque.Id}¨>     
                                     tabla 
                                    </div>";
            string htmlTabla = RenderTabla(bloque.Tabla);

            return htmlBloque.Replace("tabla", htmlTabla);
        }

        private static string RenderTabla(TablaBloque tabla)
        {

            var htmlTabla = $@"<table id=¨{tabla.Id}¨ width=¨100%¨
                                  filas
                               </table>";
            var htmlFilas = "";
            for (var i = 0; i < tabla.Dimension.Filas; i++)
                htmlFilas = $"{htmlFilas}{(htmlFilas.IsNullOrEmpty() ? "" : Environment.NewLine)}{RenderFila(tabla, i)}";

            return htmlTabla.Replace("filas", htmlFilas);
        }

        private static string RenderFila(TablaBloque tabla, int i)
        {
            var idFila = $"{tabla.Id}_{i}";
            var htmlFila = $@"<tr id=¨{idFila}¨>
                                 columnas
                              </tr>";
            var htmlColumnas = "";
            for (var j = 0; j < tabla.Dimension.Columnas; j++)
                htmlColumnas = $"{htmlColumnas}{(htmlColumnas.IsNullOrEmpty() ? "" : Environment.NewLine)}{RenderColumnasControl(tabla, idFila, i, j)}";


            return htmlFila.Replace("columnas", htmlColumnas);
        }

        private static string RenderColumnasControl(TablaBloque tabla, string idFila, int i, int j)
        {
            var idColumna = $"{idFila}_{j}";
            var htmlColumnaEtiqueta = $@"<td id=¨{idColumna}_e¨ style=¨width:15%¨>
                                            etiqueta
                                         </td>";
            var htmlColumnaControl = $@"<td id=¨{idColumna}_c¨ style=¨width:35%¨>
                                           control
                                        </td>";
            var htmlControl = "";
            var htmlEtiqueta = "";
            foreach (Control c in tabla.Controles)
            {
                if (c.Posicion.fila == i && c.Posicion.columna == j)
                    htmlEtiqueta = $"{c.RenderLabel()}";

                if (c.Posicion.fila == i && c.Posicion.columna == j)
                    htmlControl = $"{c.RenderControl()}";
            }


            return htmlColumnaEtiqueta.Replace("etiqueta", htmlEtiqueta) + 
                   Environment.NewLine+
                   htmlColumnaControl.Replace("control", htmlControl);
        }

        private static string RenderControl(Control c)
        {
            switch (c.Tipo)
            {
                case TipoControl.Selector: return RenderSelector(((Selector)c));
                case TipoControl.Editor: return ((Editor)c).RenderInput();
            }
            throw new Exception($"El tipo {c.Tipo} de control no está definido");
        }



        private static object RenderEtiqueta(string etiqueta)
        {
            return etiqueta;
        }

        private static string RenderOpcionesMenu(ZonaDeOpciones menu)
        {
            var htmlRef = "<div id=¨id¨> <a href =¨/{ruta}/{accion}¨>{titulo}</a> </div>";
            var htmlOpciones = "";
            foreach(Opcion o in menu.Opciones)
            {
                htmlOpciones = htmlOpciones + htmlRef.Replace("{id}", o.Id).Replace("{ruta}", o.Ruta).Replace("{accion}", o.Accion).Replace("{titulo}", o.Titulo);
            }

            return htmlOpciones;
        }
    }
}
