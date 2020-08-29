namespace Seguridad {

    export function CrearCrudDeRoles(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
        Crud.crudMnt = new Seguridad.CrudDeRoles(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        Crud.crudMnt.Inicializar();
    }

    export class CrudDeRoles extends Crud.CrudMnt {

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionRol(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionRol(this, idPanelEdicion);
        }
    }

    export class CrudCreacionRol extends Crud.CrudCreacion {

        constructor(crud: Crud.CrudMnt, idPanelCreacion: string) {
            super(crud, idPanelCreacion);
        }

    }

    export class CrudEdicionRol extends Crud.CrudEdicion {

        constructor(crud: Crud.CrudMnt, idPanelEdicion: string) {
            super(crud, idPanelEdicion);
        }
    }
}