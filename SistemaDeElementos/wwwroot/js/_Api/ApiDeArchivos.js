var ApiDeArchivos;
(function (ApiDeArchivos) {
    class DatosPeticionSubirArchivo {
        constructor(idArchivo) {
            this._idArchivo = idArchivo;
        }
        Archivo() {
            return document.getElementById(this._idArchivo);
        }
    }
    ApiDeArchivos.DatosPeticionSubirArchivo = DatosPeticionSubirArchivo;
    function BlanquearArchivo(archivo, blanquearImagen) {
        archivo.setAttribute(atArchivo.idArchivo, '0');
        archivo.value = "";
        archivo.removeAttribute(atArchivo.idArchivo);
        archivo.removeAttribute(atArchivo.nombre);
        archivo.classList.remove(ClaseCss.crtlNoValido);
        archivo.classList.add(ClaseCss.crtlValido);
        InicializarBarra(archivo);
        BlanquearInfoArchivo(archivo);
        if (EsImagen(archivo) && blanquearImagen) {
            BlanquearImagen(archivo);
        }
    }
    ApiDeArchivos.BlanquearArchivo = BlanquearArchivo;
    function PrometoSubirLosArchivos(htmlPanel) {
        const promesas = [];
        let archivos = htmlPanel.querySelectorAll(`[${atControl.tipo}=${TipoControl.Archivo}]`);
        BlanquearEstado(archivos);
        for (let i = 0; i < archivos.length; i++) {
            if (archivos[i].files.length > 0) {
                let idArchivo = archivos[i].getAttribute(literal.id);
                let controlador = archivos[i].getAttribute(atArchivo.controlador);
                let promesa = PrometoSubirElArchivo(controlador, idArchivo);
                promesas.push(promesa);
            }
            else {
                CambiarEstado(archivos[i], atArchivo.situacion.sinArchivo);
            }
        }
        return Promise.all(promesas);
    }
    ApiDeArchivos.PrometoSubirLosArchivos = PrometoSubirLosArchivos;
    function BlanquearEstado(archivos) {
        for (let i = 0; i < archivos.length; i++) {
            CambiarEstado(archivos[i], atArchivo.situacion.pendiente);
        }
    }
    function SeleccionarImagen(idArchivo) {
        let inputFile = document.getElementById(idArchivo);
        if (inputFile) {
            inputFile.click();
        }
    }
    ApiDeArchivos.SeleccionarImagen = SeleccionarImagen;
    function MostrarCanvas(controlador, idArchivo, idCanva) {
        function visializarImagen() {
            let htmlCanvas = document.getElementById(idCanva);
            htmlCanvas.width = 100;
            htmlCanvas.height = 100;
            var canvas = htmlCanvas.getContext('2d');
            canvas.drawImage(img, 0, 0, 100, 100);
            PrometoSubirElArchivo(controlador, idArchivo)
                .then()
                .catch(() => {
                let archivo = document.getElementById(idArchivo);
                BlanquearImagen(archivo);
            });
        }
        function ErrorAlVisializar() {
            ApiDeArchivos.BlanquearArchivo(archivo, true);
            MensajesSe.Error("ErrorAlVisializar", "Fichero no válido para mostrar en un Canvas");
        }
        BlanquearMensaje();
        let archivo = document.getElementById(idArchivo);
        InicializarBarra(archivo);
        let ficheros = archivo.files;
        let filePath = ficheros[0].name;
        let extensiones = archivo.getAttribute(atArchivo.extensionesValidas);
        var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();
        if (extensiones.indexOf(ext) < 0) {
            MensajesSe.Error("MostrarCanvas", `Extensión no valida, sólo se permite extensiones del tipo '${extensiones}'`);
            return;
        }
        var img = new Image();
        img.src = URL.createObjectURL(ficheros[0]);
        img.onload = visializarImagen;
        img.onerror = ErrorAlVisializar;
    }
    ApiDeArchivos.MostrarCanvas = MostrarCanvas;
    ;
    function SeleccionarArchivo(idSelector) {
        let inputFile = document.getElementById(idSelector);
        if (inputFile) {
            inputFile.click();
        }
    }
    ApiDeArchivos.SeleccionarArchivo = SeleccionarArchivo;
    function MostrarArchivo(idArchivo, idInfoArchivo) {
        BlanquearMensaje();
        let archivo = document.getElementById(idArchivo);
        if (archivo.files === undefined || archivo.files.length === 0 || IsNullOrEmpty(archivo.files[0].name)) {
            BlanquearInfoArchivo(archivo);
            return;
        }
        let ficheros = archivo.files;
        InicializarBarra(archivo);
        let filePath = ficheros[0].name;
        let extensiones = archivo.getAttribute(atArchivo.extensionesValidas);
        let limite = Numero(archivo.getAttribute(atArchivo.limiteEnByte));
        var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();
        if (extensiones.indexOf(ext) < 0) {
            MensajesSe.Error("MostrarArchivo", `Extensión no valida, sólo se permite extensiones del tipo '${extensiones}'`);
            return;
        }
        if (limite > 0 && limite < ficheros[0].size) {
            MensajesSe.Error("MostrarArchivo", `Tamaño del fichero demasiado grande, el límite es ${limite} bytes`);
            return;
        }
        let infoArchivo = document.getElementById(idInfoArchivo);
        infoArchivo.style.display = "block";
        infoArchivo.value = `${filePath} (${ficheros[0].size} bytes, ${ficheros[0].type} )`;
    }
    ApiDeArchivos.MostrarArchivo = MostrarArchivo;
    ;
    function PrometoSubirElArchivo(controlador, idArchivo) {
        return new Promise((resolve, reject) => {
            let archivo = document.getElementById(idArchivo);
            let ficheros = archivo.files;
            let url = `/${controlador}/${Ajax.EndPoint.SubirArchivo}`;
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.EndPoint.SubirArchivo, new DatosPeticionSubirArchivo(idArchivo), url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Post, (peticion) => {
                TrasSubirElArchivo(peticion);
                resolve(`el archivo ${idArchivo} ha subido`);
            }, (peticion) => {
                SiHayErrorAlSubirElArchivo(peticion);
                let etiqueta = document.getElementById(`${idArchivo}.ref`);
                reject(`el archivo '${etiqueta !== null && etiqueta !== undefined ? etiqueta.innerText : idArchivo}' no se ha podido subir, el trabajo no será sometido`);
            });
            let datosPost = new FormData();
            datosPost.append(Ajax.Param.fichero, ficheros[0]);
            let rutaDestino = archivo.getAttribute(atArchivo.rutaDestino);
            datosPost.append(Ajax.Param.rutaDestino, IsNullOrEmpty(rutaDestino) ? '' : rutaDestino);
            let extensionesValidas = archivo.getAttribute(atArchivo.extensionesValidas);
            datosPost.append(Ajax.Param.extensiones, extensionesValidas);
            a.DatosPost = datosPost;
            DefinirBarraDeProceso(a, archivo);
            CambiarEstado(archivo, atArchivo.situacion.subiendo);
            a.Ejecutar();
        });
    }
    function DefinirBarraDeProceso(descriptor, archivo) {
        let infoArchivo = document.getElementById(archivo.getAttribute(atArchivo.infoArchivo));
        infoArchivo.style.display = "none";
        let barraHtml = document.getElementById(archivo.getAttribute(atArchivo.barra));
        let span = barraHtml.children[0];
        barraHtml.classList.remove(ClaseCss.barraVerde, ClaseCss.barraRoja);
        barraHtml.classList.add(ClaseCss.barraAzul);
        descriptor.Request.upload.addEventListener("progress", (event) => {
            let porcentaje = Math.round((event.loaded / event.total) * 100);
            barraHtml.style.width = porcentaje + '%';
            span.innerHTML = porcentaje + '%';
        });
        HacerVisibleLaBarra(barraHtml, true);
    }
    function HacerVisibleLaBarra(barraHtml, mostrar) {
        let idContenedorBarra = barraHtml.getAttribute(atArchivo.contenedorBarra);
        if (!IsNullOrEmpty(idContenedorBarra)) {
            let contenedorBarraHtml = document.getElementById(idContenedorBarra);
            contenedorBarraHtml.style.display = mostrar ? "block" : "none";
        }
        barraHtml.style.display = mostrar ? "block" : "none";
    }
    function CambiarEstado(archivo, situacion) {
        let idInfoArchivo = archivo.getAttribute(atArchivo.infoArchivo);
        let infoArchivoHtml = document.getElementById(idInfoArchivo);
        infoArchivoHtml.setAttribute(atArchivo.estado, situacion);
    }
    function TrasSubirElArchivo(peticion) {
        let datos = peticion.DatosDeEntrada;
        let archivo = datos.Archivo();
        CambiarEstado(archivo, atArchivo.situacion.subido);
        BlanquearArchivo(archivo, false);
        VisualizarBarraDeOk(archivo);
        let tipo = archivo.getAttribute(atControl.tipo);
        if (tipo === TipoControl.Archivo)
            archivo.setAttribute(atArchivo.idArchivo, peticion.resultado.datos);
        if (tipo === TipoControl.UrlDeArchivo)
            archivo.setAttribute(atArchivo.nombre, peticion.resultado.datos);
    }
    function SiHayErrorAlSubirElArchivo(peticion) {
        let datos = peticion.DatosDeEntrada;
        let archivo = datos.Archivo();
        CambiarEstado(archivo, atArchivo.situacion.error);
        BlanquearArchivo(archivo, true);
        VisualizarBarraDeError(archivo);
        //Mensaje(MensajesSe.enumTipoMensaje.error, peticion.resultado.mensaje);
    }
    function BlanquearImagen(archivo) {
        let canvasHtml = document.getElementById(archivo.getAttribute(atArchivo.canvas));
        canvasHtml.width = canvasHtml.width;
        let imagenHtml = document.getElementById(archivo.getAttribute(atArchivo.canvas));
        imagenHtml.src = "";
    }
    function VisualizarBarraDeOk(archivo) {
        let barraHtml = document.getElementById(archivo.getAttribute(atArchivo.barra));
        barraHtml.classList.remove(ClaseCss.barraRoja);
        barraHtml.classList.remove(ClaseCss.barraAzul);
        barraHtml.classList.add(ClaseCss.barraVerde);
        let span = barraHtml.children[0];
        span.innerHTML = "Proceso completado";
        HacerVisibleLaBarra(barraHtml, true);
        let infoArchivo = document.getElementById(archivo.getAttribute(atArchivo.infoArchivo));
        infoArchivo.style.display = "none";
    }
    function VisualizarBarraDeError(archivo) {
        let barraHtml = document.getElementById(archivo.getAttribute(atArchivo.barra));
        barraHtml.classList.remove(ClaseCss.barraVerde);
        barraHtml.classList.remove(ClaseCss.barraAzul);
        barraHtml.classList.add(ClaseCss.barraRoja);
        let span = barraHtml.children[0];
        span.innerHTML = "Error al subir el fichero";
        HacerVisibleLaBarra(barraHtml, true);
        let infoArchivo = document.getElementById(archivo.getAttribute(atArchivo.infoArchivo));
        infoArchivo.style.display = "none";
    }
    function InicializarBarra(archivo) {
        let barraHtml = document.getElementById(archivo.getAttribute(atArchivo.barra));
        HacerVisibleLaBarra(barraHtml, false);
        barraHtml.classList.remove(ClaseCss.barraVerde);
        barraHtml.classList.remove(ClaseCss.barraRoja);
        barraHtml.classList.add(ClaseCss.barraAzul);
    }
    function BlanquearInfoArchivo(archivo) {
        let idInfoArchivo = archivo.getAttribute(atArchivo.infoArchivo);
        let infoArchivoHtml = document.getElementById(idInfoArchivo);
        infoArchivoHtml.value = "";
        if (!EsImagen(archivo))
            infoArchivoHtml.style.display = "block";
    }
    function EsImagen(archivo) {
        return archivo.getAttribute(atArchivo.canvas) !== null;
    }
})(ApiDeArchivos || (ApiDeArchivos = {}));
//# sourceMappingURL=ApiDeArchivos.js.map