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
            bloquearOpcionDeMenu(opcion, true);
        }
    }
    ApiControl.BloquearMenu = BloquearMenu;
    function OcultarOpcionDeMenu(panel, nombreOpcion) {
        let opcion = buscarOpcionDeMenu(panel, nombreOpcion);
        if (opcion !== null) {
            ocultarOpcionDeMenu(opcion, true);
            return true;
        }
        return false;
    }
    ApiControl.OcultarOpcionDeMenu = OcultarOpcionDeMenu;
    function OcultarMostrarOpcionDeMenu(opcion, ocultar) {
        ocultarOpcionDeMenu(opcion, ocultar);
    }
    ApiControl.OcultarMostrarOpcionDeMenu = OcultarMostrarOpcionDeMenu;
    function BloquearDesbloquearOpcionDeMenu(opcion, bloquear) {
        bloquearOpcionDeMenu(opcion, bloquear);
    }
    ApiControl.BloquearDesbloquearOpcionDeMenu = BloquearDesbloquearOpcionDeMenu;
    function BloquearOpcionDeMenu(panel, nombreOpcion) {
        let opcion = buscarOpcionDeMenu(panel, nombreOpcion);
        if (opcion !== null) {
            bloquearOpcionDeMenu(opcion, true);
            return true;
        }
        return false;
    }
    ApiControl.BloquearOpcionDeMenu = BloquearOpcionDeMenu;
    function DesbloquearOpcionDeMenu(panel, nombreOpcion) {
        let opcion = buscarOpcionDeMenu(panel, nombreOpcion);
        if (opcion !== null) {
            bloquearOpcionDeMenu(opcion, false);
            return true;
        }
        return false;
    }
    ApiControl.DesbloquearOpcionDeMenu = DesbloquearOpcionDeMenu;
    function buscarOpcionDeMenu(panel, nombreOpcion) {
        let opciones = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.opcion}"]`);
        for (var i = 0; i < opciones.length; i++) {
            let opcion = opciones[i];
            if (opcion.value === nombreOpcion)
                return opcion;
        }
        return null;
    }
    function bloquearOpcionDeMenu(opcion, bloquear) {
        opcion.disabled = bloquear;
        opcion.setAttribute(atOpcionDeMenu.bloqueada, bloquear ? "S" : "N");
    }
    function ocultarOpcionDeMenu(opcion, ocultar) {
        opcion.hidden = ocultar;
        opcion.setAttribute(atOpcionDeMenu.oculta, ocultar ? "S" : "N");
    }
    function EstaBloqueada(opcion) { return opcion.getAttribute(atOpcionDeMenu.bloqueada) === "S" || opcion.disabled; }
    ApiControl.EstaBloqueada = EstaBloqueada;
    function EstaOculta(opcion) { return opcion.getAttribute(atOpcionDeMenu.oculta) === "S" || opcion.hidden; }
    ApiControl.EstaOculta = EstaOculta;
    function BloquearListaDinamicaPorPropiedad(panel, propiedad) {
        let lista = BuscarListaDinamica(panel, propiedad, atControl.propiedad);
        if (!NoDefinida(lista)) {
            lista.disabled = true;
            lista.readOnly = true;
            return true;
        }
        return false;
    }
    ApiControl.BloquearListaDinamicaPorPropiedad = BloquearListaDinamicaPorPropiedad;
    function DesbloquearListaDinamicaPorPropiedad(panel, propiedad) {
        let lista = BuscarListaDinamica(panel, propiedad, atControl.propiedad);
        if (lista !== null) {
            lista.disabled = false;
            lista.readOnly = false;
            return true;
        }
        return false;
    }
    ApiControl.DesbloquearListaDinamicaPorPropiedad = DesbloquearListaDinamicaPorPropiedad;
    function BloquearListaDinamica(lista, bloquear) {
        lista.disabled = bloquear;
        lista.readOnly = bloquear;
    }
    ApiControl.BloquearListaDinamica = BloquearListaDinamica;
    function BloquearEditorPorPropiedad(panel, propiedad) {
        let editor = BuscarEditor(panel, propiedad);
        if (editor !== null) {
            return BloquearEditor(editor);
        }
        return false;
    }
    ApiControl.BloquearEditorPorPropiedad = BloquearEditorPorPropiedad;
    function BloquearEditor(editor) {
        if (editor !== null) {
            editor.disabled = true;
            editor.readOnly = true;
            return true;
        }
        return false;
    }
    ApiControl.BloquearEditor = BloquearEditor;
    function DesbloquearEditorPorPropiedad(panel, propiedad) {
        let editor = BuscarEditor(panel, propiedad);
        if (editor !== null) {
            return DesbloquearEditor(editor);
        }
        return false;
    }
    ApiControl.DesbloquearEditorPorPropiedad = DesbloquearEditorPorPropiedad;
    function DesbloquearEditor(editor) {
        if (editor !== null) {
            editor.disabled = false;
            editor.readOnly = false;
            return true;
        }
        return false;
    }
    ApiControl.DesbloquearEditor = DesbloquearEditor;
    function BuscarListaDinamicaPorGuardarEn(panel, guardarEn) {
        return BuscarListaDinamica(panel, guardarEn, atListasDinamicasDto.guardarEn);
    }
    ApiControl.BuscarListaDinamicaPorGuardarEn = BuscarListaDinamicaPorGuardarEn;
    function BuscarListaDinamicaPorPropiedad(panel, propiedad) {
        return BuscarListaDinamica(panel, propiedad, atControl.propiedad);
    }
    ApiControl.BuscarListaDinamicaPorPropiedad = BuscarListaDinamicaPorPropiedad;
    function BuscarEditor(panel, propiedad) {
        let editores = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.Editor}"]`);
        for (let i = 0; i < editores.length; i++) {
            let lista = editores[i];
            if (lista.getAttribute(atControl.propiedad) == propiedad.toLocaleLowerCase()) {
                return lista;
            }
        }
        return null;
    }
    function BuscarListaDinamica(panel, propiedad, atributo) {
        let listas = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.ListaDinamica}"]`);
        for (let i = 0; i < listas.length; i++) {
            let lista = listas[i];
            if (lista.getAttribute(atributo).toLocaleLowerCase() === propiedad.toLocaleLowerCase()) {
                return lista;
            }
        }
        return null;
    }
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
    function AsignarFecha(panel, propiedad, fecha) {
        let control = BuscarFecha(panel, propiedad);
        if (control !== null) {
            MapearAlControl.FechaDate(control, fecha);
            if (control.getAttribute(atControl.tipo) === TipoControl.SelectorDeFechaHora)
                return MapearAlControl.HoraDate(control, fecha);
            return true;
        }
        return false;
    }
    ApiControl.AsignarFecha = AsignarFecha;
    function BuscarFecha(panel, propiedad) {
        let fechas = panel.querySelectorAll(`input[tipo="${TipoControl.SelectorDeFecha}"]`);
        for (var i = 0; i < fechas.length; i++) {
            let fecha = fechas[i];
            if (fecha.getAttribute(atControl.propiedad) == propiedad.toLocaleLowerCase()) {
                return fecha;
            }
        }
        fechas = panel.querySelectorAll(`input[tipo="${TipoControl.SelectorDeFechaHora}"]`);
        for (var i = 0; i < fechas.length; i++) {
            let fecha = fechas[i];
            if (fecha.getAttribute(atControl.propiedad) == propiedad.toLocaleLowerCase()) {
                return fecha;
            }
        }
        return null;
    }
    function AjustarColumnaDelGrid(columanDeOrdenacion) {
        let columna = document.getElementById(columanDeOrdenacion.IdColumna);
        if (NoDefinida(columna)) {
            MensajesSe.Error("AjustarColumnaDelGrid", `la columna ${columanDeOrdenacion.IdColumna} no está definida en el Grid`);
            return false;
        }
        let a = columna.getElementsByTagName('a')[0];
        if (NoDefinida(a)) {
            MensajesSe.Error("AjustarColumnaDelGrid", `el orden aplicado a la propiedad ${columanDeOrdenacion.Propiedad} no se puede aplicar`);
            return false;
        }
        columna.setAttribute(atControl.modoOrdenacion, columanDeOrdenacion.Modo);
        a.setAttribute("class", columanDeOrdenacion.ccsClase);
        return true;
    }
    ApiControl.AjustarColumnaDelGrid = AjustarColumnaDelGrid;
    function LimpiarEditor(editor) {
        MapearAlControl.Restrictor(editor, 0, "");
        BlanquearDependientes(editor);
    }
    ApiControl.LimpiarEditor = LimpiarEditor;
    function LimpiarListaDinamica(lista) {
        MapearAlControl.ListaDinamica(lista, 0, "");
        BorrarOpcionesListaDinamica(lista);
        BlanquearDependientes(lista);
    }
    ApiControl.LimpiarListaDinamica = LimpiarListaDinamica;
    function BorrarOpcionesListaDinamica(lista) {
        let idDatos = lista.getAttribute(atListas.idDeLaLista);
        if (!IsNullOrEmpty(idDatos)) {
            var opciones = document.getElementById(idDatos);
            //var numChilds = opciones.children.length;
            //for (var i = 0; i < numChilds; i++) {
            //    opciones.children[i].remove();
            //}
            opciones.innerHTML = "";
        }
    }
    ApiControl.BorrarOpcionesListaDinamica = BorrarOpcionesListaDinamica;
    function BuscarOpcionesListaDinamica(lista, valor) {
        let idDatos = lista.getAttribute(atListas.idDeLaLista);
        if (!IsNullOrEmpty(idDatos)) {
            var opciones = document.getElementById(idDatos);
            var numChilds = opciones.children.length;
            for (var i = 0; i < numChilds; i++) {
                if (opciones.children[i].attributes[1].value === valor)
                    return Numero(opciones.children[i].attributes[0].value);
            }
            opciones.innerHTML = "";
        }
        MensajesSe.EmitirExcepcion("Buscar opción en lista", `No se ha localizado el valor ${valor} en la lista ${lista.id}`);
    }
    ApiControl.BuscarOpcionesListaDinamica = BuscarOpcionesListaDinamica;
    function BlanquearDependientes(control) {
        let BlanquearControlDePropiedad = control.getAttribute(atListasDinamicas.BlanquearControlAsociado);
        if (!IsNullOrEmpty(BlanquearControlDePropiedad)) {
            let contenedor = control.getAttribute(atListasDinamicas.ContenidoEn);
            let divContenedor = document.getElementById(contenedor);
            let controlDependiente = divContenedor.querySelector(`[${atControl.propiedad}=${BlanquearControlDePropiedad}]`);
            let tipo = controlDependiente.getAttribute(atControl.tipo);
            if (tipo === TipoControl.restrictorDeEdicion)
                LimpiarEditor(controlDependiente);
            else if (tipo === TipoControl.ListaDinamica) {
                LimpiarListaDinamica(controlDependiente);
            }
        }
    }
    ApiControl.BlanquearDependientes = BlanquearDependientes;
    function BlanquearEditor(editor) {
        AnularError(editor);
        editor.value = "";
    }
    ApiControl.BlanquearEditor = BlanquearEditor;
    function AnularError(control) {
        control.classList.remove(ClaseCss.crtlNoValido);
        control.classList.add(ClaseCss.crtlValido);
    }
    ApiControl.AnularError = AnularError;
    function MarcarError(control) {
        control.classList.add(ClaseCss.crtlNoValido);
        control.classList.remove(ClaseCss.crtlValido);
    }
    ApiControl.MarcarError = MarcarError;
    function BlanquearListaDeElemento(selector) {
        selector.classList.remove(ClaseCss.crtlNoValido);
        selector.classList.add(ClaseCss.crtlValido);
        selector.selectedIndex = 0;
    }
    ApiControl.BlanquearListaDeElemento = BlanquearListaDeElemento;
    function LeerEntreFechas(controlDeFechaDesde) {
        let idHora = controlDeFechaDesde.getAttribute(atEntreFechas.horaDesde);
        let entreFechas = LeerFechaHora(controlDeFechaDesde, idHora);
        let idFechaHasta = controlDeFechaDesde.getAttribute(atEntreFechas.fechaHasta);
        let fechaHasta = document.getElementById(idFechaHasta);
        idHora = controlDeFechaDesde.getAttribute(atEntreFechas.horaHasta);
        entreFechas = entreFechas + '-' + LeerFechaHora(fechaHasta, idHora);
        return entreFechas;
    }
    ApiControl.LeerEntreFechas = LeerEntreFechas;
    function LeerFechaHora(controlDeFecha, idHora) {
        let valorDeFecha = controlDeFecha.value;
        let resultado = "";
        if (!IsNullOrEmpty(valorDeFecha)) {
            let fecha = new Date(valorDeFecha);
            resultado = fecha.toLocaleDateString();
            let controlDeHora = document.getElementById(idHora);
            let valorDeHora = controlDeHora.value;
            if (!IsNullOrEmpty(valorDeHora)) {
                resultado = resultado + ' ' + valorDeHora;
            }
        }
        return resultado;
    }
    function MapearComoOrdenar(columna, orden) {
        columna.setAttribute(atControl.ordenarPor, orden.OrdenarPor);
        columna.setAttribute(atControl.modoOrdenacion, orden.Modo);
    }
    ApiControl.MapearComoOrdenar = MapearComoOrdenar;
})(ApiControl || (ApiControl = {}));
var ApiCrud;
(function (ApiCrud) {
    function ElementoSeleccionado(lista) {
        let id = ApiControl.BuscarOpcionesListaDinamica(lista, lista.value);
        let idAnterior = Numero(lista.getAttribute(atListasDinamicas.idSelAlEntrar));
        if (idAnterior !== id) {
            ApiControl.BlanquearDependientes(lista);
            lista.setAttribute(atListasDinamicas.idSeleccionado, id.toString());
        }
    }
    ApiCrud.ElementoSeleccionado = ElementoSeleccionado;
    function CrearEnlaceAlElemento(divDeElementos, elemento) {
        let a = document.createElement("a");
        let url = `${window.location}`;
        if (url.indexOf("?id=") <= 0)
            url = url + `?id=${elemento.Id}`;
        a.setAttribute("href", url);
        a.target = "_blank";
        a.setAttribute(atControl.idElemento, elemento.Id.toString());
        let aTexto = document.createTextNode(elemento.Texto);
        a.appendChild(aTexto);
        divDeElementos.appendChild(a);
        var br = document.createElement("br");
        divDeElementos.appendChild(br);
    }
    ApiCrud.CrearEnlaceAlElemento = CrearEnlaceAlElemento;
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
        BlanquearListaDeElementos(panel);
        BlanquearArchivos(panel);
    }
    ApiCrud.BlanquearControlesDeIU = BlanquearControlesDeIU;
    function MostrarPanel(panel) {
        panel.classList.remove(ClaseCss.divNoVisible);
        panel.classList.add(ClaseCss.divVisible);
    }
    ApiCrud.MostrarPanel = MostrarPanel;
    function OcultarPanel(panel) {
        panel.classList.add(ClaseCss.divNoVisible);
        panel.classList.remove(ClaseCss.divVisible);
    }
    ApiCrud.OcultarPanel = OcultarPanel;
    function CerrarModalPorId(id) {
        let modal = document.getElementById(id);
        if (NoDefinida(modal))
            throw new Error(`La modal ${id} no está definida`);
        CerrarModal(modal);
    }
    ApiCrud.CerrarModalPorId = CerrarModalPorId;
    function OcultarModalPorId(id) {
        let modal = document.getElementById(id);
        if (NoDefinida(modal))
            throw new Error(`La modal ${id} no está definida`);
        OcultarModal(modal);
    }
    ApiCrud.OcultarModalPorId = OcultarModalPorId;
    function CerrarModal(modal) {
        BlanquearSelectoresDeElemento(modal);
        OcultarModal(modal);
    }
    ApiCrud.CerrarModal = CerrarModal;
    function OcultarModal(modal) {
        modal.style.display = "none";
    }
    ApiCrud.OcultarModal = OcultarModal;
    function AbrirModalPorId(id) {
        let modal = document.getElementById(id);
        if (NoDefinida(modal))
            throw new Error(`La modal ${id} no está definida`);
        AbriModal(modal);
    }
    ApiCrud.AbrirModalPorId = AbrirModalPorId;
    function AbriModal(modal) {
        modal.style.display = 'block';
    }
    ApiCrud.AbriModal = AbriModal;
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
                if (EsTrue(permiteMultiSeleccion)) {
                    let numero = Numero(opcion.getAttribute(atOpcionDeMenu.numeroMaximoSeleccionable));
                    if (numero === -1 || seleccionadas <= numero)
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
            if (!opcion.disabled) {
                let permiteMultiSeleccion = opcion.getAttribute(atOpcionDeMenu.permiteMultiSeleccion);
                if (!EsTrue(permiteMultiSeleccion)) {
                    opcion.disabled = !(seleccionadas === 1);
                    return;
                }
                let numero = Numero(opcion.getAttribute(atOpcionDeMenu.numeroMaximoSeleccionable));
                if (numero !== -1)
                    opcion.disabled = (seleccionadas > numero);
            }
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
    function ObtenerSelector(idSelector) {
        let selector = document.getElementById(idSelector);
        if (NoDefinida(selector))
            throw new Error(`el selector ${idSelector} no está definido`);
        return selector;
    }
    ApiCrud.ObtenerSelector = ObtenerSelector;
    function ObtenerEditorAsociadoAlSelector(selector) {
        let idEditor = selector.getAttribute(atSelectorDeElementos.EditorAsociado);
        let editor = document.getElementById(idEditor);
        if (NoDefinida(editor))
            throw new Error(`el editor ${idEditor} no está definido en el selector ${selector.id}`);
        return editor;
    }
    ApiCrud.ObtenerEditorAsociadoAlSelector = ObtenerEditorAsociadoAlSelector;
    function BlanquearEditores(panel) {
        let editores = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.Editor}"]`);
        for (let i = 0; i < editores.length; i++) {
            ApiControl.BlanquearEditor(editores[i]);
        }
    }
    function BlanquearListaDeElementos(panel) {
        let selectores = panel.querySelectorAll(`select[${atControl.tipo}="${TipoControl.ListaDeElementos}"]`);
        for (let i = 0; i < selectores.length; i++) {
            ApiControl.BlanquearListaDeElemento(selectores[i]);
        }
    }
    function BlanquearArchivos(panel) {
        let archivos = panel.querySelectorAll(`${atControl.tipo}[tipo="${TipoControl.Archivo}"]`);
        for (let i = 0; i < archivos.length; i++) {
            ApiDeArchivos.BlanquearArchivo(archivos[i], true);
        }
    }
    function BlanquearSelectoresDeElemento(modal) {
        let selectores = modal.querySelectorAll(`[${atControl.tipo}=${TipoControl.SelectorDeElementos}]`);
        for (let i = 0; i < selectores.length; i++) {
            selectores[i].setAttribute(atSelectorDeElementos.Seleccionados, '');
            let idEditor = selectores[i].getAttribute(atSelectorDeElementos.EditorAsociado);
            let editor = document.getElementById(idEditor);
            ApiControl.BlanquearEditor(editor);
        }
    }
    function EliminarReferenciasDeUnDiv(modal) {
        let referencias = modal.querySelectorAll("a");
        for (let i = 0; i < referencias.length; i++) {
            referencias[i].remove();
        }
    }
    ApiCrud.EliminarReferenciasDeUnDiv = EliminarReferenciasDeUnDiv;
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
        let filtros = [];
        filtros.push(filtroRestrictor);
        valores.Agregar(Sesion.paginaDestino, navegarAlCrud);
        valores.Agregar(Sesion.restrictores, filtros);
        valores.Agregar(Sesion.idSeleccionado, idSeleccionado);
        Navegar(crud, form, valores);
    }
    ApiRuote.NavegarARelacionar = NavegarARelacionar;
    function NavegarADependientes(crud, idOpcionDeMenu, idSeleccionado, filtroRestrictor) {
        let form = document.getElementById(idOpcionDeMenu);
        if (form === null)
            throw new Error(`La opción de menú '${idOpcionDeMenu}' está mal definida, actualice el descriptor`);
        let navegarAlCrud = form.getAttribute(atNavegar.navegarAlCrud);
        let soloMapearEnELFiltro = EsTrue(form.getAttribute(atNavegar.soloMapearEnElFiltro));
        let valores = new Diccionario();
        valores.Agregar(Sesion.paginaDestino, navegarAlCrud);
        valores.Agregar(Sesion.restrictores, filtroRestrictor);
        valores.Agregar(Sesion.idSeleccionado, idSeleccionado);
        valores.Agregar(Sesion.SoloMapearEnElFiltro, soloMapearEnELFiltro);
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
        let filtros = AnadirRestrictores(input);
        let clausula = new ClausulaDeFiltrado(buscarPor, criterio, valor.toString());
        filtros.push(clausula);
        return filtros;
    }
    ApiFiltro.DefinirFiltroListaDinamica = DefinirFiltroListaDinamica;
    function AnadirRestrictores(input) {
        var filtros = new Array();
        let restringirPor = input.getAttribute(atListasDinamicas.RestringidoPor);
        if (Definida(restringirPor)) {
            let contenedor = input.getAttribute(atListasDinamicas.ContenidoEn);
            if (NoDefinida(contenedor))
                MensajesSe.EmitirExcepcion("Definir filtro lista dinámica", `No se puede definir el filtro para la propiedad ${input.id} ya que no se ha definido el atributo ${atListasDinamicas.ContenidoEn}`);
            let divControl = document.getElementById(contenedor);
            let restrictor = divControl.querySelector(`[${atControl.propiedad}=${restringirPor}]`);
            if (NoDefinida(restrictor))
                MensajesSe.EmitirExcepcion("Definir filtro lista dinámica", `No se  ha encontratado el control con la propiedad ${restringirPor} asociado a la lista ${input.id} en el contenedor ${contenedor}`);
            if (restrictor instanceof HTMLInputElement) {
                let tipo = restrictor.getAttribute(atControl.tipo);
                if (tipo === TipoControl.restrictorDeEdicion)
                    filtros.push(ObtenerValorDelRestrictorDeEdicion(restrictor, restringirPor));
                else if (tipo === TipoControl.ListaDinamica) {
                    let propiedadRestrictora = input.getAttribute(atListasDinamicas.PropiedadRestrictora);
                    let valorRestrictor = restrictor.getAttribute(atListasDinamicas.idSeleccionado);
                    let a = ObtenerValorDeLaListaDinamica(restrictor, propiedadRestrictora, Numero(valorRestrictor));
                    if (Definida(a))
                        filtros.push(a);
                }
                else
                    MensajesSe.EmitirExcepcion("Definir filtro lista dinámica", `No se  ha definido como obtener el valor que restringir en el control ${restringirPor} asociado a la lista ${input.id} en el contenedor ${contenedor}`);
            }
        }
        return filtros;
    }
    ApiFiltro.AnadirRestrictores = AnadirRestrictores;
    function ObtenerValorDelRestrictorDeEdicion(restrictor, restringirPor) {
        let valorRestrictor = restrictor.getAttribute(atControl.restrictor);
        if (Numero(valorRestrictor) === 0)
            MensajesSe.EmitirExcepcion("Definir filtro lista dinámica", `No se  ha definido el valor por el que restringir en el control ${restringirPor}`);
        return new ClausulaDeFiltrado(restringirPor, atCriterio.igual, valorRestrictor);
    }
    function ObtenerValorDeLaListaDinamica(lista, propiedadRestrictora, valorRestrictor) {
        if (IsNullOrEmpty(propiedadRestrictora))
            MensajesSe.EmitirExcepcion("Obtener filtro de la lista dinámica", `no se ha definido la propiedad restrictora en el control ${lista.id}`);
        if (Number(valorRestrictor) === 0)
            return null;
        return new ClausulaDeFiltrado(propiedadRestrictora, atCriterio.igual, valorRestrictor.toString());
    }
})(ApiFiltro || (ApiFiltro = {}));
//# sourceMappingURL=ApiCrud.js.map