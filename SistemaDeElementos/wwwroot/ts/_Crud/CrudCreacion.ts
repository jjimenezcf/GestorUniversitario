namespace Crud {

    export class CrudCreacion extends CrudBase {

        protected PanelDeCrear: HTMLDivElement;
        protected PanelDeMnt: HTMLDivElement;
        protected CrudDeMnt: CrudMnt;

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
                if (accion === LiteralCrt.nuevoelemento)
                    this.Crear();
                else
                    if (accion === LiteralCrt.cancelarnuevo)
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
            this.OcultarPanel(panelAnterior);
            this.MostrarPanel(this.PanelDeCrear);
            this.InicializarValores();
        }

        protected InicializarValores(): void {

        }

        public Crear() {
            let json: JSON = this.MapearControlesDeIU(this.PanelDeCrear);
            this.CrearElemento(json);
        }

        public CerrarCreacion() {
            this.Cerrar(this.PanelDeMnt, this.PanelDeCrear);
            this.CrudDeMnt.Buscar(0);
        }

        private CrearElemento(json: JSON) {
            let controlador = this.PanelDeCrear.getAttribute(Literal.controlador);
            let url: string = `/${controlador}/${Ajax.EndPoint.Crear}?${Ajax.Param.elementoJson}=${JSON.stringify(json)}`;
            let req: XMLHttpRequest = new XMLHttpRequest();
            this.PeticionSincrona(req, url, Ajax.EndPoint.Crear);
        }
    }

    export function EjecutarMenuCrt(accion: string): void {
        crudMnt.crudDeCreacion.EjecutarAcciones(accion);
    }

}
