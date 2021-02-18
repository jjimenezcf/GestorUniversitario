var Seguridad;
(function (Seguridad) {
    function CrearCrudDePuestosDeTrabajo(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Seguridad.CrudDePuestosDeTrabajo(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
    }
    Seguridad.CrearCrudDePuestosDeTrabajo = CrearCrudDePuestosDeTrabajo;
    class CrudDePuestosDeTrabajo extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionPuestoDeTrabajo(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionPuestoDeTrabajo(this, idPanelEdicion);
        }
    }
    Seguridad.CrudDePuestosDeTrabajo = CrudDePuestosDeTrabajo;
    class CrudCreacionPuestoDeTrabajo extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Seguridad.CrudCreacionPuestoDeTrabajo = CrudCreacionPuestoDeTrabajo;
    class CrudEdicionPuestoDeTrabajo extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Seguridad.CrudEdicionPuestoDeTrabajo = CrudEdicionPuestoDeTrabajo;
})(Seguridad || (Seguridad = {}));
//# sourceMappingURL=PuestoDeTrabajo.js.map