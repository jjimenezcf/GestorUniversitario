var Registro;
(function (Registro) {
    Registro.misRegistros = {
        UsuarioConectado: 'usuario-conectado',
        EsAdministrador: 'administrador'
    };
    class UsuarioDeConexion {
    }
    Registro.UsuarioDeConexion = UsuarioDeConexion;
    function CrearUsuarioDeConexion(usuario) {
        let u = new UsuarioDeConexion();
        u.id = Numero(usuario['id']);
        u.login = usuario['login'];
        return u;
    }
    function UsuarioConectado() {
        return CrearUsuarioDeConexion(JSON.parse(sessionStorage.getItem(Registro.misRegistros.UsuarioConectado)));
    }
    Registro.UsuarioConectado = UsuarioConectado;
    ;
    function EsAdministrador() {
        return JSON.parse(sessionStorage.getItem(Registro.misRegistros.EsAdministrador)) === 'S';
    }
    Registro.EsAdministrador = EsAdministrador;
    ;
    function RegistrarUsuarioDeConexion(llamador) {
        function RegistrarUsuario(peticion) {
            let registro = peticion.resultado.datos;
            sessionStorage.setItem(Registro.misRegistros.UsuarioConectado, JSON.stringify(registro));
            sessionStorage.setItem(Registro.misRegistros.EsAdministrador, JSON.stringify(registro));
        }
        let usuarioConectado = sessionStorage.getItem('usuario-conectado');
        return new Promise((resolve, reject) => {
            let url = `/${Ajax.Usuarios.ruta}/${Ajax.Usuarios.accion.LeerUsuarioDeConexion}`;
            let a = new ApiDeAjax.DescriptorAjax(llamador, Ajax.Usuarios.accion.LeerUsuarioDeConexion, llamador, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, (peticion) => {
                RegistrarUsuario(peticion);
                resolve(usuarioConectado);
            }, () => {
                reject();
            });
            if (usuarioConectado != null)
                resolve(usuarioConectado);
            else
                a.Ejecutar();
        });
    }
    Registro.RegistrarUsuarioDeConexion = RegistrarUsuarioDeConexion;
})(Registro || (Registro = {}));
//# sourceMappingURL=Registro.js.map