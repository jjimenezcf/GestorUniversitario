var Callejero;
(function (Callejero) {
    function CrearCrudDeCodigosPostales(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Callejero.CrudDeCodigosPostales(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Callejero.CrearCrudDeCodigosPostales = CrearCrudDeCodigosPostales;
    class CrudDeCodigosPostales extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionCodigoPostal(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionCodigoPostal(this, idPanelEdicion);
        }
    }
    Callejero.CrudDeCodigosPostales = CrudDeCodigosPostales;
    class CrudCreacionCodigoPostal extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Callejero.CrudCreacionCodigoPostal = CrudCreacionCodigoPostal;
    class CrudEdicionCodigoPostal extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Callejero.CrudEdicionCodigoPostal = CrudEdicionCodigoPostal;
})(Callejero || (Callejero = {}));
//# sourceMappingURL=CodigosPostales.js.map