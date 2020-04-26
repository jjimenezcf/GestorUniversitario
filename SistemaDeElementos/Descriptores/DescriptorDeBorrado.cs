namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeBorrado<TElemento> : ControlHtml
    {
        public DescriptorDeBorrado(DescriptorDeCrud<TElemento> crud, string etiqueta)
        : base(
          padre: crud,
          id: $"{crud.Id}_{TipoControl.pnlBorrado}",
          etiqueta: etiqueta,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
        }

        private string RendelModal()
        {
            var htmlModal =$@"<div id=¨{IdHtml}¨ class=¨contenedor-modal¨>
                              		<div id=¨{IdHtml}_contenido¨ class=¨cotenido-modal¨>
                              			<span class=¨span-cerrar¨>×</span>
                              			<h2>Modal</h2>
                              			<p>Se ha desplegado el modal y bloqueado el scroll del body!</p>
                              		</div>
                              </div>";
            return htmlModal;
        }

        public override string RenderControl()
        {
            return RendelModal();
        }
    }
}