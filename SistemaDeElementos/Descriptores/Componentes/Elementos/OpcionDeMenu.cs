using System;
using System.Collections.Generic;

namespace MVCSistemaDeElementos.Descriptores
{
    public enum TipoAccion { MostrarDiv, OcultarDiv }
    public class AccionDeMenu
    {
        public TipoAccion tipoAccion { get; protected set; }

        public virtual string RenderAccion()
        {
            return "EjecutarAccionMenu({parametros})";
        }

    }

    public class MostrarDiv : AccionDeMenu
    {

        public string IdDivMostrar { get; set; }
        public string idDivMostrarHtml => IdDivMostrar.ToLower();

        public string IdDivOcultar { get; set; }
        public string idDivOcultarHtml => IdDivOcultar.ToLower();

        public MostrarDiv(string idDivMostrar, string idDivOcultar)
        {
            tipoAccion = TipoAccion.MostrarDiv;
            this.IdDivMostrar = idDivMostrar;
            this.IdDivOcultar = idDivOcultar;
        }

        public override string RenderAccion()
        {
            var htmlAccion = base.RenderAccion();
            return htmlAccion.Replace("{parametros}", $"'{idDivMostrarHtml}','{idDivOcultarHtml}'");
        }


    }

    public class OcultarDiv : AccionDeMenu
    {

        public string IdDivOcultar { get; set; }
        public string IdDivOcultarHtml => IdDivOcultar.ToLower();

        public OcultarDiv(string idDivOcultar)
        {
            tipoAccion = TipoAccion.OcultarDiv;
            this.IdDivOcultar = idDivOcultar;
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
            var htmlOpcionMenu = $"<input id=¨{IdHtml}¨ type=¨button¨ value=¨{Etiqueta}¨ onClick=¨{Accion.RenderAccion()}¨>";
            return htmlOpcionMenu;
        }
    }
}
