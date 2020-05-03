namespace Seguridad {

    export class CrudMntPermiso extends Crud.CrudMnt {

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt);
            this.crudDeCreacion = new CrudCreacionPermiso(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionPermiso(this, idPanelEdicion);
            this.idModalBorrar = idModalBorrar;
        }
    }

    export class CrudCreacionPermiso extends Crud.CrudCreacion {

        constructor(crud: Crud.CrudMnt, idPanelCreacion: string) {
            super(crud, idPanelCreacion);
        }

    }

    export class CrudEdicionPermiso extends Crud.CrudEdicion {

        constructor(crud: Crud.CrudMnt, idPanelEdicion: string) {
            super(crud, idPanelEdicion);
        }
    }
}