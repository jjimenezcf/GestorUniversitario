using ModeloDeDto;

namespace MVCSistemaDeElementos.Descriptores
{
    public class RestrictorDeFiltro<TElemento> : ControlFiltroHtml where TElemento : ElementoDto
    {
        public RestrictorDeFiltro(BloqueDeFitro<TElemento> bloque, string etiqueta, string propiedad, string ayuda, Posicion posicion)
        : base(padre: bloque
              , id: $"{bloque.Id}-{propiedad}"
              , etiqueta
              , propiedad
              , ayuda
              , posicion
              )
        {
            Tipo = TipoControl.RestrictorDeFiltro;
            Criterio = TipoCriterio.igual.ToString();
            bloque.Tabla.Dimension.CambiarDimension(posicion);
            bloque.AnadirControlEn(this);
        }

        public override string RenderControl()
        {
            return RenderEditor();
        }

        public string RenderEditor()
        {
            return $@"<div class=¨input-group mb-3¨>
                         <input id=¨{IdHtml}¨ type = ¨text¨ class=¨form-control¨ {base.RenderAtributos()} readonly placeholder=¨{Ayuda}¨>
                      </div>
                  ";
        }
    }
}
