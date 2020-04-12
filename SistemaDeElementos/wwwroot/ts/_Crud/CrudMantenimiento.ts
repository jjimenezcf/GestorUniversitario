module Crud.MenuMnt {
    export function EjecutarAccionMenu(accion: string, idDivMostrarHtml: string, idDivOcultarHtml: string, gestorDeCreacion: CrudCreacion): void {

        if (accion === "iracrear")
            IraCrear(idDivMostrarHtml, idDivOcultarHtml, gestorDeCreacion);

    }

    function IraCrear(idDivMostrarHtml: string, idDivOcultarHtml: string, gestorDeCreacion: CrudCreacion) {
        var htmlDivMostrar = document.getElementById(`${idDivMostrarHtml}`);
        var htmlDivOcultar = document.getElementById(`${idDivOcultarHtml}`);

        htmlDivMostrar.classList.add("div-visible");
        htmlDivMostrar.classList.remove("div-no-visible");

        htmlDivOcultar.classList.add("div-no-visible");
        htmlDivOcultar.classList.remove("div-visible");

        gestorDeCreacion.InicializarValores();
    }
}