namespace ApiControl {

    export function BloquearMenu(panel: HTMLDivElement): void {
        let opciones: NodeListOf<HTMLButtonElement> = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.opcion}"]`) as NodeListOf<HTMLButtonElement>;
        for (var i = 0; i < opciones.length; i++) {
            let opcion: HTMLButtonElement = opciones[i];
            opcion.disabled = true;
            opcion.setAttribute(atOpcionDeMenu.bloqueada, "S");
        }
    }

    export function EstaBloqueada(opcion: HTMLButtonElement) { return opcion.getAttribute(atOpcionDeMenu.bloqueada) === "S"; }

    export function MapearFechaAlControl(control: HTMLInputElement, fecha: string) {
        var fechaLeida = new Date(fecha);
        if (FechaValida(fechaLeida)) {
            let dia: number = fechaLeida.getDate();
            let mes: number = fechaLeida.getMonth() + 1;
            let ano: number = fechaLeida.getFullYear();
            control.value = `${ano}-${PadLeft(mes.toString(), "00")}-${PadLeft(dia.toString(), "00")}`;
        }
        else {
            var propiedad: string = control.getAttribute(atControl.propiedad);
            Mensaje(TipoMensaje.Error, `Fecha leida para la propiedad ${propiedad} es no válida, valor ${fecha}`);
        }
    }

    export function MapearTextoAlControl(area: HTMLTextAreaElement, texto: string): void {
        area.textContent = texto;
    }

    export function MapearPropiedadRestrictoraAlFiltro(panel: HTMLDivElement, propiedadRestrictora: string, id: number, texto: string) {
        let restrictores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.restrictorDeFiltro}"]`) as NodeListOf<HTMLInputElement>;

        for (let i = 0; i < restrictores.length; i++) {
            if (restrictores[i].getAttribute(atControl.propiedad) === propiedadRestrictora) {
                ApiControl.MapearRestrictorAlControl(restrictores[i], id, texto);
            }
        }
    }

    export function MapearPropiedadRestrictoraAlControl(panel: HTMLDivElement, propiedadRestrictora: string, id: number, texto: string) {
        let restrictores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.restrictorDeEdicion}"]`) as NodeListOf<HTMLInputElement>;

        for (let i = 0; i < restrictores.length; i++) {
            if (restrictores[i].getAttribute(atControl.propiedad) === propiedadRestrictora) {
                ApiControl.MapearRestrictorAlControl(restrictores[i], id, texto);
            }
        }
    }

    export function MapearRestrictorAlControl(restrictor: HTMLInputElement, id: number, texto: string): void {
        restrictor.setAttribute(atControl.valorInput, texto);
        restrictor.setAttribute(atControl.restrictor, id.toString());
    }

    export function MapearHoraAlControl(control: HTMLInputElement, fechaHora: string) {
        var fechaLeida = new Date(fechaHora);
        if (FechaValida(fechaLeida)) {
            let hora: number = fechaLeida.getHours();
            let minuto: number = fechaLeida.getMinutes();
            let segundos: number = fechaLeida.getSeconds();
            let milisegundos: number = fechaLeida.getMilliseconds();
            let idHora: string = control.getAttribute(atSelectorDeFecha.hora);
            if (!IsNullOrEmpty(idHora)) {
                let controlHora: HTMLInputElement = document.getElementById(idHora) as HTMLInputElement;
                controlHora.value = `${PadLeft(hora.toString(), "00")}:${PadLeft(minuto.toString(), "00")}:${PadLeft(segundos.toString(), "00")}`;
                controlHora.setAttribute(atSelectorDeFecha.milisegundos, milisegundos.toString());
                return;
            }
        }
        var propiedad: string = control.getAttribute(atControl.propiedad);
        Mensaje(TipoMensaje.Error, `Fecha leida para la propiedad ${propiedad} es no válida, valor ${fechaHora}`);

    }
    export function AjustarColumnaDelGrid(columanDeOrdenacion: Tipos.Orden) {
        let columna: HTMLTableHeaderCellElement = document.getElementById(columanDeOrdenacion.IdColumna) as HTMLTableHeaderCellElement;
        columna.setAttribute(atControl.modoOrdenacion, columanDeOrdenacion.Modo);
        let a: HTMLElement = columna.getElementsByTagName('a')[0] as HTMLElement;
        a.setAttribute("class", columanDeOrdenacion.ccsClase);
    }

    export function BlanquearEditor(editor: HTMLInputElement): void {
        editor.classList.remove(ClaseCss.crtlNoValido);
        editor.classList.add(ClaseCss.crtlValido);
        editor.value = "";
    }

    export function BlanquearSelector(selector: HTMLSelectElement): void {
        selector.classList.remove(ClaseCss.crtlNoValido);
        selector.classList.add(ClaseCss.crtlValido);
        selector.selectedIndex = 0;
    }

    export function MapearListasDinamicasAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
        let ListaDinamica: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.ListaDinamica}"]`) as NodeListOf<HTMLInputElement>;
        for (let i = 0; i < ListaDinamica.length; i++) {
            MapearListaDinamicaAlJson(ListaDinamica[i], elementoJson);
        }
    }

    export function AlmacenarValorDeListaDinamica(input: HTMLInputElement, valor: number) {
        input.setAttribute(atListasDinamicas.idSeleccionado, Numero(valor).toString());
        if (Numero(valor) === 0)
            input.value = "";
    }

    function MapearListaDinamicaAlJson(input: HTMLInputElement, elementoJson: JSON): void {
        let propiedadDto = input.getAttribute(atControl.propiedad);
        let guardarEn: string = input.getAttribute(atListasDinamicasDto.guardarEn);
        let obligatorio: string = input.getAttribute(atControl.obligatorio);
        let valor: number = Numero(input.getAttribute(atListasDinamicas.idSeleccionado));

        if (obligatorio === "S" && Number(valor) === 0) {
            input.classList.remove(ClaseCss.crtlValido);
            input.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`Debe seleccionar un elemento de la lista ${propiedadDto}`);
        }

        input.classList.remove(ClaseCss.crtlNoValido);
        input.classList.add(ClaseCss.crtlValido);
        elementoJson[guardarEn] = valor === 0 ? '' : valor.toString();
    }

    export function MapearListasDeElementosAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
        let selectores: NodeListOf<HTMLSelectElement> = panel.querySelectorAll(`select[tipo="${TipoControl.ListaDeElementos}"]`) as NodeListOf<HTMLSelectElement>;
        for (let i = 0; i < selectores.length; i++) {
            MapearSelectorDeElementosAlJson(selectores[i], elementoJson);
        }
    }

    function MapearSelectorDeElementosAlJson(selector: HTMLSelectElement, elementoJson: JSON) {
        let propiedadDto = selector.getAttribute(atControl.propiedad);
        let guardarEn: string = selector.getAttribute(atListasDinamicasDto.guardarEn);
        let obligatorio: string = selector.getAttribute(atControl.obligatorio);

        if (obligatorio === "S" && Number(selector.value) === 0) {
            selector.classList.remove(ClaseCss.crtlValido);
            selector.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`Debe seleccionar un elemento de la lista ${propiedadDto}`);
        }

        selector.classList.remove(ClaseCss.crtlNoValido);
        selector.classList.add(ClaseCss.crtlValido);
        elementoJson[guardarEn] = selector.value;
    }

    export function MapearRestrictoresAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
        let restrictores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.restrictorDeEdicion}"]`) as NodeListOf<HTMLInputElement>;
        for (let i = 0; i < restrictores.length; i++) {
            MapearRestrictorAlJson(restrictores[i], elementoJson);
        }
    }

    function MapearRestrictorAlJson(input: HTMLInputElement, elementoJson: JSON): void {
        let propiedadDto: string = input.getAttribute(atControl.propiedad);
        let idRestrictor: string = input.getAttribute(atControl.restrictor);

        if (!NumeroMayorDeCero(idRestrictor)) {
            input.classList.remove(ClaseCss.crtlValido);
            input.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`El campo ${propiedadDto} es obligatorio`);
        }

        input.classList.remove(ClaseCss.crtlNoValido);
        input.classList.add(ClaseCss.crtlValido);
        elementoJson[propiedadDto] = idRestrictor;
    }

    export function MapearFechasAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
        let fechas: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.SelectorDeFecha}"]`) as NodeListOf<HTMLInputElement>;
        for (var i = 0; i < fechas.length; i++) {
            let fecha: HTMLInputElement = fechas[i] as HTMLInputElement;
            MapearFechaAlJson(fecha, elementoJson);
        }

        let fechasHoras: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.SelectorDeFechaHora}"]`) as NodeListOf<HTMLInputElement>;
        for (var i = 0; i < fechasHoras.length; i++) {
            let fecha: HTMLInputElement = fechasHoras[i] as HTMLInputElement;
            MapearFechaAlJson(fecha, elementoJson);
        }
    }

    function MapearFechaAlJson(controlDeFecha: HTMLInputElement, elementoJson: JSON): void {
        let propiedadDto: string = controlDeFecha.getAttribute(atControl.propiedad);
        let obligatorio: string = controlDeFecha.getAttribute(atControl.obligatorio);
        let valorDeFecha: string = controlDeFecha.value; //.replace(/\n/g, "\r\n");
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

        let fecha: Date = new Date(valorDeFecha);
        if (FechaValida(fecha)) {
            let idHora = controlDeFecha.getAttribute(atSelectorDeFecha.hora);
            if (!IsNullOrEmpty(idHora)) {
                let controlDeHora: HTMLInputElement = document.getElementById(idHora) as HTMLInputElement;

                if (!fechaHoraFijada) {
                    let valorDeHora = controlDeHora.value.split(':');
                    let hora: number = Numero(valorDeHora[0]);
                    let minuto: number = Numero(valorDeHora[1]);
                    let segundos: number = Numero(valorDeHora[2]);
                    let milisegundos: number = Numero(controlDeHora.getAttribute(atSelectorDeFecha.milisegundos));
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

    export function MapearTextosAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
        let areas: NodeListOf<HTMLTextAreaElement> = panel.querySelectorAll(`textarea[tipo="${TipoControl.AreaDeTexto}"]`) as NodeListOf<HTMLTextAreaElement>;
        for (let i = 0; i < areas.length; i++) {
            MapearTextoAlJson(areas[i], elementoJson);
        }
    }

    function MapearTextoAlJson(area: HTMLTextAreaElement, elementoJson: JSON): void {
        let propiedadDto: string = area.getAttribute(atControl.propiedad);
        let obligatorio: string = area.getAttribute(atControl.obligatorio);
        let valor: string = area.value; //.replace(/\n/g, "\r\n");
        if (obligatorio === "S" && NoDefinida(valor)) {
            area.classList.remove(ClaseCss.crtlValido);
            area.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`El campo ${propiedadDto} es obligatorio`);
        }
        elementoJson[propiedadDto] = valor;
    }

    export function MapearEditoresAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
        let editores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.Editor}"]`) as NodeListOf<HTMLInputElement>;
        for (let i = 0; i < editores.length; i++) {
            MapearEditorAlJson(editores[i], elementoJson);
        }
    }

    function MapearEditorAlJson(input: HTMLInputElement, elementoJson: JSON): void {
        var propiedadDto = input.getAttribute(atControl.propiedad);
        let valor: string = (input as HTMLInputElement).value;
        let obligatorio: string = input.getAttribute(atControl.obligatorio);

        if (obligatorio === "S" && NoDefinida(valor)) {
            input.classList.remove(ClaseCss.crtlValido);
            input.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`El campo ${propiedadDto} es obligatorio`);
        }

        input.classList.remove(ClaseCss.crtlNoValido);
        input.classList.add(ClaseCss.crtlValido);
        elementoJson[propiedadDto] = valor;
    }

    export function MapearArchivosAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
        let archivos: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.Archivo}"]`) as NodeListOf<HTMLInputElement>;
        for (let i = 0; i < archivos.length; i++) {
            MapearArchivoAlJson(archivos[i], elementoJson);
        }
    }

    function MapearArchivoAlJson(archivo: HTMLInputElement, elementoJson: JSON): void {
        var propiedadDto = archivo.getAttribute(atControl.propiedad);
        let valor: string = archivo.getAttribute(atArchivo.idArchivo);
        let obligatorio: string = archivo.getAttribute(atControl.obligatorio);

        if (obligatorio === "S" && IsNullOrEmpty(valor)) {
            archivo.classList.remove(ClaseCss.crtlValido);
            archivo.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`El campo ${propiedadDto} es obligatorio`);
        }

        archivo.classList.remove(ClaseCss.crtlNoValido);
        archivo.classList.add(ClaseCss.crtlValido);
        elementoJson[propiedadDto] = valor;
    }

    export function MapearUrlArchivosAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
        let urlsDeArchivos: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.UrlDeArchivo}"]`) as NodeListOf<HTMLInputElement>;
        for (let i = 0; i < urlsDeArchivos.length; i++) {
            MapearUrlArchivoAlJson(urlsDeArchivos[i], elementoJson);
        }
    }

    function MapearUrlArchivoAlJson(urlDeArchivo: HTMLInputElement, elementoJson: JSON): void {
        var propiedadDto = urlDeArchivo.getAttribute(atControl.propiedad);
        let valor: string = urlDeArchivo.getAttribute(atArchivo.nombre);
        let obligatorio: string = urlDeArchivo.getAttribute(atControl.obligatorio);

        if (obligatorio === "S" && IsNullOrEmpty(valor)) {
            urlDeArchivo.classList.remove(ClaseCss.crtlValido);
            urlDeArchivo.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`El campo ${propiedadDto} es obligatorio`);
        }

        urlDeArchivo.classList.remove(ClaseCss.crtlNoValido);
        urlDeArchivo.classList.add(ClaseCss.crtlValido);
        elementoJson[propiedadDto] = valor;
    }

    export function MapearCheckesAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
        let checkes: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.Check}"]`) as NodeListOf<HTMLInputElement>;
        for (let i = 0; i < checkes.length; i++) {
            MapearCheckAlJson(checkes[i], elementoJson);
        }
    }

    function MapearCheckAlJson(check: HTMLInputElement, elementoJson: JSON): void {
        var propiedadDto = check.getAttribute(atControl.propiedad);
        elementoJson[propiedadDto] = check.checked;
    }

}

namespace ApiCrud {

    export function MapearControlesDesdeLaIuAlJson(crud: Crud.CrudBase, panel: HTMLDivElement, modoDeTrabajo: string): JSON {

        let elementoJson: JSON = crud.AntesDeMapearDatosDeIU(crud, panel, modoDeTrabajo);
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

    export function BlanquearControlesDeIU(panel: HTMLDivElement) {
        BlanquearEditores(panel);
        BlanquearSelectores(panel);
        BlanquearArchivos(panel);
    }

    export function MostrarPanel(panel: HTMLDivElement) {
        panel.classList.remove(ClaseCss.divNoVisible);
    }

    export function OcultarPanel(panel: HTMLDivElement) {
        panel.classList.add(ClaseCss.divNoVisible);
        panel.classList.remove(ClaseCss.divVisible);
    }

    export function CerrarModal(modal: HTMLDivElement) {
        modal.style.display = "none";
        var body = document.getElementsByTagName("body")[0];
        body.style.position = "inherit";
        body.style.height = "auto";
        body.style.overflow = "visible";
    }

    export function QuitarClaseDeCtrlNoValido(panel: HTMLDivElement) {
        let crtls: HTMLCollectionOf<HTMLElement> = panel.getElementsByClassName(ClaseCss.crtlNoValido) as HTMLCollectionOf<HTMLElement>;
        for (let i = 0; i < crtls.length; i++) {
            crtls[i].classList.remove(ClaseCss.crtlNoValido);
        }

    }

    function BlanquearEditores(panel: HTMLDivElement) {
        let editores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.Editor}"]`) as NodeListOf<HTMLInputElement>;
        for (let i = 0; i < editores.length; i++) {
            ApiControl.BlanquearEditor(editores[i]);
        }
    }

    function BlanquearSelectores(panel: HTMLDivElement) {
        let selectores: NodeListOf<HTMLSelectElement> = panel.querySelectorAll(`select[${atControl.tipo}="${TipoControl.ListaDeElementos}"]`) as NodeListOf<HTMLSelectElement>;
        for (let i = 0; i < selectores.length; i++) {
            ApiControl.BlanquearSelector(selectores[i]);
        }
    }

    function BlanquearArchivos(panel: HTMLDivElement) {
        let archivos: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`${atControl.tipo}[tipo="${TipoControl.Archivo}"]`) as NodeListOf<HTMLInputElement>;
        for (let i = 0; i < archivos.length; i++) {
            ApiDeArchivos.BlanquearArchivo(archivos[i], true);
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

        let navegarAlCrud: string = form.getAttribute(atRelacion.navegarAlCrud);
        let idRestrictor: string = form.getAttribute(atRelacion.idRestrictor) as string;
        let idOrden: string = form.getAttribute(atRelacion.orden) as string;

        let restrictor: HTMLInputElement = document.getElementById(idRestrictor) as HTMLInputElement;
        restrictor.value = filtroJson;
        let ordenInput: HTMLInputElement = document.getElementById(idOrden) as HTMLInputElement;
        ordenInput.value = "";

        let valores: Diccionario<any> = new Diccionario<any>();
        valores.Agregar(Sesion.paginaDestino, navegarAlCrud);
        valores.Agregar(Sesion.restrictor, filtroRestrictor);
        valores.Agregar(Sesion.idSeleccionado, idSeleccionado);
        Navegar(crud, form, crud.Estado, valores);
    }



    function Navegar(crud: Crud.GridDeDatos, form: HTMLFormElement, estado: HistorialSe.EstadoPagina, valores: Diccionario<any>) {
        crud.AntesDeNavegar(valores);
        EntornoSe.Historial.GuardarEstadoDePagina(estado);
        EntornoSe.Historial.Persistir();
        PonerCapa();
        form.submit();
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


    export function DefinirFiltroListaDinamica(input: HTMLInputElement, criterio: string): ClausulaDeFiltrado {
        let buscarPor: string = input.getAttribute(atListasDinamicas.buscarPor);
        let longitud: number = Numero(input.getAttribute(atListasDinamicas.longitudNecesaria));
        let valor: string = input.value;

        if (longitud == 0)
            longitud = 3;

        if (valor.length < longitud)
            return null;

        let clausula: ClausulaDeFiltrado = new ClausulaDeFiltrado(buscarPor, criterio, valor.toString());
        return clausula;
    }
}
