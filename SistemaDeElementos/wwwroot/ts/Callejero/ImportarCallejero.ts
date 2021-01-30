namespace Callejero {

    export function CrearFormulario(idFormulario: string) {
        Formulario.formulario = new Callejero.ImportarCallejero(idFormulario);
        window.addEventListener("load", function () { Formulario.formulario.Inicializar(); }, false);
    }

    export class ImportarCallejero extends Formulario.Base {

        constructor(idFormulario: string) {
            super(idFormulario);
        }
    }

}