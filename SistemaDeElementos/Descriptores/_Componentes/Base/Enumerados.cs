﻿
using System;
using ServicioDeDatos.Seguridad;

namespace MVCSistemaDeElementos.Descriptores
{
    public enum enumTipoDeModal { ModalDeSeleccion, ModalDeRelacion, ModalDeConsulta, ModalParaSeleccionar }

    public enum GestorDeEventos { EventosModalDeConsultaDeRelaciones
            , EventosModalDeCrearRelaciones
            , EventosModalDeExportacion
            , EventosModalDeEnviarCorreo
            , EventosDelMantenimiento
            , EventosDelFormulario
            , EventosModalDeSeleccion
            , EventosModalParaSeleccionar
            , EventosDeListaDinamica
            , EventosDeSelectorDeElementosEnModal
            , EventosMenuDelGrid
    }

    public static class TipoAccionDeListaDinamica
    {
        public const string cargar = "cargar-lista-dinamica";
        public const string perderFoco = "perder-foco-lista-dinamica";
        public const string obtenerFoco = "obtener-foco-lista-dinamica";
    }


    public enum TipoDeLlamada { Post, Get }
    public static class TipoDeAccionDeMnt
    {
        public const string CrearElemento = "crear-elemento";
        public const string EditarElemento = "editar-elemento"; 
        public const string ExportarElemento = "exportar-elementos";
        public const string EnviarElementos = "enviar-elementos";
        public const string EliminarElemento = "eliminar-elemento";
        public const string RelacionarElementos = "relacionar-elementos";
        public const string GestionarDependencias = "gestionar-dependencias";
        public const string AbrirModalParaRelacionar = "abrir-modal-para-relacionar";
        public const string AbrirModalParaConsultarRelaciones = "abrir-modal-para-consultar-relaciones";
        public const string OcultarMostrarFiltro = "ocultar-mostrar-filtro";
        public const string OcultarMostrarBloque = "ocultar-mostrar-bloque";
        public const string FilaPulsada = "fila-pulsada";
        public const string TeclaPulsada = "tecla-pulsada";
        public const string OcultarMostrarColumnas = "ocultar-mostrar-columnas";
    }

    public static class TipoAccionDeGrid
    {
        public const string MostrarSoloSeleccionadas = "mostrar-solo-seleccionadas";
        public const string SeleccionarTodo = "seleccionar-todo";
        public const string AnularSeleccion = "anular-seleccion";
        public const string AnularOrden = "anular-ordenacion";
        public const string AplicarOrdenInicial = "aplicar-orden-inicial";
        public const string RecargarGrid = "recargar-grid";

    }

    public static class TipoDeAccionFormulario
    {
        public const string Cerrar = "cerrar";
        public const string Aceptar = "aceptar";
        public const string OcultarMostrarBloque = "ocultar-mostrar-bloque";
    }
    public static class TipoDeAccionExpansor
    {
        public const string OcultarMostrarBloque = "ocultar-mostrar-bloque";
        public const string NavegarDesdeEdicion = "navegar-desde-edicion";
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
    public static class TipoDeAccionParaSeleccionar
    {
        public const string Cerrar = "cerrar-seleccionar";
    }
    public static class TipoDeAccionDeExportar
    {
        public const string Exportar = "exportar";
        public const string Cerrar = "cerrar-exportacion";
        public const string PulsarSometer = "pulsar-someter";
        public const string SalirListaDeCorreos = "salir-lista-correos";
    }

    public static class TipoDeAccionDeEnviarCorreo
    {
        public const string Enviar = "enviar-correo";
        public const string Cerrar = "cerrar-enviar-correo";
        public const string SalirListaDeCorreos = "salir-lista-correos";
        public const string SeleccionaUsuarios = "seleccionar-usuarios";
    }
    public static class TipoDeAccionSelectorEnModal
    {
        public const string PerderFoco = "perder-foco";
        public const string ObtenerFoco = "obtener-foco";
        public const string Blanquear = "blanquear-selector";
        public const string TrasSeleccionar = "tras-seleccionar";
        public const string OpcionSeleccionada = "opcion-seleccionar";

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
        ContenedorEntreFechas,
        Check,
        Hora,
        Fecha
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
        ColumnaCabecera,
        ColumnaAlineadaDerecha
    }

    public enum enumCssCreacion
    {
        TablaDeCreacion,
        CuerpoDeCrearcion,
        ContenedorPieOpciones,
        ContenedorPieModalOpciones
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

    public enum enumCssExpansor
    {
        Contenedor,
        Cabecera,
        Cuerpo,
        Pie,
        ContenedorDeControl,
        Expansor
    }

    public enum enumCssControlesDto
    {
        FormDeArchivo,
        ContenedorListaDeElementos,
        ContenedorEditorConEtiquetaIzquierda,
        ContenedorListaDinamica,
        ContenedorEtiqueta,
        ContenedorEtiquetaIzquierda,
        ContenedorEditor,
        ContenedorBotonSelector,
        ContenedorCheck,
        ContenedorFecha,
        ContenedorFechaHora,
        ContenedorAreaDeTexto,
        ContenedorArchivo,
        ContenedorReferencias,
        TablaDeArchivo,
        FilaDeArchivo,
        ColumnaDeArchivo,
        SelectorDeFecha,
        SelectorDeHora,
        Check,
        Selector,
        Editor,
        BotonSelector,
        Etiqueta,
        ListaDeElementos,
        ListaDinamica,
        AreaDeTexto,
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

    public enum enumCssExportacion
    {
        Contenedor,
        lista,
        sometido,
        enviar
    }

    public enum enumCssEnviarCorreo
    {
        Contenedor,
        cabecera,
        cuerpo,
        adjuntos
    }

    public enum enumCssSelectorEnModal
    {
        Contenedor,
        Editor,
        botonSelector
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
        public static string Render(this enumCssSelectorEnModal clase)
        {
            switch (clase)
            {
                case enumCssSelectorEnModal.Contenedor: return "selector-en-modal-contenedor";
                case enumCssSelectorEnModal.Editor: return "selector-en-modal-editor";
                case enumCssSelectorEnModal.botonSelector: return "selector-en-modal-boton-selector";
            }
            throw new Exception($"No se ha definido que renderizar para la clase {clase} para el selector en modal");

        }

        public static string Render(this enumCssEnviarCorreo clase)
        {
            switch (clase)
            {
                case enumCssEnviarCorreo.Contenedor: return "enviar-correo-contenedor";
                case enumCssEnviarCorreo.cabecera: return "enviar-correo-cabecera";
                case enumCssEnviarCorreo.cuerpo: return "enviar-correo-cuerpo";
                case enumCssEnviarCorreo.adjuntos: return "enviar-correo-adjuntos";
            }
            throw new Exception($"No se ha definido que renderizar para la clase {clase} para un formulario de envío de correo");

        }


        public static string Render(this enumCssExportacion clase)
        {
            switch (clase)
            {
                case enumCssExportacion.Contenedor: return "exportacion-contenedor";
                case enumCssExportacion.lista: return "exportacion-lista";
                case enumCssExportacion.sometido: return "exportacion-sometido";
                case enumCssExportacion.enviar: return "exportacion-enviar";
            }
            throw new Exception($"No se ha definido que renderizar para la clase {clase} para un formulario de exportación");

        }

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


        public static string Render(enumCssExpansor clase)
        {
            switch (clase)
            {
                case enumCssExpansor.Contenedor: return "grid-span";
                case enumCssExpansor.Pie: return "grid-span-pie";
                case enumCssExpansor.Cabecera: return "grid-span-cabecera";
                case enumCssExpansor.Cuerpo: return "grid-span-cuerpo";
                case enumCssExpansor.ContenedorDeControl: return "grid-span-cuerpo-control";
                case enumCssExpansor.Expansor: return "span-expansor";
            }
            throw new Exception($"No se ha definido que renderizar para la clase {clase}");
        }

        public static string Render(this enumCssControlesDto clase)
        {
            switch (clase)
            {
                case enumCssControlesDto.ContenedorEditorConEtiquetaIzquierda: return "contenedor-editor-etiqueta-izquierda";
                case enumCssControlesDto.ContenedorListaDeElementos: return "contenedor-listas";
                case enumCssControlesDto.ContenedorListaDinamica: return "contenedor-listas";
                case enumCssControlesDto.ContenedorEditor: return "contenedor-editor";
                case enumCssControlesDto.ContenedorBotonSelector: return "contenedor-boton-selector";
                case enumCssControlesDto.ContenedorEtiqueta: return "contenedor-etiqueta";
                case enumCssControlesDto.ContenedorEtiquetaIzquierda: return "contenedor-etiqueta-izquierda";
                case enumCssControlesDto.ContenedorArchivo: return "contenedor-archivo";
                case enumCssControlesDto.ContenedorReferencias: return "contenedor-referencias";
                case enumCssControlesDto.ContenedorCheck: return "contenedor-check";
                case enumCssControlesDto.ContenedorFecha: return "contenedor-fecha";
                case enumCssControlesDto.ContenedorFechaHora: return "contenedor-fecha-hora";
                case enumCssControlesDto.ContenedorAreaDeTexto: return "contenedor-area-de-texto";
                case enumCssControlesDto.SelectorDeFecha: return "fecha-dto";
                case enumCssControlesDto.SelectorDeHora: return "hora-dto";
                case enumCssControlesDto.Check: return "check-dto";
                case enumCssControlesDto.Selector: return "selector-dto";
                case enumCssControlesDto.Editor: return "editor-dto";
                case enumCssControlesDto.BotonSelector: return "boton-selector-dto";
                case enumCssControlesDto.EditorRestrictor: return "form-control";
                case enumCssControlesDto.Etiqueta: return "etiqueta-dto";
                case enumCssControlesDto.ListaDinamica: return "lista-dinamica";
                case enumCssControlesDto.AreaDeTexto: return "area-de-texto-dto";
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

        public static string Render(this enumCssFiltro clase)
        {
            switch (clase)
            {
                case enumCssFiltro.ListaDinamica: return "lista-dinamica";
                case enumCssFiltro.ListaDeElementos: return "lista-de-elementos";
                case enumCssFiltro.ContenedorListaDinamica: return "contenedor-listas-filtro";
                case enumCssFiltro.ContenedorListaDeElementos: return "contenedor-listas-filtro";
                case enumCssFiltro.ContenedorCheck: return "contenedor-check";
                case enumCssFiltro.ContenedorEditor: return "contenedor-editor-filtro";
                case enumCssFiltro.ContenedorSelector: return "contenedor-selector-filtro";
                case enumCssFiltro.ContenedorEntreFechas: return "contenedor-entre-fechas-filtro";
                case enumCssFiltro.Check: return "check-flt";
                case enumCssFiltro.Hora: return "hora-flt";
                case enumCssFiltro.Fecha: return "fecha-flt";
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
                case enumCssCreacion.ContenedorPieOpciones: return "contenedor-pie-opciones";
                case enumCssCreacion.ContenedorPieModalOpciones: return "contenedor-pie-modal-opciones";
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
                case enumCssGrid.ColumnaAlineadaDerecha: return "columna-alineacion-derecha";
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

}