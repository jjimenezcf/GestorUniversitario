var Menu;
(function (Menu) {
    function MostrarMenu() {
        var idProductoHtml = document.getElementById('id_menu');
        var idModalMenu = idProductoHtml.getAttribute('modal-menu');
        var idModalHtml = document.getElementById(idModalMenu);
        if (idModalHtml === undefined) {
            console.log("No se ha definido el contenedor del men\u00FA " + idModalMenu);
        }
        else {
            var menuAbierto = idProductoHtml.getAttribute("menu-abierto");
            if (menuAbierto === undefined || menuAbierto === "false") {
                idProductoHtml.setAttribute("menu-abierto", "true");
                idModalHtml.style.display = "block";
                idModalHtml.style.height = document.documentElement.clientHeight - 60 + "px";
            }
            else {
                idProductoHtml.setAttribute("menu-abierto", "false");
                idModalHtml.style.display = "none";
            }
        }
    }
    Menu.MostrarMenu = MostrarMenu;
    function OpcionSeleccionada(idVistaMvc, controlador, accion) {
        var urlBase = window.location.origin;
        window.location.href = urlBase + "/" + controlador + "/" + accion;
    }
    Menu.OpcionSeleccionada = OpcionSeleccionada;
    function MenuPulsado(id_menu_pulsado) {
        var elementosHtml = document.getElementsByName("menu");
        var menuHtmlPulsado = document.getElementById(id_menu_pulsado);
        if (menuHtmlPulsado.getAttribute("menu-plegado") == "false") {
            plegarMenu(menuHtmlPulsado);
            return;
        }
        for (var i = 0; i < elementosHtml.length; i++)
            plegarMenu(elementosHtml[i]);
        desplegarMenu(menuHtmlPulsado);
        var padreHtml = menuHtmlPulsado.parentElement;
        while (padreHtml !== null) {
            if (padreHtml.constructor.toString().indexOf("HTMLUListElement") > 0)
                desplegarMenu(padreHtml);
            padreHtml = padreHtml.parentElement;
        }
    }
    Menu.MenuPulsado = MenuPulsado;
    function SolicitarMenu(idContenedorMenu, usuario) {
        var url = urlPeticion(usuario);
        LeeMenu(url, idContenedorMenu, SustituirMenu);
    }
    Menu.SolicitarMenu = SolicitarMenu;
    function LeeMenu(url, idContenedorMenu, funcionDeRespuesta) {
        function respuestaCorrecta() {
            if (req.status >= 200 && req.status < 400) {
                funcionDeRespuesta(idContenedorMenu, req.responseText);
            }
            else {
                console.log(req.status + ' ' + req.statusText);
            }
        }
        function respuestaErronea() {
            console.log('Error de conexión');
        }
        var req = new XMLHttpRequest();
        req.open('GET', url, true);
        req.addEventListener("load", respuestaCorrecta);
        req.addEventListener("error", respuestaErronea);
        req.send();
    }
    function SustituirMenu(idContenedorMenu, htmlMenu) {
        var htmlContenedorMenu = document.getElementById("" + idContenedorMenu);
        if (!htmlContenedorMenu) {
            console.log("No se ha localizado el contenedor " + idContenedorMenu);
            return;
        }
        htmlContenedorMenu.innerHTML = htmlMenu;
    }
    function urlPeticion(usuario) {
        var url = "/Menus/RenderMenu?usuario=" + usuario;
        return url;
    }
    function desplegarMenu(menuHtml) {
        menuHtml.style.display = "block";
        menuHtml.compact = false;
        menuHtml.setAttribute("menu-plegado", "false");
    }
    function plegarMenu(menuHtml) {
        menuHtml.style.display = "none";
        menuHtml.compact = true;
        menuHtml.setAttribute("menu-plegado", "true");
    }
})(Menu || (Menu = {}));
//# sourceMappingURL=tsMenu.js.map