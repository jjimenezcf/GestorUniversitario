using System;
using System.Collections.Generic;
using System.Reflection;
using UtilidadesParaIu;
using Gestor.Elementos.ModeloIu;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeCreacion<TElemento> : ControlHtml
    {
        public DescriptorDeCrud<TElemento> Crud => (DescriptorDeCrud<TElemento>)Padre;
        public ZonaDeMenuMnt<TElemento> ZonaMenu { get; private set; }

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
            ZonaMenu = new ZonaDeMenuMnt<TElemento>(creador: this);
            ZonaMenu.AnadirOpcionDeNuevoElemento();
            ZonaMenu.AnadirOpcionDeCancelarNuevo();
        }



        public override string RenderControl()
        {
            var htmContenedorMnt =
                $@"
                   <Div id=¨{IdHtml}¨ class=¨div-no-visible¨ controlador=¨{Crud.Controlador}¨>
                     <h2>Div de creación</h2>
                     {ZonaMenu.RenderControl()}
                     {htmlRenderObjetoVacio()}
                   </Div>
                ";

            return htmContenedorMnt.Render();
        }


        protected virtual string htmlRenderObjetoVacio()
        {
            var tabla = new DescriptorDeTabla(typeof(TElemento));

            var htmlObjeto = @$"<table id=¨{tabla.IdHtml}¨ name=¨table_propiedad¨  class=¨tabla-creacion¨>
                                  htmlFilas
                                </table>
                               ";

            var htmlFilas = "";
            for (short i = 0; i < tabla.NumeroDeFilas; i++)
            {
                htmlFilas = htmlFilas + Environment.NewLine + RenderFila(tabla, i);
            }

            return htmlObjeto.Replace("htmlFilas", $"{htmlFilas}");
        }

        private static string RenderFila(DescriptorDeTabla tabla, short i)
        {
            var fila = tabla.ObtenerFila(i);
            var htmlColumnas = "";
            var htmlFila =
                    $@"<tr id=¨{fila.IdHtml}_{i}¨ name=¨tr_lbl_propiedad¨ class=¨tr-propiedad¨>
                         htmlColumnas
                       </tr>
                      ";

            double anchoColumna = tabla.NumeroDeColumnas == 0 ? 0 : (double)(100 / tabla.NumeroDeColumnas);
            for (short j = 0; j < tabla.NumeroDeColumnas; j++)
            {
                htmlColumnas = htmlColumnas + RenderColumna(tabla, i, j, anchoColumna);
            }
            return htmlFila.Replace("htmlColumnas", $"{htmlColumnas}");
        }

        private static string RenderColumna(DescriptorDeTabla tabla, short i, short j, double anchoColumna)
        {

            return $@"<td id=¨{tabla.IdHtml}_{i}_{j}_ctrl¨ name=¨td_propiedad¨ class=¨td-propiedad¨  style=¨width:{anchoColumna}%¨>
                         <div id=¨{tabla.IdHtml}_{i}_{j}¨ name=¨div_propiedad¨ class=¨div-propiedad¨>
                              {RenderControles(tabla, i, j)}
                         </div>
                      </td>
                     ";
        }

        private static string RenderControles(DescriptorDeTabla tabla, short i, short j)
        {
            var porcentajeDeEtiqueta = (short)Elemento.ValorDelAtributo(typeof(TElemento), nameof(IUDtoAttribute.AnchoEtiqueta)); 
            var pocentajeDeControl = 100 - porcentajeDeEtiqueta;
            var porcentajeDelSeparador = (short)Elemento.ValorDelAtributo(typeof(TElemento), nameof(IUDtoAttribute.AnchoSeparador)); 
            var columna = tabla.ObtenerFila(i).ObtenerColumna(j);
            var htmlControles = "";
            double anchoEtiqueta = columna.NumeroDeEtiquetasVisibles == 0 ? 0 : porcentajeDeEtiqueta / columna.NumeroDeEtiquetasVisibles;
            double anchoControl = columna.NumeroControlesVisibles == 0 ? 0 : (pocentajeDeControl - (porcentajeDelSeparador * (columna.NumeroControlesVisibles - 1))) / columna.NumeroControlesVisibles;
            var anadirSeparador = false;

            double anchoTotal = 0;
            for (short z = 0; z <= columna.PosicionMaxima; z++)
            {
                var descriptorControl = columna.ObtenerControlEnLaPosicion(z);
                if (descriptorControl == null || !descriptorControl.atributos.Visible)
                    continue;

                if (anadirSeparador)
                {
                    htmlControles = htmlControles
                        + "<div id=¨{tabla.IdHtml}_{i}_{j}_separador¨ name=¨div_separardor_propiedad¨ class=¨div-separardor-propiedad¨ style=¨width:2%¨></div>";
                    anchoTotal += 2;
                }

                if (!descriptorControl.atributos.Etiqueta.IsNullOrEmpty())
                {
                    htmlControles = htmlControles + RenderEtiqueta(tabla, descriptorControl.atributos.Etiqueta, i, j, anchoEtiqueta);
                    anchoTotal = anchoTotal + anchoEtiqueta;
                }

                if (z == columna.PosicionMaxima)
                    anchoControl = 100 - anchoTotal;

                htmlControles = htmlControles + RenderDescriptorControl(tabla, descriptorControl, i, j, anchoControl);
                anchoTotal = anchoTotal + anchoControl;

                anadirSeparador = true;

            }

            return htmlControles;
        }

        private static string RenderEtiqueta(DescriptorDeTabla tabla, string etiqueta, short i, short j, double ancho)
        {
            return $@"<div id=¨{tabla.IdHtml}_{i}_{j}_lbl¨ name=¨lbl_propiedad¨ class=¨div-lbl-propiedad¨ style=¨width: {ancho}%¨>
                         {etiqueta}: 
                       </div>
                      ";
        }

        private static string RenderDescriptorControl(DescriptorDeTabla tabla, DescriptorControl descriptorControl, short i, short j, double ancho)
        {
            var htmdDescriptorControl = $"<div id=¨{tabla.IdHtml}_{i}_{j}_crtl¨ name=¨crtl_propiedad¨ class=¨div-crtl-propiedad¨ style=¨width: {ancho}%¨ >" + Environment.NewLine +
                                        $"   <input id=¨{descriptorControl.Descriptor.Name}¨ "+ Environment.NewLine +
                                        $"       propiedad-dto=¨{descriptorControl.Descriptor.Name}¨ " + Environment.NewLine +
                                        $"       class=¨propiedad {descriptorControl.atributos.ClaseCss}¨ " + Environment.NewLine +
                                        $"       type=¨text¨ " + Environment.NewLine +
                                        $"       {(!descriptorControl.atributos.Editable ? "readonly" : "")} " + Environment.NewLine +
                                        $"       value=¨¨" + Environment.NewLine +
                                        $"       placeholder =¨{descriptorControl.atributos.Ayuda}¨" + Environment.NewLine +
                                        $"       ValorPorDefecto=¨{descriptorControl.atributos.ValorPorDefecto}¨>" + Environment.NewLine +
                                        $"   </input>" + Environment.NewLine +
                                        $"</div>";
            return htmdDescriptorControl;
        }
    }
}