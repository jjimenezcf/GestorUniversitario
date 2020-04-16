﻿using System;
using Gestor.Elementos.ModeloIu;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class BarraDeMenu<TElemento> : ControlHtml
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

        internal void AnadirOpcionDeCreacion()
        {
            var gestor = (string)Elemento.ValorDelAtributo(typeof(TElemento), nameof(IUDtoAttribute.ClaseTypeScriptDeCreacion));
            var crearElemento = new CrearElemento(idDivMostrar: Mnt.Crud.Creador.Id, idDivOcultar: Mnt.Id, gestor: gestor == null ? "" : gestor);
            var opcion = new OpcionDeMenu<TElemento>(Menu, crearElemento, $"Nuevo {Mnt.Crud.NombreElemento}");
            Menu.Add(opcion);
        }
        internal void AnadirOpcionDeEditarElemento()
        {
            var gestor = (string)Elemento.ValorDelAtributo(typeof(TElemento), nameof(IUDtoAttribute.ClaseTypeScriptDeEdicion));
            var editarElemento = new EditarElemento(idDivMostrar: Mnt.Crud.Editor.Id, idDivOcultar: Mnt.Id, gestor: gestor == null ? "" : gestor);
            var opcion = new OpcionDeMenu<TElemento>(Menu, editarElemento, $"Editar {Mnt.Crud.NombreElemento}");
            Menu.Add(opcion);
        }

        internal void AnadirOpcionDeNuevoElemento()
        {
            var gestor = (string)Elemento.ValorDelAtributo(typeof(TElemento), nameof(IUDtoAttribute.ClaseTypeScriptDeCreacion));
            var nuevoElemento = new NuevoElemento(idDivMostrar: Creador.Crud.Mnt.Id, idDivOcultar: Creador.Id, gestor: gestor == null ? "" : gestor);
            var opcion = new OpcionDeMenu<TElemento>(Menu, nuevoElemento, $"Crear");
            Menu.Add(opcion);
        }

        internal void AnadirOpcionDeCancelarNuevo()
        {
            var gestor = (string)Elemento.ValorDelAtributo(typeof(TElemento), nameof(IUDtoAttribute.ClaseTypeScriptDeCreacion));
            var cancelarNuevo = new CancelarNuevo(idDivMostrar: Creador.Crud.Mnt.Id, idDivOcultar: Creador.Id, gestor: gestor == null ? "" : gestor);
            var opcion = new OpcionDeMenu<TElemento>(Menu, cancelarNuevo, $"Cancelar");
            Menu.Add(opcion);
        }

        internal void AnadirOpcionDeModificarElemento()
        {
            var gestor = (string)Elemento.ValorDelAtributo(typeof(TElemento), nameof(IUDtoAttribute.ClaseTypeScriptDeEdicion));
            var modificarElemento = new ModificarElemento(idDivMostrar: Editor.Crud.Mnt.Id, idDivOcultar: Editor.Id, gestor: gestor == null ? "" : gestor);
            var opcion = new OpcionDeMenu<TElemento>(Menu, modificarElemento, $"Modificar");
            Menu.Add(opcion);
        }
        internal void AnadirOpcionDeCancelarEdicion()
        {
            var gestor = (string)Elemento.ValorDelAtributo(typeof(TElemento), nameof(IUDtoAttribute.ClaseTypeScriptDeCreacion));
            var cancelarEdicion = new CancelarEdicion(idDivMostrar: Editor.Crud.Mnt.Id, idDivOcultar: Editor.Id, gestor: gestor == null ? "" : gestor);
            var opcion = new OpcionDeMenu<TElemento>(Menu, cancelarEdicion, $"Cancelar");
            Menu.Add(opcion);
        }

    }
}