var EntornoSe;
(function (EntornoSe) {
    class AlmacenDeMensajes {
        constructor(id) {
            this._errores = [];
            this._mensajes = [];
            this._id = id;
        }
        Error(mensaje, mensajeDeConsola) {
            Mensaje(TipoMensaje.Error, mensaje, mensajeDeConsola);
            this._errores.push(mensaje);
            return this._errores.length;
        }
        Info(mensaje, mensajeDeConsola) {
            Mensaje(TipoMensaje.Info, mensaje, mensajeDeConsola);
            this._mensajes.push(mensaje);
            return this._mensajes.length;
        }
    }
    EntornoSe.AlmacenDeMensajes = AlmacenDeMensajes;
    EntornoSe.Historial = undefined;
    function IniciarEntorno() {
        ArbolDeMenu.ReqSolicitarMenu('id-contenedor-menu');
        window.onpopstate = function (e) {
            console.log(e.state);
        };
        AjustarDivs();
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
        else {
            Mensaje(TipoMensaje.Info, "No hay crud");
        }
    }
    EntornoSe.AjustarDivs = AjustarDivs;
    function InicializarHistorial() {
        EntornoSe.Historial = new HistorialSe.HistorialDeNavegacion();
    }
    EntornoSe.InicializarHistorial = InicializarHistorial;
    function NavegarAUrl(url) {
        PonerCapa();
        EntornoSe.Historial.Persistir();
        window.location.href = url;
    }
    EntornoSe.NavegarAUrl = NavegarAUrl;
})(EntornoSe || (EntornoSe = {}));
//# sourceMappingURL=InicializarEntorno.js.map