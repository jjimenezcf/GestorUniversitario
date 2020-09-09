namespace Seguridad {

    export function CrearCrudDeUsuariosDeUnPuesto(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
        Crud.crudMnt = new Seguridad.CrudDeUsuariosDeUnPuesto(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        Crud.crudMnt.Inicializar();
    }

    export class CrudDeUsuariosDeUnPuesto extends Crud.CrudMnt {

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionUsuarioDeUnPuesto(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionUsuarioDeUnPuesto(this, idPanelEdicion);
        }
    }

    export class CrudCreacionUsuarioDeUnPuesto extends Crud.CrudCreacion {

        constructor(crud: Crud.CrudMnt, idPanelCreacion: string) {
            super(crud, idPanelCreacion);
        }

    }

    export class CrudEdicionUsuarioDeUnPuesto extends Crud.CrudEdicion {

        constructor(crud: Crud.CrudMnt, idPanelEdicion: string) {
            super(crud, idPanelEdicion);
        }
    }
}