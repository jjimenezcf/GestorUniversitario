
//************************************************************************************************************************************************************************************/

/// Gestión de los info selectores

//************************************************************************************************************************************************************************************/

//TODO:
// Añadir tres propiedades más 
//  Si es para un htmlSelector
//  El id del htmSelector
//  La columna a mostrar en el htmlSelector vinculado

class InfoSelector {


    get Id() { return this._idGrid; }
    get Cantidad() { return this._seleccionados.length; }
    get Seleccionados() { return this._seleccionados; }

    iniciarClase(idGrid) {
        this._idGrid = idGrid;
        this._grid = document.getElementById(idGrid);
        this._seleccionables = this._grid.getAttribute("seleccionables");
        this._seleccionados = new Array();
    }

    constructor(idGrid) {
        this.iniciarClase(idGrid);
    }

    deshabilitarCheck(deshabilitar) {

        var ejecutar = false;
        //si has desmarcado checks y los seleccionados son menos que los seleccionables --> ok a habilitar
        if (deshabilitar === false && this._seleccionados.length < this._seleccionables)
            ejecutar = true;

        //Si has marcado y los seleccionados son más o igual que los seleccionables --> ok a deshabilitar
        if (deshabilitar === true && this.Seleccionados.length >= this._seleccionables)
            ejecutar= true;

        if (ejecutar) {
            var checkboxes = document.getElementsByName(`chksel.${this._idGrid}`);
            for (var x = 0; x < checkboxes.length; x++) {
                if (!checkboxes[x].checked) {
                    checkboxes[x].disabled = deshabilitar;
                }
            }
        }
    }

    Insertar(idsSeleccionados) {

        if (!idsSeleccionados || (idsSeleccionados.length === 1 && idsSeleccionados[0] === ""))
            return;

        for (var i = 0; i < idsSeleccionados.length; i++) {

            var idSeleccionado = idsSeleccionados[i];

            if (this._seleccionados.length < this._seleccionables) {
                if (parseInt(idSeleccionado) > 0)
                    this._seleccionados.push(idSeleccionado);

                this.deshabilitarCheck(true);
            }
            else {
                console.log(`Está intentando añadir un elemento a la lista seleccionable ${this.Id} y esta lista sólo admite ${this._seleccionables}`);
            }
        }
    }

    Quitar(idSeleccionado) {
        var pos = this._seleccionados.indexOf(idSeleccionado);
        if (pos >= 0) {
            this._seleccionados.splice(pos, 1);
            this.deshabilitarCheck(false);
        }
        else
            Mensaje(TipoMensaje.Info, `No se ha localizado el elemento con id  ${idSeleccionado}`);
    }

    ToString() {
        var ids = "";
        for (var i = 0; i < this._seleccionados.length; i++) {
            ids = ids + this._seleccionados[i];
            if (i < this._seleccionados.length - 1)
                ids = ids + ';';
        }
        return ids;
    }

    SincronizarCheck() {
        this.deshabilitarCheck(true);
    }

}

class InfoSelectores {

    _infoSelectores = new Array();

    get Cantidad() {
        if (!this._infoSelectores)
            return 0;
        else
            return this._infoSelectores.length;
    }

    Obtener(id) {
        if (!this._infoSelectores || this._infoSelectores.length === 0)
            return;

        for (var i = 0; i<this.Cantidad; i++) {
            if (this._infoSelectores[i].Id === id)
                return this._infoSelectores[i];
        }
        return undefined;
    }


    Insertar(infoSelector) {
        var infSel = this.Obtener(infoSelector.Id);
        if (!infSel)
            this._infoSelectores.push(infoSelector);

        return this._infoSelectores.length;
    }

    Borrar(id) {
        var infSel = this.Obtener(id);
        if (infSel)
            this._infoSelectores.splice(infSel, 1);

        return this._infoSelectores.length;
    }
}


var infoSelectores = new InfoSelectores();




