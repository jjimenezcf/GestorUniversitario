namespace Crud {

    export class ModalSeleccion extends ModalConGrid {

        private get Selector(): HTMLSelector {
            return document.getElementById(this.idSelector) as HTMLSelector;
        }

        private get EditorDelGrid(): HTMLInputElement {
            var idEditorMostrar: string = this.Selector.getAttribute(atSelector.idEditorMostrar);
            return <HTMLInputElement>document.getElementById(idEditorMostrar);
        }


        private get idSelector(): string {
            return this.Modal.getAttribute(atSelector.selector);
        }

        private get ExpresionNombre(): string {
            return this.Grid.getAttribute(atControl.expresionElemento);
        }
        private get ColumnaId(): string {
            return this.Selector.getAttribute(atSelector.propiedadParaFiltrar);
        }


        constructor(idModal: string) {
            super(idModal, document.getElementById(idModal).getAttribute(atControl.crudModal));
        }

        public InicializarModalDeSeleccion() {
            super.InicializarModalConGrid();
            if (this.Selector.hasAttribute(atSelector.ListaDeSeleccionados))
                this.Selector.setAttribute(atSelector.ListaDeSeleccionados, '');
        };

        public AbrirModalDeSeleccion() {
            this.EditorDelGrid.value = this.Selector.value;

            super.AbrirModalConGrid();

            var arrayMarcados = this.ElementosMarcados();
            this.InfoSelector.InsertarElementos(arrayMarcados);
            this.MarcarElementos();
            this.InfoSelector.SincronizarCheck();
        }

        private ElementosMarcados(): Array<Elemento> {
            let elementos: Array<Elemento> = new Array<Elemento>();

            let seleccionados: string = this.Selector.hasAttribute(atSelector.ListaDeSeleccionados) ?
                this.Selector.getAttribute(atSelector.ListaDeSeleccionados) :
                "";

            if (!IsNullOrEmpty(seleccionados)) {
                var listaNombres = (<HTMLSelector>this.Selector).value.split('|');
                var listaIds = seleccionados.split(';');
                for (var i = 0; i < listaIds.length; i++) {
                    this.LeerElementoSeleccionado(Numero(listaIds[i]))
                }
            }
            return elementos;
        }

        public CerrarModalDeSeleccion() {
            this.CerrarModalConGrid();
        }

        public SeleccionarElementos() {
            this.Selector.value = "";
            this.BlanquearListaDeSeleccionados();

            for (var x = 0; x < this.InfoSelector.Cantidad; x++) {
                var elemento: Elemento = this.InfoSelector.LeerElemento(x);
                if (!elemento.EsVacio())
                    this.mapearElementoAlHtmlSelector(elemento);
                else
                    MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `Se ha leido mal el elemento del selector ${this.IdGrid} de la posición ${x}`);
            }
            this.CerrarModalDeSeleccion();
        }


        private BlanquearListaDeSeleccionados() {
            if (this.Selector.hasAttribute(atSelector.ListaDeSeleccionados))
                this.Selector.setAttribute(atSelector.ListaDeSeleccionados, '');
        }


        public TextoSelectorCambiado() {
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
            var controlador = this.Navegador.Controlador;
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

        private ProcesarRegistrosLeidos(registros: Array<Elemento>) {
            this.InicializarModalDeSeleccion();
            var propiedadmostrar = this.Selector.getAttribute(atSelector.propiedadmostrar);
            if (registros.length === 1) {
                var registro = registros[0];
                for (let key in registro) {
                    if (key === propiedadmostrar) {
                        this.Selector.value = '';
                        this.mapearElementoAlHtmlSelector(new Elemento(registro));
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