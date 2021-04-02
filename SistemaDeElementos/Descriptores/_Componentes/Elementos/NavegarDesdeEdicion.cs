using System;
using System.Collections.Generic;
using ModeloDeDto;

namespace MVCSistemaDeElementos.Descriptores
{
    public class NavegarDesdeEdicion : ControlHtml
    {
        string Url { get; set; }
        public NavegarDesdeEdicion(ControlHtml padre, string etiqueta, string ayuda, string url) 
        : base(padre, $"{padre.Id}-abrir", etiqueta, null, ayuda, null)
        {
            Tipo = Enumerados.enumTipoControl.Opcion;
            Url = url;
        }

        public override string RenderControl()
        {
            var a = new AtributosHtml();
            a = AtributosHtml.AtributosComunes(
                       idHtmlContenedor: $"{IdHtml}-contenedor",
                       idHtml: IdHtml,
                       propiedad: null,
                       tipoDeControl: Tipo);

            a.Etiqueta = Etiqueta;
            a.Url = Url;

            return RenderAbrirEnPestana(a);
        }

        public static string RenderAbrirEnPestana(AtributosHtml atributos)
        {
            Dictionary<string, object> valores = atributos.MapearComunes();
            valores["CssContenedor"] = Css.Render(enumCssControlesDto.ContenedorEditor);
            valores["Css"] = Css.Render(enumCssControlesDto.Editor);
            valores["PermisosNecesarios"] = "";
            valores["Accion"] = $"Crud.EventosDeExpansores('{TipoDeAccionExpansor.NavegarDesdeEdicion}','{atributos.Url}')";

            var htmlEditorTexto = PlantillasHtml.Render(PlantillasHtml.abrirEnPestana, valores);

            return htmlEditorTexto;
        }

    }
}
