class Diccionario {
    constructor(inicilizar) {
        this._claves = new Array();
        this._valores = new Array();
        if (inicilizar) {
            for (var x = 0; x < inicilizar.length; x++) {
                this.Agregar(inicilizar[x].clave, inicilizar[x].valor);
            }
        }
    }
    get Elementos() {
        return this._claves.length;
    }
    Agregar(clave, valor) {
        if (!this.Contiene(clave)) {
            this._claves.push(clave);
            this._valores.push(valor);
        }
        else {
            let i = this._claves.indexOf(clave);
            this._valores.splice(i, 1, valor);
        }
    }
    Quitar(clave) {
        var indice = this._claves.indexOf(clave, 0);
        if (indice >= 0) {
            this._claves.splice(indice, 1);
            this._valores.splice(indice, 1);
        }
    }
    Contiene(clave) {
        return this._claves.indexOf(clave) > -1;
    }
    Obtener(clave) {
        let pos = this._claves.indexOf(clave);
        if (pos >= 0)
            return this._valores.slice(pos)[0];
        return undefined;
    }
    Sacar(clave) {
        let objeto = this.Obtener(clave);
        if (objeto !== undefined)
            this.Quitar(clave);
        return objeto;
    }
    Valor(posicion) {
        if (this._valores.length <= posicion)
            return undefined;
        let clave = this.Clave(posicion);
        return this.Obtener(clave);
    }
    Clave(posicion) {
        if (posicion <= this.Elementos)
            return this._claves.slice(posicion)[0];
        return undefined;
    }
}
function ObjetoToDiccionario(objeto) {
    let diccionario = new Diccionario();
    for (var i = 0; i < objeto["_claves"].length; i++)
        diccionario.Agregar(objeto["_claves"][i], objeto["_valores"][i]);
    return diccionario;
}
function JsonToDiccionario(json) {
    let pares = JSON.parse(json);
    return ObjetoToDiccionario(pares);
}
class Pila {
    constructor() {
        this._pila = [];
    }
    meter(valor) {
        this._pila.push(valor);
    }
    sacar() {
        let valor = this._pila[this._pila.length - 1];
        this._pila.splice(this._pila.length - 1);
        return valor;
    }
}
//# sourceMappingURL=Diccionario.js.map