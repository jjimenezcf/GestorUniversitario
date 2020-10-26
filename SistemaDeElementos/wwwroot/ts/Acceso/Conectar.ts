module Acceso {

    export function AlSalirDelEMail() {
        let eMail: HTMLInputElement = document.getElementById('email') as HTMLInputElement;
        LeerUsuario(eMail.value);
    }

    function MostrarImagenUrl(visor: HTMLImageElement, url: any) {
        visor.setAttribute('src', url);
        let idCanva: string = visor.getAttribute(atControl.id).replace('img', 'canvas');
        let htmlCanvas: HTMLCanvasElement = document.getElementById(idCanva) as HTMLCanvasElement;
        htmlCanvas.width = 90;
        htmlCanvas.height = 90;
        var canvas = htmlCanvas.getContext('2d');
        var img = new Image();
        img.src = url;
        img.onload = function () {
            canvas.drawImage(img, 0, 0, 90, 90);
        };

        let divCambas: HTMLDivElement = document.getElementById(`div-${idCanva}`) as HTMLDivElement;
        divCambas.style.display = "block";

        let idIcono = idCanva.replace('canvas','icono')
        let divIcono: HTMLDivElement = document.getElementById(`div-${idIcono}`) as HTMLDivElement;
        divIcono.style.display = "none";

    }

    
    function LeerUsuario(eMail: string) {
        let restrictor: string = DefinirRestrictorCadena("login", eMail);
        let url: string = `/Acceso/${Ajax.EpDeAcceso.ReferenciarFoto}?restrictor=${restrictor}`;

        let a = new ApiDeAjax.DescriptorAjax(this
            , Ajax.EpDeAcceso.ReferenciarFoto
            , null
            , url
            , ApiDeAjax.TipoPeticion.Asincrona
            , ApiDeAjax.ModoPeticion.Get
            , MapearFoto
            , SiHayErrorTrasPeticionAjax
        );

        a.Ejecutar();
    }

    function MapearFoto(peticion: ApiDeAjax.DescriptorAjax) {
        let visor: HTMLImageElement = document.getElementById('img-usuario') as HTMLImageElement;
        MostrarImagenUrl(visor, peticion.resultado.datos);        
    }

    function SiHayErrorTrasPeticionAjax(peticion: ApiDeAjax.DescriptorAjax) {

    }

}