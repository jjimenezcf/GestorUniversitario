using System.Linq;
using Gestor.Errores;
using GestorDeElementos;
using ModeloDeDto;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{
    public class ListasDinamicas<TElemento> : ControlFiltroHtml where TElemento : ElementoDto
    {

        public string SeleccionarDe { get; private set; }
        public string MostrarExpresion { get; private set; }
        public string BuscarPor { get; set; } = CamposDeFiltrado.PorDefecto;
        public int LongitudMinimaParaBuscar { get; set; } = 3;
        public string FiltrarPor { get; set; }
        public int Cantidad { get; set; } = 10;

        public ListasDinamicas(BloqueDeFitro<TElemento> padre, string etiqueta, string filtrarPor, string ayuda, string seleccionarDe, string buscarPor, string mostrarExpresion, CriteriosDeFiltrado criterioDeBusqueda, Posicion posicion)
        : base(
            padre: padre
          , id: $"{padre.Id}_{TipoControl.ListaDeElemento}_{filtrarPor}"
          , etiqueta
          , propiedad: filtrarPor
          , ayuda
          , posicion
        )
        {
            SeleccionarDe = seleccionarDe;
            FiltrarPor = filtrarPor;
            BuscarPor = buscarPor;
            MostrarExpresion = mostrarExpresion;

            Tipo = TipoControl.ListaDinamica;
            Criterio = criterioDeBusqueda;
            padre.AnadirSelectorElemento(this);
        }


        public override string RenderControl()
        {
            var htmlSelect = $@"<div id=¨div-{IdHtml}¨  class=¨{Css.Render(enumCssFiltro.ContenedorListaDinamica)}¨>
                                    <input id=¨{IdHtml}¨
                                           propiedad=¨{Propiedad.ToLower()}¨ 
                                           class=¨{Css.Render(enumCssFiltro.ListaDinamica)}¨ 
                                           tipo=¨{Tipo}¨
                                           carga-dinamica='S'
                                           clase-elemento=¨{SeleccionarDe}¨
                                           mostrar-expresion=¨{MostrarExpresion.ToLower()}¨  
                                           como-buscar='{BuscarPor}'
                                           criterio-de-filtro=¨{Criterio}¨ 
                                           filtrar-por=¨{Propiedad.ToLower()}¨ 
                                           longitud='{LongitudMinimaParaBuscar}'
                                           cantidad-a-leer= '{Cantidad}'
                                           oninput=¨Crud.{GestorDeEventos.EventosDeListaDinamica}('cargar',this)¨ 
                                           onchange=¨Crud.{GestorDeEventos.EventosDeListaDinamica}('seleccionar',this)¨ 
                                           placeholder=¨Seleccionar ({Criterio}) ...¨ 
                                           list=¨{IdHtml}-lista¨
                                           control-de-filtro=¨S¨/>
                                    <datalist id=¨{IdHtml}-lista¨>
                                    </datalist>
                                </div>";

            return htmlSelect;
        }


    }
}


