function AlSeleccionar(idSelector, referenciaChecks, numColDeSeleccion) {

    var selector = document.getElementById(idSelector);

    blanquearSelector(selector);
    var checkboxes = $(`input[name='${referenciaChecks}']:checked`);
    for (var x = 0; x < checkboxes.length; x++) {
        var posUlt = checkboxes[x].id.lastIndexOf("_");
        var idColumna = checkboxes[x].id.substring(0, posUlt + 1) + numColDeSeleccion;
        var elemento = {
            id: document.getElementById(idColumna).value.length,
            valor: document.getElementById(idColumna).value
        };
        mapearValoresAlSelector(selector, elemento);
    }
    cerrar(referenciaChecks);
}

function AlAbrir(idModal, elementosMarcados, referenciaChecks) {
    console.log(`se ha abierto la modal ${idModal}, hay que marcar los checks del selector`);
    marcarElementos(idModal, elementosMarcados, referenciaChecks);
}


function AlCerrar(idModal, referenciaChecks) {
    console.log(`se ha abierto la modal ${idModal}, hay que desmarcar los checks del selector`);
    cerrar(referenciaChecks);
}

function ElementosMarcados(idSelector) {

    var elementosMarcados = "";
    var selector = document.getElementById(idSelector);
    if (selector.hasAttribute("idsSeleccionados")) {
        var a = selector.getAttribute("idsSeleccionados");
        elementosMarcados = a.value;
    }

    return elementosMarcados;
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
        listaDeIds.value = "";
        selector.setAttribute("idsSeleccionados", listaDeIds);
    }
}

function cerrar(referenciaChecks) {
    blanquearCheck(referenciaChecks);
}

function marcarElementos(idModal, elementosMarcados, referenciaChecks) {
    console.log(`recoorer los elementos del la  ${idModal} y marcar lo elementosMarcados`);
}

function blanquearCheck(referenciaChecks) {
    var checkboxes = $(`input[name='${referenciaChecks}']:checked`);

    for (var x = 0; x < checkboxes.length; x++) {
        checkboxes[x].checked = false;
    }

}

