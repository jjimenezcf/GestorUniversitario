using System;
using System.Collections.Generic;
using Gestor.Elementos.ModeloIu;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{

    public class ZonaDeFiltro<TElemento> : ControlFiltroHtml where TElemento : Elemento
    {

        public ICollection<BloqueDeFitro<TElemento>> Bloques { get; private set; } = new List<BloqueDeFitro<TElemento>>();

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
            if (!EstaElBloqueAnadido(bloque.Etiqueta))
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


        public BloqueDeFitro<TElemento> ObtenerBloquePorEtiqueta(string etiqueta)
        {
            foreach (BloqueDeFitro<TElemento> b in Bloques)
            {
                if (b.Etiqueta == etiqueta)
                    return b;
            }

            throw new Exception($"El bloque con la {etiqueta} no está en la zona de filtrado");
        }

        private bool EstaElBloqueAnadido(string etiqueta)
        {
            foreach (BloqueDeFitro<TElemento> b in Bloques)
            {
                if (b.Etiqueta == etiqueta)
                    return true;
            }
            return false;
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
