var Crud;
(function (Crud) {
    class ClausulaDeOrdenacion {
        constructor(ordenarPor, modo) {
            this.ordenarPor = ordenarPor;
            this.modo = modo;
        }
    }
    class PropiedadesDeLaFila {
        constructor() {
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
            this.ActualizarMensaje(seleccionados);
        }
        ActualizarMensaje(seleccionados) {
            this.Mensaje = `Seleccionados ${seleccionados} de ${this.Total}`;
        }
    }
    class GridDeDatos extends Crud.CrudBase {
        constructor(idPanelMnt) {
            super();
            this._idCuerpoCabecera = idPanelMnt;
            if (this.CuerpoCabecera === null)
                throw Error(`No se puede crear el Crud ${idPanelMnt}`);
            this._controlador = this.CuerpoCabecera.getAttribute(atMantenimniento.controlador);
            this._idGrid = this.CuerpoCabecera.getAttribute(atMantenimniento.gridDelMnt);
            this._idHtmlZonaMenu = this.CuerpoCabecera.getAttribute(atMantenimniento.zonaMenu);
            this._idHtmlFiltro = this.Grid.getAttribute(atMantenimniento.zonaDeFiltro);
            this._infoSelector = new InfoSelector(this.IdGrid);
            this.Navegador = new Navegador(this.IdGrid);
            this.Ordenacion = new Tipos.Ordenacion();
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
        get EsModalConGrid() {
            return this.EsModalParaRelacionar || this.EsModalDeSeleccion || this.EsModalParaConsultarRelaciones;
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
            //htmlColumna.setAttribute(atControl.modoOrdenacion, modo);
        }
        ObtenerExpresionMostrar(idCheck) {
            let expresion = this.Grid.getAttribute(atControl.expresionElemento).toLowerCase();
            if (!IsNullOrEmpty(expresion)) {
                let fila = this.ObtenerlaFila(idCheck);
                let columnas = fila.getElementsByTagName('td');
                for (let j = 0; j < columnas.length; j++) {
                    let input = columnas[j].getElementsByTagName('input')[0];
                    if (input !== undefined) {
                        let propiedad = input.getAttribute(atControl.propiedad).toLowerCase();
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
        ObtenerOrdenacion() {
            var clausulas = new Array();
            for (var i = 0; i < this.Ordenacion.Count(); i++) {
                let orden = this.Ordenacion.Leer(i);
                clausulas.push(new ClausulaDeOrdenacion(orden.OrdenarPor, orden.Modo));
            }
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
                    default: {
                        Notificar(TipoMensaje.Error, `No está implementado como definir la cláusula de filtrado de un tipo ${tipo}`);
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
            if (selector.hasAttribute(atSelector.ListaDeSeleccionados)) {
                var ids = selector.getAttribute(atSelector.ListaDeSeleccionados);
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
            var propiedad = input.getAttribute(atControl.propiedad);
            var criterio = literal.filtro.criterio.igual;
            let filtrarPorFalse = input.getAttribute(atCheck.filtrarPorFalse);
            let valor = input.checked;
            var clausula = null;
            if (valor || (filtrarPorFalse === "S" && !valor))
                clausula = new ClausulaDeFiltrado(propiedad, criterio, valor.toString());
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
        AnadirAlInfoSelector(grid, id, expresionElemento) {
            grid.InfoSelector.InsertarElemento(id, expresionElemento);
            grid.Navegador.ActualizarMensaje(grid.InfoSelector.Cantidad);
        }
        QuitarDelSelector(grid, id) {
            grid.InfoSelector.Quitar(id);
            grid.Navegador.ActualizarMensaje(grid.InfoSelector.Cantidad);
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
            for (var i = 0; i < this.InfoSelector.Cantidad; i++) {
                for (var j = 0; j < len; j++) {
                    let id = this.InfoSelector.LeerId(i);
                    if (Numero(celdasId[j].value) == id) {
                        var idCheck = celdasId[j].id.replace(`.${atControl.id}`, LiteralMnt.postfijoDeCheckDeSeleccion);
                        var check = document.getElementById(idCheck);
                        check.checked = true;
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
        ActualizarInformacionDelGrid(grid, accion, posicionDesdeLaQueSeLeyo, registrosLeidos) {
            grid.ActualizarNavegadorDelGrid(accion, posicionDesdeLaQueSeLeyo, registrosLeidos);
            if (!grid.EsModalConGrid && grid.Estado.Contiene(atGrid.idSeleccionado)) {
                let idSeleccionado = grid.Estado.Obtener(atGrid.idSeleccionado);
                let nombreSeleccionado = grid.Estado.Obtener(atGrid.nombreSeleccionado);
                grid.AnadirAlInfoSelector(grid, idSeleccionado, nombreSeleccionado);
                grid.Estado.Quitar(atGrid.idSeleccionado);
                grid.Estado.Quitar(atGrid.nombreSeleccionado);
            }
            grid.MarcarElementos();
            grid.InfoSelector.SincronizarCheck();
            if (grid.InfoSelector.Cantidad > 0)
                grid.AjustarOpcionesDeMenu(this.InfoSelector.LeerElemento(0).Id);
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
        // permite relacionar un elemento con diferentes entidades
        // parametros de entrada:
        // idOpcionDeMenu --> id de la opción de menú que almacena los parámetros y la acción a someter
        // relacionarCon --> entidad con la que se relaciona
        // PropiedadRestrictora --> propiedad bindeada al control de filtro de la página de destino donde se mapea el restrictor seleccionado en el grid
        RelacionarCon(parametrosDeEntrada) {
            try {
                let datos = this.PrepararParametrosDeRelacionarCon(this._infoSelector, parametrosDeEntrada);
                if (datos.FiltroRestrictor !== null)
                    ApiRuote.NavegarARelacionar(this, datos.idOpcionDeMenu, datos.idSeleccionado, datos.FiltroRestrictor);
            }
            catch (error) {
                Notificar(TipoMensaje.Error, error);
                return;
            }
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
            this.CargarGrid(atGrid.accion.anterior, posicion);
        }
        ObtenerSiguientes() {
            let cantidad = this.Navegador.Cantidad;
            let pagina = this.Navegador.Pagina;
            let total = this.Navegador.Total;
            let posicion = pagina * cantidad;
            if (posicion >= total)
                return;
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
        PromesaDeCargarGrid(accion, posicion) {
            if (this.Grid.getAttribute(atGrid.cargando) == 'S')
                return null;
            let promesa = new Promise((resolve, reject) => {
                let url = this.DefinirPeticionDeBusqueda(Ajax.EndPoint.LeerDatosParaElGrid, accion, posicion);
                var datosDePeticion = new DatosPeticionNavegarGrid(this, accion, posicion);
                let a = new ApiDeAjax.DescriptorAjax(this, Ajax.EndPoint.LeerDatosParaElGrid, datosDePeticion, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, (peticion) => {
                    this.CrearFilasEnElGrid(peticion);
                    resolve(true);
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
                Notificar(TipoMensaje.Error, peticion.resultado.mensaje);
            }
            finally {
                grid.Grid.setAttribute(atGrid.cargando, 'N');
            }
        }
        CrearFilasEnElGrid(peticion) {
            let datosDeEntrada = peticion.DatosDeEntrada;
            let grid = datosDeEntrada.Grid;
            try {
                let infoObtenida = peticion.resultado.datos;
                var registros = infoObtenida.registros;
                if (datosDeEntrada.Accion === atGrid.accion.buscar)
                    grid.Navegador.Total = infoObtenida.total;
                var cuerpo = grid.CrearCuerpoDeLaTabla(grid, registros, datosDeEntrada.Accion);
                grid.AnadirCuerpoALaTabla(grid, cuerpo, datosDeEntrada.Accion);
                grid.ActualizarInformacionDelGrid(grid, datosDeEntrada.Accion, datosDeEntrada.PosicionDesdeLaQueSeLee, registros.length);
                grid.RecalcularTamanoDelCuerpoDeLaTabla(grid, cuerpo);
                grid.AplicarQueFilasMostrar(grid.InputSeleccionadas, grid.CuerpoTablaGrid, grid.InfoSelector);
            }
            finally {
                grid.Grid.setAttribute(atGrid.cargando, 'N');
                if (!grid.EsCrud) {
                    grid.Modal.style.display = 'block';
                    EntornoSe.AjustarModalesAbiertas();
                }
            }
        }
        CrearCuerpoDeLaTabla(grid, registros, accion) {
            let filaCabecera = grid.obtenerDescriptorDeLaCabecera(grid);
            //let cuerpoDeLaTabla: HTMLTableSectionElement = accion !== atGrid.accion.siguiente ?
            //    document.createElement("tbody") :
            //    grid.Grid.querySelector("table").querySelector("tbody");
            let cuerpoDeLaTabla = document.createElement("tbody");
            cuerpoDeLaTabla.id = `${grid.Grid.id}_tbody`;
            cuerpoDeLaTabla.classList.add(ClaseCss.cuerpoDeLaTabla);
            for (let i = 0; i < registros.length; i++) {
                let fila = grid.crearFila(filaCabecera, registros[i], i);
                cuerpoDeLaTabla.append(fila);
            }
            return cuerpoDeLaTabla;
        }
        AnadirCuerpoALaTabla(grid, cuerpoDeLaTabla, accion) {
            let tabla = grid.Grid.querySelector("table");
            let tbody = tabla.querySelector("tbody");
            if (tbody === null || tbody === undefined)
                tabla.append(cuerpoDeLaTabla);
            else {
                //if (accion !== atGrid.accion.siguiente)
                //    tabla.removeChild(tbody);
                tabla.removeChild(tbody);
                tabla.append(cuerpoDeLaTabla);
            }
        }
        RecalcularTamanoDelCuerpoDeLaTabla(grid, cuerpoDeLaTabla) {
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
            celdaDelTd.setAttribute(atControl.nombre, `td.${columnaCabecera.propiedad}.${this.IdGrid}`);
            celdaDelTd.setAttribute(atControl.propiedad, `${columnaCabecera.propiedad}`);
            celdaDelTd.style.textAlign = columnaCabecera.estilo.textAlign;
            celdaDelTd.style.width = `${columnaCabecera.estilo.width}`;
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
                let idModal = this.Grid.getAttribute(atSelector.idModal);
                a = `${GestorDeEventos.deSeleccion}('fila-pulsada', '${idModal}#${idCheckDeSeleccion}#${idControlHtml}');`;
            }
            else if (this.EsModalParaRelacionar) {
                let idModal = this.Grid.getAttribute(atSelector.idModal);
                a = `${GestorDeEventos.deCrearRelaciones}('fila-pulsada', '${idModal}#${idCheckDeSeleccion}#${idControlHtml}');`;
            }
            else {
                if (this.EsModalParaConsultarRelaciones) {
                    let idModal = this.Grid.getAttribute(atSelector.idModal);
                    a = `${GestorDeEventos.deConsultaDeRelaciones}('fila-pulsada', '${idModal}#${idCheckDeSeleccion}#${idControlHtml}');`;
                }
                else if (this.EsCrud)
                    a = `${GestorDeEventos.delMantenimiento}('fila-pulsada', '${idCheckDeSeleccion}#${idControlHtml}');`;
                else
                    throw new Error("No se ha definido el gestor de eventos a asociar a la pulsación de una fila en el grid");
            }
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
                let p = new PropiedadesDeLaFila();
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
        FilaPulsada(infoSelector, idCheck, idDelInput) {
            let check = document.getElementById(idCheck);
            let expresionElemento = this.ObtenerExpresionMostrar(idCheck);
            //Se hace porque antes ha pasado por aquí por haber pulsado en la fila
            if (idCheck !== idDelInput) {
                check.checked = !check.checked;
            }
            let id = this.ObtenerElIdDelElementoDelaFila(idCheck);
            if (check.checked) {
                this.AnadirAlInfoSelector(this, id, expresionElemento);
                if (!(this instanceof Crud.ModalConGrid))
                    this.AjustarOpcionesDeMenu(id);
            }
            else {
                this.QuitarDelSelector(this, id);
                if (this.InfoSelector.Cantidad === 0 && (this instanceof Crud.ModalConGrid) === false)
                    this.DeshabilitarOpcionesDeMenuDeElemento();
            }
        }
        DeshabilitarOpcionesDeMenuDeElemento() {
            let opcionesDeElemento = this.ZonaDeMenu.querySelectorAll(`input[${atOpcionDeMenu.clase}="${ClaseDeOpcioDeMenu.DeElemento}"]`);
            for (var i = 0; i < opcionesDeElemento.length; i++) {
                let opcion = opcionesDeElemento[i];
                opcion.disabled = true;
            }
        }
        AjustarOpcionesDeMenu(id) {
            let url = this.DefinirPeticionDeLeerModoDeAccesoAlElemento(id);
            let datosDeEntrada = `{"Negocio":"${this.Negocio}","id":"${id}"}`;
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.EndPoint.LeerModoDeAccesoAlElemento, datosDeEntrada, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, this.AplicarModoDeAccesoAlElemento, this.SiHayErrorTrasPeticionAjax);
            a.Ejecutar();
        }
        AplicarModoDeAccesoAlElemento(peticion) {
            let mantenimiento = peticion.llamador;
            let modoDeAccesoDelUsuario = peticion.resultado.modoDeAcceso;
            let opcionesGenerales = mantenimiento.ZonaDeMenu.querySelectorAll(`input[${atOpcionDeMenu.clase}="${ClaseDeOpcioDeMenu.DeElemento}"]`);
            let hacerLaInterseccion = mantenimiento.InfoSelector.Cantidad > 1;
            for (var i = 0; i < opcionesGenerales.length; i++) {
                let opcion = opcionesGenerales[i];
                if (ApiControl.EstaBloqueada(opcion))
                    continue;
                let estaDeshabilitado = opcion.disabled;
                let permisosNecesarios = opcion.getAttribute(atOpcionDeMenu.permisosNecesarios);
                if (permisosNecesarios === ModoDeAccesoDeDatos.Administrador && modoDeAccesoDelUsuario !== ModoDeAccesoDeDatos.Administrador)
                    opcion.disabled = true;
                else if (permisosNecesarios === ModoDeAccesoDeDatos.Gestor && (modoDeAccesoDelUsuario === ModoDeAccesoDeDatos.Consultor || modoDeAccesoDelUsuario === ModoDeAccesoDeDatos.SinPermiso))
                    opcion.disabled = true;
                else if (permisosNecesarios === ModoDeAccesoDeDatos.Consultor && modoDeAccesoDelUsuario === ModoDeAccesoDeDatos.SinPermiso)
                    opcion.disabled = true;
                else
                    opcion.disabled = (estaDeshabilitado && hacerLaInterseccion) || false;
            }
        }
        DefinirPeticionDeLeerModoDeAccesoAlElemento(id) {
            let url = `/${this.Controlador}/${Ajax.EndPoint.LeerModoDeAccesoAlElemento}`;
            let parametros = `${Ajax.Param.negocio}=${this.Negocio}&${Ajax.Param.id}=${id}`;
            let peticion = url + '?' + parametros;
            return peticion;
        }
        OrdenarPor(columna) {
            this.EstablecerOrdenacion(columna);
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