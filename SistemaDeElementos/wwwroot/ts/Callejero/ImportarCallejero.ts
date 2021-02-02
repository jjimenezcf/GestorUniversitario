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
            if (super.AntesDeAceptar()) {
                ApiDeArchivos.SubirArchivos(this.CuerpoDelFormulario);
                //let archivos: NodeListOf<HTMLInputElement> = this.CuerpoDelFormulario.querySelectorAll(`[${atControl.tipo}=${TipoControl.Archivo}]`) as NodeListOf<HTMLInputElement>;
                //let procesando: boolean = false;
                ////do {
                //    for (let i: number = 0; i < archivos.length; i++) {
                //        procesando = false;
                //        let idInfoArchivo: string = archivos[i].getAttribute(atArchivo.infoArchivo);
                //        let infoArchivoHtml: HTMLInputElement = document.getElementById(idInfoArchivo) as HTMLInputElement;
                //        let estado: string = infoArchivoHtml.getAttribute(atArchivo.estado);
                //        console.log(`mirando fichero ${idInfoArchivo}`);
                //        if (IsNullOrEmpty(estado) || estado === atArchivo.situacion.sinArchivo)
                //            continue;
                //        if (estado === atArchivo.situacion.subiendo) {
                //            procesando = true;
                //            (async () => {
                //                console.log('esperando');
                //                await delay(1000);
                //            })();
                //            break;
                //        }
                //    }
                ////}
                ////while (procesando);
                //console.log('terminé');
                return false; 
            }
            return false;
        }

    }

}