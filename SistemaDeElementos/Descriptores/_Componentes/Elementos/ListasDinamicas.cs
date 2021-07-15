using System.Linq;
using Enumerados;
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
        public string BuscarPor { get; set; } = ltrFiltros.Nombre;
        public int LongitudMinimaParaBuscar { get; set; } = 3;
        public string FiltrarPor { get; set; }
        public int Cantidad { get; set; } = 10;
        public string RestringidoPor { get; }
        public string ContenidoEn { get; }
        public string Controlador { get; }
        public string AlSeleccionarBlanquearControl { get; }

        public BloqueDeFitro<TElemento> Bloque => (BloqueDeFitro<TElemento>)Padre;

        public ListasDinamicas(BloqueDeFitro<TElemento> bloque
            , string etiqueta
            , string filtrarPor
            , string ayuda
            , string seleccionarDe
            , string buscarPor
            , string mostrarExpresion
            , CriteriosDeFiltrado criterioDeBusqueda
            , Posicion posicion
            , string controlador
            , string restringirPor = ""
            , string alSeleccionarBlanquearControl = "")
        : base(
            padre: bloque
          , id: $"{bloque.Id}_{enumTipoControl.ListaDeElemento.Render()}_{filtrarPor}"
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

            Tipo = enumTipoControl.ListaDinamica;
            Criterio = criterioDeBusqueda;
            bloque.AnadirSelectorElemento(this);
            RestringidoPor = restringirPor;
            Controlador = controlador.Replace("Controller", "");
            AlSeleccionarBlanquearControl = alSeleccionarBlanquearControl;
        }


        public override string RenderControl()
        {
            var a = AtributosHtml.AtributosComunes($"div_{IdHtml}", IdHtml, PropiedadHtml, Tipo);
            var valores = a.MapearComunes();

            valores["CssContenedor"] = Css.Render(enumCssFiltro.ContenedorListaDinamica);
            valores["Css"] = Css.Render(enumCssFiltro.ListaDinamica);
            valores["ClaseElemento"] = SeleccionarDe;
            valores["MostrarExpresion"] = MostrarExpresion.ToLower();
            valores["BuscarPor"] = BuscarPor;
            valores["Longitud"] = LongitudMinimaParaBuscar;
            valores["Cantidad"] = Cantidad;
            valores["CriterioDeFiltro"] = Criterio;
            valores["OnInput"] = $"Crud.{GestorDeEventos.EventosDeListaDinamica}('{TipoAccionDeListaDinamica.cargar}',this)";
            valores["OnChange"] = $"Crud.{GestorDeEventos.EventosDeListaDinamica}('{TipoAccionDeListaDinamica.perderFoco}',this)";
            valores["OnFocus"] = $"Crud.{GestorDeEventos.EventosDeListaDinamica}('{TipoAccionDeListaDinamica.obtenerFoco}',this)";
            valores["Placeholder"] = $"Seleccionar ({Criterio}) ...";
            valores["RestringidoPor"] = RestringidoPor.IsNullOrEmpty() ? "" : RestringidoPor.ToLower();
            valores["PropiedadRestrictora"] = RestringidoPor.IsNullOrEmpty() ? "" : RestringidoPor.ToLower();
            valores["ContenidoEn"] = Bloque.ZonaDeFiltrado.IdHtml;
            valores["Controlador"] = Controlador;
            valores["Blanquear"] = AlSeleccionarBlanquearControl.ToLower();

            return PlantillasHtml.Render(PlantillasHtml.listaDinamicaFlt, valores);
        }


    }
}


