using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor.Elementos.ModeloIu;
using Gestor.Errores;
using MVCSistemaDeElementos.Descriptores;
using Utilidades;

namespace SistemaDeElementos.Descriptores.Componentes.Elementos
{
    public class SelectorDeElemento<TElemento> : ControlFiltroHtml where TElemento : Elemento 
    {

        public string GuardarEn { get; private set; }
        public string ParaMostrarEnPropiedad { get; private set; }
        public string SeleccionarDe { get; private set; }

        public SelectorDeElemento(BloqueDeFitro<TElemento> padre, string etiqueta, string propiedad, string ayuda, Posicion posicion)
        : base(
            padre: padre
          , id: $"{padre.Id}_{TipoControl.SelectorDeElemento}_{propiedad}" 
          , etiqueta
          , propiedad
          , ayuda
          , posicion
          )
        {
            Tipo = TipoControl.SelectorDeElemento;

            var propiedades = typeof(TElemento).GetProperties();
            var p = propiedades.FirstOrDefault(x => x.Name == propiedad);
            IUPropiedadAttribute atributos = Elemento.ObtenerAtributos(p);
            
            if (atributos.GuardarEn.IsNullOrEmpty())
                GestorDeErrores.Emitir($"No ha definido el atributo {nameof(atributos.GuardarEn)} de la propiedad {propiedad}");

            if (atributos.SeleccionarDe.IsNullOrEmpty())
                GestorDeErrores.Emitir($"No ha definido el atributo {nameof(atributos.SeleccionarDe)} de la propiedad {propiedad}");

            SeleccionarDe = atributos.SeleccionarDe;
            GuardarEn = atributos.GuardarEn;

            Criterio = TipoCriterio.igual.ToString();
            padre.AnadirSelectorElemento(this);
        }

        public override string RenderControl()
        {
            return RenderSelectorDeElemento();
        }

        private string RenderSelectorDeElemento()
        {
            var htmlSelect = $@"<div id=¨div_{IdHtml}¨  class=¨contenedor-selector¨>
                                    <select id=¨{IdHtml}¨ class=¨{TipoControl.SelectorDeElemento}¨ {RenderAtributos()} clase-elemento=¨{SeleccionarDe}¨ guardar-en¨{GuardarEn}¨>
                                         <option value=¨0¨>Seleccionar ...</option>
                                    </select>
                                </div>";
            return htmlSelect;
        }
    }
}


