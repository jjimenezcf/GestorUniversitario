using System.Collections.Generic;

namespace MVCSistemaDeElementos.Descriptores
{
    public class CuerpoDeFormulario
    {
        DescriptorDeFormulario Formulario { get; }

        public List<ContenedorDeBloques> Contenedores = new List<ContenedorDeBloques>();

        public string IdHtml => $"datos-{Formulario.Id}".ToLower();

        public CuerpoDeFormulario(DescriptorDeFormulario formulario)
        {
            Formulario = formulario;
        }

        public string RenderCuerpo()
        {
            var htmlDeUnContenedor = $@"
                <!-- ****************   Bloque de Titulo ************************  -->
                <div id=¨idContenedor¨ class=¨{Css.Render(enumCssFormulario.ContenedorDeBloques)}¨>
                  <div id=¨idContenedor-expansor¨ class=¨{Css.Render(enumCssFormulario.BloqueExpansor)}¨>
                      <a id=¨mostrar.{IdHtml}.ref¨ 
                         class=¨{Css.Render(enumCssFormulario.referenciaExpansor)}¨
                         href=¨javascript:Formulario.{GestorDeEventos.EventosDelFormulario}('{TipoDeAccionDeMnt.OcultarMostrarBloque}', 'idContenedor-datos');¨>                           
                         bloque: Titulo
                      </a>
                      <input id=¨expandir.idContenedor-datos.input¨ type=¨hidden¨ value=¨1¨/>
                  </div>
                  <div id=¨idContenedor-datos¨ class=¨{Css.Render(enumCssFormulario.BloqueDatos)}¨>
                     RenderBloque
                  </div>
                </div>";
            var htmlContenedores = "";
            foreach(var contedor in Contenedores)
            {
                htmlContenedores = $"{htmlContenedores}{htmlDeUnContenedor}"
                    .Replace("idContenedor", contedor.IdHtml)
                    .Replace("Titulo",contedor.Titulo)
                    .Replace("RenderBloque", contedor.RenderBloque());
            }

            return htmlContenedores;
        }
    }
}