namespace ApiDeArchivos {


    export class DatosPeticionSubirArchivo {
        private _idArchivo: string;

        public Archivo(): HTMLInputElement {
            return document.getElementById(this._idArchivo) as HTMLInputElement;
        }

        constructor(idArchivo: string) {
            this._idArchivo = idArchivo;
        }
    }

    export function BlanquearArchivo(archivo: HTMLInputElement, blanquearImagen: boolean) {
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


    export function PrometoSubirLosArchivos(htmlPanel: HTMLDivElement): Promise<string[]> {

        const promesas: Promise<string>[] = [];

        let archivos: NodeListOf<HTMLInputElement> = htmlPanel.querySelectorAll(`[${atControl.tipo}=${TipoControl.Archivo}]`) as NodeListOf<HTMLInputElement>;
        BlanquearEstado(archivos);

        for (let i: number = 0; i < archivos.length; i++) {
            if (archivos[i].files.length > 0) {
                let idArchivo: string = archivos[i].getAttribute(literal.id);
                let controlador: string = archivos[i].getAttribute(atArchivo.controlador);

                let promesa: Promise<string> = PrometoSubirElArchivo(controlador, idArchivo)

                promesas.push(promesa);
            }
            else {
                CambiarEstado(archivos[i], atArchivo.situacion.sinArchivo);
            }
        }
        return Promise.all(promesas);
    }

    function BlanquearEstado(archivos: NodeListOf<HTMLInputElement>) {
        for (let i: number = 0; i < archivos.length; i++) {
            CambiarEstado(archivos[i], atArchivo.situacion.pendiente);
        }
    }

    export function SeleccionarImagen(idArchivo: string) {
        let inputFile: HTMLDivElement = document.getElementById(idArchivo) as HTMLDivElement;
        if (inputFile) {
            inputFile.click();
        }
    }

    export function MostrarCanvas(controlador: string, idArchivo: string, idCanva: string) {

        function visializarImagen() {
            let htmlCanvas: HTMLCanvasElement = document.getElementById(idCanva) as HTMLCanvasElement;
            htmlCanvas.width = 100;
            htmlCanvas.height = 100;
            var canvas = htmlCanvas.getContext('2d');
            canvas.drawImage(img, 0, 0, 100, 100);
            PrometoSubirElArchivo(controlador, idArchivo)
                .then()
                .catch(() => {
                    let archivo: HTMLInputElement = document.getElementById(idArchivo) as HTMLInputElement
                    BlanquearImagen(archivo);
                });
        }

        function ErrorAlVisializar() {
            ApiDeArchivos.BlanquearArchivo(archivo, true);
            Mensaje(TipoMensaje.Error, "Fichero no válido para mostrar en un Canvas");
        }

        BlanquearMensaje();
        let archivo: HTMLInputElement = document.getElementById(idArchivo) as HTMLInputElement;
        InicializarBarra(archivo);
        let ficheros = archivo.files;

        let filePath: string = ficheros[0].name;
        let extensiones: string = archivo.getAttribute(atArchivo.extensionesValidas);

        var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();
        if (extensiones.indexOf(ext) < 0) {
            Mensaje(TipoMensaje.Error, `Extensión no valida, sólo se permite extensiones del tipo '${extensiones}'`);
            return;
        }

        var img = new Image();
        img.src = URL.createObjectURL(ficheros[0]);

        img.onload = visializarImagen;
        img.onerror = ErrorAlVisializar;
    };

    export function SeleccionarArchivo(idSelector: string) {
        let inputFile: HTMLDivElement = document.getElementById(idSelector) as HTMLDivElement;
        if (inputFile) {
            inputFile.click();
        }
    }

    export function MostrarArchivo(idArchivo: string, idInfoArchivo: string): void {

        BlanquearMensaje();
        let archivo: HTMLInputElement = document.getElementById(idArchivo) as HTMLInputElement;
        if (archivo.files === undefined || archivo.files.length === 0 || IsNullOrEmpty(archivo.files[0].name)) {
            BlanquearInfoArchivo(archivo);
            return;
        }

        let ficheros = archivo.files;
        InicializarBarra(archivo);

        let filePath: string = ficheros[0].name;
        let extensiones: string = archivo.getAttribute(atArchivo.extensionesValidas);
        let limite: number = Numero(archivo.getAttribute(atArchivo.limiteEnByte));

        var ext = filePath.substring(filePath.lastIndexOf('.') + 1).toLowerCase();
        if (extensiones.indexOf(ext) < 0) {
            Mensaje(TipoMensaje.Error, `Extensión no valida, sólo se permite extensiones del tipo '${extensiones}'`);
            return;
        }
        if (limite > 0 && limite < ficheros[0].size) {
            Mensaje(TipoMensaje.Error, `Tamaño del fichero demasiado grande, el límite es ${limite} bytes`);
            return;
        }

        let infoArchivo: HTMLInputElement = document.getElementById(idInfoArchivo) as HTMLInputElement;
        infoArchivo.style.display = "block";
        infoArchivo.value = `${filePath} (${ficheros[0].size} bytes, ${ficheros[0].type} )`;
    };

    function PrometoSubirElArchivo(controlador: string, idArchivo: string): Promise<string> {

        return new Promise((resolve, reject) => {

            let archivo: HTMLInputElement = document.getElementById(idArchivo) as HTMLInputElement;
            let ficheros = archivo.files;

            let url: string = `/${controlador}/${Ajax.EndPoint.SubirArchivo}`;

            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EndPoint.SubirArchivo
                , new DatosPeticionSubirArchivo(idArchivo)
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Post
                , (peticion) => {
                    TrasSubirElArchivo(peticion);
                    resolve(`el archivo ${idArchivo} ha subido`);
                }
                , (peticion) => {
                    SiHayErrorAlSubirElArchivo(peticion);
                    let etiqueta: HTMLElement = document.getElementById(`${idArchivo}.ref`);

                    reject(`el archivo '${etiqueta !== null && etiqueta !== undefined ? etiqueta.innerText : idArchivo}' no se ha podido subir, el trabajo no será sometido`);
                }
            );

            let datosPost = new FormData();
            datosPost.append(Ajax.Param.fichero, ficheros[0]);

            let rutaDestino: string = archivo.getAttribute(atArchivo.rutaDestino);
            datosPost.append(Ajax.Param.rutaDestino, IsNullOrEmpty(rutaDestino) ? '' : rutaDestino);

            let extensionesValidas: string = archivo.getAttribute(atArchivo.extensionesValidas);
            datosPost.append(Ajax.Param.extensiones, extensionesValidas);
            a.DatosPost = datosPost;
            DefinirBarraDeProceso(a, archivo);
            CambiarEstado(archivo, atArchivo.situacion.subiendo);
            a.Ejecutar();
        });
    }

    function DefinirBarraDeProceso(descriptor: ApiDeAjax.DescriptorAjax, archivo: HTMLInputElement) {
        let infoArchivo: HTMLInputElement = document.getElementById(archivo.getAttribute(atArchivo.infoArchivo)) as HTMLInputElement;
        infoArchivo.style.display = "none";
        let barraHtml: HTMLDivElement = document.getElementById(archivo.getAttribute(atArchivo.barra)) as HTMLDivElement;
        let span: Element = barraHtml.children[0];
        barraHtml.classList.remove(ClaseCss.barraVerde, ClaseCss.barraRoja);
        barraHtml.classList.add(ClaseCss.barraAzul);
        descriptor.Request.upload.addEventListener("progress", (event) => {
            let porcentaje = Math.round((event.loaded / event.total) * 100);
            barraHtml.style.width = porcentaje + '%';
            span.innerHTML = porcentaje + '%';
        });
        HacerVisibleLaBarra(barraHtml, true);
    }

    function HacerVisibleLaBarra(barraHtml: HTMLDivElement, mostrar: boolean) {
        let idContenedorBarra = barraHtml.getAttribute(atArchivo.contenedorBarra);
        if (!IsNullOrEmpty(idContenedorBarra)) {
            let contenedorBarraHtml: HTMLDivElement = document.getElementById(idContenedorBarra) as HTMLDivElement;
            contenedorBarraHtml.style.display = mostrar ? "block" : "none";
        }
        barraHtml.style.display = mostrar ? "block" : "none";
    }

    function CambiarEstado(archivo: HTMLInputElement, situacion: string) {
        let idInfoArchivo: string = archivo.getAttribute(atArchivo.infoArchivo);
        let infoArchivoHtml: HTMLInputElement = document.getElementById(idInfoArchivo) as HTMLInputElement;
        infoArchivoHtml.setAttribute(atArchivo.estado, situacion);
    }

    function TrasSubirElArchivo(peticion: ApiDeAjax.DescriptorAjax) {
        let datos: DatosPeticionSubirArchivo = peticion.DatosDeEntrada;
        let archivo: HTMLInputElement = datos.Archivo();
        CambiarEstado(archivo, atArchivo.situacion.subido);
        BlanquearArchivo(archivo, false);
        VisualizarBarraDeOk(archivo);
        let tipo: string = archivo.getAttribute(atControl.tipo);
        if (tipo === TipoControl.Archivo)
            archivo.setAttribute(atArchivo.idArchivo, peticion.resultado.datos);
        if (tipo === TipoControl.UrlDeArchivo)
            archivo.setAttribute(atArchivo.nombre, peticion.resultado.datos);
    }

    function SiHayErrorAlSubirElArchivo(peticion: ApiDeAjax.DescriptorAjax) {
        let datos: DatosPeticionSubirArchivo = peticion.DatosDeEntrada;
        let archivo: HTMLInputElement = datos.Archivo();
        CambiarEstado(archivo, atArchivo.situacion.error);
        BlanquearArchivo(archivo, true);
        VisualizarBarraDeError(archivo);
        //Mensaje(TipoMensaje.Error, peticion.resultado.mensaje);
    }

    function BlanquearImagen(archivo: HTMLInputElement): void {
        let canvasHtml: HTMLCanvasElement = document.getElementById(archivo.getAttribute(atArchivo.canvas)) as HTMLCanvasElement;
        canvasHtml.width = canvasHtml.width;
        let imagenHtml: HTMLImageElement = document.getElementById(archivo.getAttribute(atArchivo.canvas)) as HTMLImageElement;
        imagenHtml.src = "";
    }

    function VisualizarBarraDeOk(archivo: HTMLInputElement): void {
        let barraHtml: HTMLDivElement = document.getElementById(archivo.getAttribute(atArchivo.barra)) as HTMLDivElement;
        barraHtml.classList.remove(ClaseCss.barraRoja);
        barraHtml.classList.remove(ClaseCss.barraAzul);
        barraHtml.classList.add(ClaseCss.barraVerde);
        let span: Element = barraHtml.children[0];
        span.innerHTML = "Proceso completado";
        HacerVisibleLaBarra(barraHtml, true);
        let infoArchivo: HTMLInputElement = document.getElementById(archivo.getAttribute(atArchivo.infoArchivo)) as HTMLInputElement;
        infoArchivo.style.display = "none";
    }

    function VisualizarBarraDeError(archivo: HTMLInputElement): void {
        let barraHtml: HTMLDivElement = document.getElementById(archivo.getAttribute(atArchivo.barra)) as HTMLDivElement;
        barraHtml.classList.remove(ClaseCss.barraVerde);
        barraHtml.classList.remove(ClaseCss.barraAzul);
        barraHtml.classList.add(ClaseCss.barraRoja);
        let span: Element = barraHtml.children[0];
        span.innerHTML = "Error al subir el fichero";
        HacerVisibleLaBarra(barraHtml, true);
        let infoArchivo: HTMLInputElement = document.getElementById(archivo.getAttribute(atArchivo.infoArchivo)) as HTMLInputElement;
        infoArchivo.style.display = "none";
    }

    function InicializarBarra(archivo: HTMLInputElement): void {
        let barraHtml: HTMLDivElement = document.getElementById(archivo.getAttribute(atArchivo.barra)) as HTMLDivElement;
        HacerVisibleLaBarra(barraHtml, false);
        barraHtml.classList.remove(ClaseCss.barraVerde);
        barraHtml.classList.remove(ClaseCss.barraRoja);
        barraHtml.classList.add(ClaseCss.barraAzul);
    }

    function BlanquearInfoArchivo(archivo: HTMLInputElement): void {
        let idInfoArchivo: string = archivo.getAttribute(atArchivo.infoArchivo);
        let infoArchivoHtml: HTMLInputElement = document.getElementById(idInfoArchivo) as HTMLInputElement;
        infoArchivoHtml.value = "";
        if (!EsImagen(archivo))
            infoArchivoHtml.style.display = "block";
    }

    function EsImagen(archivo: HTMLInputElement): boolean {
        return archivo.getAttribute(atArchivo.canvas) !== null;
    }

}