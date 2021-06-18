using System;
using System.Collections.Generic;
using System.Linq;
using Enumerados;
using Gestor.Errores;
using GestorDeElementos;
using ModeloDeDto;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{
    public class ListaDeValores<TElemento> : ControlFiltroHtml where TElemento : ElementoDto
    {

        public string GuardarEn { get; private set; }
        public Dictionary<string,string> Opciones { get; private set; }

        //string etiqueta, string filtrarPor, string ayuda, string seleccionarDe, string buscarPor, string mostrarExpresion, CriteriosDeFiltrado criterioDeBusqueda, Posicion posicion)

        public ListaDeValores(BloqueDeFitro<TElemento> padre, string etiqueta, string ayuda, Dictionary<string,string> opciones, string filtraPor,  Posicion posicion)
        : base(
            padre: padre
          , id: $"{padre.Id}_{enumTipoControl.ListaDeValores.Render()}_{filtraPor}"
          , etiqueta: etiqueta
          , filtraPor
          , ayuda: ayuda
          , posicion
          )
        {
            Tipo = enumTipoControl.ListaDeValores;
            Opciones = opciones;
            padre.AnadirLista(this);
        }

        public override string RenderControl()
        {
           return RenderListaDeValores();
        }

        public string RenderListaDeValores()
        {
            var a = AtributosHtml.AtributosComunes($"div_{IdHtml}", IdHtml, PropiedadHtml, Tipo);
            var atributos = a.MapearComunes();

            atributos["CssContenedor"] = Css.Render(enumCssFiltro.ContenedorListaDeElementos);
            atributos["Css"] = Css.Render(enumCssFiltro.ListaDeElementos);

            var lista = PlantillasHtml.Render(PlantillasHtml.listaDeValoresFlt, atributos);
            var opciones = "<option value='-1'>Seleccionar ...</option>";
            foreach(var clave in Opciones.Keys)
                opciones = $"{opciones}{Environment.NewLine}<option value='{clave}'>{Opciones[clave]}</option>";

           return lista.Replace("[opcionesDeLaLista]", opciones);
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

