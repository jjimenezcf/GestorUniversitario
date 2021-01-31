using GestoresDeNegocio.Entorno;
using ServicioDeDatos.Entorno;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeFormulario
    {
        public string Id { get; }
        
        public string IdHtml => Id.ToLower();

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
            <!--  ******************* cabecera de los datos del formulario ******************* -->
            <div id=¨{Cabecera.IdHtml}¨ class=¨cuerpo-cabecera¨ controlador={Controlador} accion={Vista} datos={Cuerpo.IdHtml} pie={Pie.IdHtml}>
                {Cabecera.RenderCabecera()}
            </div>            
            <!--  *******************   datos del formulario   ******************* -->
            <div id=¨{Cuerpo.IdHtml}¨ class=¨cuerpo-datos¨ style= ¨grid-template-rows: 0% 0% 100%;¨>
                <div id = formulario-filtro class=¨{Css.Render(enumCssCuerpo.CuerpoDatosFiltro)}¨ style=¨display: none; height: 0px; width: 0px;¨>
                </div>
                <div id = formulario-grid class=¨{Css.Render(enumCssCuerpo.CuerpoDatosGrid)}¨ style=¨display: none; height: 0px; width: 0px;¨>
                </div>
                <div id = formulario-cuerpo class=¨{Css.Render(enumCssCuerpo.CuerpoDatosFormulario)}¨>
                   {Cuerpo.RenderCuerpo()}
                </div>
            </div>
            <!--  *******************   pie del formulario     ******************* -->
            <div id=¨{Pie.IdHtml}¨ class=¨{Css.Render(enumCssCuerpo.CuerpoPie)}¨ style= ¨grid-template-columns: 0% 0% 0% 0% 100%;¨>
               <div id=¨formulario-navegador¨ class=¨{Css.Render(enumCssNavegadorEnMnt.Navegador)}¨ style=¨display: none; height: 0px; width: 0px;¨>
               </div>
               <div id=¨formulario-opciones¨ class=¨{Css.Render(enumCssNavegadorEnMnt.Opcion)}¨ style=¨display: none; height: 0px; width: 0px;¨>
               </div>
               <div id=¨formulario-mensaje¨ class=¨{Css.Render(enumCssNavegadorEnMnt.Mensaje)}¨ style=¨display: none; height: 0px; width: 0px;¨>
               </div>
               <div id=¨formulario-infoGrid¨ class=¨{Css.Render(enumCssNavegadorEnMnt.InfoGrid)}¨ style=¨display: none; height: 0px; width: 0px;¨>
               </div>
               <div id=¨{Pie.IdHtml}-formulario¨ class=¨{Css.Render(enumCssCuerpo.CuerpoPieFormulario)}¨>
                 {Pie.RenderPie()}
               </div>
            </div>
            ";

            return formularioHtml;
        }

    }
}
