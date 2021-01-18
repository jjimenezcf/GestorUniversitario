namespace Crud {

    export class CrudEdicion extends CrudBase {

        private _idPanelEdicion: string;
        private _infoSelectorEdicion;

        protected CrudDeMnt: CrudMnt;
        protected PanelDeMnt: HTMLDivElement;

        protected get PanelDeEditar(): HTMLDivElement {
            return document.getElementById(this._idPanelEdicion) as HTMLDivElement;
        }

        public get EsModal(): boolean {
            return this.PanelDeEditar.className === ClaseCss.contenedorModal;
        }

        private get InfoSelectorEdicion(): InfoSelector {
            return this._infoSelectorEdicion;
        }

        private set InfoSelectorEdicion(info: InfoSelector) {
            this._infoSelectorEdicion = info;
            this.TotalSeleccionados = info.Cantidad;
            this.Posicionador = 1;
        }

        private get Posicionador(): number {
            let control: HTMLInputElement = document.getElementById(`${this._idPanelEdicion}-posicionador`) as HTMLInputElement;
            return Numero(control.value);
        }

        private set Posicionador(posicionador: number) {
            let control: HTMLInputElement = document.getElementById(`${this._idPanelEdicion}-posicionador`) as HTMLInputElement;
            control.value = posicionador.toString();
        }

        private get TotalSeleccionados(): number {
            return this.InfoSelectorEdicion.Cantidad;
        }

        private set TotalSeleccionados(cantidad: number) {
            let control: HTMLInputElement = document.getElementById(`${this._idPanelEdicion}-total-seleccionados`) as HTMLInputElement;
            control.value = cantidad.toString();
        }

        private get IdEditor(): HTMLInputElement {
            var control = this.BuscarEditor(this.PanelDeEditar, literal.id);

            if (control == null) {
                Mensaje(TipoMensaje.Error, "No está definido el control para mostrar el id del elemento");
                this.CerrarEdicion();
            }

            return control as HTMLInputElement;

        }

        private get Controlador(): string {
            return this.PanelDeEditar.getAttribute(literal.controlador);
        }

        constructor(crud: CrudMnt, idPanelEdicion: string) {
            super();

            if (IsNullOrEmpty(idPanelEdicion))
                throw Error("No se puede construir un objeto del tipo CrudEdicion sin indica el panel de edición");

            this._idPanelEdicion = idPanelEdicion;
            this.PanelDeMnt = crud.CuerpoCabecera;
            this.CrudDeMnt = crud;
        }

        public EjecutarAcciones(accion: string) {
            let cerrarEdicion: boolean = false;
            try {

                switch (accion) {
                    case Evento.Edicion.Modificar: {
                        this.Modificar();
                        break;
                    }
                    case Evento.Edicion.Cerrar: {
                        cerrarEdicion = true;
                        break;
                    }
                    case Evento.Edicion.MostrarPrimero: {
                        this.EditarSeleccionado(1);
                        break;
                    }
                    case Evento.Edicion.MostrarSiguiente: {
                        this.EditarSeleccionado(this.Posicionador + 1);
                        break;
                    }
                    case Evento.Edicion.MostrarAnterior: {
                        this.EditarSeleccionado(this.Posicionador -1);
                        break;
                    }
                    case Evento.Edicion.MostrarUltimo: {
                        this.EditarSeleccionado(this.TotalSeleccionados);
                        break;
                    }
                    default: {
                        throw `la opción ${accion} no está definida`;
                    }
                }
            }
            catch (error) {
                Mensaje(TipoMensaje.Error, error);
            }

            if (cerrarEdicion)
                this.CerrarEdicion();
        }

        public ComenzarEdicion(infSel: InfoSelector) {
            this.CrudDeMnt.ModoTrabajo = ModoTrabajo.editando;            

            this.InfoSelectorEdicion = infSel;

            if (this.EsModal) {
                var ventana = document.getElementById(this._idPanelEdicion);
                ventana.style.display = 'block';
            }
            else {
                this.OcultarPanel(this.CrudDeMnt.CuerpoCabecera);
                this.OcultarPanel(this.CrudDeMnt.CuerpoDatos);
                this.OcultarPanel(this.CrudDeMnt.CuerpoPie);
                this.PosicionarEdicion()
                this.MostrarPanel(this.PanelDeEditar);
            }
            this.EditarSeleccionado(1);
        }

        public PosicionarEdicion(): void {
            this.PanelDeEditar.style.position = 'fixed';
            this.PanelDeEditar.style.top = `${AlturaCabeceraPnlControl()}px`;
            this.PanelDeEditar.style.height = `${AlturaFormulario() - AlturaPiePnlControl() - AlturaCabeceraPnlControl()}px`;
        }

        private EditarSeleccionado(seleccionado: number) {

            if (this.TotalSeleccionados === 0 ) {
                Mensaje(TipoMensaje.Error, "No hay elementos a editar.")
                this.CerrarEdicion();
            }

            if (seleccionado === 0)
                seleccionado = 1;

            if (seleccionado > this.TotalSeleccionados)
                seleccionado = this.TotalSeleccionados;

            if (0 < seleccionado && seleccionado <= this.TotalSeleccionados) {
                this.InicializarListasDeElementos(this.PanelDeEditar, this.Controlador);
                this.InicializarListasDinamicas(this.PanelDeEditar);
                this.InicializarCanvases(this.PanelDeEditar);
                this.Posicionador = seleccionado;
                this.InicializarValores(seleccionado -1);
            }

        }

        protected CerrarEdicion() {

            this.CrudDeMnt.ModoTrabajo = ModoTrabajo.mantenimiento;
            if (this.EsModal) {
                this.CerrarModal(this.PanelDeEditar);
            }
            else {
                this.OcultarPanel(this.PanelDeEditar);
                this.MostrarPanel(this.CrudDeMnt.CuerpoCabecera);
                this.MostrarPanel(this.CrudDeMnt.CuerpoDatos);
                this.MostrarPanel(this.CrudDeMnt.CuerpoPie);
                BlanquearMensaje();
            }
            this.CrudDeMnt.ModoTrabajo = ModoTrabajo.mantenimiento;
            this.CrudDeMnt.Buscar(atGrid.accion.buscar, 0);
        }

        protected InicializarValores(seleccionado: number) {
            let infSel: InfoSelector = this.InfoSelectorEdicion;
            let id: number = infSel.Seleccionados[seleccionado] as number;            
            this.IdEditor.value = id.toString();
            this.LeerElemento(id);
        }

        private LeerElemento(id: number) {
            //let idJson: string = this.DefinirFiltroPorId(id);
            let url: string = `/${this.Controlador}/${Ajax.EndPoint.LeerPorId}?${Ajax.Param.id}=${id}`;

            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EndPoint.LeerPorId
                , null
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.MapearElementoDevuelto
                , this.SiHayErrorAlLeerElemento
            );

            a.Ejecutar();
        }

        private MapearElementoDevuelto(peticion: ApiDeAjax.DescriptorAjax) {
            let edicion: CrudEdicion = peticion.llamador as CrudEdicion;
            let panel = edicion.PanelDeEditar;
            edicion.MapearElementoLeido(panel, peticion.resultado.datos, peticion.resultado.modoDeAcceso);
        }

        private SiHayErrorAlLeerElemento(peticion: ApiDeAjax.DescriptorAjax) {
            let edicion: CrudEdicion = peticion.llamador as CrudEdicion;
            edicion.CerrarEdicion();
            edicion.CrudDeMnt.BlanquearTodosLosCheck();
            edicion.SiHayErrorTrasPeticionAjax(peticion);
        }

        protected Modificar() {
            let json: JSON = this.MapearControlesDeIU(this.PanelDeEditar, ModoTrabajo.editando);
            this.ModificarElemento(json);
        }

        private ModificarElemento(json: JSON) {
            let controlador = this.PanelDeEditar.getAttribute(literal.controlador);
            let url: string = `/${controlador}/${Ajax.EndPoint.Modificar}?${Ajax.Param.elementoJson}=${JSON.stringify(json)}`;

            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EndPoint.Modificar
                , this
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.DespuesDeModificar
                , this.SiHayErrorTrasPeticionAjax
            );
            a.Ejecutar();
        }

        private DespuesDeModificar(peticion: ApiDeAjax.DescriptorAjax) {
            let crudEdicion: CrudEdicion = peticion.llamador as CrudEdicion;
            if (crudEdicion.TotalSeleccionados === 1) {
                crudEdicion.CerrarEdicion()
            }
        }

        public MaperaRestrictorDeEdicion(porpiedadRestrictora: string, valorRestrictor: number, valorMostrar: string) {
            let restrictores: NodeListOf<HTMLInputElement> = this.PanelDeEditar.querySelectorAll(`input[${atControl.tipo}="${TipoControl.restrictorDeEdicion}"]`) as NodeListOf<HTMLInputElement>;
            this.MapearRestrictor(restrictores, porpiedadRestrictora, valorMostrar, valorRestrictor);
        }

    }
}