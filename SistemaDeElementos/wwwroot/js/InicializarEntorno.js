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
    EntornoSe.misCookies = {
        UsuarioConectado: 'usuario-conectado'
    };
    function LeerCookie(nombre) {
        let lista = document.cookie.split(";");
        let micookie = "";
        let valor = "";
        for (let i = 0; i < lista.length; i++) {
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
    EntornoSe.LeerCookie = LeerCookie;
    function MostrarHistorial() {
        let modal = document.getElementById("id-modal-historial");
        if (!EsTrue(modal.getAttribute('abierto'))) {
            modal.style.display = "block";
            modal.style.height = `${AlturaDelMenu(AlturaFormulario()).toString()}px`;
        }
    }
    EntornoSe.MostrarHistorial = MostrarHistorial;
    function GuardarCookie(nombre, valor) {
        document.cookie = `${nombre}=${JSON.stringify(valor)}`;
    }
    EntornoSe.GuardarCookie = GuardarCookie;
    function LeerUsuarioDeConexion(llamador) {
        function RegistrarCookie(peticion) {
            let registro = peticion.resultado.datos;
            EntornoSe.GuardarCookie(EntornoSe.misCookies.UsuarioConectado, registro);
        }
        return new Promise((resolve, reject) => {
            let url = `/${Ajax.Usuarios.ruta}/${Ajax.Usuarios.accion.LeerUsuarioDeConexion}`;
            let a = new ApiDeAjax.DescriptorAjax(llamador, Ajax.Usuarios.accion.LeerUsuarioDeConexion, llamador, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, (peticion) => {
                RegistrarCookie(peticion);
                resolve(peticion);
            }, (peticion) => {
                reject(peticion);
            });
            a.Ejecutar();
        });
    }
    EntornoSe.LeerUsuarioDeConexion = LeerUsuarioDeConexion;
})(EntornoSe || (EntornoSe = {}));
//# sourceMappingURL=InicializarEntorno.js.map