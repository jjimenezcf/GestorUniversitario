using System.Collections.Generic;
using Enumerados;
using GestorDeElementos;
using ModeloDeDto;

namespace MVCSistemaDeElementos.Descriptores
{
    public class CheckDeAccionFlt<TElemento> : ControlFiltroHtml where TElemento : ElementoDto
    {
        public bool ValorInicial { get; private set; }

        public string Accion { get; private set; }


        public CheckDeAccionFlt(BloqueDeFitro<TElemento> bloque, string id,  string etiqueta, string ayuda, bool valorInicial, Posicion posicion, string accion)
        : base(padre: bloque
              , id: id
              , etiqueta
              , ""
              , ayuda
              , posicion
              )
        {
            Tipo = enumTipoControl.Check;
            bloque.Tabla.Dimension.CambiarDimension(posicion);
            bloque.AnadirControlEn(this);
            ValorInicial = valorInicial;
            Accion = accion;
        }

        public override string RenderControl()
        {
            return RenderCheck();
        }

        public string RenderCheck()
        {
            var a = AtributosHtml.AtributosComunes($"div_{IdHtml}", IdHtml, PropiedadHtml, Tipo);
            var valores = a.MapearComunes();
            valores["CssContenedor"] = Css.Render(enumCssFiltro.ContenedorCheck);
            valores["Css"] = Css.Render(enumCssFiltro.Check);
            valores["Etiqueta"] = Etiqueta;
            valores["Checked"] = ValorInicial.ToString().ToLower();
            valores["Accion"] = Accion;

            return PlantillasHtml.Render(PlantillasHtml.checkFlt, valores);
        }

    }

}