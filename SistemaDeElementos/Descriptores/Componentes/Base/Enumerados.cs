
using ServicioDeDatos.Seguridad;

namespace MVCSistemaDeElementos.Descriptores
{

    public enum enumCssCuerpo
    {
        Cuerpo,
        CuerpoCabecera,
        CuerpoDatos,
        CuerpoDatosFiltro,
        CuerpoDatosGrid,
        CuerpoDatosGridThead,
        CuerpoDatosGridTboby,
        CuerpoPie
    }

    public enum enumCssMnt {
        MntMenuContenedor,
        MntMenuZona,
        MntFiltroExpansor,
        MntFiltroBloqueContenedor,
        MntFiltroBloqueVacio
    }

    public enum enumCssNavegador
    {
        InfoGridModal,
        MensajeModal,
        CantidadModal,
        OpcionModal,
        NavegadorModal,
        ContenedorModal,
        InfoGridMnt,
        MensajeMnt,
        CantidadMnt,
        OpcionMnt,
        NavegadorMnt,
        ContenedorMnt
    }

    public enum enumCssDiv
    {
        DivVisible,
        DivOculto
    }

    public enum enumCssOpcionMenu
    {
        DeVista,
        DeElemento,
        Basico
    }

    public enum enumCssGrid
    {
        ColumnaOculta,
        ColumnaCabecera
    }

    public static class Css
    {
        public static string Render(enumCssMnt clase)
        {
            switch (clase)
            {
                case enumCssMnt.MntMenuContenedor: return "div-mnt-menu-contenedor";
                case enumCssMnt.MntMenuZona: return "div-mnt-menu-zona";
                case enumCssMnt.MntFiltroExpansor: return "div-mnt-filtro-expansor";
                case enumCssMnt.MntFiltroBloqueContenedor: return "div-mnt-bloque-contenedor";
                case enumCssMnt.MntFiltroBloqueVacio: return "div-mnt-bloque-vacio";
            }
            return "";
        }
        public static string Render(enumCssNavegador clase)
        {
            switch (clase)
            {
                case enumCssNavegador.InfoGridModal: return "navegador-info-grid";
                case enumCssNavegador.MensajeModal: return "navegador-mensaje";
                case enumCssNavegador.CantidadModal: return "navegador-cantidad-grid";
                case enumCssNavegador.OpcionModal: return "navegador-opcion-grid";
                case enumCssNavegador.ContenedorModal: return "pie-grid";
                case enumCssNavegador.NavegadorModal: return "navegador-grid";

                case enumCssNavegador.InfoGridMnt: return "cuerpo-pie-info";
                case enumCssNavegador.MensajeMnt: return "cuerpo-pie-mensaje";
                case enumCssNavegador.CantidadMnt: return "navegador-cantidad-grid";
                case enumCssNavegador.OpcionMnt: return "cuerpo-pie-opciones";
                case enumCssNavegador.ContenedorMnt: return "cuerpo-pie-navegador";
                case enumCssNavegador.NavegadorMnt: return "navegador-grid";
            }
            return "";
        }
        

        public static string Render(enumCssGrid clase)
        {
            switch (clase)
            {
                case enumCssGrid.ColumnaCabecera: return "columna-cabecera";
                case enumCssGrid.ColumnaOculta: return "columna-oculta";
            }
            return "";
        }

        public static string Render(enumCssCuerpo clase)
        {
            switch (clase)
            {
                case enumCssCuerpo.Cuerpo: return "cuerpo";
                case enumCssCuerpo.CuerpoCabecera: return "cuerpo-cabecera";
                case enumCssCuerpo.CuerpoDatos: return "cuerpo-datos";
                case enumCssCuerpo.CuerpoDatosFiltro: return "cuerpo-datos-filtro";
                case enumCssCuerpo.CuerpoDatosGrid: return "cuerpo-datos-grid";
                case enumCssCuerpo.CuerpoDatosGridTboby: return "cuerpo-datos-tbody";
                case enumCssCuerpo.CuerpoDatosGridThead: return "cuerpo-datos-thead";
                case enumCssCuerpo.CuerpoPie: return "cuerpo-pie";
            }
            return "";
        }

        public static string Render(enumCssDiv clase)
        {
            switch (clase)
            {
                case enumCssDiv.DivVisible: return "div-visible";
                case enumCssDiv.DivOculto: return "div-no-visible";
            }
            return "";
        }

        public static string Render(enumCssOpcionMenu clase)
        {
            switch (clase)
            {
                case enumCssOpcionMenu.DeElemento: return "de-elemento";
                case enumCssOpcionMenu.DeVista: return "de-vista";
                case enumCssOpcionMenu.Basico: return "basico";
            }
            return "";
        }
    }


    public partial class ModoDeAccesoDeDatos
    {
        public static string Render(enumModoDeAccesoDeDatos modoDeAcceso)
        {
            return ModoDeAcceso.ToString(modoDeAcceso).ToLower();
        }
    }
}