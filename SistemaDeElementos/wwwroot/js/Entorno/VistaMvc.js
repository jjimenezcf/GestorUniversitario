var Entorno;
(function (Entorno) {
    function CrearCrudVistaMvc(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Entorno.CrudMntVistaMvc(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Entorno.CrearCrudVistaMvc = CrearCrudVistaMvc;
    class CrudMntVistaMvc extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionVistaMvc(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionVistaMvc(this, idPanelEdicion);
        }
    }
    Entorno.CrudMntVistaMvc = CrudMntVistaMvc;
    class CrudCreacionVistaMvc extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Entorno.CrudCreacionVistaMvc = CrudCreacionVistaMvc;
    class CrudEdicionVistaMvc extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Entorno.CrudEdicionVistaMvc = CrudEdicionVistaMvc;
})(Entorno || (Entorno = {}));
//# sourceMappingURL=VistaMvc.js.map