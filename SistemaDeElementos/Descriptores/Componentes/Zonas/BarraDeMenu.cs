using System;
using Gestor.Elementos.ModeloIu;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class BarraDeMenu<TElemento> : ControlHtml where TElemento : Elemento
    {
        public DescriptorMantenimiento<TElemento> Mnt => (DescriptorMantenimiento<TElemento>)Padre;
        public DescriptorDeCreacion<TElemento> Creador => (DescriptorDeCreacion<TElemento>)Padre;
        public DescriptorDeEdicion<TElemento> Editor => (DescriptorDeEdicion<TElemento>)Padre;

        public Menu<TElemento> Menu { get; set; }

        public BarraDeMenu(DescriptorMantenimiento<TElemento> mnt)
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

        public BarraDeMenu(DescriptorDeCreacion<TElemento> creador)
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

        public BarraDeMenu(DescriptorDeEdicion<TElemento> editor)
        : base(
          padre: editor,
          id: $"{editor.Id}_{TipoControl.ZonaMenu}",
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

        #region Opciones de mantenimiento
        internal void AnadirOpcionDeCreacion()
        {
            var crearElemento = new CrearElemento();
            var opcion = new OpcionDeMenu<TElemento>(Menu, crearElemento, $"Nuevo {Mnt.Crud.NombreElemento}");
            Menu.Add(opcion);
        }
        internal void AnadirOpcionDeEditarElemento()
        {
            var editarElemento = new EditarElemento();
            var opcion = new OpcionDeMenu<TElemento>(Menu, editarElemento, $"Editar {Mnt.Crud.NombreElemento}");
            Menu.Add(opcion);
        }
        #endregion


        #region Opciones de creacion
        internal void AnadirOpcionDeNuevoElemento()
        {
            var nuevoElemento = new NuevoElemento();
            var opcion = new OpcionDeMenu<TElemento>(Menu, nuevoElemento, $"Crear");
            Menu.Add(opcion);
        }

        internal void AnadirOpcionDeBorrarElemento()
        {
            var BorrarElemento = new BorrarElemento();
            var opcion = new OpcionDeMenu<TElemento>(Menu, BorrarElemento, $"Borrar {Mnt.Crud.NombreElemento}");
            Menu.Add(opcion);
        }

        internal void AnadirOpcionDeCancelarNuevo()
        {
            var cancelarNuevo = new CancelarNuevo();
            var opcion = new OpcionDeMenu<TElemento>(Menu, cancelarNuevo, $"Cancelar");
            Menu.Add(opcion);
        }
        #endregion

        #region opciones de edición
        internal void AnadirOpcionDeModificarElemento()
        {
            var modificarElemento = new ModificarElemento();
            var opcion = new OpcionDeMenu<TElemento>(Menu, modificarElemento, $"Modificar");
            Menu.Add(opcion);
        }
        internal void AnadirOpcionDeCancelarEdicion()
        {
            var cancelarEdicion = new CancelarEdicion();
            var opcion = new OpcionDeMenu<TElemento>(Menu, cancelarEdicion, $"Cancelar");
            Menu.Add(opcion);
        }
        #endregion
    }
}