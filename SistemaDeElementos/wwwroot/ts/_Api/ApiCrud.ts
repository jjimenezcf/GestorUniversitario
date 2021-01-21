namespace ApiControl {

    export function BlanquearEditor(editor: HTMLInputElement): void {
        editor.classList.remove(ClaseCss.crtlNoValido);
        editor.classList.add(ClaseCss.crtlValido);
        editor.value = "";
    }

    export function BlanquearSelector(selector: HTMLSelectElement) {
        selector.classList.remove(ClaseCss.crtlNoValido);
        selector.classList.add(ClaseCss.crtlValido);
        selector.selectedIndex = 0;
    }
}

namespace ApiCrud {

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
            ApiDeArchivos.BlanquearArchivo(archivos[i]);
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