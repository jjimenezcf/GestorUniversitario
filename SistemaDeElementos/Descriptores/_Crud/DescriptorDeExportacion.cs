using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enumerados;
using ModeloDeDto;
using MVCSistemaDeElementos.Descriptores;

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
            string _htmlMiModal = $@"<div id=¨{IdHtml}¨ class=¨contenedor-modal¨ crud-modal=¨{IdHtml}¨ negocio=¨{Crud.RenderNegocio}¨>
                              		<div id=¨{IdHtml}_contenido¨ class=¨contenido-modal¨ >
                              		    <div id=¨{IdHtml}_cabecera¨ class=¨contenido-cabecera¨>
                              		    	{Etiqueta}
                                        </div>
                              		    <div id=¨{IdHtml}_cuerpo¨ class=¨contenido-cuerpo¨>
                              			    cuerpoDeExportacion
                                        </div>
                                        <div id=¨{IdHtml}_pie¨ class=¨contenido-pie¨>
                                              <input type=¨text¨ id=¨{IdHtml}-exportar¨ class=¨boton-modal¨ value=¨Exportar¨ readonly onclick=¨Crud.{GestorDeEventos.EventosModalDeExportacion}('{TipoDeAccionDeExportar.Exportar}','{IdHtml}')¨/>
                                              <input type=¨text¨ id=¨{IdHtml}-cerrar¨  class=¨boton-modal¨ value=¨Cerrar¨ readonly onclick=¨Crud.{GestorDeEventos.EventosModalDeExportacion}('{TipoDeAccionDeExportar.Cerrar}','{IdHtml}')¨ />
                                           </div>
                                      </div>
                              </div>";

            return _htmlMiModal;
        }

    }
}
