namespace Crud {

    export class ModalParaRelacionar extends ModalConGrid {

        private _crud: CrudMnt;
        protected get Crud(): CrudMnt {
            return this._crud;
        }

        constructor(crudPadre: CrudMnt, idModal: string) {
            super(idModal, document.getElementById(idModal).getAttribute(atControl.crudModal));
            this._crud = crudPadre;
        }

        public InicializarModalParaRelacionar() {
            super.InicializarModalConGrid();
        };

        public CerrarModalParaRelacionar() {
            this.CerrarModalConGrid();
        }

        public AbrirModalDeRelacion() {
            if (this.Crud.InfoSelector.Cantidad != 1)
                throw new Error(`Debe seleccionar el elemento a relacionar, ha seleccionado ${this.InfoSelector.Cantidad}`);

            super.AbrirModalConGrid();             
        }

        public CrearRelaciones() {
            this.CerrarModalConGrid();
        }

    }
}