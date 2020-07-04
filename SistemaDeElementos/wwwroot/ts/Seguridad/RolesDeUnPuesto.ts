namespace Seguridad {

    export class CrudMntRolesDeUnPuesto extends Crud.CrudMnt {
        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt);
            this.crudDeCreacion = new CrudCreacionRolDeUnPuesto(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionRolDeUnPuesto(this, idPanelEdicion);
            this.idModalBorrar = idModalBorrar;

            if (!NumeroMayorDeCero(sessionStorage[Restrictor.idPuesto])) {
                document.location.href = document.referrer;
            }
            else {

                this.MapearRestrictorDeFiltro(Restrictor.idPuesto, sessionStorage[Restrictor.idPuesto].Numero(), sessionStorage[Parametros.Puesto]);
                this.crudDeCreacion.MaperaRestrictorDeCreacion(Restrictor.idPuesto, sessionStorage[Restrictor.idPuesto].Numero(), sessionStorage[Parametros.Puesto]);
                this.crudDeEdicion.MaperaRestrictorDeEdicion(Restrictor.idPuesto, sessionStorage[Restrictor.idPuesto].Numero(), sessionStorage[Parametros.Puesto]);

                sessionStorage.removeItem(Restrictor.idPuesto);
                sessionStorage.removeItem(Parametros.Puesto);
            }
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