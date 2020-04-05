using System;
using System.Collections.Generic;
using Gestor.Elementos.ModeloIu;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{

    public class ZonaDeFiltro<TElemento> : ControlFiltroHtml
    {
        public ZonaDeFiltro(ControlHtml mnt)
        : base(
          padre: mnt,
          id: $"{mnt.Id}_Filtro",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.ZonaDeFiltro;
            var b1 = new BloqueDeFitro<TElemento>(this, "General", new Dimension(1, 2));
            new BloqueDeFitro<TElemento>(this, "Común", new Dimension(1, 2));

            new EditorFiltro<TElemento>(bloque: b1, etiqueta: "Nombre", propiedad: FiltroPor.Nombre, ayuda: "buscar por nombre", new Posicion { fila = 0, columna = 0 });
        }

        public ICollection<BloqueDeFitro<TElemento>> Bloques { get; private set; } = new List<BloqueDeFitro<TElemento>>();

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

        public void AnadirBloque(BloqueDeFitro<TElemento> bloque)
        {
            Bloques.Add(bloque);
        }

        public BloqueDeFitro<TElemento> ObtenerBloque(string identificador)
        {
            foreach (BloqueDeFitro<TElemento> b in Bloques)
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
            foreach (BloqueDeFitro<TElemento> b in Bloques)
                htmlBloques = $"{htmlBloques}{(htmlBloques.IsNullOrEmpty() ? "" : Environment.NewLine)}{b.RenderControl()}";

            return htmlFiltro.Replace("bloques", htmlBloques);
        }

        private string RenderModalesFiltro()
        {
            var htmlModalesEnFiltro = "";
            foreach (BloqueDeFitro<TElemento> b in Bloques)
                htmlModalesEnFiltro = $"{htmlModalesEnFiltro}{(htmlModalesEnFiltro.IsNullOrEmpty() ? "" : Environment.NewLine)}{b.RenderModalesBloque()}";

            return htmlModalesEnFiltro;
        }

        public override string RenderControl()
        {
            return RenderFiltro() + Environment.NewLine + RenderModalesFiltro();
        }
    }

}
