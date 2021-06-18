using System.Collections.Generic;
using Enumerados;

namespace MVCSistemaDeElementos.Descriptores
{
    public static class PlantillasHtml
    {

        private static string atributosComunesDeUnControl = @"id=¨[IdHtml]¨
                                       propiedad=¨[Propiedad]¨ 
                                       class=¨[Css]¨ 
                                       tipo=¨[Tipo]¨
                                       style=¨[Estilos]¨";

        private static string atributosComunesDeUnControlDto = $@"{atributosComunesDeUnControl}
                                       obligatorio=¨[Obligatorio]¨ 
                                       [Readonly]";

        private static string atributosComunesDeUnControlflt = $@"{atributosComunesDeUnControl}
                                       control-de-filtro=¨S¨";


        public static string Expansor = $@"
        <div id=¨[IdHtml]¨ tipo=¨span-de-controles¨ class=¨[cssClase]¨>
           <div id=¨[IdHtml]-cabecera¨ class=¨[cssCabecera]¨>[RenderCabeceraDelExpansor]</div>
           <div id=¨[IdHtml]-cuerpo¨ class=¨[cssCuerpo]¨>[RenderCuerpoDelExpansor]</div>
           <div id=¨[IdHtml]-pie¨ class=¨[cssPie]¨>[RenderPieDelExpansor]</div>
        </div>";

        public static string CabeceraExpansor = $@"	 
            <a id=¨mostrar.[IdHtml].ref¨
               class=¨[cssClase]¨
               href=¨javascript:[Evento];¨>
               bloque: [Titulo]
            </a>
            <input id=¨expandir.[IdHtml].input¨ type=¨hidden¨ value=¨1¨ />";

        //public static string ContenedorDelControlDelExpansor = $@"
        //     <div id=¨contenedor.[IdHtml].etiqueta¨ class=¨[cssClase]¨>[RenderEtiqueta]</div>
        //     <div id=¨contenedor.[IdHtml].control¨ class=¨[cssClase]¨>[RenderControl]</div>
        //";

        public static string ControlDelExpansor = $@"[RenderEtiqueta][RenderControl]";

        public static string Etiqueta = $@"
        <div id=¨etiqueta-[IdDeControl]-contenedor¨ name=¨contenedor-etiqueta¨ class=¨[CssContenedor]¨>
        <label id=¨etiqueta-[IdDeControl]¨ for=¨[IdDeControl]¨ class=¨[CssEtiqueta]¨>[Etiqueta]</label>
        </div>
        ";


        private static string listaDinamica = $@"<div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=¨[CssContenedor]¨>
                                  <input clase-elemento=¨[ClaseElemento]¨ 
                                         mostrar-expresion=¨[MostrarExpresion]¨
                                         como-buscar=¨[BuscarPor]¨
                                         criterio-de-filtro=¨[CriterioDeFiltro]¨
                                         longitud=¨[Longitud]¨ 
                                         cantidad-a-leer=¨[Cantidad]¨ 
                                         placeholder =¨[Placeholder]¨
                                         oninput=¨[OnInput]¨ 
                                         onchange=¨[OnChange]¨ 
                                         [RestoDeAtributos]
                                         list=¨[IdHtml]-lista¨>
                                         <datalist id=¨[IdHtml]-lista¨>
                                         </datalist>
                             </div>";


        public static string listaDinamicaFlt = $@"{listaDinamica}".Replace("[RestoDeAtributos]", atributosComunesDeUnControlflt);

        public static string listaDinamicaDto = $@"{listaDinamica}".Replace("[RestoDeAtributos]", $"guardar-en=¨[GuardarEn]¨ {atributosComunesDeUnControlDto}");

        public static string listaDeElementos = $@"<div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=¨[CssContenedor]¨>
                                  <select clase-elemento=¨[ClaseElemento]¨ 
                                          [RestoDeAtributos]
                                          mostrar-expresion=¨[MostrarExpresion]¨  >
                                          <option value=¨0¨>Seleccionar ...</option>
                                  </select>
                             </div>";

        public static string listaDeElementosDto = listaDeElementos.Replace("[RestoDeAtributos]", $"guardar-en=¨[GuardarEn]¨ {atributosComunesDeUnControlDto}");

        public static string listaDeElementosFlt = listaDeElementos.Replace("[RestoDeAtributos]", atributosComunesDeUnControlflt);

        private static string listaDeValores = 
                          $@"<div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=¨[CssContenedor]¨>
                                  <select [RestoDeAtributos] >                                          
                                          [opcionesDeLaLista]
                                  </select>
                             </div>";

        public static string listaDeValoresFlt = listaDeValores.Replace("[RestoDeAtributos]", atributosComunesDeUnControlflt);


        public static string opcionNavegar = @$" 
                           <div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=¨[CssContenedor]¨>
                                    <input id=¨[IdHtml]¨
                                           type=¨button¨
                                           tipo=¨[Tipo]¨
                                           clase=¨[claseBoton]¨
                                           permisos-necesarios=¨[PermisosNecesarios]¨
                                           value=¨[Etiqueta]¨
                                           onClick=¨[Accion]¨
                                           title=¨[Ayuda]¨
                                           [disbled] />
                           </div>";


        public static string editorDto = @$" <div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=¨[CssContenedor]¨>
                                  <input {atributosComunesDeUnControlDto}
                                         type=¨text¨ [LongitudMaxima]
                                         placeholder =¨[Placeholder]¨
                                         valorPorDefecto=¨[ValorPorDefecto]¨
                                         [onBlur]
                                         value=¨[ValorPorDefecto]¨>
                                  </input>
                             </div>"; 
        
        public static string restrictorDto = @$" <div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=¨[CssContenedor]¨>
                                  <input {atributosComunesDeUnControlDto}
                                         type=¨text¨
                                         mostrar-expresion=¨[MostrarExpresion]¨
                                         placeholder =¨[Placeholder]¨
                                         valorPorDefecto=¨[ValorPorDefecto]¨
                                         value=¨¨>
                                  </input>
                             </div>";


        private static string check = $@"<input [RestoDeAtributos]
                                       type=¨checkbox¨ 
                                       value =¨[Checked]¨ [Accion]> 
                                       <label for=¨[IdHtml]¨>[Etiqueta]</label>";

        private static string checkInternoFlt = check.Replace("[RestoDeAtributos]", $" filtrar-por-false=¨[FiltrarPorFalse]¨ {atributosComunesDeUnControlflt}");
        private static string checkInternoDto = check.Replace("[RestoDeAtributos]", atributosComunesDeUnControl);

        public static string checkFlt = $@"<div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=¨[CssContenedor]¨>
                                                       {checkInternoFlt}
                                                     </div>";

        public static string checkDto = $@"<div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=¨[CssContenedor]¨>
                                                {checkInternoDto}
                                           </div>";

        public static string selectorDeFechaDto = $@"
        <div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=¨input-group [CssContenedor]¨>
            <input {atributosComunesDeUnControlDto} 
                   style=¨cursor: pointer; width: 100%;¨
                   type=¨date¨
                   placeholder =¨[Placeholder]¨
                   valorPorDefecto=¨[ValorPorDefecto]¨
                   value=¨¨>
            </input>
            <!--
            <button class=¨input-group-addon¨ style=¨cursor: pointer;
                                                     border: ridge;
                                                     width: 38.24px;
                                                     height: 38.24px;¨>
                <i class=¨fa fa-2x fa-calendar¨ style=¨font-size: 1rem;¨ aria-hidden=¨true¨></i>
            </button>
            -->
        </div>
        ";

        public static string AreaDeTextoDto = $@"
        <div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=¨input-group [CssContenedor]¨>
            <textarea {atributosComunesDeUnControlDto} 
                   placeholder =¨[Placeholder]¨
                   valorPorDefecto=¨[ValorPorDefecto]¨
                   value=¨¨>
            </textarea>
        </div>
        ";

        public static string filtroEntreFechas = $@"
        <div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=¨input-group [CssContenedor]¨>
            <div class=¨fecha-desde¨>
                 <input {atributosComunesDeUnControlflt} 
                        style=¨cursor: pointer;¨
                        type=¨date¨
                        idHoraDesde=¨[IdHtml].hora¨
                        idFechaHasta=¨[IdHtml].hasta¨
                        idHoraHasta=¨[IdHtml].hora.hasta¨
                        value=¨¨>
                 </input>
                 <input id=¨[IdHtml].hora¨ 
                         class=¨[CssHora]¨ 
                         style=¨cursor: pointer;¨
                         type=¨time¨
                         value=¨¨>
                 </input>
            </div>
            <div class=¨fecha-hasta¨>
                 <input id=¨[IdHtml].hasta¨
                        class=¨[Css]¨ 
                        style=¨[Estilos]¨style=¨cursor: pointer;¨
                        type=¨date¨
                        value=¨¨>
                 </input>
                 <input id=¨[IdHtml].hora.hasta¨ 
                         class=¨[CssHora]¨ 
                         style=¨cursor: pointer;¨
                         type=¨time¨
                         value=¨¨>
                 </input>
            </div>
         </div>
        ";

        public static string selectorDeFechaHoraDto = $@"
        <div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=¨input-group [CssContenedor]¨>
            <input {atributosComunesDeUnControlDto} 
                   style=¨cursor: pointer;¨
                   type=¨date¨
                   idDeLaHora=¨[IdHtml].hora¨
                   placeholder =¨[Placeholder]¨
                   valorPorDefecto=¨[ValorPorDefecto]¨
                   value=¨¨>
            </input>
            <input id=¨[IdHtml].hora¨ 
                    obligatorio=¨[Obligatorio]¨ 
                    [Readonly]
                    class=¨[CssHora]¨ 
                    style=¨cursor: pointer;¨
                    type=¨time¨
                    valorPorDefecto=¨[ValorPorDefecto]¨
                    value=¨¨>
            </input>
        </div>
        ";

        public static string DivEnBlanco = $@"
        <div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=[CssContenedor]¨>
        </div>
        ";

        public static string BotonSeleccion = $@"
        <div id = ¨[IdHtmlContenedor]¨ class=¨[cssContenedor]¨>
           <input id=¨[IdHtml]¨ 
                  type=¨button¨ 
                  class=¨[css]¨ 
                  value=¨[Etiqueta]¨ 
                  [onClick]
                  title=¨[Ayuda]¨/>
        </div>
         ";

        public static string Render(string plantilla, Dictionary<string, object> valores)
        {
            foreach (var indice in valores.Keys)
            {
                plantilla = plantilla.Replace($"[{indice}]", valores[indice] == null ? "" : valores[indice].ToString());
            }

            plantilla = plantilla.Replace("style=¨[Estilos]¨", "");

            return plantilla;
        }

    }
}
