namespace Auditoria {

    export function CrearCrudDeAuditoria(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
        Crud.crudMnt = new Auditoria.CrudDeAuditoria(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);

        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser() ;
        };
    }

    export class CrudDeAuditoria extends Crud.CrudMnt {

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionAuditoria(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionAuditoria(this, idPanelEdicion);
        }
    }
    export class CrudCreacionAuditoria extends Crud.CrudCreacion {
        constructor(crud: Crud.CrudMnt, idPanelCreacion: string) {
            super(crud, idPanelCreacion);
        }
    }
    export class CrudEdicionAuditoria extends Crud.CrudEdicion {
        constructor(crud: Crud.CrudMnt, idPanelEdicion: string) {
            super(crud, idPanelEdicion);
        }

        protected ParametrosOpcionalesLeerPorId(): Array<Parametro>  {
            let parametros: Array<Parametro> = super.ParametrosOpcionalesLeerPorId();
            let parametro: Parametro = new Parametro(literal.idNegocio.replace('-', ''), this.CrudDeMnt.IdNegocio);
            parametros.push(parametro);
            return parametros;
        }
    }
}