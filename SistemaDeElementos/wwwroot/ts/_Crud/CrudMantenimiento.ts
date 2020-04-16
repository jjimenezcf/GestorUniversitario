module Crud.Menu {
    export function EjecutarAccionMenu(accion: string, idDivMostrarHtml: string, idDivOcultarHtml: string, gestorDeCreacion: CrudCreacion): void {

        if (accion === LiteralMnt.iracrear)
            IraCrear(gestorDeCreacion, idDivMostrarHtml, idDivOcultarHtml);
        else
            if (accion === LiteralMnt.nuevoelemento)
            NuevoElemento(gestorDeCreacion, idDivMostrarHtml, idDivOcultarHtml);
        else
                if (accion === LiteralMnt.cancelarnuevo)
           CancelarNuevo(idDivMostrarHtml, idDivOcultarHtml, gestorDeCreacion);
        else
            Mensaje(TipoMensaje.Info, `la opción ${accion} no está definida`)
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


    async function NuevoElemento(gestorDeCreacion: CrudCreacion, idDivMostrarHtml: string, idDivOcultarHtml: string) {
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
}