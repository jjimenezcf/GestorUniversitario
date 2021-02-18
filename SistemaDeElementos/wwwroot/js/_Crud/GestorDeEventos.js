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
                case Evento.Mnt.Relacionar: {
                    let crudDeRelacion = parametros;
                    Crud.crudMnt.RelacionarCon(crudDeRelacion);
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
                    Crud.crudMnt.FilaPulsada(Crud.crudMnt.InfoSelector, parIn[0], parIn[1]);
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
                    Mensaje(TipoMensaje.Error, `la opción ${accion} no está definida en el gestor de eventos del mantenimiento`);
                    break;
                }
            }
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
        }
    }
    Crud.EventosDelMantenimiento = EventosDelMantenimiento;
    function EventosModalDeSeleccion(accion, parametros) {
        let parIn = parametros.split("#");
        let modal = Crud.crudMnt.ObtenerModalDeSeleccion(parIn[0]);
        if (modal === undefined) {
            Mensaje(TipoMensaje.Error, `Modal ${parIn[0]} no definida`);
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
                    modal.FilaPulsada(modal.InfoSelector, idCheck, idOrigen);
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
                    Mensaje(TipoMensaje.Error, `la opción ${accion} no está definida en el gestor de eventos de selección`);
                    break;
                }
            }
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
        }
    }
    Crud.EventosModalDeSeleccion = EventosModalDeSeleccion;
    function EventosModalDeCrearRelaciones(accion, parametros) {
        let parIn = parametros.split("#");
        let modal = Crud.crudMnt.ObtenerModalParaRelacionar(parIn[0]);
        if (modal === undefined) {
            Mensaje(TipoMensaje.Error, `Modal ${parIn[0]} no definida`);
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
                    modal.FilaPulsada(modal.InfoSelector, idCheck, idOrigen);
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
                    Mensaje(TipoMensaje.Error, `la opción ${accion} no está definida en el gestor de eventos de relación`);
                    break;
                }
            }
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
        }
    }
    Crud.EventosModalDeCrearRelaciones = EventosModalDeCrearRelaciones;
    function EventosModalDeConsultaDeRelaciones(accion, parametros) {
        let parIn = parametros.split("#");
        let modal = Crud.crudMnt.ObtenerModalParaConsultarRelaciones(parIn[0]);
        if (modal === undefined) {
            Mensaje(TipoMensaje.Error, `Modal ${parIn[0]} no definida`);
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
                    modal.FilaPulsada(modal.InfoSelector, idCheck, idOrigen);
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
                    Mensaje(TipoMensaje.Error, `la opción ${accion} no está definida en el gestor de eventos de relación`);
                    break;
                }
            }
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
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
                    Mensaje(TipoMensaje.Error, `la opción ${accion} no está definida`);
                    break;
                }
            }
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
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
                    Mensaje(TipoMensaje.Error, `la opción ${accion} no está definida`);
                    break;
                }
            }
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
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
                    Mensaje(TipoMensaje.Error, `la opción ${accion} no está definida`);
                    break;
                }
            }
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
        }
    }
    Crud.EventosModalDeEdicion = EventosModalDeEdicion;
    function EjecutarMenuEdt(accion) {
        try {
            Crud.crudMnt.crudDeEdicion.EjecutarAcciones(accion);
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
        }
    }
    Crud.EjecutarMenuEdt = EjecutarMenuEdt;
    function EventosDeEdicion(accion) {
        try {
            Crud.crudMnt.crudDeEdicion.EjecutarAcciones(accion);
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
        }
    }
    Crud.EventosDeEdicion = EventosDeEdicion;
    function EjecutarMenuCrt(accion) {
        try {
            Crud.crudMnt.crudDeCreacion.EjecutarAcciones(accion);
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
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
                    Mensaje(TipoMensaje.Error, `la opción ${accion} no está definida`);
                    break;
                }
            }
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
        }
    }
    Crud.EventosDeListaDinamica = EventosDeListaDinamica;
})(Crud || (Crud = {}));
//# sourceMappingURL=GestorDeEventos.js.map