
//************************************************************************************************************************************************************************************/

/// Gestión de los info selectores

//************************************************************************************************************************************************************************************/

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
        var checkboxes = document.getElementsByName(`chksel.${this._idGrid}`);
        for (var x = 0; x < checkboxes.length; x++) {
            if (!checkboxes[x].checked) {
                checkboxes[x].disabled = deshabilitar;
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

                if (this._seleccionados.length >= this._seleccionables)
                    this.deshabilitarCheck(true);
            }
        }
    }

    Quitar(idSeleccionado) {
        var pos = this._seleccionados.indexOf(idSeleccionado);
        if (pos >= 0) {
            this._seleccionados.splice(pos, 1);

            if (this._seleccionados.length < this._seleccionables)
                this.deshabilitarCheck(false);
        }
        else
            Mensaje(TipoMensaje.Info, `No se ha localizado el elemento con id  ${idSeleccionado}`);
    }

}



class InfoSelectores {

    _infoSelectores = new Array();
    
    obtener(id) {
        if (!this._infoSelectores || this._infoSelectores.length === 0)
            return;

        for (var i = 0; this._infoSelectores.length; i++) {
            if (this._infoSelectores[i].Id === id)
                return this._infoSelectores[i];
        }
        return undefined;
    }


    Insertar(infoSelector) {
        var infSel = this.obtener(infoSelector.Id);
        if (!infSel)
            this._infoSelectores.push(infoSelector);

        return this._infoSelectores.length;
    }

    Borrar(id) {
        var infSel = this.obtener(id);
        if (infSel)
            this._infoSelectores.splice(infSel, 1);

        return this._infoSelectores.length;
    }
}

var infoSelectores = new InfoSelectores();


function TratarClickDeSeleccion(idGrid, idCheck) {
    var check = document.getElementById(idCheck);
    if (check.checked)
        anadirAlInfoSelector(idGrid, idCheck);
    else
        quitarDelSelector(idGrid, idCheck);
}

function anadirAlInfoSelector(idGrid, idCheck) {
    if (!infoSelectores[`${idGrid}`])
        infoSelectores[`${idGrid}`] = new InfoSelector(idGrid);

    var selector = infoSelectores[`${idGrid}`];
    var id = obtenerIdDeLaFilaChequeada(idCheck);
    selector.Insertar(id);
}


function quitarDelSelector(idGrid, idCheck) {

    var selector = infoSelectores[`${idGrid}`];
    var id = obtenerIdDeLaFilaChequeada(idCheck);
    selector.Quitar(id);
}

