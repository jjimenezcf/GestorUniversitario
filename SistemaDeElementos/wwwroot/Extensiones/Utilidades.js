
const newLine = "\n";

const TipoMensaje = { Info:"informativo"};

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


function Celdas(idGrid, numFil, fila) {
    this.items = new Array();
    var i = 0;
    var htmlCeldas = document.getElementById(idGrid + "_c0").cells;
    for (var j = 0; j < htmlCeldas.length; j++) {
        var th = htmlCeldas[j];
        var atributo = th.getAttribute("descriptor");
        if (atributo === null)
            continue;

        var descriptor = JSON.parse(atributo);
        var encontrada = false;
        for (const propiedad in fila) {
            if (`${propiedad.toLowerCase()}` === descriptor.propiedad.toLowerCase()) {
                var celda = {
                    id: `${idGrid}_${propiedad}_${numFil}`,
                    nombre: `${idGrid}_${propiedad}`,
                    valor: `${fila[propiedad]}`,
                    visible: `${descriptor.visible}`,
                    alineada: `class='${descriptor.alineada}'`
                };
                this.items.push(celda);
                i++;
                encontrada = true;
                break;
            }
        }
        if (!encontrada) {
            var celdaNueva = {
                id: `${idGrid}_${numFil}_${this.items.length}`,
                nombre: `${descriptor.propiedad}`,
                valor: `${descriptor.valor}`,
                visible: `${descriptor.visible}`,
                alineada: `class='${descriptor.alineada}'`
            };

            if (celdaNueva.valor === "CrearCheck") {
                celdaNueva.valor = newLine + `<input type='checkbox' id='${idGrid}_chk_${numFil}' name='chk_${idGrid}' class='text-center' aria-label='Marcar para seleccionar'>` + newLine;
            }

           this.items.push(celdaNueva);
        }
    }
}


function ObtenerIdDeLaFilaChequeada(idCheck) {
    return obtenerValorDeLaColumnaChequeada(idCheck, "id");
}


function obtenerValorDeLaColumnaChequeada(idCheck, columna) {
    var inputId = document.getElementById(idCheck.replace(".chksel", `.${columna}`));
    return inputId.value;
}

if (!String.prototype.trim) {
    (function () {
        // Make sure we trim BOM and NBSP
        var rtrim = /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/g;
        String.prototype.trim = function () {
            return this.replace(rtrim, '');
        };
    })();
}

if (!String.prototype.isNullOrEmpty) {
    (function () {
        String.prototype.isNullOrEmpty = function () {
            if (this !== undefined)
                return this.length === 0 || this.trim() === '';
            return true;
        };
    })();
}

function isNullOrEmpty(str) {
    return str.length === 0 || str.trim() === '';
}


