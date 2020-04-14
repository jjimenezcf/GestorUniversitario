module Crud.Menu {
    export function EjecutarAccionMenu(accion: string, idDivMostrarHtml: string, idDivOcultarHtml: string, gestorDeCreacion: CrudCreacion): void {

        if (accion === "iracrear")
            IraCrear(idDivMostrarHtml, idDivOcultarHtml, gestorDeCreacion);
        else
        if (accion === "nuevoelemento")
                NuevoElemento(idDivMostrarHtml, idDivOcultarHtml, gestorDeCreacion);
        else
        if (accion === "cancelarnuevo")
           CancelarNuevo(idDivMostrarHtml, idDivOcultarHtml, gestorDeCreacion);
        else
            Mensaje(TipoMensaje.Info, `la opción ${accion} no está definida`)
    }


    function sleep(ms: number) {
        return setTimeout(() => { }, ms);
    }

    function IraCrear(idDivMostrarHtml: string, idDivOcultarHtml: string, gestorDeCreacion: CrudCreacion) {
        let htmlDivMostrar: HTMLDivElement = document.getElementById(`${idDivMostrarHtml}`) as HTMLDivElement;
        let htmlDivOcultar: HTMLDivElement = document.getElementById(`${idDivOcultarHtml}`) as HTMLDivElement;

        htmlDivMostrar.classList.add("div-visible");
        htmlDivMostrar.classList.remove("div-no-visible");

        htmlDivOcultar.classList.add("div-no-visible");
        htmlDivOcultar.classList.remove("div-visible");

        gestorDeCreacion.InicializarValores();
    }


    function NuevoElemento(idDivMostrarHtml: string, idDivOcultarHtml: string, gestorDeCreacion: CrudCreacion) {
        let htmlDivMostrar: HTMLDivElement = document.getElementById(`${idDivMostrarHtml}`) as HTMLDivElement;
        let htmlDivOcultar: HTMLDivElement = document.getElementById(`${idDivOcultarHtml}`) as HTMLDivElement;
        let creado: boolean = true;
        let mensaje: string = "";
        try {
            gestorDeCreacion.Aceptar(htmlDivMostrar, htmlDivOcultar);

            do
            {
                sleep(1000000);
            }
            while (!gestorDeCreacion.PeticionRealizada);

            if (gestorDeCreacion.MensajeDeError.IsNullOrEmpty())
                throw Error(gestorDeCreacion.MensajeDeError);

            mensaje = gestorDeCreacion.ResultadoPeticion;
        }
        catch (error) {
            creado = false;
            mensaje = error.message;
        }
        finally {
            Mensaje(creado ? TipoMensaje.Info : TipoMensaje.Error, mensaje);
        }
    }


    function CancelarNuevo(idDivMostrarHtml: string, idDivOcultarHtml: string, gestorDeCreacion: CrudCreacion) {
        let htmlDivMostrar: HTMLDivElement = document.getElementById(`${idDivMostrarHtml}`) as HTMLDivElement;
        let htmlDivOcultar: HTMLDivElement = document.getElementById(`${idDivOcultarHtml}`) as HTMLDivElement;
        try {
            gestorDeCreacion.Cerrar(htmlDivMostrar, htmlDivOcultar);
        }
        catch (error) {
            console.error(error.message);
            Mensaje(TipoMensaje.Info, error.menssage);
            return;
        }
    }
}