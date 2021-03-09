var Seguridad;
(function (Seguridad) {
    function CrearCrudDePermisos(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Seguridad.CrudDePermisos(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            MensajesSe.Info('llendo a trás');
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Seguridad.CrearCrudDePermisos = CrearCrudDePermisos;
    class CrudDePermisos extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionPermiso(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionPermiso(this, idPanelEdicion);
        }
    }
    Seguridad.CrudDePermisos = CrudDePermisos;
    class CrudCreacionPermiso extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Seguridad.CrudCreacionPermiso = CrudCreacionPermiso;
    class CrudEdicionPermiso extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Seguridad.CrudEdicionPermiso = CrudEdicionPermiso;
})(Seguridad || (Seguridad = {}));
//# sourceMappingURL=Permisos.js.map