using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeEdicion<TElemento> : ControlHtml
    {
        public DescriptorDeCrud<TElemento> Crud => (DescriptorDeCrud<TElemento>)Padre;
        public BarraDeMenu<TElemento> MenuDeEdicion { get; private set; }


        public DescriptorDeEdicion(DescriptorDeCrud<TElemento> crud, string etiqueta)
        : base(
          padre: crud,
          id: $"{crud.Id}_{TipoControl.pnlEditor}",
          etiqueta: etiqueta,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.pnlEditor;
            MenuDeEdicion = new BarraDeMenu<TElemento>(editor: this);
            MenuDeEdicion.AnadirOpcionDeModificarElemento();
            MenuDeEdicion.AnadirOpcionDeCancelarEdicion();
        }


        public override string RenderControl()
        {
            var htmContenedorEdt =
                $@"
                   <Div id=¨{IdHtml}¨ class=¨div-no-visible¨ controlador=¨{Crud.Controlador}¨>
                     <h2>Div de Edicion</h2>
                     {MenuDeEdicion.RenderControl()}
                   </Div>
                ";

            return htmContenedorEdt.Render();
        }
    }
}