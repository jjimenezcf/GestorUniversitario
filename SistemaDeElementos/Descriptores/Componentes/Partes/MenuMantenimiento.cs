using System;
using System.Collections.Generic;

namespace MVCSistemaDeElementos.Descriptores
{

    public class OpcionMantenimiento<Telemento> : ControlHtml
    {
        public string Ruta { get; private set; }
        public string Accion { get; private set; }

        public OpcionMantenimiento(MenuMantenimiento<Telemento> padre, string ruta, string accion, string titulo)
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
            ((MenuMantenimiento<Telemento>)Padre).Opciones.Add(this);
        }

        public override string RenderControl()
        {
            throw new NotImplementedException();
        }
    }



    public class MenuMantenimiento<Telemento> : ControlHtml
    {
        public ICollection<OpcionMantenimiento<Telemento>> Opciones { get; private set; } = new List<OpcionMantenimiento<Telemento>>();

        public MenuMantenimiento(DescriptorDeCrud<Telemento> padre, VistaCsHtml vista)
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
            new OpcionMantenimiento<Telemento>(this, vista.Ruta, vista.Vista, vista.Etiqueta);
        }

        private string RenderOpcionesMenu()
        {
            var htmlRef = "<div id=¨{idOpc}¨>{newLine}<a href =¨/{ruta}/{accion}¨>{titulo}</a>{newLine}</div>";
            var htmlMenu = "<div id=¨{idMenu}¨>{hmlOpciones}</div>";
            var htmlOpciones = "";
            foreach (OpcionMantenimiento<Telemento> o in Opciones)
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
