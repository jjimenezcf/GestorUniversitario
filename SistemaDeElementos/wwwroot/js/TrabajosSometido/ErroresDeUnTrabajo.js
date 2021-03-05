var TrabajosSometido;
(function (TrabajosSometido) {
    function CrearCrudDeErroresDeUnTrabajo(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new TrabajosSometido.CrudDeErroresDeUnTrabajo(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
    }
    TrabajosSometido.CrearCrudDeErroresDeUnTrabajo = CrearCrudDeErroresDeUnTrabajo;
    class CrudDeErroresDeUnTrabajo extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionErrorDeUnTrabajo(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionErrorDeUnTrabajo(this, idPanelEdicion);
        }
    }
    TrabajosSometido.CrudDeErroresDeUnTrabajo = CrudDeErroresDeUnTrabajo;
    class CrudCreacionErrorDeUnTrabajo extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    TrabajosSometido.CrudCreacionErrorDeUnTrabajo = CrudCreacionErrorDeUnTrabajo;
    class CrudEdicionErrorDeUnTrabajo extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    TrabajosSometido.CrudEdicionErrorDeUnTrabajo = CrudEdicionErrorDeUnTrabajo;
})(TrabajosSometido || (TrabajosSometido = {}));
//# sourceMappingURL=ErroresDeUnTrabajo.js.map