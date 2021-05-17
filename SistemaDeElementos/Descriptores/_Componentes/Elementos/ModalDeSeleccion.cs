using Enumerados;
using ModeloDeDto;

namespace MVCSistemaDeElementos.Descriptores
{

    public class ModalDeSeleccion<TSeleccionado> : ControlFiltroHtml
    where TSeleccionado : ElementoDto
    {
        public DescriptorDeCrud<TSeleccionado> CrudModal { get; set; }

        public string PropiedadRestrictora { get; private set; }

        public ModalDeSeleccion(ControlHtml controlPadre, string tituloModal, DescriptorDeCrud<TSeleccionado> crudModal, string propiedadRestrictora)
        : base(padre: controlPadre
              ,id: $"{controlPadre.Id}-{enumTipoControl.ModalDeSeleccion.Render()}-{typeof(TSeleccionado).Name}"
              ,etiqueta: tituloModal
              ,propiedad: ""
              ,ayuda: ""
              ,posicion: null)
        {
            CrudModal = crudModal;
            PropiedadRestrictora = propiedadRestrictora.ToLower();
        }

        public string RenderModalDeSeleccion()
        {
          return RenderControl();
        }

        public override string RenderControl()
        {
            string _htmlMiModal = $@"<div id=¨{IdHtml}¨ class=¨contenedor-modal¨ crud-modal=¨{CrudModal.Mnt.IdHtml}¨ propiedad-restrictora=¨{PropiedadRestrictora}¨>
                              		<div id=¨{IdHtml}_contenido¨ class=¨contenido-modal¨ >
                              		    <div id=¨{IdHtml}_cabecera¨ class=¨contenido-cabecera¨>
                              		    	titulo
                                        </div>
                              		    <div id=¨{IdHtml}_cuerpo¨ class=¨contenido-cuerpo¨>
                              			    crudDeConsulta
                                        </div>
                                        <div id=¨{IdHtml}_pie¨ class=¨contenido-pie¨>
                                           <input type=¨text¨ id=¨{IdHtml}-cerrar¨  class=¨boton-modal¨ value=¨Cerrar¨ readonly onclick=¨Crud.{GestorDeEventos.EventosModalDeSeleccion}('{TipoDeAccionDeConsulta.Cerrar}','{IdHtml}')¨ />
                                        </div>
                                      </div>
                              </div>";

            return _htmlMiModal
                .Replace("titulo", Etiqueta)
                .Replace("crudDeConsulta", CrudModal.RenderCrudModal(idModal: this.IdHtml, enumTipoDeModal.ModalDeConsulta));
        }

    }
}
