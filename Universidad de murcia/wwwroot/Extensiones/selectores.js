﻿
function AlAbrir(idGrid, columnaId, elementosMarcados) {

    infoSelectores.Borrar(idGrid);
    var infSel = new InfoSelector(idGrid);
    infSel.Modal(true);
    var marcados = elementosMarcados;
    marcarElementos(idGrid, columnaId, marcados);
    infSel.InsertarIds(marcados.split(';'));
    infoSelectores.Insertar(infSel);
}

function AlSeleccionar(idSelector, referenciaChecks, columnaId, columnaMostrar) {

    var htmlSelector = document.getElementById(idSelector);

    blanquearSelector(htmlSelector);

    /*TODO:
     * 
     * Recorrer la lista de del infoSelector asociado y de ahí se obtiene el id y el valor de la columna a mostrar.
     * Esto sustituye al código de abajo
     * 
     */

    var checkboxes = $(`input[name='${referenciaChecks}']:checked`);
    for (var x = 0; x < checkboxes.length; x++) {
        var elemento = obtenerElementoSeleccionado(checkboxes[x].id, columnaId, columnaMostrar);
        mapearValoresAlSelector(htmlSelector, elemento);
    }
    cerrar(idSelector,referenciaChecks);
}

function obtenerElementoSeleccionado(idCheck, columnaId, columnaMostrar) {    
    var e = {
        id: parseInt(ObtenerIdDeLaFilaChequeada(idCheck)),
        valor: obtenerValorDeLaColumnaChequeada(idCheck, columnaMostrar)
    };

    return e;
}



function AlCerrar(idModal, idGrid,  referenciaChecks) {
    console.log(`se ha cerrado la modal ${idModal}, hay que desmarcar los checks de la modal`);
    cerrar(idGrid, referenciaChecks);
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

function cerrar(idGrid, referenciaChecks) {
    blanquearCheck(referenciaChecks);
    infoSelectores.Borrar(idGrid);
}

function marcarElementos(idGrid, columnaId, marcados) {

    var arrayDeIds = marcados.split(";");
    if (arrayDeIds.length === 0 || (arrayDeIds.length === 1 && arrayDeIds[0]===""))
        return;

    var celdasId = document.getElementsByName(`${columnaId}.${idGrid}`);
    var len = celdasId.length;
    for (var i = 0; i < arrayDeIds.length; i++) {
        if (parseInt(arrayDeIds[i]) <= 0)
            continue;

        for (var j = 0; j < len; j++) {
            if (celdasId[j].value === arrayDeIds[i]) {
                var idCheck = celdasId[j].id.replace(`.${columnaId}`, ".chksel");
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

