namespace Crud {

    export class ModalParaSeleccionar extends ModalConGrid {

        private _crud: CrudMnt;
        protected get Crud(): CrudMnt {
            return this._crud;
        }

        protected get PropiedadRestrictora(): string {
            return this.Modal.getAttribute(atControl.propiedadRestrictora);
        }

        protected get Restrictor(): HTMLInputElement {
            let propiedadRestrictora: string = this.PropiedadRestrictora;
            if (IsNullOrEmpty(propiedadRestrictora))
                throw new Error(`la modal ${this.IdModal} no tiene definida la ${propiedadRestrictora}`);

            let input: HTMLInputElement = this.ZonaDeFiltro.querySelector(`input[${atControl.propiedad}="${propiedadRestrictora}"]`);
            if (input === null)
                throw new Error(`No se ha definido el control input asociado a la ${propiedadRestrictora}`);

            return input;
        }

        constructor(crudPadre: CrudMnt, idModal: string) {
            super(idModal, document.getElementById(idModal).getAttribute(atControl.crudModal));
            this._crud = crudPadre;
        }

        public AbrirModalParaSeleccionar(selector: HTMLInputElement) {
            this.InicializarModalConGrid();
            //mapeo el texto de lo que haya en el selector a la zona de filtrado de la modal que me indique el selector
            this.RecargarGrid()
                .then((valor) => {
                    if (!valor)
                        ApiCrud.CerrarModal(this.Modal);
                })
                .catch((valor) => {
                    ApiCrud.CerrarModal(this.Modal);
                }
                );
        };

        public CerrarModalParaSeleccionar() {
            this.CerrarModalConGrid();
            this._crud.ModalEnviarCorreo_Abrir();
        }


    }
}
