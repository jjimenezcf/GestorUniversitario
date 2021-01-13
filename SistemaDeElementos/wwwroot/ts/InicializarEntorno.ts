module EntornoSe {

    export let Historial: HistorialSe.HistorialDeNavegacion = undefined;

    export function IniciarEntorno() {
        ArbolDeMenu.ReqSolicitarMenu('id-contenedor-menu');
        window.onpopstate = function (e) {
            console.log(e.state);
        }
    }

    export function AjustarDivs() {
        let alturaDelCuerpo: number =AlturaDelCuerpo();
        let cuerpo: HTMLDivElement = document.getElementById("div-cuerpo") as HTMLDivElement;
        cuerpo.style.height = `${alturaDelCuerpo.toString()}px`;

        let { modalMenu, estadoMenu }: { modalMenu: HTMLDivElement; estadoMenu: HTMLElement; } = ArbolDeMenu.ObtenerDatosMenu();
        if (estadoMenu.getAttribute(atMenu.abierto) === literal.true)
            modalMenu.style.height = `${AlturaDelMenu().toString()}px`;
    }

    export function InicializarHistorial() {
            Historial = new HistorialSe.HistorialDeNavegacion();
    }

    export function NavegarAUrl(url: string) {
        PonerCapa();
        Historial.Persistir();
        window.location.href = url;
    }
}