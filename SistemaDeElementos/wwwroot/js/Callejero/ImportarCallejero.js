var Callejero;
(function (Callejero) {
    function CrearFormulario(idFormulario) {
        Formulario.formulario = new Callejero.ImportarCallejero(idFormulario);
        window.addEventListener("load", function () { Formulario.formulario.Inicializar(); }, false);
        window.onbeforeunload = function () {
            Formulario.formulario.NavegarDesdeElBrowser();
        };
    }
    Callejero.CrearFormulario = CrearFormulario;
    class Archivo {
    }
    class ImportarCallejero extends Formulario.Base {
        constructor(idFormulario) {
            super(idFormulario);
        }
        AntesDeAceptar() {
            let sometido;
            function promesaNoResuelta(form, motivo) {
                MensajesSe.Error("PrometoSubirLosArchivos", motivo);
                return false;
            }
            if (super.AntesDeAceptar()) {
                PonerCapa();
                ApiDeArchivos.PrometoSubirLosArchivos(this.CuerpoDelFormulario)
                    .then(resultados => sometido = this.ArchivosSubidos(resultados))
                    .catch(error => sometido = promesaNoResuelta(this, error))
                    .finally(() => {
                    QuitarCapa();
                });
            }
            return sometido;
        }
        ArchivosSubidos(resultados) {
            MensajesSe.Info(`trabajo sometido con ${resultados.length.toString()} ficheros subidos`);
            let sometido;
            this.SometerTrabajo()
                .then(resultado => sometido = resultado)
                .catch(resultado => sometido = resultado);
            return sometido;
        }
        SometerTrabajo() {
            let someter = new Promise((resolve, reject) => {
                let arrayDeArchivos = [];
                let archivos = this.CuerpoDelFormulario.querySelectorAll(`[${atControl.tipo}=${TipoControl.Archivo}]`);
                for (let i = 0; i < archivos.length; i++) {
                    let idArchivo = Numero(archivos[i].getAttribute(atArchivo.idArchivo));
                    if (idArchivo > 0) {
                        let archivo = new Archivo();
                        archivo.parametro = archivos[i].getAttribute(atControl.propiedad);
                        archivo.valor = idArchivo;
                        arrayDeArchivos.push(archivo);
                    }
                }
                var parametrosSometer = JSON.stringify(arrayDeArchivos);
                let url = `/${Ajax.Callejero.Importacion}/${Ajax.Callejero.accion.importar}?parametros=${parametrosSometer}`;
                let a = new ApiDeAjax.DescriptorAjax(this, `${Ajax.Callejero.accion.importar}`, arrayDeArchivos, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, (peticion) => {
                    this.TrasSometer(peticion);
                    resolve(true);
                }, (peticion) => {
                    this.SiHayErrorAlSometer(peticion);
                    reject(false);
                });
                a.Ejecutar();
            });
            return someter;
        }
        TrasSometer(peticion) {
            let datos = peticion.DatosDeEntrada;
            MensajesSe.Apilar(MensajesSe.enumTipoMensaje.informativo, `Se ha sometido el trabajo de importación con ${datos.length} archivos`);
        }
        SiHayErrorAlSometer(peticion) {
            let datos = peticion.DatosDeEntrada;
            MensajesSe.Apilar(MensajesSe.enumTipoMensaje.informativo, `Error al someter el trabajo de importación con ${datos.length} archivos`);
        }
    }
    Callejero.ImportarCallejero = ImportarCallejero;
})(Callejero || (Callejero = {}));
//# sourceMappingURL=ImportarCallejero.js.map