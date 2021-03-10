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
            Notificar(TipoMensaje.Error, "Error al leer el usuario de conexión");
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