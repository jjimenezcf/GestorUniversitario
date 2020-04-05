using System;
using System.Collections.Generic;

namespace MVCSistemaDeElementos.Descriptores
{
    public class Menu<TElemento> : ControlHtml
    {
        public ZonaDeMenuMnt<TElemento> ZonaMenu => (ZonaDeMenuMnt<TElemento>)Padre;
        public ICollection<OpcionDeMenu<TElemento>> OpcioneDeMenu { get; private set; } = new List<OpcionDeMenu<TElemento>>();

        public Menu(ZonaDeMenuMnt<TElemento> padre)
        : base(
          padre: padre,
          id: $"{padre.Id}_{TipoControl.Menu}",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.Menu;
        }

        internal void Add(OpcionDeMenu<TElemento> opcion)
        {
            OpcioneDeMenu.Add(opcion);
        }

        private string RenderOpcionesMenu()
        {
            var htmlMenu = "<div id=¨{idMenu}¨>{hmlOpciones}</div>";
            var htmlOpciones = "";
            foreach (OpcionDeMenu<TElemento> opcioDeMenu in OpcioneDeMenu)
            {
                htmlOpciones = htmlOpciones + opcioDeMenu.RenderControl();
            }

            return htmlMenu.Replace("{idMenu}", IdHtml).Replace("{hmlOpciones}", $"{Environment.NewLine}{htmlOpciones}");
        }

        public override string RenderControl()
        {
            return RenderOpcionesMenu();
        }
    }

}
