using System;
using System.Collections.Generic;
using System.Reflection;
using UtilidadesParaIu;
using Gestor.Elementos.ModeloIu;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeCreacion<TElemento> : ControlHtml where TElemento : Elemento
    {
        public DescriptorDeCrud<TElemento> Crud => (DescriptorDeCrud<TElemento>)Padre;
        public BarraDeMenu<TElemento> MenuCreacion { get; private set; }
        public string htmlDeCreacionEspecifico { get; set; }

        private bool EnModal { set; get; }

        public DescriptorDeCreacion(DescriptorDeCrud<TElemento> crud, string etiqueta)
        : base(
          padre: crud,
          id: $"{crud.Id}_{TipoControl.pnlCreador}",
          etiqueta: etiqueta,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.pnlCreador;
            MenuCreacion = new BarraDeMenu<TElemento>(creador: this);
            MenuCreacion.AnadirOpcionDeNuevoElemento();
            MenuCreacion.AnadirOpcionDeCancelarNuevo();
            EnModal = (bool)Elemento.ValorDelAtributo(typeof(TElemento), nameof(IUDtoAttribute.CreacionEnModal));
        }



        public override string RenderControl()
        {
            string htmContenedorCreacion;
            if (this.EnModal)
            {
                htmContenedorCreacion = RendelModal();
            }
            else
            {
                htmContenedorCreacion = 
                $@"
                   <div id=¨{IdHtml}¨ class=¨div-no-visible¨ controlador=¨{Crud.Controlador}¨>
                         <h2>Creación</h2> 
                         {MenuCreacion.RenderControl()}
                         {RendelDivDeCreacion()}
                   </div>
                ";
            }

            return htmContenedorCreacion.Render();
        }


        private string RendelModal()
        {
            var htmlModal = $@"<div id=¨{IdHtml}¨ class=¨contenedor-modal¨ controlador=¨{Crud.Controlador}¨>
                              		<div id=¨{IdHtml}_contenido¨ class=¨cotenido-modal¨>
                              		    <div id=¨{IdHtml}_cabecera¨ class=¨cotenido-cabecera¨>
                              		    	<h2>Creación</h2>
                                        </div>
                              		    <div id=¨{IdHtml}_cuerpo¨ class=¨cotenido-cuerpo¨>
                                        {RendelDivDeCreacion()}
                                        </div>
                                        <div id=¨{IdHtml}_pie¨ class=¨cotenido-pie¨>
                                           <input type=¨text¨ id=¨{IdHtml}_Aceptar¨ class=¨boton-modal¨ value=¨Aceptar¨ onclick=¨Crud.EventosModalDeCreacion('crear-elemento')¨       />
                                           <input type=¨text¨ id=¨{IdHtml}_Cerrar¨  class=¨boton-modal¨ value=¨Cerrar¨  onclick=¨Crud.EventosModalDeCreacion('cerrar-modal')¨ />
                                           {htmlRenderOpciones()}
                                        </div>
                                      </div>
                              </div>";
            return htmlModal;
        }


        private string RendelDivDeCreacion()
        {
            var htmlModal = $@"
                               {htmlRenderObjetoVacio()}
                               {htmlDeCreacionEspecifico}
                               {(EnModal ? "":htmlRenderOpciones())}
            ";
            return htmlModal;
        }

        private object htmlRenderOpciones()
        {

            var htmdDescriptorControl = $@"<input id=¨crear-mas-{IdHtml}¨ type=¨checkbox¨ checked/>
                                           <label for=¨crear-mas-{IdHtml}¨>Cerrar tras crear</label>";


            var htmContenedorPie =
                   $@"
                   <Div id=¨opciones-{IdHtml}¨ class=¨contenedor-opciones-creacion¨>
                    {htmdDescriptorControl}
                  </Div>
                ";
            return htmContenedorPie;
        }

        protected virtual string htmlRenderObjetoVacio()
        {
            var tabla = new DescriptorDeTabla(typeof(TElemento), ModoDeTrabajo.Nuevo, Crud.Controlador);

            var htmlObjeto = @$"<table id=¨{tabla.IdHtml}¨ name=¨table_propiedad¨  class=¨tabla-creacion¨>
                                  htmlFilas
                                </table>
                               ";

            var htmlFilas = "";

            for (short i = 0; i < tabla.NumeroDeFilas; i++)
            {
                htmlFilas = htmlFilas + Environment.NewLine + RenderDto<TElemento>.RenderFilaParaElDto(tabla, i);
            }

            return htmlObjeto.Replace("htmlFilas", $"{htmlFilas}");
        }





    }
}