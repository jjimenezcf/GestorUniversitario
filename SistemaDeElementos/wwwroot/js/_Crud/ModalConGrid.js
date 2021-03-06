var Crud;
(function (Crud) {
    class ModalConGrid extends Crud.GridDeDatos {
        constructor(idModal, idCrudModal) {
            super(idCrudModal);
            this.IdModal = idModal;
        }
        InicializarModalConGrid() {
            let referenciaCheck = `chksel.${this.IdGrid}`;
            this.blanquearCheck(referenciaCheck);
            this.InfoSelector.QuitarTodos();
        }
        ;
        CerrarModalConGrid() {
            try {
                this.ResetearSoloSeleccionadas();
                let referenciaCheck = `chksel.${this.IdGrid}`;
                this.blanquearCheck(referenciaCheck);
                this.InfoSelector.QuitarTodos();
            }
            finally {
                ApiCrud.CerrarModal(this.Modal);
            }
        }
        blanquearCheck(refCheckDeSeleccion) {
            document.getElementsByName(`${refCheckDeSeleccion}`).forEach(c => {
                let check = c;
                check.checked = false;
            });
        }
    }
    Crud.ModalConGrid = ModalConGrid;
})(Crud || (Crud = {}));
//# sourceMappingURL=ModalConGrid.js.map