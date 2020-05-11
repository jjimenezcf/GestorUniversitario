using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor.Elementos.ModeloIu;
using Gestor.Errores;
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
                    $@"<tr id=¨{fila.IdHtml}¨ name=¨tr_lbl_propiedad¨ class=¨tr-propiedad¨>
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

            return $@"<td id=¨{tabla.IdHtml}_{i}_{j}¨ name=¨td_propiedad¨ class=¨td-propiedad¨  style=¨width:{anchoColumna}%¨>
                         <div id=¨{tabla.IdHtml}_{i}_{j}_celda¨ name=¨div_propiedad¨ class=¨div-propiedad¨>
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
                if (descriptorControl == null || !descriptorControl.atributos.EsVisible(tabla.ModoDeTrabajo))
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

                htmlControles = htmlControles + RenderDescriptorControlDto(tabla, descriptorControl, anchoControl);
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

        private static string RenderDescriptorControlDto(DescriptorDeTabla tabla, DescriptorControl descriptorControl, double ancho)
        {
            var atributos = descriptorControl.atributos;
            var htmdDescriptorControl = "";
            switch(atributos.TipoDeControl) {
                case TipoControl.Editor:
                    htmdDescriptorControl = RenderEditorDto(tabla, descriptorControl, ancho);
                    break;
                case TipoControl.SelectorDeElemento:
                    htmdDescriptorControl = RenderSelectorElemento(tabla, descriptorControl, ancho);
                    break;
                default: 
                    GestorDeErrores.Emitir($"No se ha implementado como renderizar una propiedad del tipo {atributos.TipoDeControl}");
                    break;
            }

            return htmdDescriptorControl;
        }

        private static string RenderSelectorElemento(DescriptorDeTabla tabla, DescriptorControl descriptorControl, double ancho)
        {
            var atributos = descriptorControl.atributos;
            var htmlContenedor = RenderContenedorDto(descriptorControl, ancho, "contenedor-selector");

            var htmlSelect = $@"<select {RenderAtributosComunes(tabla, descriptorControl)}
                                        clase-elemento=¨{atributos.SeleccionarDe}¨ 
                                        guardar-en¨{atributos.GuardarEn}¨>
                                        <option value=¨0¨>Seleccionar ...</option>
                                </select>";

            return htmlContenedor.Replace("control", htmlSelect);

        }

        private static string RenderEditorDto(DescriptorDeTabla tabla, DescriptorControl descriptorControl, double ancho)
        {
            var atributos = descriptorControl.atributos;
            var htmlContenedor = RenderContenedorDto(descriptorControl, ancho, "contenedor-editor");
            var htmlInput = $@"<input {RenderAtributosComunes(tabla, descriptorControl)}
                                      type=¨text¨ 
                                      value=¨¨
                                      placeholder =¨{atributos.Ayuda}¨
                                      ValorPorDefecto=¨{atributos.ValorPorDefecto}¨>
                                </input>";
            return htmlContenedor.Replace("control", htmlInput);
        }

        private static string RenderContenedorDto(DescriptorControl descriptorControl, double ancho, string cssClaseContenedor)
        {
            return $@"<div id=¨{descriptorControl.IdHtmlContenedor}¨ name=¨crtl_propiedad¨ class=¨{cssClaseContenedor}¨ style=¨width: {ancho}%¨ >
                        control
                      </div>";
        }

        private static string RenderAtributosComunes(DescriptorDeTabla tabla, DescriptorControl descriptorControl)
        {
            var atributos = descriptorControl.atributos;
            var atributosHtml = $@"id=¨{descriptorControl.IdHtml}¨ 
                                   propiedad=¨{descriptorControl.propiedad}¨ 
                                   class=¨propiedad propiedad-valida¨ 
                                   tipo=¨{atributos.TipoDeControl}¨ 
                                   obligatorio=¨{(atributos.EsVisible(tabla.ModoDeTrabajo) && atributos.Obligatorio ? "S" : "N")}¨ 
                                   {(!atributos.EsEditable(tabla.ModoDeTrabajo) ? "readonly" : "")} ";
            return atributosHtml;
        }
    }
}
