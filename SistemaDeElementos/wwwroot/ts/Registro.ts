module Registro {

    export const misRegistros = {
        UsuarioConectado: 'usuario-conectado'
    };

    export function UsuarioConectado(): number {
        return JSON.parse(sessionStorage.getItem('usuario-conectado'));
    }; 

    export function RegistrarUsuarioDeConexion(llamador: any): Promise<any> {

        function RegistrarUsuario(peticion: ApiDeAjax.DescriptorAjax) {
            let registro: any = peticion.resultado.datos;
            sessionStorage.setItem('usuario-conectado', JSON.stringify(registro));
        }

        let usuarioConectado: string = sessionStorage.getItem('usuario-conectado');

        return new Promise((resolve, reject) => {

            let url: string = `/${Ajax.Usuarios.ruta}/${Ajax.Usuarios.accion.LeerUsuarioDeConexion}`;

            let a = new ApiDeAjax.DescriptorAjax(llamador
                , Ajax.Usuarios.accion.LeerUsuarioDeConexion
                , llamador
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , (peticion) => {
                    RegistrarUsuario(peticion);
                    resolve(usuarioConectado);
                }
                , () => {
                    reject();
                }
            );
            if (usuarioConectado != null)
                resolve(usuarioConectado);
            else
                a.Ejecutar();
        });


    }

}