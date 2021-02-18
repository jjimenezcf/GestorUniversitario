var Crud;
(function (Crud) {
    class ModalConGrid extends Crud.GridDeDatos {
        constructor(idModal, idCrudModal) {
            super(idCrudModal);
            this._idModal = idModal;
        }
        get IdModal() { return this._idModal; }
        ;
        get Modal() {
            return document.getElementById(this._idModal);
        }
        ;
        InicializarModalConGrid() {
            let referenciaCheck = `chksel.${this.IdGrid}`;
            this.blanquearCheck(referenciaCheck);
            this.InfoSelector.QuitarTodos();
        }
        ;
        AbrirModalConGrid() {
            BlanquearMensaje();
            this.RecargarGrid();
            this.Modal.style.display = 'block';
        }
        CerrarModalConGrid() {
            let referenciaCheck = `chksel.${this.IdGrid}`;
            this.blanquearCheck(referenciaCheck);
            this.InfoSelector.QuitarTodos();
            ApiCrud.CerrarModal(this.Modal);
        }
        blanquearCheck(refCheckDeSeleccion) {
            document.getElementsByName(`${refCheckDeSeleccion}`).forEach(c => {
                let check = c;
                check.checked = false;
            });
        }
        RecargarGrid() {
            BlanquearMensaje();
            this.CargarGrid(atGrid.accion.buscar, 0);
        }
    }
    Crud.ModalConGrid = ModalConGrid;
})(Crud || (Crud = {}));
//# sourceMappingURL=ModalConGrid.js.map