var ApiControl;
(function (ApiControl) {
    function OcultarMostrarExpansor(idHtmlExpansor, idHtmlBloque) {
        let extensor = document.getElementById(`${idHtmlExpansor}`);
        if (NumeroMayorDeCero(extensor.value)) {
            extensor.value = "0";
            ApiCrud.OcultarPanel(document.getElementById(`${idHtmlBloque}`));
        }
        else {
            extensor.value = "1";
            ApiCrud.MostrarPanel(document.getElementById(`${idHtmlBloque}`));
        }
        //EntornoSe.AjustarModalesAbiertas();
    }
    ApiControl.OcultarMostrarExpansor = OcultarMostrarExpansor;
    function BloquearMenu(panel) {
        let opciones = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.opcion}"]`);
        for (var i = 0; i < opciones.length; i++) {
            let opcion = opciones[i];
            let clase = opcion.getAttribute(atOpcionDeMenu.clase);
            if (clase === ClaseDeOpcioDeMenu.Basico)
                continue;
            opcion.disabled = true;
            opcion.setAttribute(atOpcionDeMenu.bloqueada, "S");
        }
    }
    ApiControl.BloquearMenu = BloquearMenu;
    function EstaBloqueada(opcion) { return opcion.getAttribute(atOpcionDeMenu.bloqueada) === "S"; }
    ApiControl.EstaBloqueada = EstaBloqueada;
    function BlanquearFecha(fecha) {
        fecha.value = "";
        let tipo = fecha.getAttribute(atControl.tipo);
        if (tipo === TipoControl.SelectorDeFechaHora) {
            let idHora = fecha.getAttribute(atSelectorDeFecha.hora);
            if (!IsNullOrEmpty(idHora)) {
                let controlHora = document.getElementById(idHora);
                controlHora.value = '';
                controlHora.setAttribute(atSelectorDeFecha.milisegundos, '0');
            }
        }
    }
    ApiControl.BlanquearFecha = BlanquearFecha;
    function AjustarColumnaDelGrid(columanDeOrdenacion) {
        let columna = document.getElementById(columanDeOrdenacion.IdColumna);
        columna.setAttribute(atControl.modoOrdenacion, columanDeOrdenacion.Modo);
        let a = columna.getElementsByTagName('a')[0];
        a.setAttribute("class", columanDeOrdenacion.ccsClase);
    }
    ApiControl.AjustarColumnaDelGrid = AjustarColumnaDelGrid;
    function BlanquearEditor(editor) {
        editor.classList.remove(ClaseCss.crtlNoValido);
        editor.classList.add(ClaseCss.crtlValido);
        editor.value = "";
    }
    ApiControl.BlanquearEditor = BlanquearEditor;
    function BlanquearSelector(selector) {
        selector.classList.remove(ClaseCss.crtlNoValido);
        selector.classList.add(ClaseCss.crtlValido);
        selector.selectedIndex = 0;
    }
    ApiControl.BlanquearSelector = BlanquearSelector;
})(ApiControl || (ApiControl = {}));
var ApiCrud;
(function (ApiCrud) {
    function MapearControlesDesdeLaIuAlJson(crud, panel, modoDeTrabajo) {
        let elementoJson = crud.AntesDeMapearDatosDeIU(crud, panel, modoDeTrabajo);
        MapearAlJson.ListasDeElementos(panel, elementoJson);
        MapearAlJson.ListaDinamicas(panel, elementoJson);
        MapearAlJson.Restrictores(panel, elementoJson);
        MapearAlJson.Editores(panel, elementoJson);
        MapearAlJson.Textos(panel, elementoJson);
        MapearAlJson.Archivos(panel, elementoJson);
        MapearAlJson.Urls(panel, elementoJson);
        MapearAlJson.Checks(panel, elementoJson);
        MapearAlJson.Fechas(panel, elementoJson);
        return crud.DespuesDeMapearDatosDeIU(crud, panel, elementoJson, modoDeTrabajo);
    }
    ApiCrud.MapearControlesDesdeLaIuAlJson = MapearControlesDesdeLaIuAlJson;
    function BlanquearControlesDeIU(panel) {
        BlanquearEditores(panel);
        BlanquearSelectores(panel);
        BlanquearArchivos(panel);
    }
    ApiCrud.BlanquearControlesDeIU = BlanquearControlesDeIU;
    function MostrarPanel(panel) {
        panel.classList.remove(ClaseCss.divNoVisible);
    }
    ApiCrud.MostrarPanel = MostrarPanel;
    function OcultarPanel(panel) {
        panel.classList.add(ClaseCss.divNoVisible);
        panel.classList.remove(ClaseCss.divVisible);
    }
    ApiCrud.OcultarPanel = OcultarPanel;
    function CerrarModal(modal) {
        modal.style.display = "none";
        //modal.setAttribute('altura-inicial', "0");
        //modal.setAttribute('ratio-inicial', "0");
        //var body = document.getElementsByTagName("body")[0];
        //body.style.position = "inherit";
        //body.style.height = "auto";
        //body.style.overflow = "visible";
    }
    ApiCrud.CerrarModal = CerrarModal;
    function QuitarClaseDeCtrlNoValido(panel) {
        let crtls = panel.getElementsByClassName(ClaseCss.crtlNoValido);
        for (let i = 0; i < crtls.length; i++) {
            crtls[i].classList.remove(ClaseCss.crtlNoValido);
        }
    }
    ApiCrud.QuitarClaseDeCtrlNoValido = QuitarClaseDeCtrlNoValido;
    function ActivarOpciones(opciones, activas, seleccionadas) {
        for (var i = 0; i < opciones.length; i++) {
            let opcion = opciones[i];
            if (ApiControl.EstaBloqueada(opcion))
                continue;
            let literal = opcion.value.toLowerCase();
            if (activas.indexOf(literal) >= 0) {
                let permiteMultiSeleccion = opcion.getAttribute(atOpcionDeMenu.permiteMultiSeleccion);
                if (!EsTrue(permiteMultiSeleccion))
                    opcion.disabled = !(seleccionadas === 1);
                else {
                    if (seleccionadas === 1)
                        opcion.disabled = false;
                }
            }
        }
    }
    ApiCrud.ActivarOpciones = ActivarOpciones;
    function DesactivarOpciones(opciones, desactivas) {
        for (var i = 0; i < opciones.length; i++) {
            let opcion = opciones[i];
            if (ApiControl.EstaBloqueada(opcion))
                continue;
            let literal = opcion.value.toLowerCase();
            if (desactivas.indexOf(literal) >= 0)
                opcion.disabled = true;
        }
    }
    ApiCrud.DesactivarOpciones = DesactivarOpciones;
    function DesactivarConMultiSeleccion(opciones, seleccionadas) {
        for (var i = 0; i < opciones.length; i++) {
            let opcion = opciones[i];
            if (ApiControl.EstaBloqueada(opcion))
                continue;
            let permiteMultiSeleccion = opcion.getAttribute(atOpcionDeMenu.permiteMultiSeleccion);
            if (!EsTrue(permiteMultiSeleccion) && !opcion.disabled)
                opcion.disabled = !(seleccionadas === 1);
        }
    }
    ApiCrud.DesactivarConMultiSeleccion = DesactivarConMultiSeleccion;
    function CambiarLiteralOpcion(opciones, antiguo, nuevo) {
        for (var i = 0; i < opciones.length; i++) {
            let opcion = opciones[i];
            if (ApiControl.EstaBloqueada(opcion))
                continue;
            let literal = opcion.value.toLowerCase();
            if (literal.toLowerCase() === antiguo)
                opcion.value = nuevo;
        }
    }
    ApiCrud.CambiarLiteralOpcion = CambiarLiteralOpcion;
    function BlanquearEditores(panel) {
        let editores = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.Editor}"]`);
        for (let i = 0; i < editores.length; i++) {
            ApiControl.BlanquearEditor(editores[i]);
        }
    }
    function BlanquearSelectores(panel) {
        let selectores = panel.querySelectorAll(`select[${atControl.tipo}="${TipoControl.ListaDeElementos}"]`);
        for (let i = 0; i < selectores.length; i++) {
            ApiControl.BlanquearSelector(selectores[i]);
        }
    }
    function BlanquearArchivos(panel) {
        let archivos = panel.querySelectorAll(`${atControl.tipo}[tipo="${TipoControl.Archivo}"]`);
        for (let i = 0; i < archivos.length; i++) {
            ApiDeArchivos.BlanquearArchivo(archivos[i], true);
        }
    }
})(ApiCrud || (ApiCrud = {}));
var ApiRuote;
(function (ApiRuote) {
    function NavegarARelacionar(crud, idOpcionDeMenu, idSeleccionado, filtroRestrictor) {
        let filtroJson = ApiFiltro.DefinirRestrictorNumerico(filtroRestrictor.Propiedad, filtroRestrictor.Valor);
        let form = document.getElementById(idOpcionDeMenu);
        if (form === null) {
            throw new Error(`La opción de menú '${idOpcionDeMenu}' está mal definida, actualice el descriptor`);
        }
        let navegarAlCrud = form.getAttribute(atNavegar.navegarAlCrud);
        let idRestrictor = form.getAttribute(atNavegar.idRestrictor);
        let idOrden = form.getAttribute(atNavegar.orden);
        let restrictor = document.getElementById(idRestrictor);
        restrictor.value = filtroJson;
        let ordenInput = document.getElementById(idOrden);
        ordenInput.value = "";
        let valores = new Diccionario();
        valores.Agregar(Sesion.paginaDestino, navegarAlCrud);
        valores.Agregar(Sesion.restrictor, filtroRestrictor);
        valores.Agregar(Sesion.idSeleccionado, idSeleccionado);
        Navegar(crud, form, valores);
    }
    ApiRuote.NavegarARelacionar = NavegarARelacionar;
    function NavegarADependientes(crud, idOpcionDeMenu, idSeleccionado, filtroRestrictor) {
        let filtroJson = ApiFiltro.DefinirRestrictorNumerico(filtroRestrictor.Propiedad, filtroRestrictor.Valor);
        let form = document.getElementById(idOpcionDeMenu);
        if (form === null) {
            throw new Error(`La opción de menú '${idOpcionDeMenu}' está mal definida, actualice el descriptor`);
        }
        let navegarAlCrud = form.getAttribute(atNavegar.navegarAlCrud);
        let idRestrictor = form.getAttribute(atNavegar.idRestrictor);
        let idOrden = form.getAttribute(atNavegar.orden);
        let restrictor = document.getElementById(idRestrictor);
        restrictor.value = filtroJson;
        let ordenInput = document.getElementById(idOrden);
        ordenInput.value = "";
        let valores = new Diccionario();
        valores.Agregar(Sesion.paginaDestino, navegarAlCrud);
        valores.Agregar(Sesion.restrictor, filtroRestrictor);
        valores.Agregar(Sesion.idSeleccionado, idSeleccionado);
        Navegar(crud, form, valores);
    }
    ApiRuote.NavegarADependientes = NavegarADependientes;
    function Navegar(crud, form, valores) {
        crud.AntesDeNavegar(valores);
        EntornoSe.Historial.GuardarEstadoDePagina(crud.Estado);
        EntornoSe.Sumit(form);
    }
})(ApiRuote || (ApiRuote = {}));
;
var ApiFiltro;
(function (ApiFiltro) {
    function DefinirFiltroPorId(id) {
        return ApiFiltro.DefinirRestrictorNumerico(literal.filtro.clausulaId, id);
    }
    ApiFiltro.DefinirFiltroPorId = DefinirFiltroPorId;
    function DefinirRestrictorNumerico(propiedad, valor) {
        var clausulas = new Array();
        var clausula = new ClausulaDeFiltrado(propiedad, literal.filtro.criterio.igual, `${valor}`);
        clausulas.push(clausula);
        return JSON.stringify(clausulas);
    }
    ApiFiltro.DefinirRestrictorNumerico = DefinirRestrictorNumerico;
    function DefinirFiltroListaDinamica(input, criterio) {
        let buscarPor = input.getAttribute(atListasDinamicas.buscarPor);
        let longitud = Numero(input.getAttribute(atListasDinamicas.longitudNecesaria));
        let valor = input.value;
        if (longitud == 0)
            longitud = 3;
        if (valor.length < longitud)
            return null;
        let clausula = new ClausulaDeFiltrado(buscarPor, criterio, valor.toString());
        return clausula;
    }
    ApiFiltro.DefinirFiltroListaDinamica = DefinirFiltroListaDinamica;
})(ApiFiltro || (ApiFiltro = {}));
//# sourceMappingURL=ApiCrud.js.map