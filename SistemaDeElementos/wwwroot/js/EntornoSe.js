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
        else {
            Notificar(TipoMensaje.Info, "No hay crud");
        }
        let modal = document.getElementById("id-modal-historial");
        if (modal.style.display === "block") {
            modal.style.height = `${altura.toString()}px`;
        }
    }
    EntornoSe.AjustarDivs = AjustarDivs;
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
    function MostrarHistorial() {
        let altura = AlturaFormulario();
        let modal = document.getElementById("id-modal-historial");
        modal.style.display = "block";
        modal.style.height = `${altura.toString()}px`;
    }
    EntornoSe.MostrarHistorial = MostrarHistorial;
    function CerrarHistorial() {
        let modal = document.getElementById("id-modal-historial");
        modal.style.display = "none";
    }
    EntornoSe.CerrarHistorial = CerrarHistorial;
    function BorrarHistorial() {
        let modal = document.getElementById("id-modal-historial");
        //recorrer las lineas del body y borrar las
    }
    EntornoSe.BorrarHistorial = BorrarHistorial;
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