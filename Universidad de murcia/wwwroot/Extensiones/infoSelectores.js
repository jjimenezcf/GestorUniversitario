
//************************************************************************************************************************************************************************************/

/// Gestión de los info selectores

//************************************************************************************************************************************************************************************/

class InfoSelector {
    constructor(nombre, maximo, seleccionados) {
        this._nombre = nombre;
        this._maximo = maximo;
        if (seleccionados === "")
            this._seleccionados = new Array();
        else
            this._seleccionados = seleccionados.split(';');
    }

    get Nombre() { return this._nombre; }
    get Cantidad() { return this._seleccionados.length; }
    get Seleccionados() { return this._seleccionados };

    Insertar(idSeleccionado) {
        if (this._seleccionados.length < this._maximo)
            this._seleccionados.push(idSeleccionado);
        else
            Mensaje(`Solo se pueden seleccionar ${this._maximo} elementos`);
    }

    Quitar(idSeleccionado) {
        var pos = this._seleccionados.indexOf(idSeleccionado);
        if (pos >= 0)
            this._seleccionados.splice(pos, 1);
        else
            Mensaje(TipoMensaje.Info, `No se ha localizado el elemento con id  ${idSeleccionado}`);
    }
}

class InfoSelectores {
    constructor() {
        this._infoSelectores = new Array();
    }

    get Selectores() { return this._infoSelectores; }

    Buscar(nombreInfoSelector) {
        for (var i = 0; i < this._infoSelectores.length; i++) {
            if (this._infoSelectores[i].Nombre === nombreInfoSelector)
                return i;
        }
        return -1;
    }

    Anadir(infoSelector) {
        return this._infoSelectores.push(infoSelector);
    }

    Obtener(ind) {
        if (ind < 0 || ind >= this._infoSelectores.length)
            return undefined;
        return this._infoSelectores[ind];
    }
}

var infoSelectores = new InfoSelectores();


function MarcarParaSeleccionar(idGrid, idCheck) {
    var check = document.getElementById(idCheck);
    if (check.checked)
        anadirAlInfoSelector(idGrid, idCheck);
    else
        quitarDelSelector(idGrid, idCheck);
}

function anadirAlInfoSelector(idGrid, idCheck) {
    var ind = infoSelectores.Buscar(`${idGrid}`);
    if (ind < 0)
        ind = infoSelectores.Anadir(new InfoSelector(idGrid, 5, "")) - 1;
    var infoSelector = infoSelectores.Obtener(ind);
    var id = obtenerIdDeLaFila(idCheck);
    infoSelector.Insertar(id);
}

function obtenerIdDeLaFila(idCheck) {
    var inputId = document.getElementById(idCheck.replace("_chk_", `_id_`).replace("c_", "i_"));
    return inputId.value;
}
