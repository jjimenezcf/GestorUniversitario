using Enumerados;
using GestorDeElementos;
using ModeloDeDto;

namespace MVCSistemaDeElementos.Descriptores
{
    public class EditorFiltro<TElemento> : ControlFiltroHtml where TElemento : ElementoDto
    {
        public EditorFiltro(BloqueDeFitro<TElemento> bloque, string etiqueta, string propiedad, string ayuda, Posicion posicion)
        : base(padre: bloque
              , id: $"{bloque.Id}_{enumTipoControl.Editor.Render()}_{propiedad}"
              , etiqueta
              , propiedad
              , ayuda
              , posicion
              )
        {
            Tipo = enumTipoControl.Editor;
            Criterio = CriteriosDeFiltrado.contiene;
            bloque.Tabla.Dimension.CambiarDimension(posicion);
            bloque.AnadirControlEn(this);
        }

        public override string RenderControl()
        {
            return RenderEditor();
        }

        public string RenderEditor()
        {
            return $@"<div class=¨{enumCssFiltro.ContenedorEditor.Render()}¨>
                         <input id=¨{IdHtml}¨ type = ¨text¨  class=¨form-control¨ {base.RenderAtributos()}  placeholder=¨{Ayuda}¨>
                     </div>
                  ";
        }
    }

}
