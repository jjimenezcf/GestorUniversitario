using System;
using System.Collections.Generic;
using ModeloDeDto;

namespace MVCSistemaDeElementos.Descriptores
{
    public class Valor
    {

    }

    public class DesplegableDeFiltro<TElemento> : ControlFiltroHtml where TElemento : ElementoDto
    {
        public ICollection<Valor> valores { get; set; }

        public DesplegableDeFiltro(BloqueDeFitro<TElemento> bloque, string etiqueta, string propiedad, string ayuda, Posicion posicion)
        : base(padre: bloque
              , id: $"{bloque.Id}_{TipoControl.DesplegableDeFiltro}_{propiedad}"
              , etiqueta
              , propiedad
              , ayuda
              , posicion
              )
        {
            Tipo = TipoControl.DesplegableDeFiltro;
            Criterio = TipoCriterio.comienza.ToString();
            bloque.Tabla.Dimension.CambiarDimension(posicion);
            bloque.AnadirControlEn(this);
        }

        public override string RenderControl()
        {
            return RenderDesplegableDeFiltro();
        }
        public string RenderDesplegableDeFiltro()
        {
            var htmlSelect = $@"<div id=¨div-{IdHtml}¨  class=¨contenedor-selector¨>
                                    <input id=¨{IdHtml}¨ class=¨{Tipo}¨ {RenderAtributos()} />
                                    <datalist id=¨{IdHtml}-lista¨>
                                    </datalist>
                                </div>";

            return htmlSelect;
        }
    }
}
