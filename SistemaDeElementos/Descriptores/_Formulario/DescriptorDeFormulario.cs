using GestoresDeNegocio.Entorno;
using ServicioDeDatos.Entorno;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeFormulario
    {
        public string Id { get; }
        public string IdHtml => $"cabecera-{Id}".ToLower();
        public string IdHtmlDatos => $"datos-{Id}".ToLower();
        public string IdHtmlPie => $"pie-{Id}".ToLower();
        public string Titulo { get; }
        public string Controlador { get; }
        public string Vista { get; }

        public CabeceraDeFormulario Cabecera { get; set; }
        public CuerpoDeFormulario Cuerpo { get; set; }
        public PieDeFormulario Pie { get; set; }

        public string RutaVista { get; set; }
        public UsuarioDtm UsuarioConectado { get; internal set; }
        public GestorDeUsuarios GestorDeUsuario { get; internal set; }


        public DescriptorDeFormulario(string idHtml, string titulo, string controlador, string ruta, string vista)
        {
            Id = idHtml;
            Titulo = titulo;
            Controlador = controlador;
            Vista = vista;
            RutaVista = ruta;

            Cabecera = new CabeceraDeFormulario(this);
            Cuerpo = new CuerpoDeFormulario(this);
            Pie = new PieDeFormulario(this);
        }

        public string RenderFormulario()
        {
            string formularioHtml = $@"
            <!--  ******************* cabecera del formulario ******************* -->
            <div id=¨{IdHtml}¨ class=¨cuerpo-cabecera¨ controlador={Controlador} accion={Vista} datos={IdHtmlDatos} pie={IdHtmlPie}>
                {Cabecera.RenderCabecera()}
            </div>            
            <!--  *******************   datos del formulario   ******************* -->
            <div id=¨{IdHtmlDatos}¨ class=¨cuerpo-datos¨>
                {Cuerpo.RenderCuerpo()}
            </div>
            <!--  *******************   pie del formulario     ******************* -->
            <div id=¨{IdHtmlPie}¨ class=¨cuerpo-pie¨>
                {Pie.RenderPie()}
            </div>
            ";

            return formularioHtml;
        }

    }
}
