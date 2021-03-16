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
        AbrirModalConGrid() {
            this.RecargarGrid();
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
            this.DatosDelGrid.InicializarCache();
            this.CargarGrid(atGrid.accion.buscar, 0);
        }
    }
    Crud.ModalConGrid = ModalConGrid;
})(Crud || (Crud = {}));
//# sourceMappingURL=ModalConGrid.js.map