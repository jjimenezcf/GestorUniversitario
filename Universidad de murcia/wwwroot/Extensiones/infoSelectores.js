
//************************************************************************************************************************************************************************************/

/// Gestión de los info selectores

//************************************************************************************************************************************************************************************/

class InfoSelector {


    get Id() { return this._id; }
    get Cantidad() { return this._seleccionados.length; }
    get Seleccionados() { return this._seleccionados; }

    constructor(idGrid) {
        this._id = idGrid;
        this._grid = document.getElementById(idGrid);
        this._seleccionables = this._grid.getAttribute("seleccionables");
        this._seleccionados = new Array();

    }

    deshabilitarCheck(deshabilitar) {
        var checkboxes = document.getElementsByName(`chk_${this._id}`);
        for (var x = 0; x < checkboxes.length; x++) {
            if (!checkboxes[x].checked) {
                checkboxes[x].disabled = deshabilitar;
            }
        }
    }

    Insertar(idSeleccionado) {
        if (this._seleccionados.length < this._seleccionables) {
            this._seleccionados.push(idSeleccionado);

            if (this._seleccionados.length >= this._seleccionables)
                this.deshabilitarCheck(true);
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

var infoSelectores = {};


function MarcarParaSeleccionar(idGrid, idCheck) {
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
    var id = obtenerIdDeLaFila(idCheck);
    selector.Insertar(id);
}


function quitarDelSelector(idGrid, idCheck) {

    var selector = infoSelectores[`${idGrid}`];
    var id = obtenerIdDeLaFila(idCheck);
    selector.Quitar(id);
}

function obtenerIdDeLaFila(idCheck) {
    var inputId = document.getElementById(idCheck.replace("_chk_", `_id_`).replace("c_", "i_"));
    return inputId.value;
}
