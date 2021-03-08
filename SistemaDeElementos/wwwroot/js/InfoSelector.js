//************************************************************************************************************************************************************************************/
/// Gestión de los info selectores
//************************************************************************************************************************************************************************************/
class Elemento {
    constructor(elemento) {
        this._datos = null;
        if (elemento !== null) {
            this._datos = elemento._datos;
            this._modoDeAcceso = elemento._datos["ModoDeAcceso"].toLowerCase();
            this.expresionMostrar = elemento._datos;
        }
    }
    get Id() { return this._datos["id"]; }
    get Texto() { return this.mostrar(); }
    get ModoDeAcceso() { return this._modoDeAcceso; }
    static get ElementoVacio() { return new Elemento(null); }
    EsVacio() {
        return this._datos === null;
    }
    mostrar() {
        // recorre el datos y compone la expresione
        return "pendiente";
    }
}
class InfoSelector {
    constructor(idGrid) {
        this.elementos = [];
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
    get Cantidad() { return this.elementos.length; }
    get Seleccionados() { return this.elementos; }
    iniciarClase(idGrid) {
        this.idGrid = idGrid;
        this.htmlGrid = document.getElementById(idGrid);
        this.Seleccionables = Numero(this.htmlGrid.getAttribute("seleccionables"));
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
            return this.elementos[pos].Id;
        }
        console.log(`Ha intentado leer la posición ${pos} en una lista de longitud ${this.Cantidad}`);
        return 0;
    }
    LeerElemento(pos) {
        if (pos >= 0 && pos < this.Cantidad) {
            return this.elementos[pos];
        }
        console.log(`Ha intentado leer la posición ${pos} en una lista de longitud ${this.Cantidad}`);
        return Elemento.ElementoVacio;
    }
    InsertarElemento(elemento) {
        var pos = this.Buscar(elemento.Id);
        if (pos < 0) {
            this.elementos.push(elemento);
        }
        return pos;
    }
    InsertarElementos(elementos) {
        if (!elementos || elementos.length > 0) {
            for (var i = 0; i < elementos.length; i++) {
                let e = elementos[i];
                this.InsertarElemento(e);
            }
        }
        else {
            console.log(`Ha intentado insertar en la lista un array de elementos vacío`);
        }
        return this.Cantidad;
    }
    Buscar(id) {
        for (let i = 0; i < this.elementos.length; i++)
            if (Numero(this.elementos[i].Id) === id)
                return (i);
        return -1;
    }
    Quitar(idSeleccionado) {
        var pos = this.Buscar(idSeleccionado);
        if (pos >= 0) {
            this.elementos.splice(pos, 1);
            console.log(`Ha quitado de la lista el ${idSeleccionado}`);
            this.deshabilitarCheck(false);
        }
        else
            console.error(`No se ha localizado el elemento con id  ${idSeleccionado}`);
    }
    QuitarTodos() {
        this.elementos.splice(0, this.elementos.length);
    }
    ToString() {
        let ids = "";
        for (var i = 0; i < this.elementos.length; i++) {
            ids = ids + this.elementos[i].Id;
            if (i < this.elementos.length - 1)
                ids = ids + ';';
        }
        return ids;
    }
    SincronizarCheck() {
        this.deshabilitarCheck(true);
    }
}
//# sourceMappingURL=InfoSelector.js.map