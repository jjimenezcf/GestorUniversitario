class CrudCreacionUsuario extends CrudCreacion {

    constructor() {
        super();
        this.divDeCreacionHtml = document.getElementById("crud_usuario_creacion") as HTMLDivElement;
    }

    public InicializarValores() {        
        let inputFechaAlta: HTMLInputElement = this.divDeCreacionHtml.querySelector<HTMLInputElement>("#Alta");
        let fecha: Date = new Date();
        inputFechaAlta.value = fecha.toDateString();
    }
}