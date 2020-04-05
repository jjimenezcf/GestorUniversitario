namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorMantenimiento<TElemento>: ControlHtml
    {     
        public ZonaDeMenu Menu { get; private set; }
        public ZonaDeFiltro<TElemento> Filtro { get; private set; }
        public ZonaDeGrid<TElemento> Grid { get; set; }

        public DescriptorMantenimiento(DescriptorDeCrud<TElemento> crud)
        : base(
          padre: crud,
          id: $"{crud.Id}_Mnt",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Filtro = new ZonaDeFiltro<TElemento>(mnt: this);
            Grid = new ZonaDeGrid<TElemento>(mnt: this);
        }

        public override string RenderControl()
        {
            throw new System.NotImplementedException();
        }
    }
}