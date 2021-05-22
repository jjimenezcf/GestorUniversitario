using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeExpansor: ControlHtml
    {
        public List<ControlHtml> Controles = new List<ControlHtml>();

        public DescriptorDeExpansor(ControlHtml padre, string id, string etiqueta, string ayuda)
        : base(padre, id, etiqueta, "", ayuda, null)
        {

        }

        public override string RenderControl()
        {
            return RenderExpansor();
        }

        public string RenderExpansor()
        {
            var valores = new Dictionary<string, object>();
            valores["IdHtml"] = IdHtml;
            valores["cssClase"] = Css.Render(enumCssExpansor.Contenedor);
            valores["cssCabecera"] = Css.Render(enumCssExpansor.Cabecera);
            valores["cssCuerpo"] = Css.Render(enumCssExpansor.Cuerpo);
            valores["cssPie"] = Css.Render(enumCssExpansor.Pie);
            valores["RenderCabeceraDelExpansor"] = RenderCabeceraExpansor();
            valores["RenderCuerpoDelExpansor"] = RenderCuerpoExpansor();
            valores["RenderPieDelExpansor"] = "";

            return PlantillasHtml.Render(PlantillasHtml.Expansor, valores);
        }

        private string RenderCabeceraExpansor()
        {
            var valores = new Dictionary<string, object>();
            valores["IdHtml"] = IdHtml;
            valores["cssClase"] = Css.Render(enumCssExpansor.Expansor);
            valores["Evento"] = $"Crud.EventosDeExpansores('{TipoDeAccionExpansor.OcultarMostrarBloque}','expandir.{IdHtml}.input;{IdHtml}-cuerpo')";
            valores["Titulo"] = Etiqueta;

            return PlantillasHtml.Render(PlantillasHtml.CabeceraExpansor, valores);
        }

        private string RenderCuerpoExpansor()
        {
            var htmlCuerpoDelExpansor = "";
            foreach(var control in Controles)
            {
                htmlCuerpoDelExpansor = $"{(htmlCuerpoDelExpansor.IsNullOrEmpty() ? "" : htmlCuerpoDelExpansor + Environment.NewLine)}{RenderControlDelCuerpo(control)}";
            }

            return htmlCuerpoDelExpansor;
        }

        private object RenderControlDelCuerpo(ControlHtml control)
        {
            var valores = new Dictionary<string, object>();
            valores["IdHtml"] = control.IdHtml;
            valores["cssClase"] = Css.Render(enumCssExpansor.ContenedorDeControl);
            
            valores["RenderEtiqueta"] = control.Tipo == Enumerados.enumTipoControl.Opcion 
                   ? "" : 
                   RenderEtiqueta(IdHtml, control.Etiqueta);
            
            valores["RenderControl"] = control.RenderControl();

            return PlantillasHtml.Render(PlantillasHtml.ControlDelExpansor, valores);
        }
    }
}
