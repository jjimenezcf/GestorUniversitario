
using ServicioDeDatos.Seguridad;

namespace MVCSistemaDeElementos.Descriptores
{

    public enum enumClaseCcsCuerpo
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

    public enum enumClaseCcsMnt {
        MntMenuContenedor,
        MntMenuZona,
        MntFiltroExpansor,
        MntFiltroBloqueContenedor,
        MntFiltroBloqueVacio
    }

    public enum enumClaseCcsDiv
    {
        DivVisible,
        DivOculto
    }

    public enum enumClaseOpcionMenu
    {
        DeVista,
        DeElemento,
        Basico
    }

    public enum enumClaseCcsGrid
    {
        ColumnaOculta,
        ColumnaCabecera
    }

    public static class ClaseCss
    {
        public static string Render(enumClaseCcsMnt clase)
        {
            switch (clase)
            {
                case enumClaseCcsMnt.MntMenuContenedor: return "div-mnt-menu-contenedor";
                case enumClaseCcsMnt.MntMenuZona: return "div-mnt-menu-zona";
                case enumClaseCcsMnt.MntFiltroExpansor: return "div-mnt-filtro-expansor";
                case enumClaseCcsMnt.MntFiltroBloqueContenedor: return "div-mnt-bloque-contenedor";
                case enumClaseCcsMnt.MntFiltroBloqueVacio: return "div-mnt-bloque-vacio";
            }
            return "";
        }


        public static string Render(enumClaseCcsGrid clase)
        {
            switch (clase)
            {
                case enumClaseCcsGrid.ColumnaCabecera: return "columna-cabecera";
                case enumClaseCcsGrid.ColumnaOculta: return "columna-oculta";
            }
            return "";
        }

        public static string Render(enumClaseCcsCuerpo clase)
        {
            switch (clase)
            {
                case enumClaseCcsCuerpo.Cuerpo: return "cuerpo";
                case enumClaseCcsCuerpo.CuerpoCabecera: return "cuerpo-cabecera";
                case enumClaseCcsCuerpo.CuerpoDatos: return "cuerpo-datos";
                case enumClaseCcsCuerpo.CuerpoDatosFiltro: return "cuerpo-datos-filtro";
                case enumClaseCcsCuerpo.CuerpoDatosGrid: return "cuerpo-datos-grid";
                case enumClaseCcsCuerpo.CuerpoDatosGridTboby: return "cuerpo-datos-tbody";
                case enumClaseCcsCuerpo.CuerpoDatosGridThead: return "cuerpo-datos-thead";
                case enumClaseCcsCuerpo.CuerpoPie: return "cuerpo-pie";
            }
            return "";
        }

        public static string Render(enumClaseCcsDiv clase)
        {
            switch (clase)
            {
                case enumClaseCcsDiv.DivVisible: return "div-visible";
                case enumClaseCcsDiv.DivOculto: return "div-no-visible";
            }
            return "";
        }

        public static string Render(enumClaseOpcionMenu clase)
        {
            switch (clase)
            {
                case enumClaseOpcionMenu.DeElemento: return "de-elemento";
                case enumClaseOpcionMenu.DeVista: return "de-vista";
                case enumClaseOpcionMenu.Basico: return "basico";
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