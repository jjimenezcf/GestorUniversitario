module Crud.MenuMnt {
    export function EjecutarAccionMenu(accion: string, idDivMostrarHtml: string, idDivOcultarHtml: string, funcionDeMostrar: Function): void {

        if (accion === "iracrear")
            IraCrear(idDivMostrarHtml, idDivOcultarHtml, funcionDeMostrar);

    }

    function IraCrear(idDivMostrarHtml: string, idDivOcultarHtml: string, funcionDeMostrar: Function) {
        var htmlDivMostrar = document.getElementById(`${idDivMostrarHtml}`);
        var htmlDivOcultar = document.getElementById(`${idDivOcultarHtml}`);

        htmlDivMostrar.classList.add("div-visible");
        htmlDivMostrar.classList.remove("div-no-visible");

        htmlDivOcultar.classList.add("div-no-visible");
        htmlDivOcultar.classList.remove("div-visible");

        Crud.Crear.AlMostrar(funcionDeMostrar);
    }
}