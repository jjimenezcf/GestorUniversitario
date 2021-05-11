var TrabajosSometido;
(function (TrabajosSometido) {
    function CrearCrudDeCorreos(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new TrabajosSometido.CrudDeCorreos(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    TrabajosSometido.CrearCrudDeCorreos = CrearCrudDeCorreos;
    class CrudDeCorreos extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionCorreo(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionCorreo(this, idPanelEdicion);
        }
    }
    TrabajosSometido.CrudDeCorreos = CrudDeCorreos;
    class CrudCreacionCorreo extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    TrabajosSometido.CrudCreacionCorreo = CrudCreacionCorreo;
    class CrudEdicionCorreo extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    TrabajosSometido.CrudEdicionCorreo = CrudEdicionCorreo;
})(TrabajosSometido || (TrabajosSometido = {}));
//# sourceMappingURL=Correos.js.map