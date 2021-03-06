﻿using System;
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
        public ModalParaSeleccionar<TSeleccionado> Modal { get; set; }

        public string IdBtnSelectorHtml => $"{IdHtml}_btnsel";

        public string IdHtmlEditor => $"{IdHtml}_editor";

        public string PropiedadDondeMapear { get; private set; }

        //la propiedad es el parámetro que se enviará en la llamada ajax
        public SelectorEnModal(ControlHtml padre,string id,  string etiqueta, string ayuda, string propiedad, string paraFiltrar, string paraMostrar, ModalParaSeleccionar<TSeleccionado> crudModal)
        : base(
          padre: padre
          , id: $"{padre.Id}_{id}" 
          , etiqueta
          , propiedad
          , ayuda
          , null
          )
        {
            Tipo = enumTipoControl.SelectorDeElemento;
            propiedadParaFiltrar = paraFiltrar.ToLower();
            propiedadParaMostrar = paraMostrar.ToLower();
            Modal = crudModal;
            Criterio = CriteriosDeFiltrado.igual;
        }


        internal string RenderModalAsociadaAlSelector()
        {
            return Modal.RenderModalParaSeleccionar();
        }


        public string RenderSelectorEnModal()
        {
            return RenderControl();
        }

        public override string RenderControl()
        {
            var editorDeFiltro = Modal.CrudModal.BuscarControlEnFiltro(propiedadParaMostrar);

            var html = $@"<div id=¨{IdHtml}¨ class=¨{enumCssSelectorEnModal.Contenedor.Render()}¨ 
                              tipo=¨{Tipo.Render()}¨ 
                              propiedad=¨{PropiedadHtml}¨ 
                              idSeleccionados=¨¨ 
                              idEditor=¨{IdHtmlEditor}¨ 
                              idBotonSelector=¨{IdBtnSelectorHtml}¨
                              idEditorDelFiltro=¨{editorDeFiltro.IdHtml}¨>
                             {RenderEditorDelSelector()}
                             {RenderBotonSelectorEnModal()}
                          </div>";


            return html;
        }

        private string RenderEditorDelSelector()
        {
            var otrosAtributosEditor = new Dictionary<string, string>();
            otrosAtributosEditor["onBlur"] = $"onblur = ¨Crud.{GestorDeEventos.EventosDeSelectorDeElementosEnModal}('{TipoDeAccionSelectorEnModal.PerderFoco}','{Modal.IdHtml}#{Padre.IdHtml}#{IdHtml}')¨";
            otrosAtributosEditor["onFocus"] = $"onfocus = ¨Crud.{GestorDeEventos.EventosDeSelectorDeElementosEnModal}('{TipoDeAccionSelectorEnModal.ObtenerFoco}','{IdHtml}')¨";


            var otrosAtributosEtiqueta = new Dictionary<string, string>();
            otrosAtributosEtiqueta["estilo"] = "style='padding :0px;'";

            var div = $@"
            <div id=¨div_{IdHtmlEditor}_contenedor¨ name=¨contenedor-control¨ class={enumCssSelectorEnModal.Editor.Render()}¨>
               {RenderEditorConEtiquetaIzquierda(IdHtmlEditor,Etiqueta,Propiedad,Ayuda, otrosAtributosEditor, otrosAtributosEtiqueta)}
            </div>
            ";

            return div;
        }
        private string RenderBotonSelectorEnModal()
        {
            var htmlBotonSelector = $@"
                <div id = ¨{IdBtnSelectorHtml}_contenedor¨ class=¨{enumCssControlesDto.ContenedorBotonSelector.Render()}¨>
                   <input id=¨{IdBtnSelectorHtml}¨ 
                          type=¨button¨ 
                          class=¨{enumCssControlesDto.BotonSelector.Render()}¨ 
                          value=¨...¨ 
                          onClick = ¨Crud.{GestorDeEventos.EventosDeSelectorDeElementosEnModal}('{TipoDeAccionSelectorEnModal.OpcionSeleccionada}','{Modal.IdHtml}#{Padre.IdHtml}#{IdHtml}')¨
                          title=¨{Ayuda}¨/>
                </div>
                 ";

            return htmlBotonSelector;
        }
    }
}
