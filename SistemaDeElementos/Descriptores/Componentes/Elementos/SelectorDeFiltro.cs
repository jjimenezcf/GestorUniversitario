using Gestor.Elementos.ModeloIu;

namespace MVCSistemaDeElementos.Descriptores
{

    public class SelectorDeFiltro<TElemento,TSeleccionado> : ControlFiltroHtml
    {
        public BloqueDeFitro<TElemento> Bloque => (BloqueDeFitro<TElemento>) Padre;
        public string propiedadParaFiltrar { get; private set; }
        public string propiedadParaMostrar { get; private set; }
        public ModalDeSeleccionDeFiltro<TElemento, TSeleccionado> Modal { get; set; }

        public string idBtnSelectorHtml => $"{IdHtml}_btnSel";

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
            padre.AnadirSelector(this);
            Criterio = TipoCriterio.igual.ToString();
            CrudModal = crudModal;
            PropiedadDondeMapear = propiedadDondeMapear;
        }


        public string RenderSelector()
        {
            ControlHtml edt = CrudModal.Mnt.Filtro.BuscarControl(PropiedadDondeMapear);

            return $@"<div class=¨input-group mb-3¨>
                       <input id=¨{IdHtml}¨ 
                              type = ¨text¨ 
                              class=¨form-control¨ 
                              placeholder=¨{Ayuda}¨
                              {base.RenderAtributos()} 
                              criterioBuscar=¨{TipoCriterio.contiene}¨
                              propiedadBuscar=¨{FiltroPor.Nombre}¨
                              propiedadMostrar=¨{propiedadParaMostrar}¨
                              propiedadFiltrar=¨{propiedadParaFiltrar}¨
                              idGridModal=¨{CrudModal.Mnt.Grid.IdHtml}¨
                              idBtnSelector=¨{idBtnSelectorHtml}¨
                              idEditorMostrar=¨{edt.IdHtml}¨
                              refCheckDeSeleccion=¨chksel.{CrudModal.Mnt.Grid.IdHtml}¨
                              onchange =¨AlCambiarTextoSelector('{IdHtml}', '{CrudModal.Controlador}')¨>
                       <div class=¨input-group-append¨>
                            <button id=¨{idBtnSelectorHtml}¨ 
                                    class=¨btn btn-outline-secondary¨ 
                                    type=¨button¨ 
                                    data-toggle=¨modal¨ 
                                    data-target=¨#{Modal.IdHtml}¨>Seleccionar</button>
                       </div>
                    </div>
                  ";
        }

        public override string RenderControl()
        {
            return RenderSelector();
        }
    }
}
