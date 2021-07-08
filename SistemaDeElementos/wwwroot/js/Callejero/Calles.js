var Callejero;
(function (Callejero) {
    function CrearCrudDeCalles(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Callejero.CrudDeCalles(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Callejero.CrearCrudDeCalles = CrearCrudDeCalles;
    class CrudDeCalles extends Crud.CrudMnt {
        get EditorDePais() {
            let editor = ApiControl.BuscarListaDinamicaPorPropiedad(this.ZonaDeFiltro, Callejero.atributo.propiedad.idpais);
            if (NoDefinida(editor))
                MensajesSe.EmitirExcepcion("Propiedad EditorDePais", "No se lo caliza el editor de Pais en el filtro de Calle");
            return editor;
        }
        ;
        get EditorDeProvincia() {
            let editor = ApiControl.BuscarListaDinamicaPorPropiedad(this.ZonaDeFiltro, Callejero.atributo.propiedad.idprovincia);
            if (NoDefinida(editor))
                MensajesSe.EmitirExcepcion("Propiedad EditorDeProvincia", "No se lo caliza el editor de Provincia en el filtro de Calle");
            return editor;
        }
        ;
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionCalle(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionCalle(this, idPanelEdicion);
        }
    }
    Callejero.CrudDeCalles = CrudDeCalles;
    class CrudCreacionCalle extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Callejero.CrudCreacionCalle = CrudCreacionCalle;
    class CrudEdicionCalle extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Callejero.CrudEdicionCalle = CrudEdicionCalle;
})(Callejero || (Callejero = {}));
//# sourceMappingURL=Calles.js.map