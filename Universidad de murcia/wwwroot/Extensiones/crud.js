function Leer(idGrid, controlador) {
    realizarPeticion(`/${controlador}/Leer?cantidad=10,posicion=0`, mapearFilasAlGrid(idGrid));
}

function LeerAnteriores(controlador) {
    alert(`/${controlador}/Leer?cantidad=${0},posicion${0}`);
}

function LeerSiguientes(controlador) {
    alert(`/${controlador}/Leer?cantidad=${0},posicion${0}`);
}

function LeerUltimos(controlador) {
    alert(`/${controlador}/Leer?cantidad=${0},posicion${0}`);
}


function mapearFilasAlGrid(idGrid, respuesta) {
    $(`#${idGrid} tbody`).remove();
    console.log(respuesta);
    $.each(respuesta, function (i, f) {
        var tblRow = "<tr>" + mapearFila(respuesta(i)) + "</tr>";
        $(tblRow).appendTo("#resultdata tbody");
    });
}

function renderDetalleGrid(idGrid, filas) {
}

function renderFilaSeleccionable(idGrid, i, fila) {
}

function renderFila(numFil, fila) {
}

function renderCeldaCheck(idGrid, idCelda){
}

function renderCelda(celda) {
}