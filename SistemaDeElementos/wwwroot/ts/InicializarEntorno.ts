module EntornoSe {

    export let Historial: HistorialSe.HistorialDeNavegacion = undefined;

    export function IniciarEntorno(usuario: string) {
        ArbolDeMenu.ReqSolicitarMenu(usuario, 'id-contenedor-menu');
        window.onpopstate = function (e) {
            console.log(e.state);
        }
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