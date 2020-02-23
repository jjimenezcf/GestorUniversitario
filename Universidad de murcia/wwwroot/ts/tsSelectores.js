var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var ListaDeSeleccionados = 'idsSeleccionados';
var HTMLSelector = /** @class */ (function (_super) {
    __extends(HTMLSelector, _super);
    function HTMLSelector() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    return HTMLSelector;
}(HTMLInputElement));
HTMLInputElement.prototype.InicializarSelector = function () {
    var htmlSelector = this;
    var idGridModal = htmlSelector.getAttribute('idGridModal');
    if (!idGridModal.isNullOrEmpty()) {
        var refCheckDeSeleccion = htmlSelector.getAttribute('refCheckDeSeleccion');
        if (!refCheckDeSeleccion.isNullOrEmpty()) {
            InicializarModal(idGridModal, refCheckDeSeleccion);
            htmlSelector.InicializarAtributos();
        }
        else
            console.log("El atributo refCheckDeSeleccion del selector " + htmlSelector.id + " no est\u00E1 bien definido ");
    }
    else
        console.log("El atributo idGridModal del selector " + htmlSelector.id + " no est\u00E1 bien definido ");
};
HTMLInputElement.prototype.InicializarAtributos = function () {
    var htmlSelector = this;
    htmlSelector.value = "";
    if (htmlSelector.hasAttribute(ListaDeSeleccionados)) {
        htmlSelector.setAttribute(ListaDeSeleccionados, '');
    }
};
HTMLInputElement.prototype.ClausulaDeFiltrado = function () {
    var htmlSelector = this;
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
};
HTMLInputElement.prototype.ClausulaDeBuscarValorEditado = function () {
    var propiedad = this.getAttribute('propiedadBuscar');
    var criterio = this.getAttribute('criterioBuscar');
    var valor = this.value;
    this.InicializarSelector();
    var clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);
    this.value = clausula.Valor;
    return clausula;
};
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
    console.log("se ha cerrado la modal " + idModal + ", hay que desmarcar los checks de la modal");
    InicializarModal(idGrid, referenciaChecks);
}
function AlSeleccionar(idSelector, idGrid, referenciaChecks) {
    var htmlSelector = document.getElementById(idSelector);
    htmlSelector.InicializarAtributos();
    var selector = infoSelectores.Obtener(idGrid);
    for (var x = 0; x < selector.Cantidad; x++) {
        var elemento = selector.LeerElemento(x);
        if (!elemento.EsVacio())
            mapearElementoAlHtmlSelector(htmlSelector, elemento);
        else
            console.log("Se ha leido mal el elemento del selector " + idGrid + " de la posici\u00F3n " + x);
    }
    InicializarModal(idGrid, referenciaChecks);
}
function recargarGrid(idGrid) {
    var htmlImputCantidad;
    htmlImputCantidad = document.getElementById(idGrid + "_nav_2_reg");
    if (htmlImputCantidad === null)
        console.log("El elemento " + idGrid + "_nav_2_reg  no est\u00E1 definido");
    else {
        var cantidad = htmlImputCantidad.value.Numero();
        var posicion = htmlImputCantidad.getAttribute("posicion").Numero();
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
    htmlSelector.value = valorDelSelector + elemento.Texto;
    mapearIdAlHtmlSelector(htmlSelector, elemento.Id);
}
function mapearIdAlHtmlSelector(htmlSelector, id) {
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
    var celdasId = document.getElementsByName(columnaId + "." + idGrid);
    var len = celdasId.length;
    for (var i = 0; i < infSel.Cantidad; i++) {
        for (var j = 0; j < len; j++) {
            var id = infSel.LeerId(i);
            if (celdasId[j].value === id) {
                var idCheck = celdasId[j].id.replace("." + columnaId, ".chksel");
                var check = document.getElementById(idCheck);
                check.checked = true;
                break;
            }
        }
    }
}
function blanquearCheck(refCheckDeSeleccion) {
    document.getElementsByName("" + refCheckDeSeleccion).forEach(function (c) {
        var check = c;
        check.checked = false;
    });
}
function AlCambiarTextoSelector(idSelector, controlador) {
    var htmlSelector = document.getElementById(idSelector);
    if (!htmlSelector.value.isNullOrEmpty()) {
        var clausulas = ObtenerClausulaParaBuscarRegistro(htmlSelector);
        LeerParaSelector("/" + controlador + "/Leer?filtro=" + JSON.stringify(clausulas), ProcesarRegistrosLeidos);
    }
    else {
        htmlSelector.InicializarSelector();
        var refCheckDeSeleccion = htmlSelector.getAttribute('refCheckDeSeleccion');
        if (!refCheckDeSeleccion.isNullOrEmpty()) {
            blanquearCheck(refCheckDeSeleccion);
        }
    }
}
function ObtenerClausulaParaBuscarRegistro(htmlSelector) {
    var clausula = htmlSelector.ClausulaDeBuscarValorEditado();
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
//# sourceMappingURL=tsSelectores.js.map