using System;
using System.Collections.Generic;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{
    public enum TipoAccion { IraCrear, IraMnt, NuevoElemento, CancelarNuevo }

    public class AccionDeMenu
    {
        private string gestor1;

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
                return $"Crud.Menu.EjecutarAccionMenu('{TipoAccion.ToString().ToLower()}','{IdDivMostrarHtml}','{IdDivOcultarHtml}', new {Gestor}())";
            }
            return "";
        }
    }

    public class AccionDeMenuMnt: AccionDeMenu
    {
        public AccionDeMenuMnt(TipoAccion tipoAccion, string gestor)
        :base(tipoAccion, gestor)
        {

        }

        public AccionDeMenuMnt(TipoAccion tipoAccion, string gestor, string idDivMostrar, string idDivOcultar)
        : base(tipoAccion, gestor, idDivMostrar, idDivOcultar)
        {

        }

        public override string RenderAccion()
        {
            var renderAccion = base.RenderAccion();


            if (renderAccion.IsNullOrEmpty())
            {
                renderAccion = "Crud.Menu.EjecutarAccionMenu({parametros})";
            }

            return renderAccion;
        }
    }

    public class AccionDeMenuCreacion: AccionDeMenu
    {
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
            var renderAccion = base.RenderAccion();


            if (renderAccion.IsNullOrEmpty())
            {
                renderAccion = "Crud.Menu.EjecutarAccionMenu({parametros})";
            }

            return renderAccion;
        }
    }

    public class IrACrear : AccionDeMenuMnt
    {
        public IrACrear(string idDivMostrar, string idDivOcultar, string gestor)
        : base(TipoAccion.IraCrear, gestor,idDivMostrar,idDivOcultar)
        {
        }

        //public override string RenderAccion()
        //{
        //    var htmlAccion = base.RenderAccion();
        //    return htmlAccion.Replace("{parametros}", $"'{nameof(IrACrear).ToLower()}','{IdDivMostrarHtml}','{IdDivOcultarHtml}', new {Gestor}()");
        //}
    }

    public class NuevoElemento: AccionDeMenuCreacion
    {
        public NuevoElemento(string idDivMostrar, string idDivOcultar, string gestor)
        : base(TipoAccion.NuevoElemento, gestor, idDivMostrar, idDivOcultar)
        {
        }

        //public override string RenderAccion()
        //{
        //    var htmlAccion = base.RenderAccion();
        //    return htmlAccion.Replace("{parametros}", $"'{nameof(NuevoElemento).ToLower()}','{IdDivMostrarHtml}','{IdDivOcultarHtml}', new {Gestor}()");
        //}

    }

    public class CancelarNuevo : AccionDeMenuCreacion
    {
        public CancelarNuevo(string idDivMostrar, string idDivOcultar, string gestor)
        : base(TipoAccion.CancelarNuevo, gestor, idDivMostrar, idDivOcultar)
        {
        }

        //public override string RenderAccion()
        //{
        //    var htmlAccion = base.RenderAccion();
        //    return htmlAccion.Replace("{parametros}", $"'{nameof(CancelarNuevo).ToLower()}','{IdDivMostrarHtml}','{IdDivOcultarHtml}', new {Gestor}()");
        //}
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
