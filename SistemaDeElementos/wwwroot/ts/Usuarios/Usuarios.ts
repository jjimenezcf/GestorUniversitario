namespace Crud.Usuarios {

    export class CrudCreacionUsuario extends Crud.CrudCreacion {

        constructor(idPanelMnt: string, idPanelCreacion: string) {
            super(idPanelMnt, idPanelCreacion);
        }

        public InicializarValores() {
            super.InicializarValores();
            let inputFechaAlta: HTMLInputElement = this.divDeCreacionHtml.querySelector<HTMLInputElement>("#Alta");
            let fecha: Date = new Date();
            inputFechaAlta.value = fecha.toDateString();
        }

        protected DespuesDeMapearDatosDeIU(json: JSON): JSON {
            json = super.DespuesDeMapearDatosDeIU(json);

            /*código específico para usuariosDto*/

            return json;
        }

        protected AntesDeMapearDatosDeIU(): JSON {
            let json: JSON = super.AntesDeMapearDatosDeIU();

            /*código específico para usuariosDto*/

            return json;
        }

    }

    export class CrudEdicionUsuario extends Crud.CrudEdicion {

        constructor(idPanelMnt: string, idPanelCreacion: string) {
            super(idPanelMnt, idPanelCreacion);
        }
    }
}