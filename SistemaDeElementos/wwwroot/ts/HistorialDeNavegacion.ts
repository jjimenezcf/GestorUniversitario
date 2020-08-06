module HistorialSe {


    export class EstadoPagina extends Diccionario<any> implements IDiccionario<any> {
        constructor(pagina: string) {
            super();
            this.Agregar(Sesion.paginaActual, pagina);
        }
    }

    export class HistorialDeNavegacion {

        private _paginas: Diccionario<EstadoPagina> = undefined;

        public get Paginas(): Diccionario<EstadoPagina> {
            return this._paginas;
        }

        public get Elementos(): number {
            return this._paginas.Elementos;
        }

        constructor() {
            this._paginas = this.leerHistorial();
        }

        private leerHistorial(): Diccionario<EstadoPagina> {
            let _historialJson: string = sessionStorage.getItem(Sesion.historial);

            if (IsNullOrEmpty(_historialJson)) {
                var a = new Diccionario<EstadoPagina>();
                _historialJson = this.cachearHistorial(a);
            }
            let diccionario: Diccionario<EstadoPagina> = JsonToDiccionario(_historialJson);

            return diccionario;
        };

        private cachearHistorial(historial: Diccionario<EstadoPagina>): string {
            let jsonStringify: string = JSON.stringify(historial);
            sessionStorage.setItem(Sesion.historial, jsonStringify);
            return jsonStringify;
        }

        public ObtenerEstadoDePagina(pagina: string): EstadoPagina {
            let estadoDePagina: EstadoPagina = this._paginas.Obtener(pagina) ;

            if (estadoDePagina === undefined) {
                estadoDePagina = new EstadoPagina(pagina);
                this.GuardarEstadoDePagina(estadoDePagina);
                return estadoDePagina;
            }

            return this.ObjetoToEstadoPagina(pagina, estadoDePagina);
        }

        public Elemento(posicion: number): EstadoPagina {
            let objeto: EstadoPagina = this._paginas.Valor(posicion);
            if (objeto !== undefined) {
                return this.ObjetoToEstadoPagina(this._paginas.Clave(posicion), objeto);
            }
            return undefined;
        }

        public GuardarEstadoDePagina(estado: EstadoPagina): void {
            let clave: string = estado.Obtener(Sesion.paginaActual);
            this._paginas.Agregar(clave, estado);
        }

        public Persistir(): void {
            this.cachearHistorial(this._paginas);
        }

        public HayHistorial(pagina: string): boolean {
            return this._paginas.Contiene(pagina);
        }

        private ObjetoToEstadoPagina(pagina: string, objeto: object): EstadoPagina {
            let estadoDeLaPagina: EstadoPagina = new EstadoPagina(pagina);
            for (var i = 0; i < objeto["_claves"].length; i++)
                estadoDeLaPagina.Agregar(objeto["_claves"][i], objeto["_valores"][i]);
            return estadoDeLaPagina;
        }

    }


    //export function CrearEstado(pagina: string): EstadoPagina {
    //    let estado: EstadoPagina = new EstadoPagina(pagina);
    //    return estado;
    //}
}
