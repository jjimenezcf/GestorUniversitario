namespace Crud {

    export let crudMnt: CrudMnt = null;

    export class CrudMnt extends CrudBase {

        public crudDeCreacion: CrudCreacion;
        public crudDeEdicion: CrudEdicion;

        public PanelDeMnt: HTMLDivElement;
        public IdGrid: string;
        private infSel: InfoSelector;

        constructor(idPanelMnt: string) {
            super();

            if (EsNula(idPanelMnt))
                throw Error("No se puede construir un objeto del tipo CrudMantenimiento sin indica el panel de mantenimiento");

            this.IdGrid = `${idPanelMnt}_grid`;
            this.PanelDeMnt = document.getElementById(idPanelMnt) as HTMLDivElement;
            this.infSel = new InfoSelector(this.IdGrid);
        }

        public IraEditar() {
            if (this.infSel.Cantidad == 0) {
                Mensaje(TipoMensaje.Info, "Debe marcar el elemento a editar");
                return;
            }

            this.crudDeEdicion.ComenzarEdicion(crudMnt.PanelDeMnt, this.infSel);
        }

        public IraCrear() {
            this.crudDeCreacion.ComenzarCreacion(crudMnt.PanelDeMnt);
        }

        public AlPulsarUnCheckDeSeleccion(idCheck) {
            BlanquearMensaje();
            var check = <HTMLInputElement>document.getElementById(idCheck);
            if (check.checked)
                this.AnadirAlInfoSelector(idCheck);
            else
                this.QuitarDelSelector(idCheck);
        }

        private AnadirAlInfoSelector(idCheck) {
            var id = ObtenerIdDeLaFilaChequeada(idCheck);
            this.infSel.InsertarId(id);
        }

        private QuitarDelSelector(idCheck) {
            var id = ObtenerIdDeLaFilaChequeada(idCheck);
            this.infSel.Quitar(id);
        }
    }

    export function EjecutarMenuMnt(accion: string): void {

        if (accion === LiteralMnt.crearelemento)
            crudMnt.IraCrear();
        else if (accion === LiteralMnt.editarelemento)
            crudMnt.IraEditar();
        else
            Mensaje(TipoMensaje.Info, `la opción ${accion} no está definida`);
    }

    export function AlPulsarUnCheckDeSeleccion(idGrid, idCheck) {
        if (crudMnt.IdGrid != idGrid) {
            BlanquearMensaje();
            var check = <HTMLInputElement>document.getElementById(idCheck);
            if (check.checked)
                AnadirAlInfoSelector(idGrid, idCheck);
            else
                QuitarDelSelector(idGrid, idCheck);
        }
        else
            crudMnt.AlPulsarUnCheckDeSeleccion(idCheck);
    }

    function AnadirAlInfoSelector(idGrid, idCheck) {

        var infSel = infoSelectores.Obtener(idGrid);
        if (infSel === undefined) {
            infSel = infoSelectores.Crear(idGrid);
        }

        var id = ObtenerIdDeLaFilaChequeada(idCheck);
        if (infSel.EsModalDeSeleccion) {
            var textoMostrar = obtenerValorDeLaColumnaChequeada(idCheck, infSel.ColumnaMostrar);
            infSel.InsertarElemento(id, textoMostrar);
        }
        else {
            infSel.InsertarId(id);
        }

    }

    function QuitarDelSelector(idGrid, idCheck) {

        var infSel = infoSelectores.Obtener(idGrid);
        if (infSel !== undefined) {
            var id = ObtenerIdDeLaFilaChequeada(idCheck);
            infSel.Quitar(id);
        }
        else
            Mensaje(TipoMensaje.Error, `El selector ${idGrid} no está definido`);
    }

}