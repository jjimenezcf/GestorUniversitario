namespace Crud {

    export class ModalConGrid extends GridDeDatos {


        private _idModal: string;
        public get IdModal(): string { return this._idModal; };
        protected get Modal(): HTMLDivElement {
            return document.getElementById(this._idModal) as HTMLDivElement;
        };

        constructor(idModal: string, idCrudModal: string) {
            super(idCrudModal);
            this._idModal = idModal;
        }

        protected InicializarModalConGrid() {
            let referenciaCheck: string = `chksel.${this.IdGrid}`;
            this.blanquearCheck(referenciaCheck);
            this.InfoSelector.QuitarTodos();
        };

        protected AbrirModalConGrid() {
            BlanquearMensaje();
            this.RecargarGrid();
            this.Modal.style.display = 'block';
        }

        protected CerrarModalConGrid() {
            let referenciaCheck: string = `chksel.${this.IdGrid}`;
            this.blanquearCheck(referenciaCheck);
            this.InfoSelector.QuitarTodos();
            this.CerrarModal(this.Modal);
        }

        private blanquearCheck(refCheckDeSeleccion: string) {
            document.getElementsByName(`${refCheckDeSeleccion}`).forEach(c => {
                let check = <HTMLInputElement>c;
                check.checked = false;
            }
            );
        }

        public RecargarGrid() {
            BlanquearMensaje();
            this.CargarGrid(atGrid.accion.buscar, 0);
        }

    }
}