namespace ApiDeArchivos {

    //export function SubirArchivo(controlador: string, archivo: string, trasSubirArchivo: Function) {
    //    let url: string = `/${controlador}/${Ajax.EndPoint.SubirArchivo}`;
    //    let peticion = new ApiDeAjax.DescriptorAjax(Ajax.EndPoint.SubirArchivo
    //        , this
    //        , url
    //        , ApiDeAjax.TipoPeticion.Sincrona
    //        , ApiDeAjax.ModoPeticion.Put
    //        , trasSubirArchivo
    //        , null);

    //    //  peticion.Ejecutar();
    //}

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

    export function SubirArchivo() {
        let barra = document.getElementById("barra-estado");
        let span = barra.children[0];
        let btnSubir = document.getElementById("subir-archivo");
        let btnCancelar = document.getElementById("cancelar-subir-archivo");

        barra.classList.remove('barra-verde', 'barra-roja');

        let peticion = new XMLHttpRequest();
        peticion.upload.addEventListener("progress", (event) => {
            let porcentaje = Math.round((event.loaded / event.total) * 100);
            Mensaje(TipoMensaje.Info, porcentaje.toString());
            barra.style.width = porcentaje + '%';
            span.innerHTML = porcentaje + '%';
        });

        peticion.addEventListener("load", () => {
            barra.classList.add('barra-verde');
            span.innerHTML = "Proceso completado";
        })

        let htmlFicheros: HTMLInputElement = document.getElementById("fichero-foto") as HTMLInputElement;
        let ficheros = htmlFicheros.files;
        let url: string = `/usuarios/epSubirArchivo`;

        var datos = new FormData();
        datos.append('fichero', ficheros[0]);

        peticion.open('post', url);


        //peticion.setRequestHeader('dataType', 'application/json');
        //peticion.setRequestHeader('Content-Type', 'application/octet-stream');

        peticion.send(datos);

        btnCancelar.addEventListener("click", () => {
            peticion.abort();
            barra.classList.remove('barra-verde');
            barra.classList.add('barra-roja');
            span.innerHTML = "Proceso cancelado";
        })

    }


    export function MostrarCanvas() {
        BlanquearMensaje();
        let htmlFicheros: HTMLInputElement = document.getElementById("fichero-foto") as HTMLInputElement;
        let ficheros = htmlFicheros.files;
        
        var img = new Image();
        img.onload = visializarImagen;
        img.onerror = ErrorAlVisializar;
        img.src = URL.createObjectURL(ficheros[0]);
    };

    function visializarImagen() {
        let htmlCanvas: HTMLCanvasElement = document.getElementById('canvas-foto') as HTMLCanvasElement;
        htmlCanvas.width = 100;
        htmlCanvas.height = 100;
        var canvas = htmlCanvas.getContext('2d');
        canvas.drawImage(this, 0, 0, 100,100);
    }

    function ErrorAlVisializar() {
        let htmlCanvas: HTMLCanvasElement = document.getElementById('canvas-foto') as HTMLCanvasElement;
        htmlCanvas.width = htmlCanvas.width;
        Mensaje(TipoMensaje.Error, "El fichero seleccionado no es una imagen");
    }

}