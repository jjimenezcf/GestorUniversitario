namespace Usuarios {

    export class CrudMntUsuario extends Crud.CrudMnt {

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string) {
            super(idPanelMnt);
            this.crudDeCreacion = new CrudCreacionUsuario(idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionUsuario(idPanelEdicion);
        }
    }

    export class CrudCreacionUsuario extends Crud.CrudCreacion {

        constructor(idPanelCreacion: string) {
            super(idPanelCreacion);
        }

        protected DespuesDeMapearDatosDeIU(panel: HTMLDivElement, elementoJson: JSON): JSON {
            elementoJson = super.DespuesDeMapearDatosDeIU(panel, elementoJson);

            /*código específico para usuariosDto*/

            return elementoJson;
        }

        protected AntesDeMapearDatosDeIU(panel: HTMLDivElement): JSON {
            let elementoJson: JSON = super.AntesDeMapearDatosDeIU(panel);

            /*código específico para usuariosDto*/

            return elementoJson;
        }

    }

    export class CrudEdicionUsuario extends Crud.CrudEdicion {

        constructor(idPanelCreacion: string) {
            super(idPanelCreacion);
        }
    }
}