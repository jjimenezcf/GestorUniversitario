using ModeloDeDto;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeBorrado<TElemento> : ControlHtml where TElemento : ElementoDto
    {
        public DescriptorDeCrud<TElemento> Crud => (DescriptorDeCrud<TElemento>)Padre;

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
            var cuerpoHtml = $"<input type=¨text¨ id=¨{IdHtml}-mensaje¨ class=¨mensaje-modal¨ value=¨Desea borrar el elemento seleccionado¨></input>";
           
            var htmlModal = RenderizarModal(
                idHtml: IdHtml
                , controlador: Crud.Controlador
                , tituloH2: "Confirmación de borrado"
                , cuerpo: cuerpoHtml
                , idOpcion: $"{IdHtml}-aceptar"
                , opcion: Crud.NegocioActivo ? "Aceptar": ""
                , accion: Crud.NegocioActivo ? "Crud.EventosModalDeBorrar('borrar-elemento')": ""
                , cerrar: "Crud.EventosModalDeBorrar('cerrar-modal-de-borrado')"
                , navegador: "");

            return htmlModal;
        }


        public override string RenderControl()
        {
            return RendelModal();
        }
    }
}