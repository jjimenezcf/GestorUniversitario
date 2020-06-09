const newLine = "\n";

const TipoMensaje = { Info: "informativo", Error: "Error", Warning: "Revision" };


const Literal = {
    controlador: "controlador",
    id: "id"
};

const ModoOrdenacion = {
    ascedente: "ascendente",
    descendente: "descendente",
    sinOrden: "sin-orden"
}

const Atributo = {
    propiedad: "propiedad",
    criterio: "criterio-de-filtro",
    zonaDeFiltro: "zonaDeFiltro",
    filtro: "control-de-filtro",
    id: Literal.id,
    controlador: "controlador",
    posicion: "posicion",
    obligatorio: "obligatorio",
    totalEnBd: "totalEnBd",
    modoOrdenacion: "modo-ordenacion",
    expresionElemento: "expresion-elemento",
    tipo: "tipo",
    imagenVinculada:"imagen-vinculada"
}

const AtributoSelectorArchivo = {
    idArchivo: "id-archivo",
    nombreArchivo: "nombre-archivo",
    canvasVinculado: "canvas-vinculado",
    imagenVinculada: "imagen-vinculada",
    barraVinculada: "barra-vinculada",
    rutaDestino: "ruta-destino",
    extensionesValidas: "accept",
    urlDeArchivo: "src"
}

const AtributoSelector = {
    popiedadBuscar : "propiedadBuscar",
    criterioBuscar: "criterioBuscar",
    idEditorMostrar: "ideditormostrar",
    idGridModal: "id-grid-modal",
    propiedadmostrar: "propiedadmostrar",
    refCheckDeSeleccion: "refCheckDeSeleccion",
    idModal: "id-modal",
    idBtnSelector: "idBtnSelector",
    ListaDeSeleccionados: 'idsSeleccionados',
    selector: "selector",
    propiedadParaFiltrar: "propiedadFiltrar"
}

const AtributosDeListas = {
    claseElemento: 'clase-elemento',
    guardarEn: 'guardar-en',
    mostrarPropiedad: 'mostrar-propiedad',
    yaCargado: 'ya-cargada',
    idDeLaLista: 'list',
    identificador: 'identificador'
}

const TagName = {
    input: "input",
    select: "select"
}

const ClaseCss = {
    crtlValido: "propiedad-valida",
    crtlNoValido: "propiedad-no-valida",
    propiedad: "propiedad",
    propiedadId: "propiedad-id",
    divVisible: "div-visible",
    divNoVisible: "div-no-visible",
    obligatorio: "obligatorio",
    ordenAscendente: "ordenada-ascendente",
    ordenDescendente: "ordenada-descendente",
    sinOrden: "ordenada-sin-orden",
    selectorElemento: "selector-de-elemento",
    barraVerde: "barra-verde",
    barraRoja: "barra-roja",
    barraAzul: "barra-azul",
    contenedorModal: "contenedor-modal"
}

const Ajax = {
    EndPoint: {
        Crear: "epCrearElemento",
        LeerGridEnHtml: "epLeerGridHtml",
        SolicitarMenuEnHtml: "epSolicitarMenuHtml",
        LeerPorIds: "epLeerPorIds",
        Modificar: "epModificarPorId",
        Borrar: "epBorrarPorId",
        RecargarModalEnHtml: "epRecargarModalEnHtml",
        Leer: "epLeer",
        CargarLista: "epCargarLista",
        CargaDinamica: "epCargaDinamica",
        SubirArchivo: "epSubirArchivo"
    },
    Param: {
        elementoJson: "elementoJson",
        idModal: "idModal",
        idGrid: "idGrid",
        modo: "modo",
        posicion: "posicion",
        cantidad: "cantidad",
        filtro: "filtro",
        orden: "orden",
        usuario: "usuario",
        idsJson: "idsJson",
        claseElemento: "claseElemento",
        fichero: "fichero",
        rutaDestino: "rutaDestino",
        extensiones: "extensionesValidas"
    },
    jsonResultError: 1,
    jsonResultOk: 0,
    jsonUndefined: undefined,
    eventoLoad: "load",
    eventoError: "error",
    eventoProgreso: "progress"
};

const LiteralEdt = {
    idCtrlCantidad: "nav_2_reg"
};

const LiteralMnt = {
    posicion: "posicion",
    postfijoDeCheckDeSeleccion: ".chksel"
};

const LiteralGrid = {
    idCtrlCantidad: "nav_2_reg"
}

const Evento = {
    ModalSeleccion: {
        Abrir: "abrir-modal-seleccion",
        Seleccionar: "seleccionar-elementos",
        Cerrar: "cerrar-modal-seleccion",
        Buscar: "buscar-elementos",
        FilaPulsada: "fila-pulsada",
        ObtenerAnteriores: "obtener-anteriores",
        ObtenerSiguientes: "obtener-siguientes",
        ObtenerUltimos: "obtener-ultimos",
        OrdenarPor: "ordenar-por"
    },
    ModalCreacion: {
        Crear: "crear-elemento",
        Cerrar: "cerrar-modal",
    },
    ModalEdicion: {
        Modificar: "modificar-elemento",
        Cerrar: "cerrar-modal",
    },
    ModalBorrar: {
        Borrar: "borrar-elemento",
        Cerrar: "cerrar-modal-de-borrado",
    },
    ListaDinamica: {
        Cargar: 'cargar',
        Seleccionar: 'seleccionar'
    },
    Mnt: {
        Crear: "crear-elemento",
        Editar: "editar-elemento",
        Borrar: "eliminar-elemento",
        Buscar: "buscar-elementos",
        ObtenerAnteriores: "obtener-anteriores",
        ObtenerSiguientes: "obtener-siguientes",
        ObtenerUltimos: "obtener-ultimos",
        OrdenarPor: "ordenar-por",
        FilaPulsada: "fila-pulsada",
        CambiarSelector: "cambiar-selector"
    },
    Creacion: {
        Crear: "nuevo-elemento",
        Cerrar: "cerrar-creacion",
    },
    Edicion: {
        Modificar: "modificar-elemento",
        Cerrar: "cancelar-edicion",
        MostrarPrimero: "mostrar-primero",
        MostrarAnterior: "mostrar-anterior",
        MostrarSiguiente: "mostrar-siguiente",
        MostrarUltimo: "mostrar-ultimo",
    }
}

const TipoControl = {
    Tipo : "tipo",
    Editor: "editor",
    Check: "check",
    Selector: "selector",
    ListaDeElementos: "lista-de-elemento",
    ListaDinamica: "lista-dinamica",
    Archivo: "archivo",
    VisorDeArchivo: "visor-archivo",
    UrlDeArchivo: "url-archivo"
};

const ModoTrabajo = {
    creando: "creando",
    editando: "editando",
    consultando: "consultando",
    copiando: "copiando",
    borrando: "borrando",
    mantenimiento: "mantenimiento"
}

