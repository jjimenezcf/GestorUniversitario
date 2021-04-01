var Auditoria;
(function (Auditoria) {
    function CrearCrudDeAuditoria(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Auditoria.CrudDeAuditoria(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Auditoria.CrearCrudDeAuditoria = CrearCrudDeAuditoria;
    class CrudDeAuditoria extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionAuditoria(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionAuditoria(this, idPanelEdicion);
        }
    }
    Auditoria.CrudDeAuditoria = CrudDeAuditoria;
    class CrudCreacionAuditoria extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Auditoria.CrudCreacionAuditoria = CrudCreacionAuditoria;
    class CrudEdicionAuditoria extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Auditoria.CrudEdicionAuditoria = CrudEdicionAuditoria;
})(Auditoria || (Auditoria = {}));
//# sourceMappingURL=Auditoria.js.map