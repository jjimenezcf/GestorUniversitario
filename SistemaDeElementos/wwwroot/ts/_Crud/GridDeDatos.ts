namespace Crud {

    class ClausulaDeOrdenacion {
        ordenarPor: string;
        modo: string;

        constructor(ordenarPor: string, modo: string) {
            this.ordenarPor = ordenarPor;
            this.modo = modo;
        }
    }


    class ResultadoDeLectura {
        registros: any;
        total: number;
    }


    export class DatosPeticionNavegarGrid {
        private _grid: GridDeDatos;
        private _accion: string;
        private _posicion: number;

        public get Grid(): GridDeDatos {
            return this._grid;
        }

        public get Accion(): string {
            return this._accion;
        }
        public get PosicionDesdeLaQueSeLee(): number {
            return this._posicion;
        }
        constructor(grid: GridDeDatos, accion: string, posicion: number) {
            this._grid = grid;
            this._accion = accion;
            this._posicion = posicion;
        }

    }

    class InfoNavegador {
        public cantidad: number;
        public posicion: number;
        public pagina: number;
        public leidos: number;
        public total: number;
    }

    class Navegador {

        private id: string;
        private idInfo: string;
        private idMensaje: string;

        private esRestauracion: boolean;

        public get EsRestauracion(): boolean {
            return this.esRestauracion;
        }
        public set EsRestauracion(valor: boolean) {
            this.esRestauracion = valor;
        }

        public get Cantidad(): number {
            return Numero(this.Navegador.value);
        }
        public get Posicion(): number {
            return Numero(this.Navegador.getAttribute(atGrid.navegador.posicion));
        }
        public get Pagina(): number {
            return Numero(this.Navegador.getAttribute(atGrid.navegador.pagina));
        }
        public get Leidos(): number {
            return Numero(this.Navegador.getAttribute(atGrid.navegador.leidos));
        }
        public get Total(): number {
            return Numero(this.Navegador.getAttribute(atGrid.navegador.total));
        }
        public get Id(): string {
            return this.id;
        }

        public set Cantidad(valor: number) {
            this.Navegador.value = valor.toString();
        }
        public set Posicion(valor: number) {
            this.Navegador.setAttribute(atGrid.navegador.posicion, valor.toString());
        }
        public set Pagina(valor: number) {
            this.Navegador.setAttribute(atGrid.navegador.pagina, valor.toString());
        }
        public set Info(valor: string) {
            let div: HTMLDivElement = document.getElementById(this.idInfo) as HTMLDivElement;
            div.innerHTML = valor;
        }
        public set Mensaje(valor: string) {
            let div: HTMLDivElement = document.getElementById(this.idMensaje) as HTMLDivElement;
            div.innerHTML = valor;
        }
        public set Leidos(valor: number) {
            this.Navegador.setAttribute(atGrid.navegador.leidos, valor.toString());
        }
        public set Total(valor: number) {
            this.Navegador.setAttribute(atGrid.navegador.total, valor.toString());
        }
        public set Id(valor: string) {
            this.id = valor;
        }

        constructor(idGrid: string) {
            this.id = `${idGrid}_${atGrid.idCtrlCantidad}`;
            this.idInfo = `${idGrid}_${atGrid.idInfo}`;
            this.idMensaje = `${idGrid}_${atGrid.idMensaje}`;
        }

        public get Navegador(): HTMLInputElement {
            let input = document.getElementById(this.Id) as HTMLInputElement;
            return input;
        }

        public get Controlador(): string {
            return this.Navegador.getAttribute(atControl.controlador);
        }

        public get Datos(): InfoNavegador {
            let datos: InfoNavegador = new InfoNavegador();
            datos.cantidad = this.Cantidad;
            datos.leidos = this.Leidos;
            datos.pagina = this.Pagina;
            datos.posicion = this.Posicion;
            datos.total = this.Total;
            return datos;
        }

        public RestaurarDatos(datos: InfoNavegador): void {
            if (datos !== undefined) {
                this.Cantidad = datos.cantidad;
                this.Leidos = datos.leidos;
                this.Pagina = datos.pagina;
                this.Posicion = datos.posicion;
                this.Total = datos.total;
                this.EsRestauracion = true;
            }
            else {
                this.Cantidad = 10;
                this.Leidos = 0;
                this.Pagina = 1;
                this.Posicion = 0;
                this.Total = 0;
                this.esRestauracion = false;
            }
        }

        public Actualizar(accion: string, posicionDesdeLaQueSeLeyo: number, registrosLeidos: number, seleccionados: number): void {
            this.Leidos = registrosLeidos;
            this.Posicion = accion == atGrid.accion.ultima ? this.Total - registrosLeidos : posicionDesdeLaQueSeLeyo + registrosLeidos;
            let paginasTotales: number = Math.ceil(this.Total / this.Cantidad);

            let paginaAnterior: number = this.Pagina;
            let paginaNueva: number = 1;
            if (accion === atGrid.accion.siguiente)
                paginaNueva = paginaAnterior + 1;
            else
                if (accion === atGrid.accion.anterior)
                    paginaNueva = paginaAnterior - 1;
                else
                    if (accion === atGrid.accion.restaurar)
                        paginaNueva = paginaAnterior;
                    else
                        if (accion === atGrid.accion.ultima) {
                            posicionDesdeLaQueSeLeyo = this.Total - registrosLeidos;
                            paginaNueva = (this.Cantidad >= this.Total) ? 1 : paginasTotales;
                        }
            this.Pagina = paginaNueva <= 0 ? 1 : paginaNueva;
            this.Info = `Pagina ${this.Pagina} de ${paginasTotales}`;
            this.InformarElementosSeleccionados(seleccionados);
        }

        public InformarElementosSeleccionados(seleccionados: number): void {
            this.Mensaje = `Seleccionados ${seleccionados} de ${this.Total}`;
        }
    }

    class PaginaDelGrid {
        private _elementos: Elemento[] = [];
        private _pagina: number;
        private _posicion: number;
        private _cantidad: number;
        private _fecha: Date;
        public get fecha(): string {
            return this._fecha.toISOString();
        }
        public get Pagina(): number {
            return this._pagina;
        }
        public get Posicion(): number {
            return this._posicion;
        }
        public get Cantidad(): number {
            return this._cantidad;
        }
        public get Elementos(): Elemento[] {
            return this._elementos;
        }
        public get Registros(): any[] {
            let registros: any[] = [];
            for (let i: number = 0; i < this.Elementos.length; i++) {
                let registro: any = this.Elementos[i].Registro;
                registros.push(registro);
            }
            return registros;
        }

        constructor(pagina: number, posicion: number, cantidad: number, registros: [], expresionMostrar: string) {
            this.anadirElementos(registros, expresionMostrar);
            this._pagina = pagina;
            this._cantidad = cantidad;
            this._posicion = posicion;
            this._fecha = new Date(Date.now());
        };

        public Obtener(id: number): Elemento {
            for (let i: number = 0; i < this._elementos.length; i++) {
                if (this._elementos[i].Id === id)
                    return this._elementos[i];
            }
            return null;
        }

        private anadirElementos(registros: [], expresionMostrar: string) {
            for (let i: number = 0; i < registros.length; i++) {
                let e: Elemento = new Elemento(registros[i], expresionMostrar);
                this._elementos.push(e);
            }
        }
    }

    class DatosDelGrid {
        private _paginas: PaginaDelGrid[] = [];
        private _paginaActual: number;
        public set PaginaActual(numeroDePagina: number) {
            this._paginaActual = numeroDePagina;
        }

        public AnadirPagina(numeroDePagina: number, posicion: number, cantidad: number, registros: [], expresionMostrar: string) {
            let i: number = this.Buscar(numeroDePagina);
            if (i >= 0) {
                this._paginas.splice(i, 1);
            }
            let p: PaginaDelGrid = new PaginaDelGrid(numeroDePagina, posicion, cantidad, registros, expresionMostrar);
            this._paginas.push(p);
        }

        public InicializarCache() {
            this._paginas.splice(0, this._paginas.length);
        }

        public Pagina(numeroDePagina: number): PaginaDelGrid {
            let i: number = this.Buscar(numeroDePagina);
            if (i >= 0) {
                return this._paginas[i];
            }
            return null;
        }

        public Obtener(id: number): Elemento {
            let p: PaginaDelGrid = this.Pagina(this._paginaActual);
            if (p === null)
                throw Error(`la página ${this._paginaActual} no se encuentra en la lista de páginas del grid`);

            let e: Elemento = p.Obtener(id);
            if (e === null)
                throw Error(`El elemento con id ${id} no se encuentra en la página actual del grid`);
            return e;
        }

        private Buscar(numeroDePagina: number): number {
            for (let i: number = 0; i < this._paginas.length; i++)
                if (this._paginas[i].Pagina === numeroDePagina)
                    return i;
            return -1;
        }
    }

    export class GridDeDatos extends CrudBase {

        protected Ordenacion: Tipos.Ordenacion;
        public Navegador: Navegador;

        private _infoSelector: InfoSelector;
        public get InfoSelector(): InfoSelector {
            return this._infoSelector;
        }

        public DatosDelGrid: DatosDelGrid = new DatosDelGrid();

        private _idGrid: string;
        protected get IdGrid(): string {
            return this._idGrid;
        }

        protected get EsModalDeSeleccion(): boolean {
            return this.constructor.name === ModalSeleccion.name;
        }

        protected get EsModalParaConsultarRelaciones(): boolean {
            return this.constructor.name === ModalParaConsultarRelaciones.name;
        }

        protected get EsModalParaRelacionar(): boolean {
            return this.constructor.name === ModalParaRelacionar.name;
        }

        protected get EsModalConGrid(): boolean {
            return this.EsModalParaRelacionar || this.EsModalDeSeleccion || this.EsModalParaConsultarRelaciones;
        }

        protected get EsCrud(): boolean {
            return EsObjetoDe(this, CrudMnt);
        }


        private _idCuerpoCabecera: string;
        public get IdCuerpoCabecera(): string {
            return this._idCuerpoCabecera;
        }
        public get CuerpoCabecera(): HTMLDivElement {
            return document.getElementById(this._idCuerpoCabecera) as HTMLDivElement;
        }
        public get CuerpoDatos(): HTMLDivElement {
            return document.getElementById(`cuerpo.datos.${this._idCuerpoCabecera}`) as HTMLDivElement;
        }
        public get CuerpoPie(): HTMLDivElement {
            return document.getElementById(`cuerpo.pie.${this._idCuerpoCabecera}`) as HTMLDivElement;
        }
        public get IdNegocio(): number {
            return Numero((this.CuerpoCabecera.getAttribute(atMantenimniento.idNegocio)));
        }
        public get Negocio() {
            return this.CuerpoCabecera.getAttribute(atMantenimniento.negocio);
        }

        private _idHtmlFiltro: string;
        public get ZonaDeFiltro(): HTMLDivElement {
            return document.getElementById(this._idHtmlFiltro) as HTMLDivElement;
        }
        public get EtiquetaMostrarOcultarFiltro(): HTMLElement {
            return document.getElementById(`mostrar.${this.IdCuerpoCabecera}.ref`) as HTMLElement;
        }
        public get ExpandirFiltro(): HTMLInputElement {
            return document.getElementById(`expandir.${this.IdCuerpoCabecera}`) as HTMLInputElement;
        }
        public get InputSeleccionadas(): HTMLInputElement {
            let idInput = this.EsCrud
                ? `div.seleccion.${this.IdGrid}.input`
                : `div.seleccion.${this.IdGrid}.input`;
            return document.getElementById(idInput) as HTMLInputElement;
        }
        public get EtiquetasSeleccionadas(): HTMLElement {
            let idRef = this.EsCrud
                ? `div.seleccion.${this.IdGrid}.ref`
                : `div.seleccion.${this.IdGrid}.ref`;
            return document.getElementById(idRef) as HTMLElement;
        }

        protected get Grid(): HTMLDivElement {
            return document.getElementById(this.IdGrid) as HTMLDivElement;
        }

        protected get CabeceraTablaGrid(): HTMLTableSectionElement {
            let idCabecera = this.Grid.getAttribute(atGrid.cabeceraTabla);
            return document.getElementById(idCabecera) as HTMLTableSectionElement;
        }

        public get CuerpoTablaGrid(): HTMLTableSectionElement {
            return document.getElementById(`${this.Grid.id}_tbody`) as HTMLTableSectionElement;
        }

        protected get ZonaNavegador(): HTMLDivElement {
            let idNavegador = this.Grid.getAttribute(atGrid.zonaNavegador);
            return document.getElementById(idNavegador) as HTMLDivElement;
        }

        public get Tabla(): HTMLTableElement {
            let idTabla: string = this.Grid.getAttribute(atControl.tablaDeDatos);
            return document.getElementById(idTabla) as HTMLTableElement;
        }

        private _idHtmlZonaMenu: string;
        public get ZonaDeMenu(): HTMLDivElement {
            return document.getElementById(this._idHtmlZonaMenu) as HTMLDivElement;
        }

        public get OpcionesPorElemento(): NodeListOf<HTMLButtonElement> {
            return this.ZonaDeMenu.querySelectorAll(`input[${atOpcionDeMenu.clase}="${ClaseDeOpcioDeMenu.DeElemento}"]`) as NodeListOf<HTMLButtonElement>;
        }

        private _idModal: string;
        public set IdModal(idModal: string) { this._idModal = idModal; };
        public get IdModal(): string { return this._idModal; };
        protected get Modal(): HTMLDivElement {
            return document.getElementById(this._idModal) as HTMLDivElement;
        };

        constructor(idPanelMnt: string) {
            super();
            this._idCuerpoCabecera = idPanelMnt;

            if (this.CuerpoCabecera === null)
                throw Error(`No se puede crear el Crud ${idPanelMnt} la cabecera es nula`);

            this._controlador = this.CuerpoCabecera.getAttribute(atMantenimniento.controlador);
            this._idGrid = this.CuerpoCabecera.getAttribute(atMantenimniento.gridDelMnt);
            this._idHtmlZonaMenu = this.CuerpoCabecera.getAttribute(atMantenimniento.zonaMenu);
            this._idHtmlFiltro = this.Grid.getAttribute(atMantenimniento.zonaDeFiltro);

            this.Navegador = new Navegador(this.IdGrid);
            this.Ordenacion = new Tipos.Ordenacion();
            this._infoSelector = new InfoSelector(this.IdGrid);
        }

        public Inicializar(idPanelMnt: string) {
            super.Inicializar(idPanelMnt);
            this.InicializarNavegador();
        }

        private InicializarNavegador() {

            let elementos: Elemento[] = this.Estado.Obtener("elementos_seleccionados") as Elemento[];

            if (elementos !== undefined) {
                for (var i = 0; i < elementos.length; i++) {
                    let e: Elemento = new Elemento(elementos[i]["_registro"]);
                    this.InfoSelector.InsertarElemento(e);
                }
                this.Estado.Quitar("elementos_seleccionados");
            }

            this.Navegador.RestaurarDatos(this.Estado.Obtener(atGrid.id));
            for (var i = 0; i < this.Ordenacion.Count(); i++) {
                let orden: Tipos.Orden = this.Ordenacion.Leer(i);
                let columna: HTMLTableHeaderCellElement = document.getElementById(orden.IdColumna) as HTMLTableHeaderCellElement;
                columna.setAttribute(atControl.modoOrdenacion, orden.Modo);
                let a: HTMLElement = columna.getElementsByTagName('a')[0] as HTMLElement;
                a.setAttribute("class", orden.ccsClase);
            }
        }

        protected PosicionarGrid(): void {
            this.Grid.style.position = 'fixed';
            let posicionGrid: number = this.PosicionGrid();
            this.Grid.style.top = `${posicionGrid}px`;

            let alturaDelGrid: number = this.AlturaDelGrid(posicionGrid);
            this.Grid.style.height = `${alturaDelGrid}px`;

            let cuerpoDeLaTabla: HTMLTableSectionElement = this.CuerpoTablaGrid;
            if (cuerpoDeLaTabla !== null && cuerpoDeLaTabla !== undefined) {
                this.FijarAlturaCuerpoDeLaTabla(alturaDelGrid);
            }
        }

        public PosicionGrid(): number {
            let alturaCabeceraPnlControl: number = AlturaCabeceraPnlControl();
            let alturaCabeceraMnt: number = this.CuerpoCabecera.getBoundingClientRect().height;
            let alturaFiltro: number = 0;
            if (NumeroMayorDeCero(this.ExpandirFiltro.value)) {
                alturaFiltro = this.ZonaDeFiltro.getBoundingClientRect().height;
            }
            return alturaCabeceraPnlControl + alturaCabeceraMnt + alturaFiltro;
        }

        public AlturaDelGrid(posicionGrid: number): number {
            let alturaPiePnlControl: number = AlturaPiePnlControl();
            let alturaZonaNavegador: number = this.ZonaNavegador.getBoundingClientRect().height;
            return AlturaFormulario() - posicionGrid - alturaPiePnlControl - alturaZonaNavegador;
        }

        /**
         le he puesto -9 ya que le he pintado bordes al cuerpo del grid
         */
        public FijarAlturaCuerpoDeLaTabla(alturaDelGrid: number): void {
            let alturaCabecera = this.CabeceraTablaGrid.getBoundingClientRect().height;
            this.CuerpoTablaGrid.style.height = `${alturaDelGrid - alturaCabecera - 9}px`;
        }

        protected ActualizarNavegadorDelGrid(accion: string, posicionDesdeLaQueSeLeyo: number, registrosLeidos: number) {

            this.Navegador.Actualizar(accion, posicionDesdeLaQueSeLeyo, registrosLeidos, this.InfoSelector.Cantidad);

            //for (var i = 0; i < this.Ordenacion.Count(); i++) {
            //    let orden: ApiControl.Orden = this.Ordenacion.Leer(i);
            //    let columna: HTMLTableHeaderCellElement = document.getElementById(orden.IdColumna) as HTMLTableHeaderCellElement;
            //    columna.setAttribute(atControl.modoOrdenacion, orden.Modo);
            //    let a: HTMLElement = columna.getElementsByTagName('a')[0] as HTMLElement;
            //    a.setAttribute("class", orden.ccsClase);
            //}
        }

        protected EstablecerOrdenacion(idcolumna: string) {
            let htmlColumna: HTMLTableHeaderCellElement = document.getElementById(idcolumna) as HTMLTableHeaderCellElement;
            let modo: string = htmlColumna.getAttribute(atControl.modoOrdenacion);
            if (IsNullOrEmpty(modo))
                modo = ModoOrdenacion.ascedente;
            else if (modo === ModoOrdenacion.ascedente)
                modo = ModoOrdenacion.descendente;
            else if (modo === ModoOrdenacion.descendente)
                modo = ModoOrdenacion.sinOrden;
            else if (modo === ModoOrdenacion.sinOrden)
                modo = ModoOrdenacion.ascedente;

            let propiedad: string = htmlColumna.getAttribute(atControl.propiedad);
            let ordenarPor: string = htmlColumna.getAttribute(atControl.ordenarPor);
            this.Ordenacion.Actualizar(idcolumna, propiedad, modo, ordenarPor);

            //htmlColumna.setAttribute(atControl.modoOrdenacion, modo);

        }

        protected ObtenerOrdenacion() {
            var clausulas = new Array<ClausulaDeOrdenacion>();
            for (var i = 0; i < this.Ordenacion.Count(); i++) {
                let orden = this.Ordenacion.Leer(i);
                clausulas.push(new ClausulaDeOrdenacion(orden.OrdenarPor, orden.Modo));
            }
            return JSON.stringify(clausulas);
        }

        protected ObtenerFiltros(): string {
            let arrayIds: Array<string> = this.ObtenerControlesDeFiltro();
            var clausulas = new Array<ClausulaDeFiltrado>();
            for (let i = 0; i < arrayIds.length; i++) {
                var clausula: ClausulaDeFiltrado = null;
                var control: HTMLElement = document.getElementById(`${arrayIds[i]}`);
                var tipo: string = control.getAttribute(TipoControl.Tipo);
                switch (tipo) {
                    case TipoControl.restrictorDeFiltro: {
                        clausula = this.ObtenerClausulaRestrictor(control as HTMLInputElement);;
                        break;
                    }
                    case TipoControl.Editor: {
                        clausula = this.ObtenerClausulaEditor(control as HTMLInputElement);
                        break;
                    }
                    case TipoControl.Selector: {
                        clausula = this.ObtenerClausulaSelector(control as HTMLInputElement);;
                        break;
                    }
                    case TipoControl.ListaDeElementos: {
                        clausula = this.ObtenerClausulaListaDeELemento(control as HTMLSelectElement);
                        break;
                    }
                    case TipoControl.ListaDinamica: {
                        clausula = this.ObtenerClausulaListaDinamica(control as HTMLInputElement);
                        break;
                    }
                    case TipoControl.Check: {
                        clausula = this.ObtenerClausulaCheck(control as HTMLInputElement);
                        break;
                    }
                    case TipoControl.FiltroEntreFechas: {
                        clausula = this.ObtenerClausulaEntreFechas(control as HTMLInputElement);
                        break;
                    }
                    default: {
                        MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `No está implementado como definir la cláusula de filtrado de un tipo ${tipo}`);
                    }

                }

                if (clausula !== null)
                    clausulas.push(clausula);

                this.FiltrosExcluyentes(clausulas);
            }

            return JSON.stringify(clausulas);
        }

        protected FiltrosExcluyentes(clausulas: ClausulaDeFiltrado[]) {
            return clausulas;
        }

        private ObtenerControlesDeFiltro(): Array<string> {
            var arrayIds = new Array<string>();
            var arrayHtmlInput = this.ZonaDeFiltro.querySelectorAll(`input[${atControl.filtro}="S"]`) as NodeListOf<HTMLButtonElement>;
            for (let i = 0; i < arrayHtmlInput.length; i++) {
                var htmlInput = arrayHtmlInput[i];
                var id = htmlInput.getAttribute(atControl.id);
                if (id === null)
                    console.log(`Falta el atributo id del componente de filtro ${htmlInput}`);
                else
                    arrayIds.push(id);
            }

            var arrayHtmlSelect = this.ZonaDeFiltro.querySelectorAll(`select[${atControl.filtro}="S"]`) as NodeListOf<HTMLButtonElement>;
            for (let i = 0; i < arrayHtmlSelect.length; i++) {
                var htmlSelect = arrayHtmlSelect[i];
                var id = htmlSelect.getAttribute(atControl.id);
                if (id === null)
                    console.log(`Falta el atributo id del componente de filtro ${htmlSelect}`);
                else
                    arrayIds.push(id);
            }


            return arrayIds;
        }

        private ObtenerClausulaRestrictor(restrictor: HTMLInputElement): ClausulaDeFiltrado {
            let propiedad: string = restrictor.getAttribute(atControl.propiedad);
            let criterio: string = literal.filtro.criterio.igual;
            let valor = restrictor.getAttribute(atControl.restrictor);
            let clausula: ClausulaDeFiltrado = null;
            if (!IsNullOrEmpty(valor))
                //clausula = { propiedad: `${propiedad}`, criterio: `${criterio}`, valor: `${valor}` };
                clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);

            return clausula;
        }

        private ObtenerClausulaEditor(editor: HTMLInputElement): ClausulaDeFiltrado {
            var propiedad: string = editor.getAttribute(atControl.propiedad);
            var criterio: string = editor.getAttribute(atControl.criterio);
            var valor = editor.value;
            var clausula = null;
            if (!IsNullOrEmpty(valor))
                //clausula = { propiedad: `${propiedad}`, criterio: `${criterio}`, valor: `${valor}` };
                clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);

            return clausula;
        }

        private ObtenerClausulaSelector(selector: HTMLInputElement): ClausulaDeFiltrado {
            var propiedad = selector.getAttribute(atControl.propiedad);
            var criterio = selector.getAttribute(atControl.criterio);
            var valor = null;
            var clausula = null;
            if (selector.hasAttribute(atSelector.ListaDeSeleccionados)) {
                var ids = selector.getAttribute(atSelector.ListaDeSeleccionados);
                if (!NoDefinida(ids)) {
                    valor = ids;
                    clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);
                }
            }
            return clausula;
        }

        private ObtenerClausulaListaDinamica(input: HTMLInputElement): ClausulaDeFiltrado {
            var propiedad = input.getAttribute(atControl.propiedad);
            var criterio = literal.filtro.criterio.igual;
            let valor: number = Numero(input.getAttribute(atListasDinamicas.idSeleccionado));

            var clausula = null;
            if (Number(valor) > 0) {
                clausula = new ClausulaDeFiltrado(propiedad, criterio, valor.toString());
            }
            return clausula;
        }

        private ObtenerClausulaCheck(input: HTMLInputElement): ClausulaDeFiltrado {
            let propiedad: string = input.getAttribute(atControl.propiedad);
            let criterio: string = literal.filtro.criterio.igual;
            let filtrarPorFalse = input.getAttribute(atCheck.filtrarPorFalse);
            let valor: boolean = input.checked;

            var clausula = null;
            if (valor || (filtrarPorFalse === "S" && !valor))
                clausula = new ClausulaDeFiltrado(propiedad, criterio, valor.toString());
            return clausula;
        }

        private ObtenerClausulaEntreFechas(input: HTMLInputElement): ClausulaDeFiltrado {
            let propiedad: string = input.getAttribute(atControl.propiedad);
            let criterio: string = literal.filtro.criterio.entreFechas;
            let valor: string = ApiControl.LeerEntreFechas(input);
            var clausula = null;
            if (valor.trim() !== '-') {
                clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);
            }
            return clausula;
        }

        private ObtenerClausulaListaDeELemento(selet: HTMLSelectElement): ClausulaDeFiltrado {
            var propiedad = selet.getAttribute(atControl.propiedad);
            var criterio = atCriterio.igual;
            var valor = selet.value;
            var clausula = null;
            if (!IsNullOrEmpty(valor) && Number(valor) > 0) {
                clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);
            }
            return clausula;
        }

        private ObtenerlaFila(idCheck: string): HTMLTableRowElement {
            let idFila: string = idCheck.replace(".chksel", "");
            let fila: HTMLTableRowElement = document.getElementById(idFila) as HTMLTableRowElement;
            return fila;
        }

        protected ActualizarInfoSelector(grid: GridDeDatos, elemento: Elemento): void {
            grid.InfoSelector.Quitar(elemento.Id);
            elemento = grid.DatosDelGrid.Obtener(elemento.Id);
            grid.InfoSelector.InsertarElemento(elemento);
            grid.Navegador.InformarElementosSeleccionados(grid.InfoSelector.Cantidad);
            grid.AplicarModoAccesoAlElemento(elemento);
        }

        protected AnadirAlInfoSelector(grid: GridDeDatos, elemento: Elemento): void {
            grid.InfoSelector.InsertarElemento(elemento);
            grid.Navegador.InformarElementosSeleccionados(grid.InfoSelector.Cantidad);
            grid.AplicarModoAccesoAlElemento(elemento);
        }

        protected QuitarDelSelector(grid: GridDeDatos, id: number): void {
            grid.InfoSelector.Quitar(id);
            grid.Navegador.InformarElementosSeleccionados(grid.InfoSelector.Cantidad);
        }

        protected EstaMarcado(idCheck: string): boolean {
            let id: number = this.ObtenerElIdDelElementoDelaFila(idCheck);
            return this.InfoSelector.Buscar(id) >= 0 ? true : false;
        }

        private ObtenerElIdDelElementoDelaFila(idCheck: string): number {
            let columnaId: string = idCheck.replace(".chksel", `.${literal.id}`);
            let inputId: HTMLInputElement = document.getElementById(columnaId) as HTMLInputElement;
            let id: string = inputId.value;
            return Numero(id);
        }

        protected MarcarElementos(): void {
            if (this.InfoSelector.Cantidad === 0)
                return;

            var celdasId = document.getElementsByName(`${literal.id}.${this.IdGrid}`);
            var len = celdasId.length;
            for (var i = this.InfoSelector.Cantidad-1; i >=0; i--) {
                let elemento: Elemento = this.InfoSelector.LeerElemento(i);
                for (var j = 0; j < len; j++) {
                    if (Numero((<HTMLInputElement>celdasId[j]).value) == elemento.Id) {
                        var idCheck = celdasId[j].id.replace(`.${atControl.id}`, LiteralMnt.postfijoDeCheckDeSeleccion);
                        var check = document.getElementById(idCheck);
                        (<HTMLInputElement>check).checked = true;
                        this.ActualizarInfoSelector(this, elemento);
                        break;
                    }
                }
            }
        }

        public BlanquearTodosLosCheck() {
            var celdasId = document.getElementsByName(`${literal.id}.${this.IdGrid}`);
            var len = celdasId.length;
            for (var j = 0; j < len; j++) {
                var idCheck = celdasId[j].id.replace(`.${atControl.id}`, LiteralMnt.postfijoDeCheckDeSeleccion);
                var check = document.getElementById(idCheck);
                (<HTMLInputElement>check).checked = false;
            }
            this.InfoSelector.QuitarTodos();
        }

        protected ActualizarInformacionDelGrid(grid: GridDeDatos) {
            if (!grid.EsModalConGrid && grid.Estado.Contiene(atGrid.idSeleccionado)) {
                let idSeleccionado: number = Numero(grid.Estado.Obtener(atGrid.idSeleccionado));
                let elemento: Elemento = this.DatosDelGrid.Obtener(idSeleccionado);
                grid.AnadirAlInfoSelector(grid, elemento);
                grid.Estado.Quitar(atGrid.idSeleccionado);
                grid.Estado.Quitar(atGrid.nombreSeleccionado);
            }

            grid.MarcarElementos();
            grid.InfoSelector.SincronizarCheck();

            //if (grid.InfoSelector.Cantidad > 0)
            //    grid.AccederAlModoDeAccesoAlElemento(this.InfoSelector.LeerElemento(0).Id);
        }

        protected obtenerValorDeLaFilaParaLaPropiedad(id: number, propiedad: string): string {
            let fila: HTMLTableRowElement = this.ObtenerFila(id);
            if (fila === null)
                return null;

            let celda: HTMLTableDataCellElement = this.ObtenerCelda(fila, propiedad);
            let input: HTMLInputElement = celda.querySelector("input");
            if (input === null)
                throw new Error(`la celda asociada a la propiedad '${propiedad}' no tiene un control input definido`);

            return input.value;
        }

        private ObtenerFila(id: number): HTMLTableRowElement {
            let tabla: HTMLTableElement = this.Tabla;
            for (var i = 0; i < tabla.rows.length; i++) {
                let fila: HTMLTableRowElement = tabla.rows[i];
                for (var j = 0; j < fila.cells.length; j++) {
                    let celda: HTMLTableDataCellElement = fila.cells[j];
                    let input: HTMLInputElement = celda.querySelector("input");
                    if (input !== null) {
                        let propiedad: string = input.getAttribute(atControl.propiedad);
                        if (propiedad.toLocaleLowerCase() === atControl.id) {
                            let valor: string = input.value;
                            if (Numero(valor) == id)
                                return fila;
                        }
                    }
                }
            }
            return null;

            //throw new Error(`No se ha localizado una fila con la propiedad Id definida`);
        }

        private ObtenerCelda(fila: HTMLTableRowElement, propiedadBuscada: string): HTMLTableDataCellElement {
            for (var j = 0; j < fila.cells.length; j++) {
                let celda: HTMLTableDataCellElement = fila.cells[j];
                let propiedadCelda: string = celda.getAttribute(atControl.propiedad);
                if (propiedadCelda.toLocaleLowerCase() === propiedadBuscada)
                    return celda;
            }
            throw new Error(`No se ha localizado una celda con la propiedad '${propiedadBuscada}' definida`);
        }

        public AntesDeNavegar(valores: Diccionario<any>) {
            super.AntesDeNavegar(valores);
            this.Estado.Agregar(atGrid.id, this.Navegador.Datos);

            let datosRestrictor: Tipos.DatosRestrictor = valores.Obtener(Sesion.restrictor) as Tipos.DatosRestrictor;
            let idSeleccionado: number = valores.Obtener(Sesion.idSeleccionado) as number;
            this.Estado.Agregar(atGrid.idSeleccionado, idSeleccionado);
            this.Estado.Agregar(atGrid.nombreSeleccionado, datosRestrictor.Texto);

            let paginaDestino: string = valores.Obtener(Sesion.paginaDestino);
            let estadoPaginaDestino: HistorialSe.EstadoPagina = EntornoSe.Historial.ObtenerEstadoDePagina(paginaDestino);
            estadoPaginaDestino.Agregar(Sesion.restrictor, datosRestrictor);
            estadoPaginaDestino.Quitar(atGrid.idSeleccionado);
            estadoPaginaDestino.Quitar(atGrid.nombreSeleccionado);
            EntornoSe.Historial.GuardarEstadoDePagina(estadoPaginaDestino);
        }

        public IrAlCrudDeDependencias(parametrosDeEntrada: string): void {
            try {
                let datos: Tipos.DatosParaDependencias = this.PrepararParametrosDeDependencias(this._infoSelector, parametrosDeEntrada);
                if (datos.FiltroRestrictor !== null)
                    ApiRuote.NavegarARelacionar(this, datos.idOpcionDeMenu, datos.idSeleccionado, datos.FiltroRestrictor);
            }
            catch (error) {
                MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, error.message);
                return;
            }
        }

        // permite relacionar un elemento con diferentes entidades
        // parametros de entrada:
        // idOpcionDeMenu --> id de la opción de menú que almacena los parámetros y la acción a someter
        // relacionarCon --> entidad con la que se relaciona
        // PropiedadRestrictora --> propiedad bindeada al control de filtro de la página de destino donde se mapea el restrictor seleccionado en el grid
        public IrAlCrudDeRelacionarCon(parametrosDeEntrada: string): void {
            try {
                let datos: Tipos.DatosParaRelacionar = this.PrepararParametrosDeRelacionarCon(this._infoSelector, parametrosDeEntrada);
                if (datos.FiltroRestrictor !== null)
                    ApiRuote.NavegarARelacionar(this, datos.idOpcionDeMenu, datos.idSeleccionado, datos.FiltroRestrictor);
            }
            catch (error) {
                MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, error.message);
                return;
            }
        }

        private PrepararParametrosDeDependencias(infoSelector: InfoSelector, parametros: string): Tipos.DatosParaDependencias {

            if (infoSelector.Cantidad != 1)
                throw new Error("Debe seleccionar un elemento para poder gestionar sus dependencias");
            let partes = parametros.split('#');
            if (partes.length != 4)
                throw new Error("Los parámetros de dependencias están mal definidos");


            let elemento: Elemento = infoSelector.LeerElemento(0);
            let datos: Tipos.DatosParaDependencias = new Tipos.DatosParaDependencias();
            datos.idOpcionDeMenu = partes[0].split('==')[1];
            datos.DatosDependientes = partes[1].split('==')[1];
            datos.PropiedadQueRestringe = partes[2].split('==')[1];
            datos.PropiedadRestrictora = partes[3].split('==')[1];
            datos.idSeleccionado = elemento.Id;
            datos.MostrarEnElRestrictor = elemento.Texto;

            let valorDeLaColumna = this.obtenerValorDeLaFilaParaLaPropiedad(datos.idSeleccionado, datos.PropiedadQueRestringe);

            if (valorDeLaColumna === null)
                this.LeerElementoParaGestionarSusDependencias(datos);
            else {
                let idRestrictor: number = Numero(valorDeLaColumna);
                let filtro: Tipos.DatosRestrictor = new Tipos.DatosRestrictor(datos.PropiedadRestrictora, idRestrictor, datos.MostrarEnElRestrictor);
                datos.FiltroRestrictor = filtro;
            }
            return datos;
        }

        private PrepararParametrosDeRelacionarCon(infoSelector: InfoSelector, parametros: string): Tipos.DatosParaRelacionar {

            if (infoSelector.Cantidad != 1)
                throw new Error("Debe seleccionar un elemento para poder relacionarlo");
            let partes = parametros.split('#');
            if (partes.length != 4)
                throw new Error("Los parámetros de relación están mal definidos");


            let elemento: Elemento = infoSelector.LeerElemento(0);
            let datos: Tipos.DatosParaRelacionar = new Tipos.DatosParaRelacionar();
            datos.idOpcionDeMenu = partes[0].split('==')[1];
            datos.RelacionarCon = partes[1].split('==')[1];
            datos.PropiedadQueRestringe = partes[2].split('==')[1];
            datos.PropiedadRestrictora = partes[3].split('==')[1];
            datos.idSeleccionado = elemento.Id;
            datos.MostrarEnElRestrictor = elemento.Texto;

            let valorDeLaColumna = this.obtenerValorDeLaFilaParaLaPropiedad(datos.idSeleccionado, datos.PropiedadQueRestringe);

            if (valorDeLaColumna === null)
                this.LeerElementoParaRelacionar(datos);
            else {
                let idRestrictor: number = Numero(valorDeLaColumna);
                let filtro: Tipos.DatosRestrictor = new Tipos.DatosRestrictor(datos.PropiedadRestrictora, idRestrictor, datos.MostrarEnElRestrictor);
                datos.FiltroRestrictor = filtro;
            }
            return datos;
        }

        private LeerElementoParaGestionarSusDependencias(datos: Tipos.DatosParaDependencias) {
            let url: string = `/${this.Controlador}/${Ajax.EndPoint.LeerPorId}?${Ajax.Param.id}=${datos.idSeleccionado}`;
            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EndPoint.LeerPorId
                , datos
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.TrasLeerNavegarParaGestionarSusDependencias
                , null
            );

            a.Ejecutar();
        }

        private TrasLeerNavegarParaGestionarSusDependencias(peticion: ApiDeAjax.DescriptorAjax) {
            let grid: GridDeDatos = peticion.llamador as GridDeDatos;
            let datos: Tipos.DatosParaDependencias = peticion.DatosDeEntrada as Tipos.DatosParaDependencias;
            let idRestrictor: number = Numero(peticion.resultado.datos[datos.PropiedadQueRestringe]);
            let filtro: Tipos.DatosRestrictor = new Tipos.DatosRestrictor(datos.PropiedadRestrictora, idRestrictor, datos.MostrarEnElRestrictor);
            datos.FiltroRestrictor = filtro;
            ApiRuote.NavegarADependientes(grid, datos.idOpcionDeMenu, datos.idSeleccionado, datos.FiltroRestrictor);
        }

        private LeerElementoParaRelacionar(datos: Tipos.DatosParaRelacionar) {
            let url: string = `/${this.Controlador}/${Ajax.EndPoint.LeerPorId}?${Ajax.Param.id}=${datos.idSeleccionado}`;
            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EndPoint.LeerPorId
                , datos
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.TrasLeerNavegarParaRelacionar
                , null
            );

            a.Ejecutar();
        }

        private TrasLeerNavegarParaRelacionar(peticion: ApiDeAjax.DescriptorAjax) {
            let grid: GridDeDatos = peticion.llamador as GridDeDatos;
            let datos: Tipos.DatosParaRelacionar = peticion.DatosDeEntrada as Tipos.DatosParaRelacionar;
            let idRestrictor: number = Numero(peticion.resultado.datos[datos.PropiedadQueRestringe]);
            let filtro: Tipos.DatosRestrictor = new Tipos.DatosRestrictor(datos.PropiedadRestrictora, idRestrictor, datos.MostrarEnElRestrictor);
            datos.FiltroRestrictor = filtro;
            ApiRuote.NavegarARelacionar(grid, datos.idOpcionDeMenu, datos.idSeleccionado, datos.FiltroRestrictor);
        }


        /*
         * 
         * métodos para mapear los registros leidos a un dbgrid 
         * 
         */
        public ObtenerUltimos() {
            let total: number = this.Navegador.Total;
            let cantidad: number = this.Navegador.Cantidad;
            let ultimaPagina: number = Math.ceil(total / cantidad);
            if (ultimaPagina <= 1)
                return;

            let posicion: number = (ultimaPagina - 1) * cantidad;
            if (posicion >= total)
                return;

            let paginaDeDatos = this.DatosDelGrid.Pagina(ultimaPagina + 1);
            if (paginaDeDatos !== null && paginaDeDatos.Posicion === posicion && paginaDeDatos.Cantidad === cantidad) {
                this.ActualizarNavegadorDelGrid(atGrid.accion.ultima, posicion, paginaDeDatos.Registros.length);
                this.MapearPaginaCacheada(this, paginaDeDatos.Registros);
            }
            else
                this.CargarGrid(atGrid.accion.ultima, posicion);
        }

        public ObtenerAnteriores() {
            let cantidad: number = this.Navegador.Cantidad;
            let pagina: number = this.Navegador.Pagina;
            if (pagina == 1)
                return;

            let posicion: number = (pagina - 2) * cantidad;

            if (posicion < 0)
                posicion = 0;

            let paginaDeDatos = this.DatosDelGrid.Pagina(pagina - 1);
            if (paginaDeDatos !== null && paginaDeDatos.Posicion === posicion && paginaDeDatos.Cantidad === cantidad) {
                this.ActualizarNavegadorDelGrid(atGrid.accion.anterior, posicion, paginaDeDatos.Registros.length);
                this.MapearPaginaCacheada(this, paginaDeDatos.Registros);
            }
            else
                this.CargarGrid(atGrid.accion.anterior, posicion);
        }

        public ObtenerSiguientes() {
            let cantidad: number = this.Navegador.Cantidad;
            let pagina: number = this.Navegador.Pagina;
            let total: number = this.Navegador.Total;
            let posicion: number = pagina * cantidad;
            if (posicion >= total)
                return;

            let paginaDeDatos = this.DatosDelGrid.Pagina(pagina + 1);
            if (paginaDeDatos !== null && paginaDeDatos.Posicion === posicion && paginaDeDatos.Cantidad === cantidad) {
                this.ActualizarNavegadorDelGrid(atGrid.accion.siguiente, posicion, paginaDeDatos.Registros.length);
                this.MapearPaginaCacheada(this, paginaDeDatos.Registros);
            }
            else
                this.CargarGrid(atGrid.accion.siguiente, posicion);
        }

        protected CargarGrid(accion: string, posicion: number) {

            if (this.Grid.getAttribute(atGrid.cargando) == 'S')
                return;

            let url: string = this.DefinirPeticionDeBusqueda(Ajax.EndPoint.LeerDatosParaElGrid, accion, posicion);
            var datosDePeticion = new DatosPeticionNavegarGrid(this, accion, posicion);
            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EndPoint.LeerDatosParaElGrid
                , datosDePeticion
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.CrearFilasEnElGrid
                , this.SiHayErrorAlCargarElGrid
            );
            this.Grid.setAttribute(atGrid.cargando, 'S');
            a.Ejecutar();
        }


        protected PromesaDeCargarGrid(accion: string, posicion: number): Promise<boolean> {

            if (this.Grid.getAttribute(atGrid.cargando) == 'S')
                return null;

            let promesa: Promise<boolean> = new Promise((resolve, reject) => {
                let url: string = this.DefinirPeticionDeBusqueda(Ajax.EndPoint.LeerDatosParaElGrid, accion, posicion);
                var datosDePeticion = new DatosPeticionNavegarGrid(this, accion, posicion);
                let a = new ApiDeAjax.DescriptorAjax(this
                    , Ajax.EndPoint.LeerDatosParaElGrid
                    , datosDePeticion
                    , url
                    , ApiDeAjax.TipoPeticion.Asincrona
                    , ApiDeAjax.ModoPeticion.Get
                    , (peticion) => {
                        resolve(this.CrearFilasEnElGrid(peticion));
                    }
                    , (peticion) => {
                        this.SiHayErrorAlCargarElGrid(peticion);
                        reject(false);
                    }
                );
                this.Grid.setAttribute(atGrid.cargando, 'S');
                a.Ejecutar();
            });

            return promesa;
        }

        private DefinirPeticionDeBusqueda(endPoint: string, accion: string, posicion: number): string {
            var posicion = posicion;
            var cantidad = this.Navegador.Cantidad;
            var controlador = this.Navegador.Controlador;
            var filtroJson = this.ObtenerFiltros();
            var ordenJson = this.ObtenerOrdenacion();

            let url: string = `/${controlador}/${endPoint}`;
            let parametros: string = `${Ajax.Param.modo}=Mantenimiento` +
                `&${Ajax.Param.accion}=${accion}` +
                `&${Ajax.Param.posicion}=${posicion}` +
                `&${Ajax.Param.cantidad}=${cantidad}` +
                `&${Ajax.Param.filtro}=${filtroJson}` +
                `&${Ajax.Param.orden}=${ordenJson}`;
            let peticion: string = url + '?' + parametros;
            return peticion;
        }

        private SiHayErrorAlCargarElGrid(peticion: ApiDeAjax.DescriptorAjax) {
            let grid: GridDeDatos = peticion.llamador as GridDeDatos;
            try {
                MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, peticion.resultado.mensaje);
            }
            finally {
                grid.Grid.setAttribute(atGrid.cargando, 'N');
            }
        }

        private CrearFilasEnElGrid(peticion: ApiDeAjax.DescriptorAjax): boolean {
            let datosDeEntrada: DatosPeticionNavegarGrid = (peticion.DatosDeEntrada as DatosPeticionNavegarGrid);
            let grid: GridDeDatos = datosDeEntrada.Grid;
            let lineasCreadas: boolean = true;
            try {
                let infoObtenida: ResultadoDeLectura = peticion.resultado.datos as ResultadoDeLectura;
                var registros = infoObtenida.registros;
                if (datosDeEntrada.Accion === atGrid.accion.buscar)
                    grid.Navegador.Total = infoObtenida.total;

                grid.ActualizarNavegadorDelGrid(datosDeEntrada.Accion, datosDeEntrada.PosicionDesdeLaQueSeLee, registros.length);
                let expresionMostrar: string = grid.Grid.getAttribute(atControl.expresionElemento).toLowerCase();
                grid.DatosDelGrid.AnadirPagina(grid.Navegador.Pagina, datosDeEntrada.PosicionDesdeLaQueSeLee, grid.Navegador.Cantidad, infoObtenida.registros, expresionMostrar);
                grid.MapearPaginaCacheada(grid, registros);
                ApiGrid.RecalcularAnchoColumnas(grid.Tabla);
            }
            catch (error) {
                lineasCreadas = false;
                MensajesSe.Error("CrearFilasEnElGrid", `Error al crear las filas en el grid`, error.message);
            }
            finally {
                grid.Grid.setAttribute(atGrid.cargando, 'N');
                if (!grid.EsCrud && lineasCreadas) {
                    grid.Modal.style.display = 'block';
                    EntornoSe.AjustarModalesAbiertas();
                }
            }
            return lineasCreadas;
        }

        protected MapearPaginaCacheada(grid: GridDeDatos, registros: Elemento[]): void {
            var cuerpo = grid.CrearCuerpoDeLaTabla(grid, registros);
            grid.AnadirCuerpoALaTabla(grid, cuerpo);
            grid.ActualizarInformacionDelGrid(grid);
            grid.AjustarTamanoDelCuerpoDeLaTabla(grid, cuerpo);
            grid.AplicarQueFilasMostrar(grid.InputSeleccionadas, grid.CuerpoTablaGrid, grid.InfoSelector);

        }

        private CrearCuerpoDeLaTabla(grid: GridDeDatos, registros: any) {
            let filaCabecera: ApiGrid.PropiedadesDeLaFila[] = ApiGrid.obtenerDescriptorDeLaCabecera(grid.Tabla);
            let cuerpoDeLaTabla: HTMLTableSectionElement = document.createElement("tbody");

            cuerpoDeLaTabla.id = `${grid.Grid.id}_tbody`;
            cuerpoDeLaTabla.classList.add(ClaseCss.cuerpoDeLaTabla);
            for (let i = 0; i < registros.length; i++) {
                let fila: HTMLTableRowElement = grid.crearFila(filaCabecera, registros[i], i);
                cuerpoDeLaTabla.append(fila);
            }
            return cuerpoDeLaTabla;
        }

        private AnadirCuerpoALaTabla(grid: GridDeDatos, cuerpoDeLaTabla: HTMLTableSectionElement) {
            let tabla: HTMLTableElement = grid.Grid.querySelector("table");
            let tbody: HTMLTableSectionElement = tabla.querySelector("tbody");
            if (!(tbody === null || tbody === undefined))
                tabla.removeChild(tbody);
            tabla.append(cuerpoDeLaTabla);
            grid.DatosDelGrid.PaginaActual = grid.Navegador.Pagina;
        }

        private AjustarTamanoDelCuerpoDeLaTabla(grid: GridDeDatos, cuerpoDeLaTabla: HTMLTableSectionElement) {
            if (grid.EsCrud) {
                let posicion: number = grid.PosicionGrid();
                let altura: number = grid.AlturaDelGrid(posicion);
                grid.FijarAlturaCuerpoDeLaTabla(altura);
            }
            else {
                cuerpoDeLaTabla.style.height = `${AlturaFormulario() / 3}px`;
            }
        }

        private crearFila(filaCabecera: ApiGrid.PropiedadesDeLaFila[], registro: any, numeroDeFila: number): HTMLTableRowElement {
            let fila = document.createElement("tr");
            fila.id = `${this.IdGrid}_d_tr_${numeroDeFila}`;
            fila.classList.add(ClaseCss.filaDelGrid);
            let idDelElemento: number = 0;
            for (let j = 0; j < filaCabecera.length; j++) {

                let columnaCabecera: ApiGrid.PropiedadesDeLaFila = filaCabecera[j];
                let valor: any = this.BuscarValorDeColumnaRegistro(registro, columnaCabecera.propiedad);
                if (columnaCabecera.propiedad === atControl.id) {
                    if (!IsNumber(valor))
                        throw new Error("El id del elemento leido debe ser numérico");
                    idDelElemento = Numero(valor);
                }

                let celdaDelTd: HTMLTableCellElement = this.crearCelda(fila, columnaCabecera, j, valor);
                fila.append(celdaDelTd);
            }

            fila.setAttribute(atControl.valorTr, idDelElemento.toString());
            return fila;
        }

        private crearCelda(fila: HTMLTableRowElement, columnaCabecera: ApiGrid.PropiedadesDeLaFila, numeroDeCelda: number, valor: string): HTMLTableCellElement {
            let celdaDelTd: HTMLTableCellElement = document.createElement("td");
            celdaDelTd.id = `${fila.id}.${numeroDeCelda}`;
            celdaDelTd.headers = `${columnaCabecera.id}`;
            celdaDelTd.setAttribute(atControl.nombre, `td.${columnaCabecera.propiedad}.${this.IdGrid}`);
            celdaDelTd.setAttribute(atControl.propiedad, `${columnaCabecera.propiedad}`);
            celdaDelTd.style.textAlign = columnaCabecera.estilo.textAlign;
            celdaDelTd.style.width = columnaCabecera.estilo.width;

            let idCheckDeSeleccion: string = `${fila.id}.chksel`;
            let eventoOnClick: string = this.definirPulsarCheck(idCheckDeSeleccion, celdaDelTd.id);
            celdaDelTd.setAttribute(atControl.eventoJs.onclick, eventoOnClick);

            if (columnaCabecera.claseCss === ClaseCss.columnaOculta) {
                celdaDelTd.classList.add(ClaseCss.columnaOculta);
            }

            if (columnaCabecera.propiedad === 'chksel')
                this.insertarCheckEnElTd(fila.id, celdaDelTd, columnaCabecera.propiedad);
            else {
                this.insertarInputEnElTd(fila.id, columnaCabecera, celdaDelTd, valor);
            }
            return celdaDelTd;
        }

        private definirPulsarCheck(idCheckDeSeleccion: string, idControlHtml: string): string {
            let a: string = '';
            if (this.EsModalDeSeleccion) {
                let idModal: string = this.Grid.getAttribute(atSelector.idModal);
                a = `${GestorDeEventos.deSeleccion}('fila-pulsada', '${idModal}#${idCheckDeSeleccion}#${idControlHtml}');`;
            }
            else if (this.EsModalParaRelacionar) {
                let idModal: string = this.Grid.getAttribute(atSelector.idModal);
                a = `${GestorDeEventos.deCrearRelaciones}('fila-pulsada', '${idModal}#${idCheckDeSeleccion}#${idControlHtml}');`;
            }
            else {
                if (this.EsModalParaConsultarRelaciones) {
                    let idModal: string = this.Grid.getAttribute(atSelector.idModal);
                    a = `${GestorDeEventos.deConsultaDeRelaciones}('fila-pulsada', '${idModal}#${idCheckDeSeleccion}#${idControlHtml}');`;
                }
                else
                    if (this.EsCrud)
                        a = `${GestorDeEventos.delMantenimiento}('fila-pulsada', '${idCheckDeSeleccion}#${idControlHtml}');`;
                    else
                        throw new Error("No se ha definido el gestor de eventos a asociar a la pulsación de una fila en el grid");
            }
            return a;
        }

        private insertarInputEnElTd(idFila: string, columnaCabecera: ApiGrid.PropiedadesDeLaFila, celdaDelTd: HTMLTableCellElement, valor: string) {
            let input = document.createElement("input");
            input.type = "text";
            input.id = `${idFila}.${columnaCabecera.propiedad}`;
            input.name = `${columnaCabecera.propiedad}.${this.IdGrid}`;
            input.setAttribute(atControl.propiedad, columnaCabecera.propiedad);

            input.style.border = "0px";
            input.style.textAlign = columnaCabecera.estilo.textAlign;
            input.style.width = "100%";
            input.style.backgroundColor = "inherit";

            let idCheckBox = `${idFila}.chksel`;
            let eventoOnClick: string = this.definirPulsarCheck(idCheckBox, input.id);
            celdaDelTd.setAttribute(atControl.eventoJs.onclick, eventoOnClick);

            input.readOnly = true;
            input.hidden = celdaDelTd.hidden;
            input.value = valor;

            celdaDelTd.append(input);

        }

        private insertarCheckEnElTd(idFila: string, celdaDelTd: HTMLTableCellElement, propiedad: string) {
            let checkbox: HTMLInputElement = document.createElement('input');
            checkbox.type = "checkbox";
            checkbox.id = `${idFila}.${propiedad}`;
            checkbox.name = `${propiedad}.${this.IdGrid}`;
            checkbox.setAttribute(atControl.propiedad, `${propiedad}`);

            checkbox.style.border = "0px";
            checkbox.style.textAlign = "center";
            checkbox.style.width = "100%";
            checkbox.style.backgroundColor = "inherit";

            let eventoOnClick: string = this.definirPulsarCheck(checkbox.id, checkbox.id);
            celdaDelTd.setAttribute(atControl.eventoJs.onclick, eventoOnClick);

            checkbox.value = literal.false;
            celdaDelTd.append(checkbox);
        }

        private obtenerDescriptorDeLaCabecera(grid: GridDeDatos): Array<ApiGrid.PropiedadesDeLaFila> {
            let filaCabecera: Array<ApiGrid.PropiedadesDeLaFila> = new Array<ApiGrid.PropiedadesDeLaFila>();
            var cabecera = grid.Tabla.rows[0];
            var ths = cabecera.querySelectorAll('th');
            for (let i = 0; i < ths.length; i++) {
                let p: ApiGrid.PropiedadesDeLaFila = new ApiGrid.PropiedadesDeLaFila();
                p.id = ths[i].id;
                p.visible = !ths[i].hidden;
                p.claseCss = ths[i].className;
                p.estilo = ths[i].style;
                p.anchoEnPixel = ths[i].getBoundingClientRect().width;
                p.editable = false;
                p.propiedad = ths[i].getAttribute('propiedad');
                filaCabecera.push(p);
            }
            return filaCabecera;
        }

        private BuscarValorDeColumnaRegistro(registro, propiedadDeLaFila: string): any {
            for (const propiedad in registro) {
                if (propiedad.toLowerCase() === propiedadDeLaFila)
                    return registro[propiedad];
            }
            return "";
        }

        public FilaPulsada(idCheck: string, idDelInput: string) {

            let check: HTMLInputElement = document.getElementById(idCheck) as HTMLInputElement;
            //Se hace porque antes ha pasado por aquí por haber pulsado en la fila
            if (idCheck !== idDelInput) {
                check.checked = !check.checked;
            }

            let id: number = this.ObtenerElIdDelElementoDelaFila(idCheck);
            if (check.checked) {
                let elemento: Elemento = this.DatosDelGrid.Obtener(id);
                this.AnadirAlInfoSelector(this, elemento);
            }
            else {
                this.QuitarDelSelector(this, id);
                for (let i: number = 0; i < this.InfoSelector.Cantidad; i++) {
                    let e: Elemento = this.InfoSelector.LeerElemento(i);
                    this.AplicarModoAccesoAlElemento(e);
                }

                if (this.InfoSelector.Cantidad === 0 && (this instanceof ModalConGrid) === false)
                    this.DeshabilitarOpcionesDeMenuDeElemento();
            }

        }

        public AplicarModoAccesoAlElemento(elemento: Elemento): void {
            let modoAcceso: ModoAcceso.enumModoDeAccesoDeDatos = ModoAcceso.Parsear(elemento.ModoDeAcceso);

            //En las modales no hay menús
            if (this.ZonaDeMenu === null)
                return;

            let opcionesDeElemento: NodeListOf<HTMLButtonElement> = this.OpcionesPorElemento;
            let hacerLaInterseccion: boolean = this.InfoSelector.Cantidad > 1;
            for (var i = 0; i < opcionesDeElemento.length; i++) {
                let opcion: HTMLButtonElement = opcionesDeElemento[i];
                ModoAcceso.AplicarModoAccesoAlElemento(opcion, hacerLaInterseccion,modoAcceso);
            }
        }

        protected LeerElementoSeleccionado(id: number): void {
            let url: string = `/${this.Controlador}/${Ajax.EndPoint.LeerPorId}?${Ajax.Param.id}=${id}`;
            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EndPoint.LeerPorId
                , id
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.TrasLeerElementoSeleccionado
                , null
            );

            a.Ejecutar();
        }

        private TrasLeerElementoSeleccionado(peticion: ApiDeAjax.DescriptorAjax) {
            let grid: GridDeDatos = peticion.llamador as GridDeDatos;
            grid.AnadirAlInfoSelector(grid, peticion.resultado.datos);
        }

        protected DeshabilitarOpcionesDeMenuDeNegocio() {
            let opcionesDeElemento: NodeListOf<HTMLButtonElement> = this.ZonaDeMenu.querySelectorAll(`input[${atOpcionDeMenu.clase}="${ClaseDeOpcioDeMenu.DeVista}"]`) as NodeListOf<HTMLButtonElement>;
            for (var i = 0; i < opcionesDeElemento.length; i++) {
                let opcion: HTMLButtonElement = opcionesDeElemento[i];
                opcion.disabled = true;
            }
            this.DeshabilitarOpcionesDeMenuDeElemento();
        }

        protected DeshabilitarOpcionesDeMenuDeElemento() {
            let opcionesDeElemento: NodeListOf<HTMLButtonElement> = this.ZonaDeMenu.querySelectorAll(`input[${atOpcionDeMenu.clase}="${ClaseDeOpcioDeMenu.DeElemento}"]`) as NodeListOf<HTMLButtonElement>;
            for (var i = 0; i < opcionesDeElemento.length; i++) {
                let opcion: HTMLButtonElement = opcionesDeElemento[i];
                opcion.disabled = true;
            }
        }

        protected AccederAlModoDeAccesoAlElemento(id: number): void {
            let url: string = this.DefinirPeticionDeLeerModoDeAccesoAlElemento(id);
            let datosDeEntrada = `{"Negocio":"${this.Negocio}","id":"${id}"}`;
            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EndPoint.LeerModoDeAccesoAlElemento
                , datosDeEntrada
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.AplicarModoDeAccesoAlElemento
                , this.SiHayErrorTrasPeticionAjax
            );

            a.Ejecutar();
        }

        private AplicarModoDeAccesoAlElemento(peticion: ApiDeAjax.DescriptorAjax) {
            let mantenimiento: CrudMnt = peticion.llamador as CrudMnt;
            let modoDeAccesoDelUsuario: string = peticion.resultado.modoDeAcceso;
        }


        private DefinirPeticionDeLeerModoDeAccesoAlElemento(id: number): string {
            let url: string = `/${this.Controlador}/${Ajax.EndPoint.LeerModoDeAccesoAlElemento}`;
            let parametros: string = `${Ajax.Param.negocio}=${this.Negocio}&${Ajax.Param.id}=${id}`;
            let peticion: string = url + '?' + parametros;
            return peticion;
        }

        public OrdenarPor(columna: string): void {
            this.EstablecerOrdenacion(columna);
            this.DatosDelGrid.InicializarCache();
            this.CargarGrid(atGrid.accion.buscar, 0);
        }

        public MostrarSoloSeleccionadas(inputDeSeleccionadas: HTMLInputElement, etiquetaSeleccionadas: HTMLElement, tbodyDelGrid: HTMLTableSectionElement, seleccionadas: InfoSelector): void {
            if (NumeroMayorDeCero(inputDeSeleccionadas.value)) {
                inputDeSeleccionadas.value = "0";
                etiquetaSeleccionadas.innerText = "Seleccionadas";
            }
            else {
                inputDeSeleccionadas.value = "1";
                etiquetaSeleccionadas.innerText = "Todas las filas";
            }
            this.AplicarQueFilasMostrar(inputDeSeleccionadas, tbodyDelGrid, seleccionadas);
        }

        public TeclaPulsada(grid: GridDeDatos, e): void {
            if (e.keyCode === 13 && !e.shiftKey) {
                grid.CargarGrid(atGrid.accion.buscar, 0);
                e.preventDefault();
            }
        }

        public AplicarQueFilasMostrar(inputDeSeleccionadas: HTMLInputElement, tbodyDelGrid: HTMLTableSectionElement, seleccionadas: InfoSelector): void {
            if (NumeroMayorDeCero(inputDeSeleccionadas.value)) {
                this.MostrarFilasSeleccionadas(tbodyDelGrid, seleccionadas);
            }
            else {
                this.MostrarTodasLasFilas(tbodyDelGrid);
            }
        }

        private MostrarTodasLasFilas(tbodyDelGrid: HTMLTableSectionElement): void {
            let trs: NodeListOf<HTMLTableRowElement> = tbodyDelGrid.querySelectorAll("tr") as NodeListOf<HTMLTableRowElement>;
            let i: number = 0;
            for (i = 0; i < trs.length; i++) {
                let tr: HTMLTableRowElement = trs[i];
                tr.hidden = false;
            }

        }

        private MostrarFilasSeleccionadas(tbodyDelGrid: HTMLTableSectionElement, seleccionadas: InfoSelector): void {
            let trs: NodeListOf<HTMLTableRowElement> = tbodyDelGrid.querySelectorAll("tr") as NodeListOf<HTMLTableRowElement>;
            let i: number = 0;
            for (i = 0; i < trs.length; i++) {
                let tr: HTMLTableRowElement = trs[i];
                let idDelElemento = Numero(tr.getAttribute(atControl.valorTr));
                tr.hidden = !seleccionadas.Contiene(idDelElemento);
            }
        }
    }

}