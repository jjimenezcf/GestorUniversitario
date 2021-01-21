using System.Collections.Generic;

namespace MVCSistemaDeElementos.Descriptores
{
    public static class PlantillasHtml
    {

        private static string atributosComunesDeUnControl = @"
                                       id=¨[IdHtml]¨
                                       propiedad=¨[Propiedad]¨ 
                                       class=¨[Css]¨ 
                                       tipo=¨[Tipo]¨ 
                                       obligatorio=¨[Obligatorio]¨ 
                                       [Readonly]
                                       ";

        public static string listaDinamicaFlt = $@"
                             <div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=¨[CssContenedor]¨>
                                  <input {atributosComunesDeUnControl} 
                                         clase-elemento=¨[ClaseElemento]¨ 
                                         guardar-en=¨[GuardarEn]¨
                                         mostrar-expresion=¨[MostrarExpresion]¨
                                         como-buscar=¨[ComoBuscar]¨
                                         longitud=¨[Longitud]¨ 
                                         cantidad-a-leer=¨[Cantidad]¨ 
                                         criterio-de-filtro=¨[CriterioDeFiltro]¨
                                         filtrar-por=¨[FiltrarPor]¨
                                         placeholder =¨[Placeholder]¨
                                         oninput=¨[OnInput]¨ 
                                         onchange=¨[OnChange]¨
                                         list=¨[IdHtml]-lista¨ >
                                         <datalist id=¨[IdHtml]-lista¨>
                                         </datalist>
                             </div>";

        public static string listaDinamicaDto = $@"
                             <div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=¨[CssContenedor]¨>
                                  <input {atributosComunesDeUnControl} 
                                         clase-elemento=¨[ClaseElemento]¨ 
                                         guardar-en=¨[GuardarEn]¨
                                         mostrar-expresion=¨[MostrarExpresion]¨
                                         como-buscar=¨[ComoBuscar]¨
                                         longitud=¨[Longitud]¨ 
                                         cantidad-a-leer=¨[Cantidad]¨ 
                                         criterio-de-filtro=¨[CriterioDeFiltro]¨
                                         placeholder =¨[Placeholder]¨
                                         oninput=¨Crud.EventosDeListaDinamica('cargar',this)¨ 
                                         onchange=¨Crud.EventosDeListaDinamica('seleccionar',this)¨
                                         list=¨[IdHtml]-lista¨ >
                                         <datalist id=¨[IdHtml]-lista¨>
                                         </datalist>
                             </div>";


        public static string listaDeElementosDto = $@"
                             <div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=¨[CssContenedor]¨>
                                  <select {atributosComunesDeUnControl} 
                                          clase-elemento=¨[ClaseElemento]¨ 
                                          guardar-en=¨[GuardarEn]¨
                                          mostrar-expresion=¨[MostrarExpresion]¨  >
                                          <option value=¨0¨>Seleccionar ...</option>
                                  </select>
                             </div>";

        public static string listaDeElementosFlt = listaDeElementosDto.Replace("guardar-en=¨[GuardarEn]¨", "control-de-filtro=¨S¨")
                                                      .Replace("[Readonly]", "")
                                                      .Replace("obligatorio=¨[Obligatorio]¨", "");

        public static string editorDto = @$"
                             <div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=¨[CssContenedor]¨>
                                  <input {atributosComunesDeUnControl}
                                         type=¨text¨
                                         placeholder =¨[Placeholder]¨
                                         valorPorDefecto=¨[ValorPorDefecto]¨
                                         value=¨¨>
                                  </input>
                             </div>";

        public static string checkDto = @$"
                             <div class=¨[CssContenedor]¨>
                                <label></label>
                             </div>                             
                             <div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=¨[CssContenedor]¨>
                                <input {atributosComunesDeUnControl}
                                       type=¨checkbox¨ 
                                       checked=¨[Checked]¨>
                                <label for=¨[IdHtml]¨>Mostrar en modal</label>
                             </div>";


        public static string Render(string plantilla, Dictionary<string,object> valores)
        {
            foreach(var indice in valores.Keys)
            {
                plantilla = plantilla.Replace($"[{indice}]", valores[indice] == null ? "" : valores[indice].ToString());
            }
            return plantilla;
        }

        public static Dictionary<string, object> ValoresDeAtributesComunes(string idHtmlContenedor, string idHtml, string propiedad, string tipoDeControl)
        {
            var valores = new Dictionary<string, object>();
            valores["IdHtmlContenedor"] = idHtmlContenedor;
            valores["IdHtml"] = idHtml;
            valores["Propiedad"] = propiedad;
            valores["Tipo"] = tipoDeControl;
            return valores;
        }
    }
}
