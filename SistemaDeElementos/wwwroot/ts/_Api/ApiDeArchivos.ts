﻿namespace ApiDeArchivos {


    export class DatosPeticionSubirArchivo {
        private _idSelector: string;

        public Selector(): HTMLDivElement {
            return document.getElementById(this._idSelector) as HTMLDivElement;
        }

        constructor(idSelector: string) {
            this._idSelector = idSelector;
        }

    }


    function SubirArchivo(controlador: string, idSelectorDeArchivo: string, idBarra: string) {

        let htmlFicheros: HTMLInputElement = document.getElementById(idSelectorDeArchivo) as HTMLInputElement;
        let ficheros = htmlFicheros.files;

        let url: string = `/${controlador}/${Ajax.EndPoint.SubirArchivo}`;

        let a = new ApiDeAjax.DescriptorAjax(Ajax.EndPoint.SubirArchivo
            , new DatosPeticionSubirArchivo(idSelectorDeArchivo)
            , url
            , ApiDeAjax.TipoPeticion.Asincrona
            , ApiDeAjax.ModoPeticion.Post
            , TrasSubirElArchivo
            , null
        );

        let datosPost = new FormData();
        datosPost.append(Ajax.Param.fichero, ficheros[0]);
        a.DatosPost = datosPost;
        a.IdBarraDeProceso = idBarra;
       
        a.Ejecutar();
    }

    function TrasSubirElArchivo(peticion: ApiDeAjax.DescriptorAjax) {
        let datos: DatosPeticionSubirArchivo = peticion.DatosDeEntrada;
        let selector = datos.Selector();
        selector.removeAttribute(AtributoSelectorArchivo.idArchivo);
        selector.setAttribute(AtributoSelectorArchivo.idArchivo, peticion.resultado.datos);
    }

    export function MostrarCanvas(controlador: string, idSelectorDeArchivo: string, idCambas: string, idBarra: string) {

        function visializarImagen() {
            let htmlCanvas: HTMLCanvasElement = document.getElementById(idCambas) as HTMLCanvasElement;
            htmlCanvas.width = 100;
            htmlCanvas.height = 100;
            var canvas = htmlCanvas.getContext('2d');
            canvas.drawImage(img, 0, 0, 100, 100);
            SubirArchivo(controlador, idSelectorDeArchivo, idBarra);
        }

        function ErrorAlVisializar() {
            let htmlCanvas: HTMLCanvasElement = document.getElementById(idCambas) as HTMLCanvasElement;
            htmlCanvas.width = htmlCanvas.width;
            Mensaje(TipoMensaje.Error, "El fichero seleccionado no es una imagen");
        }

        BlanquearMensaje();
        let htmlFicheros: HTMLInputElement = document.getElementById(idSelectorDeArchivo) as HTMLInputElement;
        let ficheros = htmlFicheros.files;
        
        var img = new Image();
        img.src = URL.createObjectURL(ficheros[0]);

        img.onload = visializarImagen;
        img.onerror = ErrorAlVisializar;
    };


}