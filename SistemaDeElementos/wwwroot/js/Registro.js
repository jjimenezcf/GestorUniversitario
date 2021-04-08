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
        return sessionStorage.getItem(Registro.misRegistros.EsAdministrador) !== '';
    }
    Registro.HayUsuarioDeConexion = HayUsuarioDeConexion;
    function crearUsuarioDeConexion(usuario) {
        let u = new UsuarioDeConexion();
        u.id = Numero(usuario['id']);
        u.login = usuario['login'];
        u.administrador = usuario['administrador'] == 'S';
        return u;
    }
    function UsuarioConectado() {
        return crearUsuarioDeConexion(JSON.parse(sessionStorage.getItem(Registro.misRegistros.UsuarioConectado)));
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