using ModeloDeDto;

namespace MVCSistemaDeElementos.Descriptores
{

    public class ModalDeConsultaDeRelaciones<TElemento, TSeleccionado> : ControlFiltroHtml
    where TElemento : ElementoDto
    where TSeleccionado : ElementoDto
    {
        public DescriptorDeCrud<TSeleccionado> CrudModal { get; set; }

        public string PropiedadRestrictora { get; private set; }

        public ModalDeConsultaDeRelaciones(DescriptorDeMantenimiento<TElemento> mantenimiento, string tituloModal, DescriptorDeCrud<TSeleccionado> crudModal, string propiedadRestrictora)
        : base(padre: mantenimiento
              ,id: $"{mantenimiento.Id}-{TipoControl.ModalDeConsulta}-{typeof(TSeleccionado).Name}"
              ,etiqueta: tituloModal
              ,propiedad: ""
              ,ayuda: ""
              ,posicion: null)
        {
            CrudModal = crudModal;
            PropiedadRestrictora = propiedadRestrictora.ToLower();
        }

        private string RenderModalDeConsultaDeRelaciones()
        {
            string _htmlMiModal = $@"<div id=¨{IdHtml}¨ class=¨contenedor-modal¨ crud-modal=¨{CrudModal.Mnt.IdHtml}¨ propiedad-restrictora=¨{PropiedadRestrictora}¨>
                              		<div id=¨{IdHtml}_contenido¨ class=¨contenido-modal modal-seleccion¨ >
                              		    <div id=¨{IdHtml}_cabecera¨ class=¨contenido-cabecera¨>
                              		    	titulo
                                        </div>
                              		    <div id=¨{IdHtml}_cuerpo¨ class=¨contenido-cuerpo¨>
                              			    crudDeConsulta
                                        </div>
                                        <div id=¨{IdHtml}_pie¨ class=¨contenido-pie¨>
                                           <input type=¨text¨ id=¨{IdHtml}-cerrar¨  class=¨boton-modal¨ value=¨Cerrar¨ readonly onclick=¨Crud.{GestorDeEventos.EventosModalDeConsultaDeRelaciones}('{TipoDeAccionDeConsulta.Cerrar}','{IdHtml}')¨ />
                                        </div>
                                      </div>
                              </div>";

            return _htmlMiModal
                .Replace("titulo", Etiqueta)
                .Replace("crudDeConsulta", CrudModal.RenderCrudModal(idModal: this.IdHtml));
        }

        public override string RenderControl()
        {
            return RenderModalDeConsultaDeRelaciones();
        }

    }
}
