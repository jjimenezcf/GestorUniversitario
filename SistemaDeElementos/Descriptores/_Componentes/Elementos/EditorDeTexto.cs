using System;
using System.Collections.Generic;
namespace MVCSistemaDeElementos.Descriptores
{
    public class EditorDeTexto : ControlHtml
    {
        public EditorDeTexto(ControlHtml padre, string id, string etiqueta, string propiedad, string ayuda) :
        base(padre: padre, id, etiqueta, propiedad, ayuda, null)
        {
            Tipo = Enumerados.enumTipoControl.Editor;
        }

        public override string RenderControl()
        {
            var a = new AtributosHtml(
                idHtmlContenedor: $"{IdHtml}-contenedor",
                idHtml: IdHtml,
                propiedad: PropiedadHtml,
                tipoDeControl: Tipo,
                visible: Visible,
                editable: Editable,
                obligatorio: Obligatorio,
                anchoMaximo: null,
                numeroDeFilas: -1,
                ayuda: Ayuda,
                valorPorDefecto: null);

            return RenderEditorDeTexto(a);
        }

        public static string RenderEditorDeTexto(AtributosHtml atributos)
        {
            Dictionary<string, object> valores = ValoresDeAtributosComunes(atributos);
            valores["CssContenedor"] = Css.Render(enumCssControlesDto.ContenedorFecha);
            valores["Css"] = Css.Render(enumCssControlesDto.SelectorDeFecha);
            valores["LongitudMaxima"] = atributos.LongitudMaxima > 0 ?
                    $"{Environment.NewLine}maxlength=¨{atributos.LongitudMaxima}¨"
                    : "";
            valores["Placeholder"] = atributos.Ayuda;
            valores["ValorPorDefecto"] = atributos.ValorPorDefecto;

            var htmSelectorDeFecha = PlantillasHtml.Render(PlantillasHtml.editorDto, valores);

            return htmSelectorDeFecha;
        }

        /*
         atributos comunes
            valores["IdHtmlContenedor"] = idHtmlContenedor;
            valores["IdHtml"] = idHtml;
            valores["Propiedad"] = propiedad;
            valores["Tipo"] = tipoDeControl.Render();

        
            valores["Obligatorio"] = atributos.Visible && atributos.Obligatorio ? "S" : "N";
            valores["Readonly"] = !atributos.Editable ? "readonly" : "";



         */

    }
}
