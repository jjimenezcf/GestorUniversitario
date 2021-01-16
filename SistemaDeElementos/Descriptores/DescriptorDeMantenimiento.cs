using System;
using ModeloDeDto;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeMantenimiento<TElemento> : ControlHtml where TElemento : ElementoDto
    {
        public static string NombreMnt = $"{DescriptorDeCrud<TElemento>.NombreCrud}_{TipoControl.Mantenimiento}".ToLower();

        public DescriptorDeCrud<TElemento> Crud => (DescriptorDeCrud<TElemento>)Padre;
        public ZonaDeMenu<TElemento> ZonaMenu { get; private set; }
        public ZonaDeFiltro<TElemento> Filtro { get; private set; }
        public ZonaDeDatos<TElemento> Datos { get; set; }

        public new string IdHtml => NombreMnt;

        public string IdHtmlZonaNavegador => $"cuerpo.pie.{IdHtml}";

        public DescriptorDeMantenimiento(DescriptorDeCrud<TElemento> crud, string etiqueta)
            : base(
              padre: crud,
              id: $"{crud.Id}_{TipoControl.Mantenimiento}",
              etiqueta: etiqueta,
              propiedad: null,
              ayuda: null,
              posicion: null
            )
        {
            Tipo = TipoControl.Mantenimiento;
            ZonaMenu = new ZonaDeMenu<TElemento>(mnt: this);
            Filtro = new ZonaDeFiltro<TElemento>(mnt: this);
            Datos = new ZonaDeDatos<TElemento>(mnt: this);
        }

        public override string RenderControl()
        {

            var htmlCuerpoCabecera = RenderCuerpoCabecera(RenderTitulo(), RenderMenuDelMnt());
            var htmlCuerpoDatos = RenderCuerpoDatos(Filtro.RenderControl(), Datos.RenderControl());
            var htmlCuerpoPie = RenderCuerpoPie();

            var htmContenedorMnt =
                $@"  
                  <!--  ******************* Cabecera del cuerpo: título y menú ******************* -->
                     {htmlCuerpoCabecera}

                  <!--  ******************* Datos del cuerpo: filtro y grid de datos ******************* -->
                     {htmlCuerpoDatos}

                  <!--  ******************* Pié del cuerpo: zona de navegación ******************* -->
                     {htmlCuerpoPie}
                ";

            foreach (var o in ZonaMenu.Menu.OpcionesDeMenu)
            {
                if (o.Accion.TipoDeAccion == TipoDeAccionDeMnt.AbrirModalParaRelacionar)
                {
                    var renderModal = ((RelacionarElementos)o.Accion).RenderDeLaModal();
                    htmContenedorMnt = htmContenedorMnt + Environment.NewLine + renderModal;
                }

                if (o.Accion.TipoDeAccion == TipoDeAccionDeMnt.AbrirModalParaConsultarRelaciones)
                {
                    var renderModal = ((ConsultarRelaciones)o.Accion).RenderDeLaModal();
                    htmContenedorMnt = htmContenedorMnt + Environment.NewLine + renderModal;
                }
            }

            htmContenedorMnt = htmContenedorMnt + Environment.NewLine + Filtro.RenderModalesFiltro();

            return htmContenedorMnt.Render();
        }
        private string RenderCuerpoCabecera(string htmlTitulo, string htmlMenu)
        {
            var propiedades = $@" id=¨{IdHtml}¨ 
                        class=¨{Css.Render(enumCssCuerpo.CuerpoCabecera)}¨ 
                        grid-del-mnt=¨{Datos.IdHtml}¨ 
                        zona-de-filtro=¨{Filtro.IdHtml}¨ 
                        zona-de-menu=¨{ZonaMenu.IdHtml}¨ 
                        controlador=¨{Crud.Controlador}¨ 
                        negocio=¨{Crud.Negocio}¨>
                     ";

            return ModoDescriptor.Mantenimiento == ((DescriptorDeCrud<TElemento>)Padre).Modo ?
            $@"<div {propiedades}
                    {htmlTitulo}
                    {htmlMenu}
               </div>
                " :
            $@"<div {propiedades}>
               </div>";
        }

        private object RenderCuerpoDatos(string htmlFiltro, string htmlDatos)
        {
            return
            $@"<div id=¨cuerpo.datos.{IdHtml}¨ class=¨{Css.Render(enumCssCuerpo.CuerpoDatos)}¨>
                     {htmlFiltro}
                     {htmlDatos}
               </div>";
        }

        private object RenderCuerpoPie()
        {
            return $@"<div id=¨{IdHtmlZonaNavegador}¨ class=¨{Css.Render(enumCssCuerpo.CuerpoPie)}¨>
                       {Datos.Grid.NavegadorToHtml()}
                     </div>";
        }


        private string RenderMenuDelMnt()
        {
            var htmlParteSuperiror = $@"
                                <!--  ******************* menú ******************* -->
                                <div id = ¨{IdHtml}.MenuDelMnt¨ class=¨{Css.Render(enumCssMnt.MntMenuContenedor)}¨>  
                                   <div id = ¨{IdHtml}¨  class=¨{Css.Render(enumCssDiv.DivVisible)} {Css.Render(enumCssMnt.MntMenuZona)}¨>     
                                     {ZonaMenu.RenderControl()} 
                                    </div>
                                    <div id = ¨div.mostrar.{IdHtml}¨ class=¨{Css.Render(enumCssDiv.DivVisible)} {Css.Render(enumCssMnt.MntFiltroExpansor)}¨>     
                                      <a id = ¨mostrar.{IdHtml}.ref¨ href=¨javascript:Crud.{GestorDeEventos.EventosDelMantenimiento}('{TipoDeAccionDeMnt.OcultarMostrarFiltro}', '{("")}');¨>Ocultar filtro</a>
                                      <input id=¨expandir.{IdHtml}¨ type=¨hidden¨ value=¨1¨ >  
                                    </div>
                                </div>";


            return htmlParteSuperiror;
        }

        public string RenderMntModal(string idModal)
        {
            Datos.IdHtmlModal = idModal.ToLower();

            var htmlMnt =
                   Filtro.RenderControl() + Environment.NewLine +
                   Datos.RenderControl() + Environment.NewLine;

            var htmContenedorMnt =
                $@"
                   <div id=¨{IdHtml}¨ class=¨{Css.Render(enumCssDiv.DivVisible)}¨ grid-del-mnt=¨{Datos.IdHtml}¨ filtro =¨{Filtro.IdHtml}¨ >
                     {htmlMnt}
                   </div>
                ";

            return htmContenedorMnt.Render();
        }

        public string RenderTitulo()
        {
            var htmlCabecera = $"<h2>{this.Etiqueta}</h2>";
            return htmlCabecera;
        }
    }
}