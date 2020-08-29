using ModeloDeDto;

namespace MVCSistemaDeElementos.Descriptores
{

    public class SelectorDeFiltro<TElemento,TSeleccionado> : ControlFiltroHtml where TElemento : ElementoDto where TSeleccionado : ElementoDto
    {
        public BloqueDeFitro<TElemento> Bloque => (BloqueDeFitro<TElemento>) Padre;
        public string propiedadParaFiltrar { get; private set; }
        public string propiedadParaMostrar { get; private set; }
        public ModalDeSeleccionDeFiltro<TElemento, TSeleccionado> Modal { get; set; }

        public string idBtnSelectorHtml => $"{IdHtml}_btnsel";

        public string PropiedadDondeMapear { get; private set; }

        public DescriptorDeCrud<TSeleccionado> CrudModal { get; private set; }

        public SelectorDeFiltro(BloqueDeFitro<TElemento> padre, string etiqueta, string filtrarPor, string ayuda, Posicion posicion, string paraFiltrar, string paraMostrar, DescriptorDeCrud<TSeleccionado> crudModal, string propiedadDondeMapear)
        : base(
          padre: padre
          , id: $"{padre.Id}_{TipoControl.Selector}_{filtrarPor}" //  $"{typeof(Tseleccionado).Name.Replace("Elemento", "")}_{TipoControl.Selector}"
          , etiqueta
          , filtrarPor
          , ayuda
          , posicion
          )
        {
            Tipo = TipoControl.Selector;
            propiedadParaFiltrar = paraFiltrar.ToLower();
            propiedadParaMostrar = paraMostrar.ToLower();
            Modal = new ModalDeSeleccionDeFiltro<TElemento, TSeleccionado>(this, crudModal);
            Criterio = TipoCriterio.igual.ToString();
            CrudModal = crudModal;
            PropiedadDondeMapear = propiedadDondeMapear;
            padre.AnadirSelector(this);
        }


        public string RenderSelector()
        {
            ControlHtml edt = CrudModal.Mnt.Filtro.BuscarControl(PropiedadDondeMapear);

            return $@"<div class=¨input-group¨>
                       <input id=¨{IdHtml}¨ 
                              type = ¨text¨ 
                              class=¨form-control¨ 
                              placeholder=¨{Ayuda}¨
                              {base.RenderAtributos()} 
                              criterioBuscar=¨{TipoCriterio.contiene}¨
                              propiedadBuscar=¨{FiltroPor.Nombre}¨
                              propiedadMostrar=¨{propiedadParaMostrar}¨
                              propiedadFiltrar=¨{propiedadParaFiltrar}¨
                              id-modal=¨{Modal.IdHtml}¨
                              id-grid-modal=¨{CrudModal.Mnt.Datos.IdHtml}¨
                              idBtnSelector=¨{idBtnSelectorHtml}¨
                              idEditorMostrar=¨{edt.IdHtml}¨
                              refCheckDeSeleccion=¨chksel.{CrudModal.Mnt.Datos.IdHtml}¨
                              onchange =¨Crud.EventosDelMantenimiento('cambiar-selector','{IdHtml}')¨>
                       <input type=¨text¨ 
                              id=¨{idBtnSelectorHtml}¨ 
                              class=¨boton-de-seleccion¨ 
                              value=¨...¨ 
                              onclick=¨Crud.EventosModalDeSeleccion('abrir-modal-seleccion', '{Modal.IdHtml}')¨       />
                    </div>
                  ";
        }

        public override string RenderControl()
        {
            return RenderSelector();
        }
    }
}
