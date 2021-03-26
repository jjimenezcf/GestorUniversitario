var Crud;
(function (Crud) {
    function EventosDelMantenimiento(accion, parametros) {
        try {
            switch (accion) {
                case Evento.Mnt.Crear: {
                    Crud.crudMnt.IraCrear();
                    break;
                }
                case Evento.Mnt.Editar: {
                    Crud.crudMnt.IraEditar();
                    break;
                }
                case Evento.Mnt.Borrar: {
                    Crud.crudMnt.AbrirModalBorrarElemento();
                    break;
                }
                case Evento.Mnt.Dependencias: {
                    let parametrosParaDependencias = parametros;
                    Crud.crudMnt.IrAlCrudDeDependencias(parametrosParaDependencias);
                    break;
                }
                case Evento.Mnt.Relacionar: {
                    let parametrosParaRelacionar = parametros;
                    Crud.crudMnt.IrAlCrudDeRelacionarCon(parametrosParaRelacionar);
                    break;
                }
                case Evento.Mnt.AbrirModalParaRelacionar: {
                    let idModal = parametros;
                    Crud.crudMnt.AbrirModalParaRelacionar(idModal);
                    break;
                }
                case Evento.Mnt.AbrirModalParaConsultarRelaciones: {
                    let idModal = parametros;
                    Crud.crudMnt.AbrirModalParaConsultarRelaciones(idModal);
                    break;
                }
                case Evento.Mnt.Buscar: {
                    Crud.crudMnt.Buscar(atGrid.accion.buscar, 0);
                    break;
                }
                case Evento.Mnt.ObtenerSiguientes: {
                    Crud.crudMnt.ObtenerSiguientes();
                    break;
                }
                case Evento.Mnt.ObtenerAnteriores: {
                    Crud.crudMnt.ObtenerAnteriores();
                    break;
                }
                case Evento.Mnt.ObtenerUltimos: {
                    Crud.crudMnt.ObtenerUltimos();
                    break;
                }
                case Evento.Mnt.FilaPulsada: {
                    let parIn = parametros.split("#");
                    Crud.crudMnt.FilaPulsada(parIn[0], parIn[1]);
                    break;
                }
                case Evento.Mnt.OrdenarPor: {
                    Crud.crudMnt.OrdenarPor(parametros);
                    break;
                }
                case Evento.Mnt.CambiarSelector: {
                    Crud.crudMnt.CambiarSelector(parametros);
                    break;
                }
                case Evento.Mnt.OcultarMostrarFiltro: {
                    Crud.crudMnt.OcultarMostrarFiltro();
                    break;
                }
                case Evento.Mnt.OcultarMostrarBloque: {
                    let idHtmlBloque = parametros;
                    Crud.crudMnt.OcultarMostrarBloque(idHtmlBloque);
                    break;
                }
                case Evento.Mnt.MostrarSoloSeleccionadas: {
                    Crud.crudMnt.MostrarSoloSeleccionadas(Crud.crudMnt.InputSeleccionadas, Crud.crudMnt.EtiquetasSeleccionadas, Crud.crudMnt.CuerpoTablaGrid, Crud.crudMnt.InfoSelector);
                    break;
                }
                case Evento.Mnt.TeclaPulsada: {
                    Crud.crudMnt.TeclaPulsada(Crud.crudMnt, event);
                    break;
                }
                default: {
                    MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `la opción ${accion} no está definida en el gestor de eventos del mantenimiento`);
                    break;
                }
            }
        }
        catch (error) {
            MensajesSe.Error(`Mantenimiento, accion: ${accion}`, error.message);
        }
    }
    Crud.EventosDelMantenimiento = EventosDelMantenimiento;
    function EventosModalDeSeleccion(accion, parametros) {
        let parIn = parametros.split("#");
        let modal = Crud.crudMnt.ObtenerModalDeSeleccion(parIn[0]);
        if (modal === undefined) {
            MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `Modal ${parIn[0]} no definida`);
            return;
        }
        try {
            switch (accion) {
                case Evento.ModalSeleccion.Abrir: {
                    modal.AbrirModalDeSeleccion();
                    break;
                }
                case Evento.ModalSeleccion.Cerrar: {
                    modal.CerrarModalDeSeleccion();
                    break;
                }
                case Evento.ModalSeleccion.Seleccionar: {
                    modal.SeleccionarElementos();
                    break;
                }
                case Evento.ModalSeleccion.FilaPulsada: {
                    let idCheck = parIn[1];
                    let idOrigen = parIn[2]; // si se ha pulsado en el check o en la fila
                    modal.FilaPulsada(idCheck, idOrigen);
                    break;
                }
                case Evento.ModalSeleccion.Buscar: {
                    modal.RecargarGrid();
                    break;
                }
                case Evento.ModalSeleccion.ObtenerSiguientes: {
                    modal.ObtenerSiguientes();
                    break;
                }
                case Evento.ModalSeleccion.ObtenerAnteriores: {
                    modal.ObtenerAnteriores();
                    break;
                }
                case Evento.ModalSeleccion.ObtenerUltimos: {
                    modal.ObtenerUltimos();
                    break;
                }
                case Evento.ModalSeleccion.OrdenarPor: {
                    let columna = parIn[1];
                    modal.OrdenarPor(columna);
                    break;
                }
                case Evento.ModalSeleccion.MostrarSoloSeleccionadas: {
                    var input = modal.InputSeleccionadas;
                    var etiqueta = modal.EtiquetasSeleccionadas;
                    var tbodyDelGrid = modal.CuerpoTablaGrid;
                    Crud.crudMnt.MostrarSoloSeleccionadas(input, etiqueta, tbodyDelGrid, modal.InfoSelector);
                    break;
                }
                case Evento.ModalSeleccion.TeclaPulsada: {
                    modal.TeclaPulsada(modal, event);
                    break;
                }
                default: {
                    MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `la opción ${accion} no está definida en el gestor de eventos de selección`);
                    break;
                }
            }
        }
        catch (error) {
            MensajesSe.Error(`Modal de selección, accion: ${accion}`, error.message);
        }
    }
    Crud.EventosModalDeSeleccion = EventosModalDeSeleccion;
    function EventosModalDeCrearRelaciones(accion, parametros) {
        let parIn = parametros.split("#");
        let modal = Crud.crudMnt.ObtenerModalParaRelacionar(parIn[0]);
        if (modal === undefined) {
            MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `Modal ${parIn[0]} no definida`);
            return;
        }
        try {
            switch (accion) {
                case Evento.ModalParaRelacionar.Cerrar: {
                    modal.CerrarModalParaRelacionar();
                    break;
                }
                case Evento.ModalParaRelacionar.Relacionar: {
                    modal.CrearRelaciones();
                    break;
                }
                case Evento.ModalParaRelacionar.Buscar: {
                    modal.RecargarGrid();
                    break;
                }
                case Evento.ModalParaRelacionar.ObtenerSiguientes: {
                    modal.ObtenerSiguientes();
                    break;
                }
                case Evento.ModalParaRelacionar.ObtenerAnteriores: {
                    modal.ObtenerAnteriores();
                    break;
                }
                case Evento.ModalParaRelacionar.ObtenerUltimos: {
                    modal.ObtenerUltimos();
                    break;
                }
                case Evento.ModalParaRelacionar.OrdenarPor: {
                    let columna = parIn[1];
                    modal.OrdenarPor(columna);
                    break;
                }
                case Evento.ModalParaRelacionar.FilaPulsada: {
                    let idCheck = parIn[1];
                    let idOrigen = parIn[2]; // si se ha pulsado en el check o en la fila
                    modal.FilaPulsada(idCheck, idOrigen);
                    break;
                }
                case Evento.ModalParaRelacionar.MostrarSoloSeleccionadas: {
                    var input = modal.InputSeleccionadas;
                    var etiqueta = modal.EtiquetasSeleccionadas;
                    var tbodyDelGrid = modal.CuerpoTablaGrid;
                    modal.MostrarSoloSeleccionadas(input, etiqueta, tbodyDelGrid, modal.InfoSelector);
                    break;
                }
                case Evento.ModalParaRelacionar.TeclaPulsada: {
                    modal.TeclaPulsada(modal, event);
                    break;
                }
                default: {
                    MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `la opción ${accion} no está definida en el gestor de eventos de relación`);
                    break;
                }
            }
        }
        catch (error) {
            MensajesSe.Error(`Modal de crear relaciones, accion: ${accion}`, error.message);
        }
    }
    Crud.EventosModalDeCrearRelaciones = EventosModalDeCrearRelaciones;
    function EventosModalDeConsultaDeRelaciones(accion, parametros) {
        let parIn = parametros.split("#");
        let modal = Crud.crudMnt.ObtenerModalParaConsultarRelaciones(parIn[0]);
        if (modal === undefined) {
            MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `Modal ${parIn[0]} no definida`);
            return;
        }
        try {
            switch (accion) {
                case Evento.ModalParaConsultaDeRelaciones.Cerrar: {
                    modal.CerrarModalParaConsultarRelaciones();
                    break;
                }
                case Evento.ModalParaConsultaDeRelaciones.Buscar: {
                    modal.RecargarGrid();
                    break;
                }
                case Evento.ModalParaConsultaDeRelaciones.ObtenerSiguientes: {
                    modal.ObtenerSiguientes();
                    break;
                }
                case Evento.ModalParaConsultaDeRelaciones.ObtenerAnteriores: {
                    modal.ObtenerAnteriores();
                    break;
                }
                case Evento.ModalParaConsultaDeRelaciones.ObtenerUltimos: {
                    modal.ObtenerUltimos();
                    break;
                }
                case Evento.ModalParaConsultaDeRelaciones.OrdenarPor: {
                    let columna = parIn[1];
                    modal.OrdenarPor(columna);
                    break;
                }
                case Evento.ModalParaConsultaDeRelaciones.FilaPulsada: {
                    let idCheck = parIn[1];
                    let idOrigen = parIn[2]; // si se ha pulsado en el check o en la fila
                    modal.FilaPulsada(idCheck, idOrigen);
                    break;
                }
                case Evento.ModalParaConsultaDeRelaciones.MostrarSoloSeleccionadas: {
                    var input = modal.InputSeleccionadas;
                    var etiqueta = modal.EtiquetasSeleccionadas;
                    var tbodyDelGrid = modal.CuerpoTablaGrid;
                    modal.MostrarSoloSeleccionadas(input, etiqueta, tbodyDelGrid, modal.InfoSelector);
                    break;
                }
                case Evento.ModalParaConsultaDeRelaciones.TeclaPulsada: {
                    modal.TeclaPulsada(modal, event);
                    break;
                }
                default: {
                    MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `la opción ${accion} no está definida en el gestor de eventos de relación`);
                    break;
                }
            }
        }
        catch (error) {
            MensajesSe.Error(`Modal de consulta de relaciones, accion: ${accion}`, error.message);
        }
    }
    Crud.EventosModalDeConsultaDeRelaciones = EventosModalDeConsultaDeRelaciones;
    function EventosModalDeBorrar(accion) {
        try {
            switch (accion) {
                case Evento.ModalBorrar.Cerrar: {
                    Crud.crudMnt.CerrarModalDeBorrado();
                    break;
                }
                case Evento.ModalBorrar.Borrar: {
                    Crud.crudMnt.BorrarElemento();
                    break;
                }
                default: {
                    MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `la opción ${accion} no está definida`);
                    break;
                }
            }
        }
        catch (error) {
            MensajesSe.Error(`Modal de borrado, accion: ${accion}`, error.message);
        }
    }
    Crud.EventosModalDeBorrar = EventosModalDeBorrar;
    function EventosModalDeCreacion(accion) {
        try {
            switch (accion) {
                case Evento.ModalCreacion.Cerrar: {
                    Crud.crudMnt.CerrarModalDeCreacion();
                    break;
                }
                case Evento.ModalCreacion.Crear: {
                    Crud.crudMnt.CrearElemento();
                    break;
                }
                default: {
                    MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `la opción ${accion} no está definida`);
                    break;
                }
            }
        }
        catch (error) {
            MensajesSe.Error(`Modal de creación, accion: ${accion}`, error.message);
        }
    }
    Crud.EventosModalDeCreacion = EventosModalDeCreacion;
    function EventosModalDeEdicion(accion) {
        try {
            switch (accion) {
                case Evento.ModalEdicion.Cerrar: {
                    Crud.crudMnt.CerrarModalDeEdicion();
                    break;
                }
                case Evento.ModalEdicion.Modificar: {
                    Crud.crudMnt.ModificarElemento();
                    break;
                }
                default: {
                    MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `la opción ${accion} no está definida`);
                    break;
                }
            }
        }
        catch (error) {
            MensajesSe.Error(`Modal de edición, accion: ${accion}`, error.message);
        }
    }
    Crud.EventosModalDeEdicion = EventosModalDeEdicion;
    function EjecutarMenuEdt(accion) {
        try {
            Crud.crudMnt.crudDeEdicion.EjecutarAcciones(accion);
        }
        catch (error) {
            MensajesSe.Error(`Edición, accion: ${accion}`, error.message);
        }
    }
    Crud.EjecutarMenuEdt = EjecutarMenuEdt;
    function EventosDeEdicion(accion) {
        try {
            Crud.crudMnt.crudDeEdicion.EjecutarAcciones(accion);
        }
        catch (error) {
            MensajesSe.Error(`Eventos de edición, accion: ${accion}`, error.message);
        }
    }
    Crud.EventosDeEdicion = EventosDeEdicion;
    function EjecutarMenuCrt(accion) {
        try {
            Crud.crudMnt.crudDeCreacion.EjecutarAcciones(accion);
        }
        catch (error) {
            MensajesSe.Error(`Creacion, accion: ${accion}`, error.message);
        }
    }
    Crud.EjecutarMenuCrt = EjecutarMenuCrt;
    function EventosDeListaDinamica(accion, selector) {
        try {
            switch (accion) {
                case Evento.ListaDinamica.Cargar: {
                    Crud.crudMnt.CargarListaDinamica(selector);
                    break;
                }
                case Evento.ListaDinamica.Seleccionar: {
                    Crud.crudMnt.SeleccionarListaDinamica(selector);
                    break;
                }
                default: {
                    MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `la opción ${accion} no está definida`);
                    break;
                }
            }
        }
        catch (error) {
            MensajesSe.Error(`Lista dinámica, accion: ${accion}`, error.message);
        }
    }
    Crud.EventosDeListaDinamica = EventosDeListaDinamica;
    function EventosDeExpansores(accion, parametros) {
        try {
            switch (accion) {
                case Evento.Expansores.OcultarMostrarBloque: {
                    let partes = parametros.split(';');
                    if (partes.length != 2)
                        throw Error(`El parametro ${parametros} ha de definir el bloque expansor y el bloque que expande`);
                    let idHtmlExpansor = partes[0];
                    let idHtmlBloque = partes[1];
                    ApiControl.OcultarMostrarExpansor(idHtmlExpansor, idHtmlBloque);
                    break;
                }
                case Evento.Expansores.AbrirPestana: {
                    EntornoSe.AbrirPestana(parametros);
                    break;
                }
                default: {
                    MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `la opción ${accion} no está definida`);
                    break;
                }
            }
        }
        catch (error) {
            MensajesSe.Error(`Expansor, accion: ${accion}`, error.message);
        }
    }
    Crud.EventosDeExpansores = EventosDeExpansores;
})(Crud || (Crud = {}));
//# sourceMappingURL=GestorDeEventos.js.map