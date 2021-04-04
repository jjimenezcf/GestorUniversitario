namespace ModoAcceso {

    export enum enumModoDeAccesoDeDatos {
        Administrador,
        Gestor,
        Consultor,
        SinPermiso
    }

    export const ModoDeAccesoDeDatos = {
        Administrador: "administrador",
        Gestor: "gestor",
        Consultor: "consultor",
        SinPermiso: "sinpermiso"
    };

    export function HayPermisos(permisosNecesarios: string, permisosDelUsuario: string): boolean {
        let pn: enumModoDeAccesoDeDatos = ModoAcceso.ModoDeAcceso(permisosNecesarios);
        let pu: enumModoDeAccesoDeDatos = ModoAcceso.ModoDeAcceso(permisosDelUsuario);

        if (pn === enumModoDeAccesoDeDatos.SinPermiso)
            return true;

        if (pn === enumModoDeAccesoDeDatos.Consultor && pu !== enumModoDeAccesoDeDatos.SinPermiso)
            return true;

        if (pn === enumModoDeAccesoDeDatos.Gestor &&
            (pu === enumModoDeAccesoDeDatos.Gestor || pu === enumModoDeAccesoDeDatos.Administrador)
        )
            return true;

        if (pn === enumModoDeAccesoDeDatos.Administrador && pu === enumModoDeAccesoDeDatos.Administrador)
            return true;

        return false;
    }

    export function ModoDeAcceso(modoDeAcceso): enumModoDeAccesoDeDatos {
        if (!HayAlgunPermisos(modoDeAcceso))
            return enumModoDeAccesoDeDatos.SinPermiso;
        if (EsAdministrador(modoDeAcceso))
            return enumModoDeAccesoDeDatos.Administrador;
        if (EsGestor(modoDeAcceso))
            return enumModoDeAccesoDeDatos.Gestor;
        if (EsConsultor(modoDeAcceso))
            return enumModoDeAccesoDeDatos.Consultor;
        return enumModoDeAccesoDeDatos.SinPermiso;
    }

    export function HayAlgunPermisos(modoAcceso: string): boolean {
        if (IsNullOrEmpty(modoAcceso) || modoAcceso === ModoDeAccesoDeDatos.SinPermiso)
            return false;
        else
            return true;
    }

    export function EsAdministrador(modoAcceso: string): boolean {
        if (IsNullOrEmpty(modoAcceso))
            return false;

        if (ModoDeAccesoDeDatos.Administrador === modoAcceso)
            return true;
        else
            return false;
    }
    export function EsGestor(modoAcceso: string): boolean {
        if (IsNullOrEmpty(modoAcceso))
            return false;

        if (EsAdministrador(modoAcceso) || ModoDeAccesoDeDatos.Gestor === modoAcceso)
            return true;
        else
            return false;
    }
    export function EsConsultor(modoAcceso: string): boolean {
        if (IsNullOrEmpty(modoAcceso))
            return false;

        if (EsGestor(modoAcceso) || ModoDeAccesoDeDatos.Consultor === modoAcceso)
            return true;
        else
            return false;
    }
}