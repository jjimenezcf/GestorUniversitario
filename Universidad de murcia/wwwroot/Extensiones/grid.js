function Leer(idGrid, controlador) {
    var htmlImputCantidad = document.getElementById(`Nav-${idGrid}-Reg`);
    if (htmlImputCantidad === null)
        console.log(`El elemento Nav-${idGrid}-Reg  no está definido`);
    else {
        var cantidad = htmlImputCantidad.value;
        LeerDatosDelGrid(`/${controlador}/Leer?idGrid=${idGrid}&posicion=${0}&cantidad=${cantidad}&orden=PorApellido`, idGrid, sustituirGrid);
    }
}

function LeerAnteriores(idGrid, controlador) {
    var htmlImputCantidad = document.getElementById(`Nav-${idGrid}-Reg`);
    if (htmlImputCantidad === null)
        console.log(`El elemento Nav-${idGrid}-Reg  no está definido`);
    else {
        var cantidad = htmlImputCantidad.value;
        var posicion = htmlImputCantidad.getAttribute("posicion") - 2 * cantidad;
        if (posicion < 0)
            Leer(idGrid, controlador);
        else
            LeerDatosDelGrid(`/${controlador}/LeerSiguientes?idGrid=${idGrid}&posicion=${posicion}&cantidad=${cantidad}&orden=PorApellido`, idGrid, sustituirGrid);
    }
}

function LeerSiguientes(idGrid, controlador) {
    var htmlImputCantidad = document.getElementById(`Nav-${idGrid}-Reg`);
    if (htmlImputCantidad === null)
        console.log(`El elemento Nav-${idGrid}-Reg  no está definido`);
    else {
        var cantidad = htmlImputCantidad.value;
        var posicion = htmlImputCantidad.getAttribute("posicion");
        var totalEnBd = htmlImputCantidad.getAttribute("totalEnBd");

        if (posicion + cantidad >= totalEnBd)
            LeerUltimos(idGrid, controlador);
        else
            LeerDatosDelGrid(`/${controlador}/LeerSiguientes?idGrid=${idGrid}&posicion=${posicion}&cantidad=${cantidad}&orden=PorApellido`, idGrid, sustituirGrid);
    }
}

function LeerUltimos(idGrid, controlador) {
    var htmlImputCantidad = document.getElementById(`Nav-${idGrid}-Reg`);
    if (htmlImputCantidad === null)
        console.log(`El elemento Nav-${idGrid}-Reg  no está definido`);
    else {
        var cantidad = htmlImputCantidad.value;
        var posicion = htmlImputCantidad.getAttribute("totalEnBd") - cantidad;
        if (posicion < 0)
            Leer(idGrid, controlador);
        else
            LeerDatosDelGrid(`/${controlador}/LeerSiguientes?idGrid=${idGrid}&posicion=${posicion}&cantidad=${cantidad}&orden=PorApellido`, idGrid, sustituirGrid);
    }
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


//************************************************************************************************************************************************************************************/

/// procesar un json con las filas del grid (en desuso)

//************************************************************************************************************************************************************************************/

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

function renderCeldaCheck(idGrid, idCelda) {
}

function renderCelda(celda) {
    return `<td id='${celda.id}' name='${celda.nombre}' ${celda.visible} ${celda.alineada} >${celda.valor}</td>${newLine}`;
}