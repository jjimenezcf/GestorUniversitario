﻿module MensajesSe {

    export enum enumTipoMensaje { informativo, advertencia, error };

    export class clsNotificacion {
        _tipo: enumTipoMensaje;
        public get tipo(): enumTipoMensaje {
            return this._tipo;
        }

        _mensaje: string;
        public get mensaje(): string {
            return this._mensaje;
        }
        constructor(tipo: enumTipoMensaje, mensaje: string) {
            this._tipo = tipo;
            this._mensaje = mensaje;
        }
    }

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
        MensajesSe.Apilar(enumTipoMensaje.informativo, mensaje, consola);
    }

    export function Error(origen: string, mensaje: string, consola?: string) {
        _Almacen.Error(origen, mensaje);
        MensajesSe.Apilar(enumTipoMensaje.error, mensaje, consola);
    }

    export function EmitirExcepcion(origen: string, mensaje: string, consola?: string) {
        Error(origen, mensaje, consola);
        throw EvalError(mensaje);
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
        ApiCrud.CerrarModal(modal);
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
                celda.textContent = mensaje[propiedad];
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

    export let Notificaciones: clsNotificacion[];

    export function Apilar(tipo: enumTipoMensaje, mensaje: string, mensajeDeConsola?: string) {
        let n: clsNotificacion = new clsNotificacion(tipo, mensaje);
        if (IsNull(Notificaciones))
            AsignarMemoria();

        Notificaciones.push(n);
        Notificar(tipo, mensaje, mensajeDeConsola);
    }

    export function Sacar() {
        if (IsNull(Notificaciones))
            return;

        if (Notificaciones.length > 0) {
            let n = Notificaciones.pop();
            Notificar(n.tipo, n.mensaje);
        }
        else {
            let cadena = 'No hay más mensajes';
            if (mensajeMostrado().indexOf(cadena) > 0)
                blanquearMensajeMostrado();
            else
                Notificar(enumTipoMensaje.informativo, cadena);
        }
    }

    function AsignarMemoria() {
        MensajesSe.Notificaciones = [] as MensajesSe.clsNotificacion[];
    }

    function Notificar(tipo: enumTipoMensaje, mensaje: string, mensajeDeConsola?: string) {
        var control = <HTMLInputElement>document.getElementById("Mensaje");
        var posicion = enumTipoMensaje.error ? mensaje.indexOf(`Error:`) : mensaje.indexOf(`Informativo:`)
        var mensajeConTipo = posicion === -1 ? `${tipo === enumTipoMensaje.informativo ? 'Informativo' : 'Error'}: ${mensaje}` : mensaje;
        if (control)
            control.value = `${mensajeConTipo}`;

        if (IsNullOrEmpty(mensajeDeConsola))
            mensajeDeConsola = mensajeConTipo;
        else
            mensajeDeConsola = mensaje + newLine + mensajeDeConsola;

        if (enumTipoMensaje.error === tipo)
            console.error(mensajeDeConsola);
        else
            console.log(mensajeDeConsola);
    }

    function mensajeMostrado(): string {
        var control = <HTMLInputElement>document.getElementById("Mensaje");
        if (control)
            return control.value;
        else "";
    }

    function blanquearMensajeMostrado(): void {
        var control = <HTMLInputElement>document.getElementById("Mensaje");
        if (control)
            control.value = "";
    }
}