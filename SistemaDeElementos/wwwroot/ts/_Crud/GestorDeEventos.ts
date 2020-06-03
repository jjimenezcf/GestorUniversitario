namespace Crud {

    export function EventosDelMantenimiento(accion: string, parametros: string): void {
        try {
            switch (accion) {
                case LiteralMnt.Accion.CrearElemento: {
                    crudMnt.IraCrear();
                    break;
                }
                case LiteralMnt.Accion.EditarElemento: {
                    crudMnt.IraEditar();
                    break;
                }
                case LiteralMnt.Accion.EliminarElemento: {
                    crudMnt.AbrirModalBorrarElemento();
                    break;
                }
                case LiteralMnt.Accion.BuscarElementos: {
                    crudMnt.Buscar(0);
                    break;
                }
                case LiteralMnt.Accion.ObtenerSiguientes: {
                    crudMnt.ObtenerSiguientes();
                    break;
                }
                case LiteralMnt.Accion.ObtenerAnteriores: {
                    crudMnt.ObtenerAnteriores();
                    break;
                }
                case LiteralMnt.Accion.ObtenerUltimos: {
                    crudMnt.ObtenerUltimos();
                    break;
                }
                case LiteralMnt.Accion.FilaPulsada: {
                    let parIn: Array<string> = parametros.split("#");
                    crudMnt.FilaPulsada(parIn[0], parIn[1]);
                    break;
                }
                case LiteralMnt.Accion.OrdenarPor: {
                    crudMnt.OrdenarPor(parametros);
                    break;
                }
                case LiteralMnt.Accion.CambiarSelector: {
                    crudMnt.CambiarSelector(parametros);
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

    export function EventosModalDeSeleccion(accion: string, parametros: string): void {

        let parIn: Array<string> = parametros.split("#");
        let modal: ModalSeleccion = crudMnt.ObtenerModal(parIn[0]);
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
                    let idCheck: string = parIn[1];
                    let idOrigen: string = parIn[2]; // si se ha pulsado en el check o en la fila
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
                    let columna: string = parIn[1];
                    modal.OrdenarPor(columna);
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

    export function EventosModalDeBorrar(accion: string): void {
        try {
            switch (accion) {
                case LiteralModalBorrar.Accion.CerrarModalDeBorrado: {
                    crudMnt.CerrarModalDeBorrado();
                    break;
                }
                case LiteralModalBorrar.Accion.BorrarElemento: {
                    crudMnt.BorrarElemento();
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

    export function EventosModalDeCreacion(accion: string): void {
        try {
            switch (accion) {
                case LiteralModalCreacion.Accion.CerrarModal: {
                    crudMnt.CerrarModalDeCreacion();
                    break;
                }
                case LiteralModalCreacion.Accion.CrearElemento: {
                    crudMnt.CrearElemento();
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

    export function EventosModalDeEdicion(accion: string): void {
        try {
            switch (accion) {
                case LiteralModalEdicion.Accion.CerrarModal: {
                    crudMnt.CerrarModalDeEdicion();
                    break;
                }
                case LiteralModalEdicion.Accion.ModificarElemento: {
                    crudMnt.ModificarElemento();
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

    export function EjecutarMenuEdt(accion: string): void {
        try {
            crudMnt.crudDeEdicion.EjecutarAcciones(accion);
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
        }
    }

    export function EventosDeEdicion(accion: string): void {
        try {
            crudMnt.crudDeEdicion.EjecutarAcciones(accion);
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
        }
    }

    export function EjecutarMenuCrt(accion: string): void {
        try {
            crudMnt.crudDeCreacion.EjecutarAcciones(accion);
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
        }
    }

    export function ListaDeElementos(accion: string, selector: HTMLInputElement) {

        try {
            switch (accion) {
                case Evento.ListaDinamica.Cargar: {
                    crudMnt.CargarListaDinamica(selector);
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


}