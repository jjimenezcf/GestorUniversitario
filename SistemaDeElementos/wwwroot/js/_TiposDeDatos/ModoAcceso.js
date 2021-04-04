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
        let pn = ModoAcceso.ModoDeAcceso(permisosNecesarios);
        let pu = ModoAcceso.ModoDeAcceso(permisosDelUsuario);
        if (pn === enumModoDeAccesoDeDatos.SinPermiso)
            return true;
        if (pn === enumModoDeAccesoDeDatos.Consultor && pu !== enumModoDeAccesoDeDatos.SinPermiso)
            return true;
        if (pn === enumModoDeAccesoDeDatos.Gestor &&
            (pu === enumModoDeAccesoDeDatos.Gestor || pu === enumModoDeAccesoDeDatos.Administrador))
            return true;
        if (pn === enumModoDeAccesoDeDatos.Administrador && pu === enumModoDeAccesoDeDatos.Administrador)
            return true;
        return false;
    }
    ModoAcceso.HayPermisos = HayPermisos;
    function ModoDeAcceso(modoDeAcceso) {
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
    ModoAcceso.ModoDeAcceso = ModoDeAcceso;
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
})(ModoAcceso || (ModoAcceso = {}));
//# sourceMappingURL=ModoAcceso.js.map