﻿
using System;
using ServicioDeDatos.Seguridad;

namespace MVCSistemaDeElementos.Descriptores
{

    public enum enumCssFiltro
    {
        ListaDinamica,
        ListaDeElementos,
        ContenedorListaDinamica,
        ContenedorListaDeElementos,
        ContenedorCheck,
        Check
    }


    public enum enumCssCuerpo
    {
        Cuerpo,
        CuerpoCabecera,
        CuerpoDatos,
        CuerpoDatosFiltro,
        CuerpoDatosGrid,
        CuerpoDatosGridThead,
        CuerpoDatosGridTboby,
        CuerpoPie,
        CuerpoDatosFiltroBloque,
    }

    public enum enumCssMnt
    {
        MntMenuContenedor,
        MntMenuZona,
        MntFiltroExpansor,
        MntFiltroBloqueContenedor,
        MntFiltroBloqueVacio,
        MntTablaDeFiltro
    }

    public enum enumCssNavegadorEnModal
    {
        InfoGrid,
        Mensaje,
        Cantidad,
        Opcion,
        Contenedor,
        Navegador
    }


    public enum enumCssNavegadorEnMnt
    {
        InfoGrid,
        Mensaje,
        Cantidad,
        Opcion,
        Navegador
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

    public enum enumCssCreacion
    {
        TablaDeCreacion,
        CuerpoDeCrearcion
    }

    public enum enumCssEdicion
    {
        TablaDeEdicion,
        CuerpoDeTablaDeEdicion,
        CuerpoDeEdicion,
        ContenedorDeEdicionCabecera,
        ContenedorDeEdicionCuerpo,
        ContenedorDeEdicionPie,
        ContenedorId
    }

    public enum enumCssControlesDto
    {
        ContenedorListaDeElementos,
        ContenedorListaDinamica,
        ContenedorEditor,
        ContenedorCheck,
        FormDeArchivo,
        ContenedorArchivo,
        TablaDeArchivo,
        FilaDeArchivo,
        ColumnaDeArchivo,
        Check,
        Selector,
        Editor,
        Etiqueta,
        ListaDeElementos,
        ListaDinamica,
        SelectorDeArchivo,
        BarraAzulArchivo,
        EditorRestrictor
    }



    public static class Css
    {
        public static string Render(enumCssControlesDto clase)
        {
            switch (clase)
            {
                case enumCssControlesDto.ContenedorListaDeElementos: return "contenedor-selector";
                case enumCssControlesDto.ContenedorListaDinamica: return "contenedor-selector";
                case enumCssControlesDto.ContenedorEditor: return "contenedor-editor";
                case enumCssControlesDto.ContenedorArchivo: return "contenedor-archivo";
                case enumCssControlesDto.ContenedorCheck: return "contenedor-check";
                case enumCssControlesDto.Check: return "check-dto";
                case enumCssControlesDto.Selector: return "selector-dto";
                case enumCssControlesDto.Editor: return "editor-dto";
                case enumCssControlesDto.EditorRestrictor: return "form-control";
                case enumCssControlesDto.Etiqueta: return "etiqueta-dto";
                case enumCssControlesDto.ListaDinamica: return "lista-dinamica";
                case enumCssControlesDto.ListaDeElementos: return "lista-de-elementos-dto";
                case enumCssControlesDto.FormDeArchivo: return "form-archivo";
                case enumCssControlesDto.TablaDeArchivo: return "tabla-archivo-subir";
                case enumCssControlesDto.FilaDeArchivo: return "tr-archivo-subir";
                case enumCssControlesDto.ColumnaDeArchivo: return "td-archivo-subir";
                case enumCssControlesDto.SelectorDeArchivo: return "selector-de-archivo";
                case enumCssControlesDto.BarraAzulArchivo: return "barra-azul";
            }
            throw new Exception($"No se ha definido que renderizar para la clase {clase}");
        }

        public static string Render(enumCssFiltro clase)
        {
            switch (clase)
            {
                case enumCssFiltro.ListaDinamica: return "lista-dinamica";
                case enumCssFiltro.ListaDeElementos: return "lista-de-elementos";
                case enumCssFiltro.ContenedorListaDinamica: return "contenedor-selector";
                case enumCssFiltro.ContenedorListaDeElementos: return "contenedor-selector";
                case enumCssFiltro.ContenedorCheck: return "contenedor-check";
                case enumCssFiltro.Check: return "check-flt";
            }
            throw new Exception($"No se ha definido que renderizar para la clase {clase}");
        }


        public static string Render(enumCssCreacion clase)
        {
            switch (clase)
            {
                case enumCssCreacion.TablaDeCreacion: return "tabla-creacion";
                case enumCssCreacion.CuerpoDeCrearcion: return "cuerpo-creacion";
            }
            throw new Exception($"No se ha definido que renderizar para la clase {clase}");
        }

        public static string Render(enumCssEdicion clase)
        {
            switch (clase)
            {
                case enumCssEdicion.TablaDeEdicion: return "tabla-edicion";
                case enumCssEdicion.CuerpoDeEdicion: return "cuerpo-edicion";
                case enumCssEdicion.ContenedorDeEdicionCabecera: return "contenedor-edicion-cabecera";
                case enumCssEdicion.ContenedorDeEdicionCuerpo: return "contenedor-edicion-cuerpo";
                case enumCssEdicion.ContenedorDeEdicionPie: return "contenedor-edicion-pie";
                case enumCssEdicion.CuerpoDeTablaDeEdicion: return "cuerpo-tabla-edicion";
                case enumCssEdicion.ContenedorId: return "contenedor-id";
            }
            throw new Exception($"No se ha definido que renderizar para la clase {clase}");
        }

        public static string Render(enumCssMnt clase)
        {
            switch (clase)
            {
                case enumCssMnt.MntMenuContenedor: return "div-mnt-menu-contenedor";
                case enumCssMnt.MntMenuZona: return "div-mnt-menu-zona";
                case enumCssMnt.MntFiltroExpansor: return "div-mnt-filtro-expansor";
                case enumCssMnt.MntFiltroBloqueContenedor: return "div-mnt-bloque-contenedor";
                case enumCssMnt.MntFiltroBloqueVacio: return "div-mnt-bloque-vacio";
                case enumCssMnt.MntTablaDeFiltro: return "tabla-filtro";
            }
            throw new Exception($"No se ha definido que renderizar para la clase {clase}");
        }
        public static string Render(enumCssNavegadorEnModal clase)
        {
            switch (clase)
            {
                case enumCssNavegadorEnModal.Contenedor: return "pie-grid";
                case enumCssNavegadorEnModal.Cantidad: return "navegador-cantidad-grid";
                case enumCssNavegadorEnModal.Opcion: return "pie-grid-opciones";
                case enumCssNavegadorEnModal.Mensaje: return "pie-grid-mensaje";
                case enumCssNavegadorEnModal.InfoGrid: return "pie-grid-info";
                case enumCssNavegadorEnModal.Navegador: return "pie-grid-navegador";
            }
            throw new Exception($"No se ha definido que renderizar para la clase {clase}");
        }


        public static string Render(enumCssNavegadorEnMnt clase)
        {
            switch (clase)
            {
                case enumCssNavegadorEnMnt.Cantidad: return "navegador-cantidad-grid";
                case enumCssNavegadorEnMnt.Opcion: return "cuerpo-pie-opciones";
                case enumCssNavegadorEnMnt.Mensaje: return "cuerpo-pie-mensaje";
                case enumCssNavegadorEnMnt.InfoGrid: return "cuerpo-pie-info";
                case enumCssNavegadorEnMnt.Navegador: return "cuerpo-pie-navegador";
            }
            throw new Exception($"No se ha definido que renderizar para la clase {clase}");
        }

        public static string Render(enumCssGrid clase)
        {
            switch (clase)
            {
                case enumCssGrid.ColumnaCabecera: return "columna-cabecera";
                case enumCssGrid.ColumnaOculta: return "columna-oculta";
            }
            throw new Exception($"No se ha definido que renderizar para la clase {clase}");
        }

        public static string Render(enumCssCuerpo clase)
        {
            switch (clase)
            {
                case enumCssCuerpo.Cuerpo: return "cuerpo";
                case enumCssCuerpo.CuerpoCabecera: return "cuerpo-cabecera";
                case enumCssCuerpo.CuerpoDatos: return "cuerpo-datos";
                case enumCssCuerpo.CuerpoDatosFiltro: return "cuerpo-datos-filtro";
                case enumCssCuerpo.CuerpoDatosFiltroBloque: return "cuerpo-datos-filtro-bloque";
                case enumCssCuerpo.CuerpoDatosGrid: return "cuerpo-datos-grid";
                case enumCssCuerpo.CuerpoDatosGridTboby: return "cuerpo-datos-tbody";
                case enumCssCuerpo.CuerpoDatosGridThead: return "cuerpo-datos-thead";
                case enumCssCuerpo.CuerpoPie: return "cuerpo-pie";
            }
            throw new Exception($"No se ha definido que renderizar para la clase {clase}");
        }

        public static string Render(enumCssDiv clase)
        {
            switch (clase)
            {
                case enumCssDiv.DivVisible: return "div-visible";
                case enumCssDiv.DivOculto: return "div-no-visible";
            }
            throw new Exception($"No se ha definido que renderizar para la clase {clase}");
        }

        public static string Render(enumCssOpcionMenu clase)
        {
            switch (clase)
            {
                case enumCssOpcionMenu.DeElemento: return "de-elemento";
                case enumCssOpcionMenu.DeVista: return "de-vista";
                case enumCssOpcionMenu.Basico: return "basico";
            }
            throw new Exception($"No se ha definido que renderizar para la clase {clase}");
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