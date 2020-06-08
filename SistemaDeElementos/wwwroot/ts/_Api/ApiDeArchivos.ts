namespace ApiDeArchivos {


    export class DatosPeticionSubirArchivo {
        private _idSelector: string;

        public Selector(): HTMLInputElement {
            return document.getElementById(this._idSelector) as HTMLInputElement;
        }

        constructor(idSelector: string) {
            this._idSelector = idSelector;
        }

    }

    export function BlanquearArchivo(archivo: HTMLInputElement) {
        archivo.classList.remove(ClaseCss.crtlNoValido);
        archivo.classList.add(ClaseCss.crtlValido);
        archivo.setAttribute(AtributoSelectorArchivo.idArchivo, '0');
        archivo.files = null;
        let canvasHtml: HTMLCanvasElement = document.getElementById(archivo.getAttribute(AtributoSelectorArchivo.canvasVinculado)) as HTMLCanvasElement;
        canvasHtml.width = canvasHtml.width;
        let imagenHtml: HTMLImageElement = document.getElementById(archivo.getAttribute(AtributoSelectorArchivo.canvasVinculado)) as HTMLImageElement;
        imagenHtml.src = "";
        let barraHtml: HTMLDivElement = document.getElementById(archivo.getAttribute(AtributoSelectorArchivo.barraVinculada)) as HTMLDivElement;
        barraHtml.removeAttribute('style');
        barraHtml.innerHTML = null;
        barraHtml.appendChild(document.createElement("span"));
        barraHtml.classList.remove(ClaseCss.barraVerde);
        barraHtml.classList.remove(ClaseCss.barraRoja);
        barraHtml.classList.add(ClaseCss.barraAzul);
    }

    export function MostrarCanvas(controlador: string, idSelectorDeArchivo: string, idCanva: string, idBarra: string) {

        function visializarImagen() {
            let htmlCanvas: HTMLCanvasElement = document.getElementById(idCanva) as HTMLCanvasElement;
            htmlCanvas.width = 100;
            htmlCanvas.height = 100;
            var canvas = htmlCanvas.getContext('2d');
            canvas.drawImage(img, 0, 0, 100, 100);
            SubirArchivo(controlador, idSelectorDeArchivo, idBarra);
        }

        function ErrorAlVisializar() {
            let htmlCanvas: HTMLCanvasElement = document.getElementById(idCanva) as HTMLCanvasElement;
            htmlCanvas.width = htmlCanvas.width;
            Mensaje(TipoMensaje.Error, "El fichero seleccionado no es una imagen");
        }

        BlanquearMensaje();
        let htmlFicheros: HTMLInputElement = document.getElementById(idSelectorDeArchivo) as HTMLInputElement;
        let ficheros = htmlFicheros.files;

        let filePath: string = ficheros[0].name;
        let extensiones: string = htmlFicheros.getAttribute(AtributoSelectorArchivo.extensionesValidas);

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

    export function InicializarCanvases(panel: HTMLDivElement) {
        let canvases: NodeListOf<HTMLCanvasElement> = panel.querySelectorAll("canvas") as NodeListOf<HTMLCanvasElement>;
        canvases.forEach((canvas) => { canvas.width = canvas.width; });
    }

    function SubirArchivo(controlador: string, idSelectorDeArchivo: string, idBarra: string) {

        let htmlFicheros: HTMLInputElement = document.getElementById(idSelectorDeArchivo) as HTMLInputElement;
        let ficheros = htmlFicheros.files;

        let url: string = `/${controlador}/${Ajax.EndPoint.SubirArchivo}`;

        let a = new ApiDeAjax.DescriptorAjax(this
            , Ajax.EndPoint.SubirArchivo
            , new DatosPeticionSubirArchivo(idSelectorDeArchivo)
            , url
            , ApiDeAjax.TipoPeticion.Asincrona
            , ApiDeAjax.ModoPeticion.Post
            , TrasSubirElArchivo
            , SiHayErrorAlSubirElArchivo
        );

        let datosPost = new FormData();
        datosPost.append(Ajax.Param.fichero, ficheros[0]);

        let rutaDestino: string = htmlFicheros.getAttribute(AtributoSelectorArchivo.rutaDestino);
        datosPost.append(Ajax.Param.rutaDestino, rutaDestino);

        let extensionesValidas: string = htmlFicheros.getAttribute(AtributoSelectorArchivo.extensionesValidas);
        datosPost.append(Ajax.Param.extensiones, extensionesValidas);

        a.DatosPost = datosPost;
        a.IdBarraDeProceso = idBarra;

        a.Ejecutar();
    }

    function TrasSubirElArchivo(peticion: ApiDeAjax.DescriptorAjax) {
        let datos: DatosPeticionSubirArchivo = peticion.DatosDeEntrada;
        let selector: HTMLInputElement = datos.Selector();
        selector.removeAttribute(AtributoSelectorArchivo.idArchivo);
        selector.setAttribute(AtributoSelectorArchivo.idArchivo, peticion.resultado.datos);
    }

    function SiHayErrorAlSubirElArchivo(peticion: ApiDeAjax.DescriptorAjax) {
        let datos: DatosPeticionSubirArchivo = peticion.DatosDeEntrada;
        let selector: HTMLInputElement = datos.Selector();
        BlanquearArchivo(selector);
        Mensaje(TipoMensaje.Error, peticion.resultado.mensaje);
    }

}