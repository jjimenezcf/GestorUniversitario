const newLine = "\n";

const TipoMensaje = { Info: "informativo", Error: "Error", Warning: "Revision" };


const Literal = {
    controlador: "controlador",
    id: "id",
    filtro: {
        clausulaId: 'id',
        criterio: {
            igual: 'igual'
        }
    }

};

const ModoOrdenacion = {
    ascedente: "ascendente",
    descendente: "descendente",
    sinOrden: "sin-orden"
};

const atControl = {
    propiedad: "propiedad",
    nombre: "name",
    criterio: "criterio-de-filtro",
    zonaDeFiltro: "zona-de-filtro",
    filtro: "control-de-filtro",
    tablaDeDatos: "tabla-de-datos",
    id: Literal.id,
    crudModal: 'crud-modal',
    controlador: "controlador",
    obligatorio: "obligatorio",
    modoOrdenacion: "modo-ordenacion",
    expresionElemento: "expresion-elemento",
    tipo: "tipo",
    imagenVinculada: "imagen-vinculada",
    valor: "value",
    restrictor: "restrictor",
    eventoJs: {
        onclick: 'onclick'
    }
};

const atGrid = {
    id: 'grid-del-mnt',
    idSeleccionado: 'id-seleccionado',
    navegador: {
        cantidad: "cantidad-a-mostrar",
        pagina: "pagina",
        leidos: "leidos",
        posicion: "posicion",
        total: "total-en-bd",
        titulo: "title"
    },
    paginaDeDatos: "pagina-de-datos",
    accion: {
        buscar: "buscar",
        siguiente: "sigiente",
        anterior: "anterior",
        restaurar: "restaurar",
        ultima: "ultima",
        ordenar: "ordenar"
    },
    selector: "selector",    
    idCtrlCantidad: "nav_2_reg",
    idInfo: "info"
};

const atArchivo = {
    id: "id-archivo",
    nombre: "nombre-archivo",
    canvas: "canvas-vinculado",
    imagen: "imagen-vinculada",
    barra: "barra-vinculada",
    rutaDestino: "ruta-destino",
    extensionesValidas: "accept",
    url: "src"
};

const atSelector = {
    popiedadBuscar: "propiedadBuscar",
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
};

const atListas = {
    claseElemento: 'clase-elemento',
    guardarEn: 'guardar-en',
    mostrarPropiedad: 'mostrar-propiedad',
    yaCargado: 'ya-cargada',
    idDeLaLista: 'list',
    identificador: 'identificador'
};


const atRelacion = {
    navegarA: 'navegar-al-crud',
    restrictor: 'restrictor',
    orden: 'orden'
};


const TagName = {
    input: "input",
    select: "select"
};

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
};

const Ajax = {
    EndPoint: {
        Crear: "epCrearElemento",
        LeerGridEnHtml: "epLeerGridHtml",
        LeerDatosParaElGrid: "epLeerDatosParaElGrid",
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
        accion: "accion",
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
        Relacionar: "relacionar-elementos",

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
};

const TipoControl = {
    Tipo: "tipo",
    Editor: "editor",
    Check: "check",
    Selector: "selector",
    ListaDeElementos: "lista-de-elemento",
    ListaDinamica: "lista-dinamica",
    Archivo: "archivo",
    VisorDeArchivo: "visor-archivo",
    UrlDeArchivo: "url-archivo",
    restrictorDeFiltro: "restrictor-filtro",
    restrictorDeEdicion: "restrictor-edicion"
};

const ModoTrabajo = {
    creando: "creando",
    editando: "editando",
    consultando: "consultando",
    copiando: "copiando",
    borrando: "borrando",
    mantenimiento: "mantenimiento"
};


const Relaciones = {
    roles: 'RolDto',
    puestos: 'PuestoDto'
};

const Variables = {
    Usuario: {
        restrictor: "idusuario",
        Usuario: "nombre-usuario",
    },
    Puesto: {
        restrictor: "idpuesto",
        puesto: "nombre-puesto",
    }
};

const Sesion = {
    historial: "historial",
    restrictorDeEntrada: "restrictor"
};

