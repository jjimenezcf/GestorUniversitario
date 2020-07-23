namespace Crud {

    class ClausulaDeOrdenacion {
        propiedad: string;
        modo: string;

        constructor(propiedad: string, modo: string) {
            this.propiedad = propiedad;
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

    export class GridMnt extends CrudBase {

        protected Ordenacion: Ordenacion;
        protected InfoSelector: InfoSelector;

        private idPanelMnt: string;

        protected get PanelMnt(): HTMLDivElement {
            return document.getElementById(this.idPanelMnt) as HTMLDivElement;
        }

        protected get IdGrid(): string {
            return this.PanelMnt.getAttribute(Atributo.grid);
        }
        private idHtmlFiltro: string;

        protected get ZonaDeFiltro(): HTMLDivElement {
            return document.getElementById(this.idHtmlFiltro) as HTMLDivElement;
        }

        protected get Grid(): HTMLDivElement {
            return document.getElementById(this.IdGrid) as HTMLDivElement;
        }

        protected get Tabla(): HTMLTableElement {
            let idTabla: string = this.Grid.getAttribute(Atributo.tablaDeDatos);
            return document.getElementById(idTabla) as HTMLTableElement;
        }

        protected get Navegador(): HTMLInputElement {
            return this.ObtenerNavegador();
        }

        protected get Controlador(): string {
            return this.Navegador.getAttribute(Atributo.controlador);
        }

        constructor(idPanelMnt: string) {
            super();
            this.idPanelMnt = idPanelMnt;
            this.InfoSelector = new InfoSelector(this.IdGrid);
            this.idHtmlFiltro = this.Grid.getAttribute(Atributo.zonaDeFiltro);
            this.Ordenacion = new Ordenacion();
        }

        protected InicializarNavegador() {
            if (this.HayHistorial) {
                let cantidad: string = this.Estado.Obtener(Variables.Grid.Cantidad);
                if (NumeroMayorDeCero(cantidad))
                    this.Navegador.value = cantidad;
            }

            for (var i = 0; i < this.Ordenacion.Count(); i++) {
                let orden: Orden = this.Ordenacion.Leer(i);
                let columna: HTMLTableHeaderCellElement = document.getElementById(orden.IdColumna) as HTMLTableHeaderCellElement;
                columna.setAttribute(Atributo.modoOrdenacion, orden.Modo);
                let a: HTMLElement = columna.getElementsByTagName('a')[0] as HTMLElement;
                a.setAttribute("class", orden.ccsClase);
            }
        }

        protected EstablecerOrdenacion(idcolumna: string) {
            let htmlColumna: HTMLTableHeaderCellElement = document.getElementById(idcolumna) as HTMLTableHeaderCellElement;
            let modo: string = htmlColumna.getAttribute(Atributo.modoOrdenacion);
            if (IsNullOrEmpty(modo))
                modo = ModoOrdenacion.ascedente;
            else if (modo === ModoOrdenacion.ascedente)
                modo = ModoOrdenacion.descendente;
            else if (modo === ModoOrdenacion.descendente)
                modo = ModoOrdenacion.sinOrden;
            else if (modo === ModoOrdenacion.sinOrden)
                modo = ModoOrdenacion.ascedente;

            let propiedad: string = htmlColumna.getAttribute(Atributo.propiedad);
            this.Ordenacion.Actualizar(idcolumna, propiedad, modo);

            htmlColumna.setAttribute(Atributo.modoOrdenacion, modo);

        }

        private ObtenerNavegador(): HTMLInputElement {
            let idCrtlCantidad: string = `${this.IdGrid}_${LiteralGrid.idCtrlCantidad}`;
            let input = document.getElementById(`${idCrtlCantidad}`) as HTMLInputElement;
            return input;
        }

        protected ObtenerExpresionMostrar(idCheck: string): string {
            let expresion: string = this.Grid.getAttribute(Atributo.expresionElemento).toLowerCase();
            if (!IsNullOrEmpty(expresion)) {
                let fila: HTMLTableRowElement = this.ObtenerlaFila(idCheck);
                let tds: HTMLCollectionOf<HTMLTableCellElement> = fila.getElementsByTagName('td') as HTMLCollectionOf<HTMLTableCellElement>;
                for (let j = 0; j < tds.length; j++) {
                    let input: HTMLInputElement = tds[j].getElementsByTagName('input')[0] as HTMLInputElement;
                    if (input !== undefined) {
                        let propiedad: string = input.getAttribute(Atributo.propiedad).toLowerCase();
                        if (!IsNullOrEmpty(propiedad) && expresion.includes(`[${propiedad}]`)) {
                            expresion = expresion.replace(`[${propiedad}]`, input.value);
                        }
                    }
                }
            }
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
            var arrayIds = this.ObtenerControlesDeFiltro();
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
            return JSON.stringify(clausulas);
        }

        private ObtenerControlesDeFiltro() {

            var arrayIds = new Array();
            var arrayHtmlInput = this.ZonaDeFiltro.getElementsByTagName(TagName.input);

            for (let i = 0; i < arrayHtmlInput.length; i++) {
                var htmlInput = arrayHtmlInput[i];
                var esFiltro = htmlInput.getAttribute(Atributo.filtro);
                if (esFiltro === 'S') {
                    var id = htmlInput.getAttribute(Atributo.id);
                    if (id === null)
                        console.log(`Falta el atributo id del componente de filtro ${htmlInput}`);
                    else
                        arrayIds.push(id);
                }
            }

            var arrayHtmlSelect = this.ZonaDeFiltro.getElementsByTagName(TagName.select);
            for (let i = 0; i < arrayHtmlSelect.length; i++) {
                var htmlSelect = arrayHtmlSelect[i];
                var id = htmlSelect.getAttribute(Atributo.id);
                arrayIds.push(id);
            }

            return arrayIds;
        }

        private ObtenerClausulaRestrictor(restrictor: HTMLInputElement): ClausulaDeFiltrado {
            let propiedad: string = restrictor.getAttribute(Atributo.propiedad);
            let criterio: string = Literal.filtro.criterio.igual;
            let valor = restrictor.getAttribute(Atributo.restrictor);
            let clausula: ClausulaDeFiltrado = null;
            if (!IsNullOrEmpty(valor))
                //clausula = { propiedad: `${propiedad}`, criterio: `${criterio}`, valor: `${valor}` };
                clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);

            return clausula;
        }

        private ObtenerClausulaEditor(editor: HTMLInputElement): ClausulaDeFiltrado {
            var propiedad: string = editor.getAttribute(Atributo.propiedad);
            var criterio: string = editor.getAttribute(Atributo.criterio);
            var valor = editor.value;
            var clausula = null;
            if (!IsNullOrEmpty(valor))
                //clausula = { propiedad: `${propiedad}`, criterio: `${criterio}`, valor: `${valor}` };
                clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);

            return clausula;
        }

        private ObtenerClausulaSelector(selector: HTMLInputElement): ClausulaDeFiltrado {
            var propiedad = selector.getAttribute(Atributo.propiedad);
            var criterio = selector.getAttribute(Atributo.criterio);
            var valor = null;
            var clausula = null;
            if (selector.hasAttribute(AtributoSelector.ListaDeSeleccionados)) {
                var ids = selector.getAttribute(AtributoSelector.ListaDeSeleccionados);
                if (!ids.NoDefinida()) {
                    valor = ids;
                    clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);
                }
            }
            return clausula;
        }

        private ObtenerClausulaListaDinamica(input: HTMLInputElement): ClausulaDeFiltrado {
            var propiedad = input.getAttribute(Atributo.propiedad);
            var criterio = input.getAttribute(Atributo.criterio);

            let lista: ListaDinamica = new ListaDinamica(input);
            let valor: number = lista.BuscarSeleccionado(input.value);


            var clausula = null;
            if (Number(valor) > 0) {
                clausula = new ClausulaDeFiltrado(propiedad, criterio, valor.toString());
            }
            return clausula;
        }

        private ObtenerClausulaListaDeELemento(selet: HTMLSelectElement): ClausulaDeFiltrado {
            var propiedad = selet.getAttribute(Atributo.propiedad);
            var criterio = selet.getAttribute(Atributo.criterio);
            var valor = selet.value;
            var clausula = null;
            if (!IsNullOrEmpty(valor) && Number(valor) > 0) {
                clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);
            }
            return clausula;
        }

        private ObtenerlaFila(idCheck: string) {
            let idFila: string = idCheck.replace(".chksel", "");
            let fila: HTMLTableRowElement = document.getElementById(idFila) as HTMLTableRowElement;
            return fila;
        }

        protected AnadirAlInfoSelector(idCheck: string, expresionElemento: string) {
            let id: string = this.ObtenerElIdDelElementoDelaFila(idCheck);
            this.InfoSelector.InsertarElemento(id, expresionElemento);
        }

        protected QuitarDelSelector(idCheck: string) {
            let id: string = this.ObtenerElIdDelElementoDelaFila(idCheck);
            this.InfoSelector.Quitar(id);
        }

        private ObtenerElIdDelElementoDelaFila(idCheck: string): string {
            let columnaId: string = idCheck.replace(".chksel", `.${Literal.id}`);
            let inputId: HTMLInputElement = document.getElementById(columnaId) as HTMLInputElement;
            let id: string = inputId.value;
            return id;
        }

        protected MarcarElementos() {
            if (this.InfoSelector.Cantidad === 0)
                return;

            var celdasId = document.getElementsByName(`${Literal.id}.${this.IdGrid}`);
            var len = celdasId.length;
            for (var i = 0; i < this.InfoSelector.Cantidad; i++) {
                for (var j = 0; j < len; j++) {
                    let id: number = this.InfoSelector.LeerId(i);
                    if ((<HTMLInputElement>celdasId[j]).value.Numero() == id) {
                        var idCheck = celdasId[j].id.replace(`.${Atributo.id}`, LiteralMnt.postfijoDeCheckDeSeleccion);
                        var check = document.getElementById(idCheck);
                        (<HTMLInputElement>check).checked = true;
                        break;
                    }
                }
            }
        }

        protected ActualizarGridHtml(contenedorGrid: GridMnt, resultadoHtml: string) {            
            contenedorGrid.Grid.innerHTML = resultadoHtml;
            contenedorGrid.InicializarNavegador();
            if (contenedorGrid.InfoSelector !== undefined && contenedorGrid.InfoSelector.Cantidad > 0) {
                contenedorGrid.MarcarElementos();
                contenedorGrid.InfoSelector.SincronizarCheck();
            }
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
                        let propiedad: string = input.getAttribute(Atributo.propiedad);
                        if (propiedad.toLocaleLowerCase() === Atributo.id) {
                            let valor: string = input.value;
                            if (valor.Numero() == id)
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
                let propiedadCelda: string = celda.getAttribute(Atributo.propiedad);
                if (propiedadCelda.toLocaleLowerCase() === propiedadBuscada)
                    return celda;
            }
            throw new Error(`No se ha localizado una celda con la propiedad '${propiedadBuscada}' definida`);
        }

        public AntesDeNavegar() {
            super.AntesDeNavegar();
            this.Estado.Agregar(Variables.Grid.Cantidad, this.Navegador.value);
        }

    }

}