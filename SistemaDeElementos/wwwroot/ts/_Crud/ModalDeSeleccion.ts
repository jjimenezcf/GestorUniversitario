namespace Crud {

    export class ModalSeleccion extends GridMnt {


        private get Selector(): HTMLSelector {
            return <HTMLSelector>document.getElementById(this.idSelector);
        }

        private get EditorDelGrid(): HTMLInputElement {
            var idEditorMostrar: string = this.Selector.getAttribute(atSelector.idEditorMostrar);
            return <HTMLInputElement>document.getElementById(idEditorMostrar);
        }

        public IdModal: string;
        protected get Modal(): HTMLDivElement {
            return document.getElementById(this.IdModal) as HTMLDivElement;
        };

        private get idSelector(): string {
            return this.Modal.getAttribute(atSelector.selector);
        }

        private get ExpresionNombre(): string {
            return this.Grid.getAttribute(atControl.expresionElemento);
        }
        private get ColumnaId(): string {
            return this.Selector.getAttribute(atSelector.propiedadParaFiltrar);
        }


        constructor(idModal: string, idCrudModal: string) {
            super(idCrudModal);
            this.IdModal = idModal;
        }

        public InicializarModal() {
            let referenciaCheck: string = `chksel.${this.IdGrid}`;
            this.blanquearCheck(referenciaCheck);
            this.InfoSelector.QuitarTodos();
            if (this.Selector.hasAttribute(atSelector.ListaDeSeleccionados))
                this.Selector.setAttribute(atSelector.ListaDeSeleccionados, '');
        };


        private InicializarListaDeSeleccionados() {
            if (this.Selector.hasAttribute(atSelector.ListaDeSeleccionados))
                this.Selector.setAttribute(atSelector.ListaDeSeleccionados, '');
        }

        public AbrirModalDeSeleccion() {
            BlanquearMensaje();
            this.EditorDelGrid.value = this.Selector.value;
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
            this.InicializarListaDeSeleccionados();

            for (var x = 0; x < this.InfoSelector.Cantidad; x++) {
                var elemento: Elemento = this.InfoSelector.LeerElemento(x);
                if (!elemento.EsVacio())
                    this.mapearElementoAlHtmlSelector(elemento);
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
                if (IsNullOrEmpty(expresionElemento))
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
            if (this.Selector.hasAttribute(atSelector.ListaDeSeleccionados)) {
                ids = this.Selector.getAttribute(atSelector.ListaDeSeleccionados);
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
            let cantidad: number = Numero(this.Navegador.value);
            let posicion: number = Numero(this.Navegador.getAttribute(atGrid.navegador.posicion));
            posicion = posicion - (cantidad * 2);
            if (posicion < 0)
                posicion = 0;
            this.Buscar(posicion);
        }

        public ObtenerSiguientes() {
            let posicion: number = Numero(this.Navegador.getAttribute(atGrid.navegador.posicion));
            this.Buscar(posicion);
        }

        public RecargarGrid() {
            BlanquearMensaje();
            this.Buscar(0);
        }

        private Buscar(posicion: number) {
            let url: string = this.DefinirPeticionDeCargarGrid(posicion);
            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EndPoint.RecargarModalEnHtml
                , this
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.ActualizarGrid
                , null
            );

            a.Ejecutar();

        }

        private DefinirPeticionDeCargarGrid(posicion: number): string {
            var cantidad = this.Navegador.value.Numero();
            var controlador = this.Navegador.getAttribute(atControl.controlador);
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

        private ActualizarGrid(peticion: ApiDeAjax.DescriptorAjax) {
            let modal: ModalSeleccion = (peticion.DatosDeEntrada as ModalSeleccion);
            let resultado = peticion.resultado as ResultadoHtml;

            if (modal.IdGrid === modal.Grid.getAttribute(atControl.id)) {
                modal.ActualizarGridHtml(modal, resultado.html);
            }
        }

        public TextoSelectorCambiado(valor: string) {
            this.EditorDelGrid.value = this.Selector.value;
            let url: string = this.DefinirPeticionLeerParaSelector();

            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EndPoint.Leer
                , this
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.TratarValoresDevuelto
                , null
            );

            a.Ejecutar();

        }

        private DefinirPeticionLeerParaSelector(): string {
            var controlador = this.Navegador.getAttribute(atControl.controlador);
            let filtroJson: string = this.ObtenerClausulaParaSelector();

            let url: string = `/${controlador}/${Ajax.EndPoint.Leer}`;
            let parametros: string = `${Ajax.Param.filtro}=${filtroJson}`;
            let peticion: string = url + '?' + parametros;
            return peticion;
        }


        private TratarValoresDevuelto(peticion: ApiDeAjax.DescriptorAjax) {
            let modal: ModalSeleccion = (peticion.DatosDeEntrada as ModalSeleccion);
            modal.ProcesarRegistrosLeidos(peticion.resultado.datos);
        }

        private ObtenerClausulaParaSelector(): string {
            var clausula: ClausulaDeFiltrado = this.ClausulaSegunLoEscrito();
            var clausulas = new Array<ClausulaDeFiltrado>();
            clausulas.push(clausula);
            return JSON.stringify(clausulas);
        }

        private ClausulaSegunLoEscrito(): ClausulaDeFiltrado {
            var propiedad = this.Selector.getAttribute(atSelector.popiedadBuscar);
            var criterio = this.Selector.getAttribute(atSelector.criterioBuscar);
            var valor = this.Selector.value.trim();
            var clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);
            return clausula;
        };

        private ProcesarRegistrosLeidos(registros: Array<any>) {
            this.InicializarModal();
            var propiedadmostrar = this.Selector.getAttribute(atSelector.propiedadmostrar);
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
            this.EditorDelGrid.value = '';
            var valorDelSelector = this.Selector.value;
            if (!IsNullOrEmpty(valorDelSelector))
                valorDelSelector = valorDelSelector + " | ";

            this.Selector.value = valorDelSelector + elemento.Texto;
            this.mapearIdAlHtmlSelector(elemento.Id);
        }

        private mapearIdAlHtmlSelector(id: number) {
            var listaDeIds = this.Selector.getAttribute(atSelector.ListaDeSeleccionados);
            if (listaDeIds === null) {
                var atributo = document.createAttribute(atSelector.ListaDeSeleccionados);
                this.Selector.setAttributeNode(atributo);
                listaDeIds = "";
            }

            if (!IsNullOrEmpty(listaDeIds))
                listaDeIds = listaDeIds + ';';
            listaDeIds = listaDeIds + id;
            this.Selector.setAttribute(atSelector.ListaDeSeleccionados, listaDeIds);
        }


    }
}