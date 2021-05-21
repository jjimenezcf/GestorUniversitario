var Crud;
(function (Crud) {
    class ClausulaDeOrdenacion {
        constructor(ordenarPor, modo) {
            this.ordenarPor = ordenarPor;
            this.modo = modo;
        }
    }
    class ResultadoDeLectura {
    }
    class DatosPeticionNavegarGrid {
        constructor(grid, accion, posicion) {
            this._grid = grid;
            this._accion = accion;
            this._posicion = posicion;
        }
        get Grid() {
            return this._grid;
        }
        get Accion() {
            return this._accion;
        }
        get PosicionDesdeLaQueSeLee() {
            return this._posicion;
        }
    }
    Crud.DatosPeticionNavegarGrid = DatosPeticionNavegarGrid;
    class DatosPeticionFiltrarPorId {
        constructor(grid, id) {
            this._grid = grid;
            this._id = id;
        }
        get Grid() {
            return this._grid;
        }
        get Accion() {
            return atGrid.accion.buscar;
        }
        get PosicionDesdeLaQueSeLee() {
            return 0;
        }
        get Id() {
            return this._id;
        }
    }
    Crud.DatosPeticionFiltrarPorId = DatosPeticionFiltrarPorId;
    class InfoNavegador {
    }
    class Navegador {
        constructor(idGrid) {
            this.id = `${idGrid}_${atGrid.idCtrlCantidad}`;
            this.idInfo = `${idGrid}_${atGrid.idInfo}`;
            this.idMensaje = `${idGrid}_${atGrid.idMensaje}`;
        }
        get EsRestauracion() {
            return this.esRestauracion;
        }
        set EsRestauracion(valor) {
            this.esRestauracion = valor;
        }
        get Cantidad() {
            return Numero(this.Navegador.value);
        }
        get Posicion() {
            return Numero(this.Navegador.getAttribute(atGrid.navegador.posicion));
        }
        get Pagina() {
            return Numero(this.Navegador.getAttribute(atGrid.navegador.pagina));
        }
        get Leidos() {
            return Numero(this.Navegador.getAttribute(atGrid.navegador.leidos));
        }
        get Total() {
            return Numero(this.Navegador.getAttribute(atGrid.navegador.total));
        }
        get Id() {
            return this.id;
        }
        set Cantidad(valor) {
            this.Navegador.value = valor.toString();
        }
        set Posicion(valor) {
            this.Navegador.setAttribute(atGrid.navegador.posicion, valor.toString());
        }
        set Pagina(valor) {
            this.Navegador.setAttribute(atGrid.navegador.pagina, valor.toString());
        }
        set Info(valor) {
            let div = document.getElementById(this.idInfo);
            div.innerHTML = valor;
        }
        set Mensaje(valor) {
            let div = document.getElementById(this.idMensaje);
            div.innerHTML = valor;
        }
        set Leidos(valor) {
            this.Navegador.setAttribute(atGrid.navegador.leidos, valor.toString());
        }
        set Total(valor) {
            this.Navegador.setAttribute(atGrid.navegador.total, valor.toString());
        }
        set Id(valor) {
            this.id = valor;
        }
        get Navegador() {
            let input = document.getElementById(this.Id);
            return input;
        }
        get Controlador() {
            return this.Navegador.getAttribute(atControl.controlador);
        }
        get Datos() {
            let datos = new InfoNavegador();
            datos.cantidad = this.Cantidad;
            datos.leidos = this.Leidos;
            datos.pagina = this.Pagina;
            datos.posicion = this.Posicion;
            datos.total = this.Total;
            return datos;
        }
        RestaurarDatos(datos) {
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
        Actualizar(accion, posicionDesdeLaQueSeLeyo, registrosLeidos, seleccionados) {
            this.Leidos = registrosLeidos;
            this.Posicion = accion == atGrid.accion.ultima ? this.Total - registrosLeidos : posicionDesdeLaQueSeLeyo + registrosLeidos;
            let paginasTotales = Math.ceil(this.Total / this.Cantidad);
            let paginaAnterior = this.Pagina;
            let paginaNueva = 1;
            if (accion === atGrid.accion.siguiente)
                paginaNueva = paginaAnterior + 1;
            else if (accion === atGrid.accion.anterior)
                paginaNueva = paginaAnterior - 1;
            else if (accion === atGrid.accion.restaurar)
                paginaNueva = paginaAnterior;
            else if (accion === atGrid.accion.ultima) {
                posicionDesdeLaQueSeLeyo = this.Total - registrosLeidos;
                paginaNueva = (this.Cantidad >= this.Total) ? 1 : paginasTotales;
            }
            this.Pagina = paginaNueva <= 0 ? 1 : paginaNueva;
            this.Info = `Pagina ${this.Pagina} de ${paginasTotales}`;
            this.InformarElementosSeleccionados(seleccionados);
        }
        InformarElementosSeleccionados(seleccionados) {
            this.Mensaje = `Seleccionados ${seleccionados} de ${this.Total}`;
        }
    }
    class PaginaDelGrid {
        constructor(pagina, posicion, cantidad, registros, expresionMostrar) {
            this._elementos = [];
            this.anadirElementos(registros, expresionMostrar);
            this._pagina = pagina;
            this._cantidad = cantidad;
            this._posicion = posicion;
            this._fecha = new Date(Date.now());
        }
        get fecha() {
            return this._fecha.toISOString();
        }
        get Pagina() {
            return this._pagina;
        }
        get Posicion() {
            return this._posicion;
        }
        get Cantidad() {
            return this._cantidad;
        }
        get Elementos() {
            return this._elementos;
        }
        get Registros() {
            let registros = [];
            for (let i = 0; i < this.Elementos.length; i++) {
                let registro = this.Elementos[i].Registro;
                registros.push(registro);
            }
            return registros;
        }
        ;
        Obtener(id) {
            for (let i = 0; i < this._elementos.length; i++) {
                if (this._elementos[i].Id === id)
                    return this._elementos[i];
            }
            return null;
        }
        anadirElementos(registros, expresionMostrar) {
            for (let i = 0; i < registros.length; i++) {
                let e = new Elemento(registros[i], expresionMostrar);
                this._elementos.push(e);
            }
        }
    }
    class DatosDelGrid {
        constructor() {
            this._paginas = [];
        }
        set PaginaActual(numeroDePagina) {
            this._paginaActual = numeroDePagina;
        }
        AnadirPagina(numeroDePagina, posicion, cantidad, registros, expresionMostrar) {
            let i = this.Buscar(numeroDePagina);
            if (i >= 0) {
                this._paginas.splice(i, 1);
            }
            let p = new PaginaDelGrid(numeroDePagina, posicion, cantidad, registros, expresionMostrar);
            this._paginas.push(p);
        }
        InicializarCache() {
            this._paginas.splice(0, this._paginas.length);
        }
        Pagina(numeroDePagina) {
            let i = this.Buscar(numeroDePagina);
            if (i >= 0) {
                return this._paginas[i];
            }
            return null;
        }
        Obtener(id) {
            let p = this.Pagina(this._paginaActual);
            if (p === null)
                throw Error(`la página ${this._paginaActual} no se encuentra en la lista de páginas del grid`);
            let e = p.Obtener(id);
            if (e === null)
                throw Error(`El elemento con id ${id} no se encuentra en la página actual del grid`);
            return e;
        }
        Buscar(numeroDePagina) {
            for (let i = 0; i < this._paginas.length; i++)
                if (this._paginas[i].Pagina === numeroDePagina)
                    return i;
            return -1;
        }
    }
    class GridDeDatos extends Crud.CrudBase {
        constructor(idPanelMnt) {
            super();
            this.DatosDelGrid = new DatosDelGrid();
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
        get InfoSelector() {
            return this._infoSelector;
        }
        get IdGrid() {
            return this._idGrid;
        }
        get EsModalDeSeleccion() {
            return this.constructor.name === Crud.ModalSeleccion.name;
        }
        get EsModalParaConsultarRelaciones() {
            return this.constructor.name === Crud.ModalParaConsultarRelaciones.name;
        }
        get EsModalParaRelacionar() {
            return this.constructor.name === Crud.ModalParaRelacionar.name;
        }
        get EsModalParaSeleccionar() {
            return this.constructor.name === Crud.ModalParaSeleccionar.name;
        }
        get EsModalConGrid() {
            return this.EsModalParaRelacionar || this.EsModalDeSeleccion || this.EsModalParaConsultarRelaciones || this.EsModalParaSeleccionar;
        }
        get EsCrud() {
            return EsObjetoDe(this, Crud.CrudMnt);
        }
        get IdCuerpoCabecera() {
            return this._idCuerpoCabecera;
        }
        get CuerpoCabecera() {
            return document.getElementById(this._idCuerpoCabecera);
        }
        get CuerpoDatos() {
            return document.getElementById(`cuerpo.datos.${this._idCuerpoCabecera}`);
        }
        get CuerpoPie() {
            return document.getElementById(`cuerpo.pie.${this._idCuerpoCabecera}`);
        }
        get IdNegocio() {
            return Numero((this.CuerpoCabecera.getAttribute(atMantenimniento.idNegocio)));
        }
        get Negocio() {
            return this.CuerpoCabecera.getAttribute(atMantenimniento.negocio);
        }
        get ZonaDeFiltro() {
            return document.getElementById(this._idHtmlFiltro);
        }
        get EtiquetaMostrarOcultarFiltro() {
            return document.getElementById(`mostrar.${this.IdCuerpoCabecera}.ref`);
        }
        get ExpandirFiltro() {
            return document.getElementById(`expandir.${this.IdCuerpoCabecera}`);
        }
        get InputSeleccionadas() {
            let idInput = this.EsCrud
                ? `div.seleccion.${this.IdGrid}.input`
                : `div.seleccion.${this.IdGrid}.input`;
            return document.getElementById(idInput);
        }
        get EtiquetasSeleccionadas() {
            let idRef = this.EsCrud
                ? `div.seleccion.${this.IdGrid}.ref`
                : `div.seleccion.${this.IdGrid}.ref`;
            return document.getElementById(idRef);
        }
        get Grid() {
            return document.getElementById(this.IdGrid);
        }
        get CabeceraTablaGrid() {
            let idCabecera = this.Grid.getAttribute(atGrid.cabeceraTabla);
            return document.getElementById(idCabecera);
        }
        get CuerpoTablaGrid() {
            return document.getElementById(`${this.Grid.id}_tbody`);
        }
        get ZonaNavegador() {
            let idNavegador = this.Grid.getAttribute(atGrid.zonaNavegador);
            return document.getElementById(idNavegador);
        }
        get Tabla() {
            let idTabla = this.Grid.getAttribute(atControl.tablaDeDatos);
            return document.getElementById(idTabla);
        }
        get ZonaDeMenu() {
            return document.getElementById(this._idHtmlZonaMenu);
        }
        get OpcionesPorElemento() {
            return this.ZonaDeMenu.querySelectorAll(`input[${atOpcionDeMenu.clase}="${ClaseDeOpcioDeMenu.DeElemento}"]`);
        }
        set IdModal(idModal) { this._idModal = idModal; }
        ;
        get IdModal() { return this._idModal; }
        ;
        get Modal() {
            return document.getElementById(this._idModal);
        }
        ;
        Inicializar(idPanelMnt) {
            super.Inicializar(idPanelMnt);
            this.InicializarNavegador();
        }
        InicializarNavegador() {
            let elementos = this.Estado.Obtener("elementos_seleccionados");
            if (elementos !== undefined) {
                for (var i = 0; i < elementos.length; i++) {
                    let e = new Elemento(elementos[i]["_registro"]);
                    this.InfoSelector.InsertarElemento(e);
                }
                this.Estado.Quitar("elementos_seleccionados");
            }
            this.Navegador.RestaurarDatos(this.Estado.Obtener(atGrid.id));
            for (var i = 0; i < this.Ordenacion.Count(); i++) {
                let orden = this.Ordenacion.Leer(i);
                let columna = document.getElementById(orden.IdColumna);
                columna.setAttribute(atControl.modoOrdenacion, orden.Modo);
                let a = columna.getElementsByTagName('a')[0];
                a.setAttribute("class", orden.ccsClase);
            }
        }
        PosicionarGrid() {
            this.Grid.style.position = 'fixed';
            let posicionGrid = this.PosicionGrid();
            this.Grid.style.top = `${posicionGrid}px`;
            let alturaDelGrid = this.AlturaDelGrid(posicionGrid);
            this.Grid.style.height = `${alturaDelGrid}px`;
            let cuerpoDeLaTabla = this.CuerpoTablaGrid;
            if (cuerpoDeLaTabla !== null && cuerpoDeLaTabla !== undefined) {
                this.FijarAlturaCuerpoDeLaTabla(alturaDelGrid);
            }
        }
        PosicionGrid() {
            let alturaCabeceraPnlControl = AlturaCabeceraPnlControl();
            let alturaCabeceraMnt = this.CuerpoCabecera.getBoundingClientRect().height;
            let alturaFiltro = 0;
            if (NumeroMayorDeCero(this.ExpandirFiltro.value)) {
                alturaFiltro = this.ZonaDeFiltro.getBoundingClientRect().height;
            }
            return alturaCabeceraPnlControl + alturaCabeceraMnt + alturaFiltro;
        }
        AlturaDelGrid(posicionGrid) {
            let alturaPiePnlControl = AlturaPiePnlControl();
            let alturaZonaNavegador = this.ZonaNavegador.getBoundingClientRect().height;
            return AlturaFormulario() - posicionGrid - alturaPiePnlControl - alturaZonaNavegador;
        }
        /**
         le he puesto -9 ya que le he pintado bordes al cuerpo del grid
         */
        FijarAlturaCuerpoDeLaTabla(alturaDelGrid) {
            let alturaCabecera = this.CabeceraTablaGrid.getBoundingClientRect().height;
            this.CuerpoTablaGrid.style.height = `${alturaDelGrid - alturaCabecera - 9}px`;
        }
        ActualizarNavegadorDelGrid(accion, posicionDesdeLaQueSeLeyo, registrosLeidos) {
            this.Navegador.Actualizar(accion, posicionDesdeLaQueSeLeyo, registrosLeidos, this.InfoSelector.Cantidad);
            //for (var i = 0; i < this.Ordenacion.Count(); i++) {
            //    let orden: ApiControl.Orden = this.Ordenacion.Leer(i);
            //    let columna: HTMLTableHeaderCellElement = document.getElementById(orden.IdColumna) as HTMLTableHeaderCellElement;
            //    columna.setAttribute(atControl.modoOrdenacion, orden.Modo);
            //    let a: HTMLElement = columna.getElementsByTagName('a')[0] as HTMLElement;
            //    a.setAttribute("class", orden.ccsClase);
            //}
        }
        EstablecerOrdenacion(idcolumna) {
            let htmlColumna = document.getElementById(idcolumna);
            let modo = htmlColumna.getAttribute(atControl.modoOrdenacion);
            if (IsNullOrEmpty(modo))
                modo = ModoOrdenacion.ascedente;
            else if (modo === ModoOrdenacion.ascedente)
                modo = ModoOrdenacion.descendente;
            else if (modo === ModoOrdenacion.descendente)
                modo = ModoOrdenacion.sinOrden;
            else if (modo === ModoOrdenacion.sinOrden)
                modo = ModoOrdenacion.ascedente;
            let propiedad = htmlColumna.getAttribute(atControl.propiedad);
            let ordenarPor = htmlColumna.getAttribute(atControl.ordenarPor);
            this.Ordenacion.Actualizar(idcolumna, propiedad, modo, ordenarPor);
        }
        ObtenerOrdenacion() {
            var clausulas = new Array();
            for (var i = 0; i < this.Ordenacion.Count(); i++) {
                let orden = this.Ordenacion.Leer(i);
                clausulas.push(new ClausulaDeOrdenacion(orden.OrdenarPor, orden.Modo));
            }
            return JSON.stringify(clausulas);
        }
        ObtenerFiltroPorId(id) {
            var clausulas = new Array();
            let propiedad = atControl.id;
            let criterio = literal.filtro.criterio.igual;
            let valor = id.toString();
            let clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);
            clausulas.push(clausula);
            return JSON.stringify(clausulas);
        }
        ObtenerFiltros() {
            let arrayIds = this.ObtenerControlesDeFiltro();
            var clausulas = new Array();
            for (let i = 0; i < arrayIds.length; i++) {
                var clausula = null;
                var control = document.getElementById(`${arrayIds[i]}`);
                var tipo = control.getAttribute(TipoControl.Tipo);
                switch (tipo) {
                    case TipoControl.restrictorDeFiltro: {
                        clausula = this.ObtenerClausulaRestrictor(control);
                        ;
                        break;
                    }
                    case TipoControl.Editor: {
                        clausula = this.ObtenerClausulaEditor(control);
                        break;
                    }
                    case TipoControl.Selector: {
                        clausula = this.ObtenerClausulaSelector(control);
                        ;
                        break;
                    }
                    case TipoControl.ListaDeElementos: {
                        clausula = this.ObtenerClausulaListaDeELemento(control);
                        break;
                    }
                    case TipoControl.ListaDinamica: {
                        clausula = this.ObtenerClausulaListaDinamica(control);
                        break;
                    }
                    case TipoControl.Check: {
                        clausula = this.ObtenerClausulaCheck(control);
                        break;
                    }
                    case TipoControl.FiltroEntreFechas: {
                        clausula = this.ObtenerClausulaEntreFechas(control);
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
        FiltrosExcluyentes(clausulas) {
            return clausulas;
        }
        ObtenerControlesDeFiltro() {
            var arrayIds = new Array();
            var arrayHtmlInput = this.ZonaDeFiltro.querySelectorAll(`input[${atControl.filtro}="S"]`);
            for (let i = 0; i < arrayHtmlInput.length; i++) {
                var htmlInput = arrayHtmlInput[i];
                var id = htmlInput.getAttribute(atControl.id);
                if (id === null)
                    console.log(`Falta el atributo id del componente de filtro ${htmlInput}`);
                else
                    arrayIds.push(id);
            }
            var arrayHtmlSelect = this.ZonaDeFiltro.querySelectorAll(`select[${atControl.filtro}="S"]`);
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
        ObtenerClausulaRestrictor(restrictor) {
            let propiedad = restrictor.getAttribute(atControl.propiedad);
            let criterio = literal.filtro.criterio.igual;
            let valor = restrictor.getAttribute(atControl.restrictor);
            let clausula = null;
            if (!IsNullOrEmpty(valor))
                //clausula = { propiedad: `${propiedad}`, criterio: `${criterio}`, valor: `${valor}` };
                clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);
            return clausula;
        }
        ObtenerClausulaEditor(editor) {
            var propiedad = editor.getAttribute(atControl.propiedad);
            var criterio = editor.getAttribute(atControl.criterio);
            var valor = editor.value;
            var clausula = null;
            if (!IsNullOrEmpty(valor))
                //clausula = { propiedad: `${propiedad}`, criterio: `${criterio}`, valor: `${valor}` };
                clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);
            return clausula;
        }
        ObtenerClausulaSelector(selector) {
            var propiedad = selector.getAttribute(atControl.propiedad);
            var criterio = selector.getAttribute(atControl.criterio);
            var valor = null;
            var clausula = null;
            if (selector.hasAttribute(atSelectorDeFiltro.ListaDeSeleccionados)) {
                var ids = selector.getAttribute(atSelectorDeFiltro.ListaDeSeleccionados);
                if (!NoDefinida(ids)) {
                    valor = ids;
                    clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);
                }
            }
            return clausula;
        }
        ObtenerClausulaListaDinamica(input) {
            var propiedad = input.getAttribute(atControl.propiedad);
            var criterio = literal.filtro.criterio.igual;
            let valor = Numero(input.getAttribute(atListasDinamicas.idSeleccionado));
            var clausula = null;
            if (Number(valor) > 0) {
                clausula = new ClausulaDeFiltrado(propiedad, criterio, valor.toString());
            }
            return clausula;
        }
        ObtenerClausulaCheck(input) {
            let propiedad = input.getAttribute(atControl.propiedad);
            let criterio = literal.filtro.criterio.igual;
            let filtrarPorFalse = input.getAttribute(atCheck.filtrarPorFalse);
            let valor = input.checked;
            var clausula = null;
            if (valor || (filtrarPorFalse === "S" && !valor))
                clausula = new ClausulaDeFiltrado(propiedad, criterio, valor.toString());
            return clausula;
        }
        ObtenerClausulaEntreFechas(input) {
            let propiedad = input.getAttribute(atControl.propiedad);
            let criterio = literal.filtro.criterio.entreFechas;
            let valor = ApiControl.LeerEntreFechas(input);
            var clausula = null;
            if (valor.trim() !== '-') {
                clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);
            }
            return clausula;
        }
        ObtenerClausulaListaDeELemento(selet) {
            var propiedad = selet.getAttribute(atControl.propiedad);
            var criterio = atCriterio.igual;
            var valor = selet.value;
            var clausula = null;
            if (!IsNullOrEmpty(valor) && Number(valor) > 0) {
                clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);
            }
            return clausula;
        }
        ObtenerlaFila(idCheck) {
            let idFila = idCheck.replace(".chksel", "");
            let fila = document.getElementById(idFila);
            return fila;
        }
        ActualizarInfoSelector(grid, elemento) {
            grid.InfoSelector.Quitar(elemento.Id);
            elemento = grid.DatosDelGrid.Obtener(elemento.Id);
            grid.InfoSelector.InsertarElemento(elemento);
            grid.Navegador.InformarElementosSeleccionados(grid.InfoSelector.Cantidad);
            grid.AplicarModoAccesoAlElemento(elemento);
        }
        AnadirAlInfoSelector(grid, elemento) {
            grid.InfoSelector.InsertarElemento(elemento);
            grid.Navegador.InformarElementosSeleccionados(grid.InfoSelector.Cantidad);
            grid.AplicarModoAccesoAlElemento(elemento);
        }
        QuitarDelSelector(grid, id) {
            grid.InfoSelector.Quitar(id);
            grid.Navegador.InformarElementosSeleccionados(grid.InfoSelector.Cantidad);
        }
        EstaMarcado(idCheck) {
            let id = this.ObtenerElIdDelElementoDelaFila(idCheck);
            return this.InfoSelector.Buscar(id) >= 0 ? true : false;
        }
        ObtenerElIdDelElementoDelaFila(idCheck) {
            let columnaId = idCheck.replace(".chksel", `.${literal.id}`);
            let inputId = document.getElementById(columnaId);
            let id = inputId.value;
            return Numero(id);
        }
        MarcarElementos() {
            if (this.InfoSelector.Cantidad === 0)
                return;
            var celdasId = document.getElementsByName(`${literal.id}.${this.IdGrid}`);
            var len = celdasId.length;
            for (var i = this.InfoSelector.Cantidad - 1; i >= 0; i--) {
                let elemento = this.InfoSelector.LeerElemento(i);
                for (var j = 0; j < len; j++) {
                    if (Numero(celdasId[j].value) == elemento.Id) {
                        var idCheck = celdasId[j].id.replace(`.${atControl.id}`, LiteralMnt.postfijoDeCheckDeSeleccion);
                        var check = document.getElementById(idCheck);
                        check.checked = true;
                        this.ActualizarInfoSelector(this, elemento);
                        break;
                    }
                }
            }
        }
        BlanquearTodosLosCheck() {
            var celdasId = document.getElementsByName(`${literal.id}.${this.IdGrid}`);
            var len = celdasId.length;
            for (var j = 0; j < len; j++) {
                var idCheck = celdasId[j].id.replace(`.${atControl.id}`, LiteralMnt.postfijoDeCheckDeSeleccion);
                var check = document.getElementById(idCheck);
                check.checked = false;
            }
            this.InfoSelector.QuitarTodos();
        }
        ActualizarInformacionDelGrid(grid) {
            if (!grid.EsModalConGrid && grid.Estado.Contiene(atGrid.idSeleccionado)) {
                let idSeleccionado = Numero(grid.Estado.Obtener(atGrid.idSeleccionado));
                let elemento = this.DatosDelGrid.Obtener(idSeleccionado);
                grid.AnadirAlInfoSelector(grid, elemento);
                grid.Estado.Quitar(atGrid.idSeleccionado);
                grid.Estado.Quitar(atGrid.nombreSeleccionado);
            }
            grid.MarcarElementos();
            grid.InfoSelector.SincronizarCheck();
            //if (grid.InfoSelector.Cantidad > 0)
            //    grid.AccederAlModoDeAccesoAlElemento(this.InfoSelector.LeerElemento(0).Id);
        }
        obtenerValorDeLaFilaParaLaPropiedad(id, propiedad) {
            let fila = this.ObtenerFila(id);
            if (fila === null)
                return null;
            let celda = this.ObtenerCelda(fila, propiedad);
            let input = celda.querySelector("input");
            if (input === null)
                throw new Error(`la celda asociada a la propiedad '${propiedad}' no tiene un control input definido`);
            return input.value;
        }
        ObtenerFila(id) {
            let tabla = this.Tabla;
            for (var i = 0; i < tabla.rows.length; i++) {
                let fila = tabla.rows[i];
                for (var j = 0; j < fila.cells.length; j++) {
                    let celda = fila.cells[j];
                    let input = celda.querySelector("input");
                    if (input !== null) {
                        let propiedad = input.getAttribute(atControl.propiedad);
                        if (propiedad.toLocaleLowerCase() === atControl.id) {
                            let valor = input.value;
                            if (Numero(valor) == id)
                                return fila;
                        }
                    }
                }
            }
            return null;
            //throw new Error(`No se ha localizado una fila con la propiedad Id definida`);
        }
        ObtenerCelda(fila, propiedadBuscada) {
            for (var j = 0; j < fila.cells.length; j++) {
                let celda = fila.cells[j];
                let propiedadCelda = celda.getAttribute(atControl.propiedad);
                if (propiedadCelda.toLocaleLowerCase() === propiedadBuscada)
                    return celda;
            }
            throw new Error(`No se ha localizado una celda con la propiedad '${propiedadBuscada}' definida`);
        }
        AntesDeNavegar(valores) {
            super.AntesDeNavegar(valores);
            this.Estado.Agregar(atGrid.id, this.Navegador.Datos);
            let datosRestrictor = valores.Obtener(Sesion.restrictor);
            let idSeleccionado = valores.Obtener(Sesion.idSeleccionado);
            this.Estado.Agregar(atGrid.idSeleccionado, idSeleccionado);
            this.Estado.Agregar(atGrid.nombreSeleccionado, datosRestrictor.Texto);
            let paginaDestino = valores.Obtener(Sesion.paginaDestino);
            let estadoPaginaDestino = EntornoSe.Historial.ObtenerEstadoDePagina(paginaDestino);
            estadoPaginaDestino.Agregar(Sesion.restrictor, datosRestrictor);
            estadoPaginaDestino.Quitar(atGrid.idSeleccionado);
            estadoPaginaDestino.Quitar(atGrid.nombreSeleccionado);
            EntornoSe.Historial.GuardarEstadoDePagina(estadoPaginaDestino);
        }
        IrAlCrudDeDependencias(parametrosDeEntrada) {
            try {
                let datos = this.PrepararParametrosDeDependencias(this._infoSelector, parametrosDeEntrada);
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
        IrAlCrudDeRelacionarCon(parametrosDeEntrada) {
            try {
                let datos = this.PrepararParametrosDeRelacionarCon(this._infoSelector, parametrosDeEntrada);
                if (datos.FiltroRestrictor !== null)
                    ApiRuote.NavegarARelacionar(this, datos.idOpcionDeMenu, datos.idSeleccionado, datos.FiltroRestrictor);
            }
            catch (error) {
                MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, error.message);
                return;
            }
        }
        PrepararParametrosDeDependencias(infoSelector, parametros) {
            if (infoSelector.Cantidad != 1)
                throw new Error("Debe seleccionar un elemento para poder gestionar sus dependencias");
            let partes = parametros.split('#');
            if (partes.length != 4)
                throw new Error("Los parámetros de dependencias están mal definidos");
            let elemento = infoSelector.LeerElemento(0);
            let datos = new Tipos.DatosParaDependencias();
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
                let idRestrictor = Numero(valorDeLaColumna);
                let filtro = new Tipos.DatosRestrictor(datos.PropiedadRestrictora, idRestrictor, datos.MostrarEnElRestrictor);
                datos.FiltroRestrictor = filtro;
            }
            return datos;
        }
        PrepararParametrosDeRelacionarCon(infoSelector, parametros) {
            if (infoSelector.Cantidad != 1)
                throw new Error("Debe seleccionar un elemento para poder relacionarlo");
            let partes = parametros.split('#');
            if (partes.length != 4)
                throw new Error("Los parámetros de relación están mal definidos");
            let elemento = infoSelector.LeerElemento(0);
            let datos = new Tipos.DatosParaRelacionar();
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
                let idRestrictor = Numero(valorDeLaColumna);
                let filtro = new Tipos.DatosRestrictor(datos.PropiedadRestrictora, idRestrictor, datos.MostrarEnElRestrictor);
                datos.FiltroRestrictor = filtro;
            }
            return datos;
        }
        LeerElementoParaGestionarSusDependencias(datos) {
            let url = `/${this.Controlador}/${Ajax.EndPoint.LeerPorId}?${Ajax.Param.id}=${datos.idSeleccionado}`;
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.EndPoint.LeerPorId, datos, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, this.TrasLeerNavegarParaGestionarSusDependencias, null);
            a.Ejecutar();
        }
        TrasLeerNavegarParaGestionarSusDependencias(peticion) {
            let grid = peticion.llamador;
            let datos = peticion.DatosDeEntrada;
            let idRestrictor = Numero(peticion.resultado.datos[datos.PropiedadQueRestringe]);
            let filtro = new Tipos.DatosRestrictor(datos.PropiedadRestrictora, idRestrictor, datos.MostrarEnElRestrictor);
            datos.FiltroRestrictor = filtro;
            ApiRuote.NavegarADependientes(grid, datos.idOpcionDeMenu, datos.idSeleccionado, datos.FiltroRestrictor);
        }
        LeerElementoParaRelacionar(datos) {
            let url = `/${this.Controlador}/${Ajax.EndPoint.LeerPorId}?${Ajax.Param.id}=${datos.idSeleccionado}`;
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.EndPoint.LeerPorId, datos, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, this.TrasLeerNavegarParaRelacionar, null);
            a.Ejecutar();
        }
        TrasLeerNavegarParaRelacionar(peticion) {
            let grid = peticion.llamador;
            let datos = peticion.DatosDeEntrada;
            let idRestrictor = Numero(peticion.resultado.datos[datos.PropiedadQueRestringe]);
            let filtro = new Tipos.DatosRestrictor(datos.PropiedadRestrictora, idRestrictor, datos.MostrarEnElRestrictor);
            datos.FiltroRestrictor = filtro;
            ApiRuote.NavegarARelacionar(grid, datos.idOpcionDeMenu, datos.idSeleccionado, datos.FiltroRestrictor);
        }
        /*
         *
         * métodos para mapear los registros leidos a un dbgrid
         *
         */
        ObtenerUltimos() {
            let total = this.Navegador.Total;
            let cantidad = this.Navegador.Cantidad;
            let ultimaPagina = Math.ceil(total / cantidad);
            if (ultimaPagina <= 1)
                return;
            let posicion = (ultimaPagina - 1) * cantidad;
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
        ObtenerAnteriores() {
            let cantidad = this.Navegador.Cantidad;
            let pagina = this.Navegador.Pagina;
            if (pagina == 1)
                return;
            let posicion = (pagina - 2) * cantidad;
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
        ObtenerSiguientes() {
            let cantidad = this.Navegador.Cantidad;
            let pagina = this.Navegador.Pagina;
            let total = this.Navegador.Total;
            let posicion = pagina * cantidad;
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
        CargarGrid(accion, posicion) {
            if (this.Grid.getAttribute(atGrid.cargando) == 'S')
                return;
            let url = this.DefinirPeticionDeBusqueda(Ajax.EndPoint.LeerDatosParaElGrid, accion, posicion);
            var datosDePeticion = new DatosPeticionNavegarGrid(this, accion, posicion);
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.EndPoint.LeerDatosParaElGrid, datosDePeticion, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, this.CrearFilasEnElGrid, this.SiHayErrorAlCargarElGrid);
            this.Grid.setAttribute(atGrid.cargando, 'S');
            a.Ejecutar();
        }
        FiltrarPorId(id) {
            if (this.Grid.getAttribute(atGrid.cargando) == 'S') {
                MensajesSe.Error("FiltrarPorId", "El grid se está cargando");
                return null;
            }
            let promesa = new Promise((resolve, reject) => {
                let url = this.DefinirFiltroPorId(id);
                var datosDePeticion = new DatosPeticionFiltrarPorId(this, id);
                let a = new ApiDeAjax.DescriptorAjax(this, Ajax.EndPoint.LeerDatosParaElGrid, datosDePeticion, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, (peticion) => {
                    resolve(this.CrearFilaDelIdEnElGrid(peticion));
                }, (peticion) => {
                    this.SiHayErrorAlCargarElGrid(peticion);
                    reject(false);
                });
                this.Grid.setAttribute(atGrid.cargando, 'S');
                a.Ejecutar();
            });
            return promesa;
        }
        DefinirFiltroPorId(id) {
            var controlador = this.Navegador.Controlador;
            var filtroJson = this.ObtenerFiltroPorId(id);
            let url = `/${controlador}/${Ajax.EndPoint.LeerDatosParaElGrid}`;
            let parametros = `${Ajax.Param.modo}=Mantenimiento` +
                `&${Ajax.Param.accion}=${atGrid.accion.buscar}` +
                `&${Ajax.Param.posicion}=${0}` +
                `&${Ajax.Param.cantidad}=${1}` +
                `&${Ajax.Param.filtro}=${filtroJson}` +
                `&${Ajax.Param.orden}=${[]}`;
            let peticion = url + '?' + parametros;
            return peticion;
        }
        PromesaDeCargarGrid(accion, posicion) {
            if (this.Grid.getAttribute(atGrid.cargando) == 'S')
                return null;
            let promesa = new Promise((resolve, reject) => {
                let url = this.DefinirPeticionDeBusqueda(Ajax.EndPoint.LeerDatosParaElGrid, accion, posicion);
                var datosDePeticion = new DatosPeticionNavegarGrid(this, accion, posicion);
                let a = new ApiDeAjax.DescriptorAjax(this, Ajax.EndPoint.LeerDatosParaElGrid, datosDePeticion, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, (peticion) => {
                    resolve(this.CrearFilasEnElGrid(peticion));
                }, (peticion) => {
                    this.SiHayErrorAlCargarElGrid(peticion);
                    reject(false);
                });
                this.Grid.setAttribute(atGrid.cargando, 'S');
                a.Ejecutar();
            });
            return promesa;
        }
        DefinirPeticionDeBusqueda(endPoint, accion, posicion) {
            var posicion = posicion;
            var cantidad = this.Navegador.Cantidad;
            var controlador = this.Navegador.Controlador;
            var filtroJson = this.ObtenerFiltros();
            var ordenJson = this.ObtenerOrdenacion();
            let url = `/${controlador}/${endPoint}`;
            let parametros = `${Ajax.Param.modo}=Mantenimiento` +
                `&${Ajax.Param.accion}=${accion}` +
                `&${Ajax.Param.posicion}=${posicion}` +
                `&${Ajax.Param.cantidad}=${cantidad}` +
                `&${Ajax.Param.filtro}=${filtroJson}` +
                `&${Ajax.Param.orden}=${ordenJson}`;
            let peticion = url + '?' + parametros;
            return peticion;
        }
        SiHayErrorAlCargarElGrid(peticion) {
            let grid = peticion.llamador;
            try {
                MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, peticion.resultado.mensaje);
            }
            finally {
                grid.Grid.setAttribute(atGrid.cargando, 'N');
            }
        }
        CrearFilaDelIdEnElGrid(peticion) {
            let datosDeEntrada = peticion.DatosDeEntrada;
            let grid = datosDeEntrada.Grid;
            grid.Estado.Agregar(atGrid.idSeleccionado, datosDeEntrada.Id);
            return this.CrearFilasEnElGrid(peticion);
        }
        CrearFilasEnElGrid(peticion) {
            let datosDeEntrada = peticion.DatosDeEntrada;
            let grid = datosDeEntrada.Grid;
            let lineasCreadas = true;
            try {
                let infoObtenida = peticion.resultado.datos;
                var registros = infoObtenida.registros;
                if (datosDeEntrada.Accion === atGrid.accion.buscar)
                    grid.Navegador.Total = infoObtenida.total;
                grid.ActualizarNavegadorDelGrid(datosDeEntrada.Accion, datosDeEntrada.PosicionDesdeLaQueSeLee, registros.length);
                let expresionMostrar = grid.Grid.getAttribute(atControl.expresionElemento).toLowerCase();
                grid.DatosDelGrid.AnadirPagina(grid.Navegador.Pagina, datosDeEntrada.PosicionDesdeLaQueSeLee, grid.Navegador.Cantidad, infoObtenida.registros, expresionMostrar);
                grid.MapearPaginaCacheada(grid, registros);
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
                ApiGrid.RecalcularAnchoColumnas(grid.Tabla);
            }
            return lineasCreadas;
        }
        MapearPaginaCacheada(grid, registros) {
            var cuerpo = grid.CrearCuerpoDeLaTabla(grid, registros);
            grid.AnadirCuerpoALaTabla(grid, cuerpo);
            grid.ActualizarInformacionDelGrid(grid);
            grid.AjustarTamanoDelCuerpoDeLaTabla(grid, cuerpo);
            grid.AplicarQueFilasMostrar(grid.InputSeleccionadas, grid.CuerpoTablaGrid, grid.InfoSelector);
        }
        CrearCuerpoDeLaTabla(grid, registros) {
            let filaCabecera = ApiGrid.ObtenerDescriptorDeLaCabecera(grid.Tabla);
            let cuerpoDeLaTabla = document.createElement("tbody");
            cuerpoDeLaTabla.id = `${grid.Grid.id}_tbody`;
            cuerpoDeLaTabla.classList.add(ClaseCss.cuerpoDeLaTabla);
            for (let i = 0; i < registros.length; i++) {
                let fila = grid.crearFila(filaCabecera, registros[i], i);
                cuerpoDeLaTabla.append(fila);
            }
            return cuerpoDeLaTabla;
        }
        AnadirCuerpoALaTabla(grid, cuerpoDeLaTabla) {
            let tabla = grid.Grid.querySelector("table");
            let tbody = tabla.querySelector("tbody");
            if (!(tbody === null || tbody === undefined))
                tabla.removeChild(tbody);
            tabla.append(cuerpoDeLaTabla);
            grid.DatosDelGrid.PaginaActual = grid.Navegador.Pagina;
        }
        AjustarTamanoDelCuerpoDeLaTabla(grid, cuerpoDeLaTabla) {
            if (grid.EsCrud) {
                let posicion = grid.PosicionGrid();
                let altura = grid.AlturaDelGrid(posicion);
                grid.FijarAlturaCuerpoDeLaTabla(altura);
            }
            else {
                cuerpoDeLaTabla.style.height = `${AlturaFormulario() / 3}px`;
            }
        }
        crearFila(filaCabecera, registro, numeroDeFila) {
            let fila = document.createElement("tr");
            fila.id = `${this.IdGrid}_d_tr_${numeroDeFila}`;
            fila.classList.add(ClaseCss.filaDelGrid);
            let idDelElemento = 0;
            for (let j = 0; j < filaCabecera.length; j++) {
                let columnaCabecera = filaCabecera[j];
                let valor = this.BuscarValorDeColumnaRegistro(registro, columnaCabecera.propiedad);
                if (columnaCabecera.propiedad === atControl.id) {
                    if (!IsNumber(valor))
                        throw new Error("El id del elemento leido debe ser numérico");
                    idDelElemento = Numero(valor);
                }
                let celdaDelTd = this.crearCelda(fila, columnaCabecera, j, valor);
                fila.append(celdaDelTd);
            }
            fila.setAttribute(atControl.valorTr, idDelElemento.toString());
            return fila;
        }
        crearCelda(fila, columnaCabecera, numeroDeCelda, valor) {
            let celdaDelTd = document.createElement("td");
            celdaDelTd.id = `${fila.id}.${numeroDeCelda}`;
            celdaDelTd.headers = `${columnaCabecera.id}`;
            celdaDelTd.setAttribute(atControl.nombre, `td.${columnaCabecera.propiedad}.${this.IdGrid}`);
            celdaDelTd.setAttribute(atControl.propiedad, `${columnaCabecera.propiedad}`);
            celdaDelTd.style.textAlign = columnaCabecera.estilo.textAlign;
            celdaDelTd.style.width = columnaCabecera.estilo.width;
            if (celdaDelTd.style.textAlign == "right")
                celdaDelTd.style.paddingRight = "10px";
            let idCheckDeSeleccion = `${fila.id}.chksel`;
            let eventoOnClick = this.definirPulsarCheck(idCheckDeSeleccion, celdaDelTd.id);
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
        definirPulsarCheck(idCheckDeSeleccion, idControlHtml) {
            let a = '';
            if (this.EsModalDeSeleccion) {
                let idModal = this.Grid.getAttribute(atSelectorDeFiltro.idModal);
                a = `${GestorDeEventos.deSeleccionDeFiltro}('fila-pulsada', '${idModal}#${idCheckDeSeleccion}#${idControlHtml}');`;
            }
            else if (this.EsModalParaRelacionar) {
                let idModal = this.Grid.getAttribute(atSelectorDeFiltro.idModal);
                a = `${GestorDeEventos.deCrearRelaciones}('fila-pulsada', '${idModal}#${idCheckDeSeleccion}#${idControlHtml}');`;
            }
            else if (this.EsModalParaSeleccionar) {
                let idModal = this.Grid.getAttribute(atSelectorDeFiltro.idModal);
                a = `${GestorDeEventos.paraSeleccionarElementos}('${Evento.ModalParaSeleccionarElementos.FilaPulsada}', '${idModal}#${idCheckDeSeleccion}#${idControlHtml}');`;
            }
            else if (this.EsModalParaConsultarRelaciones) {
                let idModal = this.Grid.getAttribute(atSelectorDeFiltro.idModal);
                a = `${GestorDeEventos.deConsultaDeRelaciones}('fila-pulsada', '${idModal}#${idCheckDeSeleccion}#${idControlHtml}');`;
            }
            else if (this.EsCrud)
                a = `${GestorDeEventos.delMantenimiento}('fila-pulsada', '${idCheckDeSeleccion}#${idControlHtml}');`;
            else
                throw new Error("No se ha definido el gestor de eventos a asociar a la pulsación de una fila en el grid");
            return a;
        }
        insertarInputEnElTd(idFila, columnaCabecera, celdaDelTd, valor) {
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
            let eventoOnClick = this.definirPulsarCheck(idCheckBox, input.id);
            celdaDelTd.setAttribute(atControl.eventoJs.onclick, eventoOnClick);
            input.readOnly = true;
            input.hidden = celdaDelTd.hidden;
            input.value = valor;
            celdaDelTd.append(input);
        }
        insertarCheckEnElTd(idFila, celdaDelTd, propiedad) {
            let checkbox = document.createElement('input');
            checkbox.type = "checkbox";
            checkbox.id = `${idFila}.${propiedad}`;
            checkbox.name = `${propiedad}.${this.IdGrid}`;
            checkbox.setAttribute(atControl.propiedad, `${propiedad}`);
            checkbox.style.border = "0px";
            checkbox.style.textAlign = "center";
            checkbox.style.width = "100%";
            checkbox.style.backgroundColor = "inherit";
            let eventoOnClick = this.definirPulsarCheck(checkbox.id, checkbox.id);
            celdaDelTd.setAttribute(atControl.eventoJs.onclick, eventoOnClick);
            checkbox.value = literal.false;
            celdaDelTd.append(checkbox);
        }
        obtenerDescriptorDeLaCabecera(grid) {
            let filaCabecera = new Array();
            var cabecera = grid.Tabla.rows[0];
            var ths = cabecera.querySelectorAll('th');
            for (let i = 0; i < ths.length; i++) {
                let p = new ApiGrid.PropiedadesDeLaFila();
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
        BuscarValorDeColumnaRegistro(registro, propiedadDeLaFila) {
            for (const propiedad in registro) {
                if (propiedad.toLowerCase() === propiedadDeLaFila)
                    return registro[propiedad];
            }
            return "";
        }
        FilaPulsada(idCheck, idDelInput) {
            let check = document.getElementById(idCheck);
            //Se hace porque antes ha pasado por aquí por haber pulsado en la fila
            if (idCheck !== idDelInput) {
                check.checked = !check.checked;
            }
            let id = this.ObtenerElIdDelElementoDelaFila(idCheck);
            if (check.checked) {
                let elemento = this.DatosDelGrid.Obtener(id);
                this.AnadirAlInfoSelector(this, elemento);
            }
            else {
                this.QuitarDelSelector(this, id);
                for (let i = 0; i < this.InfoSelector.Cantidad; i++) {
                    let e = this.InfoSelector.LeerElemento(i);
                    this.AplicarModoAccesoAlElemento(e);
                }
                if (this.InfoSelector.Cantidad === 0 && (this instanceof Crud.ModalConGrid) === false)
                    this.DeshabilitarOpcionesDeMenuDeElemento();
            }
        }
        AplicarModoAccesoAlElemento(elemento) {
            let modoAcceso = ModoAcceso.Parsear(elemento.ModoDeAcceso);
            //En las modales no hay menús
            if (this.ZonaDeMenu === null)
                return;
            let opcionesDeElemento = this.OpcionesPorElemento;
            let hacerLaInterseccion = this.InfoSelector.Cantidad > 1;
            for (var i = 0; i < opcionesDeElemento.length; i++) {
                let opcion = opcionesDeElemento[i];
                ModoAcceso.AplicarModoAccesoAlElemento(opcion, hacerLaInterseccion, modoAcceso);
            }
        }
        LeerElementoSeleccionado(id) {
            let url = `/${this.Controlador}/${Ajax.EndPoint.LeerPorId}?${Ajax.Param.id}=${id}`;
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.EndPoint.LeerPorId, id, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, this.TrasLeerElementoSeleccionado, null);
            a.Ejecutar();
        }
        TrasLeerElementoSeleccionado(peticion) {
            let grid = peticion.llamador;
            grid.AnadirAlInfoSelector(grid, peticion.resultado.datos);
        }
        DeshabilitarOpcionesDeMenuDeNegocio() {
            let opcionesDeElemento = this.ZonaDeMenu.querySelectorAll(`input[${atOpcionDeMenu.clase}="${ClaseDeOpcioDeMenu.DeVista}"]`);
            for (var i = 0; i < opcionesDeElemento.length; i++) {
                let opcion = opcionesDeElemento[i];
                opcion.disabled = true;
            }
            this.DeshabilitarOpcionesDeMenuDeElemento();
        }
        DeshabilitarOpcionesDeMenuDeElemento() {
            let opcionesDeElemento = this.ZonaDeMenu.querySelectorAll(`input[${atOpcionDeMenu.clase}="${ClaseDeOpcioDeMenu.DeElemento}"]`);
            for (var i = 0; i < opcionesDeElemento.length; i++) {
                let opcion = opcionesDeElemento[i];
                opcion.disabled = true;
            }
        }
        AccederAlModoDeAccesoAlElemento(id) {
            let url = this.DefinirPeticionDeLeerModoDeAccesoAlElemento(id);
            let datosDeEntrada = `{"Negocio":"${this.Negocio}","id":"${id}"}`;
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.EndPoint.LeerModoDeAccesoAlElemento, datosDeEntrada, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, this.AplicarModoDeAccesoAlElemento, this.SiHayErrorTrasPeticionAjax);
            a.Ejecutar();
        }
        AplicarModoDeAccesoAlElemento(peticion) {
            let mantenimiento = peticion.llamador;
            let modoDeAccesoDelUsuario = peticion.resultado.modoDeAcceso;
        }
        DefinirPeticionDeLeerModoDeAccesoAlElemento(id) {
            let url = `/${this.Controlador}/${Ajax.EndPoint.LeerModoDeAccesoAlElemento}`;
            let parametros = `${Ajax.Param.negocio}=${this.Negocio}&${Ajax.Param.id}=${id}`;
            let peticion = url + '?' + parametros;
            return peticion;
        }
        OrdenarPor(columna) {
            this.EstablecerOrdenacion(columna);
            this.DatosDelGrid.InicializarCache();
            this.CargarGrid(atGrid.accion.buscar, 0);
        }
        MostrarSoloSeleccionadas(inputDeSeleccionadas, etiquetaSeleccionadas, tbodyDelGrid, seleccionadas) {
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
        TeclaPulsada(grid, e) {
            if (e.keyCode === 13 && !e.shiftKey) {
                grid.CargarGrid(atGrid.accion.buscar, 0);
                e.preventDefault();
            }
        }
        MostrarAuditoria(idCheck) {
            let check = document.getElementById(idCheck);
            let idFecha = `${this.Grid.id}_c_tr_0.creadoel`;
            let idUsuario = `${this.Grid.id}_c_tr_0.creador`;
            if (check.checked) {
                ApiGrid.ColumnaVisible(this.Tabla, idFecha);
                ApiGrid.ColumnaVisible(this.Tabla, idUsuario);
            }
            else {
                ApiGrid.ColumnaInvisible(this.Tabla, idFecha);
                ApiGrid.ColumnaInvisible(this.Tabla, idUsuario);
            }
            ApiGrid.RecalcularAnchoColumnas(this.Tabla);
        }
        OcultarMostrarColumnas(propiedades) {
            for (let i = 0; i < propiedades.length; i++) {
                ApiGrid.OcultarMostrarColumna(this.Tabla, propiedades[i]);
            }
            ApiGrid.RecalcularAnchoColumnas(this.Tabla);
        }
        AplicarQueFilasMostrar(inputDeSeleccionadas, tbodyDelGrid, seleccionadas) {
            if (NumeroMayorDeCero(inputDeSeleccionadas.value)) {
                this.MostrarFilasSeleccionadas(tbodyDelGrid, seleccionadas);
            }
            else {
                this.MostrarTodasLasFilas(tbodyDelGrid);
            }
        }
        MostrarTodasLasFilas(tbodyDelGrid) {
            let trs = tbodyDelGrid.querySelectorAll("tr");
            let i = 0;
            for (i = 0; i < trs.length; i++) {
                let tr = trs[i];
                tr.hidden = false;
            }
        }
        MostrarFilasSeleccionadas(tbodyDelGrid, seleccionadas) {
            let trs = tbodyDelGrid.querySelectorAll("tr");
            let i = 0;
            for (i = 0; i < trs.length; i++) {
                let tr = trs[i];
                let idDelElemento = Numero(tr.getAttribute(atControl.valorTr));
                tr.hidden = !seleccionadas.Contiene(idDelElemento);
            }
        }
    }
    Crud.GridDeDatos = GridDeDatos;
})(Crud || (Crud = {}));
//# sourceMappingURL=GridDeDatos.js.map