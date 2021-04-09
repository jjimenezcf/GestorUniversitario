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

    export function HayPermisos(permisosNecesarios: enumModoDeAccesoDeDatos, permisosDelUsuario: enumModoDeAccesoDeDatos): boolean {
        if (permisosNecesarios === enumModoDeAccesoDeDatos.SinPermiso)
            return true;

        if (permisosNecesarios === enumModoDeAccesoDeDatos.Consultor && permisosDelUsuario !== enumModoDeAccesoDeDatos.SinPermiso)
            return true;

        if (permisosNecesarios === enumModoDeAccesoDeDatos.Gestor &&
            (permisosDelUsuario === enumModoDeAccesoDeDatos.Gestor || permisosDelUsuario === enumModoDeAccesoDeDatos.Administrador)
        )
            return true;

        if (permisosNecesarios === enumModoDeAccesoDeDatos.Administrador && permisosDelUsuario === enumModoDeAccesoDeDatos.Administrador)
            return true;

        return false;
    }

    export function Parsear(modoDeAcceso: string): enumModoDeAccesoDeDatos {
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


    export function AplicarModoDeAccesoAlNegocio(opcionesGenerales: NodeListOf<HTMLButtonElement>, modoDeAccesoDelUsuario: enumModoDeAccesoDeDatos): void {
        for (var i = 0; i < opcionesGenerales.length; i++) {
            let opcion: HTMLButtonElement = opcionesGenerales[i];

            if (ApiControl.EstaOculta(opcion))
                continue;

            let permisosNecesarios: string = opcion.getAttribute(atOpcionDeMenu.permisosNecesarios);
            ApiControl.OcultarMostrarOpcionDeMenu(opcion, !ModoAcceso.HayPermisos(ModoAcceso.Parsear(permisosNecesarios), modoDeAccesoDelUsuario));
        }
    }

    export function AplicarModoAccesoAlElemento(opcion: HTMLButtonElement, hayMasDeUnaSeleccionada: boolean, permisos: ModoAcceso.enumModoDeAccesoDeDatos) {
        if (hayMasDeUnaSeleccionada && ApiControl.EstaBloqueada(opcion))
            return;

        let estaDeshabilitado = ApiControl.EstaBloqueada(opcion);
        let permisosNecesarios: string = opcion.getAttribute(atOpcionDeMenu.permisosNecesarios);
        let permiteMultiSeleccion: string = opcion.getAttribute(atOpcionDeMenu.permiteMultiSeleccion);
        if (!EsTrue(permiteMultiSeleccion) && hayMasDeUnaSeleccionada) {
            ApiControl.BloquearDesbloquearOpcionDeMenu(opcion, true);
            return;
        }

        if (!ModoAcceso.HayPermisos(ModoAcceso.Parsear(permisosNecesarios), permisos))
            ApiControl.BloquearDesbloquearOpcionDeMenu(opcion, true);
        else
            ApiControl.BloquearDesbloquearOpcionDeMenu(opcion, (estaDeshabilitado && hayMasDeUnaSeleccionada) || false);
    }

    export function AplicarloALosEditores(panel: HTMLDivElement, permisosDeUsuario: enumModoDeAccesoDeDatos) {
        let editores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.tipo}='${TipoControl.Editor}']`) as NodeListOf<HTMLInputElement>;
        for (var i = 0; i < editores.length; i++) {
            var control = editores[i] as HTMLInputElement;
            AplicarAlControl(control, permisosDeUsuario);
        }
    }

    export function AplicarloALosRestrictores(panel: HTMLDivElement) {
        let restrictores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.tipo}='${TipoControl.restrictorDeEdicion}']`) as NodeListOf<HTMLInputElement>;
        for (var i = 0; i < restrictores.length; i++) {
            var control = restrictores[i] as HTMLInputElement;
            control.readOnly = true;
        }
    }

    export function AplicarloAlasAreasDeTexto(panel: HTMLDivElement, permisosDeUsuario: enumModoDeAccesoDeDatos) {
        let areas: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`textarea[${atControl.tipo}='${TipoControl.AreaDeTexto}']`) as NodeListOf<HTMLInputElement>;
        for (var i = 0; i < areas.length; i++) {
            var control = areas[i] as HTMLInputElement;
            AplicarAlControl(control, permisosDeUsuario);
        }
    }

    export function AplicarloALasFechas(panel: HTMLDivElement, permisosDeUsuario: enumModoDeAccesoDeDatos) {
        let fechas: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.tipo}='${TipoControl.SelectorDeFecha}']`) as NodeListOf<HTMLInputElement>;
        for (var i = 0; i < fechas.length; i++) {
            var control = fechas[i] as HTMLInputElement;
            AplicarAlControl(control, permisosDeUsuario);
        }

        let fechaHora: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.tipo}='${TipoControl.SelectorDeFechaHora}']`) as NodeListOf<HTMLInputElement>;
        for (var i = 0; i < fechaHora.length; i++) {
            var control = fechaHora[i] as HTMLInputElement;
            AplicarAlControl(control, permisosDeUsuario);
            let idHora: string = control.getAttribute(atSelectorDeFecha.hora);
            let controlHora: HTMLInputElement = document.getElementById(idHora) as HTMLInputElement;
            AplicarAlControl(controlHora, permisosDeUsuario);
        }

    }

    function AplicarAlControl(control: HTMLInputElement, permiso: enumModoDeAccesoDeDatos): void {
        var editable = control.getAttribute(atControl.editable);
        if (EsTrue(editable)) {
            control.readOnly = !HayPermisos(enumModoDeAccesoDeDatos.Gestor, permiso);
        }
        else
            control.readOnly = true;
    }

}