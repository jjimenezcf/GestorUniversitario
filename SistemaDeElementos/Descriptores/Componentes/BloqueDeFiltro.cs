using System;
using System.Collections.Generic;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{

    public class BloqueDeFitro : ControlFiltroHtml
    {
        public TablaBloqueDeFiltro Tabla { get; set; }

        public ICollection<ControlFiltroHtml> Controles => Tabla.Controles;


        public BloqueDeFitro(ZonaDeFiltro padre, string titulo, Dimension dimension)
        : base(
          padre: padre,
          id: $"{padre.Id}_{padre.Bloques.Count}_bloque",
          etiqueta: titulo,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.Bloque;
            Tabla = new TablaBloqueDeFiltro(this, dimension, new List<ControlFiltroHtml>());
            padre.Bloques.Add(this);
        }


        public void AnadirControl(ControlFiltroHtml c)
        {
            Controles.Add(c);
        }

        public void AnadirSelector<T>(SelectorDeFiltro<T> s)
        {
            Controles.Add(s);
            Controles.Add(s.Modal);
        }
        public ControlHtml ObtenerControl(string id)
        {

            foreach (ControlHtml c in Controles)
            {
                if (c.Id == id)
                    return c;
            }

            throw new Exception($"El control {id} no está en la zona de filtrado");
        }

        private string RenderBloque()
        {
            string htmlBloque = $@"<div id = ¨{IdHtml}¨>     
                                     tabla 
                                    </div>";
            string htmlTabla = Tabla.RenderControl();

            return htmlBloque.Replace("tabla", htmlTabla);
        }

        public string RenderModalesBloque()
        {
            var htmlModalesEnBloque = "";
            foreach (ControlHtml c in Controles)
            {
                if (c.Tipo == TipoControl.GridModal)
                    htmlModalesEnBloque =
                        $"{htmlModalesEnBloque}{(htmlModalesEnBloque.IsNullOrEmpty() ? "" : Environment.NewLine)}" +
                        $"{c.RenderControl()}";

            }
            return htmlModalesEnBloque;
        }

        public override string RenderControl()
        {
            return RenderBloque();
        }

        internal ControlFiltroHtml BuscarControl(string propiedad)
        {
            foreach (ControlFiltroHtml c in Controles)
            {
                
                if (c.Id == $"{Id}_{c.Tipo}_{propiedad}")
                    return c;
            }
            return null;
        }
    }

}
