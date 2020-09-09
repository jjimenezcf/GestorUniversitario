namespace Seguridad {

    export function CrearCrudDeRolesDeUnPermiso(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
        Crud.crudMnt = new Seguridad.CrudDeRolesDeUnPermiso(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        Crud.crudMnt.Inicializar();
    }

    export class CrudDeRolesDeUnPermiso extends Crud.CrudMnt {
        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionRolesDeUnPermiso(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionRolesDeUnPermiso(this, idPanelEdicion);
        }
    }

    export class CrudCreacionRolesDeUnPermiso extends Crud.CrudCreacion {

        constructor(crud: Crud.CrudMnt, idPanelCreacion: string) {
            super(crud, idPanelCreacion);
        }

    }

    export class CrudEdicionRolesDeUnPermiso extends Crud.CrudEdicion {

        constructor(crud: Crud.CrudMnt, idPanelEdicion: string) {
            super(crud, idPanelEdicion);
        }
    }
}