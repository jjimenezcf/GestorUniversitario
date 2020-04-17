using System;
using System.Collections.Generic;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{
    public enum TipoAccion { IraMnt }

    public enum TipoAccionMnt {CrearElemento, EditarElemento }
    public enum TipoAccionCreacion {NuevoElemento, CancelarNuevo }
    public enum TipoAccionEdicion {ModificarElemento,CancelarEdicion }

    public class AccionDeMenu
    {
        public string Gestor { get; private set; }

        public string IdDivMostrar { get; set; }
        public string IdDivMostrarHtml => IdDivMostrar.ToLower();

        public string IdDivOcultar { get; set; }
        public string IdDivOcultarHtml => IdDivOcultar.ToLower();

        public AccionDeMenu(string gestor)
        {
            Gestor = gestor;
        }

        public AccionDeMenu(string gestor, string idDivMostrar, string idDivOcultar)
        : this(gestor)
        {
            IdDivOcultar = idDivOcultar;
            IdDivMostrar = idDivMostrar;
        }

        public virtual string RenderAccion()
        {
            return "";
        }
    }

    public class AccionDeMenuMnt : AccionDeMenu
    {
        TipoAccionMnt TipoAccion;

        protected string IdPanelMnt => IdDivOcultarHtml;
        protected string IdPanelEdicionCreacion => IdDivMostrarHtml;

        public AccionDeMenuMnt(TipoAccionMnt tipoAccion, string gestor)
        : base(gestor)
        {
            TipoAccion = tipoAccion;
        }

        public AccionDeMenuMnt(TipoAccionMnt tipoAccion, string gestor, string idDivMostrar, string idDivOcultar)
        : base(gestor, idDivMostrar, idDivOcultar)
        {
            TipoAccion = tipoAccion;
        }
        public override string RenderAccion()
        {
            return $"Crud.Mnt.EjecutarAccionMenu('{TipoAccion.ToString().ToLower()}','{IdPanelEdicionCreacion}','{IdPanelMnt}', new {Gestor}('{IdPanelMnt}','{IdPanelEdicionCreacion}'))";
        }
    }

    public class AccionDeMenuCreacion : AccionDeMenu
    {
        TipoAccionCreacion TipoAccion;
        protected string IdPanelMnt => IdDivMostrarHtml;
        protected string IdPanelCreacion => IdDivOcultarHtml;

        public AccionDeMenuCreacion(TipoAccionCreacion tipoAccion, string gestor)
            : base(gestor)
        {
            TipoAccion = tipoAccion;
        }

        public AccionDeMenuCreacion(TipoAccionCreacion tipoAccion, string gestor, string idDivMostrar, string idDivOcultar)
        : base(gestor, idDivMostrar, idDivOcultar)
        {
            TipoAccion = tipoAccion;
        }

        public override string RenderAccion()
        {
           return $"Crud.Creacion.EjecutarAccionMenu('{TipoAccion.ToString().ToLower()}','{IdPanelMnt}','{IdPanelCreacion}', new {Gestor}('{IdPanelMnt}', '{IdPanelCreacion}'))";
        }
    }


    public class AccionDeMenuEdicion : AccionDeMenu
    {
        TipoAccionEdicion TipoAccion;
        protected string IdPanelMnt => IdDivMostrarHtml;
        protected string IdPanelEdicion => IdDivOcultarHtml;
        public AccionDeMenuEdicion(TipoAccionEdicion tipoAccion, string gestor)
            : base(gestor)
        {
            TipoAccion = tipoAccion;
        }

        public AccionDeMenuEdicion(TipoAccionEdicion tipoAccion, string gestor, string idDivMostrar, string idDivOcultar)
        : base(gestor, idDivMostrar, idDivOcultar)
        {
            TipoAccion = tipoAccion;
        }

        public override string RenderAccion()
        {
            return $"Crud.Edicion.EjecutarAccionMenu('{TipoAccion.ToString().ToLower()}','{IdPanelMnt}','{IdPanelEdicion}', new {Gestor}('{IdPanelMnt}', '{IdPanelEdicion}'))";
        }
    }

    public class CrearElemento : AccionDeMenuMnt
    {
        public CrearElemento(string idDivMostrar, string idDivOcultar, string gestor)
        : base(TipoAccionMnt.CrearElemento, gestor, idDivMostrar, idDivOcultar)
        {
        }
    }
    public class EditarElemento : AccionDeMenuMnt
    {
        public EditarElemento(string idDivMostrar, string idDivOcultar, string gestor)
        : base(TipoAccionMnt.EditarElemento, gestor, idDivMostrar, idDivOcultar)
        {
        }
    }

    public class NuevoElemento : AccionDeMenuCreacion
    {
        public NuevoElemento(string idDivMostrar, string idDivOcultar, string gestor)
        : base(TipoAccionCreacion.NuevoElemento, gestor, idDivMostrar, idDivOcultar)
        {
        }
    }

    public class CancelarNuevo : AccionDeMenuCreacion
    {
        public CancelarNuevo(string idDivMostrar, string idDivOcultar, string gestor)
        : base(TipoAccionCreacion.CancelarNuevo, gestor, idDivMostrar, idDivOcultar)
        {
        }
    }

    public class ModificarElemento : AccionDeMenuEdicion
    {
        public ModificarElemento(string idDivMostrar, string idDivOcultar, string gestor)
        : base(TipoAccionEdicion.ModificarElemento, gestor, idDivMostrar, idDivOcultar)
        {
        }
    }
    public class CancelarEdicion : AccionDeMenuEdicion
    {
        public CancelarEdicion(string idDivMostrar, string idDivOcultar, string gestor)
        : base(TipoAccionEdicion.CancelarEdicion, gestor, idDivMostrar, idDivOcultar)
        {
        }
    }

    public class OpcionDeMenu<TElemento> : ControlHtml
    {
        public Menu<TElemento> Menu => (Menu<TElemento>)Padre;
        public AccionDeMenu Accion { get; private set; }

        public OpcionDeMenu(Menu<TElemento> menu, AccionDeMenu accion, string titulo)
        : base(
          padre: menu,
          id: $"{menu.Id}_{TipoControl.Opcion}_{menu.OpcioneDeMenu.Count}",
          etiqueta: titulo,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.Opcion;
            Accion = accion;
        }

        public override string RenderControl()
        {
            var htmlOpcionMenu = $"<input id=¨{IdHtml}¨ type=¨button¨ value=¨{Etiqueta}¨ onClick=¨{Accion.RenderAccion()}¨ />";
            return htmlOpcionMenu;
        }
    }
}
