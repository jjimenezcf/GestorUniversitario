using System;
using Gestor.Elementos.ModeloIu;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeEdicion<TElemento> : ControlHtml where TElemento : Elemento
    {
        public DescriptorDeCrud<TElemento> Crud => (DescriptorDeCrud<TElemento>)Padre;
        public BarraDeMenu<TElemento> MenuDeEdicion { get; private set; }
        private bool EnModal { set; get; }


        public DescriptorDeEdicion(DescriptorDeCrud<TElemento> crud, string etiqueta)
        : base(
          padre: crud,
          id: $"{crud.Id}_{TipoControl.pnlEditor}",
          etiqueta: etiqueta,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.pnlEditor;
            MenuDeEdicion = new BarraDeMenu<TElemento>(editor: this);
            MenuDeEdicion.AnadirOpcionDeModificarElemento();
            MenuDeEdicion.AnadirOpcionDeCancelarEdicion();
            EnModal = (bool)Elemento.ValorDelAtributo(typeof(TElemento), nameof(IUDtoAttribute.EdicionEnModal));
        }


        public override string RenderControl()
        {
            var tabla = new DescriptorDeTabla(typeof(TElemento), ModoDeTrabajo.Edicion, Crud.Controlador);
            string htmContenedorEdt;
            if (this.EnModal)
            {
                htmContenedorEdt = RendelModal(tabla);
            }
            else
            {
                htmContenedorEdt =
                $@"
                   <div id=¨{IdHtml}¨ class=¨div-no-visible¨ controlador=¨{Crud.Controlador}¨>
                         <h2>Edición</h2> 
                         {MenuDeEdicion.RenderControl()}
                         {RendelDivDeEdicion(tabla)}
                   </div>
                ";
            }

            return htmContenedorEdt.Render();
        }

        private string RendelModal(DescriptorDeTabla tabla)
        {
            var htmlModal = $@"<div id=¨{IdHtml}¨ class=¨contenedor-modal¨ controlador=¨{Crud.Controlador}¨>
                              		<div id=¨{IdHtml}_contenido¨ class=¨cotenido-modal¨>
                              		    <div id=¨{IdHtml}_cabecera¨ class=¨cotenido-cabecera¨>
                              		    	<h2>Edición</h2>
                                        </div>
                              		    <div id=¨{IdHtml}_cuerpo¨ class=¨cotenido-cuerpo¨>
                                        {RendelDivDeEdicion(tabla)}
                                        </div>
                                        <div id=¨{IdHtml}_pie¨ class=¨cotenido-pie¨>
                                           <input type=¨text¨ id=¨{IdHtml}_Aceptar¨ class=¨boton-modal¨ value=¨Aceptar¨ onclick=¨Crud.EventosModalDeEdicion('modificar-elemento')¨       />
                                           <input type=¨text¨ id=¨{IdHtml}_Cerrar¨  class=¨boton-modal¨ value=¨Cerrar¨  onclick=¨Crud.EventosModalDeEdicion('cerrar-modal')¨ />
                                        </div>
                                      </div>
                              </div>";
            return htmlModal;
        }

        private string RendelDivDeEdicion(DescriptorDeTabla tabla)
        {
            var htmlModal = $@"{htmlRenderObjetoVacio(tabla)}
                               {htmlRenderPie(tabla)}";
            return htmlModal;
        }

        private object htmlRenderPie(DescriptorDeTabla tabla)
        {
            var htmContenedorPie =
                   $@"
                   <Div id=¨{IdHtml}¨ class=¨contenedor-id¨>
                     {RenderInputId(tabla)}
                  </Div>
                ";
            return htmContenedorPie;
        }

        private string RenderInputId(DescriptorDeTabla tabla)
        {
            var htmdDescriptorControl = $@"<input id=¨{tabla.IdHtml}_idElemento¨ 
                                             propiedad=¨{nameof(Elemento.Id).ToLower()}¨ 
                                             class=¨propiedad propiedad-id¨ 
                                             tipo=¨{TipoControl.Editor}¨ 
                                             type=¨text¨ 
                                             readonly
                                             value=¨¨>
                                           </input >";
            return htmdDescriptorControl;
        }

        protected virtual string htmlRenderObjetoVacio(DescriptorDeTabla tabla)
        {
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