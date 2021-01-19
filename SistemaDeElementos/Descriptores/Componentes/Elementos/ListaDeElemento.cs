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
        public string MostrarPropiedad { get; private set; }
        public bool CargaDinamica { get; private set; }

        public ListaDeElemento(BloqueDeFitro<TElemento> padre, string etiqueta, string propiedad,  string ayuda, string  seleccionarDe,  string guardarEn, string mostrarPropiedad , bool cargaDinamica, CriteriosDeFiltrado criterioDeBusqueda, Posicion posicion)
        : base(
            padre: padre
          , id: $"{padre.Id}_{TipoControl.ListaDeElemento}_{propiedad}" 
          , etiqueta
          , propiedad
          , ayuda
          , posicion
        )
        {
            IniciarClase(padre, seleccionarDe, guardarEn, mostrarPropiedad, cargaDinamica, criterioDeBusqueda);
        }

        public ListaDeElemento(BloqueDeFitro<TElemento> padre, string propiedad, Posicion posicion)
        : base(
            padre: padre
          , id: $"{padre.Id}_{TipoControl.ListaDeElemento}_{propiedad}" 
          , etiqueta: ""
          , propiedad
          , ayuda: ""
          , posicion
          )
        {

            var propiedades = typeof(TElemento).GetProperties();
            var p = propiedades.FirstOrDefault(x => x.Name == propiedad);
            IUPropiedadAttribute atributos = ElementoDto.ObtenerAtributos(p);

            if (atributos.Etiqueta.IsNullOrEmpty())
                GestorDeErrores.Emitir($"No ha definido el atributo {nameof(atributos.Etiqueta)} de la propiedad {propiedad}");

            if (atributos.Ayuda.IsNullOrEmpty())
                GestorDeErrores.Emitir($"No ha definido el atributo {nameof(atributos.Ayuda)} de la propiedad {propiedad}");

            if (atributos.GuardarEn.IsNullOrEmpty())
                GestorDeErrores.Emitir($"No ha definido el atributo {nameof(atributos.GuardarEn)} de la propiedad {propiedad}");

            if (atributos.SeleccionarDe.IsNullOrEmpty())
                GestorDeErrores.Emitir($"No ha definido el atributo {nameof(atributos.SeleccionarDe)} de la propiedad {propiedad}");

            Etiqueta = atributos.Etiqueta;
            Ayuda = atributos.Ayuda;

            IniciarClase(padre, 
                atributos.SeleccionarDe, 
                atributos.GuardarEn, 
                atributos.MostrarPropiedad.IsNullOrEmpty() ? propiedad : atributos.MostrarPropiedad, 
                atributos.CargaDinamica,
                CriteriosDeFiltrado.contiene);
        }

        private void IniciarClase(BloqueDeFitro<TElemento> padre, string seleccionarDe, string guardarEn, string mostrarPropiedad, bool cargaDinamica, CriteriosDeFiltrado criterio)
        {
            SeleccionarDe = seleccionarDe;
            GuardarEn = guardarEn;
            CargaDinamica = cargaDinamica;
            MostrarPropiedad = mostrarPropiedad;

            Tipo = TipoControl.ListaDinamica;
            Criterio = criterio;
            padre.AnadirSelectorElemento(this);
        }


        public override string RenderControl()
        {
            if (CargaDinamica)
                return RenderListaDinamica();

            return RenderListaDeElementos();
        }

        private string RenderListaDinamica()
        {
            var htmlSelect = $@"<div id=¨div-{IdHtml}¨  class=¨contenedor-selector¨>
                                    <input id=¨{IdHtml}¨
                                           propiedad=¨{Propiedad.ToLower()}¨ 
                                           class=¨{Css.Render(enumCssFiltro.ListaDinamica)}¨ 
                                           tipo=¨{Tipo}¨
                                           clase-elemento=¨{SeleccionarDe}¨
                                           guardar-en=¨{GuardarEn}¨ 
                                           carga-dinamica='S'
                                           oninput=¨Crud.{GestorDeEventos.EventosDeListaDinamica}('cargar',this)¨ 
                                           onchange=¨Crud.{GestorDeEventos.EventosDeListaDinamica}('seleccionar',this)¨ 
                                           placeholder=¨Seleccionar ({Criterio}) ...¨ 
                                           list=¨{IdHtml}-lista¨
                                           control-de-filtro=¨S¨
                                           criterio-de-filtro=¨{Criterio}¨ />
                                    <datalist id=¨{IdHtml}-lista¨>
                                    </datalist>
                                </div>";

            return htmlSelect;
        }

        
        private string RenderListaDeElementos()
        {
            var htmlSelect = $@"<div id=¨div_{IdHtml}¨  class=¨contenedor-selector¨>
                                    <select id=¨{IdHtml}¨ 
                                         class=¨{TipoControl.ListaDeElemento}¨ 
                                          {RenderAtributos()}
                                          clase-elemento=¨{SeleccionarDe}¨ 
                                          guardar-en=¨{GuardarEn}¨ 
                                          mostrar-propiedad=¨{MostrarPropiedad.ToLower()}¨  >
                                         <option value=¨0¨>Seleccionar ...</option>
                                    </select>
                                </div>";
            return htmlSelect;
        }
    }
}


