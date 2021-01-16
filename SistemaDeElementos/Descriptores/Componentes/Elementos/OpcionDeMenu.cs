using System;
using System.Collections.Generic;
using GestorDeElementos;
using ModeloDeDto;
using ServicioDeDatos.Seguridad;
using ServicioDeDatos.Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{
    public enum TipoDeLlamada { Post, Get }

    public static class TipoDeAccionDeMnt
    {
        public const string CrearElemento = "crear-elemento";
        public const string EditarElemento = "editar-elemento";
        public const string EliminarElemento = "eliminar-elemento";
        public const string RelacionarElementos = "relacionar-elementos";
        public const string AbrirModalParaRelacionar = "abrir-modal-para-relacionar";
        public const string AbrirModalParaConsultarRelaciones = "abrir-modal-para-consultar-relaciones";
        public const string MostrarSoloSeleccionadas = "mostrar-solo-seleccionadas";
        public const string OcultarMostrarFiltro = "ocultar-mostrar-filtro";
        public const string OcultarMostrarBloque = "ocultar-mostrar-bloque";
        public const string FilaPulsada = "fila-pulsada";
    }

    public static class TipoDeAccionDeCreacion
    {
        public const string NuevoElemento = "nuevo-elemento";
        public const string CerrarCreacion = "cerrar-creacion";
    }
    public static class TipoDeAccionDeEdicion
    {
        public const string ModificarElemento = "modificar-elemento";
        public const string CancelarEdicion = "cancelar-edicion";
    }
    public static class TipoDeAccionDeRelacionar
    {
        public const string Relacionar = "nuevas-relaciones";
        public const string Cerrar = "cerrar-relacionar";
    }

    public static class TipoDeAccionDeConsulta
    {
        public const string Cerrar = "cerrar-consulta";
    }
    public class AccionDeMenu
    {

        public string TipoDeAccion { get; private set; }

        public AccionDeMenu(string tipoDeAccion)
        {
            TipoDeAccion = tipoDeAccion;
        }

        public virtual string RenderAccion()
        {
            return "";
        }
    }

    public enum GestorDeEventos { EventosModalDeConsultaDeRelaciones, EventosModalDeCrearRelaciones, EventosDelMantenimiento, EventosModalDeSeleccion }

    /**********************************************************/
    // Acciones de menú de para navegar
    // renderiza llamada Crud.EventosDelMantenimiento(...)
    /**********************************************************/
    public class AccionDeNavegarParaRelacionar : AccionDeMenu
    {
        public string TipoAccion { get; private set; }
        public string UrlDelCrudDeRelacion { get; private set; }
        public string RelacionarCon { get; private set; }
        public string PropiedadRestrictora { get; private set; }
        public string PropiedadQueRestringe { get; private set; }
        public string NavegarAlCrud { get; private set; }

        public AccionDeNavegarParaRelacionar(string urlDelCrud, string relacionarCon, string nombreDelMnt,string propiedadQueRestringe, string propiedadRestrictora)
        : base(TipoDeAccionDeMnt.RelacionarElementos)
        {
            TipoAccion = TipoDeAccionDeMnt.RelacionarElementos;
            RelacionarCon = relacionarCon.ToLower();
            PropiedadRestrictora = propiedadRestrictora.ToLower();
            PropiedadQueRestringe = propiedadQueRestringe.ToLower();
            UrlDelCrudDeRelacion = urlDelCrud;
            NavegarAlCrud = nombreDelMnt;
        }

        public override string RenderAccion()
        {
            return $"Crud.EventosDelMantenimiento('{TipoAccion}','IdOpcionDeMenu==idDeOpcMenu#{nameof(RelacionarCon)}=={RelacionarCon}#{nameof(PropiedadQueRestringe)}=={PropiedadQueRestringe}#{nameof(PropiedadRestrictora)}=={PropiedadRestrictora}')";
        }
    }

    /**********************************************************/
    // Acciones de menú de un mantenimiento
    // renderiza llamada Crud.EventosDelMantenimiento(...)
    /**********************************************************/
    public class AccionDeMenuMnt : AccionDeMenu
    {
        protected List<string> Parametros = new List<string>();

        public AccionDeMenuMnt(string tipoAccion)
        : base(tipoAccion)
        {
        }

        public override string RenderAccion()
        {
            var parametros = ""; 
            for(var i=0; i< Parametros.Count; i++)
                parametros = $"{parametros}{(i ==0 ? "": "#")}{Parametros[i]}";

            return $"Crud.EventosDelMantenimiento('{TipoDeAccion}'{(Parametros.Count == 0 ? "": $",'{parametros}'")})";
        }
    }

    public class CrearElemento : AccionDeMenuMnt
    {
        public CrearElemento()
        : base(TipoDeAccionDeMnt.CrearElemento)
        {
        }
    }

    public class BorrarElemento : AccionDeMenuMnt
    {
        public BorrarElemento()
        : base(TipoDeAccionDeMnt.EliminarElemento)
        {
        }
    }

    public class EditarElemento : AccionDeMenuMnt
    {
        public EditarElemento()
        : base(TipoDeAccionDeMnt.EditarElemento)
        {
        }
    }

    public class RelacionarElementos: AccionDeMenuMnt
    {
        public string IdHtmlDeLaModalAsociada {get; private set;}
        public Func<string> RenderDeLaModal { get; private set; }
        public RelacionarElementos(string idHtmlDeLaModalAsociada, Func<string> renderDeLaModal)
        : base(TipoDeAccionDeMnt.AbrirModalParaRelacionar)
        {
            IdHtmlDeLaModalAsociada = idHtmlDeLaModalAsociada;
            Parametros.Add(idHtmlDeLaModalAsociada);
            RenderDeLaModal = renderDeLaModal;
        }

        public override string RenderAccion()
        {
            return base.RenderAccion();
        }
    }

    public class ConsultarRelaciones : AccionDeMenuMnt
    {
        public string IdHtmlDeLaModalAsociada { get; private set; }
        public Func<string> RenderDeLaModal { get; private set; }
        public ConsultarRelaciones(string idHtmlDeLaModalAsociada, Func<string> renderDeLaModal)
        : base(TipoDeAccionDeMnt.AbrirModalParaConsultarRelaciones)
        {
            IdHtmlDeLaModalAsociada = idHtmlDeLaModalAsociada;
            Parametros.Add(idHtmlDeLaModalAsociada);
            RenderDeLaModal = renderDeLaModal;
        }

        public override string RenderAccion()
        {
            return base.RenderAccion();
        }
    }

    /**********************************************************/
    // Acciones de menú de la modal o vista de creación
    // renderiza llamada Crud.EjecutarMenuCrt(...)
    /**********************************************************/
    public class AccionDeMenuCreacion : AccionDeMenu
    {
        public AccionDeMenuCreacion(string tipoDeAccionDeCreacion)
            : base(tipoDeAccionDeCreacion)
        {
        }

        public override string RenderAccion()
        {
            return $"Crud.EjecutarMenuCrt('{TipoDeAccion}')";
        }
    }

    public class NuevoElemento : AccionDeMenuCreacion
    {
        public NuevoElemento()
        : base(TipoDeAccionDeCreacion.NuevoElemento)
        {
        }
    }

    public class CerrarCreacion : AccionDeMenuCreacion
    {
        public CerrarCreacion()
        : base(TipoDeAccionDeCreacion.CerrarCreacion)
        {
        }
    }


    /**********************************************************/
    // Acciones de menú de la modal o vista de creación
    // renderiza llamada Crud.EjecutarMenuEdt(...)
    /**********************************************************/
    public class AccionDeMenuEdicion : AccionDeMenu
    {
        public AccionDeMenuEdicion(string tipoDeAccionDeEdicion)
        : base(tipoDeAccionDeEdicion)
        {
        }

        public override string RenderAccion()
        {
            return $"Crud.EjecutarMenuEdt('{TipoDeAccion}')";
        }
    }

    public class ModificarElemento : AccionDeMenuEdicion
    {
        public ModificarElemento()
        : base(TipoDeAccionDeEdicion.ModificarElemento)
        {
        }
    }
    public class CancelarEdicion : AccionDeMenuEdicion
    {
        public CancelarEdicion()
        : base(TipoDeAccionDeEdicion.CancelarEdicion)
        {
        }
    }


    /**********************************************************/
    // Definir una opción dentro de un menú. 
    // - la opción define que acción que ha de realizar
    // - renderiza un boton que al pulsarlo ejecuta la opción
    /**********************************************************/
    public class OpcionDeMenu<TElemento> : ControlHtml where TElemento : ElementoDto
    {
        public Menu<TElemento> Menu => (Menu<TElemento>)Padre;
        public AccionDeMenu Accion { get; private set; }
        public TipoDeLlamada TipoDeLLamada { get; private set; } = TipoDeLlamada.Get;

        public enumModoDeAccesoDeDatos PermisosNecesarios { get; private set; }

        public enumCssOpcionMenu ClaseBoton { get; private set; }

        public OpcionDeMenu(Menu<TElemento> menu, AccionDeMenu accion, string titulo, enumModoDeAccesoDeDatos permisosNecesarios, enumCssOpcionMenu clase)
        : this(menu, accion, TipoDeLlamada.Get, titulo, permisosNecesarios,clase)
        {
        }

        public OpcionDeMenu(Menu<TElemento> menu, AccionDeMenu accion, TipoDeLlamada tipoAccion, string titulo, enumModoDeAccesoDeDatos permisosNecesarios, enumCssOpcionMenu clase)
        : base(
          padre: menu,
          id: $"{menu.Id}_{TipoControl.Opcion}_{menu.OpcionesDeMenu.Count}",
          etiqueta: titulo,
          propiedad: null,
          ayuda: null,
          posicion: null
        )
        {
            Tipo = TipoControl.Opcion;
            TipoDeLLamada = tipoAccion;
            Accion = accion;
            PermisosNecesarios = permisosNecesarios;
            ClaseBoton = clase;
        }

        public override string RenderControl()
        {
            var disbled = !Menu.ZonaMenu.Mnt.Crud.GestorDeUsuario.TienePermisoDeDatos(usuarioConectado: Menu.ZonaMenu.Mnt.Crud.UsuarioConectado
                                                                    , permisosNecesarios: PermisosNecesarios
                                                                    , elemento: Menu.ZonaMenu.Mnt.Crud.Negocio) 
                ? "disabled"
                : "";

            if (TipoDeLLamada == TipoDeLlamada.Post)
            {
                var htmlFormPost = $@"
                    <form id=¨{IdHtml}¨ action=¨{((AccionDeNavegarParaRelacionar)Accion).UrlDelCrudDeRelacion}¨ method=¨post¨ navegar-al-crud=¨{((AccionDeNavegarParaRelacionar)Accion).NavegarAlCrud}¨ restrictor=¨{IdHtml}-restrictor¨ orden=¨{IdHtml}-orden¨ style=¨display: inline-block;¨ >
                        <input id=¨{IdHtml}-restrictor¨ type=¨hidden¨ name =¨restrictor¨ >
                        <input id=¨{IdHtml}-orden¨ type=¨hidden¨ name = ¨orden¨ >
                        <input type=¨button¨ clase=¨{Css.Render(ClaseBoton)}¨ permisos-necesarios=¨{ModoDeAccesoDeDatos.Render(PermisosNecesarios)}¨ value=¨{Etiqueta}¨ onClick=¨{Accion.RenderAccion().Replace("idDeOpcMenu", IdHtml)}¨ {disbled} />
                    </form>
                ";
                return htmlFormPost;
            }

            var htmlOpcionMenu = $"<input id=¨{IdHtml}¨ type=¨button¨ clase=¨{Css.Render(ClaseBoton)}¨ permisos-necesarios=¨{ModoDeAccesoDeDatos.Render(PermisosNecesarios)}¨ value=¨{Etiqueta}¨ onClick=¨{Accion.RenderAccion()}¨ {disbled} />";
            return htmlOpcionMenu;
        }
    }
}
