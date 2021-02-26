
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

    export const misCookies = {
        UsuarioConectado : 'usuario-conectado'
    }

    export function LeerCookie(nombre): any {
        let lista: string[] = document.cookie.split(";");
        let micookie: string = "";
        let valor: string = "";
        for (let i: number = 0; i < lista.length; i++) {
            var busca = lista[i].search(nombre);
            if (busca > -1) {
                micookie = lista[i];
                break;
            }
        }
        if (!IsNullOrEmpty(micookie)) {
            var igual = micookie.indexOf("=");
            valor = micookie.substring(igual + 1);
        }
        return IsNullOrEmpty(valor) ? null : JSON.parse(valor);
    }

    export function MostrarHistorial()
    {
        let modal: HTMLDivElement = document.getElementById("id-modal-historial") as HTMLDivElement

        if (!EsTrue(modal.getAttribute('abierto'))) {
            modal.style.display = "block";
            modal.style.height = `${AlturaDelMenu(AlturaFormulario()).toString()}px`;
        }
    }


    export function GuardarCookie(nombre, valor): void {
        document.cookie = `${nombre}=${JSON.stringify(valor)}`;
    }

    export function LeerUsuarioDeConexion(llamador: any): Promise<ApiDeAjax.DescriptorAjax> {

        function RegistrarCookie(peticion: ApiDeAjax.DescriptorAjax) {
            let registro: any = peticion.resultado.datos;
            EntornoSe.GuardarCookie(EntornoSe.misCookies.UsuarioConectado, registro);
        }
        return new Promise((resolve, reject) => {

            let url: string = `/${Ajax.Usuarios.ruta}/${Ajax.Usuarios.accion.LeerUsuarioDeConexion}`;

            let a = new ApiDeAjax.DescriptorAjax(llamador
                , Ajax.Usuarios.accion.LeerUsuarioDeConexion
                , llamador
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , (peticion) => {
                    RegistrarCookie(peticion);
                    resolve(peticion);
                }
                , (peticion) => {
                    reject(peticion);
                }
            );
            a.Ejecutar();
        });
    }

}