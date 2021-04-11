using ModeloDeDto;

namespace MVCSistemaDeElementos.Descriptores
{

    public class FiltroEntreFechas<TElemento> : ControlFiltroHtml where TElemento : ElementoDto
    {
        public FiltroEntreFechas(BloqueDeFitro<TElemento> bloque, string etiqueta, string propiedad, string ayuda, Posicion posicion)
        : base(bloque, $"{bloque.Id}-{propiedad}", etiqueta, propiedad, ayuda, posicion)
        {
            Tipo = Enumerados.enumTipoControl.FiltroEntreFechas;
            Criterio = CriteriosDeFiltrado.entreFechas;
            bloque.Tabla.Dimension.CambiarDimension(posicion);
            bloque.AnadirControlEn(this);
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
                ayuda: Ayuda,
                valorPorDefecto: null);

            return RenderFiltroEntreFechas(a);
        }

        public static string RenderFiltroEntreFechas(AtributosHtml atributos)
        {
            var valores = atributos.MapearComunes();
            valores["CssContenedor"] = Css.Render(enumCssFiltro.ContenedorEntreFechas);
            valores["Css"] = Css.Render(enumCssFiltro.Fecha);
            valores["CssHora"] = Css.Render(enumCssFiltro.Hora);
            valores["Placeholder"] = atributos.Ayuda;

            var htmSelectorDeFecha = PlantillasHtml.Render(PlantillasHtml.filtroEntreFechas, valores);

            return htmSelectorDeFecha;
        }
    }
}
