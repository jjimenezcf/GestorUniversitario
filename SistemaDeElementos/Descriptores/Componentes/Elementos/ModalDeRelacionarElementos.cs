using ModeloDeDto;

namespace MVCSistemaDeElementos.Descriptores
{

    public class ModalDeRelacionarElementos<TElemento, TSeleccionado> : ControlFiltroHtml
    where TElemento : ElementoDto
    where TSeleccionado : ElementoDto
    {
        public DescriptorDeCrud<TSeleccionado> CrudModal { get; set; }

        public string PropiedadRestrictora { get; private set; }

        public ModalDeRelacionarElementos(DescriptorDeMantenimiento<TElemento> mantenimiento, string tituloModal, DescriptorDeCrud<TSeleccionado> crudModal, string propiedadRestrictora)
        : base(padre: mantenimiento
              ,id: $"{mantenimiento.Id}-{TipoControl.ModalDeRelacion}-{typeof(TSeleccionado).Name}"
              ,etiqueta: tituloModal
              ,propiedad: ""
              ,ayuda: ""
              ,posicion: null)
        {
            CrudModal = crudModal;
            PropiedadRestrictora = propiedadRestrictora.ToLower();
        }

        private string RenderModalDeRelacionarElementos()
        {
            string _htmlMiModal = $@"<div id=¨{IdHtml}¨ class=¨contenedor-modal¨ crud-modal=¨{CrudModal.Mnt.IdHtml}¨ propiedad-restrictora=¨{PropiedadRestrictora}¨>
                              		    <div id=¨{IdHtml}_contenido¨ class=¨contenido-modal¨ >
                              		       <div id=¨{IdHtml}_cabecera¨ class=¨contenido-cabecera¨>
                              		       	titulo
                                           </div>
                              		       <div id=¨{IdHtml}_cuerpo¨ class=¨contenido-cuerpo¨>
                              		 	       crudDeSeleccion
                                           </div>
                                           <div id=¨{IdHtml}_pie¨ class=¨contenido-pie¨>
                                              <input type=¨text¨ id=¨{IdHtml}-relacionar¨ class=¨boton-modal¨ value=¨Relacionar¨ readonly onclick=¨Crud.EventosModalDeCrearRelaciones('{TipoDeAccionDeRelacionar.Relacionar}','{IdHtml}')¨/>
                                              <input type=¨text¨ id=¨{IdHtml}-cerrar¨  class=¨boton-modal¨ value=¨Cerrar¨ readonly onclick=¨Crud.EventosModalDeCrearRelaciones('{TipoDeAccionDeRelacionar.Cerrar}','{IdHtml}')¨ />
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
