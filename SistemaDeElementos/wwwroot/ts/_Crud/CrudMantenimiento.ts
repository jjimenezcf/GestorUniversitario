namespace Crud {

    export let crudMnt: CrudMnt = null;

    export class CrudMnt extends CrudBase {

        public crudDeCreacion: CrudCreacion;
        public crudDeEdicion: CrudEdicion;

        public PanelDeMnt: HTMLDivElement;
        public IdGrid: string;

        constructor(idPanelMnt: string) {
            super(ModoTrabajo.mantenimiento);

            if (EsNula(idPanelMnt))
                throw Error("No se puede construir un objeto del tipo CrudMantenimiento sin indica el panel de mantenimiento");
            this.IdGrid = `${idPanelMnt}_grid`;
            this.PanelDeMnt = document.getElementById(idPanelMnt) as HTMLDivElement;
        }

        InicializarValores() {
            
        }

    }

    export function EjecutarMenuMnt(accion: string): void {

        if (accion === LiteralMnt.crearelemento)
            IraCrear();
        else if (accion === LiteralMnt.editarelemento)
            IraEditar();
        else
            Mensaje(TipoMensaje.Info, `la opción ${accion} no está definida`);
    }

    export function AlPulsarUnCheckDeSeleccion(idGrid, idCheck) {
        BlanquearMensaje();
        var check = <HTMLInputElement>document.getElementById(idCheck);
        if (check.checked)
            AnadirAlInfoSelector(idGrid, idCheck);
        else
            QuitarDelSelector(idGrid, idCheck);
    }

    function IraEditar() {

        //obtener los elementos del grid seleccionado
        let idInfSel: string = crudMnt.IdGrid;
        let infSel = infoSelectores.Obtener(idInfSel);
        if (!infSel || infSel.Cantidad == 0) {
            Mensaje(TipoMensaje.Info, "Debe marcar el elemento a editar");
            return;
        }

        crudMnt.crudDeEdicion.ComenzarEdicion(crudMnt.PanelDeMnt, infSel);

    }

    function IraCrear() {
        crudMnt.crudDeCreacion.ComenzarCreacion(crudMnt.PanelDeMnt);
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