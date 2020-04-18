namespace Crud {

    export class ResultadoJson {
        mensaje: string;
        consola: string;
        datos: any;
    }

    export class CrudBase {

        protected ModoTrabajo;

        public ResultadoPeticion: string;
        public PeticioCorrecta: boolean = false;
        public PeticionRealizada: boolean = false;

        constructor(modo) {
            this.ModoTrabajo = modo
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

        // funciones para mapear un elemento Json a los controles de un panel

        protected MapearElemento(panel: HTMLDivElement,elementoJson: JSON) {

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

        private MapearPropiedad(panel: HTMLDivElement,propiedad: string, valor: any) {
            var control = this.BuscarControl(panel, propiedad);
            if (control instanceof HTMLInputElement)
                control.value = valor;

        }


       // funciones para la gestión de los mapeos de controles a un json  ****************************************************************************

       protected BuscarControl(controlPadre: HTMLDivElement, propiedadDto: string): HTMLElement {

           let controles: HTMLCollectionOf<Element> = controlPadre.getElementsByClassName(ClaseCss.propiedad);
            for (var i = 0; i < controles.length; i++) {
                var control = controles[i] as HTMLElement;
                var dto = control.getAttribute(Atributo.propiedadDto);
                if (dto === propiedadDto)
                    return control;
            }
            return null;
        }

        protected AntesDeMapearDatosDeIU(panel: HTMLDivElement): JSON {           
                return JSON.parse(`{"${Literal.id}":"0"}`);
        }

        protected MapearControlesDeIU(panel: HTMLDivElement): JSON {
            let elementoJson: JSON = this.AntesDeMapearDatosDeIU(panel);

            let controles: HTMLCollectionOf<Element> = panel.getElementsByClassName("propiedad");
            for (var i = 0; i < controles.length; i++) {
                var control = controles[i] as HTMLElement;
                if (control instanceof HTMLInputElement) {
                    this.MapearInput(control, elementoJson);
                }
            }

            return this.DespuesDeMapearDatosDeIU(panel, elementoJson);
        }

        protected MapearInput(input: HTMLInputElement, elementoJson: JSON): void {
            var propiedadDto = input.getAttribute(Atributo.propiedadDto);
            let valor: string = (input as HTMLInputElement).value;
            let obligatorio: string = input.getAttribute(Atributo.obligatorio);

            if (obligatorio === "S" && valor.NoDefinida()) {
                //let cssNoValida: string = input.getAttribute(Atributo.cssCrtlNoValido);
                //input.className = `${ClaseCss.propiedad} ${cssNoValida}`;
                input.classList.remove(ClaseCss.crtlValido);
                input.classList.add(ClaseCss.crtlNoValido);
                throw new Error(`El campo ${propiedadDto} es obligatorio`);
            }

            //let cssValida: string = input.getAttribute(Atributo.cssCrtlValido);
            //input.className = `${ClaseCss.propiedad} ${cssValida}`;

            input.classList.remove(ClaseCss.crtlNoValido);
            input.classList.add(ClaseCss.crtlValido);
            elementoJson[propiedadDto] = valor;
        }

        protected DespuesDeMapearDatosDeIU(panel: HTMLDivElement, elementoJson: JSON): JSON {
            return elementoJson;
        }


        // funciones para las peticiones al servidor  ***********************************************************************************************

        protected PeticionSincrona(req: XMLHttpRequest, url: string,  peticion: string) {
            this.PeticionAjax(req, url,  () => this.DespuesDeLaPeticion(req), () => this.ErrorEnPeticion(req, peticion));
        }

        private PeticionAjax(req: XMLHttpRequest, url: string,  despuesDePeticion: Function, errorEnPeticion: Function) {

            function respuestaCorrecta() {
                if (EsNula(req.response)) {
                    errorEnPeticion();
                }
                else {
                    var resultado: any = ParsearRespuesta(req);
                    if (resultado.estado === Ajax.jsonResultError) {
                        errorEnPeticion();
                    }
                    else {
                        despuesDePeticion();
                    }
                }
            }

            function respuestaErronea() {
                this.ResultadoPeticion = "Peticion no realizada";
                this.PeticionRealizada = false;
            }

            req.addEventListener(Ajax.eventoLoad, respuestaCorrecta);
            req.addEventListener(Ajax.eventoError, respuestaErronea);

            req.open('GET', url, false);
            req.send();
        }

        protected ErrorEnPeticion(req: XMLHttpRequest, peticion: string): void {
            if (EsNula(req.response)) {
                this.ResultadoPeticion = `La peticion ${peticion} no está definida`;
            }
            else {
                let resultado: ResultadoJson = JSON.parse(req.response);
                this.ResultadoPeticion = resultado.mensaje;
                this.PeticionRealizada = true;
                console.error(resultado.consola);
            }
        }

        protected DespuesDeLaPeticion(req: XMLHttpRequest): ResultadoJson {
            let resultado: ResultadoJson = JSON.parse(req.response);
            this.ResultadoPeticion = resultado.mensaje;
            this.PeticionRealizada = true;
            this.PeticioCorrecta = true;
            return resultado;
        }
    }

}