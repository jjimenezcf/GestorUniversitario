namespace Crud {

    export let crudMnt: CrudMnt = null;

    export class CrudMnt extends GridDeDatos {

        public crudDeCreacion: CrudCreacion;
        public crudDeEdicion: CrudEdicion;
        public idModalBorrar: string;

        private _idPanelMnt
        public get PanelMnt(): HTMLDivElement {
            return  document.getElementById(this._idPanelMnt) as HTMLDivElement;
        }

        public Modales: Array<ModalSeleccion> = new Array<ModalSeleccion>();

        private get ModalDeBorrado(): HTMLDivElement {
            return document.getElementById(this.idModalBorrar) as HTMLDivElement;
        }

        constructor(idPanelMnt: string) {
            super(idPanelMnt);
            this._idPanelMnt = idPanelMnt;
        }

        public Inicializar() {
            super.Inicializar(this._idPanelMnt);
            this.InicializarSelectores();
            this.InicializarListasDeElementos(this.ZonaDeFiltro, this.Navegador.Controlador);

            this.AplicarRestrictores();

            this.Buscar(atGrid.accion.buscar, 0);

        }


        public AplicarRestrictores() {
            if (this.Estado.Contiene(Sesion.restrictor)) {
                let restrictor: DatosRestrictor = this.Estado.Obtener(Sesion.restrictor);
                this.MapearRestrictorDeFiltro(restrictor.Propiedad, restrictor.Valor, restrictor.Texto);
                this.crudDeCreacion.MaperaRestrictorDeCreacion(restrictor.Propiedad, restrictor.Valor, restrictor.Texto);
                this.crudDeEdicion.MaperaRestrictorDeEdicion(restrictor.Propiedad, restrictor.Valor, restrictor.Texto);
            }
        }

        private InicializarSelectores() {
            let selectores: NodeListOf<HTMLSelector> = this.ZonaDeFiltro.querySelectorAll(`input[tipo="${TipoControl.Selector}"]`) as NodeListOf<HTMLSelector>;;
            selectores.forEach((selector) => {
                let idModal: string = selector.getAttribute(atSelector.idModal);
                let modalHtml = document.getElementById(idModal);
                let idPanelMnt = modalHtml.getAttribute(atControl.crudModal);
                let modal: ModalSeleccion = new ModalSeleccion(idModal, idPanelMnt);
                this.Modales.push(modal);

            });
        }

        public ObtenerModal(idModal: string): ModalSeleccion {
            for (let i: number = 0; i < this.Modales.length; i++) {
                let modal: ModalSeleccion = this.Modales[i];
                if (modal.IdModal === idModal)
                    return modal;
            }
            return undefined;
        }

        public AbrirModalBorrarElemento() {
            if (this.InfoSelector.Cantidad == 0) {
                Mensaje(TipoMensaje.Info, "Debe marcar el elemento a borrar");
                return;
            }
            this.AbrirModalDeBorrar();
        }

        private AbrirModalDeBorrar() {
            this.ModoTrabajo = ModoTrabajo.borrando;
            this.ModalDeBorrado.style.display = 'block';
            let mensaje: HTMLInputElement = document.getElementById(`${this.idModalBorrar}-mensaje`) as HTMLInputElement;
            if (this.InfoSelector.Cantidad > 1) {
                mensaje.value = "Seguro desea borrar todos los elementos seleccionados";
            }
            else {
                mensaje.value = "Seguro desea borrar el elemento seleccionado";
            }
        }

        public BorrarElemento() {
            let ids: string = "";
            for (var i = 0; i < this.InfoSelector.Seleccionados.length; i++) {
                if (ids !== "")
                    ids = ids + ",";
                ids = ids + this.InfoSelector.Seleccionados[i];
            }
            this.InfoSelector.Seleccionados[0] as number;
            let url: string = this.DefinirPeticionDeBorrado(ids);

            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EndPoint.Borrar
                , null
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.DespuesDeBorrar
                , this.SiHayErrorTrasPeticionAjax
            );

            a.Ejecutar();

        }

        private DespuesDeBorrar(peticion: ApiDeAjax.DescriptorAjax) {
            let mantenimiento: CrudMnt = peticion.llamador as CrudMnt;
            mantenimiento.CerrarModalDeBorrado();
            mantenimiento.InfoSelector.QuitarTodos();
            mantenimiento.CargarGrid(atGrid.accion.buscar, 0);
        }


        public CerrarModalDeBorrado() {
            this.ModoTrabajo = ModoTrabajo.mantenimiento;
            this.CerrarModal(this.idModalBorrar);
        }

        public IraEditar() {
            if (this.InfoSelector.Cantidad == 0) {
                Mensaje(TipoMensaje.Info, "Debe marcar el elemento a editar");
                return;
            }

            this.crudDeEdicion.ComenzarEdicion(crudMnt.PanelMnt, this.InfoSelector);
        }

        public CerrarModalDeEdicion() {
            this.crudDeEdicion.EjecutarAcciones(Evento.Edicion.Cerrar);
        }

        public ModificarElemento() {
            this.crudDeEdicion.EjecutarAcciones(Evento.ModalEdicion.Modificar);
        }

        public IraCrear() {
            this.crudDeCreacion.ComenzarCreacion(crudMnt.PanelMnt);
        }

        public CrearElemento() {
            this.crudDeCreacion.EjecutarAcciones(Evento.Creacion.Crear);
        }

        public CerrarModalDeCreacion() {
            this.crudDeCreacion.EjecutarAcciones(Evento.Creacion.Cerrar);
        }

        public RestaurarPagina() {
            this.Navegador.EsRestauracion = false;
            let cantidad: number = this.Navegador.Cantidad;
            let pagina: number = this.Navegador.Pagina;
            if (pagina <= 1)
                this.CargarGrid(atGrid.accion.buscar, 0);
            else {
                let posicion: number = (pagina - 1) * cantidad;
                if (posicion < 0)
                    posicion = 0;
                this.Buscar(atGrid.accion.restaurar, posicion);
            }
        }

        private DefinirPeticionDeBorrado(ids: string): string {
            let idsJson: JSON = JSON.parse(`[${ids}]`);
            var controlador = this.Navegador.Controlador;
            let url: string = `/${controlador}/${Ajax.EndPoint.Borrar}`;
            let parametros: string = `${Ajax.Param.idsJson}=${JSON.stringify(idsJson)}`;
            let peticion: string = url + '?' + parametros;
            return peticion;
        }
                
        public Buscar(accion: string, posicion: number) {

            if (this.Navegador.EsRestauracion) {
                this.RestaurarPagina();
            }
            else {
              this.CargarGrid(accion, posicion);
            }
        }

        public CambiarSelector(idSelector: string) {
            var htmlSelector: HTMLSelector = <HTMLSelector>document.getElementById(idSelector);

            let modal: ModalSeleccion = crudMnt.ObtenerModal(htmlSelector.getAttribute(atSelector.idModal));
            if (IsNullOrEmpty(htmlSelector.value))
                modal.InicializarModal();
            else
                modal.TextoSelectorCambiado();
        }

        public CargarListaDinamica(selector: HTMLInputElement) {
            super.CargarListaDinamica(selector, this.Navegador.Controlador);
        }

        public SeleccionarListaDinamica(selector: HTMLInputElement) {
            super.CargarListaDinamica(selector, this.Navegador.Controlador);
        }

        public MapearRestrictorDeFiltro(porpiedadRestrictora: string, valorRestrictor: number, valorMostrar: string) {
            let restrictoresDeFiltro: NodeListOf<HTMLInputElement> = this.PanelMnt.querySelectorAll(`input[${atControl.tipo}="${TipoControl.restrictorDeFiltro}"]`) as NodeListOf<HTMLInputElement>;
            this.MapearRestrictor(restrictoresDeFiltro, porpiedadRestrictora, valorMostrar, valorRestrictor);
        }

    }
}