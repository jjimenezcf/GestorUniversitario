using System.Collections.Generic;
namespace MVCSistemaDeElementos.Descriptores
{
    public class DivEnBlanco : ControlHtml
    {
        public DivEnBlanco(ControlHtml padre) 
        : base(padre: padre, $"{padre.Id}-blanco", "", "", "", null)
        {
            Tipo = Enumerados.enumTipoControl.Bloque;
        }

        public override string RenderControl()
        {

            var a = new AtributosHtml(
                idHtmlContenedor: $"{IdHtml}-contenedor",
                idHtml: IdHtml,
                propiedad: "",
                tipoDeControl: Tipo,
                visible: true,
                editable: false,
                obligatorio: false,
                ayuda: null,
                valorPorDefecto: null);

            return RenderDivEnBlanco(a);
        }

        public static string RenderDivEnBlanco(AtributosHtml atributos)
        {
            var valores = atributos.MapearComunes();
            valores["CssContenedor"] = Css.Render(enumCssControlesDto.ContenedorFecha);

            var htmlDivEnBlanco = PlantillasHtml.Render(PlantillasHtml.DivEnBlanco, valores);

            return htmlDivEnBlanco;
        }
    }
}
