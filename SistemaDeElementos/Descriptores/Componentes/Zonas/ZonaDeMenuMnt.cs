using System;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class ZonaDeMenuMnt<TElemento>: ControlHtml
    {
        public DescriptorMantenimiento<TElemento> Mnt => (DescriptorMantenimiento<TElemento>)Padre;

        public Menu<TElemento> MenuMnt { get; set; }

        public ZonaDeMenuMnt(DescriptorMantenimiento<TElemento> mnt)
        : base(
          padre: mnt,
          id: $"{mnt.Id}_{TipoControl.ZonaMenu}",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            MenuMnt = new Menu<TElemento>(this);
            Tipo = TipoControl.ZonaMenu;
        }

        public override string RenderControl()
        {
            var htmContenedorMnt =
                $@"
                   <Div id=¨{IdHtml}¨>
                     {MenuMnt.RenderControl()}
                   </Div>
                ";

            return htmContenedorMnt.Render();
        }

        internal void AnadirOpcionDeCreacion()
        {
            var mostrarCreacion = new MostrarDiv(idDivMostrar: Mnt.Crud.Creacion.Id, idDivOcultar: Mnt.Id);
            var opcion = new OpcionDeMenu<TElemento>(MenuMnt, mostrarCreacion, $"Crear {Mnt.Crud.NombreElemento}");
            MenuMnt.Add(opcion);
        }

        internal void AnadirOpcionDeEdicion()
        {
            throw new NotImplementedException();
        }
    }
}