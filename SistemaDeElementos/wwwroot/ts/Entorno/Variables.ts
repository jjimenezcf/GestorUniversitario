namespace Entorno {

    export class CrudMntVariable extends Crud.CrudMnt {

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt);
            this.crudDeCreacion = new CrudCreacionVariable(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionVariable(this, idPanelEdicion);
            this.idModalBorrar = idModalBorrar;
        }
    }

    export class CrudCreacionVariable extends Crud.CrudCreacion {

        constructor(crud: Crud.CrudMnt, idPanelCreacion: string) {
            super(crud, idPanelCreacion);
        }

    }

    export class CrudEdicionVariable extends Crud.CrudEdicion {

        constructor(crud: Crud.CrudMnt, idPanelEdicion: string) {
            super(crud, idPanelEdicion);
        }
    }}