module MensajesSe {

    enum enumTipoMensaje { informativo, advertencia, error };

    class clsMensaje {
        _tipo: enumTipoMensaje;
        _mensaje: string;
        _origen: string;
        _fecha: Date;

        constructor(tipo: enumTipoMensaje, origen: string, mensaje: string) {
            this._tipo = tipo;
            this._mensaje = mensaje;
            this._fecha = new Date(Date.now());
            this._origen = origen;
        }
    }

    class AlmacenDeMensajes {
        private _mensajes: clsMensaje[] = [];

        public set Mensajes(mensajes: any) {
            this._mensajes = mensajes["_mensajes"];
        }

        constructor() {
        }

        public Error(mensaje: string): void {
            this._mensajes.push(new clsMensaje(enumTipoMensaje.error, EntornoSe.Llamador(), mensaje));
        }

        public Info(mensaje: string): void {
            this._mensajes.push(new clsMensaje(enumTipoMensaje.informativo, 'info', mensaje));
        }

        public Advertencia(mensaje: string): void {
            this._mensajes.push(new clsMensaje(enumTipoMensaje.advertencia, EntornoSe.Llamador(), mensaje));
        }
    }

    let _mensajes: AlmacenDeMensajes = null;

    function ObtenerAlmacen(): AlmacenDeMensajes {
        if (_mensajes != null)
            return _mensajes;

        let almacen: string = sessionStorage.getItem('almacen-mensajes');
        if (almacen === null || almacen == undefined) {
            _mensajes = new AlmacenDeMensajes();
        }
        else {
            _mensajes = new AlmacenDeMensajes();
            _mensajes.Mensajes = JSON.parse(almacen);
        }
        return _mensajes;
    }

    function Persistir(): void {
        if (_mensajes != null)
            sessionStorage.setItem('almacen-mensajes', JSON.stringify(_mensajes));
    };

    export function Info(mensaje: string, consola?: string) {
        let a: AlmacenDeMensajes = ObtenerAlmacen();
        a.Info(mensaje);
        Persistir();
        Notificar(TipoMensaje.Info, mensaje, consola);
    }

    export function Error(mensaje: string, consola?: string) {
        let a: AlmacenDeMensajes = ObtenerAlmacen();
        a.Error(mensaje);
        Persistir();
        Notificar(TipoMensaje.Error, mensaje, consola);
    }



}