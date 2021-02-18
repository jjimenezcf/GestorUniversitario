var Entorno;
(function (Entorno) {
    function CrearCrudDeUsuarios(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Entorno.CrudDeUsuarios(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () {
            Crud.crudMnt.Inicializar(idPanelMnt);
        }, false);
    }
    Entorno.CrearCrudDeUsuarios = CrearCrudDeUsuarios;
    class CrudDeUsuarios extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionUsuario(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionUsuario(this, idPanelEdicion);
        }
    }
    Entorno.CrudDeUsuarios = CrudDeUsuarios;
    class CrudCreacionUsuario extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
        DespuesDeMapearDatosDeIU(crud, panel, elementoJson, modoDeTrabajo) {
            super.DespuesDeMapearDatosDeIU(crud, panel, elementoJson, modoDeTrabajo);
            return elementoJson;
        }
    }
    Entorno.CrudCreacionUsuario = CrudCreacionUsuario;
    class CrudEdicionUsuario extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
        AntesDeMapearDatosDeIU(crud, panel, modoDeTrabajo) {
            let json = super.AntesDeMapearDatosDeIU(crud, panel, modoDeTrabajo);
            console.log('funciona');
            return json;
        }
    }
    Entorno.CrudEdicionUsuario = CrudEdicionUsuario;
})(Entorno || (Entorno = {}));
//# sourceMappingURL=Usuario.js.map