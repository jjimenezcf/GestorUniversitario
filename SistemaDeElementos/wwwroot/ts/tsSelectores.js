const ListaDeSeleccionados = 'idsSeleccionados';
class HTMLSelector extends HTMLInputElement {
}
HTMLInputElement.prototype.InicializarSelector = function () {
    var htmlSelector = this;
    var idGridModal = htmlSelector.getAttribute(AtributoSelector.idGridModal);
    if (!idGridModal.NoDefinida()) {
        var refCheckDeSeleccion = htmlSelector.getAttribute(AtributoSelector.refCheckDeSeleccion);
        if (!refCheckDeSeleccion.NoDefinida()) {
            InicializarModal(idGridModal, refCheckDeSeleccion);
            htmlSelector.BanquearEditorDelGrid();
            htmlSelector.InicializarAtributos();
        }
        else
            console.log(`El atributo refCheckDeSeleccion del selector ${htmlSelector.id} no está bien definido `);
    }
    else
        console.log(`El atributo idGridModal del selector ${htmlSelector.id} no está bien definido `);
};
HTMLInputElement.prototype.MapearTextoAlEditorDelGrid = function () {
    var htmlSelector = this;
    var htmlEditor = htmlSelector.EditorDelGrid();
    if (!htmlSelector.value.NoDefinida()) {
        var listaDeIds = htmlSelector.getAttribute(ListaDeSeleccionados);
        if (listaDeIds === null || listaDeIds.NoDefinida())
            htmlEditor.value = htmlSelector.value;
        else
            htmlEditor.value = '';
    }
    else {
        htmlEditor.value = '';
    }
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
    var propiedad = htmlSelector.getAttribute(Atributo.propiedad);
    var criterio = htmlSelector.getAttribute(Atributo.criterio);
    var valor = null;
    var clausula = null;
    if (htmlSelector.hasAttribute(ListaDeSeleccionados)) {
        var ids = htmlSelector.getAttribute(ListaDeSeleccionados);
        if (!ids.NoDefinida()) {
            valor = ids;
            clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);
        }
    }
    return clausula;
};
HTMLInputElement.prototype.ClausulaDeBuscarValorEditado = function () {
    var propiedad = this.getAttribute(AtributoSelector.popiedadBuscar);
    var criterio = this.getAttribute(AtributoSelector.criterioBuscar);
    var valor = this.value.trim();
    this.InicializarSelector();
    var clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);
    this.value = clausula.Valor;
    return clausula;
};
HTMLInputElement.prototype.BanquearEditorDelGrid = function () {
    var htmlSelector = this;
    var htmlEditor = htmlSelector.EditorDelGrid();
    htmlEditor.value = '';
};
HTMLInputElement.prototype.EditorDelGrid = function () {
    var htmlSelector = this;
    var idEditorMostrar = htmlSelector.getAttribute(AtributoSelector.idEditorMostrar);
    var htmlEditor = document.getElementById(idEditorMostrar);
    return htmlEditor;
};
/***************************************************************************************************************
Eventos en el selector y en la ventana modal
 ***************************************************************************************************************/
function AlAbrir(idGrid, idSelector, columnaId, columnaMostrar) {
    BlanquearMensaje();
    var htmlSelector = document.getElementById(idSelector);
    htmlSelector.MapearTextoAlEditorDelGrid();
    recargarGrid(idGrid);
    var infSel = infoSelectores.Crear(idGrid);
    infSel.Modal(columnaMostrar);
    var arrayMarcados = elementosMarcados(idSelector);
    infSel.InsertarElementos(arrayMarcados);
    marcarElementos(idGrid, columnaId, infSel);
    infSel.SincronizarCheck();
}
function AlCerrar(idModal, idGrid, referenciaChecks) {
    console.log(`se ha cerrado la modal ${idModal}, hay que desmarcar los checks de la modal`);
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
            console.log(`Se ha leido mal el elemento del selector ${idGrid} de la posición ${x}`);
    }
    InicializarModal(idGrid, referenciaChecks);
}
function AlCambiarTextoSelector(idSelector, controlador) {
    var htmlSelector = document.getElementById(idSelector);
    if (!htmlSelector.value.NoDefinida()) {
        var clausulas = ObtenerClausulaParaBuscarRegistro(htmlSelector);
        LeerParaSelector(`/${controlador}/Leer?filtro=${JSON.stringify(clausulas)}`, htmlSelector, ProcesarRegistrosLeidos);
    }
    else {
        htmlSelector.InicializarSelector();
        var refCheckDeSeleccion = htmlSelector.getAttribute(AtributoSelector.refCheckDeSeleccion);
        if (!refCheckDeSeleccion.NoDefinida()) {
            blanquearCheck(refCheckDeSeleccion);
        }
    }
}
function recargarGrid(idGrid) {
    BlanquearMensaje();
    var htmlImputCantidad = document.getElementById(`${idGrid}_${LiteralMnt.idCtrlCantidad}`);
    if (htmlImputCantidad === null)
        console.log(`El elemento ${idGrid}_${LiteralMnt.idCtrlCantidad} no está definido`);
    else {
        var cantidad = htmlImputCantidad.value.Numero();
        var posicion = htmlImputCantidad.getAttribute(LiteralMnt.posicion).Numero();
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
        if (!ids.NoDefinida()) {
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
    if (!valorDelSelector.NoDefinida())
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
    if (!listaDeIds.NoDefinida())
        listaDeIds = listaDeIds + ';';
    listaDeIds = listaDeIds + id;
    htmlSelector.setAttribute(ListaDeSeleccionados, listaDeIds);
}
function InicializarModal(idGrid, referenciaChecks) {
    BlanquearMensaje();
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
                var idCheck = celdasId[j].id.replace(`.${columnaId}`, LiteralMnt.postfijoDeCheckDeSeleccion);
                var check = document.getElementById(idCheck);
                check.checked = true;
                break;
            }
        }
    }
}
function blanquearCheck(refCheckDeSeleccion) {
    document.getElementsByName(`${refCheckDeSeleccion}`).forEach(c => {
        let check = c;
        check.checked = false;
    });
}
function ObtenerClausulaParaBuscarRegistro(htmlSelector) {
    var clausula = htmlSelector.ClausulaDeBuscarValorEditado();
    var clausulas = new Array();
    clausulas.push(clausula);
    return clausulas;
}
function LeerParaSelector(url, htmlSelector, funcionDeRespuesta) {
    function respuestaCorrecta() {
        if (req.status >= 200 && req.status < 400) {
            funcionDeRespuesta(htmlSelector, req.responseText);
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
function ProcesarRegistrosLeidos(htmlSelector, registros) {
    var propiedadmostrar = htmlSelector.getAttribute(AtributoSelector.propiedadmostrar);
    if (!propiedadmostrar.NoDefinida()) {
        var registrosJson = JSON.parse(registros);
        if (registrosJson.length === 1) {
            var registroJson = registrosJson[0];
            for (let key in registroJson) {
                if (key === propiedadmostrar) {
                    htmlSelector.value = '';
                    mapearElementoAlHtmlSelector(htmlSelector, new Elemento(registroJson['id'], registroJson[key]));
                    return;
                }
            }
        }
        else {
            var idBtnSelector = htmlSelector.getAttribute(AtributoSelector.idBtnSelector);
            var btnSelector = document.getElementById(idBtnSelector);
            btnSelector.click();
        }
    }
    else
        console.log(`No se ha definido la propiedad propiedadMostrar en el selector ${htmlSelector.id}`);
}
//# sourceMappingURL=tsSelectores.js.map