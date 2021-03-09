var Negocio;
(function (Negocio) {
    function CrearCrudDeNegocios(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Negocio.CrudDeNegocios(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Negocio.CrearCrudDeNegocios = CrearCrudDeNegocios;
    class CrudDeNegocios extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionNegocio(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionNegocio(this, idPanelEdicion);
        }
    }
    Negocio.CrudDeNegocios = CrudDeNegocios;
    class CrudCreacionNegocio extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Negocio.CrudCreacionNegocio = CrudCreacionNegocio;
    class CrudEdicionNegocio extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Negocio.CrudEdicionNegocio = CrudEdicionNegocio;
})(Negocio || (Negocio = {}));
//# sourceMappingURL=Negocio.js.map