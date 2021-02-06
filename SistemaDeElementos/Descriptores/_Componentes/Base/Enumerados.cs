
using System;
using ServicioDeDatos.Seguridad;

namespace MVCSistemaDeElementos.Descriptores
{
    public enum enumTipoDeModal { ModalDeSeleccion, ModalDeRelacion, ModalDeConsulta }

    public enum GestorDeEventos { EventosModalDeConsultaDeRelaciones, EventosModalDeCrearRelaciones, EventosDelMantenimiento, EventosDelFormulario, EventosModalDeSeleccion, EventosDeListaDinamica }

    public static class TipoAccionDeListaDinamica
    {
        public const string cargar = "cargar-lista-dinamica";
        public const string seleccionar = "seleccionar--lista-dinamica";
    }


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
        public const string TeclaPulsada = "tecla-pulsada";
    }
    public static class TipoDeAccionFormulario
    {
        public const string Cerrar = "cerrar";
        public const string Aceptar = "aceptar";
        public const string OcultarMostrarBloque = "ocultar-mostrar-bloque";
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

    public enum enumModoOrdenacion
    {
        ascendente,
        descendente,
        sinOrden,
    };

    public enum enumCssOrdenacion
    {
        SinOrden,
        Ascendente,
        Descendente
    }

    public enum enumCssFiltro
    {
        ListaDinamica,
        ListaDeElementos,
        ContenedorListaDinamica,
        ContenedorListaDeElementos,
        ContenedorCheck,
        ContenedorEditor,
        ContenedorSelector,
        Check
    }

    public enum enumCssCuerpo
    {
        Cuerpo,
        CuerpoCabecera,
        CuerpoDatos,
        CuerpoDatosFiltro,
        CuerpoDatosGrid,
        CuerpoDatosFormulario,
        CuerpoDatosGridThead,
        CuerpoDatosGridTboby,
        CuerpoPie,
        CuerpoPieFormulario,
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

    public enum enumCssControlesFormulario
    {
        Editor,
        Lista,
        Check,
        Archivo,
        SelectorArchivo,
        InfoArchivo,
        ContenedorOpcion,
        ContenedorBarra,
        Menu
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
        SelectorDeImagen,
        BarraAzulArchivo,
        EditorRestrictor
    }

    public enum enumCssFormulario
    {
        ContenedorDeBloques,
        BloqueExpansor,
        BloqueDatos,
        BloqueIzquierdo,
        BloqueDerecho,
        Tabla,
        fila,
        columnaLabel,
        columnaControl,
        referenciaExpansor

    }

    public static class MetodosDeRenderizacion
    {
        public static string Render(this enumModoOrdenacion modo)
        {
            switch (modo)
            {
                case enumModoOrdenacion.ascendente: return "ascendente";
                case enumModoOrdenacion.descendente: return "descendente";
                case enumModoOrdenacion.sinOrden: return "sin-orden";
            }

            throw new Exception($"No se ha definido como renderizar el modo {modo}");
        }
    }

    public static class Css
    {
        public static string Render(enumCssControlesFormulario clase)
        {
            switch (clase)
            {
                case enumCssControlesFormulario.Editor: return "formulario-editor";
                case enumCssControlesFormulario.Lista: return "formulario-lista";
                case enumCssControlesFormulario.Check: return "formulario-check";
                case enumCssControlesFormulario.Archivo: return "formulario-archivo";
                case enumCssControlesFormulario.SelectorArchivo: return "formulario-selector-archivo";
                case enumCssControlesFormulario.InfoArchivo: return "formulario-visor-datos-archivo";
                case enumCssControlesFormulario.ContenedorOpcion: return "formulario-contenedor-opcion";
                case enumCssControlesFormulario.Menu: return "formulario-menu";
                case enumCssControlesFormulario.ContenedorBarra: return "formulario-contenedor-barra";
            }
            throw new Exception($"No se ha definido que renderizar para la clase {clase} para un formulario");
        }

        public static string Render(enumCssFormulario clase)
        {
            switch (clase)
            {
                case enumCssFormulario.ContenedorDeBloques: return "formulario-contenedor-de-bloques";
                case enumCssFormulario.BloqueIzquierdo: return "formulario-bloque-izquierdo";
                case enumCssFormulario.BloqueDerecho: return "formulario-bloque-derecho";
                case enumCssFormulario.BloqueExpansor: return "formulario-contenedor-bloque-expansor";
                case enumCssFormulario.BloqueDatos: return "formulario-contenedor-bloque-datos";
                case enumCssFormulario.Tabla: return "formulario-tabla";
                case enumCssFormulario.fila: return "formulario-fila";
                case enumCssFormulario.columnaControl: return "formulario-columna-control";
                case enumCssFormulario.columnaLabel: return "formulario-columna-label";
                case enumCssFormulario.referenciaExpansor: return "formulario-referencia";
            }
            throw new Exception($"No se ha definido que renderizar para la clase {clase} para un formulario");
        }


        public static string Render(enumCssControlesDto clase)
        {
            switch (clase)
            {
                case enumCssControlesDto.ContenedorListaDeElementos: return "contenedor-listas";
                case enumCssControlesDto.ContenedorListaDinamica: return "contenedor-listas";
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
                case enumCssControlesDto.SelectorDeImagen: return "selector-de-archivo";
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
                case enumCssFiltro.ContenedorListaDinamica: return "contenedor-listas";
                case enumCssFiltro.ContenedorListaDeElementos: return "contenedor-listas";
                case enumCssFiltro.ContenedorCheck: return "contenedor-check";
                case enumCssFiltro.ContenedorEditor: return "contenedor-editor-filtro";
                case enumCssFiltro.ContenedorSelector: return "contenedor-selector-filtro";
                case enumCssFiltro.Check: return "check-flt";
            }
            throw new Exception($"No se ha definido que renderizar para la clase {clase}");
        }


        public static string Render(enumCssOrdenacion clase)
        {
            switch (clase)
            {
                case enumCssOrdenacion.SinOrden: return "ordenada-sin-orden";
                case enumCssOrdenacion.Ascendente: return "ordenada-ascendente";
                case enumCssOrdenacion.Descendente: return "ordenada-desscendente";
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
                case enumCssCuerpo.CuerpoDatosFormulario: return "cuerpo-datos-formulario";
                case enumCssCuerpo.CuerpoPieFormulario: return "cuerpo-pie-formulario";
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
                case enumCssOpcionMenu.DeElemento: return "opcion-menu-de-elemento";
                case enumCssOpcionMenu.DeVista: return "opcion-menu-de-vista";
                case enumCssOpcionMenu.Basico: return "opcion-menu-basica";
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