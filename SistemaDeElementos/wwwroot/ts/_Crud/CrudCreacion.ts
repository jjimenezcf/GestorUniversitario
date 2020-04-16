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
            json = this.MapearControlesDeIU();
        }
        catch (error) {
            this.ResultadoPeticion = error.message;
            return;
        }
        this.CrearElemento(json, htmlDivMostrar, htmlDivOcultar);
    }

    protected AntesDeMapearDatosDeIU(): JSON {
        return JSON.parse(`{"${Literal.id}":"0"}`);
    }

    private MapearControlesDeIU(): JSON {
        let json: JSON = this.AntesDeMapearDatosDeIU();

        let propiedades: HTMLCollectionOf<Element> = this.divDeCreacionHtml.getElementsByClassName("propiedad");
        for (var i = 0; i < propiedades.length; i++) {
            var propiedad = propiedades[i] as HTMLElement;
            if (propiedad instanceof HTMLInputElement) {
                var propiedadDto = propiedad.getAttribute(Atributo.propiedadDto);
                json[propiedadDto] = this.MapearInput(propiedad, propiedadDto);
            }
        }

        return this.DespuesDeMapearDatosDeIU(json);
    }

    private MapearInput(propiedad: HTMLInputElement, propiedadDto: string): string {
        let valor: string = (propiedad as HTMLInputElement).value;
        let obligatorio: string = propiedad.getAttribute(Atributo.obligatorio);

        if (obligatorio === "S" && valor.IsNullOrEmpty()) {
            let cssNoValida: string = propiedad.getAttribute(Atributo.classNoValido);
            propiedad.className = `${ClaseCss.classPropiedad} ${cssNoValida}`;
            throw new Error(`El campo ${propiedadDto} es obligatorio`);
        }

        let cssValida: string = propiedad.getAttribute(Atributo.classValido);
        propiedad.className = `${ClaseCss.classPropiedad} ${cssValida}`;
        return valor;
    }

    protected DespuesDeMapearDatosDeIU(json: JSON): JSON {
        return json;
    }

    private CrearElemento(json: JSON, htmlDivMostrar: HTMLDivElement, htmlDivOcultar: HTMLDivElement) {
        let url: string = this.urlPeticionCrear(json);
        let req: XMLHttpRequest = new XMLHttpRequest();
        req.open('GET', url, false);
        this.PeticionCrear(req, () => this.DespuesDeCrear(req), () => this.ErrorAlCrear(req));
    }

    private urlPeticionCrear(json: JSON): string {
        let controlador = this.divDeCreacionHtml.getAttribute(Literal.controlador);
        let url: string = `/${controlador}/${Ajax.EndPoint.Crear}?${Ajax.Param.elementoJson}=${JSON.stringify(json)}`;
        return url;
    }

    private PeticionCrear(req: XMLHttpRequest, despuesDeCrear: Function, errorAlCrear: Function) {

        function respuestaCorrecta() {
            if (req.response.IsNullOrEmpty()) {
                errorAlCrear();
            }
            else {
                var resultado: any = ParsearRespuesta(req);
                if (resultado.estado === Ajax.jsonResultError) {
                    errorAlCrear();
                }
                else {
                    despuesDeCrear();
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

    protected DespuesDeCrear(req: XMLHttpRequest): void {
        let resultado = JSON.parse(req.response);
        this.ResultadoPeticion = resultado.mensaje;
        this.PeticionRealizada = true;
        this.Creado = true;
    }

    protected ErrorAlCrear(req: XMLHttpRequest): void {
        if (req.response.IsNullOrEmpty()) {
            this.ResultadoPeticion = `La peticion ${Ajax.EndPoint.Crear} no está definida`;
        }
        else {
            let resultado = JSON.parse(req.response);
            this.ResultadoPeticion = resultado.mensaje;
            this.PeticionRealizada = true;
            console.error(resultado.consola);
        }
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
