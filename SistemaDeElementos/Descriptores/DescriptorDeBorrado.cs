using Gestor.Elementos.ModeloIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeBorrado<TElemento> : ControlHtml where TElemento : ElementoDto
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
                              		<div id=¨{IdHtml}-contenido¨ class=¨cotenido-modal¨>
                              		    <div id=¨{IdHtml}-cabecera¨ class=¨cotenido-cabecera¨>
                              		    	<h2>Confirmación de borrado</h2>
                                        </div>
                              		    <div id=¨{IdHtml}-cuerpo¨ class=¨cotenido-cuerpo¨>
                              			    <input type=¨text¨ id=¨{IdHtml}-mensaje¨ class=¨mensaje-modal¨ value=¨Desea borrar el elemento seleccionado¨></input>
                                        </div>
                                        <div id=¨{IdHtml}_pie¨ class=¨cotenido-pie¨>
                                           <input type=¨text¨ id=¨{IdHtml}-aceptar¨ class=¨boton-modal¨ value=¨Aceptar¨ readonly onclick=¨Crud.EventosModalDeBorrar('borrar-elemento')¨       />
                                           <input type=¨text¨ id=¨{IdHtml}-cerrar¨  class=¨boton-modal¨ value=¨Cerrar¨  readonly onclick=¨Crud.EventosModalDeBorrar('cerrar-modal-de-borrado')¨ />
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