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
            opcion.disabled = true;
            opcion.setAttribute(atOpcionDeMenu.bloqueada, "S");
        }
    }

    export function EstaBloqueada(opcion: HTMLButtonElement) { return opcion.getAttribute(atOpcionDeMenu.bloqueada) === "S"; }

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

}

namespace ApiCrud {

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
        //modal.setAttribute('altura-inicial', "0");
        //modal.setAttribute('ratio-inicial', "0");
        //var body = document.getElementsByTagName("body")[0];
        //body.style.position = "inherit";
        //body.style.height = "auto";
        //body.style.overflow = "visible";
    }

    export function QuitarClaseDeCtrlNoValido(panel: HTMLDivElement) {
        let crtls: HTMLCollectionOf<HTMLElement> = panel.getElementsByClassName(ClaseCss.crtlNoValido) as HTMLCollectionOf<HTMLElement>;
        for (let i = 0; i < crtls.length; i++) {
            crtls[i].classList.remove(ClaseCss.crtlNoValido);
        }

    }

    export function AplicarModoDeAccesoAlNegocio(opcionesGenerales: NodeListOf<HTMLButtonElement>, modoDeAccesoDelUsuario: string): void {
        for (var i = 0; i < opcionesGenerales.length; i++) {
            let opcion: HTMLButtonElement = opcionesGenerales[i];

            if (ApiControl.EstaBloqueada(opcion))
                continue;

            let permisosNecesarios: string = opcion.getAttribute(atOpcionDeMenu.permisosNecesarios);
            opcion.disabled = !ModoAcceso.HayPermisos(permisosNecesarios, modoDeAccesoDelUsuario);
        }
    }

    export function AplicarModoAccesoAlElemento(opcion: HTMLButtonElement, hacerLaInterseccion: boolean, modoAccesoDelUsuarioAlElemento) {
        if (ApiControl.EstaBloqueada(opcion))
            return;

        let estaDeshabilitado = opcion.disabled;
        let permisosNecesarios: string = opcion.getAttribute(atOpcionDeMenu.permisosNecesarios);
        let permiteMultiSeleccion: string = opcion.getAttribute(atOpcionDeMenu.permiteMultiSeleccion);
        if (!EsTrue(permiteMultiSeleccion) && hacerLaInterseccion) {
            opcion.disabled = true;
            return;
        }

        if (!ModoAcceso.HayPermisos(permisosNecesarios, modoAccesoDelUsuarioAlElemento))
            opcion.disabled = true;
        else
            opcion.disabled = (estaDeshabilitado && hacerLaInterseccion) || false;
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
                else {
                    if (seleccionadas === 1)
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

            let permiteMultiSeleccion: string = opcion.getAttribute(atOpcionDeMenu.permiteMultiSeleccion);
            if (!EsTrue(permiteMultiSeleccion) && !opcion.disabled)
                opcion.disabled = !(seleccionadas === 1);
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

        let navegarAlCrud: string = form.getAttribute(atNavegar.navegarAlCrud);
        let idRestrictor: string = form.getAttribute(atNavegar.idRestrictor) as string;
        let idOrden: string = form.getAttribute(atNavegar.orden) as string;

        let restrictor: HTMLInputElement = document.getElementById(idRestrictor) as HTMLInputElement;
        restrictor.value = filtroJson;
        let ordenInput: HTMLInputElement = document.getElementById(idOrden) as HTMLInputElement;
        ordenInput.value = "";

        let valores: Diccionario<any> = new Diccionario<any>();
        valores.Agregar(Sesion.paginaDestino, navegarAlCrud);
        valores.Agregar(Sesion.restrictor, filtroRestrictor);
        valores.Agregar(Sesion.idSeleccionado, idSeleccionado);
        Navegar(crud, form, valores);
    }


    export function NavegarADependientes(crud: Crud.GridDeDatos, idOpcionDeMenu: string, idSeleccionado: number, filtroRestrictor: Tipos.DatosRestrictor) {

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
        valores.Agregar(Sesion.paginaDestino, navegarAlCrud);
        valores.Agregar(Sesion.restrictor, filtroRestrictor);
        valores.Agregar(Sesion.idSeleccionado, idSeleccionado);
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
