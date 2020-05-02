namespace Crud {

    export class ModalSeleccion extends GridMnt {

        private ExpresionNombre: string;
        private ColumnaId: string;
        private ColumnaCheck: string;
        private idSelector

        private get Selector(): HTMLSelector{
            return <HTMLSelector>document.getElementById(this.idSelector);
        }


        private Modal: HTMLDivElement;

        public IdModal: string;

        constructor(idModal: string, idGrid: string) {
            super(idGrid);

            this.Modal = document.getElementById(idModal) as HTMLDivElement;

            this.IdModal = idModal;
            this.IdGrid = idGrid;

            this.ExpresionNombre = this.Grid.getAttribute(Atributo.expresionElemento);

            this.idSelector = this.Modal.getAttribute("selector");
            this.ColumnaId = this.Selector.getAttribute("propiedadFiltrar");
        }

        public InicializarModal() {
            let referenciaCheck: string = `chksel.${this.IdGrid}`;
            this.blanquearCheck(referenciaCheck);
            this.InfoSelector.QuitarTodos();
            if (this.Selector.hasAttribute(ListaDeSeleccionados))
                this.Selector.setAttribute(ListaDeSeleccionados, '');
        };

        public AbrirModalDeSeleccion() {
            BlanquearMensaje();
            this.Selector.MapearTextoAlEditorDelGrid();
            this.RecargarGrid();

            var arrayMarcados = this.elementosMarcados();
            this.InfoSelector.InsertarElementos(arrayMarcados);
            this.MarcarElementos();
            this.InfoSelector.SincronizarCheck();
            this.Modal.style.display = 'block';
        }

        public CerrarModalDeSeleccion() {
            let referenciaCheck: string = `chksel.${this.IdGrid}`;
            this.blanquearCheck(referenciaCheck);
            this.InfoSelector.QuitarTodos();

            this.CerrarModal(this.IdModal);
        }

        private blanquearCheck(refCheckDeSeleccion: string) {
            document.getElementsByName(`${refCheckDeSeleccion}`).forEach(c => {
                let check = <HTMLInputElement>c;
                check.checked = false;
            }
            );
        }

        public SeleccionarElementos() {
            this.Selector.value = "";
            this.Selector.InicializarAtributos();

            for (var x = 0; x < this.InfoSelector.Cantidad; x++) {
                var elemento: Elemento = this.InfoSelector.LeerElemento(x);
                if (!elemento.EsVacio())
                    mapearElementoAlHtmlSelector(this.Selector, elemento);
                else
                    Mensaje(TipoMensaje.Error, `Se ha leido mal el elemento del selector ${this.IdGrid} de la posición ${x}`);
            }
            this.CerrarModalDeSeleccion();
        }

        public FilaPulsada(idCheck: string, idDelInput: string) {

            let check: HTMLInputElement = document.getElementById(idCheck) as HTMLInputElement;
            //Se hace porque antes ha pasado por aquí por haber pulsado en la fila
            if (idCheck === idDelInput) {
                check.checked = !check.checked;
                return;
            }

            check.checked = !check.checked;

            if (check.checked) {
                let expresionElemento: string = this.ObtenerExpresionMostrar(idCheck);
                if (EsNula(expresionElemento))
                    Mensaje(TipoMensaje.Error, `No está definida la expresion del elemento del grid ${this.IdGrid}`);
                else
                    this.AnadirAlInfoSelector(idCheck, expresionElemento);
            }
            else
                this.QuitarDelSelector(idCheck);
        }

        private elementosMarcados() {
            var ids = "";
            var elementos = new Array();
            if (this.Selector.hasAttribute(ListaDeSeleccionados)) {
                ids = this.Selector.getAttribute(ListaDeSeleccionados);
                if (!ids.NoDefinida()) {
                    var listaNombres = (<HTMLSelector>this.Selector).value.split('|');
                    var listaIds = ids.split(';');
                    for (var i = 0; i < listaIds.length; i++) {
                        var e = { id: listaIds[i], valor: listaNombres[i] };
                        elementos.push(e);
                    }
                }
            }

            return elementos;
        }

        public OrdenarPor(columna: string) {
            this.EstablecerOrdenacion(columna);
            this.Buscar(0);
        }

        public ObtenerUltimos() {
            this.Buscar(-1);
        }

        public ObtenerAnteriores() {
            let cantidad: number = this.Navegador.value.Numero();
            let posicion: number = this.Navegador.getAttribute(Atributo.posicion).Numero();
            posicion = posicion - (cantidad * 2);
            if (posicion < 0)
                posicion = 0;
            this.Buscar(posicion);
        }

        public ObtenerSiguientes() {
            let posicion: number = this.Navegador.getAttribute(Atributo.posicion).Numero();
            this.Buscar(posicion);
        }

        public RecargarGrid() {
            BlanquearMensaje();
            this.Buscar(0);
        }

        private Buscar(posicion: number) {
            let url: string = this.DefinirPeticionDeCargarGrid(posicion);
            let req: XMLHttpRequest = new XMLHttpRequest();
            this.PeticionSincrona(req, url, Ajax.EndPoint.RecargarModalEnHtml);
        }

        private DefinirPeticionDeCargarGrid(posicion: number): string {
            var cantidad = this.Navegador.value.Numero();
            var controlador = this.Navegador.getAttribute(Atributo.controlador);
            var filtroJson = this.ObtenerFiltros();
            var ordenJson = this.ObtenerOrdenacion();

            let url: string = `/${controlador}/${Ajax.EndPoint.RecargarModalEnHtml}`;
            let parametros: string = `${Ajax.Param.idModal}=${this.IdModal}` +
                `&${Ajax.Param.posicion}=${posicion}` +
                `&${Ajax.Param.cantidad}=${cantidad}` +
                `&${Ajax.Param.filtro}=${filtroJson}` +
                `&${Ajax.Param.orden}=${ordenJson}`;
            let peticion: string = url + '?' + parametros;
            return peticion;
        }

        protected DespuesDeLaPeticion(req: XMLHttpRequest, peticion: string): ResultadoJson {
            let resultado: ResultadoHtml = super.DespuesDeLaPeticion(req, peticion) as ResultadoHtml;

            if (peticion === Ajax.EndPoint.RecargarModalEnHtml) {
                if (this.IdGrid === this.Grid.getAttribute(Literal.id)) {
                    this.Grid.innerHTML = resultado.html;
                    this.InicializarNavegador();
                    if (this.InfoSelector !== undefined && this.InfoSelector.Cantidad > 0) {
                        this.MarcarElementos();
                        this.InfoSelector.SincronizarCheck();
                    }
                }
            }

            if (peticion === Ajax.EndPoint.Leer) {
                this.ProcesarRegistrosLeidos(resultado.datos);
            }
            return resultado;
        }

        public TextoSelectorCambiado(valor: string) {

            if (!EsNula(valor)) {
                let url: string = this.DefinirPeticionLeerParaSelector();
                let req: XMLHttpRequest = new XMLHttpRequest();
                this.PeticionSincrona(req, url, Ajax.EndPoint.Leer);
            }
            else {
                this.Selector.InicializarSelector();
                var refCheckDeSeleccion: string = this.Selector.getAttribute(AtributoSelector.refCheckDeSeleccion);
                if (!EsNula(refCheckDeSeleccion))
                    this.blanquearCheck(refCheckDeSeleccion);
            }
        }

        private DefinirPeticionLeerParaSelector(): string {
            var controlador = this.Navegador.getAttribute(Atributo.controlador);
            let filtroJson: string = this.ObtenerClausulaParaSelector();

            let url: string = `/${controlador}/${Ajax.EndPoint.Leer}`;
            let parametros: string = `${Ajax.Param.filtro}=${filtroJson}`;
            let peticion: string = url + '?' + parametros;
            return peticion;
        }

        private ObtenerClausulaParaSelector(): string {
            var clausula: ClausulaDeFiltrado = this.ClausulaSegunLoEscrito();
            var clausulas = new Array<ClausulaDeFiltrado>();
            clausulas.push(clausula);
            return JSON.stringify(clausulas);
        }

        private ClausulaSegunLoEscrito(): ClausulaDeFiltrado {
            var propiedad = this.Selector.getAttribute(AtributoSelector.popiedadBuscar);
            var criterio = this.Selector.getAttribute(AtributoSelector.criterioBuscar);
            var valor = this.Selector.value.trim();
            var clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);
            return clausula;
        };

        private ProcesarRegistrosLeidos(registros: Array<any>) {
            this.InicializarModal();
            var propiedadmostrar = this.Selector.getAttribute(AtributoSelector.propiedadmostrar);
            if (registros.length === 1) {
                var registro = registros[0];
                for (let key in registro) {
                    if (key === propiedadmostrar) {
                        this.Selector.value = '';
                        this.mapearElementoAlHtmlSelector(new Elemento(registro['id'], registro[key]));
                        return;
                    }
                }
            }
            else {
                this.AbrirModalDeSeleccion();
            }
        }

        private mapearElementoAlHtmlSelector(elemento: Elemento) {
            this.Selector.BanquearEditorDelGrid();
            var valorDelSelector = this.Selector.value;
            if (!EsNula(valorDelSelector))
                valorDelSelector = valorDelSelector + " | ";

            this.Selector.value = valorDelSelector + elemento.Texto;
            this.mapearIdAlHtmlSelector(elemento.Id);
        }

        private mapearIdAlHtmlSelector(id: number) {
            var listaDeIds = this.Selector.getAttribute(ListaDeSeleccionados);
            if (listaDeIds === null) {
                var atributo = document.createAttribute(ListaDeSeleccionados);
                this.Selector.setAttributeNode(atributo);
                listaDeIds = "";
            }

            if (!EsNula(listaDeIds))
                listaDeIds = listaDeIds + ';';
            listaDeIds = listaDeIds + id;
            this.Selector.setAttribute(ListaDeSeleccionados, listaDeIds);
        }
    }
}