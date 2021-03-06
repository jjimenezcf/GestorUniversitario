﻿using System.Collections.Generic;
namespace MVCSistemaDeElementos.Descriptores
{
    public class EditorDeFecha : ControlHtml
    {
        public EditorDeFecha(ControlHtml padre, string etiqueta, string propiedad, string ayuda):
        base(padre: padre,$"{padre.Id}-{propiedad}", etiqueta, propiedad, ayuda, null )
        {
            Tipo = Enumerados.enumTipoControl.SelectorDeFechaHora;
        }

        
        public override string RenderControl()
        {
            var a = new AtributosHtml(
                idHtmlContenedor: $"{IdHtml}-contenedor",
                idHtml: IdHtml,
                propiedad: PropiedadHtml,
                tipoDeControl: Tipo ,
                visible: Visible,
                editable: Editable,
                obligatorio: Obligatorio,
                ayuda: Ayuda,
                valorPorDefecto:null);

            return RenderSelectorDeFechaHora(a);
        }


        public static string RenderSelectorDeFechaHora(AtributosHtml atributos)
        {
            var valores = atributos.MapearComunes(); 
            valores["CssContenedor"] = Css.Render(enumCssControlesDto.ContenedorFecha);
            valores["Css"] = Css.Render(enumCssControlesDto.SelectorDeFecha);
            valores["CssHora"] = Css.Render(enumCssControlesDto.SelectorDeHora);
            valores["Placeholder"] = atributos.Ayuda;
            valores["ValorPorDefecto"] = atributos.ValorPorDefecto;

            var htmSelectorDeFecha = PlantillasHtml.Render(PlantillasHtml.selectorDeFechaHoraDto, valores);

            return htmSelectorDeFecha;
        }

    }
}
