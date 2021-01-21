using System.Collections.Generic;

namespace MVCSistemaDeElementos.Descriptores
{
    public static class PlantillasHtml
    {

        /*
         *div id=¨{descriptorControl.IdHtmlContenedor}¨ name=¨contenedor-control¨ class=¨{cssClaseContenedor}¨ ¨
         */
        public static string selectorDto = @"
                             <div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=¨[CssContenedor]¨>
                                  <select id=¨[IdHtml]¨ 
                                          propiedad=¨[Propiedad]¨ 
                                          class=¨[Css]¨ 
                                          tipo=¨[Tipo]¨
                                          clase-elemento=¨[SeleccionarDe]¨ 
                                          guardar-en=¨[GuardarEn]¨
                                          mostrar-expresion=¨[MostrarExpresion]¨  >
                                          <option value=¨0¨>Seleccionar ...</option>
                                  </select>
                             </div>";

        public static string selectorFlt = selectorDto.Replace("guardar-en=¨[GuardarEn]¨", "");

        public static string editorDto = @"
                             <div id=¨[IdHtmlContenedor]¨ name=¨contenedor-control¨ class=¨[CssContenedor]¨>
                                  <input id=¨[IdHtml]¨ 
                                          type=¨text¨
                                          propiedad=¨[Propiedad]¨ 
                                          class=¨[Css]¨ 
                                          tipo=¨[Tipo]¨ 
                                          value=¨¨
                                          placeholder =¨[Ayuda]¨
                                          ValorPorDefecto=¨[ValorPorDefecto]¨>
                                  </input>
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
