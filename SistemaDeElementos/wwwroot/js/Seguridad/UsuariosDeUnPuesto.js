var Seguridad;
(function (Seguridad) {
    function CrearCrudDeUsuariosDeUnPuesto(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Seguridad.CrudDeUsuariosDeUnPuesto(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            MensajesSe.Info('llendo a trás');
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Seguridad.CrearCrudDeUsuariosDeUnPuesto = CrearCrudDeUsuariosDeUnPuesto;
    class CrudDeUsuariosDeUnPuesto extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionUsuarioDeUnPuesto(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionUsuarioDeUnPuesto(this, idPanelEdicion);
        }
    }
    Seguridad.CrudDeUsuariosDeUnPuesto = CrudDeUsuariosDeUnPuesto;
    class CrudCreacionUsuarioDeUnPuesto extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Seguridad.CrudCreacionUsuarioDeUnPuesto = CrudCreacionUsuarioDeUnPuesto;
    class CrudEdicionUsuarioDeUnPuesto extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Seguridad.CrudEdicionUsuarioDeUnPuesto = CrudEdicionUsuarioDeUnPuesto;
})(Seguridad || (Seguridad = {}));
//# sourceMappingURL=UsuariosDeUnPuesto.js.map