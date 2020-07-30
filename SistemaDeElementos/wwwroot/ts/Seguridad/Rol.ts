namespace Seguridad {

    export function CrearCrudDeRoles(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
        Crud.crudMnt = new Seguridad.CrudDeRoles(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        Crud.crudMnt.RenderGrid = false;
        Crud.crudMnt.LeerDatosParaElGrid(Variables.Grid.accion.buscar, 0);
    }

    export class CrudDeRoles extends Crud.CrudMnt {

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt);
            this.crudDeCreacion = new CrudCreacionRol(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionRol(this, idPanelEdicion);
            this.idModalBorrar = idModalBorrar;
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