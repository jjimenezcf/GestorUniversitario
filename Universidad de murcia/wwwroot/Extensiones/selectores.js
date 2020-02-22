


function AlAbrir(idGrid, idSelector, columnaId, columnaMostrar) {

    recargarGrid(idGrid);

    infoSelectores.Borrar(idGrid);
    var infSel = new InfoSelector(idGrid);
    infSel.Modal(columnaMostrar);
    var arrayMarcados = elementosMarcados(idSelector);
    infSel.InsertarElementos(arrayMarcados);
    infoSelectores.Insertar(infSel);

    marcarElementos(idGrid, columnaId, infSel);
    infSel.SincronizarCheck();
}

function AlCerrar(idModal, idGrid, referenciaChecks) {
    console.log(`se ha cerrado la modal ${idModal}, hay que desmarcar los checks de la modal`);
    cerrar(idGrid, referenciaChecks);
}

function AlSeleccionar(idSelector, idGrid, referenciaChecks) {

    var htmlSelector = document.getElementById(idSelector);

    blanquearSelector(htmlSelector);

    //var checkboxes = $(`input[name='${referenciaChecks}']:checked`);
    var infSel = infoSelectores.Obtener(idGrid);

    for (var x = 0; x < infSel.Cantidad; x++) {
        var elemento = infSel.LeerElemento(x);
        if (elemento.id > 0 && elemento.valor !== undefined)
            mapearElementoAlHtmlSelector(htmlSelector, elemento);
        else
            console.log(`Se ha leido mal el elemento del selector ${idGrid} de la posición ${x}`);

    }
    cerrar(idGrid, referenciaChecks);
}

function recargarGrid(idGrid) {
    var htmlImputCantidad = document.getElementById(`${idGrid}_nav_2_reg`);
    if (htmlImputCantidad === null)
        console.log(`El elemento ${idGrid}_nav_2_reg  no está definido`);
    else {
        var cantidad = htmlImputCantidad.value;
        var posicion = htmlImputCantidad.getAttribute("posicion");
        if (posicion - cantidad !== 0)
            Leer(idGrid);
    }
}

function obtenerElementoSeleccionado(idCheck, columnaMostrar) {
    var e = {
        id: parseInt(ObtenerIdDeLaFilaChequeada(idCheck)),
        valor: obtenerValorDeLaColumnaChequeada(idCheck, columnaMostrar)
    };

    return e;
}


function elementosMarcados(idSelector) {

    var ids = "";
    var elementos = new Array();
    var htmlSelector = document.getElementById(idSelector);
    if (htmlSelector.hasAttribute("idsSeleccionados")) {
        ids = htmlSelector.getAttribute("idsSeleccionados");
        if (!ids.isNullOrEmpty()) {
            var listaNombres = htmlSelector.value.split('|');
            var listaIds = ids.split(';');
            for (var i = 0; i < listaIds.length; i++) {
                var e = { id: listaIds[i], valor: listaNombres[i] };
                elementos.push(e);
            }
        }
    }

    return elementos;
}


function mapearElementoAlHtmlSelector(htmlSelector, elemento) {

    var valorDelSelector = htmlSelector.value;
    if (!valorDelSelector.isNullOrEmpty())
        valorDelSelector = valorDelSelector + " | ";

    htmlSelector.value = valorDelSelector + elemento.valor;
    mapearIdAlHtmlSelector(htmlSelector, elemento.id);

}

function mapearIdAlHtmlSelector(htmlSelector, id) {
    var listaDeIds = htmlSelector.getAttribute("idsSeleccionados");
    if (listaDeIds === null) {
        atributo = document.createAttribute("idsSeleccionados");
        htmlSelector.setAttributeNode(atributo);
        listaDeIds = "";
    }

    if (!listaDeIds.isNullOrEmpty())
        listaDeIds = listaDeIds + ';';
    listaDeIds = listaDeIds + id;
    htmlSelector.setAttribute("idsSeleccionados", listaDeIds);
}


function blanquearSelector(htmlSelector) {
    htmlSelector.value = "";
    if (htmlSelector.hasAttribute("idsSeleccionados")) {
        var listaDeIds = htmlSelector.getAttribute("idsSeleccionados");
        listaDeIds = "";
        htmlSelector.setAttribute("idsSeleccionados", listaDeIds);
    }
}

function cerrar(idGrid, referenciaChecks) {
    blanquearCheck(referenciaChecks);
    infoSelectores.Borrar(idGrid);
}

function marcarElementos(idGrid, columnaId, infSel) {

    if (infSel.Cantidad === 0)
        return;

    var celdasId = document.getElementsByName(`${columnaId}.${idGrid}`);
    var len = celdasId.length;
    for (var i = 0; i < infSel.Cantidad; i++) {
        for (var j = 0; j < len; j++) {
            var id = infSel.LeerId(i);
            if (celdasId[j].value === id) {
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


function BuscarRegistro(idSelector, controlador) {
    var htmlSelector = document.getElementById(idSelector);
    if (!htmlSelector.value.isNullOrEmpty()) {
        var clausulas = ObtenerClausulaParaBuscarRegistro(htmlSelector);
        LeerParaSelector(`/${controlador}/Leer?filtro=${JSON.stringify(clausulas)}`, ProcesarRegistrosLeidos);
    }
    else {
        console.log(`no hay valor para el selector ${idSelector} y controlador ${controlador}`);
    }

}

function ObtenerClausulaParaBuscarRegistro(htmlSelector) {
    var propiedad = htmlSelector.getAttribute('propiedadBuscar');
    var criterio = htmlSelector.getAttribute('criterioBuscar');
    var valor = htmlSelector.value;


    var idGridModal = htmlSelector.getAttribute('idGridModal');
    var chekSeleccionModal = htmlSelector.getAttribute('chekSeleccionModal');
    blanquearSelector(htmlSelector);
    cerrar(idGridModal, chekSeleccionModal);

    var clausula = { propiedad: `${propiedad}`, criterio: `${criterio}`, valor: `${valor}` };
    var clausulas = new Array();
    clausulas.push(clausula);
    return clausulas;
}

function LeerParaSelector(url, funcionDeRespuesta) {

    function respuestaCorrecta() {
        if (req.status >= 200 && req.status < 400) {
            funcionDeRespuesta(req.responseText);
        }
        else {
            console.log(req.status + ' ' + req.statusText);
        }
    }

    function respuestaErronea() {
        console.log('Error de conexión');
    }

    var req = new XMLHttpRequest();
    req.open('GET', url, true);
    req.addEventListener("load", respuestaCorrecta);
    req.addEventListener("error", respuestaErronea);
    req.send();
}

function ProcesarRegistrosLeidos(registros) {
    console.log(registros);
}

