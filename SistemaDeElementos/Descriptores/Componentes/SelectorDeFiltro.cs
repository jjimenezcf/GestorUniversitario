﻿using Gestor.Elementos.ModeloIu;

namespace MVCSistemaDeElementos.Descriptores
{

    public class SelectorDeFiltro<Tseleccionado> : ControlFiltroHtml
    {
        public string propiedadParaFiltrar { get; private set; }
        public string propiedadParaMostrar { get; private set; }
        public ModalDeSeleccionDeFiltro<Tseleccionado> Modal { get; set; }

        public string idBtnSelectorHtml => $"{IdHtml}_btnSel";

        public string PropiedadDondeMapear { get; private set; }

        public DescriptorDeCrud<Tseleccionado> Descriptor { get; private set; }

        public SelectorDeFiltro(BloqueDeFitro padre, string etiqueta, string propiedad, string ayuda, Posicion posicion, string paraFiltrar, string paraMostrar, DescriptorDeCrud<Tseleccionado> descriptor, string propiedadDondeMapear)
        : base(
          padre: padre
          , id: $"{padre.Id}_{TipoControl.Selector}_{propiedad}" //  $"{typeof(Tseleccionado).Name.Replace("Elemento", "")}_{TipoControl.Selector}"
          , etiqueta
          , propiedad
          , ayuda
          , posicion
          )
        {
            Tipo = TipoControl.Selector;
            propiedadParaFiltrar = paraFiltrar.ToLower();
            propiedadParaMostrar = paraMostrar.ToLower();
            Modal = new ModalDeSeleccionDeFiltro<Tseleccionado>(padre, this, descriptor);
            padre.AnadirSelector(this);
            Criterio = TipoCriterio.igual.ToString();
            Descriptor = descriptor;
            PropiedadDondeMapear = propiedadDondeMapear;
        }


        public string RenderSelector()
        {
            ControlHtml edt = Descriptor.Filtro.BuscarControl(PropiedadDondeMapear);

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
                              idGridModal=¨{Descriptor.Grid.IdHtml}¨
                              idBtnSelector=¨{idBtnSelectorHtml}¨
                              idEditorMostrar=¨{edt.IdHtml}¨
                              refCheckDeSeleccion=¨chksel.{Descriptor.Grid.IdHtml}¨
                              onchange =¨AlCambiarTextoSelector('{IdHtml}', '{Descriptor.Controlador}')¨>
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
