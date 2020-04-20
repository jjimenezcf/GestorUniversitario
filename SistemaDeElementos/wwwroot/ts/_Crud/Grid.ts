

//************************************************************************************************************************************************************************************/

/// Funciones de navegación de un grid

//************************************************************************************************************************************************************************************/
function ObtenerFiltros(idGrid) {
    var arrayIds = ObtenerControlesDeFiltro(idGrid);
    var clausulas = new Array<ClausulaDeFiltrado>();
    for (let id of arrayIds) {
        var htmlImput: HTMLInputElement = <HTMLInputElement>document.getElementById(`${id}`);
        var tipo: string = htmlImput.getAttribute(TipoControl.Tipo);
        var clausula: ClausulaDeFiltrado = null;
        if (tipo === TipoControl.Editor) {
            clausula = ObtenerClausulaEditor(htmlImput);
        }
        if (tipo === TipoControl.Selector) {
            clausula = (<HTMLSelector>htmlImput).ClausulaDeFiltrado();
        }
        if (clausula !== null)
            clausulas.push(clausula);
    }
    return JSON.stringify(clausulas);
}

function ObtenerClausulaEditor(htmlEditor) {
    var propiedad: string = htmlEditor.getAttribute(Atributo.propiedad);
    var criterio: string = htmlEditor.getAttribute(Atributo.criterio);
    var valor = htmlEditor.value;
    var clausula = null;
    if (!EsNula(valor))
        clausula = { propiedad: `${propiedad}`, criterio: `${criterio}`, valor: `${valor}` };

    return clausula;
}

function ObtenerControlesDeFiltro(idGrid) {
    var arrayIds = new Array();
    var htmlGrid = document.getElementById(`${idGrid}`);
    var idHtmlFiltro = htmlGrid.getAttribute(Atributo.zonaDeFiltro);
    var htmlFiltro = document.getElementById(`${idHtmlFiltro}`);
    var arrayHtmlImput = htmlFiltro.getElementsByTagName(TagName.input);

    for (let i = 0; i < arrayHtmlImput.length; i++) {
        var htmlImput = arrayHtmlImput[i];
        var esFiltro = htmlImput.getAttribute(Atributo.filtro);
        if (esFiltro === 'S') {
            var id = htmlImput.getAttribute(Atributo.Id);
            if (id === null)
                console.log(`Falta el atributo id del componente de filtro ${htmlImput}`);
            arrayIds.push(htmlImput.getAttribute(Atributo.Id));
        }
    }
    return arrayIds;
}

function PeticionSincrona(htmlInputCantidad: HTMLInputElement, idGrid: string, posicion: number): XMLHttpRequest {
    BlanquearMensaje();
    var cantidad = htmlInputCantidad.value.Numero();
    var controlador = htmlInputCantidad.getAttribute(Atributo.controlador);
    var filtroJson = ObtenerFiltros(idGrid);
    var ordenJson = '[]';

    let url: string = `/${controlador}/${Ajax.EndPoint.LeerGridEnHtml}`;
    let parametros: string = `${Ajax.Param.modo}=Seleccion` +
        `&${Ajax.Param.posicion}=${posicion}` +
        `&${Ajax.Param.cantidad}=${cantidad}` +
        `&${Ajax.Param.filtro}=${filtroJson}` +
        `&${Ajax.Param.orden}=${ordenJson}`;
    let peticion: string = url + '?' + parametros;

    let req: XMLHttpRequest = new XMLHttpRequest();
    req.open('GET', peticion, false);
    return req;
}

function Leer(idGrid) {
    let idCrtlCantidad: string = `${idGrid}_${LiteralMnt.idCtrlCantidad}`;
    let htmlImputCantidad: HTMLInputElement = <HTMLInputElement>document.getElementById(`${idCrtlCantidad}`);

    if (htmlImputCantidad === null)
        Mensaje(TipoMensaje.Error, `El elemento ${idCrtlCantidad} no está definido`);
    else {
        let req: XMLHttpRequest = PeticionSincrona(htmlImputCantidad, idGrid, 0);
        LeerDatosDelGrid(req, idGrid, Ajax.EndPoint.LeerGridEnHtml, () => SustituirGrid(req, idGrid), () => ErrorEnLaPeticion(req));
    }
}

function LeerAnteriores(idGrid) {
    let idCrtlCantidad: string = `${idGrid}_${LiteralMnt.idCtrlCantidad}`;
    let htmlImputCantidad: HTMLInputElement = <HTMLInputElement>document.getElementById(`${idCrtlCantidad}`);

    if (htmlImputCantidad === null)
        Mensaje(TipoMensaje.Error, `El elemento ${idCrtlCantidad}  no está definido`);
    else {
        let cantidad = (<HTMLInputElement>htmlImputCantidad).value;
        let posicion = Number((<HTMLInputElement>htmlImputCantidad).getAttribute(Atributo.posicion)) - 2 * cantidad.Numero();
        if (posicion < 0)
            Leer(idGrid);
        else {
            let req: XMLHttpRequest = PeticionSincrona(htmlImputCantidad, idGrid, posicion);
            LeerDatosDelGrid(req, idGrid, Ajax.EndPoint.LeerGridEnHtml, () => SustituirGrid(req, idGrid), () => ErrorEnLaPeticion(req));
        }
    }
}

function LeerSiguientes(idGrid: string) {
    let idCrtlCantidad: string = `${idGrid}_${LiteralMnt.idCtrlCantidad}`;
    var htmlImputCantidad: HTMLInputElement = <HTMLInputElement>document.getElementById(`${idCrtlCantidad}`);
    if (htmlImputCantidad === null)
        Mensaje(TipoMensaje.Error, `El elemento ${idCrtlCantidad} no está definido`);
    else {
        var cantidad: number = (<HTMLInputElement>htmlImputCantidad).value.Numero();
        var posicion: number = htmlImputCantidad.getAttribute(Atributo.posicion).Numero();
        var totalEnBd: number = htmlImputCantidad.getAttribute(Atributo.totalEnBd).Numero();
        if (totalEnBd > 0 && posicion + cantidad >= totalEnBd)
            LeerUltimos(idGrid);
        else {
            let req: XMLHttpRequest = PeticionSincrona(htmlImputCantidad, idGrid, posicion);
            LeerDatosDelGrid(req, idGrid, Ajax.EndPoint.LeerGridEnHtml, () => SustituirGrid(req, idGrid), () => ErrorEnLaPeticion(req));
        }
    }
}

function LeerUltimos(idGrid) {
    let idCrtlCantidad: string = `${idGrid}_${LiteralMnt.idCtrlCantidad}`;
    var htmlImputCantidad: HTMLInputElement = <HTMLInputElement>document.getElementById(`${idCrtlCantidad}`);
    if (htmlImputCantidad === null)
        Mensaje(TipoMensaje.Error, `El elemento${idCrtlCantidad} no está definido`);
    else {
        let cantidad: number = (<HTMLInputElement>htmlImputCantidad).value.Numero();
        let totalEnBd = (<HTMLInputElement>htmlImputCantidad).getAttribute(Atributo.totalEnBd).Numero();
        if (isNaN(totalEnBd) || totalEnBd == 0)
            Mensaje(TipoMensaje.Info, "No está definido el número de registros totales en BD, operación no realizable");
        else {
            let posicion: number = totalEnBd - cantidad;
            let req: XMLHttpRequest = PeticionSincrona(htmlImputCantidad, idGrid, posicion);
            LeerDatosDelGrid(req, idGrid, Ajax.EndPoint.LeerGridEnHtml, () => SustituirGrid(req, idGrid), () => ErrorEnLaPeticion(req));
        }
    }
}

function LeerDatosDelGrid(req: XMLHttpRequest, idGrid: string, peticion: string,  sustituirGrid: Function, errorEnLaPeticion: Function) {

    function respuestaCorrecta() {
        if (EsNula(req.response)) {
            errorEnLaPeticion();
        }
        else {
            var resultado: any = ParsearRespuesta(req, peticion);
            if (resultado.estado === Ajax.jsonResultError) {
                errorEnLaPeticion();
            }
            else {
                sustituirGrid(idGrid, req.responseText);
            }
        }
    }

    function respuestaErronea() {
        errorEnLaPeticion();
    }

    req.addEventListener(Ajax.eventoLoad, respuestaCorrecta);
    req.addEventListener(Ajax.eventoError, respuestaErronea);
    req.send();
}

function ErrorEnLaPeticion(req: XMLHttpRequest) {
    if (EsNula(req.response)) {
        Mensaje(TipoMensaje.Error, `La peticion ${Ajax.EndPoint.LeerGridEnHtml} no está definida`);
    }
    else {
        let resultado = JSON.parse(req.response);
        Mensaje(TipoMensaje.Error, resultado.mensaje);
        console.error(resultado.consola);
    }
}

function SustituirGrid(req: XMLHttpRequest, idGrid: string) {
    let resultado = JSON.parse(req.response);

    var htmlContenedorGrid = document.getElementById(`${idGrid}`);
    if (!htmlContenedorGrid) {
        Mensaje(TipoMensaje.Error, `No se ha localizado el contenedor ${idGrid}`);
        return;
    }

    htmlContenedorGrid.innerHTML = resultado.html;
    if (infoSelectores.Cantidad > 0) {
        var infSel = infoSelectores.Obtener(idGrid);
        if (infSel !== undefined && infSel.Cantidad > 0) {
            marcarElementos(idGrid, 'id', infSel);
            infSel.SincronizarCheck();
        }
    }
}



