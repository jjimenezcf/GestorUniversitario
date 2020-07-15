module EntornoSe {

    export let Historial: HistorialDeNavegacion = undefined;

    export function IniciarEntorno(usuario: string) {
        ArbolDeMenu.ReqSolicitarMenu(usuario, 'id_contenedormenu');
    }

    export function InicializarHistorial(): boolean {
        if (Historial === undefined) {
            Historial = new HistorialDeNavegacion();
            return true;
        }
        return false;
    }
}