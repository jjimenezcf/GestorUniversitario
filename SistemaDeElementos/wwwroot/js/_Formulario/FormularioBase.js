var Formulario;
(function (Formulario) {
    Formulario.formulario = null;
    class Base {
        constructor(idFormulario) {
            this._estado = undefined;
            this._idFormulario = idFormulario;
            MensajesSe.Info("Creado");
        }
        get Pagina() {
            return this.Estado.Obtener(Sesion.paginaActual);
        }
        get CuerpoDelFormulario() {
            return document.getElementById(`datos-${this._idFormulario}`);
        }
        get Estado() {
            if (this._estado === undefined) {
                throw new Error("Debe definir la variable estado");
            }
            return this._estado;
        }
        set Estado(valor) {
            this._estado = valor;
        }
        get Controlador() {
            return this._controlador;
        }
        NavegarDesdeElBrowser() {
            MensajesSe.Info('Ha llamado al método navegar');
        }
        Inicializar() {
            if (EntornoSe.Historial.HayHistorial(this._idFormulario))
                this._estado = EntornoSe.Historial.ObtenerEstadoDePagina(this._idFormulario);
            else
                this._estado = new HistorialSe.EstadoPagina(this._idFormulario);
            this.CuerpoDelFormulario.style.overflowY = "scroll";
        }
        Cerrar() {
            if (this.AntesDeCerrar()) {
                window.history.back();
            }
        }
        AntesDeCerrar() {
            return true;
        }
        Aceptar() {
            if (this.AntesDeAceptar()) {
                this.Cerrar();
            }
        }
        AntesDeAceptar() {
            return true;
        }
        OcultarMostrarBloque(idHtmlBloque) {
            let extensor = document.getElementById(`expandir.${idHtmlBloque}.input`);
            if (NumeroMayorDeCero(extensor.value)) {
                extensor.value = "0";
                ApiCrud.OcultarPanel(document.getElementById(`${idHtmlBloque}`));
            }
            else {
                extensor.value = "1";
                ApiCrud.MostrarPanel(document.getElementById(`${idHtmlBloque}`));
            }
        }
    }
    Formulario.Base = Base;
    function EventosDelFormulario(accion, parametros) {
        try {
            switch (accion) {
                case Evento.Formulario.Aceptar: {
                    Formulario.formulario.Aceptar();
                    break;
                }
                case Evento.Formulario.Cerrar: {
                    Formulario.formulario.Cerrar();
                    break;
                }
                case Evento.Formulario.OcultarMostrarBloque: {
                    let idHtmlBloque = parametros;
                    Formulario.formulario.OcultarMostrarBloque(idHtmlBloque);
                    break;
                }
                default: {
                    MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `la opción ${accion} no está definida`);
                    break;
                }
            }
        }
        catch (error) {
            MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, error);
        }
    }
    Formulario.EventosDelFormulario = EventosDelFormulario;
})(Formulario || (Formulario = {}));
//# sourceMappingURL=FormularioBase.js.map