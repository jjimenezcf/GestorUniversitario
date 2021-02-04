namespace Callejero {

    export function CrearFormulario(idFormulario: string) {
        Formulario.formulario = new Callejero.ImportarCallejero(idFormulario);
        window.addEventListener("load", function () { Formulario.formulario.Inicializar(); }, false);
    }

    export class ImportarCallejero extends Formulario.Base {

        constructor(idFormulario: string) {
            super(idFormulario);
        }


        protected AntesDeAceptar(): boolean {
            let someterTrabajo: boolean = true;

            function promesaNoResuelta(form: ImportarCallejero, motivo: string): boolean{
                form.Mensajes.Error(motivo);
                return false;
            }
            function promesaResuelta(form: ImportarCallejero, resultados: string[]) {
                form.Mensajes.Info(`trabajo sometido con ${resultados.length.toString()} ficheros subidos`);
            }

            if (super.AntesDeAceptar()) {
                ApiDeArchivos.PrometoSubirLosArchivos(this.CuerpoDelFormulario)
                    .then(resultados => promesaResuelta(this, resultados))
                    .catch(error => someterTrabajo = promesaNoResuelta(this, error));
            }

            if (someterTrabajo) {
                return this.SometerTrabajo();
            }

            return false;
        }

        private SometerTrabajo(): boolean {
            //llamar al api de trabajos y someter
            //pasar el nombre del trabajo
            //parámetros con los que se ha de someter
            return false;
        }

    }
}