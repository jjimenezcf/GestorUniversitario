class CrudCreacion {

    public divDeCreacionHtml: HTMLDivElement;

    public ResultadoPeticion: string;
    public PeticionRealizada: boolean = false;
    public Creado: boolean = false;

    constructor() {
    }

    public InicializarValores(): void {

    }

    public Aceptar(htmlDivMostrar: HTMLDivElement, htmlDivOcultar: HTMLDivElement) {
        let json: JSON = null;
        try {
            json = this.MapearDatosDeIu();
        }
        catch (error) {
            this.ResultadoPeticion = error.message;
            return;
        }
        this.CrearElemento(json, htmlDivMostrar, htmlDivOcultar);
    }

    private urlPeticionCrear(json: JSON): string {
        let controlador = this.divDeCreacionHtml.getAttribute(Literal.controlador);
        let url: string = `/${controlador}/${LiteralCrt.accionCrear}?${Literal.elementoJson}=${JSON.stringify(json)}`;
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
                var propiedadDto = propiedad.getAttribute(Literal.propiedadDto);
                let valor: string = (propiedad as HTMLInputElement).value;
                let obligatorio: string = propiedad.getAttribute(LiteralCrt.obligatorio);
                if (obligatorio === "S" && valor.IsNullOrEmpty()) {
                    let cssNoValida: string = propiedad.getAttribute(LiteralCrt.classNoValido);
                    propiedad.className = `${LiteralCrt.classPropiedad} ${cssNoValida}`;
                    throw new Error(`El campo ${propiedadDto} es obligatorio`);
                }

                let cssValida: string = propiedad.getAttribute(LiteralCrt.classValido);
                propiedad.className = `${LiteralCrt.classPropiedad} ${cssValida}`;
                json[propiedadDto] = valor;

            }
        }

        return this.DespuesDeMapearDatosDeIU(json);
    }

    protected DespuesDeMapearDatosDeIU(json: JSON): JSON {
        return json;
    }

    protected AntesDeMapearDatosDeIU(): JSON {
        return JSON.parse(`{"${Literal.id}":"0"}`);
    }

    public Cerrar(htmlDivMostrar: HTMLDivElement, htmlDivOcultar: HTMLDivElement) {

        this.BlanquearDatosDeCreacion();

        htmlDivMostrar.classList.add(Literal.divVisible);
        htmlDivMostrar.classList.remove(Literal.divNoVisible);

        htmlDivOcultar.classList.add(Literal.divNoVisible);
        htmlDivOcultar.classList.remove(Literal.divVisible);

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
            if (resultado.estado === Literal.jsonResultError) {
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

        req.addEventListener(Literal.eventoLoad, respuestaCorrecta);
        req.addEventListener(Literal.eventoError, respuestaErronea);
        req.send();
    }


}
