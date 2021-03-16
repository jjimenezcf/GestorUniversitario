var ArbolDeMenu;
(function (ArbolDeMenu) {
    function ObtenerDatosMenu() {
        let estadoMenu = document.getElementById('id-menu');
        let idModalMenu = estadoMenu.getAttribute('modal-menu');
        let modalMenu = document.getElementById(idModalMenu);
        return { modalMenu, estadoMenu };
    }
    ArbolDeMenu.ObtenerDatosMenu = ObtenerDatosMenu;
    function MostrarMenu() {
        let { modalMenu, estadoMenu } = ObtenerDatosMenu();
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
    ArbolDeMenu.MostrarMenu = MostrarMenu;
    function OpcionSeleccionada(idVistaMvc, controlador, accion) {
        MostrarMenu();
        let urlBase = window.location.origin;
        EntornoSe.NavegarAUrl(`${urlBase}/${controlador}/${accion}`);
    }
    ArbolDeMenu.OpcionSeleccionada = OpcionSeleccionada;
    function MenuPulsado(id_menu_pulsado) {
        let menuHtmlPulsado = document.getElementById(id_menu_pulsado);
        if (menuHtmlPulsado.getAttribute(atMenu.plegado) == literal.false) {
            plegarMenu(menuHtmlPulsado);
            return;
        }
        desplegarMenu(menuHtmlPulsado);
        let padreHtml = menuHtmlPulsado.parentElement;
        while (padreHtml !== null) {
            if (padreHtml.constructor.toString().indexOf("HTMLUListElement") > 0)
                desplegarMenu(padreHtml);
            padreHtml = padreHtml.parentElement;
        }
    }
    ArbolDeMenu.MenuPulsado = MenuPulsado;
    function ReqSolicitarMenu(idContenedorMenu) {
        let url = `/ArbolDeMenu/${Ajax.EndPoint.SolicitarMenuEnHtml}`; //?${Ajax.Param.usuario}=${usuario}
        let req = new XMLHttpRequest();
        req.open('GET', url, true);
        PeticionSolicitarMenu(req, Ajax.EndPoint.SolicitarMenuEnHtml, () => DespuesDeSolitarMenu(req, idContenedorMenu), () => ErrorAlSolicitarMenu(req));
    }
    ArbolDeMenu.ReqSolicitarMenu = ReqSolicitarMenu;
    function PeticionSolicitarMenu(req, peticion, despuesDeSolitarMenu, errorAlSolicitarMenu) {
        function respuestaCorrecta() {
            if (IsNullOrEmpty(req.response)) {
                errorAlSolicitarMenu();
            }
            else {
                var resultado = ParsearRespuesta(req, peticion);
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
    function DespuesDeSolitarMenu(req, idContenedorMenu) {
        let resultado = JSON.parse(req.response);
        var htmlContenedorMenu = document.getElementById(`${idContenedorMenu}`);
        if (!htmlContenedorMenu) {
            MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `No se ha localizado el contenedor ${idContenedorMenu}`);
            return;
        }
        htmlContenedorMenu.innerHTML = resultado.html;
    }
    function ErrorAlSolicitarMenu(req) {
        if (IsNullOrEmpty(req.response)) {
            MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `La peticion ${Ajax.EndPoint.SolicitarMenuEnHtml} no está definida`);
        }
        else {
            let resultado = JSON.parse(req.response);
            MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, resultado.mensaje);
            console.error(resultado.consola);
        }
    }
    function ParsearRespuesta(req, peticion) {
        var resultado;
        try {
            resultado = JSON.parse(req.response);
        }
        catch {
            MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `Error al procesar la respuesta de ${peticion}`);
            return undefined;
        }
        return resultado;
    }
    function desplegarMenu(menuHtml) {
        menuHtml.style.display = "block";
        menuHtml.compact = false;
        menuHtml.setAttribute(atMenu.plegado, literal.false);
    }
    function plegarMenu(menuHtml) {
        menuHtml.style.display = "none";
        menuHtml.compact = true;
        menuHtml.setAttribute(atMenu.plegado, literal.true);
    }
})(ArbolDeMenu || (ArbolDeMenu = {}));
//# sourceMappingURL=ArbolDeMenu.js.map