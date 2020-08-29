using ModeloDeDto;

namespace MVCSistemaDeElementos.Descriptores
{

    public class ModalDeRelacionarElementos<TElemento, TSeleccionado> : ControlFiltroHtml
    where TElemento : ElementoDto
    where TSeleccionado : ElementoDto
    {
        public DescriptorDeCrud<TSeleccionado> CrudModal { get; set; }

        public ModalDeRelacionarElementos(DescriptorDeMantenimiento<TElemento> mantenimiento, string tituloModal, DescriptorDeCrud<TSeleccionado> crudModal)
        : base(padre: mantenimiento
              ,id: $"{mantenimiento.Id}-{TipoControl.ModalDeRelacion}-{typeof(TSeleccionado).Name}"
              ,etiqueta: tituloModal
              ,propiedad: ""
              ,ayuda: ""
              ,posicion: null)
        {
            CrudModal = crudModal;
        }

        private string RenderModalDeRelacionarElementos()
        {
            string _htmlMiModal = $@"<div id=¨{IdHtml}¨ class=¨contenedor-modal¨ >
                              		<div id=¨{IdHtml}_contenido¨ class=¨contenido-modal modal-seleccion¨ >
                              		    <div id=¨{IdHtml}_cabecera¨ class=¨contenido-cabecera¨>
                              		    	titulo
                                        </div>
                              		    <div id=¨{IdHtml}_cuerpo¨ class=¨contenido-cuerpo¨>
                              			    crudDeSeleccion
                                        </div>
                                        <div id=¨{IdHtml}_pie¨ class=¨contenido-pie¨>
                                           <input type=¨text¨ id=¨{IdHtml}_Aceptar¨ class=¨boton-modal¨ value=¨Relacionar¨ readonly onclick=¨Crud.EventosModalDeSeleccion('seleccionar-elementos','{IdHtml}')¨/>
                                           <input type=¨text¨ id=¨{IdHtml}_Cerrar¨  class=¨boton-modal¨ value=¨Cerrar¨ readonly onclick=¨Crud.EventosModalDeSeleccion('cerrar-modal-seleccion','{IdHtml}')¨ />
                                        </div>
                                      </div>
                              </div>";

            return _htmlMiModal
                .Replace("titulo", Etiqueta)
                .Replace("crudDeSeleccion", CrudModal.RenderCrudModal(idModal: this.IdHtml));
        }

        public override string RenderControl()
        {
            return RenderModalDeRelacionarElementos();
        }

    }
}
