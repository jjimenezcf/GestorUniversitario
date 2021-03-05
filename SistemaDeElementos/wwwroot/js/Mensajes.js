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
        get Mensajes() {
            let mensajesGuardados = sessionStorage.getItem('mensajes-guardados');
            if (!(mensajesGuardados === null || mensajesGuardados == undefined)) {
                this._mensajes = JSON.parse(mensajesGuardados);
            }
            ;
            return this._mensajes;
        }
        Error(origen, mensaje) {
            this.Mensajes.push(new clsMensaje(enumTipoMensaje.error, origen, mensaje));
            this.Persistir();
        }
        Info(mensaje) {
            this.Mensajes.push(new clsMensaje(enumTipoMensaje.informativo, 'info', mensaje));
            this.Persistir();
        }
        Advertencia(mensaje) {
            this.Mensajes.push(new clsMensaje(enumTipoMensaje.advertencia, EntornoSe.Llamador(), mensaje));
            this.Persistir();
        }
        BorrarMensajes() {
            this._mensajes.splice(0, this._mensajes.length);
            this.Persistir();
        }
        Persistir() {
            sessionStorage.setItem('mensajes-guardados', JSON.stringify(_Almacen._mensajes));
        }
    }
    let _Almacen = new AlmacenDeMensajes();
    function Info(mensaje, consola) {
        _Almacen.Info(mensaje);
        Notificar(TipoMensaje.Info, mensaje, consola);
    }
    MensajesSe.Info = Info;
    function Error(origen, mensaje, consola) {
        _Almacen.Error(origen, mensaje);
        Notificar(TipoMensaje.Error, mensaje, consola);
    }
    MensajesSe.Error = Error;
    function MostrarMensajes() {
        let modal = document.getElementById("id-modal-historial");
        MapearMensajesAlGrid();
        let contenedor = document.getElementById("id-contenedor-historial");
        let tabla = document.getElementById('id-historial-cuerpo.tabla');
        tabla.style.height = `${contenedor.getBoundingClientRect().height - 130}px`;
        modal.style.display = "block";
        EntornoSe.AjustarModalesAbiertas();
    }
    MensajesSe.MostrarMensajes = MostrarMensajes;
    function CerrarHistorial() {
        let modal = document.getElementById("id-modal-historial");
        modal.style.display = "none";
        modal.setAttribute('altura-inicial', "0");
    }
    MensajesSe.CerrarHistorial = CerrarHistorial;
    function BorrarHistorial() {
        _Almacen.BorrarMensajes();
        CerrarHistorial();
        let contenedor = document.getElementById("id-contenedor-historial");
        let tabla = document.getElementById('id-historial-cuerpo.tabla');
        tabla.style.height = ``;
        contenedor.style.height = ``;
        EntornoSe.AjustarModalesAbiertas();
    }
    MensajesSe.BorrarHistorial = BorrarHistorial;
    function MapearMensajesAlGrid() {
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
        let tabla = document.getElementById('id-historial-cuerpo.tabla');
        let cuerpoOld = document.getElementById('id-historial-tabla.body');
        tabla.removeChild(cuerpoOld);
        let cuerpo = document.createElement("tbody");
        cuerpo.id = cuerpoOld.id;
        let filaCabecera = document.getElementById('id-historial-tabla.cabecera.fila');
        for (let i = _Almacen.Mensajes.length - 1; i >= 0; i--) {
            let fila = crearFila(filaCabecera, _Almacen.Mensajes[i]);
            cuerpo.append(fila);
        }
        tabla.append(cuerpo);
    }
    MensajesSe.MapearMensajesAlGrid = MapearMensajesAlGrid;
})(MensajesSe || (MensajesSe = {}));
//# sourceMappingURL=Mensajes.js.map