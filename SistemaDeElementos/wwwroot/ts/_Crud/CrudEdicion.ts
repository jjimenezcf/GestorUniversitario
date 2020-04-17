namespace Crud {

    export class CrudEdicion extends Crud.Base.CrudBase {

        constructor(idPanelMnt: string, idPanelCreacion: string) {
            super(idPanelMnt, null, idPanelCreacion);
        }

        InicializarValores() {
            Mensaje(TipoMensaje.Info, "He de leer el registro con ID pasado");
        }

    }

    export function EjecutarMenuEdt(accion: string, idDivMostrarHtml: string, idDivOcultarHtml: string, gestor: Crud.Base.CrudBase): void {

        if (accion === LiteralEdt.cancelaredicion)
            CancelarEdicion(idDivMostrarHtml, idDivOcultarHtml, gestor as CrudEdicion);
        else
            Mensaje(TipoMensaje.Info, `la opción ${accion} no está definida`);
    }

    function CancelarEdicion(idDivMostrarHtml: string, idDivOcultarHtml: string, gestorDeEdicion: CrudEdicion) {
        let htmlDivMostrar: HTMLDivElement = document.getElementById(`${idDivMostrarHtml}`) as HTMLDivElement;
        let htmlDivOcultar: HTMLDivElement = document.getElementById(`${idDivOcultarHtml}`) as HTMLDivElement;
        try {
            gestorDeEdicion.Cerrar(htmlDivMostrar, htmlDivOcultar);
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error.menssage);
        }
    }
}