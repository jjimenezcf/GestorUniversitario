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


        public static string span = $@"
        <div id=¨[IdHtml]¨ tipo=¨span-de-controles¨ class=¨[cssClase]¨>
           <div id=¨[IdHtml]-cabecera¨ class=¨[cssCabecera]¨>[RenderCabeceraDelSpan]</div>
           <div id=¨[IdHtml]-cuerpo¨ class=¨[cssCuerpo]¨>[RenderCuerpoDelSpan]</div>
           <div id=¨[IdHtml]-pie¨ class=¨[cssPie]¨>[RenderPieDelSpan]</div>
        </div>";

        public static string expansor = $@"	 
            <a id=¨mostrar.[IdHtml].ref¨
               class=¨[cssClase]¨
               href=¨javascript:[Evento];¨>
               bloque: [Titulo]
            </a>
            <input id=¨expandir.[IdHtml].input¨ type=¨hidden¨ value=¨1¨ />";

        public static string contenedorGridControl = $@"
             <div id=¨contenedor.[IdHtml].etiqueta¨ class=¨[cssClase]¨>[RenderEtiqueta]</div>
             <div id=¨contenedor.[IdHtml].control¨  class=¨[cssClase]¨>[RenderControl]</div>
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

        public static string editorDto = @$" <div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=¨[CssContenedor]¨>
                                  <input {atributosComunesDeUnControlDto}
                                         type=¨text¨ [LongitudMaxima]
                                         placeholder =¨[Placeholder]¨
                                         valorPorDefecto=¨[ValorPorDefecto]¨
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
                                       value =¨[Checked]¨>
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

        public static string selectorDeFechaHoraDto = $@"
        <div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=¨input-group [CssContenedor]¨>
            <input {atributosComunesDeUnControlDto} 
                   style=¨cursor: pointer; width: 65%;¨
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
                    style=¨cursor: pointer; width: 35%;¨
                    type=¨time¨
                    valorPorDefecto=¨[ValorPorDefecto]¨
                    value=¨¨>
            </input>
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

        public static Dictionary<string, object> ValoresDeAtributesComunes(string idHtmlContenedor, string idHtml, string propiedad, enumTipoControl tipoDeControl)
        {
            var valores = new Dictionary<string, object>();
            valores["IdHtmlContenedor"] = idHtmlContenedor;
            valores["IdHtml"] = idHtml;
            valores["Propiedad"] = propiedad;
            valores["Tipo"] = tipoDeControl.Render();
            return valores;
        }
    }
}
