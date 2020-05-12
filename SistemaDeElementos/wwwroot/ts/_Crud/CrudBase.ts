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

    export class PeticionAjax {
        public nombre: string;
        public datos: any;
        public resultado: ResultadoJson;

        constructor(peticion: string, datos: string) {
            this.nombre = peticion;
            this.datos = datos;
            this.resultado = undefined;
        }

        ParsearRespuesta(req: XMLHttpRequest) {
            try {
                this.resultado = JSON.parse(req.response);
            }
            catch
            {
                Mensaje(TipoMensaje.Error, `Error al procesar la respuesta de ${this.nombre}`);
            }
        }
    }

    export class CrudBase {

        private modoTrabajo: string;
        protected get ModoTrabajo(): string {
            return this.modoTrabajo;
        }
        protected set ModoTrabajo(modo: string) {
            this.modoTrabajo=modo;
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

        private BlanquearControlesDeIU(panel: HTMLDivElement) {
            let controles: HTMLCollectionOf<Element> = panel.getElementsByClassName(ClaseCss.propiedad);
            for (var i = 0; i < controles.length; i++) {
                var control = controles[i] as HTMLElement;

                if (control instanceof HTMLInputElement) {
                    this.BlanquearInput(control as HTMLInputElement);
                }
            }
        }

        private BlanquearInput(input: HTMLInputElement) {
            input.classList.remove(ClaseCss.crtlNoValido);
            input.classList.add(ClaseCss.crtlValido);
            input.value = "";
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
        }

        private MapearPropiedaAlEditor(panel: HTMLDivElement, propiedad: string, valor: any) {
            let editor: HTMLInputElement = this.BuscarInput(panel, propiedad);

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


        // funciones para la gestión de los mapeos de controles a un json  ****************************************************************************

        protected BuscarInput(controlPadre: HTMLDivElement, propiedadDto: string): HTMLInputElement {
            let input: HTMLCollectionOf<HTMLInputElement> = controlPadre.getElementsByTagName("input") as HTMLCollectionOf<HTMLInputElement>;
            for (var i = 0; i < input.length; i++) {
                var control = input[i] as HTMLInputElement;
                var dto = control.getAttribute(Atributo.propiedad);
                if (dto === propiedadDto)
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
                let input: HTMLInputElement = this.BuscarInput(panel, Literal.id);
                if (Number(input.value) <= 0)
                    throw new Error(`El valor del id ${Number(input.value)} debe ser mayor a 0`);
                return JSON.parse(`{"${Literal.id}":"${Number(input.value)}"}`);
            }
        }

        protected MapearControlesDeIU(panel: HTMLDivElement): JSON {
            let elementoJson: JSON = this.AntesDeMapearDatosDeIU(panel);

            this.MapearSelectoresDeElementos(panel, elementoJson);
            this.MapearEditores(panel, elementoJson);

            return this.DespuesDeMapearDatosDeIU(panel, elementoJson);
        }

        protected MapearSelectoresDeElementos(panel: HTMLDivElement, elementoJson: JSON): void {
            let selectores: NodeListOf<HTMLSelectElement> = panel.querySelectorAll(`select[tipo="${TipoControl.SelectorDeElemento}"]`) as NodeListOf<HTMLSelectElement>;
            for (let i = 0; i < selectores.length; i++) {
                this.MapearSelectorDeElementos(selectores[i], elementoJson);
            }
        }

        protected MapearEditores(panel: HTMLDivElement, elementoJson: JSON): void {
            let editores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.Editor}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < editores.length; i++) {
                this.MapearEditor(editores[i], elementoJson);
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

        protected DespuesDeMapearDatosDeIU(panel: HTMLDivElement, elementoJson: JSON): JSON {
            return elementoJson;
        }


        // funciones de carga de elementos para los selectores   ************************************************************************************


        protected CargarSelectorElemento(controlador: string, claseDeElementoDto: string, idSelector: string) {
            let url: string = this.DefinirPeticionDeCargar(controlador, claseDeElementoDto);
            let req: XMLHttpRequest = new XMLHttpRequest();
            let peticion: PeticionAjax = new PeticionAjax(Ajax.EndPoint.LeerTodos, `{"ClaseDeElemento":"${claseDeElementoDto}", "IdSelector":"${idSelector}"}`);
            this.PeticionSincrona(req, url, peticion);
        }

        private DefinirPeticionDeCargar(controlador: string, claseElemento: string): string {
            let url: string = `/${controlador}/${Ajax.EndPoint.LeerTodos}?${Ajax.Param.claseElemento}=${claseElemento}`;
            return url;
        }



        // funciones para las peticiones al servidor  ***********************************************************************************************
        public PeticionSincrona(req: XMLHttpRequest, url: string, peticion: PeticionAjax) {
            BlanquearMensaje();
            let error: string;
            this.PeticionAjaxSincrona(req, url, peticion, () => this.DespuesDeLaPeticion(req, peticion), () => error = this.ErrorEnPeticion(req, peticion));
            if (!EsNula(error))
                throw error;
        }

        public PeticionAsincrona(req: XMLHttpRequest, url: string, peticion: PeticionAjax) {
            BlanquearMensaje();
            let error: string;
            this.PeticionAjaxAsincrona(req, url, peticion, () => this.DespuesDeLaPeticion(req, peticion), () => error = this.ErrorEnPeticion(req, peticion));
            if (!EsNula(error))
                throw error;
        }

        private PeticionAjaxSincrona(req: XMLHttpRequest, url: string, peticion: PeticionAjax, despuesDePeticion: Function, errorEnPeticion: Function) {

            function respuestaCorrecta() {

                if (EsNula(req.response))
                    errorEnPeticion();
                else {
                    peticion.ParsearRespuesta(req);

                    if (peticion.resultado === undefined || peticion.resultado.estado === Ajax.jsonResultError)
                        errorEnPeticion();
                    else
                        despuesDePeticion();
                }
            }

            function respuestaErronea() {
                errorEnPeticion();
            }

            req.addEventListener(Ajax.eventoLoad, respuestaCorrecta);
            req.addEventListener(Ajax.eventoError, respuestaErronea);

            req.open('GET', url, false);
            req.send();
        }

        private PeticionAjaxAsincrona(req: XMLHttpRequest, url: string, peticion: PeticionAjax, despuesDePeticion: Function, errorEnPeticion: Function) {

            function respuestaCorrecta() {

                if (EsNula(req.response))
                    errorEnPeticion();
                else {
                    peticion.ParsearRespuesta(req);

                    if (peticion.resultado === undefined || peticion.resultado.estado === Ajax.jsonResultError)
                        errorEnPeticion();
                    else
                        despuesDePeticion();
                }
            }

            function respuestaErronea() {
                errorEnPeticion();
            }

            req.addEventListener(Ajax.eventoLoad, respuestaCorrecta);
            req.addEventListener(Ajax.eventoError, respuestaErronea);

            req.open('GET', url, true);
            req.send();
        }

        protected ErrorEnPeticion(req: XMLHttpRequest, peticion: PeticionAjax): string {
            if (EsNula(req.response)) {
                return `La peticion ${peticion} no se ha podido realizar`;
            }

            let resultado: ResultadoJson = JSON.parse(req.response);
            console.error(resultado.consola);
            if (!EsNula(resultado.mensaje))
                resultado.mensaje = `Error al ejecutar la peticion'${peticion.nombre}. ${resultado.mensaje}'`;

            return resultado.mensaje;

        }

        protected DespuesDeLaPeticion(req: XMLHttpRequest, peticion: PeticionAjax): ResultadoJson {
            let resultado: ResultadoJson = JSON.parse(req.response);
            if (!EsNula(resultado.mensaje))
                Mensaje(TipoMensaje.Info, resultado.mensaje);

            return resultado;
        }
    }

}