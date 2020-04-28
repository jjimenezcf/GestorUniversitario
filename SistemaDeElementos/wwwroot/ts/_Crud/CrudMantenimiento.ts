namespace Crud {

    export let crudMnt: CrudMnt = null;

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

    export class CrudMnt extends CrudBase {

        public crudDeCreacion: CrudCreacion;
        public crudDeEdicion: CrudEdicion;
        public idModalBorrar: string;

        public PanelDeMnt: HTMLDivElement;
        public IdGrid: string;
        private infSel: InfoSelector;
        private InputCantidad: HTMLInputElement;
        private ZonaDeGrid: HTMLDivElement;
        private ZonaDeFiltro: HTMLDivElement;
        private Ordenacion: Ordenacion;

        constructor(idPanelMnt: string) {
            super();

            if (EsNula(idPanelMnt))
                throw Error("No se puede construir un objeto del tipo CrudMantenimiento sin indica el panel de mantenimiento");

            this.IdGrid = `${idPanelMnt}_grid`;
            this.ZonaDeGrid = document.getElementById(`${this.IdGrid}`) as HTMLDivElement;
            this.PanelDeMnt = document.getElementById(idPanelMnt) as HTMLDivElement;
            this.infSel = new InfoSelector(this.IdGrid);
            var idHtmlFiltro = this.ZonaDeGrid.getAttribute(Atributo.zonaDeFiltro);
            this.ZonaDeFiltro = document.getElementById(`${idHtmlFiltro}`) as HTMLDivElement;
            this.Ordenacion = new Ordenacion();
            this.InicializarNavegador();
        }

        private InicializarNavegador() {
            let idCrtlCantidad: string = `${this.IdGrid}_${LiteralMnt.idCtrlCantidad}`;
            this.InputCantidad = document.getElementById(`${idCrtlCantidad}`) as HTMLInputElement;

            for (var i = 0; i < this.Ordenacion.Count(); i++) {
                let orden: Orden = this.Ordenacion.Leer(i);
                let columna: HTMLTableHeaderCellElement = document.getElementById(orden.IdColumna) as HTMLTableHeaderCellElement;
                columna.setAttribute(Atributo.modoOrdenacion, orden.Modo);
                let a: HTMLElement = columna.getElementsByTagName('a')[0] as HTMLElement;
                a.setAttribute("class", orden.ccsClase);
            }
        }

        public AbrirModalBorrarElemento() {
            if (this.infSel.Cantidad == 0) {
                Mensaje(TipoMensaje.Info, "Debe marcar el elemento a borrar");
                return;
            }
            this.AbrirModalDeBorrar();
        }

        public CerrarModalDeBorrado() {
            let modalBorrar: HTMLDivElement = document.getElementById(this.idModalBorrar) as HTMLDivElement;
            modalBorrar.style.display = "none";
            var body = document.getElementsByTagName("body")[0];
            body.style.position = "inherit";
            body.style.height = "auto";
            body.style.overflow = "visible";
        }

        AbrirModalDeBorrar() {
            var ventana = document.getElementById(this.idModalBorrar);
            var btn = document.getElementById("btnModal");
            var span = document.getElementsByClassName("span-cerrar")[0];
            var body = document.getElementsByTagName("body")[0];
            ventana.style.display = 'block';
        }


        public IraEditar() {
            if (this.infSel.Cantidad == 0) {
                Mensaje(TipoMensaje.Info, "Debe marcar el elemento a editar");
                return;
            }

            this.crudDeEdicion.ComenzarEdicion(crudMnt.PanelDeMnt, this.infSel);
        }

        public IraCrear() {
            this.crudDeCreacion.ComenzarCreacion(crudMnt.PanelDeMnt);
        }

        public AlPulsarUnCheckDeSeleccion(idCheck: string, idDelInput: string) {

            let check: HTMLInputElement = document.getElementById(idCheck) as HTMLInputElement;
            //Se hace porque antes ha pasado por aquí por haber pulsado en la fila
            if (idCheck === idDelInput) {
                check.checked = !check.checked;
                return;
            }

            BlanquearMensaje();
            check.checked = !check.checked;

            if (check.checked)
                this.AnadirAlInfoSelector(idCheck);
            else
                this.QuitarDelSelector(idCheck);
        }

        private AnadirAlInfoSelector(idCheck) {
            var id = ObtenerIdDeLaFilaChequeada(idCheck);
            this.infSel.InsertarId(id);
        }

        private QuitarDelSelector(idCheck) {
            var id = ObtenerIdDeLaFilaChequeada(idCheck);
            this.infSel.Quitar(id);
        }

        private marcarElementos() {

            if (this.infSel.Cantidad === 0)
                return;

            var celdasId = document.getElementsByName(`${Literal.id}.${this.IdGrid}`);
            var len = celdasId.length;
            for (var i = 0; i < this.infSel.Cantidad; i++) {
                for (var j = 0; j < len; j++) {
                    let id: number = this.infSel.LeerId(i);
                    if ((<HTMLInputElement>celdasId[j]).value.Numero() == id) {
                        var idCheck = celdasId[j].id.replace(`.${Literal.id}`, LiteralMnt.postfijoDeCheckDeSeleccion);
                        var check = document.getElementById(idCheck);
                        (<HTMLInputElement>check).checked = true;
                        break;
                    }
                }
            }
        }

        public OrdenarPor(columna: string) {
            this.ParamentrosDeOrdenacion(columna);
            this.Buscar(0);
        }

        ParamentrosDeOrdenacion(idcolumna: string) {
            let htmlColumna: HTMLTableHeaderCellElement = document.getElementById(idcolumna) as HTMLTableHeaderCellElement;
            let modo: string = htmlColumna.getAttribute(Atributo.modoOrdenacion);
            if (EsNula(modo))
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

        public ObtenerUltimos() {
            this.Buscar(-1);
        }

        public ObtenerAnteriores() {
            let cantidad: number = this.InputCantidad.value.Numero();
            let posicion: number = this.InputCantidad.getAttribute(Atributo.posicion).Numero();
            posicion = posicion - (cantidad * 2);
            if (posicion < 0)
                posicion = 0;
            this.Buscar(posicion);
        }

        public ObtenerSiguientes() {
            let posicion: number = this.InputCantidad.getAttribute(Atributo.posicion).Numero();
            this.Buscar(posicion);
        }

        BorrarElemento() {
            this.CerrarModalDeBorrado();
            let id: number = this.infSel.Seleccionados[0] as number;
            let url: string = this.DefinirPeticionDeBorrado(id);
            let req: XMLHttpRequest = new XMLHttpRequest();
            try {
                this.PeticionSincrona(req, url, Ajax.EndPoint.Borrar);
            }
            catch (error) {
                Mensaje(TipoMensaje.Error, error);
                return;
            }
            this.Buscar(0);
        }

        DefinirPeticionDeBorrado(id: number): string {
            let idJson: JSON = JSON.parse(`[${id}]`);
            var controlador = this.InputCantidad.getAttribute(Atributo.controlador);
            let url: string = `/${controlador}/${Ajax.EndPoint.Borrar}`;
            let parametros: string = `${Ajax.Param.idsJson}=${JSON.stringify(idJson)}`;
            let peticion: string = url + '?' + parametros;
            return peticion;
        }

        public Buscar(posicion: number) {
            if (this.InputCantidad === null)
                Mensaje(TipoMensaje.Error, `No está definido el control de la cantidad de elementos a obtener`);
            else {
                let url: string = this.DefinirPeticionDeBusqueda(posicion);
                let req: XMLHttpRequest = new XMLHttpRequest();
                this.PeticionSincrona(req, url, Ajax.EndPoint.LeerGridEnHtml);
            }
        }

        protected DespuesDeLaPeticion(req: XMLHttpRequest, peticion: string): ResultadoJson {
            let resultado: ResultadoHtml = super.DespuesDeLaPeticion(req, peticion) as ResultadoHtml;

            if (peticion === Ajax.EndPoint.LeerGridEnHtml) {
                this.ZonaDeGrid.innerHTML = resultado.html;
                this.InicializarNavegador();
                if (this.infSel !== undefined && this.infSel.Cantidad > 0) {
                    this.marcarElementos();
                    this.infSel.SincronizarCheck();
                }
            }
            return resultado;
        }

        private DefinirPeticionDeBusqueda(posicion: number): string {
            var cantidad = this.InputCantidad.value.Numero();
            var controlador = this.InputCantidad.getAttribute(Atributo.controlador);
            var filtroJson = this.ObtenerFiltros();
            var ordenJson = this.ObtenerOrdenacion();

            let url: string = `/${controlador}/${Ajax.EndPoint.LeerGridEnHtml}`;
            let parametros: string = `${Ajax.Param.modo}=Mantenimiento` +
                `&${Ajax.Param.posicion}=${posicion}` +
                `&${Ajax.Param.cantidad}=${cantidad}` +
                `&${Ajax.Param.filtro}=${filtroJson}` +
                `&${Ajax.Param.orden}=${ordenJson}`;
            let peticion: string = url + '?' + parametros;
            return peticion;
        }

        private ObtenerOrdenacion() {
            var clausulas = new Array<ClausulaDeOrdenacion>();
            for (var i = 0; i < this.Ordenacion.Count(); i++) {
                let orden = this.Ordenacion.Leer(i);
                clausulas.push(new ClausulaDeOrdenacion(orden.Propiedad, orden.Modo));
            }
            return JSON.stringify(clausulas);
        }
        private ObtenerFiltros() {
            var arrayIds = this.ObtenerControlesDeFiltro();
            var clausulas = new Array<ClausulaDeFiltrado>();
            for (let id of arrayIds) {
                var input: HTMLInputElement = <HTMLInputElement>document.getElementById(`${id}`);
                var tipo: string = input.getAttribute(TipoControl.Tipo);
                var clausula: ClausulaDeFiltrado = null;
                if (tipo === TipoControl.Editor) {
                    clausula = this.ObtenerClausulaEditor(input);
                }
                else
                    if (tipo === TipoControl.Selector) {
                        clausula = this.ObtenerClausulaSelector(input);
                    }
                    else
                        console.log(`No está implementado como definir la cláusula de filtrado de un tipo ${TipoControl}`);

                if (clausula !== null)
                    clausulas.push(clausula);
            }
            return JSON.stringify(clausulas);
        }

        private ObtenerControlesDeFiltro() {

            var arrayIds = new Array();
            var arrayHtmlImput = this.ZonaDeFiltro.getElementsByTagName(TagName.input);

            for (let i = 0; i < arrayHtmlImput.length; i++) {
                var htmlImput = arrayHtmlImput[i];
                var esFiltro = htmlImput.getAttribute(Atributo.filtro);
                if (esFiltro === 'S') {
                    var id = htmlImput.getAttribute(Atributo.Id);
                    if (id === null)
                        console.log(`Falta el atributo id del componente de filtro ${htmlImput}`);
                    else
                        arrayIds.push(htmlImput.getAttribute(Atributo.Id));
                }
            }
            return arrayIds;
        }

        private ObtenerClausulaEditor(editor: HTMLInputElement) {
            var propiedad: string = editor.getAttribute(Atributo.propiedad);
            var criterio: string = editor.getAttribute(Atributo.criterio);
            var valor = editor.value;
            var clausula = null;
            if (!EsNula(valor))
                clausula = { propiedad: `${propiedad}`, criterio: `${criterio}`, valor: `${valor}` };

            return clausula;
        }

        private ObtenerClausulaSelector(selector: HTMLInputElement) {
            var propiedad = selector.getAttribute(Atributo.propiedad);
            var criterio = selector.getAttribute(Atributo.criterio);
            var valor = null;
            var clausula = null;
            if (selector.hasAttribute(ListaDeSeleccionados)) {
                var ids = selector.getAttribute(ListaDeSeleccionados);
                if (!ids.NoDefinida()) {
                    valor = ids;
                    clausula = new ClausulaDeFiltrado(propiedad, criterio, valor);
                }
            }
            return clausula;
        }

    }

    export function EjecutarMenuMnt(accion: string, parametros: string): void {
        if (accion === LiteralMnt.CrearElemento)
            crudMnt.IraCrear();
        else if (accion === LiteralMnt.EditarElemento)
            crudMnt.IraEditar();
        else if (accion === LiteralMnt.AbrirBorrarElemento)
            crudMnt.AbrirModalBorrarElemento();
        else if (accion === LiteralMnt.CerrarModalDeBorrado)
            crudMnt.CerrarModalDeBorrado();
        else if (accion === LiteralMnt.BorrarElemento)
            crudMnt.BorrarElemento();
        else if (accion === LiteralMnt.Buscar)
            crudMnt.Buscar(0);
        else if (accion === LiteralMnt.ObtenerSiguientes)
            crudMnt.ObtenerSiguientes();
        else if (accion === LiteralMnt.ObtenerAnteriores)
            crudMnt.ObtenerAnteriores();
        else if (accion === LiteralMnt.ObtenerUltimos)
            crudMnt.ObtenerUltimos();
        else if (accion === LiteralMnt.OrdenarPor)
            crudMnt.OrdenarPor(parametros);
        else
            Mensaje(TipoMensaje.Info, `la opción ${accion} no está definida`);
    }

    export function AlPulsarUnCheckDeSeleccion(idGrid, idCheck, idDelInput) {
        if (crudMnt.IdGrid != idGrid) {
            BlanquearMensaje();
            var check = <HTMLInputElement>document.getElementById(idCheck);
            if (check.checked)
                AnadirAlInfoSelector(idGrid, idCheck);
            else
                QuitarDelSelector(idGrid, idCheck);
        }
        else
            crudMnt.AlPulsarUnCheckDeSeleccion(idCheck, idDelInput);
    }

    function AnadirAlInfoSelector(idGrid, idCheck) {

        var infSel = infoSelectores.Obtener(idGrid);
        if (infSel === undefined) {
            infSel = infoSelectores.Crear(idGrid);
        }

        var id = ObtenerIdDeLaFilaChequeada(idCheck);
        if (infSel.EsModalDeSeleccion) {
            var textoMostrar = obtenerValorDeLaColumnaChequeada(idCheck, infSel.ColumnaMostrar);
            infSel.InsertarElemento(id, textoMostrar);
        }
        else {
            infSel.InsertarId(id);
        }

    }

    function QuitarDelSelector(idGrid, idCheck) {

        var infSel = infoSelectores.Obtener(idGrid);
        if (infSel !== undefined) {
            var id = ObtenerIdDeLaFilaChequeada(idCheck);
            infSel.Quitar(id);
        }
        else
            Mensaje(TipoMensaje.Error, `El selector ${idGrid} no está definido`);
    }

}