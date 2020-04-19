namespace Crud {

    export class CrudCreacion extends CrudBase {

        protected PanelDeCrear: HTMLDivElement;
        protected PanelDeMnt: HTMLDivElement;

        constructor(panelMnt: HTMLDivElement, idPanelCreacion: string) {
            super();

            if (EsNula(idPanelCreacion))
                throw Error("No se puede construir un objeto del tipo CrudCreacion sin indica el panel de creación");

            this.PanelDeCrear = document.getElementById(idPanelCreacion) as HTMLDivElement;
            this.PanelDeMnt = panelMnt;
        }

        public ComenzarCreacion(panelAnterior: HTMLDivElement) {
            this.OcultarPanel(panelAnterior);
            this.MostrarPanel(this.PanelDeCrear);
            this.InicializarValores();
        }

        protected InicializarValores(): void {

        }

        public Aceptar(panelMostrar: HTMLDivElement) {
            let json: JSON = null;
            try {
                json = this.MapearControlesDeIU(this.PanelDeCrear);
                this.CrearElemento(json);
            }
            catch (error){
                Mensaje(TipoMensaje.Error,error);
                return;
            }

            this.CerrarCreacion(panelMostrar);

        }

        public CerrarCreacion(panelMostrar: HTMLDivElement) {
                this.Cerrar(panelMostrar, this.PanelDeCrear);
        }

        private CrearElemento(json: JSON) {
            let controlador = this.PanelDeCrear.getAttribute(Literal.controlador);
            let url: string = `/${controlador}/${Ajax.EndPoint.Crear}?${Ajax.Param.elementoJson}=${JSON.stringify(json)}`;
            let req: XMLHttpRequest = new XMLHttpRequest();
            this.PeticionSincrona(req, url, Ajax.EndPoint.Crear);
        }
    }

    export function EjecutarMenuCrt(accion: string): void {

        if (accion === LiteralCrt.nuevoelemento)
            NuevoElemento();
        else
        if (accion === LiteralCrt.cancelarnuevo)
            CancelarNuevo();
        else
            Mensaje(TipoMensaje.Info, `la opción ${accion} no está definida`);
    }

    function NuevoElemento() {
        crudMnt.crudDeCreacion.Aceptar(crudMnt.PanelDeMnt);
    }

    function CancelarNuevo() {
        try {
            crudMnt.crudDeCreacion.CerrarCreacion(crudMnt.PanelDeMnt);
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error.menssage);
        }
    }

}
