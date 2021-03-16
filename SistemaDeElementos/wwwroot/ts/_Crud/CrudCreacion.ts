namespace Crud {

    export class CrudCreacion extends CrudBase {

        private _idPanelCreacion: string;

        public CrudDeMnt: CrudMnt;
        Altura: number;

        public get PanelDeCrear(): HTMLDivElement {
            return document.getElementById(this._idPanelCreacion) as HTMLDivElement;
        }

        public get EsModal(): boolean {
            return this.PanelDeCrear.className === ClaseCss.contenedorModal;
        }

        public get PanelDeContenidoModal(): HTMLDivElement {
            return document.getElementById(`${this._idPanelCreacion}_contenido`) as HTMLDivElement;
        }
        //private get Controlador(): string {
        //    return this.PanelDeCrear.getAttribute(literal.controlador);
        //}

        private get SeguirCreando(): boolean {
            let check: HTMLInputElement = document.getElementById(`${this._idPanelCreacion}-crear-mas`) as HTMLInputElement;
            return !check.checked;
        }

        constructor(crud: CrudMnt, idPanelCreacion: string) {
            super();

            if (IsNullOrEmpty(idPanelCreacion))
                throw Error("No se puede construir un objeto del tipo CrudCreacion sin indica el panel de creación");

            this._idPanelCreacion = idPanelCreacion;
            this._controlador = this.PanelDeCrear.getAttribute(literal.controlador);
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

            ApiCrud.QuitarClaseDeCtrlNoValido(this.PanelDeCrear);

        }

        public ComenzarCreacion(): void {
            this.CrudDeMnt.ModoTrabajo = ModoTrabajo.creando;

            if (this.EsModal) {
                this.PanelDeCrear.style.display = 'block';
                this.Altura = this.PanelDeContenidoModal.getBoundingClientRect().height;
                EntornoSe.AjustarModalesAbiertas();
            }
            else {
                ApiCrud.OcultarPanel(this.CrudDeMnt.CuerpoCabecera);
                ApiCrud.OcultarPanel(this.CrudDeMnt.CuerpoDatos);
                ApiCrud.OcultarPanel(this.CrudDeMnt.CuerpoPie);
                this.PosicionarCreacion();
                ApiCrud.MostrarPanel(this.PanelDeCrear);
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
            this.InicializarArchivos(this.PanelDeCrear);
            ApiDeSeguridad.LeerModoDeAccesoAlNegocio(this, this.Controlador, this.CrudDeMnt.Negocio)
                .then((peticion) => this.AjustarMenuDeCreacion(peticion))
                .catch((peticion) => this.DesactivarMenuDeCreacion(peticion));
        }

        protected DesactivarMenuDeCreacion(peticion: any): void {
            ApiDeAjax.ErrorTrasPeticion("Al leer el modo de acceso al negocio", peticion);
            ApiControl.BloquearMenu(this.PanelDeCrear);
        }

        protected AjustarMenuDeCreacion(peticion: ApiDeAjax.DescriptorAjax): void {
            MensajesSe.Info(`Permisos sobre el negocio ${peticion.DatosDeEntrada["negocio"]} leidos`, peticion.resultado.consola);
        }

        private Crear() {
            let json: JSON = ApiCrud.MapearControlesDesdeLaIuAlJson(this, this.PanelDeCrear, ModoTrabajo.creando);
            this.CrearElemento(json);
        }

        public CerrarCreacion() {
            this.CrudDeMnt.ModoTrabajo = ModoTrabajo.mantenimiento;
            if (this.EsModal) {
                ApiCrud.CerrarModal(this.PanelDeCrear);
                EntornoSe.AjustarDivs();
            }
            else {
                ApiCrud.OcultarPanel(this.PanelDeCrear);
                ApiCrud.MostrarPanel(this.CrudDeMnt.CuerpoPie);
                ApiCrud.MostrarPanel(this.CrudDeMnt.CuerpoDatos);
                ApiCrud.MostrarPanel(this.CrudDeMnt.CuerpoCabecera);
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
            ApiCrud.BlanquearControlesDeIU(crudCreador.PanelDeCrear);
            if (crudCreador.SeguirCreando) {
                crudCreador.InicializarPanel();
            }
            else {
                crudCreador.CerrarCreacion();
            }
        }

    }

}
