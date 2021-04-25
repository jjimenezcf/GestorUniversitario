module Registro {

    export const misRegistros = {
        UsuarioConectado: 'usuario-conectado',
        EsAdministrador: 'administrador'
    };

    export class UsuarioDeConexion {
        public login: string;
        public id: number;
        public administrador: boolean;
        public mail: string;
    }

    export function HayUsuarioDeConexion(): boolean {
        let u = sessionStorage.getItem(misRegistros.UsuarioConectado);
        if (u === null || u === undefined)
            return false;

        let uc: UsuarioDeConexion = UsuarioConectado();
        return uc.id > 0;
    }

    function crearUsuarioDeConexion(usuario: any): UsuarioDeConexion {
        let u: UsuarioDeConexion = new UsuarioDeConexion();
        asignarUsuarioDeConexion(u, usuario);
        return u;
    }

    function asignarUsuarioDeConexion(u: UsuarioDeConexion, usuario: any): void {
        u.id = Numero(usuario['id']);
        u.login = usuario['login'];
        u.mail = usuario['mail'];
        u.administrador = usuario['administrador'] == 'S';
    }

    function asignarUsuarioNulo(u): void {
        u.id = 0;
        u.login = '';
        u.mail = '';
        u.administrador = false;
    }

    export function UsuarioConectado(): UsuarioDeConexion {
        let u: UsuarioDeConexion = new UsuarioDeConexion();
        try {
            asignarUsuarioDeConexion(u, JSON.parse(sessionStorage.getItem(misRegistros.UsuarioConectado)));
        }
        catch {
            asignarUsuarioNulo(u);
        }
        return u;
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