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

        protected CerrarModalConGrid() {
            try {
                this.ResetearSoloSeleccionadas();
                let referenciaCheck: string = `chksel.${this.IdGrid}`;
                this.blanquearCheck(referenciaCheck);
                this.InfoSelector.QuitarTodos();
            }
            finally {
                ApiCrud.CerrarModal(this.Modal);
            }
        }

        private blanquearCheck(refCheckDeSeleccion: string) {
            document.getElementsByName(`${refCheckDeSeleccion}`).forEach(c => {
                let check = <HTMLInputElement>c;
                check.checked = false;
            }
            );
        }

        //public RecargarGrid(): Promise<boolean> {
        //    this.DatosDelGrid.InicializarCache();
        //    return this.PromesaDeCargarGrid(atGrid.accion.buscar, 0);
        //}

    }
}