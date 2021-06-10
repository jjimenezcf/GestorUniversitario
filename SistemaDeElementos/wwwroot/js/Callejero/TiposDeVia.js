var Callejero;
(function (Callejero) {
    function CrearCrudTiposDeVia(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Callejero.CrudDeTiposDeVia(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Callejero.CrearCrudTiposDeVia = CrearCrudTiposDeVia;
    class CrudDeTiposDeVia extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionTipoDeVia(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionTipoDeVia(this, idPanelEdicion);
        }
    }
    Callejero.CrudDeTiposDeVia = CrudDeTiposDeVia;
    class CrudCreacionTipoDeVia extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Callejero.CrudCreacionTipoDeVia = CrudCreacionTipoDeVia;
    class CrudEdicionTipoDeVia extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Callejero.CrudEdicionTipoDeVia = CrudEdicionTipoDeVia;
})(Callejero || (Callejero = {}));
//# sourceMappingURL=TiposDeVia.js.map