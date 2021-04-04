var ModoAcceso;
(function (ModoAcceso) {
    let enumModoDeAccesoDeDatos;
    (function (enumModoDeAccesoDeDatos) {
        enumModoDeAccesoDeDatos[enumModoDeAccesoDeDatos["Administrador"] = 0] = "Administrador";
        enumModoDeAccesoDeDatos[enumModoDeAccesoDeDatos["Gestor"] = 1] = "Gestor";
        enumModoDeAccesoDeDatos[enumModoDeAccesoDeDatos["Consultor"] = 2] = "Consultor";
        enumModoDeAccesoDeDatos[enumModoDeAccesoDeDatos["SinPermiso"] = 3] = "SinPermiso";
    })(enumModoDeAccesoDeDatos = ModoAcceso.enumModoDeAccesoDeDatos || (ModoAcceso.enumModoDeAccesoDeDatos = {}));
    const ModoDeAccesoDeDatos = {
        Administrador: "administrador",
        Gestor: "gestor",
        Consultor: "consultor",
        SinPermiso: "sinpermiso"
    };
    function ModoDeAcceso(modoDeAcceso) {
        if (!HayPermisos(modoDeAcceso))
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
    function HayPermisos(modoAcceso) {
        if (IsNullOrEmpty(modoAcceso) || modoAcceso === ModoDeAccesoDeDatos.SinPermiso)
            return false;
        else
            return true;
    }
    ModoAcceso.HayPermisos = HayPermisos;
    function EsAdministrador(modoAcceso) {
        if (IsNullOrEmpty(modoAcceso))
            return false;
        if (ModoDeAccesoDeDatos.Administrador === modoAcceso)
            return true;
        else
            return false;
    }
    ModoAcceso.EsAdministrador = EsAdministrador;
    function EsGestor(modoAcceso) {
        if (IsNullOrEmpty(modoAcceso))
            return false;
        if (EsAdministrador(modoAcceso) || ModoDeAccesoDeDatos.Gestor === modoAcceso)
            return true;
        else
            return false;
    }
    ModoAcceso.EsGestor = EsGestor;
    function EsConsultor(modoAcceso) {
        if (IsNullOrEmpty(modoAcceso))
            return false;
        if (EsGestor(modoAcceso) || ModoDeAccesoDeDatos.Consultor === modoAcceso)
            return true;
        else
            return false;
    }
    ModoAcceso.EsConsultor = EsConsultor;
})(ModoAcceso || (ModoAcceso = {}));
//# sourceMappingURL=Enumerados.js.map