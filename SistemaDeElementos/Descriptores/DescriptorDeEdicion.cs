using System;
using Gestor.Elementos.ModeloIu;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeEdicion<TElemento> : ControlHtml where TElemento : Elemento
    {
        public DescriptorDeCrud<TElemento> Crud => (DescriptorDeCrud<TElemento>)Padre;
        public BarraDeMenu<TElemento> MenuDeEdicion { get; private set; }


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
        }


        public override string RenderControl()
        {
            var tabla = new DescriptorDeTabla(typeof(TElemento), ModoDeTrabajo.Edicion);
            var htmContenedorEdt =
                $@"
                   <Div id=¨{IdHtml}¨ class=¨div-no-visible¨ controlador=¨{Crud.Controlador}¨>
                     <h2>Div de Edicion</h2>
                     {MenuDeEdicion.RenderControl()}
                     {htmlRenderObjetoVacio(tabla)}
                     {htmlRenderPie(tabla)}
                  </Div>
                ";

            return htmContenedorEdt.Render();
        }

        private object htmlRenderPie(DescriptorDeTabla tabla)
        {
            var htmContenedorPie =
                   $@"
                   <Div id=¨{IdHtml}¨ class=¨div-pie-propiedad¨>
                     {RenderInputId(tabla)}
                  </Div>
                ";
            return htmContenedorPie;
        }

        private string RenderInputId(DescriptorDeTabla tabla)
        {
            var htmdDescriptorControl = $"   <input id=¨{tabla.IdHtml}_idElemento¨ " + Environment.NewLine +
                                        $"       propiedad-dto=¨id¨ " + Environment.NewLine +
                                        $"       class=¨propiedad propiedad-id¨ " + Environment.NewLine +
                                        $"       type=¨text¨ " + Environment.NewLine +
                                        $"       readonly" +
                                        $"       value=¨¨" + Environment.NewLine +
                                        $"   </input>" + Environment.NewLine;
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