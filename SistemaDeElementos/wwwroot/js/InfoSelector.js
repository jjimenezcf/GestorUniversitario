//************************************************************************************************************************************************************************************/
/// Gestión de los info selectores
//************************************************************************************************************************************************************************************/
class Elemento {
    constructor(id, texto) {
        this.Id = id;
        this.Texto = texto;
    }
    static get ElementoVacio() { return new Elemento(0, ''); }
    EsVacio() {
        if (this.Id === 0 || this.Texto === '')
            return true;
    }
}
class InfoSelector {
    constructor(idGrid) {
        this.iniciarClase(idGrid);
        console.log(`Ha creado el infoselector ${idGrid}`);
    }
    get Seleccionables() {
        return this._Seleccionables == NaN
            ? 0
            : this._Seleccionables;
    }
    get PuedeSeleccionarMas() {
        if (this.Seleccionables < 0)
            return true;
        return this.Cantidad < this.Seleccionables;
    }
    set Seleccionables(seleccionables) {
        this._Seleccionables = seleccionables;
    }
    get Id() { return this.idGrid; }
    get Cantidad() { return this.seleccionados.length; }
    get Seleccionados() { return this.seleccionados; }
    iniciarClase(idGrid) {
        this.idGrid = idGrid;
        this.htmlGrid = document.getElementById(idGrid);
        this.Seleccionables = Numero(this.htmlGrid.getAttribute("seleccionables"));
        this.seleccionados = new Array();
        this.paraMostrarEnSelector = new Array();
    }
    deshabilitarCheck(deshabilitar) {
        var ejecutar = false;
        //si has desmarcado checks y los seleccionados son menos que los seleccionables --> ok a habilitar
        if (deshabilitar === false && this.PuedeSeleccionarMas)
            ejecutar = true;
        //Si has marcado y los seleccionados son más o igual que los seleccionables --> ok a deshabilitar
        if (deshabilitar === true && !this.PuedeSeleccionarMas)
            ejecutar = true;
        if (ejecutar) {
            document.getElementsByName(`chksel.${this.idGrid}`).forEach(c => {
                let check = c;
                if (!check.checked) {
                    check.disabled = deshabilitar;
                }
                else {
                    check.disabled = false;
                }
            });
        }
    }
    Contiene(id) {
        if (this.Buscar(id) < 0)
            return false;
        else
            return true;
    }
    LeerId(pos) {
        if (pos >= 0 && pos < this.Cantidad) {
            return this.seleccionados[pos];
        }
        console.log(`Ha intentado leer la posición ${pos} en una lista de longitud ${this.Cantidad}`);
        return 0;
    }
    LeerElemento(pos) {
        var id = this.LeerId(pos);
        if (id > 0) {
            var texto = this.paraMostrarEnSelector[pos];
            return new Elemento(id, texto);
        }
        return Elemento.ElementoVacio;
    }
    InsertarId(id) {
        if (this.PuedeSeleccionarMas) {
            this.seleccionados.push(id);
        }
        this.deshabilitarCheck(true);
        return this.Cantidad;
    }
    InsertarElemento(id, textoMostrar) {
        if (!id || isNaN(id)) {
            Mensaje(TipoMensaje.Error, `Ha intentado insertar en la lista un id no válido ${id}`);
            return -1;
        }
        var pos = this.Buscar(id);
        if (pos < 0) {
            pos = this.InsertarId(id);
            if (pos === this.seleccionados.length) {
                this.paraMostrarEnSelector.push(textoMostrar);
            }
        }
        return pos;
    }
    InsertarElementos(elementos) {
        if (!elementos || elementos.length > 0) {
            for (var i = 0; i < elementos.length; i++) {
                let e = elementos[i];
                if (this.seleccionados.indexOf(e.Id) < 0)
                    this.InsertarElemento(e.Id, e.Texto);
            }
        }
        else {
            console.log(`Ha intentado insertar en la lista un array de elementos vacío`);
        }
        return this.Cantidad;
    }
    Buscar(id) {
        return this.seleccionados.indexOf(id);
    }
    Quitar(idSeleccionado) {
        var pos = this.seleccionados.indexOf(idSeleccionado);
        if (pos >= 0) {
            this.seleccionados.splice(pos, 1);
            this.paraMostrarEnSelector.splice(pos, 1);
            console.log(`Ha quitado de la lista el ${idSeleccionado}`);
            this.deshabilitarCheck(false);
        }
        else
            console.error(`No se ha localizado el elemento con id  ${idSeleccionado}`);
    }
    QuitarTodos() {
        this.seleccionados.splice(0, this.seleccionados.length);
        this.paraMostrarEnSelector.splice(0, this.paraMostrarEnSelector.length);
    }
    ToString() {
        let ids = "";
        for (var i = 0; i < this.seleccionados.length; i++) {
            ids = ids + this.seleccionados[i];
            if (i < this.seleccionados.length - 1)
                ids = ids + ';';
        }
        return ids;
    }
    SincronizarCheck() {
        this.deshabilitarCheck(true);
    }
}
//# sourceMappingURL=InfoSelector.js.map