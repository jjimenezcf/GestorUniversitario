
using System.Collections.Generic;
using Enumerados;
using ModeloDeDto;

namespace MVCSistemaDeElementos.Descriptores
{

    public class ControlDeFormulario
    {
        public string Id { get;}
        public string IdHtml => $@"{Padre.IdHtml}-{Id}".ToLower();
        public string Etiqueta { get;}
        public string Ayuda { get; }
        public enumTipoControl Tipo { get; }
        public enumCssControlesFormulario ClaseCss {get;}

        public ContenedorDeBloques Padre { get; set; }

        public ControlDeFormulario(ContenedorDeBloques padre, string id, enumTipoControl tipo,  string etiqueta, enumCssControlesFormulario claseCss, string ayuda)
        {
            Padre = padre;
            Id = id;
            Etiqueta = etiqueta;
            Ayuda = ayuda;
            ClaseCss = claseCss;
            Tipo = tipo;
        }


        public static string RenderAtributos(string idHtml, enumTipoControl tipo, enumCssControlesFormulario clase, string ayuda,  string otrosAtributos = "")
        {
            var atributos = $@"id=¨{idHtml}¨ {otrosAtributos}
                            tipo=¨{tipo.Render()}¨
                            class=¨{Css.Render(clase)}¨
                            title=¨{ayuda}¨";
            return atributos;
        }

    }

}