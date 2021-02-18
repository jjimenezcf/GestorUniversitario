var ApiControl;
(function (ApiControl) {
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
    function MapearListasDinamicasAlJson(panel, elementoJson) {
        let ListaDinamica = panel.querySelectorAll(`input[tipo="${TipoControl.ListaDinamica}"]`);
        for (let i = 0; i < ListaDinamica.length; i++) {
            MapearListaDinamicaAlJson(ListaDinamica[i], elementoJson);
        }
    }
    ApiControl.MapearListasDinamicasAlJson = MapearListasDinamicasAlJson;
    function AlmacenarValorDeListaDinamica(input, valor) {
        input.setAttribute(atListasDinamicas.idSeleccionado, Numero(valor).toString());
        if (Numero(valor) === 0)
            input.value = "";
    }
    ApiControl.AlmacenarValorDeListaDinamica = AlmacenarValorDeListaDinamica;
    function MapearListaDinamicaAlJson(input, elementoJson) {
        let propiedadDto = input.getAttribute(atControl.propiedad);
        let guardarEn = input.getAttribute(atListasDinamicasDto.guardarEn);
        let obligatorio = input.getAttribute(atControl.obligatorio);
        let valor = Numero(input.getAttribute(atListasDinamicas.idSeleccionado));
        if (obligatorio === "S" && Number(valor) === 0) {
            input.classList.remove(ClaseCss.crtlValido);
            input.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`Debe seleccionar un elemento de la lista ${propiedadDto}`);
        }
        input.classList.remove(ClaseCss.crtlNoValido);
        input.classList.add(ClaseCss.crtlValido);
        elementoJson[guardarEn] = valor === 0 ? '' : valor.toString();
    }
    function MapearListasDeElementosAlJson(panel, elementoJson) {
        let selectores = panel.querySelectorAll(`select[tipo="${TipoControl.ListaDeElementos}"]`);
        for (let i = 0; i < selectores.length; i++) {
            MapearSelectorDeElementosAlJson(selectores[i], elementoJson);
        }
    }
    ApiControl.MapearListasDeElementosAlJson = MapearListasDeElementosAlJson;
    function MapearSelectorDeElementosAlJson(selector, elementoJson) {
        let propiedadDto = selector.getAttribute(atControl.propiedad);
        let guardarEn = selector.getAttribute(atListasDinamicasDto.guardarEn);
        let obligatorio = selector.getAttribute(atControl.obligatorio);
        if (obligatorio === "S" && Number(selector.value) === 0) {
            selector.classList.remove(ClaseCss.crtlValido);
            selector.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`Debe seleccionar un elemento de la lista ${propiedadDto}`);
        }
        selector.classList.remove(ClaseCss.crtlNoValido);
        selector.classList.add(ClaseCss.crtlValido);
        elementoJson[guardarEn] = selector.value;
    }
    function MapearRestrictoresAlJson(panel, elementoJson) {
        let restrictores = panel.querySelectorAll(`input[tipo="${TipoControl.restrictorDeEdicion}"]`);
        for (let i = 0; i < restrictores.length; i++) {
            MapearRestrictorAlJson(restrictores[i], elementoJson);
        }
    }
    ApiControl.MapearRestrictoresAlJson = MapearRestrictoresAlJson;
    function MapearRestrictorAlJson(input, elementoJson) {
        let propiedadDto = input.getAttribute(atControl.propiedad);
        let idRestrictor = input.getAttribute(atControl.restrictor);
        if (!NumeroMayorDeCero(idRestrictor)) {
            input.classList.remove(ClaseCss.crtlValido);
            input.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`El campo ${propiedadDto} es obligatorio`);
        }
        input.classList.remove(ClaseCss.crtlNoValido);
        input.classList.add(ClaseCss.crtlValido);
        elementoJson[propiedadDto] = idRestrictor;
    }
    function MapearEditoresAlJson(panel, elementoJson) {
        let editores = panel.querySelectorAll(`input[tipo="${TipoControl.Editor}"]`);
        for (let i = 0; i < editores.length; i++) {
            MapearEditorAlJson(editores[i], elementoJson);
        }
    }
    ApiControl.MapearEditoresAlJson = MapearEditoresAlJson;
    function MapearEditorAlJson(input, elementoJson) {
        var propiedadDto = input.getAttribute(atControl.propiedad);
        let valor = input.value;
        let obligatorio = input.getAttribute(atControl.obligatorio);
        if (obligatorio === "S" && NoDefinida(valor)) {
            input.classList.remove(ClaseCss.crtlValido);
            input.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`El campo ${propiedadDto} es obligatorio`);
        }
        input.classList.remove(ClaseCss.crtlNoValido);
        input.classList.add(ClaseCss.crtlValido);
        elementoJson[propiedadDto] = valor;
    }
    function MapearArchivosAlJson(panel, elementoJson) {
        let archivos = panel.querySelectorAll(`input[tipo="${TipoControl.Archivo}"]`);
        for (let i = 0; i < archivos.length; i++) {
            MapearArchivoAlJson(archivos[i], elementoJson);
        }
    }
    ApiControl.MapearArchivosAlJson = MapearArchivosAlJson;
    function MapearArchivoAlJson(archivo, elementoJson) {
        var propiedadDto = archivo.getAttribute(atControl.propiedad);
        let valor = archivo.getAttribute(atArchivo.idArchivo);
        let obligatorio = archivo.getAttribute(atControl.obligatorio);
        if (obligatorio === "S" && IsNullOrEmpty(valor)) {
            archivo.classList.remove(ClaseCss.crtlValido);
            archivo.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`El campo ${propiedadDto} es obligatorio`);
        }
        archivo.classList.remove(ClaseCss.crtlNoValido);
        archivo.classList.add(ClaseCss.crtlValido);
        elementoJson[propiedadDto] = valor;
    }
    function MapearUrlArchivosAlJson(panel, elementoJson) {
        let urlsDeArchivos = panel.querySelectorAll(`input[tipo="${TipoControl.UrlDeArchivo}"]`);
        for (let i = 0; i < urlsDeArchivos.length; i++) {
            MapearUrlArchivoAlJson(urlsDeArchivos[i], elementoJson);
        }
    }
    ApiControl.MapearUrlArchivosAlJson = MapearUrlArchivosAlJson;
    function MapearUrlArchivoAlJson(urlDeArchivo, elementoJson) {
        var propiedadDto = urlDeArchivo.getAttribute(atControl.propiedad);
        let valor = urlDeArchivo.getAttribute(atArchivo.nombre);
        let obligatorio = urlDeArchivo.getAttribute(atControl.obligatorio);
        if (obligatorio === "S" && IsNullOrEmpty(valor)) {
            urlDeArchivo.classList.remove(ClaseCss.crtlValido);
            urlDeArchivo.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`El campo ${propiedadDto} es obligatorio`);
        }
        urlDeArchivo.classList.remove(ClaseCss.crtlNoValido);
        urlDeArchivo.classList.add(ClaseCss.crtlValido);
        elementoJson[propiedadDto] = valor;
    }
    function MapearCheckesAlJson(panel, elementoJson) {
        let checkes = panel.querySelectorAll(`input[tipo="${TipoControl.Check}"]`);
        for (let i = 0; i < checkes.length; i++) {
            MapearCheckAlJson(checkes[i], elementoJson);
        }
    }
    ApiControl.MapearCheckesAlJson = MapearCheckesAlJson;
    function MapearCheckAlJson(check, elementoJson) {
        var propiedadDto = check.getAttribute(atControl.propiedad);
        elementoJson[propiedadDto] = check.checked;
    }
})(ApiControl || (ApiControl = {}));
var ApiCrud;
(function (ApiCrud) {
    function MapearControlesDesdeLaIuAlJson(crud, panel, modoDeTrabajo) {
        let elementoJson = crud.AntesDeMapearDatosDeIU(crud, panel, modoDeTrabajo);
        ApiControl.MapearListasDeElementosAlJson(panel, elementoJson);
        ApiControl.MapearListasDinamicasAlJson(panel, elementoJson);
        ApiControl.MapearRestrictoresAlJson(panel, elementoJson);
        ApiControl.MapearEditoresAlJson(panel, elementoJson);
        ApiControl.MapearArchivosAlJson(panel, elementoJson);
        ApiControl.MapearUrlArchivosAlJson(panel, elementoJson);
        ApiControl.MapearCheckesAlJson(panel, elementoJson);
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
        var body = document.getElementsByTagName("body")[0];
        body.style.position = "inherit";
        body.style.height = "auto";
        body.style.overflow = "visible";
    }
    ApiCrud.CerrarModal = CerrarModal;
    function QuitarClaseDeCtrlNoValido(panel) {
        let crtls = panel.getElementsByClassName(ClaseCss.crtlNoValido);
        for (let i = 0; i < crtls.length; i++) {
            crtls[i].classList.remove(ClaseCss.crtlNoValido);
        }
    }
    ApiCrud.QuitarClaseDeCtrlNoValido = QuitarClaseDeCtrlNoValido;
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
        let navegarAlCrud = form.getAttribute(atRelacion.navegarAlCrud);
        let idRestrictor = form.getAttribute(atRelacion.idRestrictor);
        let idOrden = form.getAttribute(atRelacion.orden);
        let restrictor = document.getElementById(idRestrictor);
        restrictor.value = filtroJson;
        let ordenInput = document.getElementById(idOrden);
        ordenInput.value = "";
        let valores = new Diccionario();
        valores.Agregar(Sesion.paginaDestino, navegarAlCrud);
        valores.Agregar(Sesion.restrictor, filtroRestrictor);
        valores.Agregar(Sesion.idSeleccionado, idSeleccionado);
        Navegar(crud, form, crud.Estado, valores);
    }
    ApiRuote.NavegarARelacionar = NavegarARelacionar;
    function Navegar(crud, form, estado, valores) {
        crud.AntesDeNavegar(valores);
        EntornoSe.Historial.GuardarEstadoDePagina(estado);
        EntornoSe.Historial.Persistir();
        PonerCapa();
        form.submit();
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