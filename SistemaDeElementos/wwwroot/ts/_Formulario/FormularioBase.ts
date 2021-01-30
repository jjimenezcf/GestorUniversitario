namespace Formulario {


    export let formulario: Base = null;

    export class Base {

        private _idFormulario: string;

        protected get Pagina(): string {
            return this.Estado.Obtener(Sesion.paginaActual);
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
        }

    }

}