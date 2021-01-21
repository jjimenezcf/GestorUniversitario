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
            var valores = PlantillasHtml.ValoresDeAtributesComunes($"div_{IdHtml}", IdHtml, PropiedadHtml, Tipo);
            valores["CssContenedor"] = Css.Render(enumCssFiltro.ContenedorListaDinamica);
            valores["Css"] = Css.Render(enumCssFiltro.ListaDinamica);
            valores["ClaseElemento"] = SeleccionarDe;
            valores["MostrarExpresion"] = MostrarExpresion.ToLower();
            valores["ComoBuscar"] = BuscarPor;
            valores["Longitud"] = LongitudMinimaParaBuscar;
            valores["Cantidad"] = Cantidad;
            valores["CriterioDeFiltro"] = Criterio ;
            valores["OnInput"] = $"Crud.{GestorDeEventos.EventosDeListaDinamica}('cargar',this)";
            valores["OnChange"] = $"Crud.{GestorDeEventos.EventosDeListaDinamica}('seleccionar',this)";
            valores["Placeholder"] = $"Seleccionar ({Criterio}) ...";

            return PlantillasHtml.Render(PlantillasHtml.listaDinamicaFlt, valores);
        }


    }
}


