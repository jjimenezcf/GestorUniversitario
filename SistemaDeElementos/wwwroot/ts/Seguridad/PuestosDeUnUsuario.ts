namespace Seguridad {

    export class CrudMntPuestosDeUnUsuario extends Crud.CrudMnt {
        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt);
            this.crudDeCreacion = new CrudCreacionPuestoDeUnUsuario(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionPuestoDeUnUsuario(this, idPanelEdicion);
            this.idModalBorrar = idModalBorrar;

            if (!NumeroMayorDeCero(sessionStorage[Restrictor.idUsuario])) {
                document.location.href = document.referrer;
            }
            else {

                this.MapearRestrictorDeFiltro(Restrictor.idUsuario, sessionStorage[Restrictor.idUsuario].Numero(), sessionStorage[Parametros.Usuario]);
                this.crudDeCreacion.MaperaRestrictorDeCreacion(Restrictor.idUsuario, sessionStorage[Restrictor.idUsuario].Numero(), sessionStorage[Parametros.Usuario]);
                this.crudDeEdicion.MaperaRestrictorDeEdicion(Restrictor.idUsuario, sessionStorage[Restrictor.idUsuario].Numero(), sessionStorage[Parametros.Usuario]);

                sessionStorage.removeItem(Restrictor.idUsuario);
                sessionStorage.removeItem(Parametros.Usuario);
            }
        }
    }

    export class CrudCreacionPuestoDeUnUsuario extends Crud.CrudCreacion {

        constructor(crud: Crud.CrudMnt, idPanelCreacion: string) {
            super(crud, idPanelCreacion);
        }

    }

    export class CrudEdicionPuestoDeUnUsuario extends Crud.CrudEdicion {

        constructor(crud: Crud.CrudMnt, idPanelEdicion: string) {
            super(crud, idPanelEdicion);
        }
    }
}