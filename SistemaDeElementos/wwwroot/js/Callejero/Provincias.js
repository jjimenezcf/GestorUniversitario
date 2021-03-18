var Callejero;
(function (Callejero) {
    function CrearCrudDeProvincias(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Callejero.CrudDeProvincias(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Callejero.CrearCrudDeProvincias = CrearCrudDeProvincias;
    class CrudDeProvincias extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionProvincia(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionProvincia(this, idPanelEdicion);
        }
    }
    Callejero.CrudDeProvincias = CrudDeProvincias;
    class CrudCreacionProvincia extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Callejero.CrudCreacionProvincia = CrudCreacionProvincia;
    class CrudEdicionProvincia extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Callejero.CrudEdicionProvincia = CrudEdicionProvincia;
})(Callejero || (Callejero = {}));
//# sourceMappingURL=Provincias.js.map