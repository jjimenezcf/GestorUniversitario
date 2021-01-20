using System.Linq;
using Gestor.Errores;
using GestorDeElementos;
using ModeloDeDto;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{
    public class ListaDeElemento<TElemento> : ControlFiltroHtml where TElemento : ElementoDto
    {

        public string GuardarEn { get; private set; }
        public string SeleccionarDe { get; private set; }
        public string MostrarExpresion { get; private set; }

        //string etiqueta, string filtrarPor, string ayuda, string seleccionarDe, string buscarPor, string mostrarExpresion, CriteriosDeFiltrado criterioDeBusqueda, Posicion posicion)

        public ListaDeElemento(BloqueDeFitro<TElemento> padre, string etiqueta, string ayuda, string seleccionarDe, string filtraPor, string mostrarExpresion, Posicion posicion)
        : base(
            padre: padre
          , id: $"{padre.Id}_{TipoControl.ListaDeElemento}_{filtraPor}"
          , etiqueta: etiqueta
          , filtraPor
          , ayuda: ayuda
          , posicion
          )
        {
            Tipo = TipoControl.ListaDeElemento;
            SeleccionarDe = seleccionarDe;
            MostrarExpresion = mostrarExpresion;
            padre.AnadirSelectorElemento(this);
        }


        public override string RenderControl()
        {
            //var htmlSelect = $@"<div id=¨div-{IdHtml}¨  class=¨{Css.Render(enumCssFiltro.ContenedorListaDinamica)}¨>
            //                        <input id=¨{IdHtml}¨
            //                               propiedad=¨{Propiedad.ToLower()}¨ 
            //                               class=¨{Css.Render(enumCssFiltro.ListaDinamica)}¨ 
            //                               tipo=¨{Tipo}¨
            //                               carga-dinamica='S'
            //                               clase-elemento=¨{SeleccionarDe}¨
            //                               mostrar-propiedad=¨{MostrarPropiedad.ToLower()}¨  
            //                               como-buscar='{BuscarPor}'
            //                               criterio-de-filtro=¨{Criterio}¨ 
            //                               filtrar-por=¨{Propiedad.ToLower()}¨ 
            //                               longitud='{LongitudMinimaParaBuscar}'
            //                               oninput=¨Crud.{GestorDeEventos.EventosDeListaDinamica}('cargar',this)¨ 
            //                               onchange=¨Crud.{GestorDeEventos.EventosDeListaDinamica}('seleccionar',this)¨ 
            //                               placeholder=¨Seleccionar ({Criterio}) ...¨ 
            //                               list=¨{IdHtml}-lista¨
            //                               control-de-filtro=¨S¨/>
            //                        <datalist id=¨{IdHtml}-lista¨>
            //                        </datalist>
            //                    </div>";

            var htmlSelect = $@"<div id=¨div_{IdHtml}¨  class=¨{Css.Render(enumCssFiltro.ContenedorListaDeElementos)}¨>
                                    <select id=¨{IdHtml}¨ 
                                            propiedad=¨{Propiedad.ToLower()}¨ 
                                            class=¨{Css.Render(enumCssFiltro.ListaDeElementos)}¨ 
                                            tipo=¨{Tipo}¨
                                            clase-elemento=¨{SeleccionarDe}¨ 
                                            mostrar-expresion=¨{MostrarExpresion.ToLower()}¨  >
                                            <option value=¨0¨>Seleccionar ...</option>
                                    </select>
                                </div>";
            return htmlSelect;
        }
    }
}


#region validaciones de propiedades
//var propiedades = typeof(TElemento).GetProperties();
//var p = propiedades.FirstOrDefault(x => x.Name == filtraPor);
//IUPropiedadAttribute atributos = ElementoDto.ObtenerAtributos(p);

//if (atributos.Etiqueta.IsNullOrEmpty())
//    GestorDeErrores.Emitir($"No ha definido el atributo {nameof(atributos.Etiqueta)} de la propiedad {propiedad}");

//if (atributos.Ayuda.IsNullOrEmpty())
//    GestorDeErrores.Emitir($"No ha definido el atributo {nameof(atributos.Ayuda)} de la propiedad {propiedad}");

//if (atributos.GuardarEn.IsNullOrEmpty())
//    GestorDeErrores.Emitir($"No ha definido el atributo {nameof(atributos.GuardarEn)} de la propiedad {propiedad}");

//if (atributos.SeleccionarDe.IsNullOrEmpty())
//    GestorDeErrores.Emitir($"No ha definido el atributo {nameof(atributos.SeleccionarDe)} de la propiedad {propiedad}");
#endregion

