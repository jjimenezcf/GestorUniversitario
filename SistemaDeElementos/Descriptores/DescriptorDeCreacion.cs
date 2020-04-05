using System;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeCreacion<TElemento> : ControlHtml
    {
        public DescriptorDeCrud<TElemento> Crud => (DescriptorDeCrud<TElemento>)Padre;

        public DescriptorDeCreacion(DescriptorDeCrud<TElemento> crud, string etiqueta)
        : base(
          padre: crud,
          id: $"{crud.Id}_{TipoControl.Creacion}",
          etiqueta: etiqueta,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.Creacion;
        }

        public override string RenderControl()
        {
            var htmlOpcionMenu = $"<input id=¨idAceptar¨ type=¨button¨ value=¨Aceptar¨ onClick=¨Menu.EjecutarAccionMenu('{Crud.Mnt.IdHtml}','{IdHtml}')¨ />";

            var htmContenedorMnt =
                $@"
                   <Div id=¨{IdHtml}¨ class=¨div-no-visible¨>
                     <h2>Div de creación</h2>
                     {htmlOpcionMenu}
                   </Div>
                ";

            return htmContenedorMnt.Render();
        }
    }
}