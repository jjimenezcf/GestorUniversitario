
module EntornoSe {

    export class AlmacenDeMensajes {
        private _id: string;
        private _errores: string[] = [];
        private _mensajes: string[] = [];

        constructor(id: string) {
            this._id = id;
        }

        public Error(mensaje: string, mensajeDeConsola?: string): number {
            Mensaje(TipoMensaje.Error, mensaje, mensajeDeConsola);
            this._errores.push(mensaje);
            return this._errores.length;
        }

        public Info(mensaje: string, mensajeDeConsola?: string): number {
            Mensaje(TipoMensaje.Info, mensaje, mensajeDeConsola);
            this._mensajes.push(mensaje);
            return this._mensajes.length;
        }
    }

    export let Historial: HistorialSe.HistorialDeNavegacion = undefined;

    export function IniciarEntorno() {
        ArbolDeMenu.ReqSolicitarMenu('id-contenedor-menu');
        window.onpopstate = function (e) {
            console.log(e.state);
        };
        AjustarDivs();
    }

    export function AjustarDivs() {
        let altura: number = AlturaFormulario();

        let alturaDelCuerpo: number = AlturaDelCuerpo(altura);
        let cuerpo: HTMLDivElement = document.getElementById(LiteralMnt.idCuerpoDePagina) as HTMLDivElement;
        cuerpo.style.height = `${alturaDelCuerpo.toString()}px`;

        let { modalMenu, estadoMenu }: { modalMenu: HTMLDivElement; estadoMenu: HTMLElement; } = ArbolDeMenu.ObtenerDatosMenu();
        if (estadoMenu.getAttribute(atMenu.abierto) === literal.true)
            modalMenu.style.height = `${AlturaDelMenu(altura).toString()}px`;

        if (Crud.crudMnt !== null) {
            Crud.crudMnt.PosicionarPanelesDelCuerpo();
        }
        else {
            Mensaje(TipoMensaje.Info, "No hay crud");
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