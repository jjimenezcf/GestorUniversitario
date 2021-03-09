var Entorno;
(function (Entorno) {
    function CrearCrudDePermisosDeUnUsuario(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Entorno.CrudDePermisosDeUnUsuario(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            MensajesSe.Info('llendo a trás');
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Entorno.CrearCrudDePermisosDeUnUsuario = CrearCrudDePermisosDeUnUsuario;
    class CrudDePermisosDeUnUsuario extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionPermisoDeUnUsuario(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionPermisoDeUnUsuario(this, idPanelEdicion);
        }
    }
    Entorno.CrudDePermisosDeUnUsuario = CrudDePermisosDeUnUsuario;
    class CrudCreacionPermisoDeUnUsuario extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Entorno.CrudCreacionPermisoDeUnUsuario = CrudCreacionPermisoDeUnUsuario;
    class CrudEdicionPermisoDeUnUsuario extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Entorno.CrudEdicionPermisoDeUnUsuario = CrudEdicionPermisoDeUnUsuario;
})(Entorno || (Entorno = {}));
//# sourceMappingURL=PermisosDeUnUsuario.js.map