interface IDiccionario<T> {
    _claves: Array<string>;
    _valores: Array<T>;

    Agregar(clave: string, valor: T);
    Quitar(clave: string);
    Contiene(clave: string): boolean;
    Obtener(clave: string): T;
}

class Diccionario<T> implements IDiccionario<T> {

    _claves = new Array<string>();
    _valores = new Array<T>();

    public get Elementos(): number {
        return this._claves.length;
    }

    constructor(inicilizar?: { clave: string; valor: T; }[]) {
        if (inicilizar) {
            for (var x = 0; x < inicilizar.length; x++) {
                this.Agregar(inicilizar[x].clave, inicilizar[x].valor);
            }
        }
    }

    public Agregar(clave: string, valor: T) {

        if (!this.Contiene(clave)) {
            this._claves.push(clave);
            this._valores.push(valor);
        }
        else {
            let i: number = this._claves.indexOf(clave);
            this._valores.splice(i, 1, valor);
        }
    }

    public Quitar(clave: string) {
        var indice = this._claves.indexOf(clave, 0);
        this._claves.splice(indice, 1);
        this._valores.splice(indice, 1);
    }

    public Contiene(clave: string): boolean {
        return this._claves.indexOf(clave) > -1;
    }

    public Obtener(clave: string): T {
        let pos: number = this._claves.indexOf(clave);
        if (pos >= 0)
            return this._valores.slice(pos)[0] as T;

        return undefined;
    }

    public Valor(posicion: number): T {
        if (this._valores.length <= posicion)
            return undefined;

        let clave: string = this.Clave(posicion);
        return this.Obtener(clave);
    }

    public Clave(posicion: number): string {
        if (posicion <= this.Elementos)
            return this._claves.slice(posicion)[0];

        return undefined;
    }


}

function ObjetoToDiccionario<T>(objeto: object): Diccionario<T> {
    let diccionario: Diccionario<T> = new Diccionario<T>();
    for (var i = 0; i < objeto["_claves"].length; i++)
        diccionario.Agregar(objeto["_claves"][i], objeto["_valores"][i]);
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