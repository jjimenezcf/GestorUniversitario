using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ModeloDeDto;
using ModeloDeDto.Seguridad;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeMantenimiento<TElemento> : ControlHtml where TElemento : ElementoDto
    {
        public static string nombreMnt = $"{DescriptorDeCrud<TElemento>.nombreCrud}_{TipoControl.Mantenimiento}";

        public DescriptorDeCrud<TElemento> Crud => (DescriptorDeCrud<TElemento>)Padre;
        public ZonaDeMenu<TElemento> ZonaMenu { get; private set; }
        public ZonaDeFiltro<TElemento> Filtro { get; private set; }
        public ZonaDeDatos<TElemento> Datos { get; set; }

        public new string IdHtml => nombreMnt;

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
                   <Div id=¨{IdHtml}¨ class=¨div-visible¨ grid-del-mnt=¨{Datos.IdHtml}¨ filtro =¨{Filtro.IdHtml}¨ menu=¨{ZonaMenu.IdHtml}¨>
                     {htmlMnt}
                   </Div>
                ";

            foreach (var o in ZonaMenu.Menu.OpcioneDeMenu)
            {
                if (o.Accion.TipoDeAccion == TipoAccionMnt.CrearRelaciones)
                {
                    var renderModal = ((RelacionarElementos)o.Accion).RenderDeLaModal();
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