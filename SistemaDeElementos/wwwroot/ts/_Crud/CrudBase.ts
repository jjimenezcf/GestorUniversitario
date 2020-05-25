namespace Crud {

    export class HTMLSelector extends HTMLInputElement {
    }

    export class SelectorDeElementos {
        private selector: HTMLSelectElement;

        get Selector(): HTMLSelectElement {
            return this.selector;
        }

        constructor(idSelector: string) {
            this.selector = document.getElementById(idSelector) as HTMLSelectElement;
        }

        public AgregarOpcion(valor: number, texto: string): void {

            var miOption = document.createElement("option");
            miOption.setAttribute("value", valor.toString());
            miOption.setAttribute("label", texto);

            this.Selector.appendChild(miOption);
        }
    }

    export class DatosPeticionSelector {
        ClaseDeElemento: string;
        IdSelector: string;

        get Selector(): SelectorDeElementos {
            return new SelectorDeElementos(this.IdSelector);
        }

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

        protected InicializarSlectoresDeElementos(panel: HTMLDivElement, controlador: string) {
            let selectores: NodeListOf<HTMLSelectElement> = panel.querySelectorAll(`select[tipo="${TipoControl.SelectorDeElemento}"]`) as NodeListOf<HTMLSelectElement>;
            for (let i = 0; i < selectores.length; i++) {
                if (selectores[i].getAttribute("ya-cargada") === "S")
                    continue;

                let claseElemento: string = selectores[i].getAttribute(AtributoSelectorElemento.claseElemento);
                try {
                    this.CargarSelectorElemento(controlador, claseElemento, selectores[i].getAttribute(Atributo.id));
                    selectores[i].setAttribute("ya-cargada", "S");
                }
                catch (error) {
                    Mensaje(TipoMensaje.Error, `Error en el selector de elemento ${selectores[0].getAttribute(Atributo.propiedad)} al ejecutar ${controlador}/${Ajax.EndPoint.LeerTodos}. ${error}`);
                }
            }
        }

        protected Cerrar(panelMostrar: HTMLDivElement, panelCerrar: HTMLDivElement) {

            this.BlanquearControlesDeIU(panelCerrar);

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
            let editores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.Editor}"]`) as NodeListOf<HTMLInputElement>;
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
            let selectores: NodeListOf<HTMLSelectElement> = panel.querySelectorAll(`select[tipo="${TipoControl.SelectorDeElemento}"]`) as NodeListOf<HTMLSelectElement>;
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
            let archivos: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.Archivo}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < archivos.length; i++) {
                this.BlanquearArchivo(archivos[i]);
            }
        }

        private BlanquearArchivo(archivo: HTMLInputElement) {
            archivo.classList.remove(ClaseCss.crtlNoValido);
            archivo.classList.add(ClaseCss.crtlValido);
            archivo.setAttribute(AtributoSelectorArchivo.idArchivo, '0');
            archivo.files = null;
            let canvasHtml: HTMLCanvasElement = document.getElementById(archivo.getAttribute(AtributoSelectorArchivo.canvasVinculado)) as HTMLCanvasElement;
            canvasHtml.width = canvasHtml.width;
            let imagenHtml: HTMLImageElement = document.getElementById(archivo.getAttribute(AtributoSelectorArchivo.canvasVinculado)) as HTMLImageElement;
            imagenHtml.src = "";
            let barraHtml: HTMLDivElement = document.getElementById(archivo.getAttribute(AtributoSelectorArchivo.barraVinculada)) as HTMLDivElement;
            barraHtml.removeAttribute('style');
            barraHtml.innerHTML = null;
            barraHtml.appendChild(document.createElement("span"));
            barraHtml.classList.remove(ClaseCss.barraVerde);
            barraHtml.classList.remove(ClaseCss.barraRoja);
            barraHtml.classList.add(ClaseCss.barraAzul);
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

        // funciones para mapear un elemento Json a los controles de un panel

        protected MapearElementoLeido(panel: HTMLDivElement, elementoJson: JSON) {
            this.MapearPropiedadesDelElemento(panel, "elementoJson", elementoJson);
        }

        private MapearPropiedadesDelElemento(panel: HTMLDivElement, propiedad: string, valorPropiedadJson: any) {
            var tipoDeObjeto = typeof valorPropiedadJson;
            if (tipoDeObjeto == "object") {
                for (var propiedad in valorPropiedadJson) {
                    console.log("clave: ", propiedad);
                    this.MapearPropiedadesDelElemento(panel, propiedad, valorPropiedadJson[propiedad]);
                }
            } else {
                this.MapearPropiedad(panel, propiedad, valorPropiedadJson);
            }

        }

        private MapearPropiedad(panel: HTMLDivElement, propiedad: string, valor: any) {
            this.MapearPropiedaAlEditor(panel, propiedad, valor);
            this.MapearPropiedadAlSelectorDeElemento(panel, propiedad, valor);
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


        private MapearPropiedadAlSelectorDeArchivo(panel: HTMLDivElement, propiedad: string, valor: any) {
            let selector: HTMLInputElement = this.BuscarSelectorDeArchivo(panel, propiedad);

            if (selector === null)
                return;

            selector.setAttribute(AtributoSelectorArchivo.idArchivo, valor);
        }

        private MapearPropiedadAlVisorDeImagen(panel: HTMLDivElement, propiedad: string, valor: any) {

            let visor: HTMLImageElement = this.BuscarVisorDeImagen(panel, propiedad);

            if (visor === null)
                return

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
            }

        }


        // funciones para la gestión de los mapeos de controles a un json  ****************************************************************************

        protected BuscarEditor(controlPadre: HTMLDivElement, propiedadDto: string): HTMLInputElement {
            let editores: NodeListOf<HTMLInputElement> = controlPadre.querySelectorAll(`input[tipo="${TipoControl.Editor}"]`) as NodeListOf<HTMLInputElement>;
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

        protected MapearSelectoresDeElementos(panel: HTMLDivElement, elementoJson: JSON): void {
            let selectores: NodeListOf<HTMLSelectElement> = panel.querySelectorAll(`select[tipo="${TipoControl.SelectorDeElemento}"]`) as NodeListOf<HTMLSelectElement>;
            for (let i = 0; i < selectores.length; i++) {
                this.MapearSelectorDeElementos(selectores[i], elementoJson);
            }
        } 

        private MapearSelectorDeElementos(selector: HTMLSelectElement, elementoJson: JSON) {
            let propiedadDto = selector.getAttribute(Atributo.propiedad);
            let guardarEn: string = selector.getAttribute(AtributoSelectorElemento.guardarEn);
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

            if (obligatorio === "S" && valor.NoDefinida()) {
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


        protected CargarSelectorElemento(controlador: string, claseDeElementoDto: string, idSelector: string) {

            let url: string = this.DefinirPeticionDeCargar(controlador, claseDeElementoDto);
            let datosDeEntrada = `{"ClaseDeElemento":"${claseDeElementoDto}", "IdSelector":"${idSelector}"}`;
            let a = new ApiDeAjax.DescriptorAjax(Ajax.EndPoint.LeerTodos
                , datosDeEntrada
                , url
                , ApiDeAjax.TipoPeticion.Sincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.MapearElementosAlSelector
                , null
            );

            a.Ejecutar();
        }

        private MapearElementosAlSelector(peticion: ApiDeAjax.DescriptorAjax) {
            let datos: DatosPeticionSelector = JSON.parse(peticion.DatosDeEntrada);
            let idSelector = datos.IdSelector;
            let selector = new SelectorDeElementos(idSelector);
            for (var i = 0; i < peticion.resultado.datos.length; i++) {
                selector.AgregarOpcion(peticion.resultado.datos[i].id, peticion.resultado.datos[i].nombre);

            }
        }

        private DefinirPeticionDeCargar(controlador: string, claseElemento: string): string {
            let url: string = `/${controlador}/${Ajax.EndPoint.LeerTodos}?${Ajax.Param.claseElemento}=${claseElemento}`;
            return url;
        }

    }

}