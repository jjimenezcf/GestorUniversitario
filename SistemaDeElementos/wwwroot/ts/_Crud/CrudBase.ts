namespace Crud {

    export class CrudBase {

        protected divDeCreacionHtml: HTMLDivElement;
        protected divDeEdicionHtml: HTMLDivElement;
        protected divDeMntHtml: HTMLDivElement;

        public ResultadoPeticion: string;
        public PeticionRealizada: boolean = false;

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string) {
            if (!EsNula(idPanelCreacion))
                this.divDeCreacionHtml = document.getElementById(idPanelCreacion) as HTMLDivElement;

            if (!EsNula(idPanelEdicion))
                this.divDeEdicionHtml = document.getElementById(idPanelEdicion) as HTMLDivElement;

            if (!EsNula(idPanelMnt))
                this.divDeMntHtml = document.getElementById(idPanelMnt) as HTMLDivElement;
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
            let propiedades: HTMLCollectionOf<Element> = this.divDeCreacionHtml.getElementsByClassName("propiedad");
            for (var i = 0; i < propiedades.length; i++) {
                var propiedad = propiedades[i] as HTMLElement;
                if (propiedad instanceof HTMLInputElement) {
                    let cssValida: string = propiedad.getAttribute(Atributo.classValido);
                    propiedad.className = `${ClaseCss.classPropiedad} ${cssValida}`;
                    (propiedad as HTMLInputElement).value = "";
                }
            }
        }
    }

}