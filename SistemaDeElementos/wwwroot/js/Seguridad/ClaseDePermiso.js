var Seguridad;
(function (Seguridad) {
    function CrearCrudDeClasesDePermiso(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Seguridad.CrudDeClasesDePermiso(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
    }
    Seguridad.CrearCrudDeClasesDePermiso = CrearCrudDeClasesDePermiso;
    class CrudDeClasesDePermiso extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionClaseDePermiso(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionClaseDePermiso(this, idPanelEdicion);
        }
    }
    Seguridad.CrudDeClasesDePermiso = CrudDeClasesDePermiso;
    class CrudCreacionClaseDePermiso extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Seguridad.CrudCreacionClaseDePermiso = CrudCreacionClaseDePermiso;
    class CrudEdicionClaseDePermiso extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Seguridad.CrudEdicionClaseDePermiso = CrudEdicionClaseDePermiso;
})(Seguridad || (Seguridad = {}));
//# sourceMappingURL=ClaseDePermiso.js.map