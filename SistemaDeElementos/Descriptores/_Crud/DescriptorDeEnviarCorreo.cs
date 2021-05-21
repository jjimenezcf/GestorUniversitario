using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enumerados;
using ModeloDeDto;
using ModeloDeDto.Entorno;
using ModeloDeDto.Seguridad;
using ModeloDeDto.TrabajosSometidos;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Seguridad;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeEnviarCorreo<TElemento> : ControlHtml where TElemento : ElementoDto
    {
        public DescriptorDeCrud<TElemento> Crud => (DescriptorDeCrud<TElemento>)Padre;
        public DescriptorDeMantenimiento<TElemento> Mnt => Crud.Mnt;

        private ModalParaSeleccionar<UsuarioDto> ModalDeUsuarios { get; }
        private ModalParaSeleccionar<PuestoDto> ModalDePuestos { get; }

        private SelectorEnModal<UsuarioDto> SelectorDeUsuarios { get; }
        private SelectorEnModal<PuestoDto> SelectorDePuestoTr { get; }

        public DescriptorDeEnviarCorreo(DescriptorDeCrud<TElemento> crud)
        : base(
          padre: crud,
          id: $"{crud.Id}_{enumTipoControl.pnlEnviarCorreo.Render()}",
          etiqueta: "Envío de correo",
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = enumTipoControl.pnlEnviarCorreo;

            ModalDeUsuarios = new ModalParaSeleccionar<UsuarioDto>(this,
                                         tituloModal: "Seleccionar usuario",
                                         crudModal: new DescriptorDeUsuario(ModoDescriptor.ParaSeleccionar),
                                         propiedadRestrictora: "");

            ModalDePuestos = new ModalParaSeleccionar<PuestoDto>(this,
                                         tituloModal: "Seleccionar puestos de trabajo",
                                         crudModal: new DescriptorDePuestoDeTrabajo(ModoDescriptor.ParaSeleccionar),
                                         propiedadRestrictora: "");


            SelectorDeUsuarios = new SelectorEnModal<UsuarioDto>(this, "selector-usuario", "Usuario", "Seleccione usuarios", "IdsDeUsuarios", nameof(UsuarioDto.Id), nameof(UsuarioDto.NombreCompleto), ModalDeUsuarios);
            SelectorDePuestoTr = new SelectorEnModal<PuestoDto>(this, "selector-puestos", "Puestos", "Seleccione puestos", "IdsDePuestos", nameof(PuestoDto.Id), nameof(UsuarioDto.Nombre), ModalDePuestos);

        }

        public string RenderDeEnvioDeCorreo()
        {
            return RenderControl();
        }

        public override string RenderControl()
        {
                var htmlModal = RenderizarModal(
                    idHtml: IdHtml
                    , controlador: Crud.Controlador
                    , tituloH2: Etiqueta
                    , cuerpo: cuerpoDeEnvioDeCorreo()
                    , idOpcion: $"{IdHtml}-enviar"
                    , opcion: Crud.NegocioActivo ? "Enviar" : ""
                    , accion: Crud.NegocioActivo ? $"Crud.{GestorDeEventos.EventosModalDeEnviarCorreo}('{TipoDeAccionDeEnviarCorreo.Enviar}','{IdHtml}')" : ""
                    , cerrar: $"Crud.{GestorDeEventos.EventosModalDeEnviarCorreo}('{TipoDeAccionDeEnviarCorreo.Cerrar}','{IdHtml}')"
                    , navegador: ""
                    , claseBoton: enumCssOpcionMenu.DeElemento
                    , permisosNecesarios: enumModoDeAccesoDeDatos.Consultor);

            return htmlModal;
        }

        private string cuerpoDeEnvioDeCorreo()
        {
            //
            var htmlCuerpo = $@"<div id=¨{IdHtml}_cuerpo_contenedor¨ class=¨{enumCssEnviarCorreo.Contenedor.Render()}¨>
                                     <div id=¨{IdHtml}_cuerpo_enviar_correo¨ class=¨{enumCssEnviarCorreo.cabecera.Render()}¨>
                                        PuestosDeTrabajoReceptores
                                        UsuariosReceptores
                                     </div>
                                     <div id=¨{IdHtml}_cuerpo_sometido¨ class=¨{enumCssEnviarCorreo.cuerpo.Render()}¨>
                                        Asunto
                                        {RenderTextArea($"{IdHtml}_mensaje", "mensaje", "cuerpoMensaje", "indique el mensaje")}
                                     </div>
                                     <div id=¨{IdHtml}_cuerpo_enviar¨ class=¨{enumCssEnviarCorreo.adjuntos.Render()}¨>                                        
                                        Elementos
                                     </div>
                                </div>
                                ";

            var htmlUsuariosReceptores = SelectorDeUsuarios.RenderSelectorEnModal();
            var htmlPuestosDeTrabajoReceptores = SelectorDePuestoTr.RenderSelectorEnModal();

            htmlCuerpo = htmlCuerpo.Replace("UsuariosReceptores", htmlUsuariosReceptores);
            htmlCuerpo = htmlCuerpo.Replace("PuestosDeTrabajoReceptores", htmlPuestosDeTrabajoReceptores);
            htmlCuerpo = htmlCuerpo.Replace("Asunto", RenderEditorAsunto());
            return htmlCuerpo;
        }

        private string RenderEditorAsunto()
        {

            //    var otrosAtributos = new Dictionary<string, string>();
            //    otrosAtributos["estiloEtiqueta"] = "style='padding :0px;'";

            var idHtmlAsunto = $"{IdHtml}_asunto";
            var a = AtributosHtml.AtributosComunes($"div_{idHtmlAsunto}", idHtmlAsunto, "", enumTipoControl.Editor);
            a.Editable = true;
            a.Ayuda = "indique el asunto";
            a.Etiqueta = "Asunto";

            return RenderEditorConEtiquetaEncima(a);
        }

        internal object RenderDeModalesParaSeleccionarReceptores()
        {
            return SelectorDeUsuarios.RenderModalAsociadaAlSelector() + Environment.NewLine + SelectorDePuestoTr.RenderModalAsociadaAlSelector();
        }


    }
}
