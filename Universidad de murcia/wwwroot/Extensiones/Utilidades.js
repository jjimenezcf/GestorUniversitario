
const newLine = "\n";

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
    for (const prop in fila) {
        var celda = {
            id: `${idGrid}_${numFil}_${i}`,
            nombre: `${prop}`,
            valor: `${fila[prop]}`
        };
        this.items.push(celda);
        i++;
    }
}

