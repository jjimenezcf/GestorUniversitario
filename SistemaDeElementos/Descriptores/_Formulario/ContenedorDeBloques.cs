using System.Collections.Generic;
using Enumerados;

namespace MVCSistemaDeElementos.Descriptores
{
    public class ContenedorDeBloques
    {
        public CuerpoDeFormulario Cuerpo { get; }
        public string Id { get; }

        public string IdHtml => $"{Cuerpo.IdHtml}-{Id.ToLower()}";
        public string Titulo { get; }
        public List<ControlDeFormulario> Izquierdo { get; }
        public List<ControlDeFormulario> Derecho { get; }

        public ContenedorDeBloques(CuerpoDeFormulario cuerpo, string id, string titulo)
        {
            Cuerpo = cuerpo;
            Id = id;
            Titulo = titulo;
            Izquierdo = new List<ControlDeFormulario>();
            Derecho = new List<ControlDeFormulario>();
        }

        public string RenderBloque()
        {
            var maximoNumeroDeFila = Izquierdo.Count > Derecho.Count ? Izquierdo.Count : Derecho.Count;

            var bloqueHtml = $@"<div id = {IdHtml}-izquierdo class = {Css.Render(enumCssFormulario.BloqueIzquierdo)}>
                                    <table class = {Css.Render(enumCssFormulario.Tabla)}>
                                    <thead></thead>
                                    <tbody>
                                         {RenderTbody(Izquierdo, maximoNumeroDeFila)}
                                    </tbody>
                                </table>
                                </div>
                                <div id = {IdHtml}-derecho class = {Css.Render(enumCssFormulario.BloqueDerecho)}>
                                    <table class = {Css.Render(enumCssFormulario.Tabla)}>
                                    <thead></thead>
                                    <tbody>
                                         {RenderTbody(Derecho, maximoNumeroDeFila)}
                                    </tbody>
                                </table>
                                </div>
                              ";
            return bloqueHtml;
        }

        private string RenderTbody(List<ControlDeFormulario> controles, int numeroDeFilas)
        {
            var filas = "";
            var i = 0;
            foreach (var control in controles)
            {
                filas = $@"{filas}{RenderFila(control)}";
                i++;
            }
            if (i < numeroDeFilas)
                for (int j = i; j < numeroDeFilas; j++)
                    filas = $@"{filas}{RenderFilaVacia()}";

            return filas;
        }


        public string RenderFilaVacia()
        {
            return $@"<tr class = ¨{Css.Render(enumCssFormulario.fila)}¨>
                        <td class = ¨{Css.Render(enumCssFormulario.columnaLabel)}¨>
                        </td>
                        <td class = ¨{Css.Render(enumCssFormulario.columnaControl)}¨>
                        </td>
                      </tr>";
        }

        public string RenderFila(ControlDeFormulario control)
        {
            return $@"<tr class = ¨{Css.Render(enumCssFormulario.fila)}¨>
                        {RenderContenidoDeLaFila(control)}
                      </tr>";
        }

        private object RenderContenidoDeLaFila(ControlDeFormulario control)
        {
            switch (control.Tipo)
            {
                case enumTipoControl.Archivo: return RenderArchivo((ControlDeArchivo)control);
                case enumTipoControl.Editor: return RenderEditor(control);
            }

            throw new System.Exception($"No se ha implementado como renderizar un control del tipo {control.Tipo.Render()}");
        }

        private object RenderEditor(ControlDeFormulario control)
        {
            var htmlfilaEditor = $@"
                        <td class = ¨{Css.Render(enumCssFormulario.columnaLabel)}¨>
                           <label for=¨{control.IdHtml}¨>{control.Etiqueta}</label>
                        </td>
                        <td class = ¨{Css.Render(enumCssFormulario.columnaControl)}¨>
                           {((ControlDeEdicion)control).RenderEditor()}
                        </td>";
            return htmlfilaEditor;
        }

        private object RenderArchivo(ControlDeArchivo control)
        {
            var htmlfilaArchivo = $@"
                        <td class = ¨{Css.Render(enumCssFormulario.columnaLabel)}¨>
                           <a id=¨{control.IdHtmlSelector}¨ 
                              class=¨{Css.Render(enumCssControlesFormulario.SelectorArchivo)}¨ 
                              href=¨javascript:ApiDeArchivos.SeleccionarArchivo('{control.IdHtml}')¨>
                              {control.Etiqueta}
                           </a>
                        </td>
                        <td class = ¨{Css.Render(enumCssFormulario.columnaControl)}¨>
                           {control.RenderArchivo()}
                        </td>";
            return htmlfilaArchivo;
        }


    }
}
