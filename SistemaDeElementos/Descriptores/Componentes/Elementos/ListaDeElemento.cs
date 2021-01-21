using System.Collections.Generic;
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
        public string MostrarExpresionHtml => MostrarExpresion.ToLower();



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
            var valores = new Dictionary<string,object>();

            valores["IdHtmlContenedor"] = $"div_{IdHtml}";
            valores["IdHtml"] = IdHtml;
            valores["CssContenedor"] = Css.Render(enumCssFiltro.ContenedorListaDeElementos);
            valores["Propiedad"] = PropiedadHtml;
            valores["Css"] = Css.Render(enumCssFiltro.ListaDeElementos);
            valores["Tipo"] = Tipo;
            valores["SeleccionarDe"] = SeleccionarDe;
            valores["MostrarExpresion"] = MostrarExpresionHtml;

            return PlantillasHtml.Render(PlantillasHtml.selectorFlt, valores);

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

