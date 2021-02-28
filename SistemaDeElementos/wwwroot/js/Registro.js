var Registro;
(function (Registro) {
    Registro.misRegistros = {
        UsuarioConectado: 'usuario-conectado'
    };
    function UsuarioConectado() {
        return JSON.parse(sessionStorage.getItem('usuario-conectado'));
    }
    Registro.UsuarioConectado = UsuarioConectado;
    ;
    function RegistrarUsuarioDeConexion(llamador) {
        function RegistrarUsuario(peticion) {
            let registro = peticion.resultado.datos;
            sessionStorage.setItem('usuario-conectado', JSON.stringify(registro));
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