var Crud;
(function (Crud) {
    class ModalSeleccion extends Crud.ModalConGrid {
        get Selector() {
            return document.getElementById(this.idSelector);
        }
        get EditorDelGrid() {
            var idEditorMostrar = this.Selector.getAttribute(atSelector.idEditorMostrar);
            return document.getElementById(idEditorMostrar);
        }
        get idSelector() {
            return this.Modal.getAttribute(atSelector.selector);
        }
        get ExpresionNombre() {
            return this.Grid.getAttribute(atControl.expresionElemento);
        }
        get ColumnaId() {
            return this.Selector.getAttribute(atSelector.propiedadParaFiltrar);
        }
        constructor(idModal) {
            super(idModal, document.getElementById(idModal).getAttribute(atControl.crudModal));
        }
        InicializarModalDeSeleccion() {
            super.InicializarModalConGrid();
            if (this.Selector.hasAttribute(atSelector.ListaDeSeleccionados))
                this.Selector.setAttribute(atSelector.ListaDeSeleccionados, '');
        }
        ;
        AbrirModalDeSeleccion() {
            this.EditorDelGrid.value = this.Selector.value;
            this.RecargarGrid()
                .then((valor) => {
                this.TrasAbrirModalDeSeleccion(valor);
            })
                .catch((valor) => {
                ApiCrud.CerrarModal(this.Modal);
            });
        }
        TrasAbrirModalDeSeleccion(valor) {
            if (valor) {
                try {
                    var arrayMarcados = this.ElementosMarcados();
                    this.InfoSelector.InsertarElementos(arrayMarcados);
                    this.MarcarElementos();
                    this.InfoSelector.SincronizarCheck();
                }
                catch (error) {
                    ApiCrud.CerrarModal(this.Modal);
                    throw error;
                }
            }
            else
                ApiCrud.CerrarModal(this.Modal);
        }
        ElementosMarcados() {
            let elementos = new Array();
            let seleccionados = this.Selector.hasAttribute(atSelector.ListaDeSeleccionados) ?
                this.Selector.getAttribute(atSelector.ListaDeSeleccionados) :
                "";
            if (!IsNullOrEmpty(seleccionados)) {
                var listaNombres = this.Selector.value.split('|');
                var listaIds = seleccionados.split(';');
                for (var i = 0; i < listaIds.length; i++) {
                    this.LeerElementoSeleccionado(Numero(listaIds[i]));
                }
            }
            return elementos;
        }
        CerrarModalDeSeleccion() {
            this.CerrarModalConGrid();
        }
        SeleccionarElementos() {
            this.Selector.value = "";
            this.BlanquearListaDeSeleccionados();
            for (var x = 0; x < this.InfoSelector.Cantidad; x++) {
                var elemento = this.InfoSelector.LeerElemento(x);
                if (!elemento.EsVacio())
                    this.mapearElementoAlHtmlSelector(elemento);
                else
                    MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `Se ha leido mal el elemento del selector ${this.IdGrid} de la posición ${x}`);
            }
            this.CerrarModalDeSeleccion();
        }
        BlanquearListaDeSeleccionados() {
            if (this.Selector.hasAttribute(atSelector.ListaDeSeleccionados))
                this.Selector.setAttribute(atSelector.ListaDeSeleccionados, '');
        }
        TextoSelectorCambiado() {
            this.EditorDelGrid.value = this.Selector.value;
            let url = this.DefinirPeticionLeerParaSelector();
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.EndPoint.Leer, this, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, this.TratarValoresDevuelto, null);
            a.Ejecutar();
        }
        DefinirPeticionLeerParaSelector() {
            var controlador = this.Navegador.Controlador;
            let filtroJson = this.ObtenerClausulaParaSelector();
            let url = `/${controlador}/${Ajax.EndPoint.Leer}`;
            let parametros = `${Ajax.Param.filtro}=${filtroJson}`;
            let peticion = url + '?' + parametros;
            return peticion;
        }
        TratarValoresDevuelto(peticion) {
            let modal = peticion.DatosDeEntrada;
            modal.ProcesarRegistrosLeidos(peticion.resultado.datos);
        }
        ObtenerClausulaParaSelector() {
            var clausula = this.ClausulaSegunLoEscrito();
            var clausulas = new Array();
            clausulas.push(clausula);
            return JSON.stringify(clausulas);
        }
        ClausulaSegunLoEscrito() {
            var propiedad = this.Selector.getAttribute(atSelector.popiedadBuscar);
            var criterio = this.Selector.getAttribute(atSelector.criterioBuscar);
            var valor = this.Selector.value.trim();
            var clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);
            return clausula;
        }
        ;
        ProcesarRegistrosLeidos(registros) {
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
        mapearElementoAlHtmlSelector(elemento) {
            this.EditorDelGrid.value = '';
            var valorDelSelector = this.Selector.value;
            if (!IsNullOrEmpty(valorDelSelector))
                valorDelSelector = valorDelSelector + " | ";
            this.Selector.value = valorDelSelector + elemento.Texto;
            this.mapearIdAlHtmlSelector(elemento.Id);
        }
        mapearIdAlHtmlSelector(id) {
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
    Crud.ModalSeleccion = ModalSeleccion;
})(Crud || (Crud = {}));
//# sourceMappingURL=ModalDeSeleccion.js.map