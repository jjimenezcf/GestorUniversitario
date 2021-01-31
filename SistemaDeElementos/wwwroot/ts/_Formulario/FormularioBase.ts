namespace Formulario {


    export let formulario: Base = null;

    export class Base {

        private _idFormulario: string;

        protected get Pagina(): string {
            return this.Estado.Obtener(Sesion.paginaActual);
        }

        protected get CuerpoDelFormulario(): HTMLDivElement {           
            return document.getElementById(`datos-${this._idFormulario}`) as HTMLDivElement
        }

        private _estado: HistorialSe.EstadoPagina = undefined;

        public get Estado(): HistorialSe.EstadoPagina {
            if (this._estado === undefined) {
                throw new Error("Debe definir la variable estado");
            }
            return this._estado;
        }

        public set Estado(valor: HistorialSe.EstadoPagina) {
            this._estado = valor;
        }

        protected _controlador: string;

        public get Controlador() {
            return this._controlador;
        }

        constructor(idFormulario: string) {

            this._idFormulario = idFormulario;

        }

        public Inicializar() {
            if (EntornoSe.Historial.HayHistorial(this._idFormulario))
                this._estado = EntornoSe.Historial.ObtenerEstadoDePagina(this._idFormulario);
            else
                this._estado = new HistorialSe.EstadoPagina(this._idFormulario);

            this.CuerpoDelFormulario.style.overflowY = "scroll"
        }

        public OcultarMostrarBloque(idHtmlBloque: string): void {
            let extensor: HTMLInputElement = document.getElementById(`expandir.${idHtmlBloque}.input`) as HTMLInputElement;
            if (NumeroMayorDeCero(extensor.value)) {
                extensor.value = "0";
                ApiCrud.OcultarPanel(document.getElementById(`${idHtmlBloque}`) as HTMLDivElement);
            }
            else {
                extensor.value = "1";
                ApiCrud.MostrarPanel(document.getElementById(`${idHtmlBloque}`) as HTMLDivElement);
            }
        }
    }

    export function EventosDelFormulario(accion: string, parametros: any) {
        try {
            switch (accion) {
                case Evento.Mnt.OcultarMostrarBloque: {
                    let idHtmlBloque: string = parametros;
                    formulario.OcultarMostrarBloque(idHtmlBloque);
                    break;
                }
                default: {
                    Mensaje(TipoMensaje.Error, `la opción ${accion} no está definida`);
                    break;
                }
            }
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
        }
    }

}