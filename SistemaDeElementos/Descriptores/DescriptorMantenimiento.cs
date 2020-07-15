using System;
using Gestor.Elementos.ModeloIu;
using GestorDeEntorno.Migrations;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorMantenimiento<TElemento>: ControlHtml where TElemento : ElementoDto
    {
        public static string nombreMnt = $"{DescriptorDeCrud<TElemento>.nombreCrud}_{TipoControl.Mantenimiento}";

        public DescriptorDeCrud<TElemento> Crud => (DescriptorDeCrud<TElemento>)Padre;
        public BarraDeMenu<TElemento> MenuDeMnt { get; private set; }
        public ZonaDeFiltro<TElemento> Filtro { get; private set; }
        public ZonaDeDatos<TElemento> Datos { get; set; }

        public new string IdHtml => nombreMnt;

        public DescriptorMantenimiento(DescriptorDeCrud<TElemento> crud, string etiqueta)
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
            MenuDeMnt = new BarraDeMenu<TElemento>(mnt: this);
            Filtro = new ZonaDeFiltro<TElemento>(mnt: this);
            Datos = new ZonaDeDatos<TElemento>(mnt: this);  
        }

        public override string RenderControl()
        {

            var htmlMnt = ModoDescriptor.Mantenimiento == ((DescriptorDeCrud<TElemento>)Padre).Modo
                   ?
                   RenderTitulo() + Environment.NewLine +
                   MenuDeMnt.RenderControl() + Environment.NewLine +
                   Filtro.RenderControl() + Environment.NewLine +
                   Datos.RenderControl() + Environment.NewLine
                   :
                   Filtro.RenderControl() + Environment.NewLine +
                   Datos.RenderControl() + Environment.NewLine;

            var htmContenedorMnt =
                $@"
                   <Div id=¨{IdHtml}¨ class=¨div-visible¨ grid=¨{Datos.IdHtml}¨ filtro =¨{Filtro.IdHtml}¨ menu=¨{MenuDeMnt.IdHtml}¨>
                     {htmlMnt}
                   </Div>
                ";

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
                   <Div id=¨{IdHtml}¨ class=¨div-visible¨ grid=¨{Datos.IdHtml}¨ filtro =¨{Filtro.IdHtml}¨ >
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