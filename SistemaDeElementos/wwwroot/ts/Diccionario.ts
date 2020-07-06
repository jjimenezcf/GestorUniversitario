interface IDiccionario<T> {
    add(key: string, value: T): void;
    remove(key: string): void;
    containsKey(key: string): boolean;
    keys(): string[];
    values(): T[];
}


class Diccionario<T> implements IDiccionario<T> {

    _keys: string[] = [];
    _values: T[] = [];

    constructor(init?: { key: string; value: T; }[]) {
        if (init) {
            for (var x = 0; x < init.length; x++) {
                this[init[x].key] = init[x].value;
                this._keys.push(init[x].key);
                this._values.push(init[x].value);
            }
        }
    }

    add(key: string, value: T) {
        this[key] = value;
        this._keys.push(key);
        this._values.push(value);
    }

    remove(key: string) {
        var index = this._keys.indexOf(key, 0);
        this._keys.splice(index, 1);
        this._values.splice(index, 1);

        delete this[key];
    }

    keys(): string[] {
        return this._keys;
    }

    values(): T[] {
        return this._values;
    }

    containsKey(key: string) {
        if (typeof this[key] === "undefined") {
            return false;
        }

        return true;
    }

    toLookup(): IDiccionario<T> {
        return this;
    }
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
