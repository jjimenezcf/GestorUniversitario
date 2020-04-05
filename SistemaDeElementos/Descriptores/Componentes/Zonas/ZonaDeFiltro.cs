using System;
using System.Collections.Generic;
using Gestor.Elementos.ModeloIu;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{

    public class ZonaDeFiltro : ControlFiltroHtml
    {
        public ICollection<BloqueDeFitro> Bloques { get; private set; } = new List<BloqueDeFitro>();

        public ZonaDeFiltro(ControlHtml padre)
        : base(
          padre: padre,
          id: $"{padre.Id}_Filtro",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.ZonaDeFiltro;
            var b1 = new BloqueDeFitro(this, "General", new Dimension(1, 2));
            new BloqueDeFitro(this, "Común", new Dimension(1, 2));

            new EditorFiltro(padre: b1, etiqueta: "Nombre", propiedad: FiltroPor.Nombre, ayuda: "buscar por nombre", new Posicion { fila = 0, columna = 0 });
        }

        public ControlFiltroHtml BuscarControl(string propiedad)
        {
            ControlFiltroHtml c = null;
            foreach (var b in Bloques)
            {
                c = b.BuscarControl(propiedad);
                if (c != null)
                    return c;
            }
            return c;
        }

        public void AnadirBloque(BloqueDeFitro bloque)
        {
            Bloques.Add(bloque);
        }

        public BloqueDeFitro ObtenerBloque(string identificador)
        {
            foreach (BloqueDeFitro b in Bloques)
            {
                if (b.Id == identificador)
                    return b;
            }

            throw new Exception($"El bloque {identificador} no está en la zona de filtrado");
        }

        private string RenderFiltro()
        {
            var htmlFiltro = $@"<div id = ¨{IdHtml}¨ style=¨width:100%¨>     
                                     bloques 
                                </div>";

            var htmlBloques = "";
            foreach (BloqueDeFitro b in Bloques)
                htmlBloques = $"{htmlBloques}{(htmlBloques.IsNullOrEmpty() ? "" : Environment.NewLine)}{b.RenderControl()}";

            return htmlFiltro.Replace("bloques", htmlBloques);
        }


        private string RenderModalesFiltro()
        {
            var htmlModalesEnFiltro = "";
            foreach (BloqueDeFitro b in Bloques)
                htmlModalesEnFiltro = $"{htmlModalesEnFiltro}{(htmlModalesEnFiltro.IsNullOrEmpty() ? "" : Environment.NewLine)}{b.RenderModalesBloque()}";

            return htmlModalesEnFiltro;
        }

        public override string RenderControl()
        {
            return RenderFiltro() + Environment.NewLine + RenderModalesFiltro();
        }
    }

}
