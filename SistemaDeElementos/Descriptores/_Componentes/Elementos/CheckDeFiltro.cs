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

        public string Accion { get; private set; }


        public CheckFiltro(BloqueDeFitro<TElemento> bloque, string etiqueta, string filtrarPor, string ayuda, bool valorInicial, bool filtrarPorFalse, Posicion posicion, string accion = null)
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
            Accion = accion;
        }

        public override string RenderControl()
        {
            return RenderCheckFlt();
        }

        public string RenderCheckFlt()
        {
            var render = RenderCheck(PlantillasHtml.checkFlt, IdHtml, PropiedadHtml, ValorInicial, Etiqueta, Accion);
            return render.Replace("[FiltrarPorFalse]", FiltrarPorFalse ? "S" : "N");

            //var a = AtributosHtml.AtributosComunes($"div_{IdHtml}", IdHtml, PropiedadHtml, Tipo);
            //var valores = a.MapearComunes();
            //valores["CssContenedor"] = Css.Render(enumCssFiltro.ContenedorCheck);
            //valores["Css"] = Css.Render(enumCssFiltro.Check);
            //valores["Checked"] = ValorInicial.ToString().ToLower();
            //valores["Etiqueta"] = Etiqueta;
            //valores["Accion"] = Accion;
            //valores["FiltrarPorFalse"] = FiltrarPorFalse ? "S" : "N";

            //return PlantillasHtml.Render(PlantillasHtml.checkFlt, valores);
        }

    }

}