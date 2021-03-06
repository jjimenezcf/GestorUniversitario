var EntornoSe;
(function (EntornoSe) {
    EntornoSe.Historial = undefined;
    function IniciarEntorno() {
        AjustarDivs();
        if (!Registro.HayUsuarioDeConexion())
            Registro.RegistrarUsuarioDeConexion(this)
                .then((usuarioConectado) => {
                ArbolDeMenu.ReqSolicitarMenu('id-contenedor-menu');
            })
                .catch(() => {
                MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, "Error al leer el usuario de conexión");
            });
        else
            ArbolDeMenu.ReqSolicitarMenu('id-contenedor-menu');
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
    function AbrirPestana(url) {
        var pattern = /^(http|https)\:\/\/[a-z0-9\.-]+\.[a-z]{2,4}/gi;
        if (!url.match(pattern))
            throw Error(`La url ${url} no es válida`);
        let v = window.open(url);
        return v;
    }
    EntornoSe.AbrirPestana = AbrirPestana;
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