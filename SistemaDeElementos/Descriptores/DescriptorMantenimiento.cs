using System;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorMantenimiento<TElemento>: ControlHtml
    {     
        public ZonaDeMenuMnt<TElemento> Menu { get; private set; }
        public ZonaDeFiltro<TElemento> Filtro { get; private set; }
        public ZonaDeGrid<TElemento> Grid { get; set; }

        public DescriptorMantenimiento(DescriptorDeCrud<TElemento> crud, string etiqueta)
        : base(
          padre: crud,
          id: $"{crud.Id}_Mnt",
          etiqueta: etiqueta,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Menu = new ZonaDeMenuMnt<TElemento>(mnt: this);
            Filtro = new ZonaDeFiltro<TElemento>(mnt: this);
            Grid = new ZonaDeGrid<TElemento>(mnt: this);
        }

        public override string RenderControl()
        {
            var htmlCrud = ModoDescriptor.Mantenimiento == ((DescriptorDeCrud<TElemento>)Padre).Modo
                   ?
                   RenderTitulo() + Environment.NewLine +
                   Menu.RenderControl() + Environment.NewLine +
                   Filtro.RenderControl() + Environment.NewLine +
                   Grid.RenderControl() + Environment.NewLine
                   :
                   Filtro.RenderControl() + Environment.NewLine +
                   Grid.RenderControl() + Environment.NewLine;

            return htmlCrud.Render();
        }

        public string RenderTitulo()
        {
            var htmlCabecera = $"<h2>{this.Etiqueta}</h2>";
            return htmlCabecera;
        }
    }
}