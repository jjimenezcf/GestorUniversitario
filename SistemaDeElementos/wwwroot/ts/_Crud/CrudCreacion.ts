class CrudCreacion {

    public divDeCreacionHtml: HTMLDivElement;

    public ResultadoPeticion: string;
    public PeticionRealizada: boolean;
    public Creado: boolean;

    constructor() {
    }

    public InicializarValores(): void {

    }

    public Aceptar(htmlDivMostrar: HTMLDivElement, htmlDivOcultar: HTMLDivElement) {
        let json: JSON = this.MapearDatosDeIu();
        this.CrearElemento(json, htmlDivMostrar, htmlDivOcultar);
    }

    private urlPeticionCrear(json: JSON): string {
        let controlador = this.divDeCreacionHtml.getAttribute("controlador");
        let url: string = `/${controlador}/CrearElemento?elementoJson=${JSON.stringify(json)}`;
        return url;
    }

    private CrearElemento(json: JSON, htmlDivMostrar: HTMLDivElement, htmlDivOcultar: HTMLDivElement) {
        let url: string = this.urlPeticionCrear(json);
        let req: XMLHttpRequest = new XMLHttpRequest();
        req.open('GET', url, false);
        this.PeticionCrear(req, () => this.DespuesDeCrear(req), () => this.ErrorAlCrear(req));
    }

    protected DespuesDeCrear(req: XMLHttpRequest): void {
        let resultado = JSON.parse(req.response);
        this.ResultadoPeticion = resultado.mensaje;
        this.Creado = true;
    }

    protected ErrorAlCrear(req: XMLHttpRequest): void {
        let resultado = JSON.parse(req.response);
        this.ResultadoPeticion = resultado.mensaje;
        this.Creado = false;
    }

    private MapearDatosDeIu(): JSON {
        let json: JSON = this.AntesDeMapearDatosDeIU();

        let propiedades: HTMLCollectionOf<Element> = this.divDeCreacionHtml.getElementsByClassName("propiedad");
        for (var i = 0; i < propiedades.length; i++) {
            var propiedad = propiedades[i] as HTMLElement;
            if (propiedad instanceof HTMLInputElement) {
                var propiedadDto = propiedad.getAttribute("propiedad-dto");
                json[propiedadDto] = (propiedad as HTMLInputElement).value;
            }
        }

        return this.DespuesDeMapearDatosDeIU(json);
    }

    protected DespuesDeMapearDatosDeIU(json: JSON): JSON {
        return json;
    }

    protected AntesDeMapearDatosDeIU(): JSON {
        return JSON.parse('{"id":"0"}');
    }

    public Cerrar(htmlDivMostrar: HTMLDivElement, htmlDivOcultar: HTMLDivElement) {

        this.BlanquearDatosDeCreacion();

        htmlDivMostrar.classList.add("div-visible");
        htmlDivMostrar.classList.remove("div-no-visible");

        htmlDivOcultar.classList.add("div-no-visible");
        htmlDivOcultar.classList.remove("div-visible");

        BlanquearMensaje();

    }


    private BlanquearDatosDeCreacion() {
        let propiedades: HTMLCollectionOf<Element> = this.divDeCreacionHtml.getElementsByClassName("propiedad");
        for (var i = 0; i < propiedades.length; i++) {
            var propiedad = propiedades[i] as HTMLElement;
            if (propiedad instanceof HTMLInputElement) {
                (propiedad as HTMLInputElement).value = "";
            }
        }
    }


    private PeticionCrear(req: XMLHttpRequest, despuesDeCrear: Function, errorAlCrear: Function) {

        function respuestaCorrecta() {
            let resultado = JSON.parse(req.response);
            if (resultado.estado === "Error") {
                errorAlCrear();
            }
            else {
                despuesDeCrear();
            }
        }

        function respuestaErronea() {
            this.ResultadoPeticion = "Peticion no realizada";
            this.PeticionRealizada = false;
        }

        req.addEventListener("load", respuestaCorrecta);
        req.addEventListener("error", respuestaErronea);
        req.send();
    }


}
