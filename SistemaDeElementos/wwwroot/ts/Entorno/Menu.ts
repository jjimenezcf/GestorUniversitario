namespace Entorno {

    export function CrearCrudDeMenus(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
        Crud.crudMnt = new Entorno.CrudDeMenus(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        Crud.crudMnt.RenderGrid = false;
        Crud.crudMnt.Buscar(atGrid.accion.buscar, 0);
    }

    export class CrudDeMenus extends Crud.CrudMnt {

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt);
            this.crudDeCreacion = new CrudCreacionMenu(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionMenu(this, idPanelEdicion);
            this.idModalBorrar = idModalBorrar;
        }
    }

    export class CrudCreacionMenu extends Crud.CrudCreacion {

        constructor(crud: Crud.CrudMnt, idPanelCreacion: string) {
            super(crud, idPanelCreacion);
        }

    }

    export class CrudEdicionMenu extends Crud.CrudEdicion {

        constructor(crud: Crud.CrudMnt, idPanelEdicion: string) {
            super(crud, idPanelEdicion);
        }
    }
}