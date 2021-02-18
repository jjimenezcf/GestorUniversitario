var Entorno;
(function (Entorno) {
    function CrearCrudDeVariables(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Entorno.CrudDeVariables(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
    }
    Entorno.CrearCrudDeVariables = CrearCrudDeVariables;
    class CrudDeVariables extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionVariable(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionVariable(this, idPanelEdicion);
        }
    }
    Entorno.CrudDeVariables = CrudDeVariables;
    class CrudCreacionVariable extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Entorno.CrudCreacionVariable = CrudCreacionVariable;
    class CrudEdicionVariable extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Entorno.CrudEdicionVariable = CrudEdicionVariable;
})(Entorno || (Entorno = {}));
//# sourceMappingURL=Variables.js.map