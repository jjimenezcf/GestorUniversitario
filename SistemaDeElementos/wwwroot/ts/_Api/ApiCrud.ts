namespace ApiControl {

    export function OcultarMostrarExpansor(idHtmlExpansor: string, idHtmlBloque: string): void {
        let extensor: HTMLInputElement = document.getElementById(`${idHtmlExpansor}`) as HTMLInputElement;
        if (NumeroMayorDeCero(extensor.value)) {
            extensor.value = "0";
            ApiCrud.OcultarPanel(document.getElementById(`${idHtmlBloque}`) as HTMLDivElement);
        }
        else {
            extensor.value = "1";
            ApiCrud.MostrarPanel(document.getElementById(`${idHtmlBloque}`) as HTMLDivElement);
        }
        //EntornoSe.AjustarModalesAbiertas();
    }

    export function BloquearMenu(panel: HTMLDivElement): void {
        let opciones: NodeListOf<HTMLButtonElement> = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.opcion}"]`) as NodeListOf<HTMLButtonElement>;
        for (var i = 0; i < opciones.length; i++) {
            let opcion: HTMLButtonElement = opciones[i];
            let clase: string = opcion.getAttribute(atOpcionDeMenu.clase);
            if (clase === ClaseDeOpcioDeMenu.Basico)
                continue;
            bloquearOpcionDeMenu(opcion, true);
        }
    }

    export function OcultarOpcionDeMenu(panel: HTMLDivElement, nombreOpcion: string): boolean {
        let opcion: HTMLButtonElement = buscarOpcionDeMenu(panel, nombreOpcion);
        if (opcion !== null) {
            ocultarOpcionDeMenu(opcion, true);
            return true;
        }
        return false;
    }

    export function OcultarMostrarOpcionDeMenu(opcion: HTMLButtonElement, ocultar: boolean): void {
        ocultarOpcionDeMenu(opcion, ocultar);
    }


    export function BloquearDesbloquearOpcionDeMenu(opcion: HTMLButtonElement, bloquear: boolean): void {
        bloquearOpcionDeMenu(opcion, bloquear);
    }

    export function BloquearOpcionDeMenu(panel: HTMLDivElement, nombreOpcion: string): boolean {
        let opcion: HTMLButtonElement = buscarOpcionDeMenu(panel, nombreOpcion);
        if (opcion !== null) {
            bloquearOpcionDeMenu(opcion, true);
            return true;
        }
        return false;
    }

    export function DesbloquearOpcionDeMenu(panel: HTMLDivElement, nombreOpcion: string): boolean {
        let opcion: HTMLButtonElement = buscarOpcionDeMenu(panel, nombreOpcion);
        if (opcion !== null) {
            bloquearOpcionDeMenu(opcion, false);
            return true;
        }
        return false;
    }

    function buscarOpcionDeMenu(panel: HTMLDivElement, nombreOpcion: string): HTMLButtonElement {
        let opciones: NodeListOf<HTMLButtonElement> = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.opcion}"]`) as NodeListOf<HTMLButtonElement>;
        for (var i = 0; i < opciones.length; i++) {
            let opcion: HTMLButtonElement = opciones[i];
            if (opcion.value === nombreOpcion)
                return opcion;
        }
        return null;
    }

    function bloquearOpcionDeMenu(opcion: HTMLButtonElement, bloquear: boolean): void {
        opcion.disabled = bloquear;
        opcion.setAttribute(atOpcionDeMenu.bloqueada, bloquear ? "S" : "N");
    }

    function ocultarOpcionDeMenu(opcion: HTMLButtonElement, ocultar: boolean): void {
        opcion.hidden = ocultar;
        opcion.setAttribute(atOpcionDeMenu.oculta, ocultar ? "S" : "N");
    }

    export function EstaBloqueada(opcion: HTMLButtonElement) { return opcion.getAttribute(atOpcionDeMenu.bloqueada) === "S" || opcion.disabled; }

    export function EstaOculta(opcion: HTMLButtonElement) { return opcion.getAttribute(atOpcionDeMenu.oculta) === "S" || opcion.hidden; }

    export function BloquearListaDinamicaPorPropiedad(panel: HTMLDivElement, propiedad: string): boolean {
        let lista: HTMLInputElement = BuscarListaDinamica(panel, propiedad, atControl.propiedad);
        if (!NoDefinida(lista)) {
            lista.disabled = true;
            lista.readOnly = true;
            return true;
        }
        return false;
    }

    export function DesbloquearListaDinamicaPorPropiedad(panel: HTMLDivElement, propiedad: string): boolean {
        let lista: HTMLInputElement = BuscarListaDinamica(panel, propiedad, atControl.propiedad);
        if (lista !== null) {
            lista.disabled = false;
            lista.readOnly = false;
            return true;
        }
        return false;
    }

    export function BloquearListaDinamica(lista: HTMLInputElement, bloquear: boolean): void {
        lista.disabled = bloquear;
        lista.readOnly = bloquear;
    }

    export function BloquearEditorPorPropiedad(panel: HTMLDivElement, propiedad: string): boolean {
        let editor: HTMLInputElement = BuscarEditor(panel, propiedad);
        if (editor !== null) {
            return BloquearEditor(editor);
        }
        return false;
    }

    export function BloquearEditor(editor: HTMLInputElement): boolean {
        if (editor !== null) {
            editor.disabled = true;
            editor.readOnly = true;
            return true;
        }
        return false;
    }

    export function DesbloquearEditorPorPropiedad(panel: HTMLDivElement, propiedad: string): boolean {
        let editor: HTMLInputElement = BuscarEditor(panel, propiedad);
        if (editor !== null) {
            return DesbloquearEditor(editor);
        }
        return false;
    }

    export function DesbloquearEditor(editor: HTMLInputElement): boolean {
        if (editor !== null) {
            editor.disabled = false;
            editor.readOnly = false;
            return true;
        }
        return false;
    }


    export function BuscarListaDinamicaPorGuardarEn(panel: HTMLDivElement, guardarEn: string): HTMLInputElement {
        return BuscarListaDinamica(panel, guardarEn, atListasDinamicasDto.guardarEn);
    }

    export function BuscarListaDinamicaPorPropiedad(panel: HTMLDivElement, propiedad: string): HTMLInputElement {
        return BuscarListaDinamica(panel, propiedad, atControl.propiedad);
    }


    function BuscarEditor(panel: HTMLDivElement, propiedad: string): HTMLInputElement {
        let editores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.Editor}"]`) as NodeListOf<HTMLInputElement>;
        for (let i = 0; i < editores.length; i++) {
            let lista: HTMLInputElement = editores[i];
            if (lista.getAttribute(atControl.propiedad) == propiedad.toLocaleLowerCase()) {
                return lista;
            }
        }
        return null;
    }

    function BuscarListaDinamica(panel: HTMLDivElement, propiedad: string, atributo: string): HTMLInputElement {
        let listas: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.ListaDinamica}"]`) as NodeListOf<HTMLInputElement>;
        for (let i = 0; i < listas.length; i++) {
            let lista: HTMLInputElement = listas[i] as HTMLInputElement;
            if (lista.getAttribute(atributo).toLocaleLowerCase() === propiedad.toLocaleLowerCase()) {
                return lista;
            }
        }
        return null;
    }

    export function BlanquearFecha(fecha: HTMLInputElement) {
        fecha.value = "";
        let tipo: string = fecha.getAttribute(atControl.tipo);
        if (tipo === TipoControl.SelectorDeFechaHora) {
            let idHora: string = fecha.getAttribute(atSelectorDeFecha.hora);
            if (!IsNullOrEmpty(idHora)) {
                let controlHora: HTMLInputElement = document.getElementById(idHora) as HTMLInputElement;
                controlHora.value = '';
                controlHora.setAttribute(atSelectorDeFecha.milisegundos, '0');
            }
        }
    }

    export function AsignarFecha(panel: HTMLDivElement, propiedad: string, fecha: Date): boolean {
        let control: HTMLInputElement = BuscarFecha(panel, propiedad);
        if (control !== null) {
            MapearAlControl.FechaDate(control, fecha);

            if (control.getAttribute(atControl.tipo) === TipoControl.SelectorDeFechaHora)
                return MapearAlControl.HoraDate(control, fecha);
            return true;
        }
        return false;
    }


    function BuscarFecha(panel: HTMLDivElement, propiedad: string): HTMLInputElement {
        let fechas: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.SelectorDeFecha}"]`) as NodeListOf<HTMLInputElement>;
        for (var i = 0; i < fechas.length; i++) {
            let fecha: HTMLInputElement = fechas[i] as HTMLInputElement;
            if (fecha.getAttribute(atControl.propiedad) == propiedad.toLocaleLowerCase()) {
                return fecha;
            }
        }

        fechas = panel.querySelectorAll(`input[tipo="${TipoControl.SelectorDeFechaHora}"]`) as NodeListOf<HTMLInputElement>;
        for (var i = 0; i < fechas.length; i++) {
            let fecha: HTMLInputElement = fechas[i] as HTMLInputElement;
            if (fecha.getAttribute(atControl.propiedad) == propiedad.toLocaleLowerCase()) {
                return fecha;
            }
        }

        return null;
    }

    export function AjustarColumnaDelGrid(columanDeOrdenacion: Tipos.Orden): boolean {
        let columna: HTMLTableHeaderCellElement = document.getElementById(columanDeOrdenacion.IdColumna) as HTMLTableHeaderCellElement;
        if (NoDefinida(columna)) {
            MensajesSe.Error("AjustarColumnaDelGrid", `la columna ${columanDeOrdenacion.IdColumna} no está definida en el Grid`);
            return false;
        }

        let a: HTMLElement = columna.getElementsByTagName('a')[0] as HTMLElement;
        if (NoDefinida(a)) {
            MensajesSe.Error("AjustarColumnaDelGrid", `el orden aplicado a la propiedad ${columanDeOrdenacion.Propiedad} no se puede aplicar`);
            return false;
        }

        columna.setAttribute(atControl.modoOrdenacion, columanDeOrdenacion.Modo);
        a.setAttribute("class", columanDeOrdenacion.ccsClase);
        return true;
    }

    export function LimpiarEditor(editor: HTMLInputElement): void {
        MapearAlControl.Restrictor(editor, 0, "");
        BlanquearDependientes(editor);
    }

    export function LimpiarListaDinamica(lista: HTMLInputElement): void {
        MapearAlControl.ListaDinamica(lista, 0, "");
        BorrarOpcionesListaDinamica(lista);
        BlanquearDependientes(lista);
    }

    export function BorrarOpcionesListaDinamica(lista: HTMLInputElement) {
        let idDatos = lista.getAttribute(atListas.idDeLaLista);
        if (!IsNullOrEmpty(idDatos)) {
            var opciones: HTMLDataListElement = document.getElementById(idDatos) as HTMLDataListElement;
            //var numChilds = opciones.children.length;
            //for (var i = 0; i < numChilds; i++) {
            //    opciones.children[i].remove();
            //}
            opciones.innerHTML = "";
        }
    }

    export function BlanquearDependientes(control: HTMLInputElement) {
        let BlanquearControlDePropiedad: string = control.getAttribute(atListasDinamicas.BlanquearControlAsociado);
        if (!IsNullOrEmpty(BlanquearControlDePropiedad)) {
            let contenedor: string = control.getAttribute(atListasDinamicas.ContenidoEn);
            let divContenedor: HTMLDivElement = document.getElementById(contenedor) as HTMLDivElement;
            let controlDependiente: HTMLInputElement = divContenedor.querySelector(`[${atControl.propiedad}=${BlanquearControlDePropiedad}]`);
            let tipo: string = controlDependiente.getAttribute(atControl.tipo);
            if (tipo === TipoControl.restrictorDeEdicion)
                LimpiarEditor(controlDependiente);

            else if (tipo === TipoControl.ListaDinamica) {
                LimpiarListaDinamica(controlDependiente);
            }
        }
    }


    export function BlanquearEditor(editor: HTMLInputElement): void {
        AnularError(editor);
        editor.value = "";
    }

    export function AnularError(control: HTMLInputElement): void {
        control.classList.remove(ClaseCss.crtlNoValido);
        control.classList.add(ClaseCss.crtlValido);
    }

    export function MarcarError(control: HTMLInputElement): void {
        control.classList.add(ClaseCss.crtlNoValido);
        control.classList.remove(ClaseCss.crtlValido);
    }

    export function BlanquearListaDeElemento(selector: HTMLSelectElement): void {
        selector.classList.remove(ClaseCss.crtlNoValido);
        selector.classList.add(ClaseCss.crtlValido);
        selector.selectedIndex = 0;
    }

    export function LeerEntreFechas(controlDeFechaDesde: HTMLInputElement): string {
        let idHora = controlDeFechaDesde.getAttribute(atEntreFechas.horaDesde);
        let entreFechas: string = LeerFechaHora(controlDeFechaDesde, idHora);
        let idFechaHasta = controlDeFechaDesde.getAttribute(atEntreFechas.fechaHasta);
        let fechaHasta: HTMLInputElement = document.getElementById(idFechaHasta) as HTMLInputElement;
        idHora = controlDeFechaDesde.getAttribute(atEntreFechas.horaHasta);
        entreFechas = entreFechas + '-' + LeerFechaHora(fechaHasta, idHora);
        return entreFechas;
    }

    function LeerFechaHora(controlDeFecha: HTMLInputElement, idHora: string): string {
        let valorDeFecha: string = controlDeFecha.value;
        let resultado: string = "";
        if (!IsNullOrEmpty(valorDeFecha)) {
            let fecha: Date = new Date(valorDeFecha);
            resultado = fecha.toLocaleDateString();
            let controlDeHora: HTMLInputElement = document.getElementById(idHora) as HTMLInputElement;
            let valorDeHora: string = controlDeHora.value;
            if (!IsNullOrEmpty(valorDeHora)) {
                resultado = resultado + ' ' + valorDeHora;
            }
        }

        return resultado;
    }

    export function MapearComoOrdenar(columna: HTMLTableHeaderCellElement, orden: Tipos.Orden) {
        columna.setAttribute(atControl.ordenarPor, orden.OrdenarPor);
        columna.setAttribute(atControl.modoOrdenacion, orden.Modo);
    }

}

namespace ApiCrud {

    export function CrearEnlaceAlElemento(divDeElementos: HTMLDivElement, elemento: Elemento) {
        let a = document.createElement("a");
        let url: string = `${window.location}`;
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

    export function MapearControlesDesdeLaIuAlJson(crud: Crud.CrudBase, panel: HTMLDivElement, modoDeTrabajo: string): JSON {

        let elementoJson: JSON = crud.AntesDeMapearDatosDeIU(crud, panel, modoDeTrabajo);
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

    export function BlanquearControlesDeIU(panel: HTMLDivElement) {
        BlanquearEditores(panel);
        BlanquearListaDeElementos(panel);
        BlanquearArchivos(panel);
    }

    export function MostrarPanel(panel: HTMLDivElement) {
        panel.classList.remove(ClaseCss.divNoVisible);
        panel.classList.add(ClaseCss.divVisible);
    }

    export function OcultarPanel(panel: HTMLDivElement) {
        panel.classList.add(ClaseCss.divNoVisible);
        panel.classList.remove(ClaseCss.divVisible);
    }

    export function CerrarModalPorId(id: string) {
        let modal: HTMLDivElement = document.getElementById(id) as HTMLDivElement;
        if (NoDefinida(modal))
            throw new Error(`La modal ${id} no está definida`);
        CerrarModal(modal);
    }

    export function OcultarModalPorId(id: string) {
        let modal: HTMLDivElement = document.getElementById(id) as HTMLDivElement;
        if (NoDefinida(modal))
            throw new Error(`La modal ${id} no está definida`);
        OcultarModal(modal);
    }

    export function CerrarModal(modal: HTMLDivElement) {
        BlanquearSelectoresDeElemento(modal);
        OcultarModal(modal);
    }

    export function OcultarModal(modal: HTMLDivElement) {
        modal.style.display = "none";
    }

    export function AbrirModalPorId(id: string) {
        let modal: HTMLDivElement = document.getElementById(id) as HTMLDivElement;
        if (NoDefinida(modal))
            throw new Error(`La modal ${id} no está definida`);
        AbriModal(modal);
    }

    export function AbriModal(modal: HTMLDivElement) {
        modal.style.display = 'block';
    }

    export function QuitarClaseDeCtrlNoValido(panel: HTMLDivElement) {
        let crtls: HTMLCollectionOf<HTMLElement> = panel.getElementsByClassName(ClaseCss.crtlNoValido) as HTMLCollectionOf<HTMLElement>;
        for (let i = 0; i < crtls.length; i++) {
            crtls[i].classList.remove(ClaseCss.crtlNoValido);
        }

    }

    export function ActivarOpciones(opciones: NodeListOf<HTMLButtonElement>, activas: string[], seleccionadas: number): void {
        for (var i = 0; i < opciones.length; i++) {
            let opcion: HTMLButtonElement = opciones[i];
            if (ApiControl.EstaBloqueada(opcion))
                continue;

            let literal: string = opcion.value.toLowerCase();
            if (activas.indexOf(literal) >= 0) {

                let permiteMultiSeleccion: string = opcion.getAttribute(atOpcionDeMenu.permiteMultiSeleccion);
                if (!EsTrue(permiteMultiSeleccion))
                    opcion.disabled = !(seleccionadas === 1);

                if (EsTrue(permiteMultiSeleccion)) {
                    let numero: Number = Numero(opcion.getAttribute(atOpcionDeMenu.numeroMaximoSeleccionable));
                    if (numero === -1 || seleccionadas <= numero)
                        opcion.disabled = false;
                }
            }
        }
    }

    export function DesactivarOpciones(opciones: NodeListOf<HTMLButtonElement>, desactivas: string[]): void {
        for (var i = 0; i < opciones.length; i++) {
            let opcion: HTMLButtonElement = opciones[i];
            if (ApiControl.EstaBloqueada(opcion))
                continue;

            let literal: string = opcion.value.toLowerCase();
            if (desactivas.indexOf(literal) >= 0)
                opcion.disabled = true;
        }
    }

    export function DesactivarConMultiSeleccion(opciones: NodeListOf<HTMLButtonElement>, seleccionadas: number): void {
        for (var i = 0; i < opciones.length; i++) {
            let opcion: HTMLButtonElement = opciones[i];
            if (ApiControl.EstaBloqueada(opcion))
                continue;

            if (!opcion.disabled) {
                let permiteMultiSeleccion: string = opcion.getAttribute(atOpcionDeMenu.permiteMultiSeleccion);
                if (!EsTrue(permiteMultiSeleccion)) {
                    opcion.disabled = !(seleccionadas === 1);
                    return;
                }

                let numero: Number = Numero(opcion.getAttribute(atOpcionDeMenu.numeroMaximoSeleccionable));
                if (numero !== -1)
                    opcion.disabled = (seleccionadas > numero);
            }

        }
    }

    export function CambiarLiteralOpcion(opciones: NodeListOf<HTMLButtonElement>, antiguo: string, nuevo: string): void {
        for (var i = 0; i < opciones.length; i++) {
            let opcion: HTMLButtonElement = opciones[i];
            if (ApiControl.EstaBloqueada(opcion))
                continue;

            let literal: string = opcion.value.toLowerCase();
            if (literal.toLowerCase() === antiguo)
                opcion.value = nuevo;
        }
    }

    export function ObtenerSelector(idSelector: string): HTMLDivElement {
        let selector: HTMLDivElement = document.getElementById(idSelector) as HTMLDivElement;
        if (NoDefinida(selector))
            throw new Error(`el selector ${idSelector} no está definido`);
        return selector;
    }

    export function ObtenerEditorAsociadoAlSelector(selector: HTMLDivElement): HTMLInputElement {
        let idEditor = selector.getAttribute(atSelectorDeElementos.EditorAsociado);
        let editor: HTMLInputElement = document.getElementById(idEditor) as HTMLInputElement;
        if (NoDefinida(editor))
            throw new Error(`el editor ${idEditor} no está definido en el selector ${selector.id}`);
        return editor;
    }

    function BlanquearEditores(panel: HTMLDivElement) {
        let editores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.Editor}"]`) as NodeListOf<HTMLInputElement>;
        for (let i = 0; i < editores.length; i++) {
            ApiControl.BlanquearEditor(editores[i]);
        }
    }

    function BlanquearListaDeElementos(panel: HTMLDivElement) {
        let selectores: NodeListOf<HTMLSelectElement> = panel.querySelectorAll(`select[${atControl.tipo}="${TipoControl.ListaDeElementos}"]`) as NodeListOf<HTMLSelectElement>;
        for (let i = 0; i < selectores.length; i++) {
            ApiControl.BlanquearListaDeElemento(selectores[i]);
        }
    }

    function BlanquearArchivos(panel: HTMLDivElement) {
        let archivos: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`${atControl.tipo}[tipo="${TipoControl.Archivo}"]`) as NodeListOf<HTMLInputElement>;
        for (let i = 0; i < archivos.length; i++) {
            ApiDeArchivos.BlanquearArchivo(archivos[i], true);
        }
    }

    function BlanquearSelectoresDeElemento(modal: HTMLDivElement) {
        let selectores: NodeListOf<HTMLInputElement> = modal.querySelectorAll(`[${atControl.tipo}=${TipoControl.SelectorDeElementos}]`) as NodeListOf<HTMLInputElement>;
        for (let i: number = 0; i < selectores.length; i++) {
            selectores[i].setAttribute(atSelectorDeElementos.Seleccionados, '');
            let idEditor = selectores[i].getAttribute(atSelectorDeElementos.EditorAsociado);
            let editor: HTMLInputElement = document.getElementById(idEditor) as HTMLInputElement;
            ApiControl.BlanquearEditor(editor);
        }
    }

    export function EliminarReferenciasDeUnDiv(modal: HTMLDivElement) {
        let referencias: NodeListOf<HTMLElement> = modal.querySelectorAll("a") as NodeListOf<HTMLElement>;
        for (let i: number = 0; i < referencias.length; i++) {
            referencias[i].remove();
        }
    }
}

namespace ApiRuote {

    export function NavegarARelacionar(crud: Crud.GridDeDatos, idOpcionDeMenu: string, idSeleccionado: number, filtroRestrictor: Tipos.DatosRestrictor) {

        let filtroJson: string = ApiFiltro.DefinirRestrictorNumerico(filtroRestrictor.Propiedad, filtroRestrictor.Valor);

        let form: HTMLFormElement = document.getElementById(idOpcionDeMenu) as HTMLFormElement;

        if (form === null) {
            throw new Error(`La opción de menú '${idOpcionDeMenu}' está mal definida, actualice el descriptor`);
        }

        let navegarAlCrud: string = form.getAttribute(atNavegar.navegarAlCrud);
        let idRestrictor: string = form.getAttribute(atNavegar.idRestrictor) as string;
        let idOrden: string = form.getAttribute(atNavegar.orden) as string;

        let restrictor: HTMLInputElement = document.getElementById(idRestrictor) as HTMLInputElement;
        restrictor.value = filtroJson;
        let ordenInput: HTMLInputElement = document.getElementById(idOrden) as HTMLInputElement;
        ordenInput.value = "";

        let valores: Diccionario<any> = new Diccionario<any>();
        let filtros: Tipos.DatosRestrictor[] = [];
        filtros.push(filtroRestrictor);
        valores.Agregar(Sesion.paginaDestino, navegarAlCrud);
        valores.Agregar(Sesion.restrictores, filtros);
        valores.Agregar(Sesion.idSeleccionado, idSeleccionado);
        Navegar(crud, form, valores);
    }


    export function NavegarADependientes(crud: Crud.GridDeDatos, idOpcionDeMenu: string, idSeleccionado: number, filtroRestrictor: Array<Tipos.DatosRestrictor>) {

        let form: HTMLFormElement = document.getElementById(idOpcionDeMenu) as HTMLFormElement;

        if (form === null)
            throw new Error(`La opción de menú '${idOpcionDeMenu}' está mal definida, actualice el descriptor`);

        let navegarAlCrud: string = form.getAttribute(atNavegar.navegarAlCrud);
        let soloMapearEnELFiltro: boolean = EsTrue(form.getAttribute(atNavegar.soloMapearEnElFiltro));

        let valores: Diccionario<any> = new Diccionario<any>();
        valores.Agregar(Sesion.paginaDestino, navegarAlCrud);
        valores.Agregar(Sesion.restrictores, filtroRestrictor);
        valores.Agregar(Sesion.idSeleccionado, idSeleccionado);
        valores.Agregar(Sesion.SoloMapearEnElFiltro, soloMapearEnELFiltro);
        Navegar(crud, form, valores);
    }



    function Navegar(crud: Crud.GridDeDatos, form: HTMLFormElement, valores: Diccionario<any>) {
        crud.AntesDeNavegar(valores);
        EntornoSe.Historial.GuardarEstadoDePagina(crud.Estado);
        EntornoSe.Sumit(form);
    }
};

namespace ApiFiltro {

    export function DefinirFiltroPorId(id: number): string {
        return ApiFiltro.DefinirRestrictorNumerico(literal.filtro.clausulaId, id);
    }

    export function DefinirRestrictorNumerico(propiedad: string, valor: number): string {
        var clausulas = new Array<ClausulaDeFiltrado>();
        var clausula: ClausulaDeFiltrado = new ClausulaDeFiltrado(propiedad, literal.filtro.criterio.igual, `${valor}`);
        clausulas.push(clausula);
        return JSON.stringify(clausulas);
    }


    export function DefinirFiltroListaDinamica(input: HTMLInputElement, criterio: string): Array<ClausulaDeFiltrado> {
        let buscarPor: string = input.getAttribute(atListasDinamicas.buscarPor);
        let longitud: number = Numero(input.getAttribute(atListasDinamicas.longitudNecesaria));
        let valor: string = input.value;

        if (longitud == 0)
            longitud = 3;

        if (valor.length < longitud)
            return null;


        let filtros: Array<ClausulaDeFiltrado> = AnadirRestrictores(input);

        let clausula: ClausulaDeFiltrado = new ClausulaDeFiltrado(buscarPor, criterio, valor.toString());
        filtros.push(clausula);
        return filtros;
    }

    export function AnadirRestrictores(input: HTMLInputElement): Array<ClausulaDeFiltrado> {

        var filtros = new Array<ClausulaDeFiltrado>();
        let restringirPor: string = input.getAttribute(atListasDinamicas.RestringidoPor);
        if (Definida(restringirPor)) {
            let contenedor: string = input.getAttribute(atListasDinamicas.ContenidoEn);
            if (NoDefinida(contenedor))
                MensajesSe.EmitirExcepcion("Definir filtro lista dinámica", `No se puede definir el filtro para la propiedad ${input.id} ya que no se ha definido el atributo ${atListasDinamicas.ContenidoEn}`);
            let divControl: HTMLDivElement = document.getElementById(contenedor) as HTMLDivElement;
            let restrictor: HTMLElement = divControl.querySelector(`[${atControl.propiedad}=${restringirPor}]`);
            if (NoDefinida(restrictor))
                MensajesSe.EmitirExcepcion("Definir filtro lista dinámica", `No se  ha encontratado el control con la propiedad ${restringirPor} asociado a la lista ${input.id} en el contenedor ${contenedor}`);


            if (restrictor instanceof HTMLInputElement) {
                let tipo: string = restrictor.getAttribute(atControl.tipo);
                if (tipo === TipoControl.restrictorDeEdicion)
                    filtros.push(ObtenerValorDelRestrictorDeEdicion(restrictor, restringirPor));

                else if (tipo === TipoControl.ListaDinamica) {
                    let propiedadRestrictora: string = input.getAttribute(atListasDinamicas.PropiedadRestrictora);
                    let valorRestrictor: string = restrictor.getAttribute(atListasDinamicas.idSeleccionado);
                    let a: ClausulaDeFiltrado = ObtenerValorDeLaListaDinamica(restrictor, propiedadRestrictora, Numero(valorRestrictor));
                    if (Definida(a))
                        filtros.push(a);
                }

                else
                    MensajesSe.EmitirExcepcion("Definir filtro lista dinámica", `No se  ha definido como obtener el valor que restringir en el control ${restringirPor} asociado a la lista ${input.id} en el contenedor ${contenedor}`);
            }
        }
        return filtros;
    }

    function ObtenerValorDelRestrictorDeEdicion(restrictor: HTMLInputElement, restringirPor: string): ClausulaDeFiltrado {
        let valorRestrictor: string = (restrictor as HTMLInputElement).getAttribute(atControl.restrictor);

        if (Numero(valorRestrictor) === 0)
            MensajesSe.EmitirExcepcion("Definir filtro lista dinámica", `No se  ha definido el valor por el que restringir en el control ${restringirPor}`);

        return new ClausulaDeFiltrado(restringirPor, atCriterio.igual, valorRestrictor);

    }

    function ObtenerValorDeLaListaDinamica(lista: HTMLInputElement, propiedadRestrictora: string, valorRestrictor: number): ClausulaDeFiltrado {
        if (IsNullOrEmpty(propiedadRestrictora))
            MensajesSe.EmitirExcepcion("Obtener filtro de la lista dinámica", `no se ha definido la propiedad restrictora en el control ${lista.id}`);

        if (Number(valorRestrictor) === 0)
            return null;

        return new ClausulaDeFiltrado(propiedadRestrictora, atCriterio.igual, valorRestrictor.toString());

    }
}
