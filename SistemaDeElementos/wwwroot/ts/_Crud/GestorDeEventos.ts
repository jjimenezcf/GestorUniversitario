namespace Crud {

    export function EventosDelMantenimiento(accion: string, parametros: string): void {
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

    export function EventosModalDeSeleccion(accion: string, parametros: string): void {

        let parIn: Array<string> = parametros.split("#");
        let modal: ModalSeleccion = crudMnt.ObtenerModal(parIn[0]);
        try {
            switch (accion) {
                case LiteralModalSeleccion.Accion.AbrirModalDeSeleccion: {
                    modal.AbrirModalDeSeleccion();
                    break;
                }
                case LiteralModalSeleccion.Accion.CerrarModalDeSeleccion: {
                    modal.CerrarModalDeSeleccion();
                    break;
                }
                case LiteralModalSeleccion.Accion.SeleccionarElementos: {
                    modal.SeleccionarElementos();
                    break;
                }
                case LiteralModalSeleccion.Accion.FilaPulsada: {
                    let idCheck: string = parIn[1];
                    let idOrigen: string = parIn[2]; // si se ha pulsado en el check o en la fila
                    modal.FilaPulsada(idCheck, idOrigen);
                    break;
                }
                case LiteralModalSeleccion.Accion.BuscarElementos: {
                    modal.RecargarGrid();
                    break;
                }
                case LiteralModalSeleccion.Accion.ObtenerSiguientes: {
                    modal.ObtenerSiguientes();
                    break;
                }
                case LiteralModalSeleccion.Accion.ObtenerAnteriores: {
                    modal.ObtenerAnteriores();
                    break;
                }
                case LiteralModalSeleccion.Accion.ObtenerUltimos: {
                    modal.ObtenerUltimos();
                    break;
                }
                case LiteralModalSeleccion.Accion.OrdenarPor: {
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
        switch (accion) {
            case LiteralModalBorrar.Accion.CerrarModalDeBorrado: {
                crudMnt.BorrarElemento();
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
}