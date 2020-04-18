namespace Crud.Usuarios {

    export class CrudCreacionUsuario extends Crud.CrudCreacion {

        constructor(idPanelMnt: string, idPanelCreacion: string) {
            super(idPanelMnt, idPanelCreacion);
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

        constructor(idPanelMnt: string, idPanelCreacion: string) {
            super(idPanelMnt, idPanelCreacion);
        }
    }
}