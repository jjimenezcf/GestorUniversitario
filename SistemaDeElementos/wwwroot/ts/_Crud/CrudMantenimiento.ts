namespace Crud {

    export class CrudMnt extends CrudBase {

        protected crudDeCreacion: CrudCreacion;
        protected crudDeEdicion: CrudEdicion;

        protected PanelDeMnt: HTMLDivElement;

        constructor(idPanelMnt: string) {
            super(ModoTrabajo.consultando);

            if (EsNula(idPanelMnt))
                throw Error("No se puede construir un objeto del tipo CrudMantenimiento sin indica el panel de mantenimiento");

            this.PanelDeMnt = document.getElementById(idPanelMnt) as HTMLDivElement;
        }

        InicializarValores() {
            
        }

    }

    export function EjecutarMenuMnt(accion: string, idDivMnt: string, gestor: Crud.CrudBase): void {

        if (accion === LiteralMnt.crearelemento)
            IraCrear(gestor as Crud.CrudCreacion, idDivMnt);
        else if (accion === LiteralMnt.editarelemento)
            IraEditar(gestor as Crud.CrudEdicion, idDivMnt);
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

    function IraEditar(gestorDeEdicion: Crud.CrudEdicion, idDivMnt: string) {

        //obtener los elementos del grid seleccionado
        let idInfSel: string = `${idDivMnt}_grid`;
        let infSel = infoSelectores.Obtener(idInfSel);
        if (!infSel || infSel.Cantidad == 0) {
            Mensaje(TipoMensaje.Info, "Debe marcar el elemento a editar");
            return;
        }

        let panelMnt: HTMLDivElement = document.getElementById(`${idDivMnt}`) as HTMLDivElement;

        gestorDeEdicion.ComenzarEdicion(panelMnt, infSel);

        if (!EsNula(gestorDeEdicion.ResultadoPeticion)) {
            Mensaje(gestorDeEdicion.PeticioCorrecta ? TipoMensaje.Info : TipoMensaje.Error, gestorDeEdicion.ResultadoPeticion);
        }
    }

    function IraCrear(gestorDeCreacion: Crud.CrudCreacion, idDivMnt: string) {

        let panelMnt: HTMLDivElement = document.getElementById(`${idDivMnt}`) as HTMLDivElement;
        gestorDeCreacion.ComenzarCreacion(panelMnt);
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