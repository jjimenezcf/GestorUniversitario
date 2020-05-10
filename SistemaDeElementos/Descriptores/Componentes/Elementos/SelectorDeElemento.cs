using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor.Elementos.ModeloIu;
using MVCSistemaDeElementos.Descriptores;

namespace SistemaDeElementos.Descriptores.Componentes.Elementos
{
    public class SelectorDeElemento<TElemento> : ControlFiltroHtml where TElemento : Elemento 
    {

        public string ParaGuardarEnPropiedad { get; private set; }
        public string ParaMostrarEnPropiedad { get; private set; }
        public string SeleccionarDeLaClase { get; private set; }

        public SelectorDeElemento(BloqueDeFitro<TElemento> padre, string etiqueta, string propiedad, string ayuda, Posicion posicion, string paraGuardarEn, string claseElemento)
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
            ParaGuardarEnPropiedad = paraGuardarEn.ToLower();
            SeleccionarDeLaClase = claseElemento;
            Criterio = TipoCriterio.igual.ToString();
            padre.AnadirSelectorElemento(this);
        }

        public override string RenderControl()
        {
            return RenderSelectorDeElemento();
        }

        private string RenderSelectorDeElemento()
        {
            var htmlSelect = $@"<div id=¨div_{IdHtml}¨ class=¨contenedor-selector¨>
                                    <select id=¨{IdHtml}¨ class=¨selector-elemento¨ {RenderAtributos()} clase-elemento=¨{SeleccionarDeLaClase}¨ guardar-en¨{ParaGuardarEnPropiedad}¨>
                                         <option value=¨0¨>Seleccionar ...</option>
                                    </select>
                                </div>";
            return htmlSelect;
        }
    }
}


