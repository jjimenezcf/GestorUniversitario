function Mensaje(tipo: string, mensaje: string) {
    var control = <HTMLInputElement>document.getElementById("Mensaje");
    var mensaje = `(${tipo}) ${mensaje}`;
    if (control)
        control.value = `${mensaje}`;

    if (TipoMensaje.Error === tipo)
        console.error(mensaje);
    else
        console.log(mensaje)
}

function AlturaDelCuerpo(): number {
    var altura = document.defaultView.innerHeight;
    let cabecera: HTMLDivElement = document.getElementById("div-cabecera") as HTMLDivElement;
    let pie: HTMLDivElement = document.getElementById("div-pie") as HTMLDivElement;
    return altura - cabecera.clientHeight - pie.clientHeight;
}

function AlturaDelMenu(): number {
    return AlturaDelCuerpo() - 4;
}

function PonerCapa() {
    var capa = document.getElementById("CapaDeBloqueo");
    if (capa != null) {
        capa.classList.remove(ClaseCss.sinCapaDeBloqueo);
        capa.classList.add(ClaseCss.conCapaDeBloqueo);
    }
}

function QuitarCapa() {
    var capa = document.getElementById("CapaDeBloqueo");
    if (capa != null) {
        capa.classList.remove(ClaseCss.conCapaDeBloqueo);
        capa.classList.add(ClaseCss.sinCapaDeBloqueo);
    }
}

function BlanquearMensaje() {
    var control = <HTMLInputElement>document.getElementById("Mensaje");
    if (control)
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


function IsString(obj: any): boolean {
    try {
        var a = Object.prototype.toString.call(obj).match(/\s([a-z|A-Z]+)/)[1].toLowerCase() === 'string';
        return a;
    }
    catch{
        return false;
    }
}
function IsBool(obj: any): boolean {
    try {
        var a = Object.prototype.toString.call(obj).match(/\s([a-z|A-Z]+)/)[1].toLowerCase() === 'boolean';
        return a;
    }
    catch{
        return false;
    }
}
function IsNumber(obj: any): boolean {
    try {
        var a = Object.prototype.toString.call(obj).match(/\s([a-z|A-Z]+)/)[1].toLowerCase() === 'number';
        return a;
    }
    catch{
        return false;
    }
}

function IsNullOrEmpty(valor: string): boolean {

    if (valor == null || valor.NoDefinida())
        return true;

    return valor.NoDefinida();
}

function NumeroMayorDeCero(valor: string): boolean {

    if ( valor === null || valor === undefined )
        return false;

    return valor.Numero() > 0;
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

function Numero(valor: string): number {
    if (valor === undefined || valor === null)
        return 0;

    return valor.Numero();
}


class ClausulaDeFiltrado {
    clausula: string;
    criterio: string;
    valor: string;

    constructor(clausula: string, criterio: string, valor: string) {
        this.clausula = clausula;
        this.criterio = criterio;
        this.valor = valor;
    }

    EsVacia(): boolean {
        return this.clausula.NoDefinida() || this.valor.NoDefinida() || this.criterio.NoDefinida();
    }
}


function DefinirRestrictorCadena(propiedad: string, valor: string): string {
    var clausulas = new Array<ClausulaDeFiltrado>();
    var clausula: ClausulaDeFiltrado = new ClausulaDeFiltrado(propiedad, literal.filtro.criterio.igual, `${valor}`);
    clausulas.push(clausula);
    return JSON.stringify(clausulas);
}

function Encriptar(clave: string, textoParaEncriptar: string) {
    return textoParaEncriptar;
}

class ResultadoJson {
    estado: number;
    mensaje: string;
    consola: string;
    total: number;
    datos: any;
    modoDeAcceso: string;
    error: boolean;
}

class ResultadoHtml extends ResultadoJson {
    html: string;
}





