module Registro {

    export const misRegistros = {
        UsuarioConectado: 'usuario-conectado',
        EsAdministrador: 'administrador'
    };

    export class UsuarioDeConexion {
        public login: string;
        public id: number;
    }

    function CrearUsuarioDeConexion(usuario: any): UsuarioDeConexion {
        let u: UsuarioDeConexion = new UsuarioDeConexion();
        u.id = Numero(usuario['id']);
        u.login = usuario['login'];
        return u;
    }

    export function UsuarioConectado(): UsuarioDeConexion {
        return CrearUsuarioDeConexion(JSON.parse(sessionStorage.getItem(misRegistros.UsuarioConectado)));
    };
    export function EsAdministrador(): boolean {
        return JSON.parse(sessionStorage.getItem(misRegistros.EsAdministrador)) === 'S';
    }; 

    export function RegistrarUsuarioDeConexion(llamador: any): Promise<any> {

        function RegistrarUsuario(peticion: ApiDeAjax.DescriptorAjax) {
            let registro: any = peticion.resultado.datos;
            sessionStorage.setItem(misRegistros.UsuarioConectado, JSON.stringify(registro));
            sessionStorage.setItem(misRegistros.EsAdministrador, JSON.stringify(registro));
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