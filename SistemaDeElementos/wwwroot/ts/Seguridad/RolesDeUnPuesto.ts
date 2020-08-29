namespace Seguridad {

    export function CrearCrudDeRolesDeUnPuesto(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
        Crud.crudMnt = new Seguridad.CrudDeRolesDeUnPuesto(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        Crud.crudMnt.Inicializar();
    }

      export class CrudDeRolesDeUnPuesto extends Crud.CrudMnt {
        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionRolDeUnPuesto(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionRolDeUnPuesto(this, idPanelEdicion);
        }
    }

    export class CrudCreacionRolDeUnPuesto extends Crud.CrudCreacion {

        constructor(crud: Crud.CrudMnt, idPanelCreacion: string) {
            super(crud, idPanelCreacion);
        }

    }

    export class CrudEdicionRolDeUnPuesto extends Crud.CrudEdicion {

        constructor(crud: Crud.CrudMnt, idPanelEdicion: string) {
            super(crud, idPanelEdicion);
        }
    }
}