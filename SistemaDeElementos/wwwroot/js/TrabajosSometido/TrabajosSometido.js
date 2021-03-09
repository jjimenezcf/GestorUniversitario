var TrabajosSometido;
(function (TrabajosSometido) {
    function CrearCrudDeTrabajosSometido(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new TrabajosSometido.CrudDeTrabajosSometido(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    TrabajosSometido.CrearCrudDeTrabajosSometido = CrearCrudDeTrabajosSometido;
    class CrudDeTrabajosSometido extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionTrabajoSometido(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionTrabajoSometido(this, idPanelEdicion);
        }
    }
    TrabajosSometido.CrudDeTrabajosSometido = CrudDeTrabajosSometido;
    class CrudCreacionTrabajoSometido extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    TrabajosSometido.CrudCreacionTrabajoSometido = CrudCreacionTrabajoSometido;
    class CrudEdicionTrabajoSometido extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    TrabajosSometido.CrudEdicionTrabajoSometido = CrudEdicionTrabajoSometido;
})(TrabajosSometido || (TrabajosSometido = {}));
//# sourceMappingURL=TrabajosSometido.js.map