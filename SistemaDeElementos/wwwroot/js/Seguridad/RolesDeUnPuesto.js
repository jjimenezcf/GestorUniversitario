var Seguridad;
(function (Seguridad) {
    function CrearCrudDeRolesDeUnPuesto(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Seguridad.CrudDeRolesDeUnPuesto(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Seguridad.CrearCrudDeRolesDeUnPuesto = CrearCrudDeRolesDeUnPuesto;
    class CrudDeRolesDeUnPuesto extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionRolDeUnPuesto(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionRolDeUnPuesto(this, idPanelEdicion);
        }
    }
    Seguridad.CrudDeRolesDeUnPuesto = CrudDeRolesDeUnPuesto;
    class CrudCreacionRolDeUnPuesto extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Seguridad.CrudCreacionRolDeUnPuesto = CrudCreacionRolDeUnPuesto;
    class CrudEdicionRolDeUnPuesto extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Seguridad.CrudEdicionRolDeUnPuesto = CrudEdicionRolDeUnPuesto;
})(Seguridad || (Seguridad = {}));
//# sourceMappingURL=RolesDeUnPuesto.js.map