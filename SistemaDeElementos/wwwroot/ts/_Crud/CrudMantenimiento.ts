module Crud.Menu {
    export function EjecutarAccionMenu(accion: string, idDivMostrarHtml: string, idDivOcultarHtml: string, gestor: CrudBase): void {

        if (accion === LiteralMnt.crearelemento)
            IraCrear(gestor as CrudCreacion, idDivMostrarHtml, idDivOcultarHtml);
        else if (accion === LiteralMnt.editarelemento)
            IraEditar(gestor as CrudEdicion, idDivMostrarHtml, idDivOcultarHtml);
        else if (accion === LiteralMnt.nuevoelemento)
            NuevoElemento(gestor as CrudCreacion, idDivMostrarHtml, idDivOcultarHtml);
        else if (accion === LiteralMnt.cancelarnuevo)
            CancelarNuevo(idDivMostrarHtml, idDivOcultarHtml, gestor as CrudCreacion);
        else if (accion === LiteralMnt.cancelaredicion)
            CancelarEdicion(idDivMostrarHtml, idDivOcultarHtml, gestor as CrudEdicion);
        else
            Mensaje(TipoMensaje.Info, `la opción ${accion} no está definida`)
    }

    function IraEditar(gestorDeEdicion: CrudEdicion, idDivMostrarHtml: string, idDivOcultarHtml: string) {
        let htmlDivMostrar: HTMLDivElement = document.getElementById(`${idDivMostrarHtml}`) as HTMLDivElement;
        let htmlDivOcultar: HTMLDivElement = document.getElementById(`${idDivOcultarHtml}`) as HTMLDivElement;

        htmlDivMostrar.classList.add(ClaseCss.divVisible);
        htmlDivMostrar.classList.remove(ClaseCss.divNoVisible);

        htmlDivOcultar.classList.add(ClaseCss.divNoVisible);
        htmlDivOcultar.classList.remove(ClaseCss.divVisible);

        gestorDeEdicion.InicializarValores();
    }

    function IraCrear(gestorDeCreacion: CrudCreacion, idDivMostrarHtml: string, idDivOcultarHtml: string) {
        let htmlDivMostrar: HTMLDivElement = document.getElementById(`${idDivMostrarHtml}`) as HTMLDivElement;
        let htmlDivOcultar: HTMLDivElement = document.getElementById(`${idDivOcultarHtml}`) as HTMLDivElement;

        htmlDivMostrar.classList.add(ClaseCss.divVisible);
        htmlDivMostrar.classList.remove(ClaseCss.divNoVisible);

        htmlDivOcultar.classList.add(ClaseCss.divNoVisible);
        htmlDivOcultar.classList.remove(ClaseCss.divVisible);

        gestorDeCreacion.InicializarValores();
    }

    function NuevoElemento(gestorDeCreacion: CrudCreacion, idDivMostrarHtml: string, idDivOcultarHtml: string) {
        let htmlDivMostrar: HTMLDivElement = document.getElementById(`${idDivMostrarHtml}`) as HTMLDivElement;
        let htmlDivOcultar: HTMLDivElement = document.getElementById(`${idDivOcultarHtml}`) as HTMLDivElement;
        gestorDeCreacion.Aceptar(htmlDivMostrar, htmlDivOcultar);
        if (gestorDeCreacion.Creado) {
            gestorDeCreacion.Cerrar(htmlDivMostrar, htmlDivOcultar)
        }
        Mensaje(gestorDeCreacion.Creado ? TipoMensaje.Info : TipoMensaje.Error, gestorDeCreacion.ResultadoPeticion);
    }

    function CancelarNuevo(idDivMostrarHtml: string, idDivOcultarHtml: string, gestorDeCreacion: CrudCreacion) {
        let htmlDivMostrar: HTMLDivElement = document.getElementById(`${idDivMostrarHtml}`) as HTMLDivElement;
        let htmlDivOcultar: HTMLDivElement = document.getElementById(`${idDivOcultarHtml}`) as HTMLDivElement;
        try {
            gestorDeCreacion.Cerrar(htmlDivMostrar, htmlDivOcultar);
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error.menssage);
        }
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