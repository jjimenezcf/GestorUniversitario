using System.Collections.Generic;
using Enumerados;
using GestorDeElementos;
using ModeloDeDto;

namespace MVCSistemaDeElementos.Descriptores
{
    public class CheckFiltro<TElemento> : ControlFiltroHtml where TElemento : ElementoDto
    {
        public bool ValorInicial { get; private set; }
        public bool FiltrarPorFalse { get; private set; }

        public CheckFiltro(BloqueDeFitro<TElemento> bloque, string etiqueta, string filtrarPor, string ayuda, bool valorInicial, bool filtrarPorFalse, Posicion posicion)
        : base(padre: bloque
              , id: $"{bloque.Id}_{enumTipoControl.Check.Render()}_{filtrarPor}"
              , etiqueta
              , filtrarPor
              , ayuda
              , posicion
              )
        {
            Tipo = enumTipoControl.Check;
            Criterio = CriteriosDeFiltrado.igual;
            bloque.Tabla.Dimension.CambiarDimension(posicion);
            bloque.AnadirControlEn(this);
            ValorInicial = valorInicial;
        }

        public override string RenderControl()
        {
            return RenderCheck();
        }

        public string RenderCheck()
        {
            var valores = PlantillasHtml.ValoresDeAtributesComunes($"div_{IdHtml}", IdHtml, PropiedadHtml, Tipo);
            valores["CssContenedor"] = Css.Render(enumCssFiltro.ContenedorCheck);
            valores["Css"] = Css.Render(enumCssFiltro.Check);
            valores["Etiqueta"] = Etiqueta;
            valores["Checked"] = ValorInicial.ToString().ToLower();
            valores["FiltrarPorFalse"] = FiltrarPorFalse ? "S" : "N";

            return PlantillasHtml.Render(PlantillasHtml.checkFlt, valores);
        }

    }

}