class CrudCreacion {

    //protected static Crud: CrudCreacion;

    public divDeCreacionHtml: HTMLDivElement;

    public PeticionRealizada: boolean = false;

    public ResultadoPeticion: string;
    public MensajeDeError: string;    

    constructor() {
    }

    public InicializarValores(): void {

    }

    public Aceptar(htmlDivMostrar: HTMLDivElement, htmlDivOcultar: HTMLDivElement): void {

        let json: JSON = this.MapearDatosDeIu();
        try {
            this.Persistir(json, htmlDivMostrar, htmlDivOcultar);
        }
        finally {
            Mensaje(TipoMensaje.Info, "petición de creación realizada");
        }

    }

    private urlPeticionCrear(json: JSON): string {
        let controlador = this.divDeCreacionHtml.getAttribute("controlador");
        let url: string = `/${controlador}/CrearElemento?elementoJson=${JSON.stringify(json)}`;
        return url;
    }

    private Persistir(json: JSON, htmlDivMostrar: HTMLDivElement, htmlDivOcultar: HTMLDivElement): void {
        var url: string = this.urlPeticionCrear(json);
        this.PeticionCrear(url, () => this.Cerrar(htmlDivMostrar, htmlDivOcultar));
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


    private PeticionCrear(url: string, funcionDeRespuesta: Function) {

        function respuestaCorrecta() {
            if (req.status >= 200 && req.status < 400) {
                funcionDeRespuesta();
            }
            else {
                Mensaje(TipoMensaje.Error, req.responseText);
            }
            this.PeticionRealizada = true;
            this.ResultadoPeticion = "Elemento creado";
        }

        function respuestaErronea() {
            this.PeticionRealizada = true;
            this.MensajeDeError = req.responseText;
        }

        var req = new XMLHttpRequest();
        req.open('GET', url, true);
        req.addEventListener("load", respuestaCorrecta);
        req.addEventListener("error", respuestaErronea);
        req.send();
    }


}
