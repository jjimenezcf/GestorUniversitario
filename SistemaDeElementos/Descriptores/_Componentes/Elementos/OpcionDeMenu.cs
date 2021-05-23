using System;
using System.Collections.Generic;
using Enumerados;
using GestorDeElementos;
using ModeloDeDto;
using ServicioDeDatos.Seguridad;
using ServicioDeDatos.Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{
    public class AccionDeMenu
    {

        public string TipoDeAccion { get; private set; }
        public enumCssOpcionMenu ClaseDeAccion { get; private set; }

        public string Ayuda { get; private set; }

        public AccionDeMenu(string tipoDeAccion, enumCssOpcionMenu claseDeAccion, string ayuda)
        {
            TipoDeAccion = tipoDeAccion;
            ClaseDeAccion = claseDeAccion;
            Ayuda = ayuda;
        }

        public virtual string RenderAccion()
        {
            return "";
        }
    }

    /**********************************************************/
    // Acciones de menú de para navegar
    // renderiza llamada Crud.EventosDelMantenimiento(...)
    /**********************************************************/
    public class AccionDeRelacionarElemenetos : AccionDeMenu
    {
        public string TipoAccion { get; private set; }
        public string UrlDelCrudDeRelacion { get; private set; }
        public string RelacionarCon { get; private set; }
        public string PropiedadRestrictora { get; private set; }
        public string PropiedadQueRestringe { get; private set; }
        public string NavegarAlCrud { get; private set; }

        public AccionDeRelacionarElemenetos(string urlDelCrud, string relacionarCon, string nombreDelMnt, string propiedadQueRestringe, string propiedadRestrictora, string ayuda)
        : base(TipoDeAccionDeMnt.RelacionarElementos, enumCssOpcionMenu.DeElemento, ayuda)
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

    public class AccionDeGetionarDatosDependientes : AccionDeMenu
    {
        public string TipoAccion { get; private set; }
        public string UrlDelCrudDeDependientes { get; private set; }
        public string DatosDependientes { get; private set; }
        public string PropiedadRestrictora { get; private set; }
        public string PropiedadQueRestringe { get; private set; }
        public string NavegarAlCrud { get; private set; }

        public AccionDeGetionarDatosDependientes(string urlDelCrud, string datosDependientes, string nombreDelMnt, string propiedadQueRestringe, string propiedadRestrictora, string ayuda)
        : base(TipoDeAccionDeMnt.GestionarDependencias, enumCssOpcionMenu.DeElemento, ayuda)
        {
            TipoAccion = TipoDeAccionDeMnt.GestionarDependencias;
            DatosDependientes = datosDependientes.ToLower();
            PropiedadRestrictora = propiedadRestrictora.ToLower();
            PropiedadQueRestringe = propiedadQueRestringe.ToLower();
            UrlDelCrudDeDependientes = urlDelCrud;
            NavegarAlCrud = nombreDelMnt;
        }

        public override string RenderAccion()
        {
            return $"Crud.EventosDelMantenimiento('{TipoAccion}','IdOpcionDeMenu==idDeOpcMenu#{nameof(DatosDependientes)}=={DatosDependientes}#{nameof(PropiedadQueRestringe)}=={PropiedadQueRestringe}#{nameof(PropiedadRestrictora)}=={PropiedadRestrictora}')";
        }

    }

    /**********************************************************/
    // Acciones de menú de un mantenimiento
    // renderiza llamada Crud.EventosDelMantenimiento(...)
    /**********************************************************/
    public class AccionDeMenuMnt : AccionDeMenu
    {
        protected List<string> Parametros = new List<string>();
        public bool PermiteMultiSeleccion { get; set; } = false;
        public AccionDeMenuMnt(string tipoAccion, enumCssOpcionMenu claseDeAccion, string ayuda)
        : base(tipoAccion, claseDeAccion, ayuda)
        {
        }

        public override string RenderAccion()
        {
            var parametros = "";
            for (var i = 0; i < Parametros.Count; i++)
                parametros = $"{parametros}{(i == 0 ? "" : "#")}{Parametros[i]}";

            return $"Crud.EventosDelMantenimiento('{TipoDeAccion}'{(Parametros.Count == 0 ? "" : $",'{parametros}'")})";
        }
    }

    public class CrearElemento : AccionDeMenuMnt
    {
        public CrearElemento()
        : base(TipoDeAccionDeMnt.CrearElemento, enumCssOpcionMenu.DeVista, "Crear nuevo elemento")
        {
        }
    }

    public class BorrarElemento : AccionDeMenuMnt
    {
        public BorrarElemento()
        : base(TipoDeAccionDeMnt.EliminarElemento, enumCssOpcionMenu.DeElemento, "Borrar elemento")
        {
            PermiteMultiSeleccion = false;
        }
    }

    public class EditarElemento : AccionDeMenuMnt
    {
        public EditarElemento()
        : base(TipoDeAccionDeMnt.EditarElemento, enumCssOpcionMenu.DeElemento, "Editar elemento")
        {
            PermiteMultiSeleccion = true;
        }
    }

    public class ExportarElementos : AccionDeMenuMnt
    {
        public ExportarElementos()
        : base(TipoDeAccionDeMnt.ExportarElemento, enumCssOpcionMenu.DeVista, "Exportar elementos")
        {
        }
    }
    public class EnviarElementos : AccionDeMenuMnt
    {
        public EnviarElementos()
        : base(TipoDeAccionDeMnt.EnviarElementos, enumCssOpcionMenu.DeElemento, "Enviar elementos")
        {
            PermiteMultiSeleccion = true;
        }
    }
    public class RelacionarElementos : AccionDeMenuMnt
    {
        public string IdHtmlDeLaModalAsociada { get; private set; }
        public Func<string> RenderDeLaModal { get; private set; }
        public RelacionarElementos(string idHtmlDeLaModalAsociada, Func<string> renderDeLaModal, string ayuda)
        : base(TipoDeAccionDeMnt.AbrirModalParaRelacionar, enumCssOpcionMenu.DeVista, ayuda)
        {
            IdHtmlDeLaModalAsociada = idHtmlDeLaModalAsociada;
            Parametros.Add(idHtmlDeLaModalAsociada);
            RenderDeLaModal = renderDeLaModal;
            PermiteMultiSeleccion = false;
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
        public ConsultarRelaciones(string idHtmlDeLaModalAsociada, Func<string> renderDeLaModal, string ayuda)
        : base(TipoDeAccionDeMnt.AbrirModalParaConsultarRelaciones, enumCssOpcionMenu.DeElemento, ayuda)
        {
            IdHtmlDeLaModalAsociada = idHtmlDeLaModalAsociada;
            Parametros.Add(idHtmlDeLaModalAsociada);
            RenderDeLaModal = renderDeLaModal;
            PermiteMultiSeleccion = false;
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
        public AccionDeMenuCreacion(string tipoDeAccionDeCreacion, enumCssOpcionMenu claseAccion, string ayuda)
            : base(tipoDeAccionDeCreacion, claseAccion, ayuda)
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
        : base(TipoDeAccionDeCreacion.NuevoElemento, enumCssOpcionMenu.DeElemento, "Crear un nuevo elemento")
        {
        }
    }

    public class CerrarCreacion : AccionDeMenuCreacion
    {
        public CerrarCreacion()
        : base(TipoDeAccionDeCreacion.CerrarCreacion, enumCssOpcionMenu.Basico, "Cerrar creación")
        {
        }
    }


    /**********************************************************/
    // Acciones de menú de la modal o vista de creación
    // renderiza llamada Crud.EjecutarMenuEdt(...)
    /**********************************************************/
    public class AccionDeMenuEdicion : AccionDeMenu
    {
        public AccionDeMenuEdicion(string tipoDeAccionDeEdicion, enumCssOpcionMenu claseAccion, string ayuda)
        : base(tipoDeAccionDeEdicion, claseAccion, ayuda)
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
        : base(TipoDeAccionDeEdicion.ModificarElemento, enumCssOpcionMenu.DeElemento, "Modificar datos")
        {
        }
    }
    public class CancelarEdicion : AccionDeMenuEdicion
    {
        public CancelarEdicion()
        : base(TipoDeAccionDeEdicion.CancelarEdicion, enumCssOpcionMenu.Basico, "Cancelar edicion")
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

        public OpcionDeMenu(Menu<TElemento> menu, AccionDeMenu accion, string titulo, enumModoDeAccesoDeDatos permisosNecesarios)
        : this(menu, accion, TipoDeLlamada.Get, titulo, permisosNecesarios, accion.ClaseDeAccion)
        {
        }

        public OpcionDeMenu(Menu<TElemento> menu, AccionDeMenu accion, TipoDeLlamada tipoAccion, string titulo, enumModoDeAccesoDeDatos permisosNecesarios, enumCssOpcionMenu clase)
        : base(
          padre: menu,
          id: $"{menu.Id}_{enumTipoControl.Opcion.Render()}_{menu.OpcionesDeMenu.Count}",
          etiqueta: titulo,
          propiedad: null,
          ayuda: accion.Ayuda,
          posicion: null
        )
        {
            Tipo = enumTipoControl.Opcion;
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
                var htmlFormPost = "";
                if (Accion is AccionDeRelacionarElemenetos)
                    htmlFormPost = RenderAccionDeRelacion(disbled);

                if (Accion is AccionDeGetionarDatosDependientes)
                    htmlFormPost = RenderAccionDeDependencias(disbled);

                return htmlFormPost;
            }

            var permite = (Accion is AccionDeMenuMnt) ? ((AccionDeMenuMnt)Accion).PermiteMultiSeleccion ? "S" : "N" : "N";
            var htmlOpcionMenu = $@"<input id=¨{IdHtml}¨
                                           type=¨button¨
                                           tipo=¨{Tipo.Render()}¨
                                           clase=¨{Css.Render(ClaseBoton)}¨
                                           permisos-necesarios=¨{PermisosNecesarios.Render()}¨
                                           permite-multi-seleccion=¨{permite}¨
                                           value=¨{Etiqueta}¨
                                           onClick=¨{Accion.RenderAccion()}¨
                                           title=¨{Ayuda}¨
                                           {disbled} />";
            return htmlOpcionMenu;
        }

        private string RenderAccionDeRelacion(string disbled)
        {

            return $@"
                    <form id=¨{IdHtml}¨ action=¨{((AccionDeRelacionarElemenetos)Accion).UrlDelCrudDeRelacion}¨ method=¨post¨ navegar-al-crud=¨{((AccionDeRelacionarElemenetos)Accion).NavegarAlCrud}¨ restrictor=¨{IdHtml}-restrictor¨ orden=¨{IdHtml}-orden¨ style=¨display: inline-block;¨ >
                        <input id=¨{IdHtml}-restrictor¨ type=¨hidden¨ name =¨restrictor¨ >
                        <input id=¨{IdHtml}-orden¨ type=¨hidden¨ name = ¨orden¨ >
                        <input type=¨button¨ 
                               tipo=¨{Tipo.Render()}¨
                               clase=¨{Css.Render(ClaseBoton)}¨ 
                               permisos-necesarios=¨{PermisosNecesarios.Render()}¨ 
                               permite-multi-seleccion=¨N¨
                               value=¨{Etiqueta}¨ 
                               onClick=¨{Accion.RenderAccion().Replace("idDeOpcMenu", IdHtml)}¨ 
                               title=¨{Ayuda}¨
                               {disbled} />
                    </form>
                ";
        }
        private string RenderAccionDeDependencias(string disbled)
        {
            return $@"
                    <form id=¨{IdHtml}¨ action=¨{((AccionDeGetionarDatosDependientes)Accion).UrlDelCrudDeDependientes}¨ method=¨post¨ navegar-al-crud=¨{((AccionDeGetionarDatosDependientes)Accion).NavegarAlCrud}¨ restrictor=¨{IdHtml}-restrictor¨ orden=¨{IdHtml}-orden¨ style=¨display: inline-block;¨ >
                        <input id=¨{IdHtml}-restrictor¨ type=¨hidden¨ name =¨restrictor¨ >
                        <input id=¨{IdHtml}-orden¨ type=¨hidden¨ name = ¨orden¨ >
                        <input type=¨button¨ 
                               tipo=¨{Tipo.Render()}¨
                               clase=¨{Css.Render(ClaseBoton)}¨ 
                               permisos-necesarios=¨{PermisosNecesarios.Render()}¨ 
                               permite-multi-seleccion=¨N¨
                               value=¨{Etiqueta}¨ 
                               onClick=¨{Accion.RenderAccion().Replace("idDeOpcMenu", IdHtml)}¨ 
                               title=¨{Ayuda}¨
                               {disbled} />
                    </form>
                ";
        }
    }
}
