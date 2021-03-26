
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
                MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, "Error al leer el usuario de conexión");
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
                let alturaMaxima: number = AlturaFormulario() - AlturaCabeceraPnlControl();
                AjustarModal(modales[i] as HTMLDivElement, alturaMaxima);
            }
        }
    }

    function AjustarModal(modal: HTMLDivElement, alturaMaxima: number): void {
        let contenedor: HTMLDivElement = modal.querySelector(`div[class="${ClaseCss.contenidoModal}"]`);

        let alturaCuerpoPagina: number = AlturaDelCuerpo(AlturaFormulario());
        let alturaModal: number = contenedor.getBoundingClientRect().height;
        if (alturaCuerpoPagina < alturaModal)
            contenedor.style.height = `${alturaCuerpoPagina}px`;
        else {
            let ratio: number = Numero(modal.getAttribute('ratio-inicial'));
            if (ratio > 0) {
                let alturaInicial: number = Numero(modal.getAttribute('altura-inicial'));
                if (alturaMaxima < alturaInicial)
                    contenedor.style.height = `${alturaMaxima * ratio / 100}px`;
                else
                    contenedor.style.height = "auto";
            }
            else {
                ratio = alturaModal * 100 / alturaMaxima;
                modal.setAttribute('ratio-inicial', ratio.toString());
                modal.setAttribute('altura-inicial', alturaMaxima.toString());
            }
        }

        let altura: number = contenedor.getBoundingClientRect().height;
        let padding: number = (AlturaFormulario() - altura) / 2;
        modal.style.paddingTop = `${padding}px`;
    }

    function AjustarModal2(modal: HTMLDivElement, alturaMaxima: number): void {
        let contenedor: HTMLDivElement = modal.querySelector(`div[class="${ClaseCss.contenidoModal}"]`);
        let cabecera: HTMLDivElement = modal.querySelector(`div[class="${ClaseCss.cabeceraModal}"]`);
        let cuerpo: HTMLDivElement = modal.querySelector(`div[class="${ClaseCss.cuerpoModal}"]`);
        let pie: HTMLDivElement = modal.querySelector(`div[class="${ClaseCss.pieModal}"]`);

        let alturaCabecera: number = cabecera.getBoundingClientRect().height;
        let alturaCuerpo: number = cuerpo.getBoundingClientRect().height;
        let alturaPie: number = pie.getBoundingClientRect().height;
        contenedor.style.height = `${alturaMaxima - 2 * AlturaPiePnlControl()}px`;


        let altura = alturaCabecera + alturaCuerpo + alturaPie;
        if (altura < alturaMaxima - 200) {
            //cuerpo.style.height = `${alturaMaxima - alturaCabecera - alturaPie - 2 * AlturaPiePnlControl()}px`;
            contenedor.style.height = `${altura + 200 - 2 * AlturaPiePnlControl()}px`;
        }
        else {
            //cuerpo.style.height = `${alturaMaxima - alturaCabecera - alturaPie - 2 * AlturaPiePnlControl()}px`;
            contenedor.style.height = `${alturaMaxima - 200 - 2 * AlturaPiePnlControl()}px`;
        }

        //let padding: number = (alturaMaxima - altura) / 2;
        //modal.style.paddingTop = `${padding}px`;
        modal.style.height = `${alturaMaxima + AlturaPiePnlControl()}px`;
    }

    function AjustarModalOld(modal: HTMLDivElement, alturaMaxima: number): void {
        let contenido: HTMLDivElement = modal.querySelector(`div[class="${ClaseCss.contenidoModal}"]`);
        let altura: number = contenido.getBoundingClientRect().height;
        let alturaInicial: number = Numero(modal.getAttribute('altura-inicial'));
        if (alturaInicial === 0) {
            alturaInicial = altura;
            modal.setAttribute('altura-inicial', `${alturaInicial}px`);
        }

        if (alturaInicial >= alturaMaxima - AlturaPiePnlControl()) {
            alturaInicial = alturaMaxima - AlturaPiePnlControl() - 1;
            modal.setAttribute('altura-inicial', "0");
        }

        if (altura > alturaMaxima)
            contenido.style.height = `${alturaMaxima - 2 * AlturaPiePnlControl()}px`;
        else {
            contenido.style.height = `${alturaInicial}px`;
        }
        let padding: number = (alturaMaxima - altura) / 2;
        modal.style.paddingTop = `${padding}px`;
        modal.style.height = `${alturaMaxima + AlturaPiePnlControl()}px`;
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


    export function AbrirPestana(url: string) {
        var pattern = /^(http|https)\:\/\/[a-z0-9\.-]+\.[a-z]{2,4}/gi;
        if (url.match(pattern))
            throw Error(`La url ${url} no es válida`);
        window.open(url);
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