namespace Crud {

    export class CrudEdicion extends CrudBase {

        protected PanelDeEditar: HTMLDivElement;
        protected PanelDeMnt: HTMLDivElement;
        protected CrudDeMnt: CrudMnt;

        constructor(crud: CrudMnt, idPanelEdicion: string) {
            super();

            if (EsNula(idPanelEdicion))
                throw Error("No se puede construir un objeto del tipo CrudEdicion sin indica el panel de edición");

            this.PanelDeEditar = document.getElementById(idPanelEdicion) as HTMLDivElement;
            this.PanelDeMnt = crud.PanelDeMnt;
            this.CrudDeMnt = crud;
        }

        public EjecutarAcciones(accion: string) {
            let hayError: boolean = false;
            try {
                if (accion === LiteralEdt.modificarelemento)
                    this.Modificar();
                else
                    if (accion === LiteralEdt.cancelaredicion)
                        hayError = false;
                    else
                        throw `la opción ${accion} no está definida`;
            }
            catch (error) {
                hayError = true;
                Mensaje(TipoMensaje.Error, error);
            }

            if (!hayError) this.CerrarEdicion();
        }

        public ComenzarEdicion(panelAnterior: HTMLDivElement, infSel: InfoSelector) {
            this.OcultarPanel(panelAnterior);
            this.MostrarPanel(this.PanelDeEditar);
            this.InicializarValores(infSel);
        }

        protected CerrarEdicion() {
            this.Cerrar(this.PanelDeMnt, this.PanelDeEditar);
            this.CrudDeMnt.Buscar(0);
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

        protected Modificar() {
            let json: JSON = this.MapearControlesDeIU(this.PanelDeEditar);
            this.ModificarElemento(json);
        }

        private ModificarElemento(json: JSON) {
            let controlador = this.PanelDeEditar.getAttribute(Literal.controlador);
            let url: string = `/${controlador}/${Ajax.EndPoint.Modificar}?${Ajax.Param.elementoJson}=${JSON.stringify(json)}`;
            let req: XMLHttpRequest = new XMLHttpRequest();
            this.PeticionSincrona(req, url, Ajax.EndPoint.Modificar);
        }

        protected DespuesDeLaPeticion(req: XMLHttpRequest, peticion: string): ResultadoJson {
            let resultado = super.DespuesDeLaPeticion(req, peticion);
            this.MapearElemento(this.PanelDeEditar, resultado.datos);
            return resultado;
        }

    }

    export function EjecutarMenuEdt(accion: string): void {
        crudMnt.crudDeEdicion.EjecutarAcciones(accion);
    }

}