namespace Acceso {

    export let Gestor: GestorDeAcceso = undefined;

    export function AlSalirDeLogin() {
        OcultarMensaje();
        let login: HTMLInputElement = document.getElementById('login') as HTMLInputElement;
        Gestor = new GestorDeAcceso(login.value);
    }

    export function AlPulsarConectar() {
        OcultarMensaje();
        if (Gestor === undefined) {
            MostrarMensaje("Debe indicar un usuario");
        }

        let password: HTMLInputElement = document.getElementById('password') as HTMLInputElement;
        Gestor.ValidarAcceso(password.value);
    }

    function MostrarMensaje(mensaje: string) {
        let divInfoConexion: HTMLDivElement = document.getElementById('div-info-conexion') as HTMLDivElement;
        divInfoConexion.style.display = "flex";

        let infoConexion: HTMLInputElement = document.getElementById('info-conexion') as HTMLInputElement;
        infoConexion.value = mensaje;
    }


    function OcultarMensaje() {
        let infoConexion: HTMLInputElement = document.getElementById('info-conexion') as HTMLInputElement;
        infoConexion.value = "";
        let divInfoConexion: HTMLDivElement = document.getElementById('div-info-conexion') as HTMLDivElement;
        divInfoConexion.style.display = "none";
    }


    export class GestorDeAcceso {

        private _login: string;


        constructor(login: string) {
            this._login = login;
            this.LeerUsuario();
        }


        private LeerUsuario() {
            let restrictor: string = DefinirRestrictorCadena(Ajax.Param.login, this._login);
            let url: string = `/Acceso/${Ajax.EpDeAcceso.ReferenciarFoto}?restrictor=${restrictor}`;

            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EpDeAcceso.ReferenciarFoto
                , null
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.MapearFoto
                , this.SiHayErrorTrasPeticionAjax
            );

            a.Ejecutar();
        }

        private MapearFoto(peticion: ApiDeAjax.DescriptorAjax) {
            let visor: HTMLImageElement = document.getElementById('img-usuario') as HTMLImageElement;
            (peticion.llamador as GestorDeAcceso).MostrarImagenUrl(visor, peticion.resultado.datos);
            let divInfoConexion: HTMLDivElement = document.getElementById('div-info-conexion') as HTMLDivElement;
            divInfoConexion.style.display = "none";
        }

        private MostrarImagenUrl(visor: HTMLImageElement, url: any) {
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

            let idIcono = idCanva.replace('canvas', 'icono');
            let divIcono: HTMLDivElement = document.getElementById(`div-${idIcono}`) as HTMLDivElement;
            divIcono.style.display = "none";

        }


        public ValidarAcceso(password: string) {
           
            let url: string = `/Acceso/${Ajax.EpDeAcceso.ValidarAcceso}`;

            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EpDeAcceso.ValidarAcceso
                , null
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Post
                , null
                , this.SiHayErrorTrasPeticionAjax
            );

            let datosPost = new FormData();
            datosPost.append(Ajax.Param.login, this._login);

            let passwordEncriptada: string = Encriptar(this._login, password);
            datosPost.append(Ajax.Param.password, passwordEncriptada);

            a.DatosPost = datosPost;

            a.Ejecutar();
        }


        private SiHayErrorTrasPeticionAjax(peticion: ApiDeAjax.DescriptorAjax) {
            MostrarMensaje(peticion.resultado.mensaje);
        }


    }
}