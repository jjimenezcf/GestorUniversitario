class CrudCreacion {

    //protected static Crud: CrudCreacion;

    public divDeCreacionHtml: HTMLDivElement;

    constructor() {
    }

    public InicializarValores(): void {

    }

    public Aceptar(htmlDivMostrar: HTMLDivElement, htmlDivOcultar: HTMLDivElement): void {

        let json: JSON = this.MapearDatosDeIu();
        let persistido: boolean = false;
        try {
            persistido = this.Persistir(json);
        }
        finally {
            if (persistido)
              this.Cerrar(htmlDivMostrar, htmlDivOcultar);
        }

    }


    private Persistir(json: JSON): boolean {
       // throw new Error("fallo al crear");
       return true;
    }

    private MapearDatosDeIu(): JSON {
        let json: JSON = this.AntesDeMapearDatosDeIU();

        /* código estandar de mapeo */

        return this.DespuesDeMapearDatosDeIU(json);
    }

    protected DespuesDeMapearDatosDeIU(json: JSON): JSON {
        return json;
    }

    protected AntesDeMapearDatosDeIU(): JSON {
        return JSON.parse('{ "name":"John", "age":30, "city":"New York"}');
    }

    public Cerrar(htmlDivMostrar: HTMLDivElement, htmlDivOcultar: HTMLDivElement) {

        //* limpiar los controles del panel *//

        htmlDivMostrar.classList.add("div-visible");
        htmlDivMostrar.classList.remove("div-no-visible");

        htmlDivOcultar.classList.add("div-no-visible");
        htmlDivOcultar.classList.remove("div-visible");

        BlanquearMensaje();

    }

    //public static Aceptar(): void {
    //    this.Crud.Aceptar();
    //}

    //public static Cerrar(): void {
    //    this.Crud.Cerrar();
    //}
}
