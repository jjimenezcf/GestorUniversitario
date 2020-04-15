module Crud.Menu {
    export function EjecutarAccionMenu(accion: string, idDivMostrarHtml: string, idDivOcultarHtml: string, gestorDeCreacion: CrudCreacion): void {

        if (accion === "iracrear")
            IraCrear(gestorDeCreacion, idDivMostrarHtml, idDivOcultarHtml);
        else
        if (accion === "nuevoelemento")
            NuevoElemento(gestorDeCreacion, idDivMostrarHtml, idDivOcultarHtml);
        else
        if (accion === "cancelarnuevo")
           CancelarNuevo(idDivMostrarHtml, idDivOcultarHtml, gestorDeCreacion);
        else
            Mensaje(TipoMensaje.Info, `la opción ${accion} no está definida`)
    }


    function IraCrear(gestorDeCreacion: CrudCreacion, idDivMostrarHtml: string, idDivOcultarHtml: string) {
        let htmlDivMostrar: HTMLDivElement = document.getElementById(`${idDivMostrarHtml}`) as HTMLDivElement;
        let htmlDivOcultar: HTMLDivElement = document.getElementById(`${idDivOcultarHtml}`) as HTMLDivElement;

        htmlDivMostrar.classList.add("div-visible");
        htmlDivMostrar.classList.remove("div-no-visible");

        htmlDivOcultar.classList.add("div-no-visible");
        htmlDivOcultar.classList.remove("div-visible");

        gestorDeCreacion.InicializarValores();
    }


    async function NuevoElemento(gestorDeCreacion: CrudCreacion, idDivMostrarHtml: string, idDivOcultarHtml: string) {
        Mensaje(TipoMensaje.Info, "Lamando");
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