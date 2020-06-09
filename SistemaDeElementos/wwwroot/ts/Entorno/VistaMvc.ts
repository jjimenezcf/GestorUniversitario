namespace Entorno {

    export class CrudMntVistaMvc extends Crud.CrudMnt {

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt);
            this.crudDeCreacion = new CrudCreacionVistaMvc(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionVistaMvc(this, idPanelEdicion);
            this.idModalBorrar = idModalBorrar;
        }
    }

    export class CrudCreacionVistaMvc extends Crud.CrudCreacion {

        constructor(crud: Crud.CrudMnt, idPanelCreacion: string) {
            super(crud, idPanelCreacion);
        }

    }

    export class CrudEdicionVistaMvc extends Crud.CrudEdicion {

        constructor(crud: Crud.CrudMnt, idPanelEdicion: string) {
            super(crud, idPanelEdicion);
        }
    }
}