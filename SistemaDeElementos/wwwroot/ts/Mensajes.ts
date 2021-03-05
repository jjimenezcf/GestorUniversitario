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

        public get Mensajes(): clsMensaje[] {
            let mensajesGuardados: string = sessionStorage.getItem('mensajes-guardados');
            if (!(mensajesGuardados === null || mensajesGuardados == undefined)) {
                this._mensajes = JSON.parse(mensajesGuardados);
            };
            return this._mensajes;
        }

        constructor() {
        }

        public Error(origen: string, mensaje: string): void {
            this.Mensajes.push(new clsMensaje(enumTipoMensaje.error, origen, mensaje));
            this.Persistir();
        }

        public Info(mensaje: string): void {
            this.Mensajes.push(new clsMensaje(enumTipoMensaje.informativo, 'info', mensaje));
            this.Persistir();
        }

        public Advertencia(mensaje: string): void {
            this.Mensajes.push(new clsMensaje(enumTipoMensaje.advertencia, EntornoSe.Llamador(), mensaje));
            this.Persistir();
        }

        public BorrarMensajes(): void {
            this._mensajes.splice(0, this._mensajes.length);
            this.Persistir();
        }

        private Persistir(): void {
            sessionStorage.setItem('mensajes-guardados', JSON.stringify(_Almacen._mensajes));
        }

    }

    let _Almacen: AlmacenDeMensajes = new AlmacenDeMensajes();

    export function Info(mensaje: string, consola?: string) {
        _Almacen.Info(mensaje);
        Notificar(TipoMensaje.Info, mensaje, consola);
    }

    export function Error(origen: string, mensaje: string, consola?: string) {
        _Almacen.Error(origen, mensaje);
        Notificar(TipoMensaje.Error, mensaje, consola);
    }


    export function MostrarMensajes() {
        let modal: HTMLDivElement = document.getElementById("id-modal-historial") as HTMLDivElement;
        MapearMensajesAlGrid();
        let contenedor: HTMLDivElement = document.getElementById("id-contenedor-historial") as HTMLDivElement;
        let tabla: HTMLTableElement = document.getElementById('id-historial-cuerpo.tabla') as HTMLTableElement;
        tabla.style.height = `${contenedor.getBoundingClientRect().height - 130}px`;
        modal.style.display = "block";
        EntornoSe.AjustarModalesAbiertas();
    }

    export function CerrarHistorial() {
        let modal: HTMLDivElement = document.getElementById("id-modal-historial") as HTMLDivElement;
        modal.style.display = "none";
        modal.setAttribute('altura-inicial', "0");
    }

    export function BorrarHistorial() {
        _Almacen.BorrarMensajes();
        CerrarHistorial();
        let contenedor: HTMLDivElement = document.getElementById("id-contenedor-historial") as HTMLDivElement;
        let tabla: HTMLTableElement = document.getElementById('id-historial-cuerpo.tabla') as HTMLTableElement;
        tabla.style.height = ``;
        contenedor.style.height = ``;
        EntornoSe.AjustarModalesAbiertas();
    }


    export function MapearMensajesAlGrid() {

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

        let tabla: HTMLTableElement = document.getElementById('id-historial-cuerpo.tabla') as HTMLTableElement;
        let cuerpoOld: HTMLTableSectionElement = document.getElementById('id-historial-tabla.body') as HTMLTableSectionElement;
        tabla.removeChild(cuerpoOld);

        let cuerpo: HTMLTableSectionElement = document.createElement("tbody");
        cuerpo.id = cuerpoOld.id;
        let filaCabecera: HTMLTableRowElement = document.getElementById('id-historial-tabla.cabecera.fila') as HTMLTableRowElement;
        for (let i: number = _Almacen.Mensajes.length - 1; i >= 0; i--) {
            let fila: HTMLTableRowElement = crearFila(filaCabecera, _Almacen.Mensajes[i]);
            cuerpo.append(fila);
        }
        tabla.append(cuerpo);
    }



}