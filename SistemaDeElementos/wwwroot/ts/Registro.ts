module Registro {

    export const misRegistros = {
        UsuarioConectado: 'usuario-conectado',
        EsAdministrador: 'administrador'
    };

    export class UsuarioDeConexion {
        public login: string;
        public id: number;
        public administrador: boolean;
    }

    export function HayUsuarioDeConexion(): boolean {
        return sessionStorage.getItem(misRegistros.EsAdministrador) !== '';
    }

    function crearUsuarioDeConexion(usuario: any): UsuarioDeConexion {
        let u: UsuarioDeConexion = new UsuarioDeConexion();
        u.id = Numero(usuario['id']);
        u.login = usuario['login'];
        u.administrador = usuario['administrador'] == 'S';
        return u;
    }

    export function UsuarioConectado(): UsuarioDeConexion {
        return crearUsuarioDeConexion(JSON.parse(sessionStorage.getItem(misRegistros.UsuarioConectado)));
    };

    export function EsAdministrador(): boolean {
        return JSON.parse(sessionStorage.getItem(misRegistros.EsAdministrador));
    };

    export function RegistrarUsuarioDeConexion(llamador: any): Promise<any> {

        function RegistrarUsuario(peticion: ApiDeAjax.DescriptorAjax): UsuarioDeConexion {
            let usuario: UsuarioDeConexion = crearUsuarioDeConexion(peticion.resultado.datos) as UsuarioDeConexion;
            sessionStorage.setItem(misRegistros.UsuarioConectado, JSON.stringify(usuario));
            sessionStorage.setItem(misRegistros.EsAdministrador, JSON.stringify(usuario.administrador));
            return usuario;
        }

        return new Promise((resolve, reject) => {

            let url: string = `/${Ajax.Usuarios.ruta}/${Ajax.Usuarios.accion.LeerUsuarioDeConexion}`;

            let a = new ApiDeAjax.DescriptorAjax(llamador
                , Ajax.Usuarios.accion.LeerUsuarioDeConexion
                , llamador
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , (peticion) => {
                    resolve(RegistrarUsuario(peticion));
                }
                , () => {
                    reject();
                }
            );

            if (!HayUsuarioDeConexion())
               a.Ejecutar();
        });


    }

    export function EliminarUsuarioDeConexion() {
        sessionStorage.setItem(misRegistros.UsuarioConectado, '');
        sessionStorage.setItem(misRegistros.EsAdministrador, '');
    }

}