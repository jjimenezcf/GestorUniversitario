var Cookies;
(function (Cookies) {
    Cookies.Cooky = {
        UsuarioConectado: 'usuario-conectado'
    };
    function Guardar(nombre, valor) {
        document.cookie = `${nombre}=${JSON.stringify(valor)}`;
    }
    Cookies.Guardar = Guardar;
    function LeerCookie(nombre) {
        let lista = document.cookie.split(";");
        let micookie = "";
        let valor = "";
        for (let i = 0; i < lista.length; i++) {
            var busca = lista[i].search(nombre);
            if (busca > -1) {
                micookie = lista[i];
                break;
            }
        }
        if (!IsNullOrEmpty(micookie)) {
            var igual = micookie.indexOf("=");
            valor = micookie.substring(igual + 1);
        }
        return IsNullOrEmpty(valor) ? null : JSON.parse(valor);
    }
    Cookies.LeerCookie = LeerCookie;
    function IdUsuarioConectado() {
        let valor = JSON.parse(Cookies.LeerCookie(Cookies.Cooky.UsuarioConectado));
        return valor[1];
    }
    Cookies.IdUsuarioConectado = IdUsuarioConectado;
    ;
})(Cookies || (Cookies = {}));
//# sourceMappingURL=Cookies.js.map