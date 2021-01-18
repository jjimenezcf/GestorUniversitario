namespace Crud {

    export class CrudCreacion extends CrudBase {

        private _idPanelCreacion: string;

        protected CrudDeMnt: CrudMnt;

        protected get PanelDeCrear(): HTMLDivElement {
            return document.getElementById(this._idPanelCreacion) as HTMLDivElement;
        }

        public get EsModal(): boolean {
            return this.PanelDeCrear.className === ClaseCss.contenedorModal;
        }

        private get Controlador(): string {
            return this.PanelDeCrear.getAttribute(literal.controlador);
        }

        private get SeguirCreando(): boolean {
            let check: HTMLInputElement = document.getElementById(`${this._idPanelCreacion}-crear-mas`) as HTMLInputElement;
            return !check.checked;
        }

        constructor(crud: CrudMnt, idPanelCreacion: string) {
            super();

            if (IsNullOrEmpty(idPanelCreacion))
                throw Error("No se puede construir un objeto del tipo CrudCreacion sin indica el panel de creación");

            this._idPanelCreacion = idPanelCreacion;
            this.CrudDeMnt = crud;
        }


        public EjecutarAcciones(accion: string) {

            if (accion === Evento.Creacion.Crear)
                this.Crear();
            else
                if (accion === Evento.Creacion.Cerrar)
                    this.CerrarCreacion();
                else
                    throw `la opción ${accion} no está definida`;

        }

        public ComenzarCreacion() {
            this.CrudDeMnt.ModoTrabajo = ModoTrabajo.creando;

            if (this.EsModal) {
                var ventana = document.getElementById(this._idPanelCreacion);
                ventana.style.display = 'block';
            }
            else {
                this.OcultarPanel(this.CrudDeMnt.CuerpoCabecera);
                this.OcultarPanel(this.CrudDeMnt.CuerpoDatos);
                this.OcultarPanel(this.CrudDeMnt.CuerpoPie);
                this.PosicionarCreacion();
                this.MostrarPanel(this.PanelDeCrear);
            }
            this.InicializarPanel();
        }

        public PosicionarCreacion(): void {
            this.PanelDeCrear.style.position = 'fixed';
            this.PanelDeCrear.style.top = `${AlturaCabeceraPnlControl()}px`;
            this.PanelDeCrear.style.height = `${AlturaFormulario() - AlturaPiePnlControl() - AlturaCabeceraPnlControl()}px`;
        }

        private InicializarPanel() {
            this.InicializarListasDeElementos(this.PanelDeCrear, this.Controlador);
            this.InicializarListasDinamicas(this.PanelDeCrear);
            this.InicializarCanvases(this.PanelDeCrear);
        }

        private Crear() {
            let json: JSON = this.MapearControlesDeIU(this.PanelDeCrear, ModoTrabajo.creando);
            this.CrearElemento(json);
        }

        public CerrarCreacion() {
            this.CrudDeMnt.ModoTrabajo = ModoTrabajo.mantenimiento;
            if (this.EsModal) {
                this.CerrarModal(this.PanelDeCrear);
            }
            else {
                this.OcultarPanel(this.PanelDeCrear);
                this.MostrarPanel(this.CrudDeMnt.CuerpoPie);
                this.MostrarPanel(this.CrudDeMnt.CuerpoDatos);
                this.MostrarPanel(this.CrudDeMnt.CuerpoCabecera);
                BlanquearMensaje();
            }

            this.CrudDeMnt.ModoTrabajo = ModoTrabajo.mantenimiento;
            this.CrudDeMnt.Buscar(atGrid.accion.buscar,0);
        }

        private CrearElemento(json: JSON) {
            let url: string = `/${this.Controlador}/${Ajax.EndPoint.Crear}?${Ajax.Param.elementoJson}=${JSON.stringify(json)}`;
            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EndPoint.Crear
                , "{}"
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.DespuesDeCrear
                , this.SiHayErrorTrasPeticionAjax
            );
            a.Ejecutar();

        }

        private DespuesDeCrear(peticion: ApiDeAjax.DescriptorAjax) {
            let crudCreador: CrudCreacion = peticion.llamador as CrudCreacion;
            crudCreador.BlanquearControlesDeIU(crudCreador.PanelDeCrear);
            if (crudCreador.SeguirCreando) {
                crudCreador.InicializarPanel();
            }
            else {
                crudCreador.CerrarCreacion();
            }
        }

        public MaperaRestrictorDeCreacion(porpiedadRestrictora: string, valorRestrictor: number, valorMostrar: string) {
            let restrictores: NodeListOf<HTMLInputElement> = this.PanelDeCrear.querySelectorAll(`input[${atControl.tipo}="${TipoControl.restrictorDeEdicion}"]`) as NodeListOf<HTMLInputElement>;
            this.MapearRestrictor(restrictores, porpiedadRestrictora, valorMostrar, valorRestrictor);
        }

    }

}
