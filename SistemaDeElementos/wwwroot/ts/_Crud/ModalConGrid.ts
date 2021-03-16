namespace Crud {

    export class ModalConGrid extends GridDeDatos {

        constructor(idModal: string, idCrudModal: string) {
            super(idCrudModal);
            this.IdModal = idModal;
        }

        protected InicializarModalConGrid() {
            let referenciaCheck: string = `chksel.${this.IdGrid}`;
            this.blanquearCheck(referenciaCheck);
            this.InfoSelector.QuitarTodos();
        };

        protected AbrirModalConGrid() {
            this.RecargarGrid();
        }

        protected CerrarModalConGrid() {
            let referenciaCheck: string = `chksel.${this.IdGrid}`;
            this.blanquearCheck(referenciaCheck);
            this.InfoSelector.QuitarTodos();
            ApiCrud.CerrarModal(this.Modal);
        }

        private blanquearCheck(refCheckDeSeleccion: string) {
            document.getElementsByName(`${refCheckDeSeleccion}`).forEach(c => {
                let check = <HTMLInputElement>c;
                check.checked = false;
            }
            );
        }

        public RecargarGrid() {
            this.DatosDelGrid.InicializarCache();
            this.CargarGrid(atGrid.accion.buscar, 0);
        }

    }
}