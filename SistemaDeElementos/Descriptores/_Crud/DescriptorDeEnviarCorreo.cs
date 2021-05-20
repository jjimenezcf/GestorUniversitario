using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enumerados;
using ModeloDeDto;
using ModeloDeDto.Entorno;
using ModeloDeDto.TrabajosSometidos;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Seguridad;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeEnviarCorreo<TElemento> : ControlHtml where TElemento : ElementoDto
    {
        public DescriptorDeCrud<TElemento> Crud => (DescriptorDeCrud<TElemento>)Padre;
        public DescriptorDeMantenimiento<TElemento> Mnt => Crud.Mnt;

        private DescriptorDeUsuario ModalDeUsuarios { get; }

        private SelectorEnModal<UsuarioDto> SelectorDeUsuarios { get; }

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

            ModalDeUsuarios = new DescriptorDeUsuario(ModoDescriptor.ParaSeleccionar);
            SelectorDeUsuarios = new SelectorEnModal<UsuarioDto>(this, "selector-usuario", "Usuario", "Seleccione usuarios", "IdsDeUsuarios", nameof(UsuarioDto.Id), nameof(UsuarioDto.NombreCompleto), ModalDeUsuarios);
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

            return htmlModal + Environment.NewLine + " <!--  ******************* modal para selector de usuario ******************* -->" + Environment.NewLine + RenderUsuarioReceptor();
        }

        private string cuerpoDeEnvioDeCorreo()
        {
            //
            var htmlCuerpo = $@"<div id=¨{IdHtml}_cuerpo_contenedor¨ class=¨{enumCssEnviarCorreo.Contenedor.Render()}¨>
                                     <div id=¨{IdHtml}_cuerpo_enviar_correo¨ class=¨{enumCssEnviarCorreo.cabecera.Render()}¨>
                                        {RenderSelectorDeUsuario()}
                                     </div>
                                     <div id=¨{IdHtml}_cuerpo_sometido¨ class=¨{enumCssEnviarCorreo.cuerpo.Render()}¨>
                                        Asunto
                                        Cuerpo
                                     </div>
                                     <div id=¨{IdHtml}_cuerpo_enviar¨ class=¨{enumCssEnviarCorreo.adjuntos.Render()}¨>                                        
                                        Elementos
                                     </div>
            //                    </div>";
            return htmlCuerpo;
        }

        private object RenderUsuarioReceptor()
        {
            return SelectorDeUsuarios.RenderModalDeSeleccion();
        }

        private string listaDeExportaciones()
        {
            return RenderListaConEtiquetaEncima(IdHtml, "ExportacionDto", "Nombre", "Plantilla").Replace("Seleccionar ...", "Estandard");
        }
        private string checkDeSometido()
        {
            var accion = $"onClick = ¨Crud.{GestorDeEventos.EventosModalDeExportacion}('{TipoDeAccionDeExportar.PulsarSometer}')¨";
            return RenderCheck(PlantillasHtml.checkDto, $"{IdHtml}_sometido", ltrExportacion.sometido, true, "Someter", accion) +
                   RenderCheck(PlantillasHtml.checkDto, $"{IdHtml}_mostradas", "", true, "Las mostradas", accion);
        }

        private string RenderSelectorDeUsuario()
        {
            return SelectorDeUsuarios.RenderSelector();
        }
    }
}
