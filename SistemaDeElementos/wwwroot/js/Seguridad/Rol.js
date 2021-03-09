var Seguridad;
(function (Seguridad) {
    function CrearCrudDeRoles(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Seguridad.CrudDeRoles(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            MensajesSe.Info('llendo a trás');
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Seguridad.CrearCrudDeRoles = CrearCrudDeRoles;
    class CrudDeRoles extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionRol(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionRol(this, idPanelEdicion);
        }
    }
    Seguridad.CrudDeRoles = CrudDeRoles;
    class CrudCreacionRol extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Seguridad.CrudCreacionRol = CrudCreacionRol;
    class CrudEdicionRol extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Seguridad.CrudEdicionRol = CrudEdicionRol;
})(Seguridad || (Seguridad = {}));
//# sourceMappingURL=Rol.js.map