namespace Crud {

    export class ResultadoJson {
        mensaje: string;
        consola: string;
        datos: any;
    }

    export class CrudBase {

        protected PanelCreacion: HTMLDivElement;
        protected PanelEdicion: HTMLDivElement;
        protected PanelMnt: HTMLDivElement;

        public ResultadoPeticion: string;
        public PeticionRealizada: boolean = false;

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string) {
            if (!EsNula(idPanelCreacion))
                this.PanelCreacion = document.getElementById(idPanelCreacion) as HTMLDivElement;

            if (!EsNula(idPanelEdicion))
                this.PanelEdicion = document.getElementById(idPanelEdicion) as HTMLDivElement;

            if (!EsNula(idPanelMnt))
                this.PanelMnt = document.getElementById(idPanelMnt) as HTMLDivElement;
        }


        public Cerrar(htmlDivMostrar: HTMLDivElement, htmlDivOcultar: HTMLDivElement) {

            this.BlanquearControlesDeIU();

            htmlDivMostrar.classList.add(ClaseCss.divVisible);
            htmlDivMostrar.classList.remove(ClaseCss.divNoVisible);

            htmlDivOcultar.classList.add(ClaseCss.divNoVisible);
            htmlDivOcultar.classList.remove(ClaseCss.divVisible);

            BlanquearMensaje();

        }

        private BlanquearControlesDeIU() {
            let propiedades: HTMLCollectionOf<Element> = this.PanelCreacion.getElementsByClassName("propiedad");
            for (var i = 0; i < propiedades.length; i++) {
                var propiedad = propiedades[i] as HTMLElement;
                if (propiedad instanceof HTMLInputElement) {
                    let cssValida: string = propiedad.getAttribute(Atributo.classValido);
                    propiedad.className = `${ClaseCss.classPropiedad} ${cssValida}`;
                    (propiedad as HTMLInputElement).value = "";
                }
            }
        }

        protected BuscarControl(controlPadre: HTMLDivElement, propiedadDto: string): HTMLElement {

            let controles: HTMLCollectionOf<Element> = controlPadre.getElementsByClassName("propiedad");
            for (var i = 0; i < controles.length; i++) {
                var control = controles[i] as HTMLElement;
                var dto = control.getAttribute(Atributo.propiedadDto);
                if (dto === propiedadDto)
                    return control;
            }
            return null;
        }

        protected PeticionSincrona(req: XMLHttpRequest, peticion: string) {
            this.PeticionAjax(req, () => this.DespuesDeLaPeticion(req), () => this.ErrorEnPeticion(req, peticion));
        }

        private PeticionAjax(req: XMLHttpRequest, despuesDePeticion: Function, errorEnPeticion: Function) {

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
            return resultado;
        }
    }

}