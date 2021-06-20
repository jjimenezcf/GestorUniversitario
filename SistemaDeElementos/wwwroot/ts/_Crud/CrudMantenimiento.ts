namespace Crud {

    export let crudMnt: CrudMnt = null;

    export class CrudMnt extends GridDeDatos {

        public crudDeCreacion: CrudCreacion;
        public crudDeEdicion: CrudEdicion;
        private _idModalBorrar: string;

        private _modoAccesoDelUsuario: ModoAcceso.enumModoDeAccesoDeDatos;
        public get ModoAccesoDelUsuario(): ModoAcceso.enumModoDeAccesoDeDatos {
            return this._modoAccesoDelUsuario;
        }
        public set ModoAccesoDelUsuario(modoDeAcceso: ModoAcceso.enumModoDeAccesoDeDatos) {
            this._modoAccesoDelUsuario = modoDeAcceso;
        }

        public get Cuerpo(): HTMLDivElement {
            return document.getElementById(LiteralMnt.idCuerpoDePagina) as HTMLDivElement;
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

        protected get ModalDeExportacion(): HTMLDivElement {
            let idModal: string = this.IdCuerpoCabecera;
            idModal = idModal.replace('mantenimiento', '');
            idModal = idModal + 'panel-exportacion';
            return document.getElementById(idModal) as HTMLDivElement;
        };

        protected get ModalDeEnviarCorreo(): HTMLDivElement {
            let idModal: string = this.IdCuerpoCabecera;
            idModal = idModal.replace('mantenimiento', '');
            idModal = idModal + 'panel-enviar-correo';
            return document.getElementById(idModal) as HTMLDivElement;
        };

        protected get ModalDeEnviarCorreo_DivDeElementos(): HTMLDivElement {
            return document.getElementById(`${this.ModalDeEnviarCorreo.id}_elementos_ref`) as HTMLDivElement;
        };

        public ModalesDeSeleccion: Array<ModalSeleccion> = new Array<ModalSeleccion>();
        public ModalesParaRelacionar: Array<ModalParaRelacionar> = new Array<ModalParaRelacionar>();
        public ModalesParaConsultarRelaciones: Array<ModalParaConsultarRelaciones> = new Array<ModalParaConsultarRelaciones>();
        public ModalesParaSeleccionar: Array<ModalParaSeleccionar> = new Array<ModalParaSeleccionar>();

        public get OpcionesGenerales(): NodeListOf<HTMLButtonElement> {
            return this.ZonaDeMenu.querySelectorAll(`input[${atOpcionDeMenu.clase}="${ClaseDeOpcioDeMenu.DeVista}"]`) as NodeListOf<HTMLButtonElement>;
        }

        constructor(idPanelMnt: string, idModalDeBorrado: string) {
            super(idPanelMnt);
            this._idModalBorrar = idModalDeBorrado;
        }

        public NavegarDesdeElBrowser() {
            //MensajesSe.Info('Ha llamado al método navegar');
        }

        public Inicializar(idPanelMnt: string) {
            const querystring = window.location.search;
            const params = new URLSearchParams(querystring);
            try {

                if (IsNullOrEmpty(idPanelMnt))
                    idPanelMnt = this.IdCuerpoCabecera;

                super.Inicializar(this.IdCuerpoCabecera);
                this.ModoTrabajo = ModoTrabajo.mantenimiento;
                this.InicializarSelectores();
                this.InicializarListasDeElementos(this.ZonaDeFiltro, this.Navegador.Controlador);
                this.InicializarMenus();
                this.InicializarSelectoresDeFecha(this.ZonaDeFiltro);

                if (params.has("origen")) this.AplicarRestrictores(params.get("origen"));

                if (this.Navegador.EsRestauracion) {
                    this.RestaurarPagina()
                        .then((valor) => this.TrasRestaurar(valor));
                }
                else {
                    if (params.has("id"))
                        this.EditarRegistro(Numero(params.get("id")));

                    this.InicializarOrdenacion();
                    this.Buscar(atGrid.accion.buscar, 0);
                }
            }
            catch (error) {
                MensajesSe.Error("Inicializando el crud", `Error al inicializar el crud ${this.IdCuerpoCabecera}`, error.message);
            }
        }

        protected EditarRegistro(id: number) {
            this.FiltrarPorId(id)
                .then(() => this.IraEditar());
        }

        protected InicializarOrdenacion() {
            let ordenacionInicial = this.CuerpoCabecera.getAttribute(atControl.ordenInicial);
            let lista = ToLista(ordenacionInicial, ";");
            let columnas: NodeListOf<HTMLTableHeaderCellElement> = this.CabeceraTablaGrid.querySelectorAll("th") as NodeListOf<HTMLTableHeaderCellElement>;
            for (let i: number = 0; i < columnas.length; i++) {
                let columna = columnas[i];
                let propiedad: string = columna.getAttribute(atControl.propiedad);
                for (let j: number = 0; j < lista.length; j++) {
                    if (IsNullOrEmpty(lista[j]))
                        continue;

                    let partes = lista[j].split(":");

                    if (partes.length !== 3) {
                        MensajesSe.Error("InicializarOrdenacion", `La tripleta de ordenación ${lista[j]} está mal definida, ha de tener ternas separadas por ; con el patron siguiente: (Propiedad:OrdenarPor:Modo)`);
                        return;
                    }

                    if (partes[0] === propiedad) {
                        if (this.Ordenacion.Actualizar(columna.id, propiedad, partes[2].trim(), partes[1].trim()))
                            ApiControl.MapearComoOrdenar(columna, this.Ordenacion.LeerPorPropiedad(propiedad));
                    }

                }
            }
        }


        private TrasRestaurar(valor: boolean): void {
            if (valor && this.Estado.Obtener("EditarAlVolver")) {
                this.Estado.Quitar("EditarAlVolver"); EntornoSe.Historial.GuardarEstadoDePagina(this.Estado);
                EntornoSe.Historial.Persistir();
                this.IraEditar();
            }
        }


        public PosicionarPanelesDelCuerpo(): void {
            if (this.ModoTrabajo === ModoTrabajo.mantenimiento) {
                this.PosicionarFiltro();
                this.PosicionarGrid();
            }
            if (this.ModoTrabajo === ModoTrabajo.editando || this.ModoTrabajo === ModoTrabajo.consultando) {
                if (!this.crudDeEdicion.EsModal)
                    this.crudDeEdicion.PosicionarEdicion();
                else {
                    this.PosicionarFiltro();
                    this.PosicionarGrid();
                }
            }
            if (this.ModoTrabajo === ModoTrabajo.creando || this.ModoTrabajo === ModoTrabajo.copiando) {
                if (!this.crudDeCreacion.EsModal)
                    this.crudDeCreacion.PosicionarCreacion();
                else {
                    this.PosicionarFiltro();
                    this.PosicionarGrid();
                }
            }
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

        private AplicarRestrictores(origen: string): void {
            if (this.Estado.Contiene(Sesion.restrictores)) {
                let restrictores: Tipos.DatosRestrictor[] = this.Estado.Obtener(Sesion.restrictores) as Tipos.DatosRestrictor[];
                for (let i = 0; i < restrictores.length; i++) {
                    this.AplicarRestrictor(restrictores[i]);
                }
            }
        }

        private AplicarRestrictor(restrictor: Tipos.DatosRestrictor): void {
            MapearPanelDeFiltro.MapearRestrictores(this.ZonaDeFiltro, restrictor.Propiedad, restrictor.Valor, restrictor.Texto);

            if (EsTrue(this.CuerpoCabecera.getAttribute(atMantenimniento.permiteCrear)))
                MapearPanelDeCreacion.MapearRestrictores(this.crudDeCreacion.PanelDeCrear, restrictor.Propiedad, restrictor.Valor, restrictor.Texto);

            if (EsTrue(this.CuerpoCabecera.getAttribute(atMantenimniento.permiteEditar)))
                MapearPanelDeEdicion.MapearRestrictores(this.crudDeEdicion.PanelDeEditar, restrictor.Propiedad, restrictor.Valor, restrictor.Texto);

            this.DespuesDeAplicarUnRestrictor(restrictor);
        }

        protected DespuesDeAplicarUnRestrictor(restrictor: Tipos.DatosRestrictor): void {
            //método para sobrecargar que analiza el tipo de restrictor aplicado y obtirnr información de si se ha de restringir o mapear más información
            //P.eje: Al mapear una provincia al filtro de municipios, mapear el país
        }

        private InicializarSelectores() {
            let selectores: NodeListOf<HTMLSelector> = this.ZonaDeFiltro.querySelectorAll(`input[tipo="${TipoControl.Selector}"]`) as NodeListOf<HTMLSelector>;
            selectores.forEach((selector) => {
                let idModal: string = selector.getAttribute(atSelectorDeFiltro.idModal);
                let modal: ModalSeleccion = new ModalSeleccion(idModal);
                modal.InicializarModalDeSeleccion();
                this.ModalesDeSeleccion.push(modal);
            });
        }

        private InicializarMenus() {
            this.DeshabilitarOpcionesDeMenuDeElemento();
            if (this.ModoAccesoDelUsuario === undefined) {
                ApiDePeticiones.LeerModoDeAccesoAlNegocio(this, this.Controlador, this.Negocio)
                    .then((peticion) => this.AplicarModoDeAccesoAlNegocio(peticion))
                    .catch((peticion) => this.ErrorAlLeerModoAccesoAlNegocio(peticion));
            }
            else {
                ModoAcceso.AplicarModoDeAccesoAlNegocio(this.OpcionesGenerales, this.ModoAccesoDelUsuario);
            }
        }

        protected ErrorAlLeerModoAccesoAlNegocio(peticion: ApiDeAjax.DescriptorAjax): void {
            ApiDeAjax.ErrorTrasPeticion("Leer modo de acceso al negocio", peticion);
            ApiControl.BloquearMenu(this.Cuerpo);
        }

        private AplicarModoDeAccesoAlNegocio(peticion: ApiDeAjax.DescriptorAjax) {
            let mantenimiento: CrudMnt = peticion.llamador as CrudMnt;
            let modoDeAccesoDelUsuario = ModoAcceso.Parsear(peticion.resultado.modoDeAcceso);
            mantenimiento.ModoAccesoDelUsuario = modoDeAccesoDelUsuario;
            ModoAcceso.AplicarModoDeAccesoAlNegocio(mantenimiento.OpcionesGenerales, modoDeAccesoDelUsuario);
        }

        public ObtenerGrid(idModal: string): GridDeDatos {

            if (IsNullOrEmpty(idModal))
                return this;

            let grid: GridDeDatos = this.ModalDeSeleccionAsociada(idModal);
            if (Definida(grid))
                return grid;

            grid = this.ModalParaSeleccionAsociada(idModal);
            if (Definida(grid))
                return grid;

            grid = this.ModalParaConsultarRelacionesAsociada(idModal);
            if (Definida(grid))
                return grid;

            grid = this.ModalParaRelacionesAsociada(idModal);
            if (Definida(grid))
                return grid;

            MensajesSe.EmitirExcepcion("ObtenerGrid", `Se busca la modal con id ${idModal} y no se ha encontrado`)
        }

        private ModalParaRelacionesAsociada(idModal: string): GridDeDatos {
            for (let i: number = 0; i < this.ModalesParaRelacionar.length; i++) {
                let modal: ModalParaRelacionar = this.ModalesParaRelacionar[i];
                if (modal.IdModal === idModal)
                    return modal as GridDeDatos;
            }
            return undefined;
        }

        private ModalParaConsultarRelacionesAsociada(idModal: string): GridDeDatos {
            for (let i: number = 0; i < this.ModalesParaConsultarRelaciones.length; i++) {
                let modal: ModalParaConsultarRelaciones = this.ModalesParaConsultarRelaciones[i];
                if (modal.IdModal === idModal)
                    return modal as GridDeDatos;
            }
            return undefined;
        }

        private ModalParaSeleccionAsociada(idModal: string): GridDeDatos {
            for (let i: number = 0; i < this.ModalesParaSeleccionar.length; i++) {
                let modal: ModalParaSeleccionar = this.ModalesParaSeleccionar[i];
                if (modal.IdModal === idModal)
                    return modal as GridDeDatos;
            }
            return undefined;
        }

        private ModalDeSeleccionAsociada(idModal: string): GridDeDatos {
            for (let i: number = 0; i < this.ModalesDeSeleccion.length; i++) {
                let modal: ModalSeleccion = this.ModalesDeSeleccion[i];
                if (modal.IdModal === idModal)
                    return modal as GridDeDatos;
            }
            return undefined;
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

        public ModalDeBorrado_Abrir() {
            if (this.InfoSelector.Cantidad == 0)
                throw new Error(`Debe seleccionar el elemento a borrar, ha seleccionado ${this.InfoSelector.Cantidad}`);

            this.modalDeBorrardo_Abrir();
        }

        private modalDeBorrardo_Abrir(): void {
            this.ModoTrabajo = ModoTrabajo.borrando;
            this.ModalDeBorrado.style.display = 'block';
            EntornoSe.AjustarModalesAbiertas();
            let mensaje: HTMLInputElement = document.getElementById(`${this._idModalBorrar}-mensaje`) as HTMLInputElement;
            if (this.InfoSelector.Cantidad > 1) {
                mensaje.value = "Seguro desea borrar todos los elementos seleccionados";
            }
            else {
                mensaje.value = "Seguro desea borrar el elemento seleccionado";
            }
        }

        public ModalDeBorrado_Borrar() {
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
            mantenimiento.ModalDeBorrado_Cerrar();
            mantenimiento.InfoSelector.QuitarTodos();
            mantenimiento.Buscar(atGrid.accion.buscar, 0);
        }

        private SiHayErrorTrasPeticionDeBorrar(peticion: ApiDeAjax.DescriptorAjax) {
            let mantenimiento: CrudMnt = peticion.llamador as CrudMnt;
            mantenimiento.ModalDeBorrado_Cerrar();
            mantenimiento.BlanquearTodosLosCheck();
            mantenimiento.SiHayErrorTrasPeticionAjax(peticion);
        }

        public ModalDeBorrado_Cerrar() {
            this.ModoTrabajo = ModoTrabajo.mantenimiento;
            ApiCrud.CerrarModal(this.ModalDeBorrado);
        }

        public IraEditar() {
            if (this.InfoSelector.Cantidad == 0) {
                MensajesSe.Error("IraEditar", "Debe marcar el elemento a editar");
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

        public RestaurarPagina(): Promise<boolean> {
            this.DatosDelGrid.InicializarCache();
            this.Navegador.EsRestauracion = false;
            let cantidad: number = this.Navegador.Cantidad;
            let pagina: number = this.Navegador.Pagina;
            let posicion: number = 0;
            let accion: string = atGrid.accion.buscar;
            if (pagina > 1) {
                posicion = (pagina - 1) * cantidad;
                accion = atGrid.accion.restaurar;
            }
            return this.PromesaDeCargarGrid(accion, posicion);
        }

        private DefinirPeticionDeBorrado(): string {
            let idsJson: string = JSON.stringify(this.InfoSelector.IdsSeleccionados);
            var controlador = this.Navegador.Controlador;
            let url: string = `/${controlador}/${Ajax.EndPoint.Borrar}`;
            let parametros: string = `${Ajax.Param.idsJson}=${idsJson}`;
            let peticion: string = url + '?' + parametros;
            return peticion;
        }

        public Buscar(accion: string, posicion: number) {
            this.DatosDelGrid.InicializarCache();
            if (accion !== atGrid.accion.restaurar) {
                this.Navegador.Pagina = 1;
                this.Navegador.Posicion = 0;
            }
            this.CargarGrid(accion, posicion);
        }

        public CambiarSelector(idSelector: string) {
            var htmlSelector: HTMLSelector = <HTMLSelector>document.getElementById(idSelector);

            let modal: ModalSeleccion = crudMnt.ObtenerModalDeSeleccion(htmlSelector.getAttribute(atSelectorDeFiltro.idModal));
            if (IsNullOrEmpty(htmlSelector.value))
                modal.InicializarModalDeSeleccion();
            else
                modal.TextoSelectorCambiado();
        }

        private ValidarRestrictorDeFiltrado(): boolean {
            let restrictoresDeFiltro: NodeListOf<HTMLInputElement> = this.ZonaDeFiltro.querySelectorAll(`input[${atControl.tipo}="${TipoControl.restrictorDeFiltro}"]`) as NodeListOf<HTMLInputElement>;
            if (restrictoresDeFiltro.length == 0)
                return false;
            return true;
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

        public OcultarMostrarBloque(idHtmlBloque: string): void {
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

        public ModalEnviarCorreo_Abrir() {
            this.ModoTrabajo = ModoTrabajo.enviandoCorreo;
            this.ModalDeEnviarCorreo.style.display = 'block';
            for (let i = 0; i < this.InfoSelector.Cantidad; i++)
                ApiCrud.CrearEnlaceAlElemento(this.ModalDeEnviarCorreo_DivDeElementos, this.InfoSelector.LeerElemento(i));

            EntornoSe.AjustarModalesAbiertas();
        }

        public ModalEnviarCorreo_Cerrar() {
            this.ModoTrabajo = ModoTrabajo.mantenimiento;
            this.ModalDeEnviarCorreo_DivDeElementos.innerHTML = "";
            ApiCrud.CerrarModal(this.ModalDeEnviarCorreo);
        }


        public ModalEnviarCorreo_Enviar(): void {
            let parametros: Array<Parametro> = this.ParametrosDeEnviarCorreos();
            ApiDePeticiones.EnviarCorreo(this, this.Controlador, parametros)
                .then((peticion) => this.DespuesDeEnviarCorreo(peticion))
                .catch((peticion) => this.ErrorAlEnviarCorreo(peticion));
        }

        public DespuesDeEnviarCorreo(peticion: ApiDeAjax.DescriptorAjax): any {
            MensajesSe.Info(peticion.resultado.mensaje);
        }

        public ErrorAlEnviarCorreo(peticion: ApiDeAjax.DescriptorAjax): any {
            MensajesSe.Error(peticion.nombre, peticion.resultado.mensaje, peticion.resultado.consola);
        }

        public ParametrosDeEnviarCorreos(): Array<Parametro> {
            let parametros: Array<Parametro> = new Array<Parametro>();
            let idPuestos: string = this.ModalDeEnviarCorreo.id + '_selector-puestos_editor';
            let idUsuarios: string = this.ModalDeEnviarCorreo.id + '_selector-usuario_editor';
            let idAsunto: string = this.ModalDeEnviarCorreo.id + '_asunto';
            let idCuerpo: string = this.ModalDeEnviarCorreo.id + '_mensaje';
            let idAjuntos: string = this.ModalDeEnviarCorreo.id + '_elementos_ref';

            let idsDeUsuarios: string = (document.getElementById(idUsuarios) as HTMLInputElement).getAttribute(atSelectorDeElementos.Seleccionados);
            let usuarios: Array<string> = ToLista(idsDeUsuarios, ',');

            let idsDePuestos: string = (document.getElementById(idPuestos) as HTMLInputElement).getAttribute(atSelectorDeElementos.Seleccionados);
            let puestos: Array<string> = ToLista(idsDePuestos, ',');

            if (puestos.length == 0 && usuarios.length == 0)
                throw new Error("Al menos debe definir un receptor");

            let asunto: string = (document.getElementById(idAsunto) as HTMLInputElement).value;
            let cuerpo: string = (document.getElementById(idCuerpo) as HTMLInputElement).value;

            if (IsNullOrEmpty(asunto))
                throw new Error("Debe indicar el asunto");

            let divAdjuntos: HTMLDivElement = (document.getElementById(idAjuntos) as HTMLDivElement);
            let adjuntos: string[] = [];
            let refAdjuntos = divAdjuntos.querySelectorAll("a");
            for (let i: number = 0; i < refAdjuntos.length; i++)
                adjuntos.push(`${this.Dto}:${refAdjuntos[i].getAttribute(atControl.idElemento)}:${refAdjuntos[i].text}`);


            parametros.push(new Parametro(Ajax.Param.negocio, this.Negocio));
            parametros.push(new Parametro('usuarios', usuarios));
            parametros.push(new Parametro('puestos', puestos));
            parametros.push(new Parametro('asunto', asunto));
            parametros.push(new Parametro('cuerpo', cuerpo));
            parametros.push(new Parametro('adjuntos', adjuntos));
            return parametros;
        }

        public ObtenerModalParaSeleccionar(idModal: string): ModalParaSeleccionar {
            for (let i: number = 0; i < this.ModalesParaSeleccionar.length; i++) {
                let modal: ModalParaSeleccionar = this.ModalesParaSeleccionar[i];
                if (modal.IdModal === idModal)
                    return modal;
            }

            let modal: ModalParaSeleccionar = new ModalParaSeleccionar(this, idModal);
            this.ModalesParaSeleccionar.push(modal);
            return modal;
        }

        public ObtenerFocoEnSelector(idSelector: string) {
            let selector: HTMLDivElement = ApiCrud.ObtenerSelector(idSelector);
            let editor: HTMLInputElement = ApiCrud.ObtenerEditorAsociadoAlSelector(selector);
            editor.setAttribute(atSelectorDeElementos.ValorAlEntrar, editor.value);
        }

        public PerderElFocoEnUnSelectorDesdeUnaModal(idModalQueSeAbre: string, idModalQueSeCierra: string, idSelector: string) {
            let selector: HTMLDivElement = ApiCrud.ObtenerSelector(idSelector);
            let editor: HTMLInputElement = ApiCrud.ObtenerEditorAsociadoAlSelector(selector);
            let valorAlEntrar = editor.getAttribute(atSelectorDeElementos.ValorAlEntrar);
            if (editor.value === valorAlEntrar || IsNullOrEmpty(editor.value))
                return;
            selector.setAttribute(atSelectorDeElementos.CerrarAutomaticamente, 'S');
            this.AbrirModalParaSeleccionarDesdeUnaModal(idModalQueSeAbre, idModalQueSeCierra, idSelector);
        }

        public AbrirModalParaSeleccionarDesdeUnaModal(idModalQueSeAbre: string, idModalQueSeCierra: string, idSelector: string) {

            ApiCrud.OcultarModalPorId(idModalQueSeCierra);

            let modal: ModalParaSeleccionar = this.ObtenerModalParaSeleccionar(idModalQueSeAbre);
            if (NoDefinida(modal))
                throw new Error(`Modal ${idModalQueSeAbre} no definida`);

            let selector: HTMLDivElement = ApiCrud.ObtenerSelector(idSelector);

            selector.setAttribute(atSelectorDeElementos.ModalPadre, idModalQueSeCierra);

            modal.AbrirModalParaSeleccionar(selector);
        }

        public ModalExportacion_Abrir() {
            this.ModoTrabajo = ModoTrabajo.exportando;
            this.ModalDeExportacion.style.display = 'block';
            EntornoSe.AjustarModalesAbiertas();
        }

        public ModalExportacion_Cerrar() {
            this.ModoTrabajo = ModoTrabajo.mantenimiento;
            ApiCrud.CerrarModal(this.ModalDeExportacion);
        }

        public ModalExportacion_SalirDeListaDeCorreos(): void {
            let idCorreos: string = this.ModalDeExportacion.id + '_correos';
            let correos: HTMLInputElement = document.getElementById(idCorreos) as HTMLInputElement;
            if (!IsNullOrEmpty(correos.value)) {
                ApiControl.AnularError(correos);
                let lista = correos.value.split(';');
                let correoMalo: string = this.ValidarListaDeCorreos(lista);
                if (!IsNullOrEmpty(correoMalo)) {
                    ApiControl.MarcarError(correos);
                    throw Error(`El correo ${correoMalo} no es válido`);
                }
            }
        }

        private ValidarListaDeCorreos(lista: Array<string>): string {
            for (let i: number = 0; i < lista.length; i++) {
                if (!EsCorreoValido(lista[i].trim())) {
                    return lista[i].trim();
                }
            }
            return '';
        }

        public ModalExportacion_CheckSometerPulsado(): void {
            let idCheck: string = this.ModalDeExportacion.id + '_sometido';
            let idCorreos: string = this.ModalDeExportacion.id + '_correos';
            let check: HTMLInputElement = document.getElementById(idCheck) as HTMLInputElement;
            let correos: HTMLInputElement = document.getElementById(idCorreos) as HTMLInputElement;
            if (check.checked) {
                ApiControl.DesbloquearEditor(correos);
                correos.value = Registro.UsuarioConectado().mail;
            }
            else {
                ApiControl.BlanquearEditor(correos);
                ApiControl.BloquearEditor(correos);
            }

        }

        public ModalExportacion_Exportar(): void {
            let parametros: Array<Parametro> = this.ParametrosDeExportacion();
            ApiDePeticiones.Exportar(this, this.Controlador, parametros)
                .then((peticion) => this.DespuesDeExportar(peticion))
                .catch((peticion) => this.ErrorAlExportar(peticion));
        }


        public ParametrosDeExportacion(): Array<Parametro> {
            let parametros: Array<Parametro> = new Array<Parametro>();
            let idMostradas: string = this.ModalDeExportacion.id + '_mostradas';
            let idSometido: string = this.ModalDeExportacion.id + '_sometido';
            let idCorreo: string = this.ModalDeExportacion.id + '_correos';
            let mostradas: boolean = (document.getElementById(idMostradas) as HTMLInputElement).checked;

            let controlSometido: HTMLInputElement = document.getElementById(idSometido) as HTMLInputElement;
            let parametroSometido: string = controlSometido.getAttribute(atControl.propiedad);
            let sometido: boolean = controlSometido.checked;

            let controlReceptor: HTMLInputElement = document.getElementById(idCorreo) as HTMLInputElement;
            let parametroReceptor: string = controlReceptor.getAttribute(atControl.propiedad);
            let receptores: string = controlReceptor.value;

            if (sometido && IsNullOrEmpty(receptores))
                throw Error(`Debe indicar al menos un correo`);

            let lista = receptores.split(';');
            let correoMalo: string = this.ValidarListaDeCorreos(lista);
            if (!IsNullOrEmpty(correoMalo)) {
                ApiControl.MarcarError(document.getElementById(idCorreo) as HTMLInputElement);
                throw Error(`El correo ${correoMalo} no es válido`);
            }

            let posicion = 0;
            let cantidad = -1;
            if (mostradas) {
                cantidad = this.Navegador.Cantidad;
                posicion = this.Navegador.Posicion;
                posicion = posicion - cantidad;
                if (posicion < 0) posicion = 0;
            }
            parametros.push(new Parametro(Ajax.Param.negocio, this.Negocio));
            parametros.push(new Parametro(Ajax.Param.posicion, posicion));
            parametros.push(new Parametro(Ajax.Param.cantidad, cantidad));
            parametros.push(new Parametro(parametroSometido, sometido));
            parametros.push(new Parametro(parametroReceptor, receptores));
            parametros.push(new Parametro(Ajax.Param.filtro, this.ObtenerFiltros()));
            parametros.push(new Parametro(Ajax.Param.orden, this.ObtenerOrdenacion()));
            return parametros;
        }

        public DespuesDeExportar(peticion: ApiDeAjax.DescriptorAjax): any {
            let crud: CrudMnt = peticion.llamador;
            let parametros: Array<Parametro> = peticion.DatosDeEntrada;
            for (let i: number = 0; i < parametros.length; i++) {
                if (parametros[i].Parametro === 'sometido' && (parametros[i].valor)) {
                    MensajesSe.Info(peticion.resultado.mensaje);
                    return;
                }
            }
            crud.DescargarArchivo(peticion);
        }


        public DescargarArchivo(peticion: ApiDeAjax.DescriptorAjax): any {
            var downloadLink = document.createElement("a");
            document.body.appendChild(downloadLink);
            downloadLink.href = peticion.resultado.datos;
            downloadLink.click();
            document.body.removeChild(downloadLink);
        }

        public ErrorAlExportar(peticion: ApiDeAjax.DescriptorAjax): any {
            MensajesSe.Error(peticion.nombre, peticion.resultado.mensaje, peticion.resultado.consola);
        }

    }

}