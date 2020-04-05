using System;

namespace MVCSistemaDeElementos.Descriptores
{
    public class ZonaDeMenuMnt<TElemento>: ControlHtml
    {
        public MenuMantenimiento<TElemento> MenuMnt { get; set; }

        public ZonaDeMenuMnt(DescriptorMantenimiento<TElemento> mnt)
        : base(
          padre: mnt,
          id: $"{mnt.Id}_Menu",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {

        }

        public override string RenderControl()
        {
            return MenuMnt.RenderControl();
        }

        internal void AnadirOpcioDeCreacion()
        {
            var mnt = (DescriptorMantenimiento<TElemento>)Padre;
            var vistaCreacion = ((DescriptorDeCrud<TElemento>)mnt.Padre).VistaCreacion;
            MenuMnt = new MenuMantenimiento<TElemento>(this, vistaCreacion);
        }
    }
}