
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
    let inputId: HTMLInputElement = document.getElementById(idCheck.replace(".chksel", `.${columna}`)) as HTMLInputElement;
    return inputId.value;
}

interface String {
    NoDefinida(): boolean;
    Numero(): number;
}

function EsNula(valor: string): boolean {
    if (valor == null || valor == undefined)
        return true;

    return valor.NoDefinida();
}

String.prototype.NoDefinida = function () {
    var str: String = this;
    if (str !== undefined)
        return str.length === 0 || str.trim() === '';
    return true;
};

String.prototype.Numero = function () {
    if (this === undefined || this === null)
        return 0;

    if (this.NoDefinida())
        return 0;

    if (isNaN(this))
        return 0;

    return Number(this);
};


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
        return this.Propiedad.NoDefinida() || this.Valor.NoDefinida() || this.Criterio.NoDefinida();
    }
}

class ResultadoJson {
    estado: number;
    mensaje: string;
    consola: string;
    datos: any;
}

class ResultadoHtml extends ResultadoJson {
    html: string;
}

function ParsearRespuesta(req: XMLHttpRequest, peticion: string): ResultadoJson {
    var resultado: any;
    try {
        resultado = JSON.parse(req.response);
    }
    catch
    {
        Mensaje(TipoMensaje.Error, `Error al procesar la respuesta de ${peticion}`);
        return undefined;
    }
    return resultado;
}



