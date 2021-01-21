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

        /*
         *div id=¨{descriptorControl.IdHtmlContenedor}¨ name=¨contenedor-control¨ class=¨{cssClaseContenedor}¨ ¨
         */
        public static string selectorDto = $@"
                             <div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=¨[CssContenedor]¨>
                                  <select {atributosComunesDeUnControl} 
                                          clase-elemento=¨[ClaseElemento]¨ 
                                          guardar-en=¨[GuardarEn]¨
                                          mostrar-expresion=¨[MostrarExpresion]¨  >
                                          <option value=¨0¨>Seleccionar ...</option>
                                  </select>
                             </div>";

        public static string selectorFlt = selectorDto.Replace("guardar-en=¨[GuardarEn]¨", "")
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
    }
}
