using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor.Elementos.ModeloIu;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{

    public static class RenderDto<TElemento>
    {
        public static string RenderFilaParaElDto(DescriptorDeTabla tabla, short i)
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
                htmlColumnas = htmlColumnas + RenderColumnaParaElDto(tabla, i, j, anchoColumna);
            }
            return htmlFila.Replace("htmlColumnas", $"{htmlColumnas}");
        }

        private static string RenderColumnaParaElDto(DescriptorDeTabla tabla, short i, short j, double anchoColumna)
        {

            return $@"<td id=¨{tabla.IdHtml}_{i}_{j}_ctrl¨ name=¨td_propiedad¨ class=¨td-propiedad¨  style=¨width:{anchoColumna}%¨>
                         <div id=¨{tabla.IdHtml}_{i}_{j}¨ name=¨div_propiedad¨ class=¨div-propiedad¨>
                              {RenderControlesParaMapearElDto(tabla, i, j)}
                         </div>
                      </td>
                     ";
        }

        private static string RenderControlesParaMapearElDto(DescriptorDeTabla tabla, short i, short j)
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
                if (descriptorControl == null || !descriptorControl.atributos.Visible(tabla.ModoDeTrabajo))
                    continue;

                if (anadirSeparador)
                {
                    htmlControles = htmlControles
                        + "<div id=¨{tabla.IdHtml}_{i}_{j}_separador¨ name=¨div_separardor_propiedad¨ class=¨div-separardor-propiedad¨ style=¨width:2%¨></div>";
                    anchoTotal += 2;
                }

                if (!descriptorControl.atributos.Etiqueta.IsNullOrEmpty())
                {
                    htmlControles = htmlControles + RenderEtiquetaDelDto(tabla, descriptorControl.atributos.Etiqueta, i, j, anchoEtiqueta);
                    anchoTotal = anchoTotal + anchoEtiqueta;
                }

                if (z == columna.PosicionMaxima)
                    anchoControl = 100 - anchoTotal;

                htmlControles = htmlControles + RenderDescriptorControlDto(tabla, descriptorControl, i, j, anchoControl);
                anchoTotal = anchoTotal + anchoControl;

                anadirSeparador = true;

            }

            return htmlControles;
        }


        private static string RenderEtiquetaDelDto(DescriptorDeTabla tabla, string etiqueta, short i, short j, double ancho)
        {
            return $@"<div id=¨{tabla.IdHtml}_{i}_{j}_lbl¨ name=¨lbl_propiedad¨ class=¨div-lbl-propiedad¨ style=¨width: {ancho}%¨>
                         {etiqueta}: 
                       </div>
                      ";
        }

        private static string RenderDescriptorControlDto(DescriptorDeTabla tabla, DescriptorControl descriptorControl, short i, short j, double ancho)
        {
            var atributos = descriptorControl.atributos;
            var htmdDescriptorControl = $"<div id=¨{tabla.IdHtml}_{i}_{j}_crtl¨ name=¨crtl_propiedad¨ class=¨div-crtl-propiedad¨ style=¨width: {ancho}%¨ >" + Environment.NewLine +
                                        $"   <input id=¨{tabla.IdHtml}_{descriptorControl.propiedadDto}¨ " + Environment.NewLine +
                                        $"       propiedad-dto=¨{descriptorControl.propiedadDto}¨ " + Environment.NewLine +
                                        $"       class=¨propiedad propiedad-valida¨ " + Environment.NewLine + // 
                                        //$"       cssCrtlNoValido=¨{atributos.cssNoValido}¨ " + Environment.NewLine +
                                        //$"       cssCrtlValido=¨{atributos.cssValido}¨ " + Environment.NewLine +
                                        $"       obligatorio=¨{(atributos.Visible(tabla.ModoDeTrabajo) && atributos.Obligatorio ? "S" : "N")}¨ " + Environment.NewLine +
                                        $"       type=¨text¨ " + Environment.NewLine +
                                        $"       {(!atributos.Editable(tabla.ModoDeTrabajo) ? "readonly" : "")} " + Environment.NewLine +
                                        $"       value=¨¨" + Environment.NewLine +
                                        $"       placeholder =¨{atributos.Ayuda}¨" + Environment.NewLine +
                                        $"       ValorPorDefecto=¨{atributos.ValorPorDefecto}¨>" + Environment.NewLine +
                                        $"   </input>" + Environment.NewLine +
                                        $"</div>";
            return htmdDescriptorControl;
        }
    }
}
