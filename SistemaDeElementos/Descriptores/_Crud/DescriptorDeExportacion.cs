using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enumerados;
using ModeloDeDto;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Seguridad;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeExportacion<TElemento> : ControlHtml where TElemento : ElementoDto
    {
        public DescriptorDeCrud<TElemento> Crud => (DescriptorDeCrud<TElemento>)Padre;
        public DescriptorDeMantenimiento<TElemento> Mnt => Crud.Mnt;
        public DescriptorDeExportacion(DescriptorDeCrud<TElemento> crud)
        : base(
          padre: crud,
          id: $"{crud.Id}_{enumTipoControl.pnlExportacion.Render()}",
          etiqueta: "Selección de exportación",
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = enumTipoControl.pnlExportacion;
        }

        public string RenderDeExportacion()
        {
            return RenderControl();
        }

        public override string RenderControl()
        {
                var htmlModal = RenderizarModal(
                    idHtml: IdHtml
                    , controlador: Crud.Controlador
                    , tituloH2: Etiqueta
                    , cuerpo: cuerpoDeExportacion()
                    , idOpcion: $"{IdHtml}-exportar"
                    , opcion: Crud.NegocioActivo ? "Exportar" : ""
                    , accion: Crud.NegocioActivo ? $"Crud.{GestorDeEventos.EventosModalDeExportacion}('{TipoDeAccionDeExportar.Exportar}','{IdHtml}')" : ""
                    , cerrar: $"Crud.{GestorDeEventos.EventosModalDeExportacion}('{TipoDeAccionDeExportar.Cerrar}','{IdHtml}')"
                    , navegador: ""
                    , claseBoton: enumCssOpcionMenu.DeElemento
                    , permisosNecesarios: enumModoDeAccesoDeDatos.Consultor);

            return htmlModal;
        }

        private string cuerpoDeExportacion()
        {
            var htmlCuerpo = $@"<div id=¨{IdHtml}_cuerpo_contenedor¨ class=¨{enumCssExportacion.Contenedor.Render()}¨>
                                     <div id=¨{IdHtml}_cuerpo_exportacion¨ class=¨{enumCssExportacion.lista.Render()}¨>
                                        {listaDeExportaciones()}
                                     </div>
                                     <div id=¨{IdHtml}_cuerpo_sometido¨ class=¨{enumCssExportacion.sometido.Render()}¨>
                                        {checkDeSometido()}
                                     </div>
                                     <div id=¨{IdHtml}_cuerpo_enviar¨ class=¨{enumCssExportacion.enviar.Render()}¨>
                                        {editorDeEMail()}
                                     </div>
                                </div>";
            return htmlCuerpo;
        }

        private string listaDeExportaciones()
        {
            return RenderLista(IdHtml, "ExportacionDto", "Nombre", "Plantilla").Replace("Seleccionar ...", "Estandard");
        }
        private string checkDeSometido()
        {
            return RenderCheck(PlantillasHtml.checkDto, $"{IdHtml}_check", "", true, "Someter", "");
        }

        private string editorDeEMail()
        {
            var a = AtributosHtml.AtributosComunes($"div_{IdHtml}", IdHtml, "", enumTipoControl.Editor);
            a.Ayuda = "Indique los correos de e-mail receptores";
            
            return RenderEditor(PlantillasHtml.editorDto, a);
        }
    }
}
