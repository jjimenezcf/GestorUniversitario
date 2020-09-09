namespace Seguridad {

    export function CrearCrudDePuestosDeUnRol(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
        Crud.crudMnt = new Seguridad.CrudDePuestosDeUnRol(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        Crud.crudMnt.Inicializar();
    }

      export class CrudDePuestosDeUnRol extends Crud.CrudMnt {
        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionPuestoDeUnRol(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionPuestoDeUnRol(this, idPanelEdicion);
        }
    }

    export class CrudCreacionPuestoDeUnRol extends Crud.CrudCreacion {

        constructor(crud: Crud.CrudMnt, idPanelCreacion: string) {
            super(crud, idPanelCreacion);
        }

    }

    export class CrudEdicionPuestoDeUnRol extends Crud.CrudEdicion {

        constructor(crud: Crud.CrudMnt, idPanelEdicion: string) {
            super(crud, idPanelEdicion);
        }
    }
}