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
        public string AccionParaCargar { get; private set; }

        public SelectorDeElemento(BloqueDeFitro<TElemento> padre, string etiqueta, string propiedad, string ayuda, Posicion posicion, string paraGuardarEn, string accion)
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
            AccionParaCargar = accion;
        }

    }
}


