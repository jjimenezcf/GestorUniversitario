module Menu {
    export function Mostrar() {
        var idProductoHtml = document.getElementById('idproducto');
        var menuAbierto = idProductoHtml.getAttribute("menuAbierto");
        if (menuAbierto === undefined || menuAbierto === "false") {
            idProductoHtml.setAttribute("menuAbierto", "true");
            document.getElementById("sidebar").style.width = "300px";
        }
        else {
            idProductoHtml.setAttribute("menuAbierto", "false");
            document.getElementById("sidebar").style.width = "0";
            document.getElementById("contenido").style.marginLeft = "0";
        }
    }
}
