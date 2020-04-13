using System;
using Gestor.Elementos.ModeloIu;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class ZonaDeMenuMnt<TElemento> : ControlHtml
    {
        public DescriptorMantenimiento<TElemento> Mnt => (DescriptorMantenimiento<TElemento>)Padre;


        public DescriptorDeCreacion<TElemento> Creador => (DescriptorDeCreacion<TElemento>)Padre;



        public Menu<TElemento> Menu { get; set; }

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
            Menu = new Menu<TElemento>(this);
            Tipo = TipoControl.ZonaMenu;
        }
        public ZonaDeMenuMnt(DescriptorDeCreacion<TElemento> creador)
        : base(
          padre: creador,
          id: $"{creador.Id}_{TipoControl.ZonaMenu}",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Menu = new Menu<TElemento>(this);
            Tipo = TipoControl.ZonaMenu;
        }


        public override string RenderControl()
        {
            var htmContenedorMnt =
                $@"
                   <Div id=¨{IdHtml}¨>
                     {Menu.RenderControl()}
                   </Div>
                ";

            return htmContenedorMnt.Render();
        }

        internal void AnadirOpcionDeCreacion()
        {

            var gestor = (string)Elemento.ValorDelAtributo(typeof(TElemento), nameof(IUDtoAttribute.ClaseParaCreacion));
            var iraCrear = new IrACrear(idDivMostrar: Mnt.Crud.Creador.Id, idDivOcultar: Mnt.Id, gestor: gestor == null ? "" : gestor);
            var opcion = new OpcionDeMenu<TElemento>(Menu, iraCrear, $"Nuevo {Mnt.Crud.NombreElemento}");
            Menu.Add(opcion);
        }

        internal void AnadirOpcionDeNuevoElemento()
        {
            var gestor = (string)Elemento.ValorDelAtributo(typeof(TElemento), nameof(IUDtoAttribute.ClaseParaCreacion));
            var nuevoElemento = new NuevoElemento(idDivMostrar: Creador.Crud.Mnt.Id, idDivOcultar: Creador.Id, gestor: gestor == null ? "" : gestor);
            var opcion = new OpcionDeMenu<TElemento>(Menu, nuevoElemento, $"Crear");
            Menu.Add(opcion);
        }

        internal void AnadirOpcionDeCancelarNuevo()
        {
            var gestor = (string)Elemento.ValorDelAtributo(typeof(TElemento), nameof(IUDtoAttribute.ClaseParaCreacion));
            var cancelarNuevo = new CancelarNuevo(idDivMostrar: Creador.Crud.Mnt.Id, idDivOcultar: Creador.Id, gestor: gestor == null ? "" : gestor);
            var opcion = new OpcionDeMenu<TElemento>(Menu, cancelarNuevo, $"Cancelar");
            Menu.Add(opcion);
        }

        internal void AnadirOpcionDeEdicion()
        {
            throw new NotImplementedException();
        }
    }
}