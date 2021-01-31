
using System.Collections.Generic;

namespace MVCSistemaDeElementos.Descriptores
{

    public class ControlDeFormulario
    {
        public string Id { get;}
        public string IdHtml => $@"{Padre.IdHtml}-{Id}".ToLower();
        public string Etiqueta { get;}
        public string Ayuda { get; }
        public string enumCssControlesFormulario { get; }
        public string Tipo { get; }
        public enumCssControlesFormulario ClaseCss {get;}

        public ContenedorDeBloques Padre { get; set; }

        public ControlDeFormulario(ContenedorDeBloques padre, string id, string etiqueta, enumCssControlesFormulario claseCss, string ayuda)
        {
            Padre = padre;
            Id = id;
            Etiqueta = etiqueta;
            Ayuda = ayuda;
            ClaseCss = claseCss;
        }


        public virtual string RenderAtributos(string atributos = "")
        {
            atributos += $@"id=¨{IdHtml}¨ {atributos}
                            tipo=¨{Tipo}¨
                            class=¨{Css.Render(ClaseCss)}¨
                            title=¨{Ayuda}¨";
            return atributos;
        }

    }

}