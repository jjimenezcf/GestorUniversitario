function Mensaje(tipo, mensaje) {
    var control = document.getElementById("Mensaje");
    var mensaje = `(${tipo}) ${mensaje}`;
    if (control)
        control.value = `${mensaje}`;
    if (TipoMensaje.Error == tipo)
        console.error(mensaje);
    else
        console.log(mensaje);
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
function EsNula(valor) {
    if (valor == null || valor.NoDefinida())
        return true;
    return valor.NoDefinida();
}
String.prototype.NoDefinida = function () {
    var str = this;
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
    constructor(propiedad, criterio, valor) {
        this.propiedad = propiedad;
        this.criterio = criterio;
        this.valor = valor;
    }
    EsVacia() {
        return this.propiedad.NoDefinida() || this.valor.NoDefinida() || this.criterio.NoDefinida();
    }
}
class ResultadoJson {
}
class ResultadoHtml extends ResultadoJson {
}
//# sourceMappingURL=tsUtilidades.js.map