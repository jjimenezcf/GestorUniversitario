namespace Crud {

    export class CrudCreacion extends CrudBase {

        protected PanelDeCrear: HTMLDivElement;
        protected PanelDeMnt: HTMLDivElement;
        protected CrudDeMnt: CrudMnt;

        private get Controlador(): string {
            return this.PanelDeCrear.getAttribute(Literal.controlador);
        }
        constructor(crud: CrudMnt, idPanelCreacion: string) {
            super();

            if (EsNula(idPanelCreacion))
                throw Error("No se puede construir un objeto del tipo CrudCreacion sin indica el panel de creación");

            this.PanelDeCrear = document.getElementById(idPanelCreacion) as HTMLDivElement;
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
            this.OcultarPanel(panelAnterior);
            this.MostrarPanel(this.PanelDeCrear);
            this.InicializarSlectoresDeElementos(this.PanelDeCrear, this.Controlador);
        }

        public Crear() {
            let json: JSON = this.MapearControlesDeIU(this.PanelDeCrear);
            this.CrearElemento(json);
        }

        public CerrarCreacion() {
            this.Cerrar(this.PanelDeMnt, this.PanelDeCrear);
            this.ModoTrabajo = ModoTrabajo.mantenimiento;
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
