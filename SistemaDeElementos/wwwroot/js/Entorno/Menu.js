var Entorno;
(function (Entorno) {
    function CrearCrudDeMenus(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Entorno.CrudDeMenus(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            MensajesSe.Info('llendo a trás');
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Entorno.CrearCrudDeMenus = CrearCrudDeMenus;
    class CrudDeMenus extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionMenu(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionMenu(this, idPanelEdicion);
        }
    }
    Entorno.CrudDeMenus = CrudDeMenus;
    class CrudCreacionMenu extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Entorno.CrudCreacionMenu = CrudCreacionMenu;
    class CrudEdicionMenu extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Entorno.CrudEdicionMenu = CrudEdicionMenu;
})(Entorno || (Entorno = {}));
//# sourceMappingURL=Menu.js.map