function Mensaje(tipo, mensaje, mensajeDeConsola) {
    var control = document.getElementById("Mensaje");
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
function AlturaCabeceraPnlControl() {
    let cabecera = document.getElementById("cabecera-de-pagina");
    return cabecera.getBoundingClientRect().height;
}
function AlturaPiePnlControl() {
    let pie = document.getElementById("pie-de-pagina");
    return pie.getBoundingClientRect().height;
}
function AlturaFormulario() {
    return document.defaultView.innerHeight;
}
function AlturaDelCuerpo(alturaFormulario) {
    return alturaFormulario - AlturaCabeceraPnlControl() - AlturaPiePnlControl();
}
function AlturaDelMenu(alturaFormulario) {
    return AlturaDelCuerpo(alturaFormulario) - 4;
}
function PonerCapa() {
    let capa = document.getElementById("CapaDeBloqueo");
    if (capa != null) {
        let numero = Numero(capa.getAttribute('numero-de-capas'));
        if (numero <= 0) {
            capa.classList.remove(ClaseCss.sinCapaDeBloqueo);
            capa.classList.add(ClaseCss.conCapaDeBloqueo);
            numero = 0;
        }
        numero = numero + 1;
        capa.setAttribute('numero-de-capas', numero.toString());
    }
}
function QuitarCapa() {
    let capa = document.getElementById("CapaDeBloqueo");
    if (capa != null) {
        let numero = Numero(capa.getAttribute('numero-de-capas'));
        if (numero <= 1) {
            capa.classList.remove(ClaseCss.conCapaDeBloqueo);
            capa.classList.add(ClaseCss.sinCapaDeBloqueo);
            numero = 1;
        }
        numero = numero - 1;
        capa.setAttribute('numero-de-capas', numero.toString());
    }
}
function BlanquearMensaje() {
    var control = document.getElementById("Mensaje");
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
    let inputId = document.getElementById(idCheck.replace(".chksel", `.${columna}`));
    return inputId.value;
}
function IsString(obj) {
    try {
        var a = Object.prototype.toString.call(obj).match(/\s([a-z|A-Z]+)/)[1].toLowerCase() === 'string';
        return a;
    }
    catch {
        return false;
    }
}
function IsBool(obj) {
    try {
        var a = Object.prototype.toString.call(obj).match(/\s([a-z|A-Z]+)/)[1].toLowerCase() === 'boolean';
        return a;
    }
    catch {
        return false;
    }
}
function IsNumber(obj) {
    try {
        var a = Object.prototype.toString.call(obj).match(/\s([a-z|A-Z]+)/)[1].toLowerCase() === 'number';
        return a;
    }
    catch {
        return false;
    }
}
function IsNullOrEmpty(valor) {
    if (valor == null || NoDefinida(valor))
        return true;
    return NoDefinida(valor);
}
function PadLeft(cadena, rellenarCon) {
    if (cadena == null || NoDefinida(cadena))
        return rellenarCon;
    return (rellenarCon + cadena).slice(-rellenarCon.length);
}
function NumeroMayorDeCero(valor) {
    if (valor === null || valor === undefined)
        return false;
    return Numero(valor) > 0;
}
function NoDefinida(valor) {
    if (valor === null || valor === undefined)
        return true;
    if (IsString(valor) && valor === '')
        return true;
    return false;
}
;
function FechaValida(fecha) {
    if (fecha === undefined || fecha === null)
        return false;
    if (fecha.toString() === "Invalid Date")
        return false;
    return true;
}
function Numero(valor) {
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
    constructor(clausula, criterio, valor) {
        this.clausula = clausula;
        this.criterio = criterio;
        this.valor = valor;
    }
    EsVacia() {
        return NoDefinida(this.clausula) || NoDefinida(this.valor) || NoDefinida(this.criterio);
    }
}
function DefinirRestrictorCadena(propiedad, valor) {
    var clausulas = new Array();
    var clausula = new ClausulaDeFiltrado(propiedad, literal.filtro.criterio.igual, `${valor}`);
    clausulas.push(clausula);
    return JSON.stringify(clausulas);
}
function Encriptar(clave, textoParaEncriptar) {
    return textoParaEncriptar;
}
function ParsearExpresion(elemento, patron) {
    let mostrar = patron;
    for (let i = 0; i < Object.keys(elemento).length; i++) {
        let propiedad = Object.keys(elemento)[i];
        if (patron.includes(`[${propiedad.toLowerCase()}]`))
            mostrar = mostrar.replace(`[${propiedad.toLowerCase()}]`, IsNullOrEmpty(elemento[propiedad]) ? "" : elemento[propiedad]);
    }
    return mostrar;
}
function delay(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}
//# sourceMappingURL=Utilidades.js.map