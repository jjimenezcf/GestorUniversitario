namespace Crud {

    export class ModalParaRelacionar extends ModalConGrid {

        constructor(idModal: string, idCrudModal: string) {
            super(idModal, idCrudModal);
        }

        public InicializarModalParaRelacionar() {
            super.InicializarModalConGrid();
        };

        public CerrarModalParaRelacionar() {
            this.CerrarModalConGrid();
        }

        public AbrirModalDeRelacion() {
            super.AbrirModalConGrid();             
        }

        CrearRelaciones() {
            throw new Error("Method not implemented.");
        }

    }
}