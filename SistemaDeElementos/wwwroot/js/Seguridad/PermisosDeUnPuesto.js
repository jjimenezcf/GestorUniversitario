var Seguridad;
(function (Seguridad) {
    function CrearCrudDePermisosDeUnPuesto(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Seguridad.CrudDePermisosDeUnPuesto(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            MensajesSe.Info('llendo a trás');
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Seguridad.CrearCrudDePermisosDeUnPuesto = CrearCrudDePermisosDeUnPuesto;
    class CrudDePermisosDeUnPuesto extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionPermisoDeUnPuesto(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionPermisoDeUnPuesto(this, idPanelEdicion);
        }
    }
    Seguridad.CrudDePermisosDeUnPuesto = CrudDePermisosDeUnPuesto;
    class CrudCreacionPermisoDeUnPuesto extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Seguridad.CrudCreacionPermisoDeUnPuesto = CrudCreacionPermisoDeUnPuesto;
    class CrudEdicionPermisoDeUnPuesto extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Seguridad.CrudEdicionPermisoDeUnPuesto = CrudEdicionPermisoDeUnPuesto;
})(Seguridad || (Seguridad = {}));
//# sourceMappingURL=PermisosDeUnPuesto.js.map