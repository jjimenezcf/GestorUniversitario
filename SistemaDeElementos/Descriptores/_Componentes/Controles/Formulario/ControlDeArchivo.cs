﻿using System;
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
        public string IdHtmlBarra => $"{IdHtml}.barra";

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
                                       controlador=¨{Padre.Cuerpo.Formulario.Controlador}¨
                                       info-archivo=¨{IdHtmlNombre}¨
                                       limite-en-byte = {LimiteEnByte}
                                       barra-vinculada = ¨{IdHtmlBarra}¨ 
                                       onChange=¨ApiDeArchivos.MostrarArchivo('{IdHtml}','{IdHtmlNombre}')¨ />
                                   <input {RenderAtributos($"{IdHtmlNombre}"
                                       , enumTipoControl.Editor
                                       , enumCssControlesFormulario.InfoArchivo
                                       , ayuda: ""
                                       , $"type = ¨{enumInputType.text.Render()}¨")}
                                       readonly>
                                   </input>
                                   <div id = ¨{IdHtmlBarra}¨ class=¨{Css.Render(enumCssControlesDto.BarraAzulArchivo)}¨ style=¨display: none;¨>
                                       <span></span>
                                   </div>
                                 </form>
                                ";
            return htmlArchivo;
        }
    }
}
