var Seguridad;
(function (Seguridad) {
    function CrearCrudDePuestosDeUnRol(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Seguridad.CrudDePuestosDeUnRol(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            MensajesSe.Info('llendo a trás');
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Seguridad.CrearCrudDePuestosDeUnRol = CrearCrudDePuestosDeUnRol;
    class CrudDePuestosDeUnRol extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionPuestoDeUnRol(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionPuestoDeUnRol(this, idPanelEdicion);
        }
    }
    Seguridad.CrudDePuestosDeUnRol = CrudDePuestosDeUnRol;
    class CrudCreacionPuestoDeUnRol extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Seguridad.CrudCreacionPuestoDeUnRol = CrudCreacionPuestoDeUnRol;
    class CrudEdicionPuestoDeUnRol extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Seguridad.CrudEdicionPuestoDeUnRol = CrudEdicionPuestoDeUnRol;
})(Seguridad || (Seguridad = {}));
//# sourceMappingURL=PuestosDeUnRol.js.map