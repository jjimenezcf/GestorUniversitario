var Seguridad;
(function (Seguridad) {
    function CrearCrudDeRolesDeUnPermiso(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Seguridad.CrudDeRolesDeUnPermiso(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            MensajesSe.Info('llendo a trás');
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Seguridad.CrearCrudDeRolesDeUnPermiso = CrearCrudDeRolesDeUnPermiso;
    class CrudDeRolesDeUnPermiso extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionRolesDeUnPermiso(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionRolesDeUnPermiso(this, idPanelEdicion);
        }
    }
    Seguridad.CrudDeRolesDeUnPermiso = CrudDeRolesDeUnPermiso;
    class CrudCreacionRolesDeUnPermiso extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Seguridad.CrudCreacionRolesDeUnPermiso = CrudCreacionRolesDeUnPermiso;
    class CrudEdicionRolesDeUnPermiso extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Seguridad.CrudEdicionRolesDeUnPermiso = CrudEdicionRolesDeUnPermiso;
})(Seguridad || (Seguridad = {}));
//# sourceMappingURL=RolesDeUnPermiso.js.map