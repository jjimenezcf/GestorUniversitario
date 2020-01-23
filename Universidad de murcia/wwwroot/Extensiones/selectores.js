
function AlAbrir(idGrid, columnaId, elementosMarcados) {
    var seleccionados = elementosMarcados;
    //var navegador = document.getElementById(`Nav-${idGrid}-Reg`);
    var infoSelector = new InfoSelector(idGrid, 1, seleccionados);
    marcarElementos(idGrid, columnaId, infoSelector);
}

function AlSeleccionar(idSelector, referenciaChecks, columnaId, columnaMostrar) {

    var selector = document.getElementById(idSelector);

    blanquearSelector(selector);
    var checkboxes = $(`input[name='${referenciaChecks}']:checked`);
    for (var x = 0; x < checkboxes.length; x++) {
        var elemento = obtenerElementoSeleccionado(checkboxes[x].id, columnaId, columnaMostrar);
        mapearValoresAlSelector(selector, elemento);
    }
    cerrar(referenciaChecks);
}

function obtenerElementoSeleccionado(idCheck, columnaId, columnaMostrar) {

    //
    // remplazar la c por la c_ i_
    // remplazar la _chk_ por _columnaId_
    //
    //

    var inputId = document.getElementById(idCheck.replace("_chk_", `_${columnaId}_`).replace("c_", "i_"));
    var inputMostra = document.getElementById(idCheck.replace("_chk_", `_${columnaMostrar}_`).replace("c_", "i_"));

    var e = {
        id: parseInt(inputId.value),
        valor: inputMostra.value
    };

    return e;
}



function AlCerrar(idModal, referenciaChecks) {
    console.log(`se ha cerrado la modal ${idModal}, hay que desmarcar los checks de la modal`);
    cerrar(referenciaChecks);
}

function ElementosMarcados(idSelector) {

    var seleccionados = "";
    var selector = document.getElementById(idSelector);
    if (selector.hasAttribute("idsSeleccionados")) {
        seleccionados = selector.getAttribute("idsSeleccionados");
    }

    return seleccionados;
}


function mapearValoresAlSelector(selector, elemento) {

    var valorDelSelector = selector.value;
    if (valorDelSelector !== undefined && valorDelSelector.trim() !== "")
        valorDelSelector = valorDelSelector + " | ";
    selector.value = valorDelSelector + elemento.valor;

    mapearIdsAlSelector(selector, elemento.id);

}

function mapearIdsAlSelector(selector, id) {
    var listaDeIds = selector.getAttribute("idsSeleccionados");
    if (listaDeIds === null) {
        atributo = document.createAttribute("idsSeleccionados");
        selector.setAttributeNode(atributo);
        listaDeIds = "";
    }

    if (listaDeIds.trim() !== "")
        listaDeIds = listaDeIds + ';';
    listaDeIds = listaDeIds + id;
    selector.setAttribute("idsSeleccionados", listaDeIds);
}


function blanquearSelector(selector) {
    selector.value = "";
    if (selector.hasAttribute("idsSeleccionados")) {
        var listaDeIds = selector.getAttribute("idsSeleccionados");
        listaDeIds = "";
        selector.setAttribute("idsSeleccionados", listaDeIds);
    }
}

function cerrar(referenciaChecks) {
    blanquearCheck(referenciaChecks);

}

function marcarElementos(idGrid, columnaId, infoSelector) {

    var array = infoSelector.Seleccionados;
    if (array.length === 0)
        return;

    var celdasId = document.getElementsByName(`i_${idGrid}_${columnaId}`);
    var len = celdasId.length;
    for (var i = 0; i < array.length; i++) {
        for (var j = 0; j < len; j++) {
            if (celdasId[j].value === array[i]) {
                var idCheck = celdasId[j].id.replace(`_${columnaId}_`, "_chk_").replace("i_", "c_");
                var check = document.getElementById(idCheck);
                check.checked = true;
                break;
            }
        }
    }
}

function blanquearCheck(referenciaChecks) {
    var checkboxes = $(`input[name='${referenciaChecks}']:checked`);

    for (var x = 0; x < checkboxes.length; x++) {
        checkboxes[x].checked = false;
    }

}

