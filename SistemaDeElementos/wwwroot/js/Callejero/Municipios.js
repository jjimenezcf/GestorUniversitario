var Callejero;
(function (Callejero) {
    function CrearCrudDeMunicipios(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Callejero.CrudDeMunicipios(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Callejero.CrearCrudDeMunicipios = CrearCrudDeMunicipios;
    class CrudDeMunicipios extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionMunicipio(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionMunicipio(this, idPanelEdicion);
        }
    }
    Callejero.CrudDeMunicipios = CrudDeMunicipios;
    class CrudCreacionMunicipio extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Callejero.CrudCreacionMunicipio = CrudCreacionMunicipio;
    class CrudEdicionMunicipio extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Callejero.CrudEdicionMunicipio = CrudEdicionMunicipio;
})(Callejero || (Callejero = {}));
//# sourceMappingURL=Municipios.js.map