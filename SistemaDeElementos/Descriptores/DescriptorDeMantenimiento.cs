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

            var htmlMnt = ModoDescriptor.Mantenimiento == ((DescriptorDeCrud<TElemento>)Padre).Modo
                   ?
                   RenderTitulo() + Environment.NewLine +
                   ZonaMenu.RenderControl() + Environment.NewLine +
                   Filtro.RenderControl() + Environment.NewLine +
                   Datos.RenderControl() + Environment.NewLine
                   :
                   Filtro.RenderControl() + Environment.NewLine +
                   Datos.RenderControl() + Environment.NewLine;

            var htmContenedorMnt =
                $@"
                   <Div id=¨{IdHtml}¨ class=¨div-visible¨ grid-del-mnt=¨{Datos.IdHtml}¨ filtro =¨{Filtro.IdHtml}¨ menu=¨{ZonaMenu.IdHtml}¨ controlador=¨{Crud.Controlador}¨>
                     {htmlMnt}
                   </Div>
                ";

            foreach (var o in ZonaMenu.Menu.OpcioneDeMenu)
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

        public string RenderMntModal(string idModal)
        {
            Datos.IdHtmlModal = idModal.ToLower();

            var htmlMnt =
                   Filtro.RenderControl() + Environment.NewLine +
                   Datos.RenderControl() + Environment.NewLine;

            var htmContenedorMnt =
                $@"
                   <Div id=¨{IdHtml}¨ class=¨div-visible¨ grid-del-mnt=¨{Datos.IdHtml}¨ filtro =¨{Filtro.IdHtml}¨ >
                     {htmlMnt}
                   </Div>
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