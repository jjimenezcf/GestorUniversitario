﻿namespace Callejero {

    export function CrearFormulario(idFormulario: string) {
        Formulario.formulario = new Callejero.ImportarCallejero(idFormulario);
        window.addEventListener("load", function () { Formulario.formulario.Inicializar(); }, false);
    }

    class Archivo {
        parametro: string;
        valor: number;
    }

    export class ImportarCallejero extends Formulario.Base {

        constructor(idFormulario: string) {
            super(idFormulario);
        }

        protected AntesDeAceptar(): boolean {
            let sometido: boolean;

            function promesaNoResuelta(form: ImportarCallejero, motivo: string): boolean {
                form.Mensajes.Error(motivo);
                return false;
            }

            if (super.AntesDeAceptar()) {
                PonerCapa();
                ApiDeArchivos.PrometoSubirLosArchivos(this.CuerpoDelFormulario)
                    .then(resultados => sometido = this.ArchivosSubidos(this, resultados))
                    .catch(error => sometido = promesaNoResuelta(this, error))
                    .finally(() => {
                        QuitarCapa();
                    });
            }


            return sometido;
        }

        private ArchivosSubidos(form: ImportarCallejero, resultados: string[]): boolean {
            form.Mensajes.Info(`trabajo sometido con ${resultados.length.toString()} ficheros subidos`);
            let sometido: boolean;
            this.SometerTrabajo()
                .then(resultado => sometido = resultado)
                .catch(resultado => sometido = resultado);
            return sometido;
        }

        public SometerTrabajo(): Promise<boolean> {

            let someter: Promise<boolean> = new Promise((resolve, reject) => {
                let arrayDeArchivos: Archivo[] = [];

                let archivos: NodeListOf<HTMLInputElement> = this.CuerpoDelFormulario.querySelectorAll(`[${atControl.tipo}=${TipoControl.Archivo}]`) as NodeListOf<HTMLInputElement>;
                for (let i = 0; i < archivos.length; i++) {
                    let idArchivo = Numero(archivos[i].getAttribute(atArchivo.idArchivo));
                    if (idArchivo > 0) {
                        let archivo: Archivo = new Archivo();
                        archivo.parametro = archivos[i].getAttribute(literal.id);
                        archivo.valor = idArchivo;
                        arrayDeArchivos.push(archivo);
                    }
                }
                var parametrosSometer = JSON.stringify(arrayDeArchivos);

                let url: string = `/${Ajax.Callejero.Importacion}/${Ajax.Callejero.accion.importar}?parametros=${parametrosSometer}`;

                let a = new ApiDeAjax.DescriptorAjax(this
                    , `${Ajax.Callejero.accion.importar}`
                    , arrayDeArchivos
                    , url
                    , ApiDeAjax.TipoPeticion.Asincrona
                    , ApiDeAjax.ModoPeticion.Get
                    , (peticion) => {
                        this.TrasSometer(peticion);
                        resolve(true);
                    }
                    , (peticion) => {
                        this.SiHayErrorAlSometer(peticion);
                        reject(false);
                    }
                );
                a.Ejecutar();
            });

            return someter;
        }


        private TrasSometer(peticion: ApiDeAjax.DescriptorAjax) {
            let datos: Archivo[] = peticion.DatosDeEntrada;
            Mensaje(TipoMensaje.Info, `Se ha sometido el trabajo de importación con ${datos.length} archivos`);
        }

        private SiHayErrorAlSometer(peticion: ApiDeAjax.DescriptorAjax) {
            let datos: Archivo[] = peticion.DatosDeEntrada;
            Mensaje(TipoMensaje.Info, `Error al someter el trabajo de importación con ${datos.length} archivos`);
        }
    }



}