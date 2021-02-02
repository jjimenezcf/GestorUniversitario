using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Enumerados;

namespace MVCSistemaDeElementos.Descriptores
{
    public class ControlDeArchivo : ControlDeFormulario
    {
        public string IdHtmlNombre => $"{IdHtml}.nombre";
        public string IdHtmlSelector => $"{IdHtml}.ref";

        public int LimiteEnByte { get; set; } = 0;
        public string ExtensionesValidas { get; }

        public ControlDeArchivo(ContenedorDeBloques padre, string id, string etiqueta, string ayuda, string extensionesValidas = "*.*", int limiteEnByte = 0)
            : base(padre, id, enumTipoControl.Archivo, etiqueta, enumCssControlesFormulario.Archivo, ayuda)
        {
            ExtensionesValidas = extensionesValidas;
            LimiteEnByte = limiteEnByte;
        }

        public string RenderArchivo()
        {
            var htmlArchivo = $@"<form method=¨post¨ action=¨SubirArchivo¨ enctype=¨multipart/form-data¨>
                                   <input  {RenderAtributos(IdHtml, Tipo, ClaseCss, Ayuda)}
                                       type=¨{enumInputType.file.Render()}¨ 
                                       name=¨fichero¨  
                                       style=¨display: none;¨
                                       accept=¨{ExtensionesValidas}¨
                                       limite-en-byte = {LimiteEnByte}
                                       onChange=¨ApiDeArchivos.MostrarArchivo('{IdHtml}','{IdHtmlNombre}')¨ />
                                   <input {RenderAtributos($"{IdHtmlNombre}"
                                       , enumTipoControl.Editor
                                       , enumCssControlesFormulario.VisorDatosArchivo
                                       , ayuda: ""
                                       , $"type = ¨{enumInputType.text.Render()}¨")}
                                       readonly>
                                   </input>
                                 </form>
                                ";
            return htmlArchivo;
        }
    }
}
