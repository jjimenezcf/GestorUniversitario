using System;
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

        public string idBtnSelectorHtml => $"{IdHtml}_btnsel";

        public string idHtmlEditor => $"{IdHtml}_editor";

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


        public string RenderSelector()
        {
            return RenderControl();
        }

        public override string RenderControl()
        {

            var html = $@"<div id=¨{IdHtml}¨ class=¨{enumCssSelectorEnModal.Contenedor.Render()} propiedad=¨{PropiedadHtml}¨ idSeleccionados=¨¨ idEditor=¨{idHtmlEditor}¨ idBotonSelector=¨{idBtnSelectorHtml}¨>
                           {RenderEditor()}{RenderBotonSelector()}
                          </div>";


            return html;
        }

        private string RenderEditor()
        {
            var a = AtributosHtml.AtributosComunes($"div_{idHtmlEditor}", idHtmlEditor, Propiedad, enumTipoControl.Editor);
            a.Editable = false;
            a.Etiqueta = Etiqueta;
            a.Ayuda = Ayuda;
            a.AlPerderElFoco = $"onBlur = ¨Crud.{GestorDeEventos.EventosSelectorEnModal}('{TipoDeAccionSelectorEnModal.perderFoco}')¨";


            return RenderEditorConEtiquetaEncima(PlantillasHtml.editorDto, a);
        }
        private string RenderBotonSelector()
        {
            //indicar el idDivSelector= ....


            var o = new OpcionHtml(this, "boton-seleccion"
                , "Seleccionar usuarios"
                , "marque los usuarios a los que enviar el correo"
                , $"Crud.{GestorDeEventos.EventosModalDeEnviarCorreo}('{TipoDeAccionDeEnviarCorreo.SeleccionaUsuarios}','{ModalDeUsuarios.IdHtml}')"
                );

            return "";
        }
    }
}
