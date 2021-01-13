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
                  <!--  ******************* título y menú ******************* -->
                     {htmlCuerpoCabecera}

                  <!--  ******************* zona de navegación ******************* -->
                     {htmlCuerpoPie}

                  <!--  ******************* filtro y grid de datos ******************* -->
                     {htmlCuerpoDatos}
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
                        class=¨{ClaseCss.Render(enumClaseCcsCuerpo.CuerpoCabecera)}¨ 
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
            $@"<div id=¨cuerpo.datos.{IdHtml}¨ class=¨{ClaseCss.Render(enumClaseCcsCuerpo.CuerpoDatos)}¨>
                     {htmlFiltro}
                     {htmlDatos}
               </div>";
        }

        private object RenderCuerpoPie()
        {
           return $@"<div id=¨cuerpo.pie.{IdHtml}¨ class=¨{ClaseCss.Render(enumClaseCcsCuerpo.CuerpoPie)}¨>
                       <h2>{Etiqueta}</h2>
                     </div>";
        }


        private string RenderMenuDelMnt()
        {
            var htmlParteSuperiror = $@"
                                <!--  ******************* menú ******************* -->
                                <div id = ¨{IdHtml}.MenuDelMnt¨ class=¨{ClaseCss.Render(enumClaseCcsMnt.MntMenuContenedor)}¨>  
                                   <div id = ¨{IdHtml}¨  class=¨{ClaseCss.Render(enumClaseCcsDiv.DivVisible)} {ClaseCss.Render(enumClaseCcsMnt.MntMenuZona)}¨>     
                                     {ZonaMenu.RenderControl()} 
                                    </div>
                                    <div id = ¨mostrar.{IdHtml}¨ class=¨{ClaseCss.Render(enumClaseCcsDiv.DivVisible)} {ClaseCss.Render(enumClaseCcsMnt.MntFiltroExpansor)}¨>     
                                      <a id = ¨mostrar.{IdHtml}.ref¨ href=¨javascript:Crud.{GestorDeEventos.EventosDelMantenimiento}('ocultar-mostrar-filtro', '{("")}');¨>Ocultar filtro</a>
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
                   <div id=¨{IdHtml}¨ class=¨{ClaseCss.Render(enumClaseCcsDiv.DivVisible)}¨ grid-del-mnt=¨{Datos.IdHtml}¨ filtro =¨{Filtro.IdHtml}¨ >
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