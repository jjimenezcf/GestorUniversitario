namespace Crud {

    export class CrudCreacion extends CrudBase {

        private _idPanelCreacion: string;

        protected PanelDeMnt: HTMLDivElement;
        protected CrudDeMnt: CrudMnt;

        protected get PanelDeCrear(): HTMLDivElement {
            return document.getElementById(this._idPanelCreacion) as HTMLDivElement;
        }

        private get EsModal(): boolean {
            return this.PanelDeCrear.className === ClaseCss.contenedorModal;
        }

        private get Controlador(): string {
            return this.PanelDeCrear.getAttribute(Literal.controlador);
        }

        constructor(crud: CrudMnt, idPanelCreacion: string) {
            super();

            if (EsNula(idPanelCreacion))
                throw Error("No se puede construir un objeto del tipo CrudCreacion sin indica el panel de creación");

            this._idPanelCreacion = idPanelCreacion;
            this.PanelDeMnt = crud.PanelDeMnt;
            this.CrudDeMnt = crud;
        }


        public EjecutarAcciones(accion: string) {
            let hayError: boolean = false;
            try {
                if (accion === LiteralCrt.Accion.NuevoElemento)
                    this.Crear();
                else
                    if (accion === LiteralCrt.Accion.CancelarNuevo)
                        hayError = false;
                    else
                        throw `la opción ${accion} no está definida`;
            }
            catch (error) {
                hayError = true;
                Mensaje(TipoMensaje.Error, error);
            }

            if (!hayError) this.CerrarCreacion();
        }

        public ComenzarCreacion(panelAnterior: HTMLDivElement) {
            this.ModoTrabajo = ModoTrabajo.creando;

            if (this.EsModal) {
                var ventana = document.getElementById(this._idPanelCreacion);
                ventana.style.display = 'block';
            }
            else {

                this.OcultarPanel(panelAnterior);
                this.MostrarPanel(this.PanelDeCrear);
            }
            this.InicializarSlectoresDeElementos(this.PanelDeCrear, this.Controlador);
            this.InicializarCanvases(this.PanelDeCrear);
        }

        private Crear() {
            let json: JSON = this.MapearControlesDeIU(this.PanelDeCrear);
            this.CrearElemento(json);
        }

        public CerrarCreacion() {
            this.ModoTrabajo = ModoTrabajo.mantenimiento;
            if (this.EsModal) {
                this.CerrarModal(this._idPanelCreacion);
            }
            else {
                this.Cerrar(this.PanelDeMnt, this.PanelDeCrear);
            }
            this.CrudDeMnt.Buscar(0);
        }

        private CrearElemento(json: JSON) {
            let url: string = `/${this.Controlador}/${Ajax.EndPoint.Crear}?${Ajax.Param.elementoJson}=${JSON.stringify(json)}`;
            let a = new ApiDeAjax.DescriptorAjax(Ajax.EndPoint.Crear
                , "{}"
                , url
                , ApiDeAjax.TipoPeticion.Sincrona
                , ApiDeAjax.ModoPeticion.Get
                , null
                , null
            );
            a.Ejecutar();

        }
    }

    export function EjecutarMenuCrt(accion: string): void {
        crudMnt.crudDeCreacion.EjecutarAcciones(accion);
    }

}
