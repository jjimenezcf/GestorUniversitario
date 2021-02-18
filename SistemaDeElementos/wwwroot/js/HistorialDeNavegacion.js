var HistorialSe;
(function (HistorialSe) {
    class EstadoPagina extends Diccionario {
        constructor(pagina) {
            super();
            this.Agregar(Sesion.paginaActual, pagina);
        }
    }
    HistorialSe.EstadoPagina = EstadoPagina;
    class HistorialDeNavegacion {
        constructor() {
            this._paginas = undefined;
            this._paginas = this.leerHistorial();
        }
        get Paginas() {
            return this._paginas;
        }
        get Elementos() {
            return this._paginas.Elementos;
        }
        leerHistorial() {
            let _historialJson = sessionStorage.getItem(Sesion.historial);
            if (IsNullOrEmpty(_historialJson)) {
                var a = new Diccionario();
                _historialJson = this.cachearHistorial(a);
            }
            let diccionario = JsonToDiccionario(_historialJson);
            return diccionario;
        }
        ;
        cachearHistorial(historial) {
            let jsonStringify = JSON.stringify(historial);
            sessionStorage.setItem(Sesion.historial, jsonStringify);
            return jsonStringify;
        }
        ObtenerEstadoDePagina(pagina) {
            let estadoDePagina = this._paginas.Obtener(pagina);
            if (estadoDePagina === undefined) {
                estadoDePagina = new EstadoPagina(pagina);
                this.GuardarEstadoDePagina(estadoDePagina);
                return estadoDePagina;
            }
            return this.ObjetoToEstadoPagina(pagina, estadoDePagina);
        }
        Elemento(posicion) {
            let objeto = this._paginas.Valor(posicion);
            if (objeto !== undefined) {
                return this.ObjetoToEstadoPagina(this._paginas.Clave(posicion), objeto);
            }
            return undefined;
        }
        GuardarEstadoDePagina(estado) {
            let clave = estado.Obtener(Sesion.paginaActual);
            this._paginas.Agregar(clave, estado);
        }
        Persistir() {
            this.cachearHistorial(this._paginas);
        }
        HayHistorial(pagina) {
            return this._paginas.Contiene(pagina);
        }
        ObjetoToEstadoPagina(pagina, objeto) {
            let estadoDeLaPagina = new EstadoPagina(pagina);
            for (var i = 0; i < objeto["_claves"].length; i++)
                estadoDeLaPagina.Agregar(objeto["_claves"][i], objeto["_valores"][i]);
            return estadoDeLaPagina;
        }
    }
    HistorialSe.HistorialDeNavegacion = HistorialDeNavegacion;
    //export function CrearEstado(pagina: string): EstadoPagina {
    //    let estado: EstadoPagina = new EstadoPagina(pagina);
    //    return estado;
    //}
})(HistorialSe || (HistorialSe = {}));
//# sourceMappingURL=HistorialDeNavegacion.js.map