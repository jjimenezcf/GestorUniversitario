

//************************************************************************************************************************************************************************************/

/// Funciones de navegación de un grid

//************************************************************************************************************************************************************************************/
function ObtenerFiltros(idGrid) {
    var arrayIds = ObtenerControlesDeFiltro(idGrid);
    var clausulas = new Array<ClausulaDeFiltrado>();
    for (let id of arrayIds) {
        var htmlImput: HTMLInputElement = <HTMLInputElement>document.getElementById(`${id}`);
        var tipo: string = htmlImput.getAttribute('tipo');
        var clausula: ClausulaDeFiltrado = null;
        if (tipo === "editor") {
            clausula = ObtenerClausulaEditor(htmlImput);
        }
        if (tipo === "selector") {
            clausula = (<HTMLSelector>htmlImput).ClausulaDeFiltrado();
        }
        if (clausula !== null)
            clausulas.push(clausula);
    }
    return JSON.stringify(clausulas);
}


function ObtenerClausulaEditor(htmlEditor) {
    var propiedad: string = htmlEditor.getAttribute('propiedad');
    var criterio: string = htmlEditor.getAttribute('criterio');
    var valor = htmlEditor.value;
    var clausula = null;
    if (!valor.IsNullOrEmpty())
        clausula = { propiedad: `${propiedad}`, criterio: `${criterio}`, valor: `${valor}` };

    return clausula;
}

function ObtenerControlesDeFiltro(idGrid) {
    var arrayIds = new Array();
    var htmlGrid = document.getElementById(`${idGrid}`);
    var idHtmlFiltro = htmlGrid.getAttribute("zonaDeFiltro");
    var htmlFiltro = document.getElementById(`${idHtmlFiltro}`);
    var arrayHtmlImput = htmlFiltro.getElementsByTagName('input');

    for (let i = 0; i < arrayHtmlImput.length; i++) {
        var htmlImput = arrayHtmlImput[i];
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
        var cantidad = (<HTMLInputElement>htmlImputCantidad).value.Numero();
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
        var cantidad = (<HTMLInputElement>htmlImputCantidad).value;
        var posicion = Number((<HTMLInputElement>htmlImputCantidad).getAttribute("posicion")) - 2 * cantidad.Numero();
        var controlador = htmlImputCantidad.getAttribute("controlador");
        if (posicion < 0)
            Leer(idGrid);
        else
            LeerDatosDelGrid(`/${controlador}/LeerDatosDelGrid?idGrid=${idGrid}&posicion=${posicion}&cantidad=${cantidad}&orden=PorApellido`, idGrid, SustituirGrid);
    }
}

function LeerSiguientes(idGrid: string) {
    var htmlImputCantidad = document.getElementById(`${idGrid}_nav_2_reg`);
    if (htmlImputCantidad === null)
        console.log(`El elemento ${idGrid}_nav_2_reg  no está definido`);
    else {
        var cantidad: number = (<HTMLInputElement>htmlImputCantidad).value.Numero();
        var posicion: number = htmlImputCantidad.getAttribute("posicion").Numero();
        var totalEnBd: number = htmlImputCantidad.getAttribute("totalEnBd").Numero();
        var controlador = htmlImputCantidad.getAttribute("controlador");
        if (totalEnBd > 0 && posicion + cantidad >= totalEnBd)
            LeerUltimos(idGrid);
        else
            LeerDatosDelGrid(`/${controlador}/LeerDatosDelGrid?idGrid=${idGrid}&posicion=${posicion}&cantidad=${cantidad}&orden=PorApellido`, idGrid, SustituirGrid);
    }
}

function LeerUltimos(idGrid) {
    var htmlImputCantidad = document.getElementById(`${idGrid}_nav_2_reg`);
    if (htmlImputCantidad === null)
        console.log(`El elemento${idGrid}_nav_2_reg  no está definido`);
    else {
        var cantidad: number = (<HTMLInputElement>htmlImputCantidad).value.Numero();
        var posicion: number = (<HTMLInputElement>htmlImputCantidad).getAttribute("totalEnBd").Numero() - cantidad;
        var controlador = htmlImputCantidad.getAttribute("controlador");
        if (posicion < 0)
            Leer(idGrid);
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
    var check = <HTMLInputElement>document.getElementById(idCheck);
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

