var Callejero;
(function (Callejero) {
    function CrearCrudDeCpsDeUnMunicipio(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Callejero.CrudDeCpsDeUnMunicipio(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Callejero.CrearCrudDeCpsDeUnMunicipio = CrearCrudDeCpsDeUnMunicipio;
    class CrudDeCpsDeUnMunicipio extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionCpDeUnMunicipio(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionCpDeUnMunicipio(this, idPanelEdicion);
        }
    }
    Callejero.CrudDeCpsDeUnMunicipio = CrudDeCpsDeUnMunicipio;
    class CrudCreacionCpDeUnMunicipio extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Callejero.CrudCreacionCpDeUnMunicipio = CrudCreacionCpDeUnMunicipio;
    class CrudEdicionCpDeUnMunicipio extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Callejero.CrudEdicionCpDeUnMunicipio = CrudEdicionCpDeUnMunicipio;
})(Callejero || (Callejero = {}));
//# sourceMappingURL=CpsDeUnMunicipio.js.map