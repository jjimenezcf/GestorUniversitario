var Menu;
(function (Menu) {
    function MostrarMenu() {
        var idProductoHtml = document.getElementById('idproducto');
        var idModalMenu = idProductoHtml.getAttribute('menu');
        var idModalHtml = document.getElementById(idModalMenu);
        if (idModalHtml === undefined) {
            console.log("No se ha definido el contenedor del men\u00FA " + idModalMenu);
        }
        else {
            var menuAbierto = idProductoHtml.getAttribute("menuAbierto");
            if (menuAbierto === undefined || menuAbierto === "false") {
                idProductoHtml.setAttribute("menuAbierto", "true");
                idModalHtml.style.display = "block";
            }
            else {
                idProductoHtml.setAttribute("menuAbierto", "false");
                idModalHtml.style.display = "none";
            }
        }
    }
    Menu.MostrarMenu = MostrarMenu;
    function OpcionSeleccionada(opcion) {
        var urlBase = window.location.origin;
        window.location.href = urlBase + "/" + opcion;
    }
    Menu.OpcionSeleccionada = OpcionSeleccionada;
})(Menu || (Menu = {}));
//# sourceMappingURL=tsMenu.js.map