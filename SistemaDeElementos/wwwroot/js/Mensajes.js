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
        get tipo() {
            return this._tipo.toString();
        }
        get mensaje() {
            return this._mensaje;
        }
        get origen() {
            return this._origen;
        }
        get fecha() {
            return this._fecha.toISOString();
        }
    }
    class AlmacenDeMensajes {
        constructor() {
            this._mensajes = [];
        }
        set Mensajes(mensajes) {
            this._mensajes = mensajes["_mensajes"];
        }
        get Mensajes() {
            return this._mensajes;
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
    function MostrarMensajes() {
        function crearFila(filaCabecera, mensaje) {
            function crearCelda(celdaCabecera, mensaje) {
                let celda = document.createElement("td");
                let propiedad = celdaCabecera.getAttribute('propiedad');
                celda.style.width = celdaCabecera.style.width;
                celda.style.textAlign = celdaCabecera.style.textAlign;
                celda.textContent = mensaje[propiedad];
                return celda;
            }
            let fila = document.createElement("tr");
            ;
            for (let i = 0; i < filaCabecera.cells.length; i++) {
                let celda = crearCelda(filaCabecera.cells[i], mensaje);
                fila.append(celda);
            }
            return fila;
        }
        let a = ObtenerAlmacen();
        let tabla = document.getElementById('id-historial-cuerpo.tabla');
        let cuerpoOld = document.getElementById('id-historial-tabla.body');
        tabla.removeChild(cuerpoOld);
        let cuerpo = document.createElement("tbody");
        cuerpo.id = cuerpoOld.id;
        let filaCabecera = document.getElementById('id-historial-tabla.cabecera.fila');
        for (let i = 0; i < a.Mensajes.length; i++) {
            let fila = crearFila(filaCabecera, a.Mensajes[i]);
            cuerpo.append(fila);
        }
        tabla.append(cuerpo);
    }
    MensajesSe.MostrarMensajes = MostrarMensajes;
})(MensajesSe || (MensajesSe = {}));
//# sourceMappingURL=Mensajes.js.map