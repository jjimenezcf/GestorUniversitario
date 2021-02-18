var Crud;
(function (Crud) {
    class CrudCreacion extends Crud.CrudBase {
        constructor(crud, idPanelCreacion) {
            super();
            if (IsNullOrEmpty(idPanelCreacion))
                throw Error("No se puede construir un objeto del tipo CrudCreacion sin indica el panel de creación");
            this._idPanelCreacion = idPanelCreacion;
            this._controlador = this.PanelDeCrear.getAttribute(literal.controlador);
            this.CrudDeMnt = crud;
        }
        get PanelDeCrear() {
            return document.getElementById(this._idPanelCreacion);
        }
        get EsModal() {
            return this.PanelDeCrear.className === ClaseCss.contenedorModal;
        }
        get PanelDeContenidoModal() {
            return document.getElementById(`${this._idPanelCreacion}_contenido`);
        }
        //private get Controlador(): string {
        //    return this.PanelDeCrear.getAttribute(literal.controlador);
        //}
        get SeguirCreando() {
            let check = document.getElementById(`${this._idPanelCreacion}-crear-mas`);
            return !check.checked;
        }
        EjecutarAcciones(accion) {
            if (accion === Evento.Creacion.Crear)
                this.Crear();
            else if (accion === Evento.Creacion.Cerrar)
                this.CerrarCreacion();
            else
                throw `la opción ${accion} no está definida`;
            ApiCrud.QuitarClaseDeCtrlNoValido(this.PanelDeCrear);
        }
        ComenzarCreacion() {
            this.CrudDeMnt.ModoTrabajo = ModoTrabajo.creando;
            if (this.EsModal) {
                this.PanelDeCrear.style.display = 'block';
                this.Altura = this.PanelDeContenidoModal.getBoundingClientRect().height;
                this.AjustarModal();
            }
            else {
                ApiCrud.OcultarPanel(this.CrudDeMnt.CuerpoCabecera);
                ApiCrud.OcultarPanel(this.CrudDeMnt.CuerpoDatos);
                ApiCrud.OcultarPanel(this.CrudDeMnt.CuerpoPie);
                this.PosicionarCreacion();
                ApiCrud.MostrarPanel(this.PanelDeCrear);
            }
            this.InicializarPanel();
        }
        AjustarModal() {
            if (this.Altura > this.CrudDeMnt.Cuerpo.getBoundingClientRect().height)
                this.PanelDeContenidoModal.style.height = `${this.CrudDeMnt.Cuerpo.getBoundingClientRect().height}px`;
            else
                this.PanelDeContenidoModal.style.height = `${this.Altura}px`;
        }
        PosicionarCreacion() {
            this.PanelDeCrear.style.position = 'fixed';
            this.PanelDeCrear.style.top = `${AlturaCabeceraPnlControl()}px`;
            this.PanelDeCrear.style.height = `${AlturaFormulario() - AlturaPiePnlControl() - AlturaCabeceraPnlControl()}px`;
        }
        InicializarPanel() {
            this.InicializarListasDeElementos(this.PanelDeCrear, this.Controlador);
            this.InicializarListasDinamicas(this.PanelDeCrear);
            this.InicializarArchivos(this.PanelDeCrear);
        }
        Crear() {
            let json = ApiCrud.MapearControlesDesdeLaIuAlJson(this, this.PanelDeCrear, ModoTrabajo.creando);
            this.CrearElemento(json);
        }
        CerrarCreacion() {
            this.CrudDeMnt.ModoTrabajo = ModoTrabajo.mantenimiento;
            if (this.EsModal) {
                ApiCrud.CerrarModal(this.PanelDeCrear);
                EntornoSe.AjustarDivs();
            }
            else {
                ApiCrud.OcultarPanel(this.PanelDeCrear);
                ApiCrud.MostrarPanel(this.CrudDeMnt.CuerpoPie);
                ApiCrud.MostrarPanel(this.CrudDeMnt.CuerpoDatos);
                ApiCrud.MostrarPanel(this.CrudDeMnt.CuerpoCabecera);
                BlanquearMensaje();
            }
            this.CrudDeMnt.ModoTrabajo = ModoTrabajo.mantenimiento;
            this.CrudDeMnt.Buscar(atGrid.accion.buscar, 0);
        }
        CrearElemento(json) {
            let url = `/${this.Controlador}/${Ajax.EndPoint.Crear}?${Ajax.Param.elementoJson}=${JSON.stringify(json)}`;
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.EndPoint.Crear, "{}", url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, this.DespuesDeCrear, this.SiHayErrorTrasPeticionAjax);
            a.Ejecutar();
        }
        DespuesDeCrear(peticion) {
            let crudCreador = peticion.llamador;
            ApiCrud.BlanquearControlesDeIU(crudCreador.PanelDeCrear);
            if (crudCreador.SeguirCreando) {
                crudCreador.InicializarPanel();
            }
            else {
                crudCreador.CerrarCreacion();
            }
        }
        MaperaRestrictorDeCreacion(porpiedadRestrictora, valorRestrictor, valorMostrar) {
            let restrictores = this.PanelDeCrear.querySelectorAll(`input[${atControl.tipo}="${TipoControl.restrictorDeEdicion}"]`);
            this.MapearRestrictor(restrictores, porpiedadRestrictora, valorMostrar, valorRestrictor);
        }
    }
    Crud.CrudCreacion = CrudCreacion;
})(Crud || (Crud = {}));
//# sourceMappingURL=CrudCreacion.js.map