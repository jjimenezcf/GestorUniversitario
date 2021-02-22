function Mensaje(tipo: string, mensaje: string, mensajeDeConsola?: string) {
    var control = <HTMLInputElement>document.getElementById("Mensaje");
    var mensajeConTipo = `(${tipo}) ${mensaje}`;
    if (control)
        control.value = `${mensajeConTipo}`;

    if (IsNullOrEmpty(mensajeDeConsola))
        mensajeDeConsola = mensajeConTipo;
    else
        mensajeDeConsola = mensaje + newLine + mensajeDeConsola;

    if (TipoMensaje.Error === tipo)
        console.error(mensajeDeConsola);
    else
        console.log(mensajeDeConsola);
}

function AlturaCabeceraPnlControl(): number {
    let cabecera: HTMLDivElement = document.getElementById("cabecera-de-pagina") as HTMLDivElement;
    return cabecera.getBoundingClientRect().height;
}

function AlturaPiePnlControl(): number {
    let pie: HTMLDivElement = document.getElementById("pie-de-pagina") as HTMLDivElement;
    return pie.getBoundingClientRect().height;
}

function AlturaFormulario() {
    return document.defaultView.innerHeight;
}

function AlturaDelCuerpo(alturaFormulario: number): number {
    return alturaFormulario - AlturaCabeceraPnlControl() - AlturaPiePnlControl();
}

function AlturaDelMenu(alturaFormulario: number): number {
    return AlturaDelCuerpo(alturaFormulario) - 4;
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

function IsString(obj: any): boolean {
    try {
        var a = Object.prototype.toString.call(obj).match(/\s([a-z|A-Z]+)/)[1].toLowerCase() === 'string';
        return a;
    }
    catch {
        return false;
    }
}
function IsBool(obj: any): boolean {
    try {
        var a = Object.prototype.toString.call(obj).match(/\s([a-z|A-Z]+)/)[1].toLowerCase() === 'boolean';
        return a;
    }
    catch {
        return false;
    }
}
function IsNumber(obj: any): boolean {
    try {
        var a = Object.prototype.toString.call(obj).match(/\s([a-z|A-Z]+)/)[1].toLowerCase() === 'number';
        return a;
    }
    catch {
        return false;
    }
}

function IsNullOrEmpty(valor: string): boolean {

    if (valor == null || NoDefinida(valor))
        return true;

    return NoDefinida(valor);
}


function PadLeft(cadena: string, rellenarCon: string): string {

    if (cadena == null || NoDefinida(cadena))
        return rellenarCon;
    return (rellenarCon + cadena).slice(-rellenarCon.length);
}

function NumeroMayorDeCero(valor: string): boolean {

    if (valor === null || valor === undefined)
        return false;

    return Numero(valor) > 0;
}


function NoDefinida(valor: any) {
    if (valor === null || valor === undefined)
        return true;

    if (IsString(valor) && valor === '')
        return true;

    return false;
};

function FechaValida(fecha: Date): boolean {
    if (fecha === undefined || fecha === null)
        return false;

    if (fecha.toString() === "Invalid Date")
        return false;

    return true;
}

function Numero(valor: any): number {
    if (valor === undefined || valor === null)
        return 0;

    if (IsString(valor))
        return Number(valor);

    if (IsBool(valor))
        if (valor)
            return 1;
        else
            return 0;

    if (IsNumber(valor))
        return valor;

    if (isNaN(valor))
        valor;

    return 0;
}

function EsObjetoDe(objeto, constructor) {
    while (objeto != null) {
        if (objeto == constructor.prototype)
            return true;
        objeto = Object.getPrototypeOf(objeto);
    }
    return false;
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
        return NoDefinida(this.clausula) || NoDefinida(this.valor) || NoDefinida(this.criterio);
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


function ParsearExpresion(elemento: any, patron: string): string {
    let mostrar: string = patron;
    for (let i = 0; i < Object.keys(elemento).length; i++) {
        let propiedad = Object.keys(elemento)[i];
        if (patron.includes(`[${propiedad.toLowerCase()}]`))
            mostrar = mostrar.replace(`[${propiedad.toLowerCase()}]`, IsNullOrEmpty(elemento[propiedad]) ? "" : elemento[propiedad]);
    }

    return mostrar;
}

function delay(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms));
}





