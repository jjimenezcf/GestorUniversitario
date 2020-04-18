namespace Crud {

    export class CrudEdicion extends CrudBase {

        protected PanelDeEditar: HTMLDivElement;

        constructor(idPanelEdicion: string) {
            super(ModoTrabajo.editando);

            if (EsNula(idPanelEdicion))
                throw Error("No se puede construir un objeto del tipo CrudEdicion sin indica el panel de edición")

            this.PanelDeEditar = document.getElementById(idPanelEdicion) as HTMLDivElement;
        }

        public ComenzarEdicion(panelAnterior: HTMLDivElement, infSel: InfoSelector) {
            this.OcultarPanel(panelAnterior);
            this.MostrarPanel(this.PanelDeEditar);
            this.InicializarValores(infSel);
        }

        public CerrarEdicion(panelMostrar: HTMLDivElement) {
                this.Cerrar(panelMostrar, this.PanelDeEditar);
        }

        protected InicializarValores(infSel: InfoSelector) {
            let id: number = infSel.Seleccionados[0] as number;

            let control: HTMLElement = this.BuscarControl(this.PanelDeEditar, Literal.id);
            if (control == null) {
                Mensaje(TipoMensaje.Error, "No está definido el control para mostrar el id del elemento");
                return;
            }
            (control as HTMLInputElement).value = id.toString();

            this.LeerElemento(id);
        }

        private LeerElemento(id: number) {
            let idJson: JSON = JSON.parse(`[${id}]`);

            let controlador = this.PanelDeEditar.getAttribute(Literal.controlador);
            let url: string = `/${controlador}/${Ajax.EndPoint.LeerPorIds}?${Ajax.Param.idsJson}=${JSON.stringify(idJson)}`;

            let req: XMLHttpRequest = new XMLHttpRequest();
            this.PeticionSincrona(req, url, Ajax.EndPoint.LeerPorIds);
        }

        public Modificar(panelMnt: HTMLDivElement) {
            let json: JSON = null;
            try {
                json = this.MapearControlesDeIU(this.PanelDeEditar);
            }
            catch (error) {
                this.ResultadoPeticion = error.message;
                return;
            }
            this.ModificarElemento(json);

            if (this.PeticioCorrecta) {
                this.CerrarEdicion(panelMnt);
            }
        }

        private ModificarElemento(json: JSON) {
            let controlador = this.PanelDeEditar.getAttribute(Literal.controlador);
            let url: string = `/${controlador}/${Ajax.EndPoint.Modificar}?${Ajax.Param.elementoJson}=${JSON.stringify(json)}`;
            let req: XMLHttpRequest = new XMLHttpRequest();
            this.PeticionSincrona(req, url, Ajax.EndPoint.Modificar);
        }

        protected DespuesDeLaPeticion(req): ResultadoJson {
            let resultado = super.DespuesDeLaPeticion(req);
            this.MapearElemento(this.PanelDeEditar, resultado.datos);
            return resultado;
        }

    }

    export function EjecutarMenuEdt(accion: string): void {

        if (accion === LiteralEdt.modificarelemento)
            ModificarElemento();
        else
        if (accion === LiteralEdt.cancelaredicion)
            CancelarEdicion();
        else
            Mensaje(TipoMensaje.Info, `la opción ${accion} no está definida`);
    }

    function ModificarElemento() {
        crudMnt.crudDeEdicion.Modificar(crudMnt.PanelDeMnt);

    }

    function CancelarEdicion() {
        try {
            crudMnt.crudDeEdicion.CerrarEdicion(crudMnt.PanelDeMnt);
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error.menssage);
        }
    }
}