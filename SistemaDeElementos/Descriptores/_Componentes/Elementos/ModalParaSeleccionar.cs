using Enumerados;
using ModeloDeDto;

namespace MVCSistemaDeElementos.Descriptores
{

    public class ModalParaSeleccionar<TSeleccionado> : ControlFiltroHtml
    where TSeleccionado : ElementoDto
    {
        public DescriptorDeCrud<TSeleccionado> CrudModal { get; set; }

        public string PropiedadRestrictora { get; private set; }

        public ModalParaSeleccionar(ControlHtml controlPadre, string tituloModal, DescriptorDeCrud<TSeleccionado> crudModal, string propiedadRestrictora)
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

        public string RenderModalParaSeleccionar()
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
                              			    crudParaSeleccionar
                                        </div>
                                        <div id=¨{IdHtml}_pie¨ class=¨contenido-pie¨>
                                           <input type=¨text¨ id=¨{IdHtml}-cerrar¨  class=¨boton-modal¨ value=¨Cerrar¨ readonly onclick=¨Crud.{GestorDeEventos.EventosModalParaSeleccionar}('{TipoDeAccionParaSeleccionar.Cerrar}','{IdHtml}')¨ />
                                        </div>
                                      </div>
                              </div>";

            return _htmlMiModal
                .Replace("titulo", Etiqueta)
                .Replace("crudParaSeleccionar", CrudModal.RenderCrudModal(idModal: this.IdHtml, enumTipoDeModal.ModalParaSeleccionar));
        }

    }
}
