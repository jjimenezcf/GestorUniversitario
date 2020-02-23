
const ListaDeSeleccionados: string = 'idsSeleccionados';

class HTMLSelector extends HTMLInputElement {
}

interface HTMLInputElement {
    InicializarSelector(): void;
    InicializarAtributos(): void;
    ClausulaDeFiltrado(): ClausulaDeFiltrado;
    ClausulaDeBuscarValorEditado(): ClausulaDeFiltrado;
}

HTMLInputElement.prototype.InicializarSelector = function (): void {
    var htmlSelector: HTMLInputElement = this;
    var idGridModal: string = htmlSelector.getAttribute('idGridModal');
    if (!idGridModal.isNullOrEmpty()) {

        var refCheckDeSeleccion: string = htmlSelector.getAttribute('refCheckDeSeleccion');
        if (!refCheckDeSeleccion.isNullOrEmpty()) {
            InicializarModal(idGridModal, refCheckDeSeleccion);
            htmlSelector.InicializarAtributos();
        }
        else
            console.log(`El atributo refCheckDeSeleccion del selector ${htmlSelector.id} no está bien definido `);
    }
    else
        console.log(`El atributo idGridModal del selector ${htmlSelector.id} no está bien definido `);
}

HTMLInputElement.prototype.InicializarAtributos = function (): void {
    var htmlSelector: HTMLInputElement = this;
    htmlSelector.value = "";
    if (htmlSelector.hasAttribute(ListaDeSeleccionados)) {
        htmlSelector.setAttribute(ListaDeSeleccionados, '');
    }
}

HTMLInputElement.prototype.ClausulaDeFiltrado = function (): ClausulaDeFiltrado {
    var htmlSelector: HTMLInputElement = this;
    var propiedad = htmlSelector.getAttribute('propiedad');
    var criterio = htmlSelector.getAttribute('criterio');
    var valor = null;
    var clausula = null;
    if (htmlSelector.hasAttribute(ListaDeSeleccionados)) {
        var ids = htmlSelector.getAttribute(ListaDeSeleccionados);
        if (!ids.isNullOrEmpty()) {
            valor = ids;
            clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);
        }
    }
    return clausula;
} 

HTMLInputElement.prototype.ClausulaDeBuscarValorEditado = function (): ClausulaDeFiltrado {
    var propiedad = this.getAttribute('propiedadBuscar');
    var criterio = this.getAttribute('criterioBuscar');
    var valor = this.value;

    this.InicializarSelector();
    var clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);
    this.value = clausula.Valor;
    return clausula;
} 



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
    InicializarModal(idGrid, referenciaChecks);
}

function AlSeleccionar(idSelector, idGrid, referenciaChecks) {

    var htmlSelector: HTMLSelector = <HTMLSelector>document.getElementById(idSelector);

    htmlSelector.InicializarAtributos();
    var selector: InfoSelector = infoSelectores.Obtener(idGrid);

    for (var x = 0; x < selector.Cantidad; x++) {
        var elemento: Elemento = selector.LeerElemento(x);
        if (!elemento.EsVacio())
            mapearElementoAlHtmlSelector(htmlSelector, elemento);
        else
            console.log(`Se ha leido mal el elemento del selector ${idGrid} de la posición ${x}`);

    }
    InicializarModal(idGrid, referenciaChecks);
}

function recargarGrid(idGrid) {
    var htmlImputCantidad: HTMLInputElement;
    htmlImputCantidad = <HTMLInputElement>document.getElementById(`${idGrid}_nav_2_reg`);
    if (htmlImputCantidad === null)
        console.log(`El elemento ${idGrid}_nav_2_reg  no está definido`);
    else {
        var cantidad: number = htmlImputCantidad.value.Numero();
        var posicion: number = htmlImputCantidad.getAttribute("posicion").Numero();
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
    if (htmlSelector.hasAttribute(ListaDeSeleccionados)) {
        ids = htmlSelector.getAttribute(ListaDeSeleccionados);
        if (!ids.isNullOrEmpty()) {
            var listaNombres = (<HTMLSelector>htmlSelector).value.split('|');
            var listaIds = ids.split(';');
            for (var i = 0; i < listaIds.length; i++) {
                var e = { id: listaIds[i], valor: listaNombres[i] };
                elementos.push(e);
            }
        }
    }

    return elementos;
}


function mapearElementoAlHtmlSelector(htmlSelector: HTMLSelector, elemento: Elemento) {

    var valorDelSelector = htmlSelector.value;
    if (!valorDelSelector.isNullOrEmpty())
        valorDelSelector = valorDelSelector + " | ";

    htmlSelector.value = valorDelSelector + elemento.Texto;
    mapearIdAlHtmlSelector(htmlSelector, elemento.Id);

}

function mapearIdAlHtmlSelector(htmlSelector: HTMLSelector, id: number) {
    var listaDeIds = htmlSelector.getAttribute(ListaDeSeleccionados);
    if (listaDeIds === null) {
        var atributo = document.createAttribute(ListaDeSeleccionados);
        htmlSelector.setAttributeNode(atributo);
        listaDeIds = "";
    }

    if (!listaDeIds.isNullOrEmpty())
        listaDeIds = listaDeIds + ';';
    listaDeIds = listaDeIds + id;
    htmlSelector.setAttribute(ListaDeSeleccionados, listaDeIds);
}


function InicializarModal(idGrid, referenciaChecks) {
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
            if ((<HTMLInputElement>celdasId[j]).value === id) {
                var idCheck = celdasId[j].id.replace(`.${columnaId}`, ".chksel");
                var check = document.getElementById(idCheck);
                (<HTMLInputElement>check).checked = true;
                break;
            }
        }
    }
}

function blanquearCheck(refCheckDeSeleccion: string) {
    document.getElementsByName(`${refCheckDeSeleccion}`).forEach(c => {
        let check = <HTMLInputElement>c;
        check.checked = false;
    }
    );
}


function AlCambiarTextoSelector(idSelector: string, controlador: string) {
    var htmlSelector: HTMLSelector = <HTMLSelector>document.getElementById(idSelector);
    if (!htmlSelector.value.isNullOrEmpty()) {
        var clausulas = ObtenerClausulaParaBuscarRegistro(htmlSelector);
        LeerParaSelector(`/${controlador}/Leer?filtro=${JSON.stringify(clausulas)}`, ProcesarRegistrosLeidos);
    }
    else {
        htmlSelector.InicializarSelector();
        var refCheckDeSeleccion: string = htmlSelector.getAttribute('refCheckDeSeleccion');
        if (!refCheckDeSeleccion.isNullOrEmpty()) {
            blanquearCheck(refCheckDeSeleccion);
        }
    }

}

function ObtenerClausulaParaBuscarRegistro(htmlSelector: HTMLSelector) {

    var clausula: ClausulaDeFiltrado = htmlSelector.ClausulaDeBuscarValorEditado();
    var clausulas = new Array <ClausulaDeFiltrado>();
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

