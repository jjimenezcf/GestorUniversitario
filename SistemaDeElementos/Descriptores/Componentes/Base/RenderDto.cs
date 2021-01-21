using System;
using System.Collections.Generic;
using Gestor.Errores;
using ModeloDeDto;
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
            var visible = tabla.ObtenerFila(i).ObtenerColumna(j).NumeroControlesVisibles > 0;
            
            var td = $@"<td id=¨{tabla.IdHtml}_{i}_{j}¨ name=¨td-propiedad¨ class=¨td-propiedad¨ colspan=¨{tabla.ObtenerFila(i).ObtenerColumna(j).ColSpan}¨ style=¨width:{anchoColumna}%; {(visible ? "": "display:none")}¨>
                         <div id=¨{tabla.IdHtml}_{i}_{j}_celda¨ name=¨div-propiedad¨ class=¨div-propiedad¨>
                              {RenderControlesParaMapearElDto(tabla, i, j)}
                         </div>
                      </td>
                     ";
            return td;
        }

        private static string RenderControlesParaMapearElDto(DescriptorDeTabla tabla, short i, short j)
        {
            var porcentajeDeEtiqueta = (short)ElementoDto.ValorDelAtributo(typeof(TElemento), nameof(IUDtoAttribute.AnchoEtiqueta));
            var pocentajeDeControl = 100 - porcentajeDeEtiqueta;
            var porcentajeDelSeparador = (short)ElementoDto.ValorDelAtributo(typeof(TElemento), nameof(IUDtoAttribute.AnchoSeparador));
            var columna = tabla.ObtenerFila(i).ObtenerColumna(j);
            var htmlControles = "";
            double anchoEtiqueta = columna.NumeroDeEtiquetasVisibles == 0 ? 0 : porcentajeDeEtiqueta / columna.NumeroDeEtiquetasVisibles;
            double anchoControl = columna.NumeroControlesVisibles == 0 ? 0 : (pocentajeDeControl - (porcentajeDelSeparador * (columna.NumeroControlesVisibles - 1))) / columna.NumeroControlesVisibles;
            var anadirSeparador = false;

            if (columna.ColSpan > 1)
            {
                var ajuste = anchoEtiqueta / columna.ColSpan;
                anchoEtiqueta = anchoEtiqueta - ajuste;
                anchoControl = anchoControl + ajuste;
            }

            double anchoTotal = 0;
            for (short z = 0; z <= columna.PosicionMaxima; z++)
            {
                var descriptorControl = columna.ObtenerControlEnLaPosicion(z);
                if (descriptorControl == null || !descriptorControl.atributos.EsVisible(tabla.ModoDeTrabajo))
                    continue;

                if (anadirSeparador)
                {
                    htmlControles = htmlControles
                        + $"<div id=¨{tabla.IdHtml}-{i}-{j}-separador¨ class=¨div-separardor-propiedad¨ style=¨width:2%¨></div>";
                    anchoTotal += 2;
                }

                if (!descriptorControl.atributos.Etiqueta.IsNullOrEmpty())
                {
                    htmlControles = htmlControles + RenderEtiquetaDelDto(tabla, descriptorControl, i, j, anchoEtiqueta);
                    anchoTotal = anchoTotal + anchoEtiqueta;
                }
                else
                    anchoControl = 100;

                if (z == columna.PosicionMaxima)
                    anchoControl = 100 - anchoTotal;

                htmlControles = htmlControles + RenderDescriptorControlDto(tabla, descriptorControl, anchoControl);
                anchoTotal = anchoTotal + anchoControl;

                anadirSeparador = true;

            }

            return htmlControles;
        }


        private static string RenderEtiquetaDelDto(DescriptorDeTabla tabla, DescriptorDeControlDeLaTabla descriptorControl, short i, short j, double ancho)
        {
            if (descriptorControl.atributos.TipoDeControl == TipoControl.Check)
                return "";
            //17.01.2021 --> al poner el display en bloqk no hace falta width: {ancho}%
            return $@"<div id=¨{tabla.IdHtml}_{i}_{j}_lbl¨
                           name=¨lbl_propiedad¨
                           class=¨div-lbl-propiedad¨ 
                           style=¨{ (descriptorControl.atributos.TipoDeControl == TipoControl.Archivo ? "; margin-top: 11px":"")}¨>
                           <label for=¨{descriptorControl.IdHtml}¨>{descriptorControl.atributos.Etiqueta}:</label>
                       </div>
                      ";
        }

        //

        private static string RenderDescriptorControlDto(DescriptorDeTabla tabla, DescriptorDeControlDeLaTabla descriptorControl, double ancho)
        {
            var atributos = descriptorControl.atributos;
            var htmdDescriptorControl = "";
            switch(atributos.TipoDeControl) {
                case TipoControl.Editor:
                    htmdDescriptorControl = RenderEditor(tabla, descriptorControl, ancho);
                    break;
                case TipoControl.RestrictorDeEdicion:
                    htmdDescriptorControl = RenderEditor(tabla, descriptorControl, ancho);
                    break;
                case TipoControl.ListaDeElemento:
                    htmdDescriptorControl = RenderListaDeElemento(tabla, descriptorControl, ancho);
                    break;
                case TipoControl.ListaDinamica:
                    htmdDescriptorControl = RenderListaDinamica(tabla, descriptorControl, ancho);
                    break;
                case TipoControl.Archivo:
                    htmdDescriptorControl = RenderSelectorDeArchivo(tabla, descriptorControl, ancho);
                    break;
                case TipoControl.UrlDeArchivo:
                    htmdDescriptorControl = RenderSelectorDeArchivo(tabla, descriptorControl, ancho);
                    break;
                case TipoControl.Check:
                    htmdDescriptorControl = RenderCheck(tabla, descriptorControl);
                    break;
                default: 
                    GestorDeErrores.Emitir($"No se ha implementado como renderizar una propiedad del tipo {atributos.TipoDeControl}");
                    break;
            }

            return htmdDescriptorControl;
        }

        private static string RenderCheck(DescriptorDeTabla tabla, DescriptorDeControlDeLaTabla descriptorControl)
        {
            var atributos = descriptorControl.atributos;

            Dictionary<string, object> valores = ValoresDeAtributosComunes(tabla, descriptorControl, atributos);
            valores["CssContenedor"] = Css.Render(enumCssControlesDto.ContenedorCheckDto);
            valores["Css"] = Css.Render(enumCssControlesDto.CheckDto);
            valores["Checked"] = atributos.ValorPorDefecto;

            var htmlCheck = PlantillasHtml.Render(PlantillasHtml.checkDto, valores);

            return htmlCheck;
        }

        private static string RenderListaDeElemento(DescriptorDeTabla tabla, DescriptorDeControlDeLaTabla descriptorControl, double ancho)
        {
            var atributos = descriptorControl.atributos;

            Dictionary<string, object> valores = ValoresDeAtributosComunes(tabla, descriptorControl, atributos);
            valores["CssContenedor"] = Css.Render(enumCssControlesDto.ContenedorListaDeElementosDto);
            valores["Css"] = Css.Render(enumCssControlesDto.ListaDeElementosDto);
            valores["ClaseElemento"] = atributos.SeleccionarDe;
            valores["MostrarExpresion"] = atributos.MostrarExpresion.ToLower();
            valores["GuardarEn"] = atributos.GuardarEn;

            var htmlSelect = PlantillasHtml.Render(PlantillasHtml.listaDeElementosDto, valores);

            return htmlSelect;

        }

        private static string RenderListaDinamica(DescriptorDeTabla tabla, DescriptorDeControlDeLaTabla descriptorControl, double ancho)
        {
            var atributos = descriptorControl.atributos;
            var htmlContenedor = RenderContenedorDto(descriptorControl, ancho, Css.Render(enumCssControlesDto.ContenedorListaDinamicaDto));

            var htmlSelect = $@"<input {RenderAtributosComunes(tabla, descriptorControl,Css.Render(enumCssControlesDto.ListaDinamicaDto))}
                                       clase-elemento=¨{atributos.SeleccionarDe}¨ 
                                       guardar-en=¨{atributos.GuardarEn}¨ 
                                       oninput=¨Crud.{GestorDeEventos.EventosDeListaDinamica}('cargar',this)¨ 
                                       placeholder=¨Seleccionar ...¨ 
                                       list=¨{descriptorControl.IdHtml}-lista¨
                                />
                                <datalist id=¨{descriptorControl.IdHtml}-lista¨>
                                </datalist>";
            
            return htmlContenedor.Replace("controlParaRenderizar", htmlSelect);

        }

        private static string RenderEditor(DescriptorDeTabla tabla, DescriptorDeControlDeLaTabla descriptorControl, double ancho)
        {
            var atributos = descriptorControl.atributos;
            Dictionary<string, object> valores = ValoresDeAtributosComunes(tabla, descriptorControl, atributos);
            valores["CssContenedor"] = Css.Render(enumCssControlesDto.ContenedorEditorDto);
            valores["Css"] = atributos.TipoDeControl == TipoControl.RestrictorDeEdicion ? Css.Render(enumCssControlesDto.EditorRestrictorDto): Css.Render(enumCssControlesDto.EditorDto);
            valores["Placeholder"] = atributos.Ayuda;
            valores["ValorPorDefecto"] = atributos.ValorPorDefecto;

            var htmlEditor = PlantillasHtml.Render(PlantillasHtml.editorDto, valores);

            return htmlEditor;
        }

        private static string RenderSelectorDeArchivo(DescriptorDeTabla tabla, DescriptorDeControlDeLaTabla descriptorControl, double ancho)
        {
            var atributos = descriptorControl.atributos;
            var htmlContenedor = RenderContenedorDto(descriptorControl, ancho, Css.Render(enumCssControlesDto.ContenedorArchivoDto));

            var htmlArchivo = @$"
            <form class=¨{Css.Render(enumCssControlesDto.FormDeArchivoDto)}¨ method=¨post¨ enctype=¨multipart/form-data¨>
              <table class=¨{Css.Render(enumCssControlesDto.TablaDeArchivo)}¨>
                 <tr class=¨{Css.Render(enumCssControlesDto.FilaDeArchivo)}¨>
                   <td class=¨{Css.Render(enumCssControlesDto.ColumnaDeArchivo)}¨>        
                      <a id=¨{descriptorControl.IdHtml}.ref¨ href=¨javascript:ApiDeArchivos.SeleccionarArchivo('{descriptorControl.IdHtml}')¨>{atributos.Ayuda}</a>
                      <input  {RenderAtributosComunes(tabla, descriptorControl, Css.Render(enumCssControlesDto.SelectorDeArchivo))}
                              type=¨file¨ 
                              name=¨fichero¨  
                              style=¨display: none;¨
                              accept=¨{atributos.ExtensionesValidas}¨
                              ruta-destino=¨{atributos.RutaDestino}¨
                              canvas-vinculado = ¨canvas-{descriptorControl.IdHtml}¨  
                              imagen-vinculada = ¨img-{descriptorControl.IdHtml}¨   
                              barra-vinculada = ¨barra-{descriptorControl.IdHtml}¨  
                              placeholder =¨{atributos.Ayuda}¨
                              onChange=¨ApiDeArchivos.MostrarCanvas('{tabla.Controlador}','{descriptorControl.IdHtml}','canvas-{descriptorControl.IdHtml}','barra-{descriptorControl.IdHtml}')¨ />
                  </td>
                   <td class=¨{Css.Render(enumCssControlesDto.ColumnaDeArchivo)}¨>
                      <div id = ¨barra-{descriptorControl.IdHtml}¨ class=¨{Css.Render(enumCssControlesDto.BarraAzulArchivo)}¨>
                          <span></span>
                      </div>
                   </td>
                 </tr>
                 <tr class=¨{Css.Render(enumCssControlesDto.FilaDeArchivo)}¨>
                   <td class=¨{Css.Render(enumCssControlesDto.ColumnaDeArchivo)}¨>
                      <canvas id=¨canvas-{descriptorControl.IdHtml}¨></canvas>
                   </td>
                   <td class=¨{Css.Render(enumCssControlesDto.ColumnaDeArchivo)}¨>
                       <div style=¨display: none;¨>
                           <img id=¨img-{descriptorControl.IdHtml}¨
                                    tipo=¨{TipoControl.VisorDeArchivo}¨  
                                    propiedad=¨{(atributos.TipoDeControl == TipoControl.UrlDeArchivo ? descriptorControl.propiedad: atributos.UrlDelArchivo.ToLower())}¨ 
                                    src=¨¨>
                       </div>
                   </td>
                 </tr>
              </table>

             </form>
            ";
            return htmlContenedor.Replace("controlParaRenderizar", htmlArchivo);
        }

        private static string RenderContenedorDto(DescriptorDeControlDeLaTabla descriptorControl, double ancho, string cssClaseContenedor)
        {
            //17.01.2021 --> Al usar css no me hace flata, y mostrar en bloques style=¨width: {ancho}%
            return $@"<div id=¨{descriptorControl.IdHtmlContenedor}¨ name=¨contenedor-control¨ class=¨{cssClaseContenedor}¨ ¨ >
                        controlParaRenderizar
                      </div>";
        }

        private static string RenderAtributosComunes(DescriptorDeTabla tabla, DescriptorDeControlDeLaTabla descriptorControl, string claseCss)
        {
            var atributos = descriptorControl.atributos;
            var atributosHtml = $@"id=¨{descriptorControl.IdHtml}¨ 
                                   propiedad=¨{descriptorControl.propiedad}¨ 
                                   class=¨{claseCss}¨ 
                                   tipo=¨{atributos.TipoDeControl}¨ 
                                   obligatorio=¨{(atributos.EsVisible(tabla.ModoDeTrabajo) && atributos.Obligatorio ? "S" : "N")}¨ 
                                   {(!atributos.EsEditable(tabla.ModoDeTrabajo) ? "readonly" : "")} ";
            return atributosHtml;
        }

        private static Dictionary<string, object> ValoresDeAtributosComunes(DescriptorDeTabla tabla, DescriptorDeControlDeLaTabla descriptorControl, IUPropiedadAttribute atributos)
        {
            Dictionary<string, object> valores = PlantillasHtml.ValoresDeAtributesComunes(descriptorControl.IdHtmlContenedor, descriptorControl.IdHtml, descriptorControl.propiedad, atributos.TipoDeControl);
            valores["Obligatorio"] = atributos.EsVisible(tabla.ModoDeTrabajo) && atributos.Obligatorio ? "S" : "N";
            valores["Readonly"] = !atributos.EsEditable(tabla.ModoDeTrabajo) ? "readonly" : "";
            return valores;
        }

    }
}
