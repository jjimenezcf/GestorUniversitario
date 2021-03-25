using System;
using System.Collections.Generic;
namespace MVCSistemaDeElementos.Descriptores
{
    public class EditorDeTexto : ControlHtml
    {
        int LongitudMaxima { get; set; }

        public EditorDeTexto(ControlHtml padre, string id, string etiqueta, string propiedad, string ayuda, int longitudMaxima = 0) :
        base(padre: padre, id, etiqueta, propiedad, ayuda, null)
        {
            Tipo = Enumerados.enumTipoControl.Editor;
            LongitudMaxima = longitudMaxima;
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
                anchoMaximo: AnchoMaximo,
                numeroDeFilas: -1,
                ayuda: Ayuda,
                valorPorDefecto: null);

            a.LongitudMaxima = LongitudMaxima;

            return RenderEditorDeTexto(a);
        }

        public static string RenderEditorDeTexto(AtributosHtml atributos)
        {
            Dictionary<string, object> valores = ValoresDeAtributosComunes(atributos);
            valores["CssContenedor"] = Css.Render(enumCssControlesDto.ContenedorEditor);
            valores["Css"] = Css.Render(enumCssControlesDto.Editor);
            valores["LongitudMaxima"] = atributos.LongitudMaxima > 0 ? $"{Environment.NewLine}maxlength=¨{atributos.LongitudMaxima}¨"   : "";
            valores["Placeholder"] = atributos.Ayuda;
            valores["ValorPorDefecto"] = atributos.ValorPorDefecto;

            var htmlEditorTexto = PlantillasHtml.Render(PlantillasHtml.editorDto, valores);

            return htmlEditorTexto;
        }


    }
}
