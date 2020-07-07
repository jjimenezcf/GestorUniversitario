interface IDiccionario<T> {
    _claves: Array<string>;
    _valores: Array<T>;

    anadir(clave: string, valor: T);
    quitar(clave: string);
    claves(): Array<string>;
    valores(): Array<T>;
    contiene(clave: string): boolean;
    obtener(clave: string): T;
}

class Diccionario<T> implements IDiccionario<T> {

    _claves = new Array<string>();
    _valores = new Array<T>();

    constructor(inicilizar?: { clave: string; valor: T; }[]) {
        if (inicilizar) {
            for (var x = 0; x < inicilizar.length; x++) {
                this.anadir(inicilizar[x].clave, inicilizar[x].valor);
            }
        }
    }

    anadir(clave: string, valor: T) {
        this._claves.push(clave);
        this._valores.push(valor);
    }

    quitar(key: string) {
        var index = this._claves.indexOf(key, 0);
        this._claves.splice(index, 1);
        this._valores.splice(index, 1);
    }

    claves(): Array<string> {
        return this._claves;
    }

    valores(): Array<T> {
        return this._valores;
    }

    contiene(clave: string): boolean {
        return this._claves.indexOf(clave) > -1;
    }

    obtener(clave: string): T {
        let pos: number = this._claves.indexOf(clave);
        if (pos >= 0)
            return (this._valores.slice(pos)[0]) as T;

        return undefined;
    }
}

function ObjetoToDiccionario<T>(objeto: object): Diccionario<T> {
    let diccionario: Diccionario<T> = new Diccionario<T>();
    for (var i = 0; i < objeto["_claves"].length; i++)
        diccionario.anadir(objeto["_claves"][i], objeto["_valores"][i]);
    return diccionario;
}

function JsonToDiccionario<T>(json: string): Diccionario<T> {
    let pares: Diccionario<T> = JSON.parse(json);
    return ObjetoToDiccionario<T>(pares);
}

interface IPila<T> {
    meter(valor: T): void;
    sacar(): T;
}

class Pila<T> implements IPila<T> {
    _pila: T[] = [];

    constructor() {
    }

    meter(valor: T): void {
        this._pila.push(valor);
    }

    sacar(): T {
        let valor: T = this._pila[this._pila.length - 1];
        this._pila.splice(this._pila.length - 1);
        return valor;
    }

}