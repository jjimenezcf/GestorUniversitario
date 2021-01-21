﻿namespace Crud {

    export let crudMnt: CrudMnt = null;

    export class CrudMnt extends GridDeDatos {

        public crudDeCreacion: CrudCreacion;
        public crudDeEdicion: CrudEdicion;
        private _idModalBorrar: string;


        public get Cuerpo(): HTMLDivElement {
            return document.getElementById("div-cuerpo") as HTMLDivElement;
        };

        private modoTrabajo: string;
        public get ModoTrabajo(): string {
            return this.modoTrabajo;
        }

        public set ModoTrabajo(modo: string) {
            this.modoTrabajo = modo;
        }
        protected get ModalDeBorrado(): HTMLDivElement {
            return document.getElementById(this._idModalBorrar) as HTMLDivElement;
        };

        public ModalesDeSeleccion: Array<ModalSeleccion> = new Array<ModalSeleccion>();
        public ModalesParaRelacionar: Array<ModalParaRelacionar> = new Array<ModalParaRelacionar>();
        public ModalesParaConsultarRelaciones: Array<ModalParaConsultarRelaciones> = new Array<ModalParaConsultarRelaciones>();

        constructor(idPanelMnt: string, idModalDeBorrado: string) {
            super(idPanelMnt);
            this._idModalBorrar = idModalDeBorrado;
        }

        public Inicializar(idPanelMnt: string) {
            try {

                if (IsNullOrEmpty(idPanelMnt))
                    idPanelMnt = this.IdCuerpoCabecera;

                super.Inicializar(this.IdCuerpoCabecera);
                this.ModoTrabajo = ModoTrabajo.mantenimiento;
                this.InicializarSelectores();
                this.InicializarListasDeElementos(this.ZonaDeFiltro, this.Navegador.Controlador);
                this.InicializarMenus();

                this.AplicarRestrictores();

                this.Buscar(atGrid.accion.buscar, 0);
            }
            catch (error) {
                Mensaje(TipoMensaje.Error, `Error al inicializar el crud ${this.IdCuerpoCabecera}`, error);
            }
        }

        public PosicionarPanelesDelCuerpo(): void {
            if (this.ModoTrabajo === ModoTrabajo.mantenimiento) {
                this.PosicionarFiltro();
                this.PosicionarGrid();
            }
            if ((this.ModoTrabajo === ModoTrabajo.editando || this.ModoTrabajo === ModoTrabajo.consultando) && !this.crudDeEdicion.EsModal) 
                this.crudDeEdicion.PosicionarEdicion();
            if ((this.ModoTrabajo === ModoTrabajo.creando || this.ModoTrabajo === ModoTrabajo.copiando) && !this.crudDeCreacion.EsModal)
                this.crudDeCreacion.PosicionarCreacion();
        }

        public PosicionarFiltro(): void {
            this.ZonaDeFiltro.style.position = 'fixed';
            let posicionFiltro: number = this.PosicionFiltro();
            this.ZonaDeFiltro.style.top = `${posicionFiltro}px`;

            let bloques = this.ZonaDeFiltro.getElementsByClassName('cuerpo-datos-filtro-bloque') as HTMLCollectionOf<HTMLDivElement>;
            let alturaDeBloques = 0;
            for (let i = 0; i < bloques.length; i++) {
                alturaDeBloques = alturaDeBloques + bloques[i].getBoundingClientRect().height;
            }
            let alturaCalculada: number = AlturaFormulario() * 20 / 100;
            this.ZonaDeFiltro.style.height = alturaDeBloques < alturaCalculada
                ? `${alturaDeBloques}px`
                : `${alturaCalculada}px`;
        }

        private PosicionFiltro(): number {
            let alturaCabeceraPnlControl: number = AlturaCabeceraPnlControl();
            let alturaCabeceraMnt: number = this.CuerpoCabecera.getBoundingClientRect().height;
            return alturaCabeceraPnlControl + alturaCabeceraMnt;
        }

        public AplicarRestrictores() {
            if (this.Estado.Contiene(Sesion.restrictor)) {
                let restrictor: Tipos.DatosRestrictor = this.Estado.Obtener(Sesion.restrictor);
                this.MapearRestrictorDeFiltro(restrictor.Propiedad, restrictor.Valor, restrictor.Texto);
                this.crudDeCreacion.MaperaRestrictorDeCreacion(restrictor.Propiedad, restrictor.Valor, restrictor.Texto);
                this.crudDeEdicion.MaperaRestrictorDeEdicion(restrictor.Propiedad, restrictor.Valor, restrictor.Texto);
            }
        }

        private InicializarSelectores() {
            let selectores: NodeListOf<HTMLSelector> = this.ZonaDeFiltro.querySelectorAll(`input[tipo="${TipoControl.Selector}"]`) as NodeListOf<HTMLSelector>;
            selectores.forEach((selector) => {
                let idModal: string = selector.getAttribute(atSelector.idModal);
                let modal: ModalSeleccion = new ModalSeleccion(idModal);
                this.ModalesDeSeleccion.push(modal);

            });
        }

        private InicializarMenus() {

            this.DeshabilitarOpcionesDeMenuDeElemento();

            let url: string = this.DefinirPeticionDeLeerModoDeAccesoAlNegocio();
            let datosDeEntrada = `{"negocio":"${this.Negocio}"}`;
            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EndPoint.LeerModoDeAccesoAlNegocio
                , datosDeEntrada
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.AplicarModoDeAccesoAlNegocio
                , this.SiHayErrorTrasPeticionAjax
            );

            a.Ejecutar();
        }

        private AplicarModoDeAccesoAlNegocio(peticion: ApiDeAjax.DescriptorAjax) {
            let mantenimiento: CrudMnt = peticion.llamador as CrudMnt;
            let modoDeAccesoDelUsuario = peticion.resultado.modoDeAcceso;
            let opcionesGenerales: NodeListOf<HTMLButtonElement> = mantenimiento.ZonaDeMenu.querySelectorAll(`input[${atOpcionDeMenu.clase}="${ClaseDeOpcioDeMenu.DeVista}"]`) as NodeListOf<HTMLButtonElement>;
            for (var i = 0; i < opcionesGenerales.length; i++) {
                let opcion: HTMLButtonElement = opcionesGenerales[i];
                let permisosNecesarios: string = opcion.getAttribute(atOpcionDeMenu.permisosNecesarios);
                if (permisosNecesarios === ModoDeAccesoDeDatos.Administrador && modoDeAccesoDelUsuario !== ModoDeAccesoDeDatos.Administrador)
                    opcion.disabled = true;
                else
                    if (permisosNecesarios === ModoDeAccesoDeDatos.Gestor && (modoDeAccesoDelUsuario === ModoDeAccesoDeDatos.Consultor || modoDeAccesoDelUsuario === ModoDeAccesoDeDatos.SinPermiso))
                        opcion.disabled = true;
                    else
                        if (permisosNecesarios === ModoDeAccesoDeDatos.Consultor && modoDeAccesoDelUsuario === ModoDeAccesoDeDatos.SinPermiso)
                            opcion.disabled = true;
                        else
                            opcion.disabled = false;
            }
        }


        private DefinirPeticionDeLeerModoDeAccesoAlNegocio(): string {
            let url: string = `/${this.Controlador}/${Ajax.EndPoint.LeerModoDeAccesoAlNegocio}`;
            let parametros: string = `${Ajax.Param.negocio}=${this.Negocio}`;
            let peticion: string = url + '?' + parametros;
            return peticion;
        }

        public ObtenerModalDeSeleccion(idModal: string): ModalSeleccion {
            for (let i: number = 0; i < this.ModalesDeSeleccion.length; i++) {
                let modal: ModalSeleccion = this.ModalesDeSeleccion[i];
                if (modal.IdModal === idModal)
                    return modal;
            }
            return undefined;
        }

        public ObtenerModalParaRelacionar(idModal: string): ModalParaRelacionar {
            for (let i: number = 0; i < this.ModalesParaRelacionar.length; i++) {
                let modal: ModalParaRelacionar = this.ModalesParaRelacionar[i];
                if (modal.IdModal === idModal)
                    return modal;
            }

            let modal: ModalParaRelacionar = new ModalParaRelacionar(this, idModal);
            this.ModalesParaRelacionar.push(modal);
            return modal;
        }


        public ObtenerModalParaConsultarRelaciones(idModal: string): ModalParaConsultarRelaciones {
            for (let i: number = 0; i < this.ModalesParaConsultarRelaciones.length; i++) {
                let modal: ModalParaConsultarRelaciones = this.ModalesParaConsultarRelaciones[i];
                if (modal.IdModal === idModal)
                    return modal;
            }

            let modal: ModalParaConsultarRelaciones = new ModalParaConsultarRelaciones(this, idModal);
            this.ModalesParaConsultarRelaciones.push(modal);
            return modal;
        }

        public AbrirModalParaRelacionar(idModalParaRelacionar: string) {
            let modal: ModalParaRelacionar = this.ObtenerModalParaRelacionar(idModalParaRelacionar);
            if (modal === undefined)
                throw new Error(`Modal ${idModalParaRelacionar} no definida`);

            modal.AbrirModalDeRelacion();
        }

        public AbrirModalParaConsultarRelaciones(idModalParaConsultar: string) {

            if (this.InfoSelector.Cantidad != 1)
                throw new Error("Debe seleccionar un elemento para poder consultar las relaciones");


            let modal: ModalParaConsultarRelaciones = this.ObtenerModalParaConsultarRelaciones(idModalParaConsultar);
            if (modal === undefined)
                throw new Error(`Modal ${idModalParaConsultar} no definida`);

            modal.AbrirModalParaConsultarRelaciones(this.InfoSelector.LeerElemento(0));
        }

        public AbrirModalBorrarElemento() {
            if (this.InfoSelector.Cantidad == 0)
                throw new Error(`Debe seleccionar el elemento a borrar, ha seleccionado ${this.InfoSelector.Cantidad}`);

            this.AbrirModalDeBorrar();
        }

        private AbrirModalDeBorrar() {
            this.ModoTrabajo = ModoTrabajo.borrando;
            this.ModalDeBorrado.style.display = 'block';
            let mensaje: HTMLInputElement = document.getElementById(`${this._idModalBorrar}-mensaje`) as HTMLInputElement;
            if (this.InfoSelector.Cantidad > 1) {
                mensaje.value = "Seguro desea borrar todos los elementos seleccionados";
            }
            else {
                mensaje.value = "Seguro desea borrar el elemento seleccionado";
            }
        }

        public BorrarElemento() {
            let url: string = this.DefinirPeticionDeBorrado();
            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EndPoint.Borrar
                , null
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.DespuesDeBorrar
                , this.SiHayErrorTrasPeticionDeBorrar
            );
            a.Ejecutar();
        }

        private DespuesDeBorrar(peticion: ApiDeAjax.DescriptorAjax) {
            let mantenimiento: CrudMnt = peticion.llamador as CrudMnt;
            mantenimiento.CerrarModalDeBorrado();
            mantenimiento.InfoSelector.QuitarTodos();
            mantenimiento.Buscar(atGrid.accion.buscar, 0);
        }

        private SiHayErrorTrasPeticionDeBorrar(peticion: ApiDeAjax.DescriptorAjax) {
            let mantenimiento: CrudMnt = peticion.llamador as CrudMnt;
            mantenimiento.CerrarModalDeBorrado();
            mantenimiento.BlanquearTodosLosCheck();
            mantenimiento.SiHayErrorTrasPeticionAjax(peticion);
        }

        public CerrarModalDeBorrado() {
            this.ModoTrabajo = ModoTrabajo.mantenimiento;
            ApiCrud.CerrarModal(this.ModalDeBorrado);
        }

        public IraEditar() {
            if (this.InfoSelector.Cantidad == 0) {
                Mensaje(TipoMensaje.Info, "Debe marcar el elemento a editar");
                return;
            }

            this.crudDeEdicion.ComenzarEdicion(this.InfoSelector);
        }

        public CerrarModalDeEdicion() {
            this.crudDeEdicion.EjecutarAcciones(Evento.Edicion.Cerrar);
        }

        public ModificarElemento() {
            this.crudDeEdicion.EjecutarAcciones(Evento.ModalEdicion.Modificar);
        }

        public IraCrear() {
            this.crudDeCreacion.ComenzarCreacion();
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

        private DefinirPeticionDeBorrado(): string {
            let idsJson: string = JSON.stringify(this.InfoSelector.Seleccionados);
            var controlador = this.Navegador.Controlador;
            let url: string = `/${controlador}/${Ajax.EndPoint.Borrar}`;
            let parametros: string = `${Ajax.Param.idsJson}=${idsJson}`;
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

            let modal: ModalSeleccion = crudMnt.ObtenerModalDeSeleccion(htmlSelector.getAttribute(atSelector.idModal));
            if (IsNullOrEmpty(htmlSelector.value))
                modal.InicializarModalDeSeleccion();
            else
                modal.TextoSelectorCambiado();
        }

        public CargarListaDinamica(selector: HTMLInputElement) {
            super.CargarListaDinamica(selector, this.Navegador.Controlador);
        }

        public SeleccionarListaDinamica(input: HTMLInputElement) {
            let lista: Tipos.ListaDinamica = new Tipos.ListaDinamica(input);
            let valor: number = lista.BuscarSeleccionado(input.value);
            input.setAttribute(atListasDinamicas.idSeleccionado, valor.toString());
            if (valor === 0)
                input.value = "";
        }

        public MapearRestrictorDeFiltro(porpiedadRestrictora: string, valorRestrictor: number, valorMostrar: string) {
            let restrictoresDeFiltro: NodeListOf<HTMLInputElement> = this.ZonaDeFiltro.querySelectorAll(`input[${atControl.tipo}="${TipoControl.restrictorDeFiltro}"]`) as NodeListOf<HTMLInputElement>;
            this.MapearRestrictor(restrictoresDeFiltro, porpiedadRestrictora, valorMostrar, valorRestrictor);
        }

        public OcultarMostrarFiltro(): void {
            if (NumeroMayorDeCero(this.ExpandirFiltro.value)) {
                ApiCrud.OcultarPanel(this.ZonaDeFiltro);
                this.ExpandirFiltro.value = "0";
                this.EtiquetaMostrarOcultarFiltro.innerText = "Mostrar filtro";
            }
            else {
                this.ExpandirFiltro.value = "1";
                ApiCrud.MostrarPanel(this.ZonaDeFiltro);
                this.EtiquetaMostrarOcultarFiltro.innerText = "Ocultar filtro";
            }
            this.PosicionarPanelesDelCuerpo();
        }

        public OcultarMostrarBloque(idHtmlBloque: string) {
            let extensor: HTMLInputElement = document.getElementById(`expandir.${idHtmlBloque}.input`) as HTMLInputElement;
            if (NumeroMayorDeCero(extensor.value)) {
                extensor.value = "0";
                ApiCrud.OcultarPanel(document.getElementById(`${idHtmlBloque}`) as HTMLDivElement);
            }
            else {
                extensor.value = "1";
                ApiCrud.MostrarPanel(document.getElementById(`${idHtmlBloque}`) as HTMLDivElement);
            }
            this.PosicionarPanelesDelCuerpo();
        }

    }

}