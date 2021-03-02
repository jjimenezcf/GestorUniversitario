
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

        EntornoSe.AjustarModalesAbiertas();
    }

    export function AjustarModalesAbiertas() {
        let modales = document.getElementsByClassName(ClaseCss.contenedorModal);
        for (let i = 0; i < modales.length; i++) {
            let modal: HTMLDivElement = modales[i] as HTMLDivElement;
            if (modal.style.display === 'block') {
                let alturaCrud: number = AlturaFormulario() - AlturaCabeceraPnlControl();
                AjustarModal(modales[i] as HTMLDivElement, alturaCrud);
            }
        }
    }

    function AjustarModal(modal: HTMLDivElement, alturaMaxima: number): void {
        let contenido: HTMLDivElement = modal.querySelector(`div[class="${ClaseCss.contenidoModal}"]`);
        let altura: number = contenido.getBoundingClientRect().height;
        let alturaInicial: number = Numero(contenido.getAttribute('altura-inicial'));
        if (alturaInicial === 0) {
            alturaInicial = altura;
            contenido.setAttribute('altura-inicial', alturaInicial.toString());
        }
        if (altura > alturaMaxima)
            contenido.style.height = `${alturaMaxima}px`;
        else {
            contenido.style.height = `${alturaInicial}px`;
            let padding: number = (alturaMaxima - altura) / 2;
            modal.style.paddingTop = `${padding}px`;
            modal.style.height = `${alturaMaxima + AlturaPiePnlControl()}px`;
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

    export function MostrarMensajes() {
        let altura: number = AlturaFormulario();
        let modal: HTMLDivElement = document.getElementById("id-modal-historial") as HTMLDivElement;
        MensajesSe.MostrarMensajes();
        modal.style.display = "block";
        modal.style.height = `${altura.toString()}px`;
        let alturaContenedor: number = altura - AlturaCabeceraPnlControl();
        AjustarModal(modal, alturaContenedor);
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