function Leer(idGrid, controlador) {
    LeerDatosDelGrid(`/${controlador}/Leer?idGrid=${idGrid}&posicion=${0}&cantidad=${5}&orden=PorApellido`, idGrid, sustituirGrid);
}

function LeerAnteriores(controlador) {
    LeerDatosDelGrid(`/${controlador}/LeerAnteriores?idGrid=${idGrid}&posicion=${5}&cantidad=${5}&orden=PorApellido`, idGrid, sustituirGrid);
}

function LeerSiguientes(idGrid, controlador) {
    LeerDatosDelGrid(`/${controlador}/LeerSiguientes?idGrid=${idGrid}&posicion=${5}&cantidad=${5}&orden=PorApellido`, idGrid, sustituirGrid);
}

function LeerUltimos(controlador) {
    alert(`/${controlador}/Leer?posicion=${0},cantidad=${3}`);
}

function LeerDatosDelGrid(url, idGrid, funcionDeRespuesta) {
    function respuestaCorrecta() {
        if (req.status >= 200 && req.status < 400) {
            funcionDeRespuesta(idGrid, req.responseText);
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

function RenderSiguientes(url, idGrid, funcionDeRespuesta) {
    function respuestaCorrecta() {
        if (req.status >= 200 && req.status < 400) {
            funcionDeRespuesta(idGrid, req.responseText);
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

function sustituirGrid(idGrid, htmlGrid) {
    var htmlContenedorGrid = document.getElementById(`contenedor_${idGrid}`);
    console.log(htmlGrid);
    htmlContenedorGrid.innerHTML = htmlGrid;

}


function renderDetalleGrid(idGrid, respuesta) {
    var filas = JSON.parse(respuesta);
    var htmlDetalleGrid = new StringBuilder();
    var i = 0;
    for (i = 0; i < filas.length; i++) {
        var htmlFila = renderFilaSeleccionable(idGrid, i, filas[i]);
        htmlDetalleGrid.appendLine(htmlFila); 
    }
    var body = $(`#${idGrid} tbody`);
    body.html(htmlDetalleGrid.toString());
}

function renderFilaSeleccionable(idGrid, numFil, fila) {
    var htmlfila = renderFila(idGrid, numFil, fila);
    var htmlfilaseleccionable = `<tr id='${idGrid}_f${numFil}'>${newLine}${htmlfila}</tr>`;
    return htmlfilaseleccionable;
}

function renderFila(idGrid, numFil, fila) {
    var htmlCeldas = new StringBuilder();
    var celdas = new Celdas(idGrid, numFil, fila);
    var i = 0;
    for (i = 0; i < celdas.items.length; i++) {
        var htmlCelda = renderCelda(celdas.items[i]);
        htmlCeldas.append(htmlCelda);
    }
    return htmlCeldas.toString();
}

function renderCeldaCheck(idGrid, idCelda){
}

function renderCelda(celda) {
    return `<td id='${celda.id}' name='${celda.nombre}' ${celda.visible} ${celda.alineada} >${celda.valor}</td>${newLine}`;
}