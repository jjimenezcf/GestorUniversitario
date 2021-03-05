var TrabajosSometido;
(function (TrabajosSometido) {
    function CrearCrudDeTrazasDeUnTrabajo(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new TrabajosSometido.CrudDeTrazasDeUnTrabajo(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
    }
    TrabajosSometido.CrearCrudDeTrazasDeUnTrabajo = CrearCrudDeTrazasDeUnTrabajo;
    class CrudDeTrazasDeUnTrabajo extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionTrazaDeUnTrabajo(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionTrazaDeUnTrabajo(this, idPanelEdicion);
        }
    }
    TrabajosSometido.CrudDeTrazasDeUnTrabajo = CrudDeTrazasDeUnTrabajo;
    class CrudCreacionTrazaDeUnTrabajo extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    TrabajosSometido.CrudCreacionTrazaDeUnTrabajo = CrudCreacionTrazaDeUnTrabajo;
    class CrudEdicionTrazaDeUnTrabajo extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    TrabajosSometido.CrudEdicionTrazaDeUnTrabajo = CrudEdicionTrazaDeUnTrabajo;
})(TrabajosSometido || (TrabajosSometido = {}));
//# sourceMappingURL=TrazasDeUnTrabajo.js.map