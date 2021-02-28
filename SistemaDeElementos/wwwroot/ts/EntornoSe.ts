
module EntornoSe {

    export let Historial: HistorialSe.HistorialDeNavegacion = undefined;

    export function IniciarEntorno() {
        AjustarDivs();
        Registro.RegistrarUsuarioDeConexion(this)
            .then((usuarioConectado) => {
                ArbolDeMenu.ReqSolicitarMenu('id-contenedor-menu');
            }
            )
            .catch(() => {
                Notificar(TipoMensaje.Error, "Error al leer el usuario de conexión");
            });
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
            Notificar(TipoMensaje.Info, "No hay crud");
        }

        let modal: HTMLDivElement = document.getElementById("id-modal-historial") as HTMLDivElement;
        if (modal.style.display === "block") {
            modal.style.height = `${altura.toString()}px`;
        }
    }

    export function InicializarHistorial() {
        Historial = new HistorialSe.HistorialDeNavegacion();
    }

    export function NavegarAUrl(url: string) {
        PonerCapa();
        try {
            EntornoSe.Historial.Persistir();
        }
        catch (error) {
            QuitarCapa();
            throw error;
        }
        window.location.href = url;
    }

    export function Sumit(form: HTMLFormElement) {
        PonerCapa();
        try {
            EntornoSe.Historial.Persistir();
            form.submit();
        }
        catch (error) {
            QuitarCapa();
            throw error;
        }
    }

    export function MostrarHistorial() {
        let altura: number = AlturaFormulario();
        let modal: HTMLDivElement = document.getElementById("id-modal-historial") as HTMLDivElement;
        modal.style.display = "block";
        modal.style.height = `${altura.toString()}px`;
    }

    export function CerrarHistorial() {
        let modal: HTMLDivElement = document.getElementById("id-modal-historial") as HTMLDivElement;
        modal.style.display = "none";
    }

    export function BorrarHistorial() {
        let modal: HTMLDivElement = document.getElementById("id-modal-historial") as HTMLDivElement;
        //recorrer las lineas del body y borrar las
    }

    export function Llamador(): string {
        var callerName;
        try { throw new Error(); }
        catch (e) {
            var re = /(\w+)@|at (\w+) \(/g, st = e.stack, m;
            re.exec(st), m = re.exec(st);
            callerName = m[1] || m[2];
        }
        return callerName;
    };

}