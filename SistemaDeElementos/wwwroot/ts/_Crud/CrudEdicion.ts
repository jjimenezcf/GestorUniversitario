namespace Crud {

    export class CrudEdicion extends CrudBase {

        private _idPanelEdicion: string;
        private _infoSelectorEdicion;

        protected CrudDeMnt: CrudMnt;
        protected PanelDeMnt: HTMLDivElement;

        protected get PanelDeEditar(): HTMLDivElement {
            return document.getElementById(this._idPanelEdicion) as HTMLDivElement;
        }

        private get EsModal(): boolean {
            return this.PanelDeEditar.className === ClaseCss.contenedorModal;
        }

        private get InfoSelectorEdicion(): InfoSelector {
            return this._infoSelectorEdicion;
        }

        private get Posicionador(): HTMLInputElement {
            return document.getElementById(`${this._idPanelEdicion}-posicionador`) as HTMLInputElement;
        }

        private get IdEditor(): HTMLInputElement {
            var control = this.BuscarEditor(this.PanelDeEditar, Literal.id);

            if (control == null) {
                Mensaje(TipoMensaje.Error, "No está definido el control para mostrar el id del elemento");
                this.CerrarEdicion();
            }

            return control as HTMLInputElement;

        }

        private get Controlador(): string {
            return this.PanelDeEditar.getAttribute(Literal.controlador);
        }

        constructor(crud: CrudMnt, idPanelEdicion: string) {
            super();

            if (EsNula(idPanelEdicion))
                throw Error("No se puede construir un objeto del tipo CrudEdicion sin indica el panel de edición");


            this._idPanelEdicion = idPanelEdicion;
            this.PanelDeMnt = crud.PanelDeMnt;
            this.CrudDeMnt = crud;
        }

        public EjecutarAcciones(accion: string) {
            let hayError: boolean = false;
            try {

                switch (accion) {
                    case LiteralEdt.Accion.ModificarElemento: {
                        this.Modificar();
                        break;
                    }
                    case LiteralEdt.Accion.CancelarEdicion: {
                        hayError = false;
                        break;
                    }
                    case LiteralEdt.Accion.MostrarPrimero: {
                        console.log("primero");
                        break;
                    }
                    case LiteralEdt.Accion.MostrarSiguiente: {
                        console.log("siguiente");
                        break;
                    }
                    default: {
                        throw `la opción ${accion} no está definida`;
                    }
                }
            }
            catch (error) {
                hayError = true;
                Mensaje(TipoMensaje.Error, error);
            }

            if (!hayError) this.CerrarEdicion();
        }

        public ComenzarEdicion(panelAnterior: HTMLDivElement, infSel: InfoSelector) {
            this.ModoTrabajo = ModoTrabajo.editando;
            this._infoSelectorEdicion = infSel;

            if (this.EsModal) {
                var ventana = document.getElementById(this._idPanelEdicion);
                ventana.style.display = 'block';
            }
            else {

                this.OcultarPanel(panelAnterior);
                this.MostrarPanel(this.PanelDeEditar);
            }
            this.InicializarSlectoresDeElementos(this.PanelDeEditar, this.Controlador);
            this.InicializarCanvases(this.PanelDeEditar);
            this.InicializarValores();
        }

        protected CerrarEdicion() {


            this.ModoTrabajo = ModoTrabajo.mantenimiento;
            if (this.EsModal) {
                this.CerrarModal(this._idPanelEdicion);
            }
            else {
                this.Cerrar(this.PanelDeMnt, this.PanelDeEditar);
            }
            this.CrudDeMnt.Buscar(0);
        }

        protected InicializarValores() {
            let infSel: InfoSelector = this.InfoSelectorEdicion;
            this.Posicionador.value = infSel.Seleccionados.length.toString();
            let id: number = infSel.Seleccionados[0] as number;            
            this.IdEditor.value = id.toString();
            this.LeerElemento(id);
        }

        private LeerElemento(id: number) {
            let idJson: string = this.DefinirFiltroPorId(id);
            let url: string = `/${this.Controlador}/${Ajax.EndPoint.LeerPorIds}?${Ajax.Param.idsJson}=${idJson}`;

            let a = new ApiDeAjax.DescriptorAjax(Ajax.EndPoint.LeerPorIds
                , this
                , url
                , ApiDeAjax.TipoPeticion.Sincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.MapearElementoDevuelto
                , null
            );

            a.Ejecutar();
        }

        private MapearElementoDevuelto(peticion: ApiDeAjax.DescriptorAjax) {
            let edicion: CrudEdicion = (peticion.DatosDeEntrada as CrudEdicion);
            let panel = edicion.PanelDeEditar;
            edicion.MapearElementoLeido(panel, peticion.resultado.datos[0]);
        }

        private DefinirFiltroPorId(id: number): string {
            var clausulas = new Array<ClausulaDeFiltrado>();
            var clausula: ClausulaDeFiltrado = new ClausulaDeFiltrado('id', 'igual', `${id}`);
            clausulas.push(clausula);
            return JSON.stringify(clausulas);
        }

        protected Modificar() {
            let json: JSON = this.MapearControlesDeIU(this.PanelDeEditar);
            this.ModificarElemento(json);
        }

        private ModificarElemento(json: JSON) {
            let controlador = this.PanelDeEditar.getAttribute(Literal.controlador);
            let url: string = `/${controlador}/${Ajax.EndPoint.Modificar}?${Ajax.Param.elementoJson}=${JSON.stringify(json)}`;

            let a = new ApiDeAjax.DescriptorAjax(Ajax.EndPoint.Modificar
                , this
                , url
                , ApiDeAjax.TipoPeticion.Sincrona
                , ApiDeAjax.ModoPeticion.Get
                , null
                , null
            );
            a.Ejecutar();
        }
    }
}