
using System;
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
                cadena.Replace("< ", "<");

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
            return (RenderOpcionesMenu(crud.Menu) + Environment.NewLine +
                   RenderFiltro(crud.Filtro) + Environment.NewLine +
                   RenderGrid(crud.Grid) + Environment.NewLine +
                   RenderPie() + Environment.NewLine +
                   RenderModalesFiltro(crud.Filtro)).Render();
                   
        }

        private static string RenderModalesFiltro(ZonaDeFiltro filtro)
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

        private static object RenderModal(Selector s)
        {
            var idGrid = $"{s.Modal.Id}_grid";
            var nombreCheckDeSeleccion = $"chksel.{idGrid}";
            
            string _htmlModalSelector =
               $@"
             <div class=¨modal fade¨ id=¨{s.Modal.Id}¨ tabindex=¨-1¨ role=¨dialog¨ aria-labelledby=¨exampleModalLabel¨ aria-hidden=¨true¨>
               <div class=¨modal-dialog¨ role=¨document¨>
                 <div class=¨modal-content¨>
                   <div class=¨modal-header¨>
                     <h5 class=¨modal-title¨ id=¨{s.Modal.Id}_titulo¨>{s.Modal.Etiqueta}</h5>
                   </div>
                   <div id=¨{s.Modal.Id}_cuerpo¨ class=¨modal-body¨>
                     elementos
                   </div>
                   <div class=¨modal-footer¨>
                     <button type = ¨button¨ class=¨btn btn-secondary¨ data-dismiss=¨modal¨>Cerrar</button>
                     <button type = ¨button¨ class=¨btn btn-primary¨ data-dismiss=¨modal¨ onclick=¨AlSeleccionar('{s.Id}', '', '{nombreCheckDeSeleccion}')¨>Seleccionar</button>
                   </div>
                 </div>
               </div>
             </div>
             <script>
               {Js_AlAbrirModal(s)}
               {Js_AlCerrarModal(s)}
             </script>
             ";

            return _htmlModalSelector;
        }

        private static object Js_AlAbrirModal(Selector s)
        {
            var idGrid = $"{s.Modal.Id}_grid";
            const string _alAbrirLaModal = @"
                                         $('#{idModal}').on('show.bs.modal', function (event) {
                                            AlAbrir('{IdGrid}', '{idSelector}', '{columnaId}', '{columnaMostrar}')
                                          })
                                      ";
           return _alAbrirLaModal.Replace("{idModal}", s.Modal.Id)
                                 .Replace("{IdGrid}", idGrid)
                                 .Replace("{idSelector}", s.Id)
                                 .Replace("{columnaId}", s.propiedadParaFiltrar)
                                 .Replace("{columnaMostrar}", s.propiedadParaMostrar);
        }

        private static object Js_AlCerrarModal(Selector s)
        {
            var idGrid = $"{s.Modal.Id}_grid";
            var nombreCheckDeSeleccion = $"chksel.{idGrid}";
            const string _alCerrarLaModal = @"
                                         $('#{idModal}').on('hidden.bs.modal', function (event) {
                                            AlCerrar('{idModal}', '{idGrid}', 'referenciaChecks')
                                          })
                                      ";
            return _alCerrarLaModal.Replace("{idModal}", s.Modal.Id)
                                   .Replace("{idGrid}", idGrid)
                                   .Replace("referenciaChecks", nombreCheckDeSeleccion);
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
                    htmlEtiqueta = $"{RenderEtiqueta(c.Etiqueta)}";

                if (c.Posicion.fila == i && c.Posicion.columna == j)
                    htmlControl = $"{RenderControl(c)}";
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
            }
            throw new Exception($"El tipo {c.Tipo} de control no está definido");
        }

        private static string RenderSelector(Selector s)
        {
            return $@"<div class=¨input-group mb-3¨>
                       <input id=¨{s.Id}¨ type = ¨text¨ class=¨form-control¨ placeholder=¨{s.Ayuda}¨ aria-label=¨titulo¨ aria-describedby=¨basic-addon2¨>
                       <div class=¨input-group-append¨>
                            <button class=¨btn btn-outline-secondary¨ type=¨button¨ data-toggle=¨modal¨ data-target=¨#{s.Modal.Id}¨ >Seleccionar</button>
                       </div>
                    </div>
                  ";
        }

        private static object RenderEtiqueta(string etiqueta)
        {
            return etiqueta;
        }

        private static string RenderOpcionesMenu(ZonaDeOpciones menu)
        {
            return "";
        }
    }
}
