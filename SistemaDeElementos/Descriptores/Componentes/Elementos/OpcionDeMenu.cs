using System;
using System.Collections.Generic;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{
    public enum TipoAccion { IraMnt }

    public enum TipoAccionMnt {CrearElemento, EditarElemento, BorrarElemento }
    public enum TipoAccionCreacion {NuevoElemento, CancelarNuevo }
    public enum TipoAccionEdicion {ModificarElemento,CancelarEdicion }

    public class AccionDeMenu
    {
        public AccionDeMenu()
        {
        }

        public virtual string RenderAccion()
        {
            return "";
        }
    }

    public class AccionDeMenuMnt : AccionDeMenu
    {
        TipoAccionMnt TipoAccion;


        public AccionDeMenuMnt(TipoAccionMnt tipoAccion)
        : base()
        {
            TipoAccion = tipoAccion;
        }

        public override string RenderAccion()
        {
            return $"Crud.EjecutarMenuMnt('{TipoAccion.ToString().ToLower()}')";
        }
    }

    public class AccionDeMenuCreacion : AccionDeMenu
    {
        TipoAccionCreacion TipoAccion;

        public AccionDeMenuCreacion(TipoAccionCreacion tipoAccion)
            : base()
        {
            TipoAccion = tipoAccion;
        }

        public override string RenderAccion()
        {
           return $"Crud.EjecutarMenuCrt('{TipoAccion.ToString().ToLower()}')";
        }
    }


    public class AccionDeMenuEdicion : AccionDeMenu
    {
        TipoAccionEdicion TipoAccion;
        public AccionDeMenuEdicion(TipoAccionEdicion tipoAccion)
        : base()
        {
            TipoAccion = tipoAccion;
        }

        public override string RenderAccion()
        {
            return $"Crud.EjecutarMenuEdt('{TipoAccion.ToString().ToLower()}')";
        }
    }

    public class CrearElemento : AccionDeMenuMnt
    {
        public CrearElemento()
        : base(TipoAccionMnt.CrearElemento)
        {
        }
    }

    public class BorrarElemento : AccionDeMenuMnt
    {
        public BorrarElemento()
        : base(TipoAccionMnt.BorrarElemento)
        {
        }
    }

    public class EditarElemento : AccionDeMenuMnt
    {
        public EditarElemento()
        : base(TipoAccionMnt.EditarElemento)
        {
        }
    }

    public class NuevoElemento : AccionDeMenuCreacion
    {
        public NuevoElemento()
        : base(TipoAccionCreacion.NuevoElemento)
        {
        }
    }

    public class CancelarNuevo : AccionDeMenuCreacion
    {
        public CancelarNuevo()
        : base(TipoAccionCreacion.CancelarNuevo)
        {
        }
    }

    public class ModificarElemento : AccionDeMenuEdicion
    {
        public ModificarElemento()
        : base(TipoAccionEdicion.ModificarElemento)
        {
        }
    }
    public class CancelarEdicion : AccionDeMenuEdicion
    {
        public CancelarEdicion()
        : base(TipoAccionEdicion.CancelarEdicion)
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
