using System;
using System.Collections.Generic;
using Gestor.Elementos.ModeloIu;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{
    public enum TipoAccion { IraMnt }

    public static class TipoAccionMnt
    {
        public const string CrearElemento = "crear-elemento";
        public const string EditarElemento = "editar-elemento";
        public const string EliminarElemento = "eliminar-elemento";
        public const string RelacionarElementos = "relacionar-elementos";
    }

    public static class TipoAccionCreacion
    {
        public const string NuevoElemento = "nuevo-elemento";
        public const string CerrarCreacion = "cerrar-creacion";
    }
    public static class TipoAccionEdicion
    {
        public const string ModificarElemento = "modificar-elemento";
        public const string CancelarEdicion = "cancelar-edicion";
    }

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
        string TipoAccion;
        public string UrlDelCrudDeRelacion { get; set; }
        public string RelacionarCon { get; set; }


        public AccionDeMenuMnt(string tipoAccion)
        : base()
        {
            TipoAccion = tipoAccion;
        }

        public override string RenderAccion()
        {

            if (TipoAccion == TipoAccionMnt.RelacionarElementos)
                return $"Crud.EventosDelMantenimiento('{TipoAccion}','RelacionarCon=={RelacionarCon}#Url=={UrlDelCrudDeRelacion}')";
            else
                return $"Crud.EventosDelMantenimiento('{TipoAccion}')";
        }
    }


    public class AccionDeMenuCreacion : AccionDeMenu
    {
        string TipoAccion;

        public AccionDeMenuCreacion(string tipoAccion)
            : base()
        {
            TipoAccion = tipoAccion;
        }

        public override string RenderAccion()
        {
            return $"Crud.EjecutarMenuCrt('{TipoAccion}')";
        }
    }


    public class AccionDeMenuEdicion : AccionDeMenu
    {
        string TipoAccion;
        public AccionDeMenuEdicion(string tipoAccion)
        : base()
        {
            TipoAccion = tipoAccion;
        }

        public override string RenderAccion()
        {
            return $"Crud.EjecutarMenuEdt('{TipoAccion}')";
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
        : base(TipoAccionMnt.EliminarElemento)
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

    public class CerrarCreacion : AccionDeMenuCreacion
    {
        public CerrarCreacion()
        : base(TipoAccionCreacion.CerrarCreacion)
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

    public class OpcionDeMenu<TElemento> : ControlHtml where TElemento : ElementoDto
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
