using System;
using System.Collections.Generic;

namespace MVCSistemaDeElementos.Descriptores
{
    public enum TipoAccion { IraCrear, IraMnt }

    public class AccionDeMenu
    {
        public TipoAccion TipoAccion { get; protected set; }

        public virtual string RenderAccion()
        {
            return "";
        }
    }

    public class AccionDeMenuMnt: AccionDeMenu
    {
        public override string RenderAccion()
        {
            return "Crud.MenuMnt.EjecutarAccionMenu({parametros})";
        }
    }

    public class AccionDeMenuCreacion: AccionDeMenu
    {
        public override string RenderAccion()
        {
            return "Crud.MenuCrt.EjecutarAccionMenu({parametros})";
        }
    }

    public class IrACrear : AccionDeMenuMnt
    {
        public string IdDivMostrar { get; set; }
        public string IdDivMostrarHtml => IdDivMostrar.ToLower();

        public string ClaseParaCreacion { get; private set; }

        public string IdDivOcultar { get; set; }
        public string IdDivOcultarHtml => IdDivOcultar.ToLower();

        public IrACrear(string idDivMostrar, string idDivOcultar, string claseParaCreacion)
        {
            TipoAccion = TipoAccion.IraCrear;
            IdDivMostrar = idDivMostrar;
            IdDivOcultar = idDivOcultar;
            ClaseParaCreacion = claseParaCreacion;
        }

        public override string RenderAccion()
        {
            var htmlAccion = base.RenderAccion();
            return htmlAccion.Replace("{parametros}", $"'{nameof(IrACrear).ToLower()}','{IdDivMostrarHtml}','{IdDivOcultarHtml}', new {ClaseParaCreacion}()");
        }
    }

    public class IrAMnt : AccionDeMenuCreacion
    {

        public string IdDivOcultar { get; set; }
        public string IdDivOcultarHtml => IdDivOcultar.ToLower();

        public IrAMnt(string idDivOcultar)
        {
            TipoAccion = TipoAccion.IraMnt;
            IdDivOcultar = idDivOcultar;
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
