
function Mensaje(tipo: string, mensaje: string) {
    var control = <HTMLInputElement>document.getElementById("Mensaje");
    control.value = `(${tipo}) ${mensaje}`;
    console.log(control.value);
}

function BlanquearMensaje() {
    var control = <HTMLInputElement>document.getElementById("Mensaje");
    control.value = "";
}

function StringBuilder(value) {
    this.strings = new Array();
    this.append(value);
}

StringBuilder.prototype.append = function (value) {
    if (value) {
        this.strings.push(value);
    }
};

StringBuilder.prototype.appendLine = function (value) {
    if (value) {
        this.strings.push(value + newLine);
    }
};

StringBuilder.prototype.clear = function () {
    this.strings.length = 0;
};

StringBuilder.prototype.toString = function () {
    return this.strings.join("");
};


function ObtenerIdDeLaFilaChequeada(idCheck) {
    return obtenerValorDeLaColumnaChequeada(idCheck, "id");
}


function obtenerValorDeLaColumnaChequeada(idCheck, columna) {
    var inputId = <HTMLInputElement>document.getElementById(idCheck.replace(".chksel", `.${columna}`));
    return inputId.value;
}

interface String {
    IsNullOrEmpty(): boolean;
    Numero(): number;
}


String.prototype.IsNullOrEmpty = function () {
    var str: String = this;
    if (str !== undefined)
        return str.length === 0 || str.trim() === '';
    return true;
};

String.prototype.Numero = function () {
    if (this === undefined || this === null)
        return 0;

    if (this.IsNullOrEmpty())
        return 0;

    if (isNaN(this))
        return 0;

    return Number(this);
};

function isNullOrEmpty(str) {
    return str.length === 0 || str.trim() === '';
}

class ClausulaDeFiltrado {
    Propiedad: string;
    Criterio: string;
    Valor: string;

    constructor(propiedad: string, criterio: string, valor: string) {
        this.Propiedad = propiedad;
        this.Criterio = criterio;
        this.Valor = valor;
    }

    EsVacia(): boolean {
        return this.Propiedad.IsNullOrEmpty() || this.Valor.IsNullOrEmpty() || this.Criterio.IsNullOrEmpty();
    }
}



