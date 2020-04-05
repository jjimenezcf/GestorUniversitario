using System;
using System.Collections.Generic;

namespace MVCSistemaDeElementos.Descriptores
{
    public class Valor
    {

    }

    public class DesplegableDeFiltro : ControlFiltroHtml
    {
        public ICollection<Valor> valores { get; set; }

        public DesplegableDeFiltro(ControlHtml padre, string etiqueta, string propiedad, string ayuda, Posicion posicion)
        : base(padre: padre
              , id: $"{padre.Id}_{TipoControl.Desplegable}_{propiedad}"
              , etiqueta
              , propiedad
              , ayuda
              , posicion
              )
        {
            Tipo = TipoControl.Desplegable;
        }

        public override string RenderControl()
        {
            throw new NotImplementedException();
        }
    }
}
