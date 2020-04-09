using System;
using System.Collections.Generic;
using System.Reflection;
using UtilidadesParaIu;
using Gestor.Elementos.ModeloIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeCreacion<TElemento> : ControlHtml
    {
        public DescriptorDeCrud<TElemento> Crud => (DescriptorDeCrud<TElemento>)Padre;

        public DescriptorDeCreacion(DescriptorDeCrud<TElemento> crud, string etiqueta)
        : base(
          padre: crud,
          id: $"{crud.Id}_{TipoControl.Creacion}",
          etiqueta: etiqueta,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.Creacion;
        }

        public override string RenderControl()
        {
            var htmlOpcionMenu = $"<input id=¨idAceptar¨ type=¨button¨ value=¨Aceptar¨ onClick=¨Menu.EjecutarAccionMenu('{Crud.Mnt.IdHtml}','{IdHtml}')¨ />";


            var htmContenedorMnt =
                $@"
                   <Div id=¨{IdHtml}¨ class=¨div-no-visible¨>
                     <h2>Div de creación</h2>
                     {htmlOpcionMenu}
                     {htmlRenderObjetoVacio()}
                   </Div>
                ";

            return htmContenedorMnt.Render();
        }


        protected virtual string htmlRenderObjetoVacio()
        {
            var tabla = new DescriptorDeTabla(typeof(TElemento));

            var htmlObjeto = @$"<table id=¨{tabla.IdHtml}¨ name=¨table_propiedad¨  class=¨tabla-propiedad¨>
                                  htmlFilas
                                </table>
                               ";

            var htmlFilas = "";
            for (short i = 0; i < tabla.Count; i++)
            {
                htmlFilas = htmlFilas + Environment.NewLine + RenderFila(tabla, i);
            }

            return htmlObjeto.Replace("htmlFilas", $"{htmlFilas}");
        }

        private string RenderFila(DescriptorDeTabla tabla, short i)
        {
            var fila = tabla.Filas[i];
            var htmlColumnas = "";
            var htmlFila =
                    $@"<tr id=¨{fila.IdHtml}_{i}¨ name=¨tr_lbl_propiedad¨ class=¨tr-propiedad¨>
                         htmlColumnas
                       </tr>
                      ";
            
            for (short j = 0; j < fila.Count; j++)
            {
                htmlColumnas = htmlColumnas + RenderColumna(tabla, i, j);
            }
            return htmlFila.Replace("htmlColumnas", $"{htmlColumnas}");
        }

        private string RenderColumna(DescriptorDeTabla tabla, short i, short j)
        {
            var columna = tabla.Filas[i].ObtenerColumna(j);
            var htmlControles = "";
            var htmlEtiqueta =
                    $@"<td id=¨{tabla.Filas[i].IdHtml}_{i}_{j}_lbl¨ name=¨td_lbl_propiedad¨ class=¨td-propiedad¨>
                         {columna.Etiqueta}
                       </td>
                      ";
            var htmlTdControles =
                    $@"<td id=¨{tabla.Filas[i].IdHtml}_{i}_{j}_ctrl¨ name=¨td_ctrl_propiedad¨ class=¨td-propiedad¨>
                         htmlControles
                       </td>
                      ";

            for (short z = 0; z < columna.Count; z++)
            {
                var descriptorControl = columna.ObtenerControl(z);
                htmlControles = htmlControles + RenderDescriptorControl(descriptorControl);
            }

            return htmlEtiqueta + htmlTdControles.Replace("htmlControles", htmlControles);
        }

        private string RenderDescriptorControl(DescriptorControl descriptorControl)
        {
            var htmdDescriptorControl = $"<input id=¨idAceptar¨ type=¨text¨ value=¨¨ />";
            return htmdDescriptorControl;
        }
    }
}