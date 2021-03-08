namespace Crud {

    export class CrudEdicion extends CrudBase {

        private _idPanelEdicion: string;
        private _infoSelectorEdicion;

        public CrudDeMnt: CrudMnt;
        protected PanelDeMnt: HTMLDivElement;
        private Altura: number;

        public get PanelDeEditar(): HTMLDivElement {
            return document.getElementById(this._idPanelEdicion) as HTMLDivElement;
        }

        public get EsModal(): boolean {
            return this.PanelDeEditar.className === ClaseCss.contenedorModal;
        }

        public get PanelDeContenidoModal(): HTMLDivElement {
            return document.getElementById(`${this._idPanelEdicion}_contenido`) as HTMLDivElement;
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
                MensajesSe.Error("IdEditor", "No está definido el control para mostrar el id del elemento");
                this.CerrarEdicion();
            }

            return control as HTMLInputElement;

        }

        //private get Controlador(): string {
        //    return this.PanelDeEditar.getAttribute(literal.controlador);
        //}

        constructor(crud: CrudMnt, idPanelEdicion: string) {
            super();

            if (IsNullOrEmpty(idPanelEdicion))
                throw Error("No se puede construir un objeto del tipo CrudEdicion sin indica el panel de edición");

            this._idPanelEdicion = idPanelEdicion;
            this.PanelDeMnt = crud.CuerpoCabecera;
            this._controlador = this.PanelDeEditar.getAttribute(literal.controlador);
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
                        this.EditarSeleccionado(this.Posicionador - 1);
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
                MensajesSe.Error("EjecutarAcciones", error);
            }

            if (cerrarEdicion)
                this.CerrarEdicion();
        }

        public ComenzarEdicion(infSel: InfoSelector) {
            this.CrudDeMnt.ModoTrabajo = ModoTrabajo.editando;

            this.InfoSelectorEdicion = infSel;

            if (this.EsModal) {
                this.PanelDeEditar.style.display = 'block';
                this.Altura = this.PanelDeContenidoModal.getBoundingClientRect().height;
                EntornoSe.AjustarModalesAbiertas();
            }
            else {
                ApiCrud.OcultarPanel(this.CrudDeMnt.CuerpoCabecera);
                ApiCrud.OcultarPanel(this.CrudDeMnt.CuerpoDatos);
                ApiCrud.OcultarPanel(this.CrudDeMnt.CuerpoPie);
                this.PosicionarEdicion();
                ApiCrud.MostrarPanel(this.PanelDeEditar);
            }
            this.EditarSeleccionado(1);
        }

        //public AjustarModal(): void {
        //    //if (this.Altura > this.CrudDeMnt.Cuerpo.getBoundingClientRect().height)
        //    //    this.PanelDeContenidoModal.style.height = `${this.CrudDeMnt.Cuerpo.getBoundingClientRect().height}px`;
        //    //else {
        //    //    this.PanelDeContenidoModal.style.height = `${this.Altura}px`;
        //    //    let padding: number = (this.PanelDeEditar.getBoundingClientRect().height - this.PanelDeContenidoModal.getBoundingClientRect().height) / 2;
        //    //    this.PanelDeEditar.style.paddingTop = `${padding}px`;
        //    //}
        //}

        public PosicionarEdicion(): void {
            this.PanelDeEditar.style.position = 'fixed';
            this.PanelDeEditar.style.top = `${AlturaCabeceraPnlControl()}px`;
            this.PanelDeEditar.style.height = `${AlturaFormulario() - AlturaPiePnlControl() - AlturaCabeceraPnlControl()}px`;
        }

        private EditarSeleccionado(seleccionado: number) {

            if (this.TotalSeleccionados === 0) {
                MensajesSe.Error("EditarSeleccionado", "No hay elementos a editar.");
                this.CerrarEdicion();
            }

            if (seleccionado === 0)
                seleccionado = 1;

            if (seleccionado > this.TotalSeleccionados)
                seleccionado = this.TotalSeleccionados;

            if (0 < seleccionado && seleccionado <= this.TotalSeleccionados) {
                this.InicializarListasDeElementos(this.PanelDeEditar, this.Controlador);
                this.InicializarListasDinamicas(this.PanelDeEditar);
                this.InicializarArchivos(this.PanelDeEditar);
                this.InicializarSelectoresDeFecha(this.PanelDeEditar);
                this.Posicionador = seleccionado;
                this.InicializarValores(seleccionado - 1);
            }

        }

        protected CerrarEdicion() {

            this.CrudDeMnt.ModoTrabajo = ModoTrabajo.mantenimiento;
            if (this.EsModal) {
                ApiCrud.CerrarModal(this.PanelDeEditar);
                EntornoSe.AjustarDivs();
            }
            else {
                ApiCrud.OcultarPanel(this.PanelDeEditar);
                ApiCrud.MostrarPanel(this.CrudDeMnt.CuerpoCabecera);
                ApiCrud.MostrarPanel(this.CrudDeMnt.CuerpoDatos);
                ApiCrud.MostrarPanel(this.CrudDeMnt.CuerpoPie);
                BlanquearMensaje();
            }
            this.CrudDeMnt.ModoTrabajo = ModoTrabajo.mantenimiento;
            this.CrudDeMnt.Buscar(atGrid.accion.buscar, 0);
        }

        protected InicializarValores(seleccionado: number) {
            let infSel: InfoSelector = this.InfoSelectorEdicion;
            let id: number = infSel.Seleccionados[seleccionado].Id as number;
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
            let json: JSON = ApiCrud.MapearControlesDesdeLaIuAlJson(this, this.PanelDeEditar, ModoTrabajo.editando);
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
                crudEdicion.CerrarEdicion();
            }
        }
    }
}