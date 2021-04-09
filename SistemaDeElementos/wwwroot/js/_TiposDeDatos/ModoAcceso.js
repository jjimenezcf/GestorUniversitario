var ModoAcceso;
(function (ModoAcceso) {
    let enumModoDeAccesoDeDatos;
    (function (enumModoDeAccesoDeDatos) {
        enumModoDeAccesoDeDatos[enumModoDeAccesoDeDatos["Administrador"] = 0] = "Administrador";
        enumModoDeAccesoDeDatos[enumModoDeAccesoDeDatos["Gestor"] = 1] = "Gestor";
        enumModoDeAccesoDeDatos[enumModoDeAccesoDeDatos["Consultor"] = 2] = "Consultor";
        enumModoDeAccesoDeDatos[enumModoDeAccesoDeDatos["SinPermiso"] = 3] = "SinPermiso";
    })(enumModoDeAccesoDeDatos = ModoAcceso.enumModoDeAccesoDeDatos || (ModoAcceso.enumModoDeAccesoDeDatos = {}));
    ModoAcceso.ModoDeAccesoDeDatos = {
        Administrador: "administrador",
        Gestor: "gestor",
        Consultor: "consultor",
        SinPermiso: "sinpermiso"
    };
    function HayPermisos(permisosNecesarios, permisosDelUsuario) {
        if (permisosNecesarios === enumModoDeAccesoDeDatos.SinPermiso)
            return true;
        if (permisosNecesarios === enumModoDeAccesoDeDatos.Consultor && permisosDelUsuario !== enumModoDeAccesoDeDatos.SinPermiso)
            return true;
        if (permisosNecesarios === enumModoDeAccesoDeDatos.Gestor &&
            (permisosDelUsuario === enumModoDeAccesoDeDatos.Gestor || permisosDelUsuario === enumModoDeAccesoDeDatos.Administrador))
            return true;
        if (permisosNecesarios === enumModoDeAccesoDeDatos.Administrador && permisosDelUsuario === enumModoDeAccesoDeDatos.Administrador)
            return true;
        return false;
    }
    ModoAcceso.HayPermisos = HayPermisos;
    function Parsear(modoDeAcceso) {
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
    ModoAcceso.Parsear = Parsear;
    function HayAlgunPermisos(modoAcceso) {
        if (IsNullOrEmpty(modoAcceso) || modoAcceso === ModoAcceso.ModoDeAccesoDeDatos.SinPermiso)
            return false;
        else
            return true;
    }
    ModoAcceso.HayAlgunPermisos = HayAlgunPermisos;
    function EsAdministrador(modoAcceso) {
        if (IsNullOrEmpty(modoAcceso))
            return false;
        if (ModoAcceso.ModoDeAccesoDeDatos.Administrador === modoAcceso)
            return true;
        else
            return false;
    }
    ModoAcceso.EsAdministrador = EsAdministrador;
    function EsGestor(modoAcceso) {
        if (IsNullOrEmpty(modoAcceso))
            return false;
        if (EsAdministrador(modoAcceso) || ModoAcceso.ModoDeAccesoDeDatos.Gestor === modoAcceso)
            return true;
        else
            return false;
    }
    ModoAcceso.EsGestor = EsGestor;
    function EsConsultor(modoAcceso) {
        if (IsNullOrEmpty(modoAcceso))
            return false;
        if (EsGestor(modoAcceso) || ModoAcceso.ModoDeAccesoDeDatos.Consultor === modoAcceso)
            return true;
        else
            return false;
    }
    ModoAcceso.EsConsultor = EsConsultor;
    function AplicarModoDeAccesoAlNegocio(opcionesGenerales, modoDeAccesoDelUsuario) {
        for (var i = 0; i < opcionesGenerales.length; i++) {
            let opcion = opcionesGenerales[i];
            if (ApiControl.EstaOculta(opcion))
                continue;
            let permisosNecesarios = opcion.getAttribute(atOpcionDeMenu.permisosNecesarios);
            ApiControl.OcultarMostrarOpcionDeMenu(opcion, !ModoAcceso.HayPermisos(ModoAcceso.Parsear(permisosNecesarios), modoDeAccesoDelUsuario));
        }
    }
    ModoAcceso.AplicarModoDeAccesoAlNegocio = AplicarModoDeAccesoAlNegocio;
    function AplicarModoAccesoAlElemento(opcion, hayMasDeUnaSeleccionada, permisos) {
        if (hayMasDeUnaSeleccionada && ApiControl.EstaBloqueada(opcion))
            return;
        let estaDeshabilitado = ApiControl.EstaBloqueada(opcion);
        let permisosNecesarios = opcion.getAttribute(atOpcionDeMenu.permisosNecesarios);
        let permiteMultiSeleccion = opcion.getAttribute(atOpcionDeMenu.permiteMultiSeleccion);
        if (!EsTrue(permiteMultiSeleccion) && hayMasDeUnaSeleccionada) {
            ApiControl.BloquearDesbloquearOpcionDeMenu(opcion, true);
            return;
        }
        if (!ModoAcceso.HayPermisos(ModoAcceso.Parsear(permisosNecesarios), permisos))
            ApiControl.BloquearDesbloquearOpcionDeMenu(opcion, true);
        else
            ApiControl.BloquearDesbloquearOpcionDeMenu(opcion, (estaDeshabilitado && hayMasDeUnaSeleccionada) || false);
    }
    ModoAcceso.AplicarModoAccesoAlElemento = AplicarModoAccesoAlElemento;
    function AplicarloALosEditores(panel, permisosDeUsuario) {
        let editores = panel.querySelectorAll(`input[${atControl.tipo}='${TipoControl.Editor}']`);
        for (var i = 0; i < editores.length; i++) {
            var control = editores[i];
            AplicarAlControl(control, permisosDeUsuario);
        }
    }
    ModoAcceso.AplicarloALosEditores = AplicarloALosEditores;
    function AplicarloALosRestrictores(panel) {
        let restrictores = panel.querySelectorAll(`input[${atControl.tipo}='${TipoControl.restrictorDeEdicion}']`);
        for (var i = 0; i < restrictores.length; i++) {
            var control = restrictores[i];
            control.readOnly = true;
        }
    }
    ModoAcceso.AplicarloALosRestrictores = AplicarloALosRestrictores;
    function AplicarloAlasAreasDeTexto(panel, permisosDeUsuario) {
        let areas = panel.querySelectorAll(`textarea[${atControl.tipo}='${TipoControl.AreaDeTexto}']`);
        for (var i = 0; i < areas.length; i++) {
            var control = areas[i];
            AplicarAlControl(control, permisosDeUsuario);
        }
    }
    ModoAcceso.AplicarloAlasAreasDeTexto = AplicarloAlasAreasDeTexto;
    function AplicarloALasFechas(panel, permisosDeUsuario) {
        let fechas = panel.querySelectorAll(`input[${atControl.tipo}='${TipoControl.SelectorDeFecha}']`);
        for (var i = 0; i < fechas.length; i++) {
            var control = fechas[i];
            AplicarAlControl(control, permisosDeUsuario);
        }
        let fechaHora = panel.querySelectorAll(`input[${atControl.tipo}='${TipoControl.SelectorDeFechaHora}']`);
        for (var i = 0; i < fechaHora.length; i++) {
            var control = fechaHora[i];
            AplicarAlControl(control, permisosDeUsuario);
            let idHora = control.getAttribute(atSelectorDeFecha.hora);
            let controlHora = document.getElementById(idHora);
            AplicarAlControl(controlHora, permisosDeUsuario);
        }
    }
    ModoAcceso.AplicarloALasFechas = AplicarloALasFechas;
    function AplicarAlControl(control, permiso) {
        var editable = control.getAttribute(atControl.editable);
        if (EsTrue(editable)) {
            control.readOnly = !HayPermisos(enumModoDeAccesoDeDatos.Gestor, permiso);
        }
        else
            control.readOnly = true;
    }
})(ModoAcceso || (ModoAcceso = {}));
//# sourceMappingURL=ModoAcceso.js.map