using System;
using System.Collections.Generic;

namespace MVCSistemaDeElementos.Descriptores
{

    public class OpcionMantenimiento<TElemento> : ControlHtml
    {
        public string Ruta { get; private set; }
        public string Accion { get; private set; }

        public OpcionMantenimiento(MenuMantenimiento<TElemento> padre, string ruta, string accion, string titulo)
        : base(
          padre: padre,
          id: $"{padre.Id}_{padre.Opciones.Count}_opc",
          etiqueta: titulo,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.Opcion;
            Ruta = ruta;
            Accion = accion;
            ((MenuMantenimiento<TElemento>)Padre).Opciones.Add(this);
        }

        public override string RenderControl()
        {
            throw new NotImplementedException();
        }
    }



    public class MenuMantenimiento<TElemento> : ControlHtml
    {
        public ICollection<OpcionMantenimiento<TElemento>> Opciones { get; private set; } = new List<OpcionMantenimiento<TElemento>>();

        public MenuMantenimiento(ZonaDeMenuMnt<TElemento> padre, VistaCsHtml vista)
        : base(
          padre: padre,
          id: $"{padre.Id}_Menu",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.ZonaDeOpciones;
            new OpcionMantenimiento<TElemento>(this, vista.Ruta, vista.Vista, vista.Etiqueta);
        }

        private string RenderOpcionesMenu()
        {
            var htmlRef = "<div id=¨{idOpc}¨>{newLine}<a href =¨/{ruta}/{accion}¨>{titulo}</a>{newLine}</div>";
            var htmlMenu = "<div id=¨{idMenu}¨>{hmlOpciones}</div>";
            var htmlOpciones = "";
            foreach (OpcionMantenimiento<TElemento> o in Opciones)
            {
                htmlOpciones = htmlOpciones + htmlRef
                                             .Replace("{idOpc}", o.IdHtml)
                                             .Replace("{ruta}", o.Ruta)
                                             .Replace("{accion}", o.Accion)
                                             .Replace("{titulo}", o.Etiqueta)
                                             .Replace("{newLine}", Environment.NewLine) +
                                             Environment.NewLine;
            }

            return htmlMenu.Replace("{idMenu}", IdHtml).Replace("{hmlOpciones}", $"{Environment.NewLine}{htmlOpciones}");
        }

        public override string RenderControl()
        {
            return RenderOpcionesMenu();
        }
    }

}
