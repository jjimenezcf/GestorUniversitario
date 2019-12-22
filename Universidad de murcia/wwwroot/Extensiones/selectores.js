function Seleccionar(idSelector, referenciaChecks, numColDeSeleccion) {
    
    function mapearAlSelector(valor) {
        var valorDelSelector = document.getElementById(idSelector).value;
        if (valorDelSelector !== undefined && valorDelSelector.trim() !== "")
            valorDelSelector = valorDelSelector + " | ";
        document.getElementById(idSelector).value = valorDelSelector + valor;
    }

    var checkboxes = $(`input[name='${referenciaChecks}']:checked`);
    var filasSeleccionadas = new Array();
    for (var x = 0; x < checkboxes.length; x++) {
        var posUlt = checkboxes[x].id.lastIndexOf("_");
        var idColumna = checkboxes[x].id.substring(0, posUlt + 1) + numColDeSeleccion;
        var elemento = document.getElementById(idColumna).value;
        filasSeleccionadas.push(elemento);
        console.log(elemento);
    }

    filasSeleccionadas.forEach(mapearAlSelector);
}
