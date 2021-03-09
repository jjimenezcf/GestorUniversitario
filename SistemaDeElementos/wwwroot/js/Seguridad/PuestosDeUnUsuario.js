var Seguridad;
(function (Seguridad) {
    function CrearCrudDePuestosDeUnUsuario(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Seguridad.CrudDePuestosDeUnUsuario(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Seguridad.CrearCrudDePuestosDeUnUsuario = CrearCrudDePuestosDeUnUsuario;
    class CrudDePuestosDeUnUsuario extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionPuestoDeUnUsuario(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionPuestoDeUnUsuario(this, idPanelEdicion);
        }
    }
    Seguridad.CrudDePuestosDeUnUsuario = CrudDePuestosDeUnUsuario;
    class CrudCreacionPuestoDeUnUsuario extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Seguridad.CrudCreacionPuestoDeUnUsuario = CrudCreacionPuestoDeUnUsuario;
    class CrudEdicionPuestoDeUnUsuario extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Seguridad.CrudEdicionPuestoDeUnUsuario = CrudEdicionPuestoDeUnUsuario;
})(Seguridad || (Seguridad = {}));
//# sourceMappingURL=PuestosDeUnUsuario.js.map