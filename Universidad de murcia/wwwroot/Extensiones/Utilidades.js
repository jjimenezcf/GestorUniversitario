
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
                    id: `${idGrid}_${numFil}_${i}`,
                    nombre: `${propiedad}`,
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
                celdaNueva.valor = `<input type='checkbox' id='${idGrid}_chx_${numFil}' name='chx_${idGrid}' class='text-center' aria-label='Marcar para seleccionar'>`;
            }

           this.items.push(celdaNueva);
        }
    }
}

