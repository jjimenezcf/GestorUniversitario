var Callejero;
(function (Callejero) {
    function CrearFormulario(idFormulario) {
        Formulario.formulario = new Callejero.ImportarCallejero(idFormulario);
        window.addEventListener("load", function () { Formulario.formulario.Inicializar(); }, false);
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
                form.Mensajes.Error(motivo);
                return false;
            }
            if (super.AntesDeAceptar()) {
                ApiDeArchivos.PrometoSubirLosArchivos(this.CuerpoDelFormulario)
                    .then(resultados => sometido = this.ArchivosSubidos(this, resultados))
                    .catch(error => sometido = promesaNoResuelta(this, error));
            }
            return sometido;
        }
        ArchivosSubidos(form, resultados) {
            form.Mensajes.Info(`trabajo sometido con ${resultados.length.toString()} ficheros subidos`);
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
                        archivo.parametro = archivos[i].getAttribute(literal.id);
                        archivo.valor = idArchivo;
                        arrayDeArchivos.push(archivo);
                    }
                }
                var parametrosSometer = JSON.stringify(arrayDeArchivos);
                let url = `/importarCallejero/epImportarCallejero?parametros=${parametrosSometer}`;
                let a = new ApiDeAjax.DescriptorAjax(this, 'epSometerImportacion', arrayDeArchivos, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, (peticion) => {
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
            Mensaje(TipoMensaje.Info, `Se ha sometido el trabajo de importación con ${datos.length} archivos`);
        }
        SiHayErrorAlSometer(peticion) {
            let datos = peticion.DatosDeEntrada;
            Mensaje(TipoMensaje.Info, `Error al someter el trabajo de importación con ${datos.length} archivos`);
        }
    }
    Callejero.ImportarCallejero = ImportarCallejero;
})(Callejero || (Callejero = {}));
//# sourceMappingURL=ImportarCallejero.js.map