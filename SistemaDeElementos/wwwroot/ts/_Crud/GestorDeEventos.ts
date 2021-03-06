﻿namespace Crud {

    export function EventosDelMantenimiento(accion: string, parametros: any): void {
        try {
            switch (accion) {
                case Evento.Mnt.Crear: {
                    crudMnt.IraCrear();
                    break;
                }
                case Evento.Mnt.Editar: {
                    crudMnt.IraEditar();
                    break;
                }
                case Evento.Mnt.Exportar: {
                    crudMnt.ModalExportacion_Abrir();
                    break;
                }
                case Evento.Mnt.EnviarCorreo: {
                    crudMnt.ModalEnviarCorreo_Abrir();
                    break;
                }
                case Evento.Mnt.Borrar: {
                    crudMnt.ModalDeBorrado_Abrir();
                    break;
                }
                case Evento.Mnt.Dependencias: {
                    let parametrosParaDependencias: string = parametros;
                    crudMnt.IrAlCrudDeDependencias(parametrosParaDependencias);
                    break;
                }
                case Evento.Mnt.Relacionar: {
                    let parametrosParaRelacionar: string = parametros;
                    crudMnt.IrAlCrudDeRelacionarCon(parametrosParaRelacionar);
                    break;
                }
                case Evento.Mnt.AbrirModalParaRelacionar: {
                    let idModal: string = parametros;
                    crudMnt.AbrirModalParaRelacionar(idModal);
                    break;
                }
                case Evento.Mnt.AbrirModalParaConsultarRelaciones: {
                    let idModal: string = parametros;
                    crudMnt.AbrirModalParaConsultarRelaciones(idModal);
                    break;
                }
                case Evento.Mnt.Buscar: {
                    crudMnt.Buscar(atGrid.accion.buscar, 0);
                    break;
                }
                case Evento.Mnt.ObtenerSiguientes: {
                    crudMnt.ObtenerSiguientes();
                    break;
                }
                case Evento.Mnt.ObtenerAnteriores: {
                    crudMnt.ObtenerAnteriores();
                    break;
                }
                case Evento.Mnt.ObtenerUltimos: {
                    crudMnt.ObtenerUltimos();
                    break;
                }
                case Evento.Mnt.FilaPulsada: {
                    let parIn: Array<string> = parametros.split("#");
                    crudMnt.FilaPulsada(parIn[0], parIn[1]);
                    break;
                }
                case Evento.Mnt.OrdenarPor: {
                    crudMnt.OrdenarPor(parametros);
                    break;
                }
                case Evento.Mnt.CambiarSelector: {
                    crudMnt.CambiarSelector(parametros);
                    break;
                }
                case Evento.Mnt.OcultarMostrarFiltro: {
                    crudMnt.OcultarMostrarFiltro();
                    break;
                }
                case Evento.Mnt.OcultarMostrarBloque: {
                    let idHtmlBloque: string = parametros;
                    crudMnt.OcultarMostrarBloque(idHtmlBloque);
                    break;
                }
                case Evento.Mnt.TeclaPulsada: {
                    crudMnt.TeclaPulsada(crudMnt, event);
                    break;
                }
                case Evento.Mnt.OcultarMostrarColumnas: {
                    let parIn: Array<string> = parametros.split("#");
                    let propiedades = [];
                    for (let i: number = 0; i < parIn.length; i++) {
                        propiedades.push(parIn[i]);
                    }
                    crudMnt.OcultarMostrarColumnas(propiedades);
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

    export function EventosMenuDelGrid(accion: string, idModal: string): void {
        let grid: GridDeDatos = crudMnt.ObtenerGrid(idModal);

        try {
            switch (accion) {
                case Evento.OpcionesDelGrid.SeleccionarTodo: {
                    grid.SeleccionarTodasLasFilas()
                    break;
                }
                case Evento.OpcionesDelGrid.AnularOrden: {
                    grid.AnularOrdenacion();
                    break;
                }
                case Evento.OpcionesDelGrid.AnularSeleccion: {
                    grid.DeselecionarTodasLasFilas();
                    break;
                }
                case Evento.OpcionesDelGrid.AplicarOrdenInicial: {
                    grid.InicializarOrdenacion()
                    break;
                }
                case Evento.OpcionesDelGrid.MostrarLasSeleccionadas: {
                    crudMnt.MostrarSoloSeleccionadas(grid.InputSeleccionadas, grid.EtiquetasSeleccionadas, grid.CuerpoTablaGrid, grid.InfoSelector)
                    break;
                }
                case Evento.OpcionesDelGrid.RecargarGrid: {
                    grid.RecargarGrid();
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

    export function EventosModalDeSeleccion(accion: string, parametros: string): void {

        let parIn: Array<string> = parametros.split("#");
        let modal: ModalSeleccion = crudMnt.ObtenerModalDeSeleccion(parIn[0]);
        if (modal === undefined) {
            MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `Modal ${parIn[0]} no definida`);
            return;
        }

        try {
            switch (accion) {
                case Evento.ModalSeleccionDeFiltro.Abrir: {
                    modal.AbrirModalDeSeleccion();
                    break;
                }
                case Evento.ModalSeleccionDeFiltro.Cerrar: {
                    modal.CerrarModalDeSeleccion();
                    break;
                }
                case Evento.ModalSeleccionDeFiltro.Seleccionar: {
                    modal.SeleccionarElementos();
                    break;
                }
                case Evento.ModalSeleccionDeFiltro.FilaPulsada: {
                    let idCheck: string = parIn[1];
                    let idOrigen: string = parIn[2]; // si se ha pulsado en el check o en la fila
                    modal.FilaPulsada(idCheck, idOrigen);
                    break;
                }
                case Evento.ModalSeleccionDeFiltro.Buscar: {
                    EventosMenuDelGrid(Evento.OpcionesDelGrid.RecargarGrid, parIn[0]);
                    //modal.RecargarGrid();
                    break;
                }
                case Evento.ModalSeleccionDeFiltro.ObtenerSiguientes: {
                    modal.ObtenerSiguientes();
                    break;
                }
                case Evento.ModalSeleccionDeFiltro.ObtenerAnteriores: {
                    modal.ObtenerAnteriores();
                    break;
                }
                case Evento.ModalSeleccionDeFiltro.ObtenerUltimos: {
                    modal.ObtenerUltimos();
                    break;
                }
                case Evento.ModalSeleccionDeFiltro.OrdenarPor: {
                    let columna: string = parIn[1];
                    modal.OrdenarPor(columna);
                    break;
                }
                case Evento.ModalSeleccionDeFiltro.TeclaPulsada: {
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

    export function EventosModalDeCrearRelaciones(accion: string, parametros: string): void {

        let parIn: Array<string> = parametros.split("#");
        let modal: ModalParaRelacionar = crudMnt.ObtenerModalParaRelacionar(parIn[0]);
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
                    EventosMenuDelGrid(Evento.OpcionesDelGrid.RecargarGrid, parIn[0]);
                    //modal.RecargarGrid();
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
                    let columna: string = parIn[1];
                    modal.OrdenarPor(columna);
                    break;
                }
                case Evento.ModalParaRelacionar.FilaPulsada: {
                    let idCheck: string = parIn[1];
                    let idOrigen: string = parIn[2]; // si se ha pulsado en el check o en la fila
                    modal.FilaPulsada(idCheck, idOrigen);
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

    export function EventosModalParaSeleccionar(accion: string, parametros: string): void {

        let parIn: Array<string> = parametros.split("#");
        let modal: ModalParaSeleccionar = crudMnt.ObtenerModalParaSeleccionar(parIn[0]);
        if (modal === undefined) {
            MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `Modal ${parIn[0]} no definida`);
            return;
        }
        try {
            switch (accion) {
                case Evento.ModalParaSeleccionarElementos.FilaPulsada: {
                    let idCheck: string = parIn[1];
                    let idOrigen: string = parIn[2]; // si se ha pulsado en el check o en la fila
                    modal.FilaPulsada(idCheck, idOrigen);
                    break;
                }
                case Evento.ModalParaSeleccionarElementos.Buscar: {
                    EventosMenuDelGrid(Evento.OpcionesDelGrid.RecargarGrid, parIn[0]);
                    //modal.RecargarGrid();
                    break;
                }
                case Evento.ModalParaSeleccionarElementos.ObtenerSiguientes: {
                    modal.ObtenerSiguientes();
                    break;
                }
                case Evento.ModalParaSeleccionarElementos.ObtenerAnteriores: {
                    modal.ObtenerAnteriores();
                    break;
                }
                case Evento.ModalParaSeleccionarElementos.ObtenerUltimos: {
                    modal.ObtenerUltimos();
                    break;
                }
                case Evento.ModalParaSeleccionarElementos.OrdenarPor: {
                    let columna: string = parIn[1];
                    modal.OrdenarPor(columna);
                    break;
                }
                case Evento.ModalParaSeleccionarElementos.Cerrar: {
                    modal.CerrarModalParaSeleccionar();
                    break;
                }
                case Evento.ModalParaSeleccionarElementos.TeclaPulsada: {
                    modal.TeclaPulsada(modal, event);
                    break;
                }
                default: {
                    MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `la opción ${accion} no está definida en el gestor de eventos para seleccionar`);
                    break;
                }
            }
        }
        catch (error) {
            MensajesSe.Error(`Modal de crear relaciones, accion: ${accion}`, error.message);
        }
    }

    export function EventosModalDeConsultaDeRelaciones(accion: string, parametros: string): void {

        let parIn: Array<string> = parametros.split("#");
        let modal: ModalParaConsultarRelaciones = crudMnt.ObtenerModalParaConsultarRelaciones(parIn[0]);
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
                    EventosMenuDelGrid(Evento.OpcionesDelGrid.RecargarGrid, parIn[0]);
                    //modal.RecargarGrid();
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
                    let columna: string = parIn[1];
                    modal.OrdenarPor(columna);
                    break;
                }
                case Evento.ModalParaConsultaDeRelaciones.FilaPulsada: {
                    let idCheck: string = parIn[1];
                    let idOrigen: string = parIn[2]; // si se ha pulsado en el check o en la fila
                    modal.FilaPulsada(idCheck, idOrigen);
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

    export function EventosModalDeBorrar(accion: string): void {
        try {
            switch (accion) {
                case Evento.ModalBorrar.Cerrar: {
                    crudMnt.ModalDeBorrado_Cerrar();
                    break;
                }
                case Evento.ModalBorrar.Borrar: {
                    crudMnt.ModalDeBorrado_Borrar();
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

    export function EventosModalDeCreacion(accion: string): void {
        try {
            switch (accion) {
                case Evento.ModalCreacion.Cerrar: {
                    crudMnt.CerrarModalDeCreacion();
                    break;
                }
                case Evento.ModalCreacion.Crear: {
                    crudMnt.CrearElemento();
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

    export function EventosModalDeEdicion(accion: string): void {
        try {
            switch (accion) {
                case Evento.ModalEdicion.Cerrar: {
                    crudMnt.CerrarModalDeEdicion();
                    break;
                }
                case Evento.ModalEdicion.Modificar: {
                    crudMnt.ModificarElemento();
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

    export function EventosModalDeExportacion(accion: string): void {
        try {
            switch (accion) {
                case Evento.ModalExportacion.Cerrar: {
                    crudMnt.ModalExportacion_Cerrar();
                    break;
                }
                case Evento.ModalExportacion.PulsarSometer: {
                    crudMnt.ModalExportacion_CheckSometerPulsado();
                    break;
                }
                case Evento.ModalExportacion.SalirListaCorreos: {
                    crudMnt.ModalExportacion_SalirDeListaDeCorreos();
                    break;
                }
                case Evento.ModalExportacion.Exportar: {
                    crudMnt.ModalExportacion_Exportar();
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

    export function EventosModalDeEnviarCorreo(accion: string, parametros: string): void {
        let parIn: Array<string> = parametros.split("#");
        try {
            switch (accion) {
                case Evento.ModalEnviarCorreo.Cerrar: {
                    crudMnt.ModalEnviarCorreo_Cerrar();
                    break;
                }
                case Evento.ModalEnviarCorreo.Enviar: {
                    crudMnt.ModalEnviarCorreo_Enviar();
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

    export function EjecutarMenuEdt(accion: string): void {
        try {
            crudMnt.crudDeEdicion.EjecutarAcciones(accion);
        }
        catch (error) {
            MensajesSe.Error(`Edición, accion: ${accion}`, error.message);
        }
    }

    export function EventosDeEdicion(accion: string): void {
        try {
            crudMnt.crudDeEdicion.EjecutarAcciones(accion);
        }
        catch (error) {
            MensajesSe.Error(`Eventos de edición, accion: ${accion}`, error.message);
        }
    }

    export function EjecutarMenuCrt(accion: string): void {
        try {
            crudMnt.crudDeCreacion.EjecutarAcciones(accion);
        }
        catch (error) {
            MensajesSe.Error(`Creacion, accion: ${accion}`, error.message);
        }
    }

    export function EventosDeListaDinamica(accion: string, selector: HTMLInputElement) {
        try {
            switch (accion) {
                case Evento.ListaDinamica.Cargar: {
                    crudMnt.CargarListaDinamica(selector);
                    break;
                }
                case Evento.ListaDinamica.perderFoco: {
                    crudMnt.PerderFocoListaDinamica(selector);
                    break;
                }
                case Evento.ListaDinamica.obtenerFoco: {
                    crudMnt.ObtenerFocoListaDinamica(selector);
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

    export function EventosDeExpansores(accion: string, parametros: string) {
        try {
            switch (accion) {
                case Evento.Expansores.OcultarMostrarBloque: {
                    let partes: string[] = parametros.split(';');
                    if (partes.length != 2)
                        throw Error(`El parametro ${parametros} ha de definir el bloque expansor y el bloque que expande`);
                    let idHtmlExpansor: string = partes[0];
                    let idHtmlBloque: string = partes[1];
                    ApiControl.OcultarMostrarExpansor(idHtmlExpansor, idHtmlBloque);
                    break;
                }
                case Evento.Expansores.NavegarDesdeEdicion: {
                    crudMnt.crudDeEdicion.NavegarDesdeEdicion(parametros);
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

    export function EventosDeSelectorDeElementosEnModal(accion: string, parametros: string) {
        try {
            let parIn: Array<string> = parametros.split("#");
            switch (accion) {
                case Evento.SelectorDeElementos.Seleccionar: {
                    if (parIn.length !== 3)
                        throw new Error(`No se han definido los parámetros de entrada correctos para el evento ${Evento.SelectorDeElementos.Seleccionar}`);
                    crudMnt.AbrirModalParaSeleccionarDesdeUnaModal(parIn[0], parIn[1], parIn[2]);
                    break;
                }
                case Evento.SelectorDeElementos.PerderFoco: {
                    if (parIn.length !== 3)
                        throw new Error(`No se han definido los parámetros de entrada correctos para el evento ${Evento.SelectorDeElementos.Seleccionar}`);
                    crudMnt.PerderElFocoEnUnSelectorDesdeUnaModal(parIn[0], parIn[1], parIn[2]);
                    break;
                }
                case Evento.SelectorDeElementos.ObtenerFoco: {
                    if (parIn.length !== 1)
                        throw new Error(`No se han definido los parámetros de entrada correctos para el evento ${Evento.SelectorDeElementos.Seleccionar}`);
                    crudMnt.ObtenerFocoEnSelector(parIn[0]);
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
}