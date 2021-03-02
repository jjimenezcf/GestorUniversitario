module MensajesSe {

    enum enumTipoMensaje { informativo, advertencia, error };

    class clsMensaje {
        _tipo: enumTipoMensaje;
        public get tipo(): string {
            return this._tipo.toString();
        }

        _mensaje: string;
        public get mensaje(): string {
            return this._mensaje;
        }
        _origen: string;
        public get origen(): string {
            return this._origen;
        }

        _fecha: Date;
        public get fecha(): string {
            return this._fecha.toISOString();
        }

        constructor(tipo: enumTipoMensaje, origen: string, mensaje: string) {
            this._tipo = tipo;
            this._mensaje = mensaje;
            this._fecha = new Date(Date.now());
            this._origen = origen;
        }
    }

    class AlmacenDeMensajes {
        private _mensajes: clsMensaje[] = [];

        public set Mensajes(mensajes: clsMensaje[]) {
            this._mensajes = mensajes["_mensajes"];
        }
        public get Mensajes(): clsMensaje[] {
            return this._mensajes;
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

    export function MostrarMensajes() {

        function crearFila(filaCabecera: HTMLTableRowElement, mensaje: clsMensaje): HTMLTableRowElement {

            function crearCelda(celdaCabecera: HTMLTableCellElement, mensaje: clsMensaje): HTMLTableCellElement {
                let celda: HTMLTableCellElement = document.createElement("td");
                let propiedad: string = celdaCabecera.getAttribute('propiedad');
                celda.style.width = celdaCabecera.style.width;
                celda.style.textAlign = celdaCabecera.style.textAlign;
                celda.textContent = mensaje[propiedad]
                return celda;
            }

            let fila = document.createElement("tr");;
            for (let i: number = 0; i < filaCabecera.cells.length; i++) {
                let celda: HTMLTableCellElement = crearCelda(filaCabecera.cells[i], mensaje);
                fila.append(celda);
            }
            return fila;
        }

        let a: AlmacenDeMensajes = ObtenerAlmacen();
        let tabla: HTMLTableElement = document.getElementById('id-historial-cuerpo.tabla') as HTMLTableElement;
        let cuerpoOld: HTMLTableSectionElement = document.getElementById('id-historial-tabla.body') as HTMLTableSectionElement;
        tabla.removeChild(cuerpoOld);

        let cuerpo: HTMLTableSectionElement = document.createElement("tbody");
        cuerpo.id = cuerpoOld.id;
        let filaCabecera: HTMLTableRowElement = document.getElementById('id-historial-tabla.cabecera.fila') as HTMLTableRowElement;
        for (let i: number = 0; i < a.Mensajes.length; i++) {
            let fila: HTMLTableRowElement = crearFila(filaCabecera, a.Mensajes[i]);
            cuerpo.append(fila);
        }
        tabla.append(cuerpo);
    }



}