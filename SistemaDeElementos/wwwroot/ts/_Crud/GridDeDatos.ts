namespace Crud {

    class ClausulaDeOrdenacion {
        criterio: string;
        modo: string;

        constructor(propiedad: string, modo: string) {
            this.criterio = propiedad;
            this.modo = modo;
        }
    }

    class Orden {
        public IdColumna: string;
        public Propiedad: string;
        public Modo: string;
        private _cssClase: string;

        get ccsClase(): string {
            return this._cssClase;
        }

        set ccsClase(modo: string) {
            if (modo === ModoOrdenacion.ascedente)
                this._cssClase = ClaseCss.ordenAscendente;
            else if (modo === ModoOrdenacion.descendente)
                this._cssClase = ClaseCss.ordenDescendente;
            else if (modo === ModoOrdenacion.sinOrden)
                this._cssClase = ClaseCss.sinOrden;
        }

        constructor(idcolumna: string, propiedad: string, modo: string) {
            this.Modo = modo;
            this.Propiedad = propiedad;
            this.IdColumna = idcolumna;
            this.ccsClase = modo;
        }
    }

    class Ordenacion {
        private lista: Array<Orden>;

        public Count(): number {
            return this.lista.length;
        }

        constructor() {
            this.lista = new Array<Orden>();
        }

        private Anadir(idcolumna: string, propiedad: string, modo: string) {
            for (let i = 0; i < this.lista.length; i++) {
                if (this.lista[i].Propiedad === propiedad) {
                    this.lista[i].Modo = modo;
                    this.lista[i].ccsClase = modo;
                    return;
                }
            }
            let orden: Orden = new Orden(idcolumna, propiedad, modo);
            this.lista.push(orden);
        }

        private Quitar(propiedad: string) {
            for (let i = 0; i < this.lista.length; i++) {
                if (this.lista[i].Propiedad == propiedad) {
                    this.lista.splice(i, 1);
                    return;
                }
            }
        }

        public Actualizar(idcolumna: string, propiedad: string, modo: string) {
            if (modo === ModoOrdenacion.sinOrden)
                this.Quitar(propiedad);
            else
                this.Anadir(idcolumna, propiedad, modo);
        }

        public Leer(i: number): Orden {
            return this.lista[i];
        }
    }


    class PropiedadesDeLaFila {
        id: string;
        propiedad: string;
        visible: boolean;
        estilo: CSSStyleDeclaration;
        claseCss: string;
        editable: boolean;
        tipo: string;
        anchoEnPixel: number;
        constructor() {

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
            this.ActualizarMensaje(seleccionados);
        }

        public ActualizarMensaje(seleccionados: number): void {
            this.Mensaje = `Seleccionados ${seleccionados} de ${this.Total}`;
        }
    }

    export class GridDeDatos extends CrudBase {

        protected Ordenacion: Ordenacion;
        protected Navegador: Navegador;

        private _infoSelector: InfoSelector;
        public get InfoSelector(): InfoSelector {
            return this._infoSelector;
        }

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


        private _idPanelMnt: string;
        public get IdPanelMnt(): string {
            return this._idPanelMnt;
        }
        public get PanelMnt(): HTMLDivElement {
            return document.getElementById(this._idPanelMnt) as HTMLDivElement;
        }

        public get Controlador() {
            return this.PanelMnt.getAttribute(atMantenimniento.controlador);
        }

        public get Negocio() {
            return this.PanelMnt.getAttribute(atMantenimniento.negocio);
        }

        private _idHtmlFiltro: string;
        public get ZonaDeFiltro(): HTMLDivElement {
            return document.getElementById(this._idHtmlFiltro) as HTMLDivElement;
        }
        public get EtiquetaMostrarOcultarFiltro(): HTMLElement {
            return document.getElementById(`mostrar.${this.IdPanelMnt}.ref`) as HTMLElement;
        }
        public get ExpandirFiltro(): HTMLInputElement {
            return document.getElementById(`expandir.${this.IdPanelMnt}`) as HTMLInputElement;
        }
        public get SoloSeleccionadas(): HTMLInputElement {
            return document.getElementById(`seleccion.${this.IdGrid}`) as HTMLInputElement;
        }
        public get EtiquetaMostrarSeleccionadas(): HTMLElement {
            return document.getElementById(`seleccion.${this.IdGrid}.ref`) as HTMLElement;
        }

        protected get Grid(): HTMLDivElement {
            return document.getElementById(this.IdGrid) as HTMLDivElement;
        }

        protected get CabeceraTablaGrid(): HTMLTableSectionElement {
            let idCabecera = this.Grid.getAttribute(atGrid.cabeceraTabla);
            return document.getElementById(idCabecera) as HTMLTableSectionElement;
        }

        protected get CuerpoTablaGrid(): HTMLTableSectionElement {
            return document.getElementById(`${this.Grid.id}_tbody`) as HTMLTableSectionElement;
        }

        protected get ZonaNavegador(): HTMLDivElement {
            let idNavegador = this.Grid.getAttribute(atGrid.zonaNavegador);
            return document.getElementById(idNavegador) as HTMLDivElement;
        }

        protected get Tabla(): HTMLTableElement {
            let idTabla: string = this.Grid.getAttribute(atControl.tablaDeDatos);
            return document.getElementById(idTabla) as HTMLTableElement;
        }

        private _idHtmlZonaMenu: string;
        public get ZonaDeMenu(): HTMLDivElement {
            return document.getElementById(this._idHtmlZonaMenu) as HTMLDivElement;
        }

        constructor(idPanelMnt: string) {
            super();

            this._idPanelMnt = idPanelMnt;
            this._idGrid = this.PanelMnt.getAttribute(atMantenimniento.gridDelMnt);
            this._idHtmlZonaMenu = this.PanelMnt.getAttribute(atMantenimniento.zonaMenu);
            this._idHtmlFiltro = this.Grid.getAttribute(atMantenimniento.zonaDeFiltro);

            this._infoSelector = new InfoSelector(this.IdGrid);
            this.Navegador = new Navegador(this.IdGrid);
            this.Ordenacion = new Ordenacion();
        }

        public Inicializar(idPanelMnt: string) {
            super.Inicializar(idPanelMnt);
            this.InicializarNavegador();
        }

        private InicializarNavegador() {
            this.Navegador.RestaurarDatos(this.Estado.Obtener(atGrid.id));
            for (var i = 0; i < this.Ordenacion.Count(); i++) {
                let orden: Orden = this.Ordenacion.Leer(i);
                let columna: HTMLTableHeaderCellElement = document.getElementById(orden.IdColumna) as HTMLTableHeaderCellElement;
                columna.setAttribute(atControl.modoOrdenacion, orden.Modo);
                let a: HTMLElement = columna.getElementsByTagName('a')[0] as HTMLElement;
                a.setAttribute("class", orden.ccsClase);
            }
        }

        public PosicionGrid(): number {
            let alturaCabeceraPnlControl: number = AlturaCabeceraPnlControl();
            let alturaCabeceraMnt: number = this.PanelMnt.getBoundingClientRect().height;
            let alturaFiltro: number = 0;
            if (NumeroMayorDeCero(this.ExpandirFiltro.value)) {
                alturaFiltro = this.ZonaDeFiltro.getBoundingClientRect().height;
            }
            return alturaCabeceraPnlControl + alturaCabeceraMnt + alturaFiltro;
        }

        public AlturaDelGrid(posicionGrid: number): number {
            let alturaPiePnlControl: number = AlturaPiePnlControl();
            let alturaZonaNavegador: number = this.ZonaNavegador.getBoundingClientRect().height ;
            return AlturaFormulario() - posicionGrid - alturaPiePnlControl - alturaZonaNavegador;
        }

        public FijarAlturaCuerpoDeLaTabla(alturaDelGrid: number): void {
            let alturaCabecera = this.CabeceraTablaGrid.getBoundingClientRect().height;
            this.CuerpoTablaGrid.style.height = `${alturaDelGrid - alturaCabecera}px`;
        }

        protected ActualizarNavegadorDelGrid(accion: string, posicionDesdeLaQueSeLeyo: number, registrosLeidos: number) {

            this.Navegador.Actualizar(accion, posicionDesdeLaQueSeLeyo, registrosLeidos, this.InfoSelector.Cantidad);

            for (var i = 0; i < this.Ordenacion.Count(); i++) {
                let orden: Orden = this.Ordenacion.Leer(i);
                let columna: HTMLTableHeaderCellElement = document.getElementById(orden.IdColumna) as HTMLTableHeaderCellElement;
                columna.setAttribute(atControl.modoOrdenacion, orden.Modo);
                let a: HTMLElement = columna.getElementsByTagName('a')[0] as HTMLElement;
                a.setAttribute("class", orden.ccsClase);
            }
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
            this.Ordenacion.Actualizar(idcolumna, propiedad, modo);

            htmlColumna.setAttribute(atControl.modoOrdenacion, modo);

        }

        protected ObtenerExpresionMostrar(idCheck: string): string {
            let expresion: string = this.Grid.getAttribute(atControl.expresionElemento).toLowerCase();

            if (!IsNullOrEmpty(expresion)) {
                let fila: HTMLTableRowElement = this.ObtenerlaFila(idCheck);
                let columnas: HTMLCollectionOf<HTMLTableCellElement> = fila.getElementsByTagName('td') as HTMLCollectionOf<HTMLTableCellElement>;
                for (let j = 0; j < columnas.length; j++) {
                    let input: HTMLInputElement = columnas[j].getElementsByTagName('input')[0] as HTMLInputElement;
                    if (input !== undefined) {
                        let propiedad: string = input.getAttribute(atControl.propiedad).toLowerCase();
                        if (!IsNullOrEmpty(propiedad) && expresion.includes(`[${propiedad}]`)) {
                            expresion = expresion.replace(`[${propiedad}]`, input.value);
                        }
                    }
                }
            }

            if (expresion === '[nombre]')
                throw new Error('No se ha definido la expresión del elemento');

            return expresion;
        }

        protected ObtenerOrdenacion() {
            var clausulas = new Array<ClausulaDeOrdenacion>();
            for (var i = 0; i < this.Ordenacion.Count(); i++) {
                let orden = this.Ordenacion.Leer(i);
                clausulas.push(new ClausulaDeOrdenacion(orden.Propiedad, orden.Modo));
            }
            return JSON.stringify(clausulas);
        }

        protected ObtenerFiltros(): string {
            let arrayIds: Array<string> = this.ObtenerControlesDeFiltro();
            var clausulas = new Array<ClausulaDeFiltrado>();
            for (let id of arrayIds) {
                var clausula: ClausulaDeFiltrado = null;
                var control: HTMLElement = document.getElementById(`${id}`);
                var tipo: string = control.getAttribute(TipoControl.Tipo);

                switch (tipo) {
                    case TipoControl.restrictorDeFiltro: {
                        clausula = this.ObtenerClausulaRestrictor(control as HTMLInputElement);;
                        break;
                    }
                    case TipoControl.Editor: {
                        clausula = this.ObtenerClausulaEditor(control as HTMLInputElement);;
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
                    default: {
                        Mensaje(TipoMensaje.Error, `No está implementado como definir la cláusula de filtrado de un tipo ${TipoControl}`);
                    }
                }

                if (clausula !== null)
                    clausulas.push(clausula);
            }

            this.FiltrosExcluyentes(clausulas);

            return JSON.stringify(clausulas);
        }

        protected FiltrosExcluyentes(clausulas: ClausulaDeFiltrado[]) {
            return clausulas;
        }

        private ObtenerControlesDeFiltro(): Array<string> {

            var arrayIds = new Array<string>();
            var arrayHtmlInput = this.ZonaDeFiltro.getElementsByTagName(TagName.input);

            for (let i = 0; i < arrayHtmlInput.length; i++) {
                var htmlInput = arrayHtmlInput[i];
                var esFiltro = htmlInput.getAttribute(atControl.filtro);
                if (esFiltro === 'S') {
                    var id = htmlInput.getAttribute(atControl.id);
                    if (id === null)
                        console.log(`Falta el atributo id del componente de filtro ${htmlInput}`);
                    else
                        arrayIds.push(id);
                }
            }

            var arrayHtmlSelect = this.ZonaDeFiltro.getElementsByTagName(TagName.select);
            for (let i = 0; i < arrayHtmlSelect.length; i++) {
                var htmlSelect = arrayHtmlSelect[i];
                var id = htmlSelect.getAttribute(atControl.id);
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
            var criterio = input.getAttribute(atControl.criterio);

            let lista: ListaDinamica = new ListaDinamica(input);
            let valor: number = lista.BuscarSeleccionado(input.value);


            var clausula = null;
            if (Number(valor) > 0) {
                clausula = new ClausulaDeFiltrado(propiedad, criterio, valor.toString());
            }
            return clausula;
        }

        private ObtenerClausulaListaDeELemento(selet: HTMLSelectElement): ClausulaDeFiltrado {
            var propiedad = selet.getAttribute(atControl.propiedad);
            var criterio = selet.getAttribute(atControl.criterio);
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

        protected AnadirAlInfoSelector(infoSelector: InfoSelector, idCheck: string, expresionElemento: string): number {
            let id: number = this.ObtenerElIdDelElementoDelaFila(idCheck);
            infoSelector.InsertarElemento(id, expresionElemento);
            return id;
        }

        protected QuitarDelSelector(infoSelector: InfoSelector, idCheck: string): void {
            let id: number = this.ObtenerElIdDelElementoDelaFila(idCheck);
            infoSelector.Quitar(id);
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
            for (var i = 0; i < this.InfoSelector.Cantidad; i++) {
                for (var j = 0; j < len; j++) {
                    let id: number = this.InfoSelector.LeerId(i);
                    if (Numero((<HTMLInputElement>celdasId[j]).value) == id) {
                        var idCheck = celdasId[j].id.replace(`.${atControl.id}`, LiteralMnt.postfijoDeCheckDeSeleccion);
                        var check = document.getElementById(idCheck);
                        (<HTMLInputElement>check).checked = true;
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

        protected ActualizarInformacionDelGrid(contenedorGrid: GridDeDatos, accion: string, posicionDesdeLaQueSeLeyo: number, registrosLeidos: number) {
            contenedorGrid.ActualizarNavegadorDelGrid(accion, posicionDesdeLaQueSeLeyo, registrosLeidos);

            if (!this.EsModalConGrid && this.Estado.Contiene(atGrid.idSeleccionado)) {
                let idSeleccionado: number = this.Estado.Obtener(atGrid.idSeleccionado);
                let nombreSeleccionado: string = this.Estado.Obtener(atGrid.nombreSeleccionado);
                this.InfoSelector.InsertarElemento(idSeleccionado, nombreSeleccionado);
            }

            contenedorGrid.MarcarElementos();
            contenedorGrid.InfoSelector.SincronizarCheck();
        }

        protected obtenerValorDeLaFilaParaLaPropiedad(id: number, propiedad: string): string {

            let fila: HTMLTableRowElement = this.ObtenerFila(id);
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
            throw new Error(`No se ha localizado una fila con la propiedad Id definida`);
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

            let datosRestrictor: DatosRestrictor = valores.Obtener(Sesion.restrictor) as DatosRestrictor;
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

        // permite relacionar un elemento con diferentes entidades
        // parametros de entrada:
        // idOpcionDeMenu --> id de la opción de menú que almacena los parámetros y la acción a someter
        // relacionarCon --> entidad con la que se relaciona
        // PropiedadRestrictora --> propiedad bindeada al control de filtro de la página de destino donde se mapea el restrictor seleccionado en el grid
        public RelacionarCon(parametrosDeEntrada: string): void {
            try {
                let datos: Crud.DatosParaRelacionar = this.PrepararParametrosDeRelacionarCon(this._infoSelector, parametrosDeEntrada);
                super.NavegarARelacionar(datos.idOpcionDeMenu, datos.idSeleccionado, datos.FiltroRestrictor);
            }
            catch (error) {
                Mensaje(TipoMensaje.Error, error);
                return;
            }
        }

        private PrepararParametrosDeRelacionarCon(infoSelector: InfoSelector, parametros: string): DatosParaRelacionar {

            if (infoSelector.Cantidad != 1)
                throw new Error("Debe seleccionar un elemento para poder relacionarlo");

            let datos: DatosParaRelacionar = new DatosParaRelacionar();

            let partes = parametros.split('#');

            if (partes.length != 4)
                throw new Error("Los parámetros de relación están mal definidos");

            datos.idOpcionDeMenu = partes[0].split('==')[1];
            datos.RelacionarCon = partes[1].split('==')[1];
            let PropiedadQueRestringe: string = partes[2].split('==')[1];
            let PropiedadRestrictora: string = partes[3].split('==')[1];
            let idSeleccionado: number = infoSelector.LeerElemento(0).Id;
            let valorDeLaColumna = this.obtenerValorDeLaFilaParaLaPropiedad(idSeleccionado, PropiedadQueRestringe);
            let idRestrictor: number = Numero(valorDeLaColumna);
            let elemento: Elemento = infoSelector.LeerElemento(0);
            datos.idSeleccionado = elemento.Id;
            let filtro: Crud.DatosRestrictor = new Crud.DatosRestrictor(PropiedadRestrictora, idRestrictor, elemento.Texto);

            datos.FiltroRestrictor = filtro;
            return datos;
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

            this.CargarGrid(atGrid.accion.anterior, posicion);
        }
        public ObtenerSiguientes() {
            let cantidad: number = this.Navegador.Cantidad;
            let pagina: number = this.Navegador.Pagina;
            let total: number = this.Navegador.Total;
            let posicion: number = pagina * cantidad;
            if (posicion >= total)
                return;

            this.CargarGrid(atGrid.accion.siguiente, posicion);
        }

        protected CargarGrid(accion: string, posicion: number) {
            let url: string = this.DefinirPeticionDeBusqueda(Ajax.EndPoint.LeerDatosParaElGrid, accion, posicion);
            var datosDePeticion = new DatosPeticionNavegarGrid(this, accion, posicion);
            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EndPoint.LeerDatosParaElGrid
                , datosDePeticion
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.CrearFilasEnElGrid
                , null
            );

            a.Ejecutar();
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

        private CrearFilasEnElGrid(peticion: ApiDeAjax.DescriptorAjax) {
            let datosDeEntrada: DatosPeticionNavegarGrid = (peticion.DatosDeEntrada as DatosPeticionNavegarGrid);
            let grid: GridDeDatos = datosDeEntrada.Grid;
            let infoObtenida: ResultadoDeLectura = peticion.resultado.datos as ResultadoDeLectura;
            var registros = infoObtenida.registros;
            if (datosDeEntrada.Accion == atGrid.accion.buscar)
                grid.Navegador.Total = infoObtenida.total;

            let filaCabecera: PropiedadesDeLaFila[] = grid.obtenerDescriptorDeLaCabecera(grid);
            var cuerpoDeLaTabla = document.createElement("tbody");
            cuerpoDeLaTabla.id = `${grid.Grid.id}_tbody`;
            cuerpoDeLaTabla.classList.add(ClaseCss.cuerpoDeLaTabla);
            for (let i = 0; i < registros.length; i++) {
                let fila = grid.crearFila(filaCabecera, registros[i], i);
                cuerpoDeLaTabla.append(fila);
            }

            var tabla = grid.Grid.querySelector("table");
            var tbody = tabla.querySelector("tbody");
            if (tbody === null || tbody === undefined)
                tabla.append(cuerpoDeLaTabla);
            else {
                tabla.removeChild(tbody);
                tabla.append(cuerpoDeLaTabla);
            }
            grid.ActualizarInformacionDelGrid(grid, datosDeEntrada.Accion, datosDeEntrada.PosicionDesdeLaQueSeLee, registros.length);
            let posicion: number = grid.PosicionGrid();
            let altura: number = grid.AlturaDelGrid(posicion);
            grid.FijarAlturaCuerpoDeLaTabla(altura);
            grid.AplicarQueFilasMostrar();
        }

        private crearFila(filaCabecera: PropiedadesDeLaFila[], registro: any, numeroDeFila: number): HTMLTableRowElement {
            let fila = document.createElement("tr");
            fila.id = `${this.IdGrid}_d_tr_${numeroDeFila}`;
            fila.classList.add(ClaseCss.filaDelGrid);
            let idDelElemento: number = 0;
            for (let j = 0; j < filaCabecera.length; j++) {

                let columnaCabecera: PropiedadesDeLaFila = filaCabecera[j];
                let valor: any = this.BuscarValorDeColumnaRegistro(registro, columnaCabecera.propiedad);
                if (columnaCabecera.propiedad === atControl.id) {
                    if (!IsNumber(valor))
                        throw new Error("El id del elemento leido debe ser numérico");
                    idDelElemento = Numero(valor);
                }

                let celdaDelTd: HTMLTableCellElement = this.crearCelda(fila, registro, columnaCabecera, j, valor);
                fila.append(celdaDelTd);
            }

            fila.setAttribute(atControl.valorTr, idDelElemento.toString());
            return fila;
        }

        private crearCelda(fila: HTMLTableRowElement, registro: any, columnaCabecera: PropiedadesDeLaFila, numeroDeCelda: number, valor: string): HTMLTableCellElement {
            let celdaDelTd: HTMLTableCellElement = document.createElement("td");
            celdaDelTd.id = `${fila.id}.${numeroDeCelda}`;
            celdaDelTd.setAttribute(atControl.nombre, `td.${columnaCabecera.propiedad}.${this.IdGrid}`);
            celdaDelTd.setAttribute(atControl.propiedad, `${columnaCabecera.propiedad}`);
            celdaDelTd.style.width = `${columnaCabecera.anchoEnPixel}px`;
            celdaDelTd.style.textAlign = columnaCabecera.estilo.textAlign;
            celdaDelTd.style.width = `${columnaCabecera.estilo.width}`;

            let idCheckDeSeleccion: string = `${fila.id}.chksel`;
            let eventoOnClick: string = this.definirPulsarCheck(idCheckDeSeleccion, celdaDelTd.id);
            celdaDelTd.setAttribute(atControl.eventoJs.onclick, eventoOnClick);

            if (columnaCabecera.claseCss === ClaseCss.columnaOculta) {
                celdaDelTd.classList.add(ClaseCss.columnaOculta);
            }

            if (columnaCabecera.propiedad === 'chksel')
                this.insertarCheckEnElTd(fila.id, celdaDelTd, columnaCabecera.propiedad);
            else {
                this.insertarInputEnElTd(fila.id, registro, columnaCabecera, celdaDelTd, valor);
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
                if (this.EsCrud)
                    a = `${GestorDeEventos.delMantenimiento}('fila-pulsada', '${idCheckDeSeleccion}#${idControlHtml}');`;
                else
                    throw new Error("No se ha definido el gestor de eventos a asociar a la pulsación de una fila en el grid");
            }
            return a;
        }

        private insertarInputEnElTd(idFila: string, registro: any, columnaCabecera: PropiedadesDeLaFila, celdaDelTd: HTMLTableCellElement, valor: string) {
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

        private obtenerDescriptorDeLaCabecera(grid: GridDeDatos): Array<PropiedadesDeLaFila> {
            let filaCabecera: Array<PropiedadesDeLaFila> = new Array<PropiedadesDeLaFila>();
            var cabecera = grid.Tabla.rows[0];
            var ths = cabecera.querySelectorAll('th');
            for (let i = 0; i < ths.length; i++) {
                let p: PropiedadesDeLaFila = new PropiedadesDeLaFila();
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

        public FilaPulsada(infoSelector: InfoSelector, idCheck: string, idDelInput: string) {

            let check: HTMLInputElement = document.getElementById(idCheck) as HTMLInputElement;
            let expresionElemento: string = this.ObtenerExpresionMostrar(idCheck);
            //Se hace porque antes ha pasado por aquí por haber pulsado en la fila
            if (idCheck !== idDelInput) {
                check.checked = !check.checked;
            }

            if (check.checked) {
                let id: number = this.AnadirAlInfoSelector(infoSelector, idCheck, expresionElemento);
                if (!(this instanceof ModalConGrid))
                    this.AjustarOpcionesDeMenu(id);
            }
            else {
                this.QuitarDelSelector(infoSelector, idCheck);
                if (this.InfoSelector.Cantidad === 0 && (this instanceof ModalConGrid) === false)
                    this.DeshabilitarOpcionesDeMenuDeElemento();
            }

            this.Navegador.ActualizarMensaje(infoSelector.Cantidad);
        }


        protected DeshabilitarOpcionesDeMenuDeElemento() {
            let opcionesDeElemento: NodeListOf<HTMLButtonElement> = this.ZonaDeMenu.querySelectorAll(`input[${atOpcionDeMenu.clase}="${ClaseDeOpcioDeMenu.DeElemento}"]`) as NodeListOf<HTMLButtonElement>;
            for (var i = 0; i < opcionesDeElemento.length; i++) {
                let opcion: HTMLButtonElement = opcionesDeElemento[i];
                opcion.disabled = true;
            }
        }

        private AjustarOpcionesDeMenu(id: number): void {
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
            let modoDeAccesoDelUsuario = peticion.resultado.modoDeAcceso;
            let opcionesGenerales: NodeListOf<HTMLButtonElement> = mantenimiento.ZonaDeMenu.querySelectorAll(`input[${atOpcionDeMenu.clase}="${ClaseDeOpcioDeMenu.DeElemento}"]`) as NodeListOf<HTMLButtonElement>;
            let hacerLaInterseccion: boolean = mantenimiento.InfoSelector.Cantidad > 1;
            for (var i = 0; i < opcionesGenerales.length; i++) {
                let opcion: HTMLButtonElement = opcionesGenerales[i];
                let estaDeshabilitado = opcion.disabled;
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
                            opcion.disabled = (estaDeshabilitado && hacerLaInterseccion) || false;
            }
        }


        private DefinirPeticionDeLeerModoDeAccesoAlElemento(id: number): string {
            let url: string = `/${this.Controlador}/${Ajax.EndPoint.LeerModoDeAccesoAlElemento}`;
            let parametros: string = `${Ajax.Param.negocio}=${this.Negocio}&${Ajax.Param.id}=${id}`;
            let peticion: string = url + '?' + parametros;
            return peticion;
        }

        public OrdenarPor(columna: string) {
            this.EstablecerOrdenacion(columna);
            this.CargarGrid(atGrid.accion.buscar, 0);
        }

        public MostrarSoloSeleccionadas(): void {
            if (NumeroMayorDeCero(this.SoloSeleccionadas.value)) {
                this.SoloSeleccionadas.value = "0";
                this.EtiquetaMostrarSeleccionadas.innerText = "Seleccionadas";
            }
            else {
                this.SoloSeleccionadas.value = "1";
                this.EtiquetaMostrarSeleccionadas.innerText = "Todas las filas";
            }
            this.AplicarQueFilasMostrar();
        }

        public AplicarQueFilasMostrar() {
            if (NumeroMayorDeCero(this.SoloSeleccionadas.value)) {
                this.MostrarFilasSeleccionadas();
            }
            else {
                this.MostrarTodasLasFilas();
            }
        }

        private MostrarTodasLasFilas(): void {
            let trs: NodeListOf<HTMLTableRowElement> = this.CuerpoTablaGrid.querySelectorAll("tr") as NodeListOf<HTMLTableRowElement>;
            let i: number = 0;
            for (i = 0; i < trs.length; i++) {
                let tr: HTMLTableRowElement = trs[i];
                tr.hidden = false;
            }

        }

        private MostrarFilasSeleccionadas(): void {
            let trs: NodeListOf<HTMLTableRowElement> = this.CuerpoTablaGrid.querySelectorAll("tr") as NodeListOf<HTMLTableRowElement>;
            let i: number = 0;
            for (i = 0; i < trs.length; i++) {
                let tr: HTMLTableRowElement = trs[i];
                let idDelElemento = Numero(tr.getAttribute(atControl.valorTr));
                tr.hidden = !this.InfoSelector.Contiene(idDelElemento);
            }
        }
    }

}