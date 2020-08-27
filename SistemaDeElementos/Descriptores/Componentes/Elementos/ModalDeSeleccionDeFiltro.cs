using ModeloDeDto;

namespace MVCSistemaDeElementos.Descriptores
{

    public class ModalDeSeleccionDeFiltro<TElemento, TSeleccionado> : ControlFiltroHtml where TElemento : ElementoDto where TSeleccionado : ElementoDto
    {
        public SelectorDeFiltro<TElemento, TSeleccionado> Selector { get; set; }
        public DescriptorDeCrud<TSeleccionado> CrudModal { get; set; }

        public string Titulo => Ayuda;

        public ModalDeSeleccionDeFiltro(SelectorDeFiltro<TElemento, TSeleccionado> selector, DescriptorDeCrud<TSeleccionado> crudModal)
        : base(
          padre: selector.Bloque,
          id: $"Modal_{selector.IdHtml}",    //{(selectorAsociado.Id.Replace("_" + TipoControl.Selector.ToString(), ""))}",
          etiqueta: $"Seleccionar {selector.propiedadParaMostrar}",
          propiedad: selector.propiedadParaMostrar,
          ayuda: selector.Ayuda,
          posicion: null
        )
        {
            Tipo = TipoControl.GridModal;
            Selector = selector;
            Selector.Modal = this;
            CrudModal = crudModal;
        }


        private string RenderGridModal()
        {
            string _htmlMiModal = $@"<div id=¨{IdHtml}¨ class=¨contenedor-modal¨ selector=¨idSelector¨ crud-modal=¨{CrudModal.Mnt.IdHtml}¨>
                              		<div id=¨{IdHtml}_contenido¨ class=¨contenido-modal modal-seleccion¨ >
                              		    <div id=¨{IdHtml}_cabecera¨ class=¨contenido-cabecera¨>
                              		    	titulo
                                        </div>
                              		    <div id=¨{IdHtml}_cuerpo¨ class=¨contenido-cuerpo¨>
                              			    crudDeSeleccion
                                        </div>
                                        <div id=¨{IdHtml}_pie¨ class=¨contenido-pie¨>
                                           <input type=¨text¨ id=¨{IdHtml}_Aceptar¨ class=¨boton-modal¨ value=¨Seleccionar¨ readonly onclick=¨Crud.EventosModalDeSeleccion('seleccionar-elementos','{IdHtml}')¨       />
                                           <input type=¨text¨ id=¨{IdHtml}_Cerrar¨  class=¨boton-modal¨ value=¨Cerrar¨ readonly onclick=¨Crud.EventosModalDeSeleccion('cerrar-modal-seleccion','{IdHtml}')¨ />
                                        </div>
                                      </div>
                              </div>";

            return _htmlMiModal
                .Replace("titulo", Titulo)
                .Replace("crudDeSeleccion", CrudModal.RenderCrudModal(idModal: this.IdHtml))
                //.Replace("idGrid", CrudModal.Mnt.Datos.IdHtml)
                .Replace("idSelector", Selector.IdHtml);

        }

        public override string RenderControl()
        {
            return RenderGridModal();
        }
    }
}
