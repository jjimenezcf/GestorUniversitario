module Menu {
    export function MostrarMenu() {
        let idProductoHtml: HTMLElement = document.getElementById('idproducto');
        let idModalMenu: string = idProductoHtml.getAttribute('menu');
        let idModalHtml: HTMLElement = document.getElementById(idModalMenu);

        if (idModalHtml === undefined) {
            console.log(`No se ha definido el contenedor del menú ${idModalMenu}`);
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

    export function OpcionSeleccionada(opcion: string) {
        let urlBase: string = window.location.origin;
        window.location.href = `${urlBase}/${opcion}`;
    }





}
