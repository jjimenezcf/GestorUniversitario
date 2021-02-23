using Enumerados;
using GestorDeElementos;
using ModeloDeDto;
using ServicioDeDatos.Seguridad;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class ZonaDeMenu<TElemento> : ControlHtml where TElemento : ElementoDto
    {
        public DescriptorDeMantenimiento<TElemento> Mnt
        {
            get
            {
                if (Padre is DescriptorDeMantenimiento<TElemento>)
                    return (DescriptorDeMantenimiento<TElemento>)Padre;

                if (Padre is DescriptorDeCreacion<TElemento>)
                    return Creador.Mnt;

                return Editor.Mnt;
            }
        }

        public bool EsZonaDeMenuDeMantenimiento {get{ return  Padre is DescriptorDeMantenimiento<TElemento>;}
}

        public DescriptorDeCreacion<TElemento> Creador => (DescriptorDeCreacion<TElemento>)Padre;
        public DescriptorDeEdicion<TElemento> Editor => (DescriptorDeEdicion<TElemento>)Padre;

        public Menu<TElemento> Menu { get; set; }

        public ZonaDeMenu(DescriptorDeMantenimiento<TElemento> mnt)
        : base(
          padre: mnt,
          id: $"{mnt.Id}_{enumTipoControl.ZonaDeMenu.Render()}",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Menu = new Menu<TElemento>(this);
            Tipo = enumTipoControl.ZonaDeMenu;
        }

        public ZonaDeMenu(DescriptorDeCreacion<TElemento> creador)
        : base(
          padre: creador,
          id: $"{creador.Id}_{enumTipoControl.ZonaDeMenu.Render()}",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Menu = new Menu<TElemento>(this);
            Tipo = enumTipoControl.ZonaDeMenu;
        }

        public ZonaDeMenu(DescriptorDeEdicion<TElemento> editor)
        : base(
          padre: editor,
          id: $"{editor.Id}_{enumTipoControl.ZonaDeMenu.Render()}",
          etiqueta: null,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Menu = new Menu<TElemento>(this);
            Tipo = enumTipoControl.ZonaDeMenu;
        }


        public override string RenderControl()
        {
            var htmContenedorMnt =
                $@"
                   <div id=¨{IdHtml}¨>
                     {Menu.RenderControl()}
                   </div>
                ";

            return htmContenedorMnt.Render();
        }

        #region Opciones de mantenimiento
        internal void AnadirOpcionDeIrACrear()
        {
            var crearElemento = new CrearElemento();
            var opcion = new OpcionDeMenu<TElemento>(Menu, crearElemento, $"Nuevo", enumModoDeAccesoDeDatos.Gestor);
            Menu.Add(opcion);
        }
        internal void AnadirOpcionDeIrAEditarFilasSeleccionadas()
        {
            var editarElemento = new EditarElemento();
            var opcion = new OpcionDeMenu<TElemento>(Menu, editarElemento, $"Editar", enumModoDeAccesoDeDatos.Consultor);
            Menu.Add(opcion);
        }
        #endregion


        #region Opciones de creacion
        internal void AnadirOpcionDeNuevoElemento()
        {
            var nuevoElemento = new NuevoElemento();
            var opcion = new OpcionDeMenu<TElemento>(Menu, nuevoElemento, $"Crear", enumModoDeAccesoDeDatos.Gestor);
            Menu.Add(opcion);
        }

        internal void AnadirOpcionDeCerrarCreacion()
        {
            var cerrarCreacion = new CerrarCreacion();
            var opcion = new OpcionDeMenu<TElemento>(Menu, cerrarCreacion, $"Cerrar", enumModoDeAccesoDeDatos.Consultor);
            Menu.Add(opcion);
        }
        #endregion

        #region opciones de edición
        internal void AnadirOpcionDeModificarElemento()
        {
            var modificarElemento = new ModificarElemento();
            var opcion = new OpcionDeMenu<TElemento>(Menu, modificarElemento, $"Modificar", enumModoDeAccesoDeDatos.Gestor);
            Menu.Add(opcion);
        }
        internal void AnadirOpcionDeCancelarEdicion()
        {
            var cancelarEdicion = new CancelarEdicion();
            var opcion = new OpcionDeMenu<TElemento>(Menu, cancelarEdicion, $"Cancelar", enumModoDeAccesoDeDatos.Consultor);
            Menu.Add(opcion);
        }
        #endregion


        internal void AnadirOpcionDeBorrarElemento()
        {
            var BorrarElemento = new BorrarElemento();
            var opcion = new OpcionDeMenu<TElemento>(Menu, BorrarElemento, $"Borrar", enumModoDeAccesoDeDatos.Gestor);
            Menu.Add(opcion);
        }

        internal void QuitarOpcionDeMenu(string tipoDeAccion)
        {
            foreach (var opcion in Menu.OpcionesDeMenu)
            {
                if (opcion.Accion.TipoDeAccion == tipoDeAccion)
                {
                    Menu.OpcionesDeMenu.Remove(opcion);
                    break;
                }
            }
        }
    }
}