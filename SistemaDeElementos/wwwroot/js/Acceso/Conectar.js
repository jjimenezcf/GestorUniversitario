var Acceso;
(function (Acceso) {
    Acceso.Gestor = undefined;
    function AlSalirDeLogin() {
        OcultarMensaje();
        let login = document.getElementById('login');
        Acceso.Gestor = new GestorDeAcceso(login.value);
    }
    Acceso.AlSalirDeLogin = AlSalirDeLogin;
    function AlPulsarConectar() {
        OcultarMensaje();
        if (Acceso.Gestor === undefined) {
            MostrarMensaje("Debe indicar un usuario");
        }
        let password = document.getElementById('password');
        Acceso.Gestor.ValidarAcceso(password.value);
    }
    Acceso.AlPulsarConectar = AlPulsarConectar;
    function MostrarMensaje(mensaje) {
        let divInfoConexion = document.getElementById('div-info-conexion');
        divInfoConexion.style.display = "flex";
        let infoConexion = document.getElementById('info-conexion');
        infoConexion.value = mensaje;
    }
    function OcultarMensaje() {
        let infoConexion = document.getElementById('info-conexion');
        infoConexion.value = "";
        let divInfoConexion = document.getElementById('div-info-conexion');
        divInfoConexion.style.display = "none";
    }
    class GestorDeAcceso {
        constructor(login) {
            this._login = login;
            this.LeerUsuario();
        }
        LeerUsuario() {
            let restrictor = DefinirRestrictorCadena(Ajax.Param.login, this._login);
            let url = `/Acceso/${Ajax.EpDeAcceso.ReferenciarFoto}?restrictor=${restrictor}`;
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.EpDeAcceso.ReferenciarFoto, null, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, this.MapearFoto, this.SiHayErrorTrasPeticionAjax);
            a.Ejecutar();
        }
        MapearFoto(peticion) {
            let visor = document.getElementById('img-usuario');
            peticion.llamador.MostrarImagenUrl(visor, peticion.resultado.datos);
            let divInfoConexion = document.getElementById('div-info-conexion');
            divInfoConexion.style.display = "none";
        }
        MostrarImagenUrl(visor, url) {
            visor.setAttribute('src', url);
            let idCanva = visor.getAttribute(atControl.id).replace('img', 'canvas');
            let htmlCanvas = document.getElementById(idCanva);
            htmlCanvas.width = 90;
            htmlCanvas.height = 90;
            var canvas = htmlCanvas.getContext('2d');
            var img = new Image();
            img.src = url;
            img.onload = function () {
                canvas.drawImage(img, 0, 0, 90, 90);
            };
            let divCambas = document.getElementById(`div-${idCanva}`);
            divCambas.style.display = "block";
            let idIcono = idCanva.replace('canvas', 'icono');
            let divIcono = document.getElementById(`div-${idIcono}`);
            divIcono.style.display = "none";
        }
        ValidarAcceso(password) {
            let url = `/Acceso/${Ajax.EpDeAcceso.ValidarAcceso}`;
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.EpDeAcceso.ValidarAcceso, null, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Post, this.Conectar, this.SiHayErrorTrasPeticionAjax);
            let datosPost = new FormData();
            datosPost.append(Ajax.Param.login, this._login);
            let passwordEncriptada = Encriptar(this._login, password);
            datosPost.append(Ajax.Param.password, passwordEncriptada);
            a.DatosPost = datosPost;
            a.Ejecutar();
        }
        Conectar(peticion) {
            let l = document.getElementById('l');
            l.value = peticion.llamador._login;
            let password = document.getElementById('password');
            let p = document.getElementById('p');
            p.value = Encriptar(l.value, password.value);
            let f = document.getElementById('FormDeConexion');
            f.submit();
        }
        SiHayErrorTrasPeticionAjax(peticion) {
            MostrarMensaje(peticion.resultado.mensaje);
        }
    }
    Acceso.GestorDeAcceso = GestorDeAcceso;
})(Acceso || (Acceso = {}));
//# sourceMappingURL=Conectar.js.map