class CrudCreacionUsuario extends CrudCreacion {

    constructor() {
        super();
        this.divDeCreacionHtml = document.getElementById("crud_usuario_creacion") as HTMLDivElement;
    }

    public InicializarValores() {
        super.InicializarValores();
        let inputFechaAlta: HTMLInputElement = this.divDeCreacionHtml.querySelector<HTMLInputElement>("#Alta");
        let fecha: Date = new Date();
        inputFechaAlta.value = fecha.toDateString();
    }

    protected DespuesDeMapearDatosDeIU(json: JSON): JSON {
        json = super.DespuesDeMapearDatosDeIU(json);

        /*código específico para usuariosDto*/

        return json;
    }

    protected AntesDeMapearDatosDeIU(): JSON {
        let json: JSON = super.AntesDeMapearDatosDeIU();

        /*código específico para usuariosDto*/

        return json;
    }


    //public static Aceptar(): void {
    //    this.Crud = new CrudCreacionUsuario();
    //    super.Aceptar();
    //}


    //public static Cerrar(): void {
    //    let Crud = new CrudCreacionUsuario();
    //    super.Cerrar();
    //}
}