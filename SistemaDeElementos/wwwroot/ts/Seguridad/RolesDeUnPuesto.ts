namespace Seguridad {

    export function CrearCrudDeRolesDeUnPuesto(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
        Crud.crudMnt = new Seguridad.CrudDeRolesDeUnPuesto(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        Crud.crudMnt.RenderGrid = false;
        Crud.crudMnt.Buscar(atGrid.accion.buscar, 0);
    }

      export class CrudDeRolesDeUnPuesto extends Crud.CrudMnt {
        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt);
            this.crudDeCreacion = new CrudCreacionRolDeUnPuesto(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionRolDeUnPuesto(this, idPanelEdicion);
            this.idModalBorrar = idModalBorrar;

            if (this.HayHistorial)
                this.RecuperarFiltros();
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