using System;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorMantenimiento<TElemento>: ControlHtml
    {
        public DescriptorDeCrud<TElemento> Crud => (DescriptorDeCrud<TElemento>)Padre;
        public BarraDeMenu<TElemento> MenuDeMnt { get; private set; }
        public ZonaDeFiltro<TElemento> Filtro { get; private set; }
        public ZonaDeGrid<TElemento> Grid { get; set; }

        public DescriptorMantenimiento(DescriptorDeCrud<TElemento> crud, string etiqueta)
        : base(
          padre: crud,
          id: $"{crud.Id}_{TipoControl.Mantenimiento}",
          etiqueta: etiqueta,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.Mantenimiento;
            MenuDeMnt = new BarraDeMenu<TElemento>(mnt: this);
            Filtro = new ZonaDeFiltro<TElemento>(mnt: this);
            Grid = new ZonaDeGrid<TElemento>(mnt: this);  
        }

        public override string RenderControl()
        {

            var htmlMnt = ModoDescriptor.Mantenimiento == ((DescriptorDeCrud<TElemento>)Padre).Modo
                   ?
                   RenderTitulo() + Environment.NewLine +
                   MenuDeMnt.RenderControl() + Environment.NewLine +
                   Filtro.RenderControl() + Environment.NewLine +
                   Grid.RenderControl() + Environment.NewLine
                   :
                   Filtro.RenderControl() + Environment.NewLine +
                   Grid.RenderControl() + Environment.NewLine;

            var htmContenedorMnt =
                $@"
                   <Div id=¨{IdHtml}¨ class=¨div-visible¨>
                     {htmlMnt}
                   </Div>
                ";

            return htmContenedorMnt.Render();
        }

        public string RenderTitulo()
        {
            var htmlCabecera = $"<h2>{this.Etiqueta}</h2>";
            return htmlCabecera;
        }
    }
}