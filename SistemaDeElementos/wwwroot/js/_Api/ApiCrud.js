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
    function MapearFechaAlControl(control, fecha) {
        var fechaLeida = new Date(fecha);
        if (FechaValida(fechaLeida)) {
            let dia = fechaLeida.getDate();
            let mes = fechaLeida.getMonth() + 1;
            let ano = fechaLeida.getFullYear();
            control.value = `${ano}-${PadLeft(mes.toString(), "00")}-${PadLeft(dia.toString(), "00")}`;
        }
        else {
            var propiedad = control.getAttribute(atControl.propiedad);
            MensajesSe.Error("MapearFechaAlControl", `Fecha leida para la propiedad ${propiedad} es no válida, valor ${fecha}`);
        }
    }
    ApiControl.MapearFechaAlControl = MapearFechaAlControl;
    function MapearTextoAlControl(area, texto) {
        area.textContent = texto;
    }
    ApiControl.MapearTextoAlControl = MapearTextoAlControl;
    function MapearPropiedadRestrictoraAlFiltro(panel, propiedadRestrictora, id, texto) {
        let restrictores = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.restrictorDeFiltro}"]`);
        for (let i = 0; i < restrictores.length; i++) {
            if (restrictores[i].getAttribute(atControl.propiedad) === propiedadRestrictora) {
                ApiControl.MapearRestrictorAlControl(restrictores[i], id, texto);
            }
        }
    }
    ApiControl.MapearPropiedadRestrictoraAlFiltro = MapearPropiedadRestrictoraAlFiltro;
    function MapearPropiedadRestrictoraAlControl(panel, propiedadRestrictora, id, texto) {
        let restrictores = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.restrictorDeEdicion}"]`);
        for (let i = 0; i < restrictores.length; i++) {
            if (restrictores[i].getAttribute(atControl.propiedad) === propiedadRestrictora) {
                ApiControl.MapearRestrictorAlControl(restrictores[i], id, texto);
            }
        }
    }
    ApiControl.MapearPropiedadRestrictoraAlControl = MapearPropiedadRestrictoraAlControl;
    function MapearRestrictorAlControl(restrictor, id, texto) {
        restrictor.setAttribute(atControl.valorInput, texto);
        restrictor.setAttribute(atControl.restrictor, id.toString());
    }
    ApiControl.MapearRestrictorAlControl = MapearRestrictorAlControl;
    function MapearHoraAlControl(control, fechaHora) {
        var fechaLeida = new Date(fechaHora);
        if (FechaValida(fechaLeida)) {
            let hora = fechaLeida.getHours();
            let minuto = fechaLeida.getMinutes();
            let segundos = fechaLeida.getSeconds();
            let milisegundos = fechaLeida.getMilliseconds();
            let idHora = control.getAttribute(atSelectorDeFecha.hora);
            if (!IsNullOrEmpty(idHora)) {
                let controlHora = document.getElementById(idHora);
                controlHora.value = `${PadLeft(hora.toString(), "00")}:${PadLeft(minuto.toString(), "00")}:${PadLeft(segundos.toString(), "00")}`;
                controlHora.setAttribute(atSelectorDeFecha.milisegundos, milisegundos.toString());
                return;
            }
        }
        var propiedad = control.getAttribute(atControl.propiedad);
        MensajesSe.Error("MapearHoraAlControl", `Fecha leida para la propiedad ${propiedad} es no válida, valor ${fechaHora}`);
    }
    ApiControl.MapearHoraAlControl = MapearHoraAlControl;
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
    function MapearFechasAlJson(panel, elementoJson) {
        let fechas = panel.querySelectorAll(`input[tipo="${TipoControl.SelectorDeFecha}"]`);
        for (var i = 0; i < fechas.length; i++) {
            let fecha = fechas[i];
            MapearFechaAlJson(fecha, elementoJson);
        }
        let fechasHoras = panel.querySelectorAll(`input[tipo="${TipoControl.SelectorDeFechaHora}"]`);
        for (var i = 0; i < fechasHoras.length; i++) {
            let fecha = fechasHoras[i];
            MapearFechaAlJson(fecha, elementoJson);
        }
    }
    ApiControl.MapearFechasAlJson = MapearFechasAlJson;
    function MapearFechaAlJson(controlDeFecha, elementoJson) {
        let propiedadDto = controlDeFecha.getAttribute(atControl.propiedad);
        let obligatorio = controlDeFecha.getAttribute(atControl.obligatorio);
        let valorDeFecha = controlDeFecha.value; //.replace(/\n/g, "\r\n");
        let fechaHoraFijada = false;
        if (obligatorio === "S" && NoDefinida(valorDeFecha)) {
            if (controlDeFecha.readOnly) {
                valorDeFecha = new Date(Date.now()).toISOString();
                fechaHoraFijada = true;
            }
            else {
                controlDeFecha.classList.remove(ClaseCss.crtlValido);
                controlDeFecha.classList.add(ClaseCss.crtlNoValido);
                throw new Error(`El campo: ${propiedadDto}, es obligatorio`);
            }
        }
        let fecha = new Date(valorDeFecha);
        if (FechaValida(fecha)) {
            let idHora = controlDeFecha.getAttribute(atSelectorDeFecha.hora);
            if (!IsNullOrEmpty(idHora)) {
                let controlDeHora = document.getElementById(idHora);
                if (!fechaHoraFijada) {
                    let valorDeHora = controlDeHora.value.split(':');
                    let hora = Numero(valorDeHora[0]);
                    let minuto = Numero(valorDeHora[1]);
                    let segundos = Numero(valorDeHora[2]);
                    let milisegundos = Numero(controlDeHora.getAttribute(atSelectorDeFecha.milisegundos));
                    fecha.setHours(hora);
                    fecha.setMinutes(minuto);
                    fecha.setSeconds(segundos);
                    fecha.setMilliseconds(milisegundos);
                }
            }
            var utcFecha = new Date(Date.UTC(fecha.getFullYear(), fecha.getMonth(), fecha.getDate(), fecha.getHours(), fecha.getMinutes(), fecha.getSeconds(), fecha.getMilliseconds()));
            elementoJson[propiedadDto] = utcFecha;
        }
        else
            elementoJson[propiedadDto] = '';
    }
    function MapearTextosAlJson(panel, elementoJson) {
        let areas = panel.querySelectorAll(`textarea[tipo="${TipoControl.AreaDeTexto}"]`);
        for (let i = 0; i < areas.length; i++) {
            MapearTextoAlJson(areas[i], elementoJson);
        }
    }
    ApiControl.MapearTextosAlJson = MapearTextosAlJson;
    function MapearTextoAlJson(area, elementoJson) {
        let propiedadDto = area.getAttribute(atControl.propiedad);
        let obligatorio = area.getAttribute(atControl.obligatorio);
        let valor = area.value; //.replace(/\n/g, "\r\n");
        if (obligatorio === "S" && NoDefinida(valor)) {
            area.classList.remove(ClaseCss.crtlValido);
            area.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`El campo ${propiedadDto} es obligatorio`);
        }
        elementoJson[propiedadDto] = valor;
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
        ApiControl.MapearTextosAlJson(panel, elementoJson);
        ApiControl.MapearArchivosAlJson(panel, elementoJson);
        ApiControl.MapearUrlArchivosAlJson(panel, elementoJson);
        ApiControl.MapearCheckesAlJson(panel, elementoJson);
        ApiControl.MapearFechasAlJson(panel, elementoJson);
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
        modal.setAttribute('altura-inicial', "0");
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
    function AplicarModoDeAccesoAlNegocio(opcionesGenerales, modoDeAccesoDelUsuario) {
        for (var i = 0; i < opcionesGenerales.length; i++) {
            let opcion = opcionesGenerales[i];
            if (ApiControl.EstaBloqueada(opcion))
                continue;
            let permisosNecesarios = opcion.getAttribute(atOpcionDeMenu.permisosNecesarios);
            if (permisosNecesarios === ModoDeAccesoDeDatos.Administrador && modoDeAccesoDelUsuario !== ModoDeAccesoDeDatos.Administrador)
                opcion.disabled = true;
            else if (permisosNecesarios === ModoDeAccesoDeDatos.Gestor && (modoDeAccesoDelUsuario === ModoDeAccesoDeDatos.Consultor || modoDeAccesoDelUsuario === ModoDeAccesoDeDatos.SinPermiso))
                opcion.disabled = true;
            else if (permisosNecesarios === ModoDeAccesoDeDatos.Consultor && modoDeAccesoDelUsuario === ModoDeAccesoDeDatos.SinPermiso)
                opcion.disabled = true;
            else
                opcion.disabled = false;
        }
    }
    ApiCrud.AplicarModoDeAccesoAlNegocio = AplicarModoDeAccesoAlNegocio;
    function AplicarModoAccesoAlElemento(opcion, hacerLaInterseccion, modoAccesoDelUsuarioAlElemento) {
        if (ApiControl.EstaBloqueada(opcion))
            return;
        let estaDeshabilitado = opcion.disabled;
        let permisosNecesarios = opcion.getAttribute(atOpcionDeMenu.permisosNecesarios);
        let permiteMultiSeleccion = opcion.getAttribute(atOpcionDeMenu.permiteMultiSeleccion);
        if (!EsTrue(permiteMultiSeleccion) && hacerLaInterseccion) {
            opcion.disabled = true;
            return;
        }
        if (permisosNecesarios === ModoDeAccesoDeDatos.Administrador && modoAccesoDelUsuarioAlElemento !== ModoDeAccesoDeDatos.Administrador)
            opcion.disabled = true;
        else if (permisosNecesarios === ModoDeAccesoDeDatos.Gestor && (modoAccesoDelUsuarioAlElemento === ModoDeAccesoDeDatos.Consultor || modoAccesoDelUsuarioAlElemento === ModoDeAccesoDeDatos.SinPermiso))
            opcion.disabled = true;
        else if (permisosNecesarios === ModoDeAccesoDeDatos.Consultor && modoAccesoDelUsuarioAlElemento === ModoDeAccesoDeDatos.SinPermiso)
            opcion.disabled = true;
        else
            opcion.disabled = (estaDeshabilitado && hacerLaInterseccion) || false;
    }
    ApiCrud.AplicarModoAccesoAlElemento = AplicarModoAccesoAlElemento;
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