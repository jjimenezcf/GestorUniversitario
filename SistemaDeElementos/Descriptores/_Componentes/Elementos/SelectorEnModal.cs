using System;
using System.Collections.Generic;
using Enumerados;
using GestorDeElementos;
using ModeloDeDto;

namespace MVCSistemaDeElementos.Descriptores
{

    public class SelectorEnModal<TSeleccionado> : ControlFiltroHtml where TSeleccionado : ElementoDto
    {
        public string propiedadParaFiltrar { get; private set; }
        public string propiedadParaMostrar { get; private set; }
        public DescriptorDeCrud<TSeleccionado> Modal { get; set; }

        public string IdBtnSelectorHtml => $"{IdHtml}_btnsel";

        public string IdHtmlEditor => $"{IdHtml}_editor";

        public string PropiedadDondeMapear { get; private set; }

        public DescriptorDeCrud<TSeleccionado> CrudModal { get; private set; }

        //la propiedad es el parámetro que se enviará en la llamada ajax
        public SelectorEnModal(ControlHtml padre,string id,  string etiqueta, string ayuda, string propiedad, string paraFiltrar, string paraMostrar, DescriptorDeCrud<TSeleccionado> crudModal)
        : base(
          padre: padre
          , id: $"{padre.Id}_{id}" 
          , etiqueta
          , propiedad
          , ayuda
          , null
          )
        {
            Tipo = enumTipoControl.SelectorDeFiltro;
            propiedadParaFiltrar = paraFiltrar.ToLower();
            propiedadParaMostrar = paraMostrar.ToLower();
            Modal = crudModal;
            Criterio = CriteriosDeFiltrado.igual;
            CrudModal = crudModal;
        }


        public string RenderModalDeSeleccion()
        {
            return CrudModal.RenderCrudModal(CrudModal.IdHtml, enumTipoDeModal.ModalDeSeleccion);
        }


        public string RenderSelector()
        {
            return RenderControl();
        }

        public override string RenderControl()
        {

            var html = $@"<div id=¨{IdHtml}¨ class=¨{enumCssSelectorEnModal.Contenedor.Render()}¨ propiedad=¨{PropiedadHtml}¨ idSeleccionados=¨¨ idEditor=¨{IdHtmlEditor}¨ idBotonSelector=¨{IdBtnSelectorHtml}¨>
                             {RenderEditorDelSelector()}
                             {RenderBotonSelector()}
                          </div>";


            return html;
        }

        private string RenderEditorDelSelector()
        {

            var otrosAtributos = new Dictionary<string, string>();
            otrosAtributos["onBlur"] = $"onBlur = ¨Crud.{GestorDeEventos.EventosSelectorEnModal}('{TipoDeAccionSelectorEnModal.PerderFoco}')¨";

            var div = $@"
            <div id=¨div_{IdHtmlEditor}_contenedor¨ name=¨contenedor-control¨ class={enumCssSelectorEnModal.Editor.Render()}¨>
               {RenderEditorConEtiquetaIzquierda(IdHtmlEditor,Etiqueta,Propiedad,Ayuda, otrosAtributos)}
            </div>
            ";

            return div;
        }
        private string RenderBotonSelector()
        {
            var a = AtributosHtml.AtributosComunes($"div-{IdBtnSelectorHtml}", IdHtmlEditor, Propiedad, enumTipoControl.SelectorDeElemento);
            a.AlPulsarElBoton = $"onClick = ¨Crud.{GestorDeEventos.EventosSelectorEnModal}('{TipoDeAccionSelectorEnModal.OpcionSeleccionada}','{Modal.IdHtml}')¨";

            Dictionary<string, object> valores = a.MapearComunes();

            valores["CssContenedor"] = enumCssControlesDto.ContenedorEditor.Render();
            valores["Css"] = enumCssControlesDto.BotonSelector.Render();
            valores["Placeholder"] = a.Ayuda;
            valores["onClick"] = a.AlPulsarElBoton;


            return PlantillasHtml.Render(PlantillasHtml.BotonSeleccion, valores); 
        }
    }
}
