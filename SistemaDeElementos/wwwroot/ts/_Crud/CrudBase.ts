namespace Crud {

    export class HTMLSelector extends HTMLInputElement {
    }

    export class ListaDeElemento {
        private lista: HTMLSelectElement;

        get Lista(): HTMLSelectElement {
            return this.lista;
        }

        constructor(idLista: string) {
            this.lista = document.getElementById(idLista) as HTMLSelectElement;
        }

        public AgregarOpcion(valor: number, texto: string): void {

            var opcion = document.createElement("option");
            opcion.setAttribute("value", valor.toString());
            opcion.setAttribute("label", texto);
            this.Lista.appendChild(opcion);
        }
    }

    export class DatosPeticionLista {
        ClaseDeElemento: string;
        IdLista: string;

        get Selector(): ListaDeElemento {
            return new ListaDeElemento(this.IdLista);
        }
    }

    export class ListaDinamica {
        private _IdLista: string;

        get Input(): HTMLInputElement {
            return document.querySelector(`input[list="${this._IdLista}"]`)
        }

        get Lista(): HTMLDataListElement {
            return document.getElementById(this._IdLista) as HTMLDataListElement;
        }

        constructor(input: HTMLInputElement) {           
            this._IdLista = input.getAttribute(AtributosDeListas.idDeLaLista);
        }

        public AgregarOpcion(valor: number, texto: string): void {

            for (var i = 0; i < this.Lista.children.length; i++)
                if (this.Lista.children[i].getAttribute(AtributosDeListas.identificador).Numero() === valor)
                    return;

            let opcion: HTMLOptionElement = document.createElement("option");
            opcion.setAttribute(AtributosDeListas.identificador, valor.toString());
            opcion.value = texto;

            this.Lista.appendChild(opcion);
        }

        public BuscarSeleccionado(valor: string): number {
            for (var i = 0; i < this.Lista.children.length; i++) {
                if (this.Lista.children[i] instanceof HTMLOptionElement) {
                    let opcion: HTMLOptionElement = this.Lista.children[i] as HTMLOptionElement;
                    if (opcion.value === valor)
                        return opcion.getAttribute(AtributosDeListas.identificador).Numero();
                }
            }
            return 0;
        }

        public Borrar(): void {
            this.Input.value = "";
            this.Lista.innerHTML = "";
        }

    }

    export class DatosPeticionDinamica {
        public ClaseDeElemento: string;
        public IdInput: string;
    }


    export class CrudBase {

        private modoTrabajo: string;
        protected get ModoTrabajo(): string {
            return this.modoTrabajo;
        }
        protected set ModoTrabajo(modo: string) {
            this.modoTrabajo = modo;
        }

        constructor() {
        }

        //funciones de ayuda para la herencia

        protected InicializarListasDeElementos(panel: HTMLDivElement, controlador: string) {
            let listas: NodeListOf<HTMLSelectElement> = panel.querySelectorAll(`select[${Atributo.tipo}="${TipoControl.ListaDeElementos}"]`) as NodeListOf<HTMLSelectElement>;
            for (let i = 0; i < listas.length; i++) {
                if (listas[i].getAttribute(AtributosDeListas.yaCargado) === "S")
                    continue;

                let claseElemento: string = listas[i].getAttribute(AtributosDeListas.claseElemento);
                this.CargarListaDeElementos(controlador, claseElemento, listas[i].getAttribute(Atributo.id));
            }
        }


        protected InicializarListasDinamicas(panel: HTMLDivElement) {
            let listas: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${Atributo.tipo}="${TipoControl.ListaDinamica}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < listas.length; i++) {
                let lista: ListaDinamica = new ListaDinamica(listas[i]);
                lista.Borrar();
            }
        }

        protected InicializarCanvases(panel: HTMLDivElement) {
            let canvases: NodeListOf<HTMLCanvasElement> = panel.querySelectorAll("canvas") as NodeListOf<HTMLCanvasElement>;
            canvases.forEach((canvas) => { canvas.width = canvas.width; });
        }

        protected Cerrar(panelMostrar: HTMLDivElement, panelCerrar: HTMLDivElement) {
            this.OcultarPanel(panelCerrar);
            this.MostrarPanel(panelMostrar);

            BlanquearMensaje();
        }

        protected BlanquearControlesDeIU(panel: HTMLDivElement) {
            this.BlanquearEditores(panel);
            this.BlanquearSelectores(panel);
            this.BlanquearArchivos(panel);
        }

        private BlanquearEditores(panel: HTMLDivElement) {
            let editores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${Atributo.tipo}="${TipoControl.Editor}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < editores.length; i++) {
                this.BlanquearEditor(editores[i]);
            }
        }

        private BlanquearEditor(editor: HTMLInputElement) {
            editor.classList.remove(ClaseCss.crtlNoValido);
            editor.classList.add(ClaseCss.crtlValido);
            editor.value = "";
        }


        private BlanquearSelectores(panel: HTMLDivElement) {
            let selectores: NodeListOf<HTMLSelectElement> = panel.querySelectorAll(`select[${Atributo.tipo}="${TipoControl.ListaDeElementos}"]`) as NodeListOf<HTMLSelectElement>;
            for (let i = 0; i < selectores.length; i++) {
                this.BlanquearSelector(selectores[i]);
            }
        }

        private BlanquearSelector(selector: HTMLSelectElement) {
            selector.classList.remove(ClaseCss.crtlNoValido);
            selector.classList.add(ClaseCss.crtlValido);
            selector.selectedIndex = 0;
        }


        private BlanquearArchivos(panel: HTMLDivElement) {
            let archivos: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`${Atributo.tipo}[tipo="${TipoControl.Archivo}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < archivos.length; i++) {
                ApiDeArchivos.BlanquearArchivo(archivos[i]);
            }
        }

        protected MostrarPanel(panel: HTMLDivElement) {
            panel.classList.add(ClaseCss.divVisible);
            panel.classList.remove(ClaseCss.divNoVisible);
        }

        protected OcultarPanel(panel: HTMLDivElement) {
            panel.classList.add(ClaseCss.divNoVisible);
            panel.classList.remove(ClaseCss.divVisible);
        }

        protected CerrarModal(idModal: string) {
            let modalBorrar: HTMLDivElement = document.getElementById(idModal) as HTMLDivElement;
            modalBorrar.style.display = "none";
            var body = document.getElementsByTagName("body")[0];
            body.style.position = "inherit";
            body.style.height = "auto";
            body.style.overflow = "visible";

        }

        protected SiHayErrorTrasPeticionAjax(peticion: ApiDeAjax.DescriptorAjax) {
            Mensaje(TipoMensaje.Error, peticion.resultado.mensaje);
        }

        // funciones para mapear un elemento Json a los controles de un panel

        protected MapearElementoLeido(panel: HTMLDivElement, elementoJson: JSON) {
            this.MapearPropiedadesDelElemento(panel, "elementoJson", elementoJson);
            this.MaperaOpcionesListasDinamicas(panel, elementoJson);
        }

        private MaperaOpcionesListasDinamicas(panel: HTMLDivElement, elementoJson: JSON) {

            let listas: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${Atributo.tipo}="${TipoControl.ListaDinamica}"]`) as NodeListOf<HTMLInputElement>;

            for (var i = 0; i < listas.length; i++) {
                let input: HTMLInputElement = listas[i] as HTMLInputElement;
                let propiedad: string = input.getAttribute(AtributosDeListas.guardarEn);
                let id: number = this.BuscarValorEnJson(propiedad, elementoJson) as number;

                if (id > 0) {
                    let listaDinamica = new ListaDinamica(input);
                    listaDinamica.AgregarOpcion(id, input.value);
                }
                else {
                    input.value = "";
                }

            }
        }

        private MapearPropiedadesDelElemento(panel: HTMLDivElement, propiedad: string, valorPropiedadJson: any) {
            var tipoDeObjeto = typeof valorPropiedadJson;
            if (tipoDeObjeto === "object") {
                for (var propiedad in valorPropiedadJson) {
                    this.MapearPropiedadesDelElemento(panel, propiedad, valorPropiedadJson[propiedad]);
                }
            } else {
                this.MapearPropiedad(panel, propiedad, valorPropiedadJson);
            }
        }

        private BuscarValorEnJson(propiedad: string, valorPropiedadJson: any) {
            var tipoDeObjeto = typeof valorPropiedadJson;
            if (tipoDeObjeto === "object") {
                for (var p in valorPropiedadJson) {
                    if (propiedad.toLowerCase() === p.toLowerCase())
                        return valorPropiedadJson[p];
                }
            }
            return null;
        }


        private MapearPropiedad(panel: HTMLDivElement, propiedad: string, valor: any) {
            this.MapearPropiedaAlEditor(panel, propiedad, valor);
            this.MapearPropiedadAlSelectorDeElemento(panel, propiedad, valor);
            this.MapearPropiedadEnLaListaDinamica(panel, propiedad, valor);
            this.MapearPropiedadAlSelectorDeArchivo(panel, propiedad, valor);
            this.MapearPropiedadAlVisorDeImagen(panel, propiedad, valor);

        }

        private MapearPropiedaAlEditor(panel: HTMLDivElement, propiedad: string, valor: any) {
            let editor: HTMLInputElement = this.BuscarEditor(panel, propiedad);

            if (editor === null)
                return;

            editor.value = valor;
        }

        private MapearPropiedadAlSelectorDeElemento(panel: HTMLDivElement, propiedad: string, valor: any) {
            let select: HTMLSelectElement = this.BuscarSelect(panel, propiedad);

            if (select === null)
                return;

            for (var i = 0; i < select.options.length; i++) {
                if (select.options[i].label === valor) {
                    select.selectedIndex = i;
                    break;
                }
            }
        }

        private MapearPropiedadEnLaListaDinamica(panel: HTMLDivElement, propiedad: string, valor: any) {
            let input: HTMLInputElement = this.BuscarListaDinamica(panel, propiedad);
            if (input === null)
                return;
            input.value = valor;
        }


        private MapearPropiedadAlSelectorDeArchivo(panel: HTMLDivElement, propiedad: string, valor: any) {
            let selector: HTMLInputElement = this.BuscarSelectorDeArchivo(panel, propiedad);

            if (selector === null)
                return;

            selector.setAttribute(AtributoSelectorArchivo.idArchivo, valor);
        }

        private MapearPropiedadAlVisorDeImagen(panel: HTMLDivElement, propiedad: string, valor: any) {

            let visor: HTMLImageElement = this.BuscarVisorDeImagen(panel, propiedad);

            if (visor === null)
                return;

            visor.setAttribute('src', valor);
            let idCanva: string = visor.getAttribute(Atributo.id).replace('img', 'canvas');

            let htmlCanvas: HTMLCanvasElement = document.getElementById(idCanva) as HTMLCanvasElement;
            htmlCanvas.width = 100;
            htmlCanvas.height = 100;
            var canvas = htmlCanvas.getContext('2d');


            var img = new Image();
            img.src = valor;
            img.onload = function () {
                canvas.drawImage(img, 0, 0, 100, 100);
            };

        }


        // funciones para la gestión de los mapeos de controles a un json  ****************************************************************************

        protected BuscarEditor(controlPadre: HTMLDivElement, propiedadDto: string): HTMLInputElement {
            let editores: NodeListOf<HTMLInputElement> = controlPadre.querySelectorAll(`input[${Atributo.tipo}="${TipoControl.Editor}"]`) as NodeListOf<HTMLInputElement>;
            for (var i = 0; i < editores.length; i++) {
                var control = editores[i] as HTMLInputElement;
                var dto = control.getAttribute(Atributo.propiedad);
                if (dto === propiedadDto.toLowerCase())
                    return control;
            }
            return null;
        }


        protected BuscarSelectorDeArchivo(controlPadre: HTMLDivElement, propiedadDto: string): HTMLInputElement {
            let selectores: NodeListOf<HTMLInputElement> = controlPadre.querySelectorAll(`input[tipo="${TipoControl.Archivo}"]`) as NodeListOf<HTMLInputElement>;
            for (var i = 0; i < selectores.length; i++) {
                var control = selectores[i] as HTMLInputElement;
                var dto = control.getAttribute(Atributo.propiedad);
                if (dto === propiedadDto.toLowerCase())
                    return control;
            }
            return null;
        }

        protected BuscarVisorDeImagen(controlPadre: HTMLDivElement, propiedadDto: string): HTMLImageElement {
            let visor: NodeListOf<HTMLImageElement> = controlPadre.querySelectorAll(`img[tipo="${TipoControl.VisorDeArchivo}"]`) as NodeListOf<HTMLImageElement>;
            for (var i = 0; i < visor.length; i++) {
                var control = visor[i] as HTMLImageElement;
                var dto = control.getAttribute(Atributo.propiedad);
                if (dto === propiedadDto.toLowerCase())
                    return control;
            }
            return null;
        }


        protected BuscarUrlDelArchivo(controlPadre: HTMLDivElement, propiedadDto: string): HTMLInputElement {
            let selectores: NodeListOf<HTMLInputElement> = controlPadre.querySelectorAll(`input[tipo="${TipoControl.Archivo}"]`) as NodeListOf<HTMLInputElement>;
            for (var i = 0; i < selectores.length; i++) {
                var control = selectores[i] as HTMLInputElement;
                var dto = control.getAttribute(Atributo.propiedad);
                if (dto === propiedadDto.toLowerCase())
                    return control;
            }
            return null;
        }

        protected BuscarSelect(controlPadre: HTMLDivElement, propiedadDto: string): HTMLSelectElement {
            let select: HTMLCollectionOf<HTMLSelectElement> = controlPadre.getElementsByTagName("select") as HTMLCollectionOf<HTMLSelectElement>;
            for (var i = 0; i < select.length; i++) {
                var control = select[i] as HTMLSelectElement;
                var dto = control.getAttribute(Atributo.propiedad);
                if (dto === propiedadDto)
                    return control;
            }
            return null;
        }

        protected BuscarListaDinamica(controlPadre: HTMLDivElement, propiedadDto: string): HTMLInputElement {

            let inputs: NodeListOf<HTMLInputElement> = controlPadre.querySelectorAll(`input[${Atributo.tipo}="${TipoControl.ListaDinamica}"]`) as NodeListOf<HTMLInputElement>;

            for (var i = 0; i < inputs.length; i++) {
                var control = inputs[i] as HTMLInputElement;
                var dto = control.getAttribute(Atributo.propiedad);
                if (dto === propiedadDto)
                    return control;
            }
            return null;
        }

        protected AntesDeMapearDatosDeIU(panel: HTMLDivElement): JSON {
            if (this.ModoTrabajo === ModoTrabajo.creando)
                return JSON.parse(`{"${Literal.id}":"0"}`);

            if (this.ModoTrabajo === ModoTrabajo.editando) {
                let input: HTMLInputElement = this.BuscarEditor(panel, Literal.id);
                if (Number(input.value) <= 0)
                    throw new Error(`El valor del id ${Number(input.value)} debe ser mayor a 0`);
                return JSON.parse(`{"${Literal.id}":"${Number(input.value)}"}`);
            }
        }

        protected MapearControlesDeIU(panel: HTMLDivElement): JSON {
            let elementoJson: JSON = this.AntesDeMapearDatosDeIU(panel);

            this.MapearSelectoresDeElementos(panel, elementoJson);
            this.MapearSelectoresDinamicos(panel, elementoJson);
            this.MapearEditores(panel, elementoJson);
            this.MapearArchivos(panel, elementoJson);

            return this.DespuesDeMapearDatosDeIU(panel, elementoJson);
        }

        protected MapearEditores(panel: HTMLDivElement, elementoJson: JSON): void {
            let editores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.Editor}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < editores.length; i++) {
                this.MapearEditor(editores[i], elementoJson);
            }
        }

        private MapearEditor(input: HTMLInputElement, elementoJson: JSON): void {
            var propiedadDto = input.getAttribute(Atributo.propiedad);
            let valor: string = (input as HTMLInputElement).value;
            let obligatorio: string = input.getAttribute(Atributo.obligatorio);

            if (obligatorio === "S" && valor.NoDefinida()) {
                input.classList.remove(ClaseCss.crtlValido);
                input.classList.add(ClaseCss.crtlNoValido);
                throw new Error(`El campo ${propiedadDto} es obligatorio`);
            }

            input.classList.remove(ClaseCss.crtlNoValido);
            input.classList.add(ClaseCss.crtlValido);
            elementoJson[propiedadDto] = valor;
        }

        protected MapearSelectoresDinamicos(panel: HTMLDivElement, elementoJson: JSON): void {
            let selectores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.ListaDinamica}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < selectores.length; i++) {
                this.MapearSelectorDinamico(selectores[i], elementoJson);
            }
        }

        private MapearSelectorDinamico(input: HTMLInputElement, elementoJson: JSON) {
            let propiedadDto = input.getAttribute(Atributo.propiedad);
            let guardarEn: string = input.getAttribute(AtributosDeListas.guardarEn);
            let obligatorio: string = input.getAttribute(Atributo.obligatorio);
            let lista: ListaDinamica = new ListaDinamica(input);
            let valor: number = lista.BuscarSeleccionado(input.value);

            if (obligatorio === "S" && (EsNula(input.value) || Number(valor) === 0)) {
                input.classList.remove(ClaseCss.crtlValido);
                input.classList.add(ClaseCss.crtlNoValido);
                throw new Error(`Debe seleccionar un elemento de la lista ${propiedadDto}`);
            }

            input.classList.remove(ClaseCss.crtlNoValido);
            input.classList.add(ClaseCss.crtlValido);
            elementoJson[guardarEn] = valor.toString();
        }

        protected MapearSelectoresDeElementos(panel: HTMLDivElement, elementoJson: JSON): void {
            let selectores: NodeListOf<HTMLSelectElement> = panel.querySelectorAll(`select[tipo="${TipoControl.ListaDeElementos}"]`) as NodeListOf<HTMLSelectElement>;
            for (let i = 0; i < selectores.length; i++) {
                this.MapearSelectorDeElementos(selectores[i], elementoJson);
            }
        }

        private MapearSelectorDeElementos(selector: HTMLSelectElement, elementoJson: JSON) {
            let propiedadDto = selector.getAttribute(Atributo.propiedad);
            let guardarEn: string = selector.getAttribute(AtributosDeListas.guardarEn);
            let obligatorio: string = selector.getAttribute(Atributo.obligatorio);

            if (obligatorio === "S" && Number(selector.value) === 0) {
                selector.classList.remove(ClaseCss.crtlValido);
                selector.classList.add(ClaseCss.crtlNoValido);
                throw new Error(`Debe seleccionar un elemento de la lista ${propiedadDto}`);
            }

            selector.classList.remove(ClaseCss.crtlNoValido);
            selector.classList.add(ClaseCss.crtlValido);
            elementoJson[guardarEn] = selector.value;
        }

        protected MapearArchivos(panel: HTMLDivElement, elementoJson: JSON): void {
            let archivos: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.Archivo}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < archivos.length; i++) {
                this.MapearArchivo(archivos[i], elementoJson);
            }
        }

        private MapearArchivo(archivo: HTMLInputElement, elementoJson: JSON): void {
            var propiedadDto = archivo.getAttribute(Atributo.propiedad);
            let valor: string = archivo.getAttribute(AtributoSelectorArchivo.idArchivo);
            let obligatorio: string = archivo.getAttribute(Atributo.obligatorio);

            if (obligatorio === "S" && EsNula(valor)) {
                archivo.classList.remove(ClaseCss.crtlValido);
                archivo.classList.add(ClaseCss.crtlNoValido);
                throw new Error(`El campo ${propiedadDto} es obligatorio`);
            }

            archivo.classList.remove(ClaseCss.crtlNoValido);
            archivo.classList.add(ClaseCss.crtlValido);
            elementoJson[propiedadDto] = valor;
        }

        protected DespuesDeMapearDatosDeIU(panel: HTMLDivElement, elementoJson: JSON): JSON {
            return elementoJson;
        }


        // funciones de carga de elementos para los selectores   ************************************************************************************

        protected CargarListaDinamica(input: HTMLInputElement, controlador: string) {
            let clase: string = input.getAttribute(AtributosDeListas.claseElemento);
            let idInput: string = input.getAttribute('id');
            let url: string = this.DefinirPeticionDeCargarDinamica(controlador, clase, input.value);
            let datosDeEntrada = `{"ClaseDeElemento":"${clase}", "IdInput":"${idInput}"}`;
            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EndPoint.CargaDinamica
                , datosDeEntrada
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.AnadirOpciones
                , this.SiHayErrorTrasPeticionAjax
            );

            a.Ejecutar();
        }

        private AnadirOpciones(peticion: ApiDeAjax.DescriptorAjax) {
            let datos: DatosPeticionDinamica = JSON.parse(peticion.DatosDeEntrada);

            let listaDinamica: ListaDinamica = new ListaDinamica(document.getElementById(datos.IdInput) as HTMLInputElement);
            for (var i = 0; i < peticion.resultado.datos.length; i++) {
                listaDinamica.AgregarOpcion(peticion.resultado.datos[i].id, peticion.resultado.datos[i].nombre);
            }

            listaDinamica.Lista.click();
        }

        protected CargarListaDeElementos(controlador: string, claseDeElementoDto: string, idLista: string) {

            let url: string = this.DefinirPeticionDeCargarElementos(controlador, claseDeElementoDto);
            let datosDeEntrada = `{"ClaseDeElemento":"${claseDeElementoDto}", "IdLista":"${idLista}"}`;
            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EndPoint.CargarLista
                , datosDeEntrada
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.MapearElementosEnLista
                , this.SiHayErrorTrasPeticionAjax
            );

            a.Ejecutar();
        }

        private MapearElementosEnLista(peticion: ApiDeAjax.DescriptorAjax) {
            let datos: DatosPeticionLista = JSON.parse(peticion.DatosDeEntrada);
            let idLista = datos.IdLista;
            let lista = new ListaDeElemento(idLista);
            for (var i = 0; i < peticion.resultado.datos.length; i++) {
                lista.AgregarOpcion(peticion.resultado.datos[i].id, peticion.resultado.datos[i].nombre);
            }

            lista.Lista.setAttribute(AtributosDeListas.yaCargado, "S");
        }

        private DefinirPeticionDeCargarElementos(controlador: string, claseElemento: string): string {
            let url: string = `/${controlador}/${Ajax.EndPoint.CargarLista}?${Ajax.Param.claseElemento}=${claseElemento}`;
            return url;
        }


        private DefinirPeticionDeCargarDinamica(controlador: string, claseElemento: string, filtro: string): string {
            let url: string = `/${controlador}/${Ajax.EndPoint.CargaDinamica}?${Ajax.Param.claseElemento}=${claseElemento}&posicion=0&cantidad=-1&filtro=${filtro}`;
            return url;
        }

    }

}