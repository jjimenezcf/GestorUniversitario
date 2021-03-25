var EntornoSe;
(function (EntornoSe) {
    EntornoSe.Historial = undefined;
    function IniciarEntorno() {
        AjustarDivs();
        Registro.RegistrarUsuarioDeConexion(this)
            .then((usuarioConectado) => {
            ArbolDeMenu.ReqSolicitarMenu('id-contenedor-menu');
        })
            .catch(() => {
            MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, "Error al leer el usuario de conexión");
        });
    }
    EntornoSe.IniciarEntorno = IniciarEntorno;
    function AjustarDivs() {
        let altura = AlturaFormulario();
        let alturaDelCuerpo = AlturaDelCuerpo(altura);
        let cuerpo = document.getElementById(LiteralMnt.idCuerpoDePagina);
        cuerpo.style.height = `${alturaDelCuerpo.toString()}px`;
        let { modalMenu, estadoMenu } = ArbolDeMenu.ObtenerDatosMenu();
        if (estadoMenu.getAttribute(atMenu.abierto) === literal.true)
            modalMenu.style.height = `${AlturaDelMenu(altura).toString()}px`;
        if (Crud.crudMnt !== null) {
            Crud.crudMnt.PosicionarPanelesDelCuerpo();
        }
        EntornoSe.AjustarModalesAbiertas();
    }
    EntornoSe.AjustarDivs = AjustarDivs;
    function AjustarModalesAbiertas() {
        let modales = document.getElementsByClassName(ClaseCss.contenedorModal);
        for (let i = 0; i < modales.length; i++) {
            let modal = modales[i];
            if (modal.style.display === 'block') {
                let alturaMaxima = AlturaFormulario() - AlturaCabeceraPnlControl();
                AjustarModal(modales[i], alturaMaxima);
            }
        }
    }
    EntornoSe.AjustarModalesAbiertas = AjustarModalesAbiertas;
    function AjustarModal(modal, alturaMaxima) {
        let contenedor = modal.querySelector(`div[class="${ClaseCss.contenidoModal}"]`);
        let alturaCuerpoPagina = AlturaDelCuerpo(AlturaFormulario());
        let alturaModal = contenedor.getBoundingClientRect().height;
        if (alturaCuerpoPagina < alturaModal)
            contenedor.style.height = `${alturaCuerpoPagina}px`;
        else {
            let ratio = Numero(modal.getAttribute('ratio-inicial'));
            if (ratio > 0) {
                let alturaInicial = Numero(modal.getAttribute('altura-inicial'));
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
        let altura = contenedor.getBoundingClientRect().height;
        let padding = (AlturaFormulario() - altura) / 2;
        modal.style.paddingTop = `${padding}px`;
    }
    function AjustarModal2(modal, alturaMaxima) {
        let contenedor = modal.querySelector(`div[class="${ClaseCss.contenidoModal}"]`);
        let cabecera = modal.querySelector(`div[class="${ClaseCss.cabeceraModal}"]`);
        let cuerpo = modal.querySelector(`div[class="${ClaseCss.cuerpoModal}"]`);
        let pie = modal.querySelector(`div[class="${ClaseCss.pieModal}"]`);
        let alturaCabecera = cabecera.getBoundingClientRect().height;
        let alturaCuerpo = cuerpo.getBoundingClientRect().height;
        let alturaPie = pie.getBoundingClientRect().height;
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
    function AjustarModalOld(modal, alturaMaxima) {
        let contenido = modal.querySelector(`div[class="${ClaseCss.contenidoModal}"]`);
        let altura = contenido.getBoundingClientRect().height;
        let alturaInicial = Numero(modal.getAttribute('altura-inicial'));
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
        let padding = (alturaMaxima - altura) / 2;
        modal.style.paddingTop = `${padding}px`;
        modal.style.height = `${alturaMaxima + AlturaPiePnlControl()}px`;
    }
    function InicializarHistorial() {
        EntornoSe.Historial = new HistorialSe.HistorialDeNavegacion();
    }
    EntornoSe.InicializarHistorial = InicializarHistorial;
    function NavegarAUrl(url) {
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
    EntornoSe.NavegarAUrl = NavegarAUrl;
    function Sumit(form) {
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
    EntornoSe.Sumit = Sumit;
    function Llamador() {
        var callerName;
        try {
            throw new Error();
        }
        catch (e) {
            var re = /(\w+)@|at (\w+) \(/g, st = e.stack, m;
            re.exec(st), m = re.exec(st);
            callerName = m[1] || m[2];
        }
        return callerName;
    }
    EntornoSe.Llamador = Llamador;
    ;
})(EntornoSe || (EntornoSe = {}));
//# sourceMappingURL=EntornoSe.js.map