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
                              		    <div id=¨{IdHtml}_cabecera¨ class=¨cotenido-cabecera¨>
                              		    	<h2>Confirmación de borrado</h2>
                                        </div>
                              		    <div id=¨{IdHtml}_cuerpo¨ class=¨cotenido-cuerpo¨>
                              			    <input type=¨text¨ id=¨{IdHtml}_mensaje¨ class=¨mensaje-modal¨ value=¨Desea borrar el elemento seleccionado¨></input>
                                        </div>
                                        <div id=¨{IdHtml}_pie¨ class=¨cotenido-pie¨>
                                           <input type=¨text¨ id=¨{IdHtml}_Aceptar¨ class=¨boton-modal¨ value=¨Aceptar¨ onclick=¨Crud.EjecutarMenuMnt('borrarelemento')¨       />
                                           <input type=¨text¨ id=¨{IdHtml}_Cerrar¨  class=¨boton-modal¨ value=¨Cerrar¨  onclick=¨Crud.EjecutarMenuMnt('cerrarmodaldeborrado')¨ />
                                        </div>
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