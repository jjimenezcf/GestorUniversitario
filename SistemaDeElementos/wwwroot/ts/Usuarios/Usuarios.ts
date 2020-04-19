namespace Usuarios {

    export class CrudMntUsuario extends Crud.CrudMnt {

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string) {
            super(idPanelMnt);
            this.crudDeCreacion = new CrudCreacionUsuario(this.PanelDeMnt,idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionUsuario(this.PanelDeMnt,idPanelEdicion);
        }
    }

    export class CrudCreacionUsuario extends Crud.CrudCreacion {

        constructor(panelMnt: HTMLDivElement, idPanelCreacion: string) {
            super(panelMnt, idPanelCreacion);
        }

    }

    export class CrudEdicionUsuario extends Crud.CrudEdicion {

        constructor(panelMnt: HTMLDivElement, idPanelEdicion: string) {
            super(panelMnt, idPanelEdicion);
        }
    }
}