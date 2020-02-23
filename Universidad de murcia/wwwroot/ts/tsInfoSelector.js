//************************************************************************************************************************************************************************************/
/// Gestión de los info selectores
//************************************************************************************************************************************************************************************/
var Elemento = /** @class */ (function () {
    function Elemento(id, texto) {
        this.Id = id;
        this.Texto = texto;
    }
    Object.defineProperty(Elemento, "ElementoVacio", {
        get: function () { return new Elemento(0, ''); },
        enumerable: true,
        configurable: true
    });
    Elemento.prototype.EsVacio = function () {
        if (this.Id === 0 || this.Texto === '')
            return true;
    };
    return Elemento;
}());
var InfoSelector = /** @class */ (function () {
    function InfoSelector(idGrid) {
        this.iniciarClase(idGrid);
    }
    Object.defineProperty(InfoSelector.prototype, "Id", {
        get: function () { return this.idGrid; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InfoSelector.prototype, "Cantidad", {
        get: function () { return this.seleccionados.length; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InfoSelector.prototype, "Seleccionables", {
        get: function () { return this.Seleccionables == NaN ? 0 : this.seleccionables; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InfoSelector.prototype, "Seleccionados", {
        get: function () { return this.seleccionados; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InfoSelector.prototype, "ColumnaMostrar", {
        get: function () { return this.columnaMostrar; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(InfoSelector.prototype, "EsModalDeSeleccion", {
        get: function () { return this.esModal; },
        enumerable: true,
        configurable: true
    });
    InfoSelector.prototype.iniciarClase = function (idGrid) {
        this.idGrid = idGrid;
        this.htmlGrid = document.getElementById(idGrid);
        this.seleccionables = this.htmlGrid.getAttribute("seleccionables").Numero();
        this.seleccionados = new Array();
        this.paraMostrarEnSelector = new Array();
        this.esModal = false;
    };
    InfoSelector.prototype.deshabilitarCheck = function (deshabilitar) {
        var ejecutar = false;
        //si has desmarcado checks y los seleccionados son menos que los seleccionables --> ok a habilitar
        if (deshabilitar === false && this.Cantidad < this.seleccionables)
            ejecutar = true;
        //Si has marcado y los seleccionados son más o igual que los seleccionables --> ok a deshabilitar
        if (deshabilitar === true && this.Cantidad >= this.seleccionables)
            ejecutar = true;
        if (ejecutar) {
            document.getElementsByName("chksel." + this.idGrid).forEach(function (c) {
                var check = c;
                if (!check.checked) {
                    check.disabled = deshabilitar;
                }
                else {
                    check.disabled = false;
                }
            });
        }
    };
    InfoSelector.prototype.Modal = function (columnaMostar) {
        this.esModal = true;
        this.columnaMostrar = columnaMostar;
    };
    InfoSelector.prototype.LeerId = function (pos) {
        if (pos >= 0 && pos < this.Cantidad) {
            return this.seleccionados[pos];
        }
        console.log("Ha intentado leer la posici\u00F3n " + pos + " en una lista de longitud " + this.Cantidad);
        return 0;
    };
    InfoSelector.prototype.LeerElemento = function (pos) {
        if (this.esModal) {
            var id = this.LeerId(pos);
            if (id > 0) {
                var texto = this.paraMostrarEnSelector[pos];
                return new Elemento(id, texto);
            }
        }
        else
            console.log("Ha intentado leer un elemento en un infoSelector no v\u00E1lido por no estar declarado como Modal");
        return Elemento.ElementoVacio;
    };
    InfoSelector.prototype.InsertarId = function (id) {
        if (!id || isNaN(parseInt(id))) {
            console.log("Ha intentado insertar en la lista un id no v\u00E1lido " + id);
            return -1;
        }
        if (this.seleccionados.length < this.seleccionables) {
            this.seleccionados.push(id);
        }
        this.deshabilitarCheck(true);
        return this.Cantidad;
    };
    InfoSelector.prototype.InsertarElemento = function (id, textoMostrar) {
        if (this.esModal) {
            var pos = this.InsertarId(id);
            if (pos === this.seleccionados.length) {
                this.paraMostrarEnSelector.push(textoMostrar);
            }
        }
        else {
            console.log("Ha intentado insertar un elemento en un infoSelector no v\u00E1lido por no estar declarado como Modal");
            return -1;
        }
        return pos;
    };
    InfoSelector.prototype.InsertarElementos = function (elementos) {
        if (!elementos || elementos.length > 0) {
            for (var i = 0; i < elementos.length; i++) {
                var e = elementos[i];
                this.InsertarElemento(e.id, e.valor);
            }
        }
        else {
            console.log("Ha intentado insertar en la lista un array de elementos vac\u00EDo");
        }
        return this.Cantidad;
    };
    InfoSelector.prototype.InsertarIds = function (ids) {
        if (!ids || ids.length === 1 && isNaN(parseInt(ids))) {
            console.log("Ha intentado insertar en la lista un array de ids no v\u00E1lidos " + ids);
            return -2;
        }
        for (var i = 0; i < ids.length; i++) {
            var idSeleccionado = ids[i];
            this.InsertarId(idSeleccionado);
        }
        return this.Cantidad;
    };
    InfoSelector.prototype.Quitar = function (idSeleccionado) {
        var pos = this.seleccionados.indexOf(idSeleccionado);
        if (pos >= 0) {
            this.seleccionados.splice(pos, 1);
            this.paraMostrarEnSelector.splice(pos, 1);
            this.deshabilitarCheck(false);
        }
        else
            console.log("No se ha localizado el elemento con id  " + idSeleccionado);
    };
    InfoSelector.prototype.ToString = function () {
        var ids = "";
        for (var i = 0; i < this.seleccionados.length; i++) {
            ids = ids + this.seleccionados[i];
            if (i < this.seleccionados.length - 1)
                ids = ids + ';';
        }
        return ids;
    };
    InfoSelector.prototype.SincronizarCheck = function () {
        this.deshabilitarCheck(true);
    };
    return InfoSelector;
}());
var InfoSelectores = /** @class */ (function () {
    function InfoSelectores() {
        this._infoSelectores = new Array();
    }
    Object.defineProperty(InfoSelectores.prototype, "Cantidad", {
        get: function () {
            if (!this._infoSelectores)
                return 0;
            else
                return this._infoSelectores.length;
        },
        enumerable: true,
        configurable: true
    });
    InfoSelectores.prototype.Obtener = function (id) {
        if (!this._infoSelectores || this._infoSelectores.length === 0)
            return null;
        for (var i = 0; i < this.Cantidad; i++) {
            if (this._infoSelectores[i].Id === id)
                return this._infoSelectores[i];
        }
        return undefined;
    };
    InfoSelectores.prototype.Insertar = function (infoSelector) {
        var infSel = this.Obtener(infoSelector.Id);
        if (!infSel)
            this._infoSelectores.push(infoSelector);
        return this._infoSelectores.length;
    };
    InfoSelectores.prototype.Borrar = function (id) {
        for (var i = 0; i < this.Cantidad; i++) {
            if (this._infoSelectores[i].Id === id)
                this._infoSelectores.splice(i, 1);
        }
        return this._infoSelectores.length;
    };
    return InfoSelectores;
}());
var infoSelectores = new InfoSelectores();
//# sourceMappingURL=tsInfoSelector.js.map