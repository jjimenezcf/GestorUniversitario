﻿module ArbolDeMenu {

    export function ObtenerDatosMenu(): { modalMenu: HTMLDivElement; estadoMenu: HTMLElement} {
        let estadoMenu: HTMLImageElement = document.getElementById('id-menu') as HTMLImageElement;
        let idModalMenu: string = estadoMenu.getAttribute('modal-menu');
        let modalMenu: HTMLDivElement = document.getElementById(idModalMenu) as HTMLDivElement;
        return { modalMenu, estadoMenu };
    }

    export function MostrarMenu() {
        let { modalMenu, estadoMenu }: { modalMenu: HTMLDivElement; estadoMenu: HTMLElement; } = ObtenerDatosMenu();

        var menuAbierto = estadoMenu.getAttribute(atMenu.abierto);
        if (menuAbierto === undefined || menuAbierto === literal.false) {
            estadoMenu.setAttribute(atMenu.abierto, literal.true);
            modalMenu.style.display = "block";
            modalMenu.style.height = `${AlturaDelMenu(AlturaFormulario()).toString()}px`;
        }
        else {
            estadoMenu.setAttribute(atMenu.abierto, literal.false);
            modalMenu.style.display = "none";
        }

    }

    export function OpcionSeleccionada(idVistaMvc: string, controlador: string, accion) {
        MostrarMenu();
        let urlBase: string = window.location.origin;
        EntornoSe.NavegarAUrl(`${urlBase}/${controlador}/${accion}`);
    }

    export function MenuPulsado(id_menu_pulsado: string) {
        let menuHtmlPulsado: HTMLMenuElement = document.getElementById(id_menu_pulsado) as HTMLMenuElement;


        if (menuHtmlPulsado.getAttribute(atMenu.plegado) == literal.false) {
            plegarMenu(menuHtmlPulsado);
            return;
        }

        desplegarMenu(menuHtmlPulsado);

        let padreHtml: HTMLElement = menuHtmlPulsado.parentElement;
        while (padreHtml !== null) {
            if (padreHtml.constructor.toString().indexOf("HTMLUListElement") > 0)
                desplegarMenu(padreHtml as HTMLMenuElement);
            padreHtml = padreHtml.parentElement;
        }

    }

    export function ReqSolicitarMenu(idContenedorMenu: string): void {
        let url: string = `/ArbolDeMenu/${Ajax.EndPoint.SolicitarMenuEnHtml}`; //?${Ajax.Param.usuario}=${usuario}
        let req: XMLHttpRequest = new XMLHttpRequest();
        req.open('GET', url, true);
        PeticionSolicitarMenu(req, Ajax.EndPoint.SolicitarMenuEnHtml, () => DespuesDeSolitarMenu(req, idContenedorMenu), () => ErrorAlSolicitarMenu(req));
    }

    function PeticionSolicitarMenu(req: XMLHttpRequest, peticion: string, despuesDeSolitarMenu: Function, errorAlSolicitarMenu: Function) {

        function respuestaCorrecta() {
            if (IsNullOrEmpty(req.response)) {
                errorAlSolicitarMenu();
            }
            else {
                var resultado: any = ParsearRespuesta(req, peticion);
                if (resultado !== undefined) {
                    if (resultado.estado === Ajax.jsonResultError) {
                        errorAlSolicitarMenu();
                    }
                    else {
                        despuesDeSolitarMenu();
                    }
                }
            }
        }

        function respuestaErronea() {
            errorAlSolicitarMenu();
        }

        req.addEventListener(Ajax.eventoLoad, respuestaCorrecta);
        req.addEventListener(Ajax.eventoError, respuestaErronea);
        req.send();
    }

    function DespuesDeSolitarMenu(req: XMLHttpRequest, idContenedorMenu: string): void {
        let resultado = JSON.parse(req.response);
        var htmlContenedorMenu = document.getElementById(`${idContenedorMenu}`);
        if (!htmlContenedorMenu) {
            MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `No se ha localizado el contenedor ${idContenedorMenu}`);
            return;
        }
        htmlContenedorMenu.innerHTML = resultado.html;
    }

    function ErrorAlSolicitarMenu(req: XMLHttpRequest): void {
        if (IsNullOrEmpty(req.response)) {
            MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `La peticion ${Ajax.EndPoint.SolicitarMenuEnHtml} no está definida`);
        }
        else {
            let resultado = JSON.parse(req.response);
            MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, resultado.mensaje);
            console.error(resultado.consola);
        }
    }


    function ParsearRespuesta(req: XMLHttpRequest, peticion: string): ApiDeAjax.ResultadoJson {
        var resultado: any;
        try {
            resultado = JSON.parse(req.response);
        }
        catch
        {
            MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `Error al procesar la respuesta de ${peticion}`);
            return undefined;
        }
        return resultado;
    }

    function desplegarMenu(menuHtml: HTMLMenuElement) {
        menuHtml.style.display = "block";
        menuHtml.compact = false;
        menuHtml.setAttribute(atMenu.plegado, literal.false);
    }

    function plegarMenu(menuHtml: HTMLMenuElement) {
        menuHtml.style.display = "none";
        menuHtml.compact = true;
        menuHtml.setAttribute(atMenu.plegado, literal.true);
    }

}






