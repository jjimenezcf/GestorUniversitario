var Negocio;
(function (Negocio) {
    function CrearCrudDeParametrosDeNegocio(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Negocio.CrudDeParametrosDeNegocio(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Negocio.CrearCrudDeParametrosDeNegocio = CrearCrudDeParametrosDeNegocio;
    class CrudDeParametrosDeNegocio extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionDeParametroDeNegocio(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionDeParametroDeNegocio(this, idPanelEdicion);
        }
    }
    Negocio.CrudDeParametrosDeNegocio = CrudDeParametrosDeNegocio;
    class CrudCreacionDeParametroDeNegocio extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Negocio.CrudCreacionDeParametroDeNegocio = CrudCreacionDeParametroDeNegocio;
    class CrudEdicionDeParametroDeNegocio extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Negocio.CrudEdicionDeParametroDeNegocio = CrudEdicionDeParametroDeNegocio;
})(Negocio || (Negocio = {}));
//# sourceMappingURL=ParametrosDeNegocio.js.map