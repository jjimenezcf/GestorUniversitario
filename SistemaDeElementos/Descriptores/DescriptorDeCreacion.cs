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

            var htmContenedorMnt =
                $@"
                   <Div id=¨{IdHtml}¨>
                     <h2>Div de creación</h2>
                   </Div>
                ";

            return htmContenedorMnt.Render();
        }
    }
}