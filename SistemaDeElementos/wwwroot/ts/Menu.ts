module Menu {

    export function MostrarMenu() {
        let idProductoHtml: HTMLElement = document.getElementById('id_menu');
        let idModalMenu: string = idProductoHtml.getAttribute('modal-menu');
        let idModalHtml: HTMLElement = document.getElementById(idModalMenu);

        if (idModalHtml === undefined) {
            console.log(`No se ha definido el contenedor del menú ${idModalMenu}`);
        }
        else {
            var menuAbierto = idProductoHtml.getAttribute("menu-abierto");
            if (menuAbierto === undefined || menuAbierto === "false") {
                idProductoHtml.setAttribute("menu-abierto", "true");
                idModalHtml.style.display = "block";
                idModalHtml.style.height = `${document.documentElement.clientHeight - 60}px`;
            }
            else {
                idProductoHtml.setAttribute("menu-abierto", "false");
                idModalHtml.style.display = "none";
            }
        }
    }

    export function OpcionSeleccionada(idVistaMvc: string, controlador: string, accion) {
        let urlBase: string = window.location.origin;
        window.location.href = `${urlBase}/${controlador}/${accion}`;
    }

    export function MenuPulsado(id_menu_pulsado: string) {
        let elementosHtml: NodeListOf<HTMLElement> = document.getElementsByName("menu");
        let menuHtmlPulsado: HTMLMenuElement = document.getElementById(id_menu_pulsado) as HTMLMenuElement;


        if (menuHtmlPulsado.getAttribute("menu-plegado") == "false") {
            plegarMenu(menuHtmlPulsado);
            return;
        }

        for (let i = 0; i < elementosHtml.length; i++)
            plegarMenu(elementosHtml[i] as HTMLMenuElement);

        desplegarMenu(menuHtmlPulsado);

        let padreHtml: HTMLElement = menuHtmlPulsado.parentElement;
        while (padreHtml !== null) {
            if (padreHtml.constructor.toString().indexOf("HTMLUListElement") > 0)
                desplegarMenu(padreHtml as HTMLMenuElement);
            padreHtml = padreHtml.parentElement;
        }

    }

    export function ReqSolicitarMenu(usuario: string, idContenedorMenu: string): void {
        let url: string = `/Menus/${Ajax.EndPoint.SolicitarMenu}?${Ajax.Param.usuario}=${usuario}`;
        let req: XMLHttpRequest = new XMLHttpRequest();
        req.open('GET', url, false);
        PeticionSolicitarMenu(req, () => DespuesDeSolitarMenu(req, idContenedorMenu), () => ErrorAlSolicitarMenu(req));
    }

    function PeticionSolicitarMenu(req: XMLHttpRequest, despuesDeSolitarMenu: Function, errorAlSolicitarMenu: Function) {

        function respuestaCorrecta() {
            if (EsNula(req.response)) {
                errorAlSolicitarMenu();
            }
            else {
                var resultado: any = ParsearRespuesta(req);
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
            Mensaje(TipoMensaje.Error, `No se ha localizado el contenedor ${idContenedorMenu}`);
            return;
        }
        htmlContenedorMenu.innerHTML = resultado.html;
    }

    function ErrorAlSolicitarMenu(req: XMLHttpRequest): void {
        if (EsNula(req.response)) {
            Mensaje(TipoMensaje.Error, `La peticion ${Ajax.EndPoint.SolicitarMenu} no está definida`);
        }
        else {
            let resultado = JSON.parse(req.response);
            Mensaje(TipoMensaje.Error, resultado.mensaje);
            console.error(resultado.consola);
        }
    }

    function desplegarMenu(menuHtml: HTMLMenuElement) {
        menuHtml.style.display = "block";
        menuHtml.compact = false;
        menuHtml.setAttribute("menu-plegado", "false");
    }

    function plegarMenu(menuHtml: HTMLMenuElement) {
        menuHtml.style.display = "none";
        menuHtml.compact = true;
        menuHtml.setAttribute("menu-plegado", "true");
    }
}






