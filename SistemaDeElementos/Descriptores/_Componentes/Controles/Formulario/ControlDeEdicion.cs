using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enumerados;

namespace MVCSistemaDeElementos.Descriptores
{
    public class ControlDeEdicion : ControlDeFormulario
    {
        public ControlDeEdicion(ContenedorDeBloques padre, string id, string etiqueta,string ayuda)
            : base(padre, id, enumTipoControl.Editor, etiqueta, enumCssControlesFormulario.Editor, ayuda)
        {
        }

        public string RenderEditor()
        {
            return $@"<input {RenderAtributos(Id, IdHtml, Tipo, ClaseCss, Ayuda, $"type = ¨text¨")}></input>";
        }
    }
}
