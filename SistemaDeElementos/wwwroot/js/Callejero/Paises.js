var Callejero;
(function (Callejero) {
    function CrearCrudDePaises(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Callejero.CrudDePaises(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            MensajesSe.Info('llendo a trás');
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Callejero.CrearCrudDePaises = CrearCrudDePaises;
    class CrudDePaises extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionPais(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionPais(this, idPanelEdicion);
        }
    }
    Callejero.CrudDePaises = CrudDePaises;
    class CrudCreacionPais extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Callejero.CrudCreacionPais = CrudCreacionPais;
    class CrudEdicionPais extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Callejero.CrudEdicionPais = CrudEdicionPais;
})(Callejero || (Callejero = {}));
//# sourceMappingURL=Paises.js.map