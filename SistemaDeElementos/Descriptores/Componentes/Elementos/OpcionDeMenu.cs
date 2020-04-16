using System;
using System.Collections.Generic;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{
    public enum TipoAccion { CrearElemento, IraMnt, NuevoElemento, CancelarNuevo, ModificarElemento, EditarElemento, CancelarEdicion }

    public class AccionDeMenu
    {
        public TipoAccion TipoAccion { get; private set; }
        public string Gestor { get; private set; }

        public string IdDivMostrar { get; set; }
        public string IdDivMostrarHtml => IdDivMostrar.ToLower();

        public string IdDivOcultar { get; set; }
        public string IdDivOcultarHtml => IdDivOcultar.ToLower();

        public AccionDeMenu(TipoAccion tipoAccion, string gestor)
        {
            TipoAccion = tipoAccion;
            Gestor = gestor;
        }

        public AccionDeMenu(TipoAccion tipoAccion, string gestor, string idDivMostrar, string idDivOcultar)
        : this(tipoAccion, gestor)
        {
            IdDivOcultar = idDivOcultar;
            IdDivMostrar = idDivMostrar;
        }

        public virtual string RenderAccion()
        {
            if (!IdDivMostrar.IsNullOrEmpty() && !IdDivOcultar.IsNullOrEmpty())
            {
                return $"Crud.Menu.EjecutarAccionMenu('{TipoAccion.ToString().ToLower()}','{IdDivMostrarHtml}','{IdDivOcultarHtml}', new {Gestor}('{IdDivMostrarHtml}'))";
            }
            return "";
        }
    }

    public class AccionDeMenuMnt : AccionDeMenu
    {

        protected string IdPanelMnt => IdDivOcultarHtml;
        protected string IdPanelEdicionCreacion => IdDivMostrarHtml;

        public AccionDeMenuMnt(TipoAccion tipoAccion, string gestor)
        : base(tipoAccion, gestor)
        {

        }

        public AccionDeMenuMnt(TipoAccion tipoAccion, string gestor, string idDivMostrar, string idDivOcultar)
        : base(tipoAccion, gestor, idDivMostrar, idDivOcultar)
        {

        }
        public override string RenderAccion()
        {
            return $"Crud.Menu.EjecutarAccionMenu('{TipoAccion.ToString().ToLower()}','{IdPanelEdicionCreacion}','{IdPanelMnt}', new {Gestor}('{IdPanelMnt}','{IdPanelEdicionCreacion}'))";
        }
    }

    public class AccionDeMenuCreacion : AccionDeMenu
    {
        protected string IdPanelMnt => IdDivMostrarHtml;
        protected string IdPanelCreacion => IdDivOcultarHtml;

        public AccionDeMenuCreacion(TipoAccion tipoAccion, string gestor)
            : base(tipoAccion, gestor)
        {

        }

        public AccionDeMenuCreacion(TipoAccion tipoAccion, string gestor, string idDivMostrar, string idDivOcultar)
        : base(tipoAccion, gestor, idDivMostrar, idDivOcultar)
        {

        }

        public override string RenderAccion()
        {
           return $"Crud.Menu.EjecutarAccionMenu('{TipoAccion.ToString().ToLower()}','{IdPanelMnt}','{IdPanelCreacion}', new {Gestor}('{IdPanelMnt}', '{IdPanelCreacion}'))";
        }
    }


    public class AccionDeMenuEdicion : AccionDeMenu
    {
        protected string IdPanelMnt => IdDivMostrarHtml;
        protected string IdPanelEdicion => IdDivOcultarHtml;
        public AccionDeMenuEdicion(TipoAccion tipoAccion, string gestor)
            : base(tipoAccion, gestor)
        {

        }

        public AccionDeMenuEdicion(TipoAccion tipoAccion, string gestor, string idDivMostrar, string idDivOcultar)
        : base(tipoAccion, gestor, idDivMostrar, idDivOcultar)
        {

        }

        public override string RenderAccion()
        {
            return $"Crud.Menu.EjecutarAccionMenu('{TipoAccion.ToString().ToLower()}','{IdPanelMnt}','{IdPanelEdicion}', new {Gestor}('{IdPanelMnt}', '{IdPanelEdicion}'))";
        }
    }

    public class CrearElemento : AccionDeMenuMnt
    {
        public CrearElemento(string idDivMostrar, string idDivOcultar, string gestor)
        : base(TipoAccion.CrearElemento, gestor, idDivMostrar, idDivOcultar)
        {
        }
    }
    public class EditarElemento : AccionDeMenuMnt
    {
        public EditarElemento(string idDivMostrar, string idDivOcultar, string gestor)
        : base(TipoAccion.EditarElemento, gestor, idDivMostrar, idDivOcultar)
        {
        }
    }

    public class NuevoElemento : AccionDeMenuCreacion
    {
        public NuevoElemento(string idDivMostrar, string idDivOcultar, string gestor)
        : base(TipoAccion.NuevoElemento, gestor, idDivMostrar, idDivOcultar)
        {
        }
    }

    public class CancelarNuevo : AccionDeMenuCreacion
    {
        public CancelarNuevo(string idDivMostrar, string idDivOcultar, string gestor)
        : base(TipoAccion.CancelarNuevo, gestor, idDivMostrar, idDivOcultar)
        {
        }
    }

    public class ModificarElemento : AccionDeMenuEdicion
    {
        public ModificarElemento(string idDivMostrar, string idDivOcultar, string gestor)
        : base(TipoAccion.ModificarElemento, gestor, idDivMostrar, idDivOcultar)
        {
        }
    }
    public class CancelarEdicion : AccionDeMenuEdicion
    {
        public CancelarEdicion(string idDivMostrar, string idDivOcultar, string gestor)
        : base(TipoAccion.CancelarEdicion, gestor, idDivMostrar, idDivOcultar)
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
