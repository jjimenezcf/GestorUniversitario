var Seguridad;
(function (Seguridad) {
    function CrearCrudDePermisosDeUnRol(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Seguridad.CrudDePermisosDeUnRol(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Seguridad.CrearCrudDePermisosDeUnRol = CrearCrudDePermisosDeUnRol;
    class CrudDePermisosDeUnRol extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionPermisoDeUnRol(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionPermisoDeUnRol(this, idPanelEdicion);
        }
    }
    Seguridad.CrudDePermisosDeUnRol = CrudDePermisosDeUnRol;
    class CrudCreacionPermisoDeUnRol extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Seguridad.CrudCreacionPermisoDeUnRol = CrudCreacionPermisoDeUnRol;
    class CrudEdicionPermisoDeUnRol extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Seguridad.CrudEdicionPermisoDeUnRol = CrudEdicionPermisoDeUnRol;
})(Seguridad || (Seguridad = {}));
//# sourceMappingURL=PermisosDeUnRol.js.map