const newLine = "\n";

const TipoMensaje = { Info: "informativo", Error: "Error", Warning: "Revision" };


const literal = {
    controlador: "controlador",
    negocio: "negocio",
    id: "id",
    true: "true",
    false: "false",
    filtro: {
        clausulaId: 'id',
        criterio: {
            igual: 'igual',
            diferente: 'diferente'
        }
    }
};

const atMenu = {
    abierto: "menu-abierto",
    plegado: "menu-plegado",
}

const atControl = {
    propiedad: "propiedad",
    nombre: "name",
    criterio: "criterio-de-filtro",
    zonaDeFiltro: "zona-de-filtro",
    filtro: "control-de-filtro",
    tablaDeDatos: "tabla-de-datos",
    id: literal.id,
    crudModal: 'crud-modal',
    propiedadRestrictora: 'propiedad-restrictora',
    controlador: "controlador",
    obligatorio: "obligatorio",
    modoOrdenacion: "modo-ordenacion",
    expresionElemento: "expresion-elemento",
    tipo: "tipo",
    imagenVinculada: "imagen-vinculada",
    valorInput: "value",
    valorTr: "idDelElemento",
    restrictor: "restrictor",
    nombreModal: "idModal",
    eventoJs: {
        onclick: 'onclick'
    }
};

const atMantenimniento = {
    zonaDeFiltro: atControl.zonaDeFiltro,
    controlador: literal.controlador,
    negocio: literal.negocio,
    zonaMenu: "zona-de-menu",
    gridDelMnt: "grid-del-mnt"
}

const ModoOrdenacion = {
    ascedente: "ascendente",
    descendente: "descendente",
    sinOrden: "sin-orden"
};


const atGrid = {
    id: atMantenimniento.gridDelMnt,
    zonaNavegador: 'zona-de-navegador',
    cabeceraTabla: 'cabecera-de-tabla',
    idSeleccionado: 'id-seleccionado',
    nombreSeleccionado: 'nombre-Seleccionado',
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
    idInfo: "info",
    idMensaje: "mensaje"
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

const atOpcionDeMenu = {
    permisosNecesarios: "permisos-necesarios",
    clase: "clase"
}

const atSelector = {
    popiedadBuscar: "propiedadBuscar",
    criterioBuscar: "criterioBuscar",
    idEditorMostrar: "ideditormostrar",
    idGridModal: "id-grid-modal",
    propiedadmostrar: "propiedadmostrar",
    refCheckDeSeleccion: "refCheckDeSeleccion",
    idModal: "id-modal",
    idBtnSelector: "idBtnSelector",
    ListaDeSeleccionados: 'ids-seleccionados',
    selector: "selector",
    propiedadParaFiltrar: "propiedadFiltrar"
};

const atListas = {
    claseElemento: 'clase-elemento',
    mostrarExpresion: 'mostrar-expresion',
    yaCargado: 'ya-cargada',
    idDeLaLista: 'list',
    identificador: 'identificador',
    expresionPorDefecto: 'nombre',
};

const atListasDeElemento = {
    claseElemento: atListas.claseElemento,
    mostrarExpresion: atListas.mostrarExpresion,
    yaCargado: atListas.yaCargado,
    expresionPorDefecto: atListas.expresionPorDefecto
}

const atListasDinamicas = {
    claseElemento: atListas.claseElemento,
    buscarPor: 'como-buscar',
    criterio: atControl.criterio,
    mostrarExpresion: atListas.mostrarExpresion,
    longitudNecesaria: 'longitud',
    idSeleccionado: 'idseleccionado',
    cargando: 'cargando',
    expresionPorDefecto: atListas.expresionPorDefecto,
    ultimaCadenaBuscada: 'ultima-busqueda',
    cantidad:'cantidad-a-leer'
};


const atListasDinamicasDto = {
    guardarEn: 'guardar-en'
};

const atCriterio = {
    contiene: 'contiene',
    comienza: 'comienza',
    igual: 'igual'
}

const atRelacion = {
    navegarAlCrud: 'navegar-al-crud',
    idRestrictor: atControl.restrictor,
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
    contenedorModal: "contenedor-modal",
    soloLectura: "solo-lectura",
    columnaOculta: "columna-oculta",
    filaDelGrid: "cuerpo-datos-tbody-tr",
    cuerpoDeLaTabla: 'cuerpo-datos-tbody',
    sinCapaDeBloqueo: "sin-capa-de-bloqueo",
    conCapaDeBloqueo: "con-capa-de-bloqueo"
};

const Ajax = {
    EndPoint: {
        Crear: "epCrearElemento",
        LeerGridEnHtml: "epLeerGridHtml",
        LeerDatosParaElGrid: "epLeerDatosParaElGrid",
        SolicitarMenuEnHtml: "epSolicitarMenuHtml",
        LeerPorId: "epLeerPorId",
        Modificar: "epModificarPorId",
        Borrar: "epBorrarPorId",
        RecargarModalEnHtml: "epRecargarModalEnHtml",
        Leer: "epLeer",
        CargarLista: "epCargarLista",
        CargaDinamica: "epCargaDinamica",
        SubirArchivo: "epSubirArchivo",
        CrearRelaciones: "epCrearRelaciones",
        LeerModoDeAccesoAlNegocio: "epLeerModoDeAccesoAlNegocio",
        LeerModoDeAccesoAlElemento: "epLeerModoDeAccesoAlElemento"
    },
    EpDeAcceso: {
        ReferenciarFoto: "epReferenciarFoto",
        ValidarAcceso: "epValidarAcceso"
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
        id: "id",
        idsJson: "idsJson",
        claseElemento: "claseElemento",
        fichero: "fichero",
        rutaDestino: "rutaDestino",
        extensiones: "extensionesValidas",
        login: "login",
        password: "password",
        negocio: "negocio"
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
        OrdenarPor: "ordenar-por",
        MostrarSoloSeleccionadas: "mostrar-solo-seleccionadas"
    },
    ModalParaRelacionar: {
        Relacionar: "nuevas-relaciones",
        Cerrar: "cerrar-relacionar",
        Buscar: "buscar-elementos",
        FilaPulsada: "fila-pulsada",
        ObtenerAnteriores: "obtener-anteriores",
        ObtenerSiguientes: "obtener-siguientes",
        ObtenerUltimos: "obtener-ultimos",
        OrdenarPor: "ordenar-por",
        MostrarSoloSeleccionadas: "mostrar-solo-seleccionadas"
    },
    ModalParaConsultaDeRelaciones: {
        Cerrar: "cerrar-consulta",
        Buscar: "buscar-elementos",
        FilaPulsada: "fila-pulsada",
        ObtenerAnteriores: "obtener-anteriores",
        ObtenerSiguientes: "obtener-siguientes",
        ObtenerUltimos: "obtener-ultimos",
        OrdenarPor: "ordenar-por",
        MostrarSoloSeleccionadas: "mostrar-solo-seleccionadas"
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
        AbrirModalParaRelacionar: "abrir-modal-para-relacionar",
        AbrirModalParaConsultarRelaciones: "abrir-modal-para-consultar-relaciones",
        Buscar: "buscar-elementos",
        ObtenerAnteriores: "obtener-anteriores",
        ObtenerSiguientes: "obtener-siguientes",
        ObtenerUltimos: "obtener-ultimos",
        OrdenarPor: "ordenar-por",
        FilaPulsada: "fila-pulsada",
        CambiarSelector: "cambiar-selector",
        OcultarMostrarFiltro: "ocultar-mostrar-filtro",
        OcultarMostrarBloque: "ocultar-mostrar-bloque",
        MostrarSoloSeleccionadas: "mostrar-solo-seleccionadas"
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

const ClaseDeOpcioDeMenu = {
    DeElemento: "de-elemento",
    DeVista: "de-vista",
    Basico: "basico"
}

const ModoDeAccesoDeDatos = {
    Administrador: "Administrador",
    Gestor: "gestor",
    Consultor: "Consultor",
    SinPermiso: "SinPermiso"
}

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
    restrictor: "restrictor",
    idSeleccionado: "idSeleccionado",
    paginaDestino: "pagina-destino",
    paginaActual: "pagina-actual",
    urlActual: "url-actual"
};

const GestorDeEventos = {
    deSeleccion: "Crud.EventosModalDeSeleccion",
    deCrearRelaciones: "Crud.EventosModalDeCrearRelaciones",
    deConsultaDeRelaciones: "Crud.EventosModalDeConsultaDeRelaciones",
    delMantenimiento: "Crud.EventosDelMantenimiento",
    deListaDinamica: "Crud.EventosDeListaDinamica"
}

