namespace ApiDeArchivos {

    export function SubirArchivo(controlador: string, archivo: string, trasSubirArchivo: Function) {
        let url: string = `/${controlador}/${Ajax.EndPoint.SubirArchivo}`;
        let peticion = new ApiDeAjax.DescriptorAjax(Ajax.EndPoint.SubirArchivo
            , this
            , url
            , ApiDeAjax.TipoPeticion.Sincrona
            , ApiDeAjax.ModoPeticion.Put
            , trasSubirArchivo
            , null);

        //  peticion.Ejecutar();
    }

    //export function leer(div: HTMLDivElement, input: HTMLInputElement) {
    //    for (var i = 0; i < input.files.length; i++) {
    //        if (input.files[i]) {
    //            var reader = new FileReader();

    //            reader.onload = function (e) {
    //                var a = document.createElement("img");
    //                div.appendChild(a);
    //                a.setAttribute('src', e.target.result);
    //            };
    //            reader.readAsDataURL(input.files[i]);
    //        }
    //    }
    //}

    export function MostrarPreview() {

        let htmlFicheros: HTMLInputElement = document.getElementById("fichero-icono") as HTMLInputElement;
        let htmlPreview: HTMLDivElement = document.getElementById("preview-icono") as HTMLDivElement;

        let ficheros = htmlFicheros.files;
        for (let i = 0; i < ficheros.length; i++) {
            const file = ficheros[i];

            if (!file.type.startsWith('image/')) { continue; }

            let imagen: HTMLImageElement = document.createElement("img") as HTMLImageElement;
            imagen.classList.add("obj");

            htmlPreview.innerHTML = '';
            htmlPreview.appendChild(imagen);

            const reader = new FileReader();

            reader.onload = (function (imagen: HTMLImageElement) {
                return function (e) {
                    imagen.src = e.target.result;
                };
            })(imagen);

            reader.readAsDataURL(file);
        }

    }

    export function MostrarCanvas() {
        BlanquearMensaje();
        let htmlFicheros: HTMLInputElement = document.getElementById("fichero-icono") as HTMLInputElement;
        let ficheros = htmlFicheros.files;
        
        var img = new Image();
        img.onload = visializarImagen;
        img.onerror = ErrorAlVisializar;
        img.src = URL.createObjectURL(ficheros[0]);
    };

    function visializarImagen() {
        let htmlCanvas: HTMLCanvasElement = document.getElementById('canvas-icono') as HTMLCanvasElement;
        htmlCanvas.width = 100;
        htmlCanvas.height = 100;
        var canvas = htmlCanvas.getContext('2d');
        canvas.drawImage(this, 0, 0, 100,100);
    }

    function ErrorAlVisializar() {
        let htmlCanvas: HTMLCanvasElement = document.getElementById('canvas-icono') as HTMLCanvasElement;
        htmlCanvas.width = htmlCanvas.width;
        Mensaje(TipoMensaje.Error, "El fichero seleccionado no es una imagen");
    }

}