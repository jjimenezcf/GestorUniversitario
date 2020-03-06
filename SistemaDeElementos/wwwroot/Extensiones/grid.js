﻿

//************************************************************************************************************************************************************************************/

/// Funciones de navegación de un grid

//************************************************************************************************************************************************************************************/
function ObtenerFiltros(idGrid) {
    var arrayIds = ObtenerControlesDeFiltro(idGrid);
    var clausulas = new Array();
    for (let id of arrayIds) {
        var htmlImput = document.getElementById(`${id}`);
        var tipo = htmlImput.getAttribute('tipo');
        var clausula = null;
        if (tipo === "editor") {
            clausula = ObtenerClausulaEditor(htmlImput);
        }
        if (tipo === "selector") {
            clausula = ObtenerClausulaSelector(htmlImput);
        }
        if (clausula)
            clausulas.push(clausula);
    }
    return JSON.stringify(clausulas);
}

function ObtenerClausulaSelector(htmlSelector) {
    var propiedad = htmlSelector.getAttribute('propiedad');
    var criterio = htmlSelector.getAttribute('criterio');
    var valor = null;
    var clausula = null;
    if (htmlSelector.hasAttribute("idsSeleccionados")) {
        ids = htmlSelector.getAttribute("idsSeleccionados");
        if (!ids.isNullOrEmpty()) {
            valor = ids;
            clausula = { propiedad: `${propiedad}`, criterio: `${criterio}`, valor: `${valor}` };
        }
    }
    return clausula;
}

function ObtenerClausulaEditor(htmlEditor) {
    var propiedad = htmlEditor.getAttribute('propiedad');
    var criterio = htmlEditor.getAttribute('criterio');
    var valor = htmlEditor.value;
    var clausula = null;
    if (!valor.isNullOrEmpty())
        clausula = {propiedad: `${propiedad}`, criterio: `${criterio}`, valor: `${valor}`};

    return clausula;
}

function ObtenerControlesDeFiltro(idGrid) {
    var arrayIds = new Array();
    var htmlGrid = document.getElementById(`${idGrid}`);
    var idHtmlFiltro = htmlGrid.getAttribute("zonaDeFiltro");
    var htmlFiltro = document.getElementById(`${idHtmlFiltro}`);
    var arrayHtmlImput = htmlFiltro.getElementsByTagName('input');

    for (let htmlImput of arrayHtmlImput) {
        var esFiltro = htmlImput.getAttribute('filtro');
        if (esFiltro === 'S') {
            var id = htmlImput.getAttribute('Id');
            if (id === null)
                console.log(`Falta el atributo id del componente de filtro ${htmlImput}`);
            arrayIds.push(htmlImput.getAttribute('Id'));
        }
    }
    return arrayIds;
}

function Leer(idGrid) {
    var htmlImputCantidad = document.getElementById(`${idGrid}_nav_2_reg`);
    if (htmlImputCantidad === null)
        console.log(`El elemento ${idGrid}_nav_2_reg  no está definido`);
    else {
        var cantidad = htmlImputCantidad.value;
        var controlador = htmlImputCantidad.getAttribute("controlador");
        var filtroJson = ObtenerFiltros(idGrid);
        LeerDatosDelGrid(`/${controlador}/LeerDatosDelGrid?idGrid=${idGrid}&posicion=${0}&cantidad=${cantidad}&filtro=${filtroJson}&orden=PorApellido`, idGrid, SustituirGrid);
    }
}

function LeerAnteriores(idGrid) {
    var htmlImputCantidad = document.getElementById(`${idGrid}_nav_2_reg`);
    if (htmlImputCantidad === null)
        console.log(`El elemento ${idGrid}_nav_2_reg  no está definido`);
    else {
        var cantidad = htmlImputCantidad.value;
        var posicion = htmlImputCantidad.getAttribute("posicion") - 2 * cantidad;
        var controlador = htmlImputCantidad.getAttribute("controlador");
        if (posicion < 0)
            Leer(idGrid, controlador);
        else
            LeerDatosDelGrid(`/${controlador}/LeerDatosDelGrid?idGrid=${idGrid}&posicion=${posicion}&cantidad=${cantidad}&orden=PorApellido`, idGrid, SustituirGrid);
    }
}

function LeerSiguientes(idGrid) {
    var htmlImputCantidad = document.getElementById(`${idGrid}_nav_2_reg`);
    if (htmlImputCantidad === null)
        console.log(`El elemento ${idGrid}_nav_2_reg  no está definido`);
    else {
        var cantidad = htmlImputCantidad.value;
        var posicion = htmlImputCantidad.getAttribute("posicion");
        var totalEnBd = htmlImputCantidad.getAttribute("totalEnBd");
        var controlador = htmlImputCantidad.getAttribute("controlador");
        if (posicion + cantidad >= totalEnBd)
            LeerUltimos(idGrid, controlador);
        else
            LeerDatosDelGrid(`/${controlador}/LeerDatosDelGrid?idGrid=${idGrid}&posicion=${posicion}&cantidad=${cantidad}&orden=PorApellido`, idGrid, SustituirGrid);
    }
}

function LeerUltimos(idGrid) {
    var htmlImputCantidad = document.getElementById(`${idGrid}_nav_2_reg`);
    if (htmlImputCantidad === null)
        console.log(`El elemento${idGrid}_nav_2_reg  no está definido`);
    else {
        var cantidad = htmlImputCantidad.value;
        var posicion = htmlImputCantidad.getAttribute("totalEnBd") - cantidad;
        var controlador = htmlImputCantidad.getAttribute("controlador");
        if (posicion < 0)
            Leer(idGrid, controlador);
        else
            LeerDatosDelGrid(`/${controlador}/LeerDatosDelGrid?idGrid=${idGrid}&posicion=${posicion}&cantidad=${cantidad}&orden=PorApellido`, idGrid, SustituirGrid);
    }
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

function SustituirGrid(idGrid, htmlGrid) {
    var htmlContenedorGrid = document.getElementById(`${idGrid}`);
    if (!htmlGrid) {
        console.log(`No se ha localizado el contenedor ${idGrid}`);
        return;
    }

    htmlContenedorGrid.innerHTML = htmlGrid;
    if (infoSelectores.Cantidad > 0) {
        var infSel = infoSelectores.Obtener(idGrid);
        if (infSel !== undefined && infSel.Cantidad > 0) {
            marcarElementos(idGrid, 'id', infSel);
            infSel.SincronizarCheck();
        }
    }

}


function AlPulsarUnCheckDeSeleccion(idGrid, idCheck) {
    var check = document.getElementById(idCheck);
    if (check.checked)
        AnadirAlInfoSelector(idGrid, idCheck);
    else
        QuitarDelSelector(idGrid, idCheck);
}


function AnadirAlInfoSelector(idGrid, idCheck) {

    var infSel = infoSelectores.Obtener(idGrid);
    if (infSel === undefined) {
        infSel = new InfoSelector(idGrid);
        infoSelectores.Insertar(infSel);
    }

    var id = ObtenerIdDeLaFilaChequeada(idCheck);
    if (infSel.EsModalDeSeleccion) {
        var textoMostrar = obtenerValorDeLaColumnaChequeada(idCheck, infSel.ColumnaMostrar);
        infSel.InsertarElemento(id, textoMostrar);
    }
    else {
        infSel.InsertarId(id);
    }

}


function QuitarDelSelector(idGrid, idCheck) {

    var infSel = infoSelectores.Obtener(idGrid);
    if (infSel !== undefined) {
        var id = ObtenerIdDeLaFilaChequeada(idCheck);
        infSel.Quitar(id);
    }
    else
        console.log(`El selector ${idGrid} no está definido`);
}


//************************************************************************************************************************************************************************************/

/// procesar un json con las filas del grid (en desuso)

//************************************************************************************************************************************************************************************/
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