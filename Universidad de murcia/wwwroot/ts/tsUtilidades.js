var newLine = "\n";
var TipoMensaje = { Info: "informativo" };
function Mensaje(tipo, mensaje) {
    var control = document.getElementById("Mensaje");
    control.value = mensaje;
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
    var inputId = document.getElementById(idCheck.replace(".chksel", "." + columna));
    return inputId.value;
}
String.prototype.trim = function () {
    var quitar = /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g;
    return this.replace(quitar, '');
};
String.prototype.isNullOrEmpty = function () {
    if (this !== undefined)
        return this.length === 0 || this.trim() === '';
    return true;
};
String.prototype.Numero = function () {
    if (this === undefined || this === null)
        return 0;
    if (this.isNullOrEmpty())
        return 0;
    if (isNaN(this))
        return 0;
    return Number(this);
};
function isNullOrEmpty(str) {
    return str.length === 0 || str.trim() === '';
}
var ClausulaDeFiltrado = /** @class */ (function () {
    function ClausulaDeFiltrado(propiedad, criterio, valor) {
        this.Propiedad = propiedad;
        this.Criterio = criterio;
        this.Valor = valor;
    }
    ClausulaDeFiltrado.prototype.EsVacia = function () {
        return this.Propiedad.isNullOrEmpty() || this.Valor.isNullOrEmpty() || this.Criterio.isNullOrEmpty();
    };
    return ClausulaDeFiltrado;
}());
//# sourceMappingURL=tsUtilidades.js.map