using System.Linq;
using Gestor.Elementos.ModeloIu;
using Gestor.Errores;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{
    public class SelectorDeElemento<TElemento> : ControlFiltroHtml where TElemento : Elemento 
    {

        public string GuardarEn { get; private set; }
        public string SeleccionarDe { get; private set; }
        public string MostrarPropiedad { get; private set; }

        public SelectorDeElemento(BloqueDeFitro<TElemento> padre, string propiedad, Posicion posicion)
        : base(
            padre: padre
          , id: $"{padre.Id}_{TipoControl.SelectorDeElemento}_{propiedad}" 
          , ""
          , propiedad
          , ""
          , posicion
          )
        {
            Tipo = TipoControl.SelectorDeElemento;

            var propiedades = typeof(TElemento).GetProperties();
            var p = propiedades.FirstOrDefault(x => x.Name == propiedad);
            IUPropiedadAttribute atributos = Elemento.ObtenerAtributos(p);

            if (atributos.Etiqueta.IsNullOrEmpty())
                GestorDeErrores.Emitir($"No ha definido el atributo {nameof(atributos.Etiqueta)} de la propiedad {propiedad}");

            if (atributos.Ayuda.IsNullOrEmpty())
                GestorDeErrores.Emitir($"No ha definido el atributo {nameof(atributos.Ayuda)} de la propiedad {propiedad}");

            if (atributos.GuardarEn.IsNullOrEmpty())
                GestorDeErrores.Emitir($"No ha definido el atributo {nameof(atributos.GuardarEn)} de la propiedad {propiedad}");

            if (atributos.SeleccionarDe.IsNullOrEmpty())
                GestorDeErrores.Emitir($"No ha definido el atributo {nameof(atributos.SeleccionarDe)} de la propiedad {propiedad}");

            Etiqueta = atributos.Etiqueta;
            Ayuda = atributos.Ayuda;
            SeleccionarDe = atributos.SeleccionarDe;
            GuardarEn = atributos.GuardarEn;
            MostrarPropiedad = atributos.MostrarPropiedad.IsNullOrEmpty() ? propiedad : atributos.MostrarPropiedad;

            Criterio = TipoCriterio.igual.ToString();
            padre.AnadirSelectorElemento(this);
        }

        public override string RenderControl()
        {
            return RenderSelectorDeElemento();
        }

        public override string RenderAtributos(string atributos = "")
        {
            atributos = base.RenderAtributos(atributos);
            atributos = $"{atributos} clase-elemento=¨{SeleccionarDe}¨ guardar-en=¨{GuardarEn}¨ mostrar-propiedad=¨{MostrarPropiedad.ToLower()}¨";
            return atributos;
        }

        private string RenderSelectorDeElemento()
        {
            var htmlSelect = $@"<div id=¨div_{IdHtml}¨  class=¨contenedor-selector¨>
                                    <select id=¨{IdHtml}¨ class=¨{TipoControl.SelectorDeElemento}¨ {RenderAtributos()} >
                                         <option value=¨0¨>Seleccionar ...</option>
                                    </select>
                                </div>";
            return htmlSelect;
        }
    }
}


