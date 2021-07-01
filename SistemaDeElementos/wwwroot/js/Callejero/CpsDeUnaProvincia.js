var Callejero;
(function (Callejero) {
    function CrearCrudDeCpsDeUnaProvincia(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Callejero.CrudDeCpsDeUnaProvincia(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Callejero.CrearCrudDeCpsDeUnaProvincia = CrearCrudDeCpsDeUnaProvincia;
    class CrudDeCpsDeUnaProvincia extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionCpDeUnaProvincia(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionCpDeUnaProvincia(this, idPanelEdicion);
        }
    }
    Callejero.CrudDeCpsDeUnaProvincia = CrudDeCpsDeUnaProvincia;
    class CrudCreacionCpDeUnaProvincia extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Callejero.CrudCreacionCpDeUnaProvincia = CrudCreacionCpDeUnaProvincia;
    class CrudEdicionCpDeUnaProvincia extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Callejero.CrudEdicionCpDeUnaProvincia = CrudEdicionCpDeUnaProvincia;
})(Callejero || (Callejero = {}));
//# sourceMappingURL=CpsDeUnaProvincia.js.map