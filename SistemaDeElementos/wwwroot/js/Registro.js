var Registro;
(function (Registro) {
    Registro.misRegistros = {
        UsuarioConectado: 'usuario-conectado',
        EsAdministrador: 'administrador'
    };
    class UsuarioDeConexion {
    }
    Registro.UsuarioDeConexion = UsuarioDeConexion;
    function HayUsuarioDeConexion() {
        let u = sessionStorage.getItem(Registro.misRegistros.UsuarioConectado);
        if (u === null || u === undefined)
            return false;
        let uc = UsuarioConectado();
        return uc.id > 0;
    }
    Registro.HayUsuarioDeConexion = HayUsuarioDeConexion;
    function crearUsuarioDeConexion(usuario) {
        let u = new UsuarioDeConexion();
        asignarUsuarioDeConexion(u, usuario);
        return u;
    }
    function asignarUsuarioDeConexion(u, usuario) {
        u.id = Numero(usuario['id']);
        u.login = usuario['login'];
        u.mail = usuario['mail'];
        u.administrador = usuario['administrador'] == 'S';
    }
    function asignarUsuarioNulo(u) {
        u.id = 0;
        u.login = '';
        u.mail = '';
        u.administrador = false;
    }
    function UsuarioConectado() {
        let u = new UsuarioDeConexion();
        try {
            asignarUsuarioDeConexion(u, JSON.parse(sessionStorage.getItem(Registro.misRegistros.UsuarioConectado)));
        }
        catch {
            asignarUsuarioNulo(u);
        }
        return u;
    }
    Registro.UsuarioConectado = UsuarioConectado;
    ;
    function EsAdministrador() {
        return JSON.parse(sessionStorage.getItem(Registro.misRegistros.EsAdministrador));
    }
    Registro.EsAdministrador = EsAdministrador;
    ;
    function RegistrarUsuarioDeConexion(llamador) {
        function RegistrarUsuario(peticion) {
            let usuario = crearUsuarioDeConexion(peticion.resultado.datos);
            sessionStorage.setItem(Registro.misRegistros.UsuarioConectado, JSON.stringify(usuario));
            sessionStorage.setItem(Registro.misRegistros.EsAdministrador, JSON.stringify(usuario.administrador));
            return usuario;
        }
        return new Promise((resolve, reject) => {
            let url = `/${Ajax.Usuarios.ruta}/${Ajax.Usuarios.accion.LeerUsuarioDeConexion}`;
            let a = new ApiDeAjax.DescriptorAjax(llamador, Ajax.Usuarios.accion.LeerUsuarioDeConexion, llamador, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, (peticion) => {
                resolve(RegistrarUsuario(peticion));
            }, () => {
                reject();
            });
            if (!HayUsuarioDeConexion())
                a.Ejecutar();
        });
    }
    Registro.RegistrarUsuarioDeConexion = RegistrarUsuarioDeConexion;
    function EliminarUsuarioDeConexion() {
        sessionStorage.setItem(Registro.misRegistros.UsuarioConectado, '');
        sessionStorage.setItem(Registro.misRegistros.EsAdministrador, '');
    }
    Registro.EliminarUsuarioDeConexion = EliminarUsuarioDeConexion;
})(Registro || (Registro = {}));
//# sourceMappingURL=Registro.js.map