var MensajesSe;
(function (MensajesSe) {
    let enumTipoMensaje;
    (function (enumTipoMensaje) {
        enumTipoMensaje[enumTipoMensaje["informativo"] = 0] = "informativo";
        enumTipoMensaje[enumTipoMensaje["advertencia"] = 1] = "advertencia";
        enumTipoMensaje[enumTipoMensaje["error"] = 2] = "error";
    })(enumTipoMensaje || (enumTipoMensaje = {}));
    ;
    class clsMensaje {
        constructor(tipo, origen, mensaje) {
            this._tipo = tipo;
            this._mensaje = mensaje;
            this._fecha = new Date(Date.now());
            this._origen = origen;
        }
    }
    class AlmacenDeMensajes {
        constructor() {
            this._mensajes = [];
        }
        set Mensajes(mensajes) {
            this._mensajes = mensajes["_mensajes"];
        }
        Error(mensaje) {
            this._mensajes.push(new clsMensaje(enumTipoMensaje.error, EntornoSe.Llamador(), mensaje));
        }
        Info(mensaje) {
            this._mensajes.push(new clsMensaje(enumTipoMensaje.informativo, 'info', mensaje));
        }
        Advertencia(mensaje) {
            this._mensajes.push(new clsMensaje(enumTipoMensaje.advertencia, EntornoSe.Llamador(), mensaje));
        }
    }
    let _mensajes = null;
    function ObtenerAlmacen() {
        if (_mensajes != null)
            return _mensajes;
        let almacen = sessionStorage.getItem('almacen-mensajes');
        if (almacen === null || almacen == undefined) {
            _mensajes = new AlmacenDeMensajes();
        }
        else {
            _mensajes = new AlmacenDeMensajes();
            _mensajes.Mensajes = JSON.parse(almacen);
        }
        return _mensajes;
    }
    function Persistir() {
        if (_mensajes != null)
            sessionStorage.setItem('almacen-mensajes', JSON.stringify(_mensajes));
    }
    ;
    function Info(mensaje, consola) {
        let a = ObtenerAlmacen();
        a.Info(mensaje);
        Persistir();
        Notificar(TipoMensaje.Info, mensaje, consola);
    }
    MensajesSe.Info = Info;
    function Error(mensaje, consola) {
        let a = ObtenerAlmacen();
        a.Error(mensaje);
        Persistir();
        Notificar(TipoMensaje.Error, mensaje, consola);
    }
    MensajesSe.Error = Error;
})(MensajesSe || (MensajesSe = {}));
//# sourceMappingURL=Mensajes.js.map