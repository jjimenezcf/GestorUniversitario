var Crud;
(function (Crud) {
    Crud.crudMnt = null;
    class CrudMnt extends Crud.GridDeDatos {
        constructor(idPanelMnt, idModalDeBorrado) {
            super(idPanelMnt);
            this.ModalesDeSeleccion = new Array();
            this.ModalesParaRelacionar = new Array();
            this.ModalesParaConsultarRelaciones = new Array();
            this._idModalBorrar = idModalDeBorrado;
        }
        get Cuerpo() {
            return document.getElementById(LiteralMnt.idCuerpoDePagina);
        }
        ;
        get ModoTrabajo() {
            return this.modoTrabajo;
        }
        set ModoTrabajo(modo) {
            this.modoTrabajo = modo;
        }
        get ModalDeBorrado() {
            return document.getElementById(this._idModalBorrar);
        }
        ;
        Inicializar(idPanelMnt) {
            try {
                if (IsNullOrEmpty(idPanelMnt))
                    idPanelMnt = this.IdCuerpoCabecera;
                super.Inicializar(this.IdCuerpoCabecera);
                this.ModoTrabajo = ModoTrabajo.mantenimiento;
                this.InicializarSelectores();
                this.InicializarListasDeElementos(this.ZonaDeFiltro, this.Navegador.Controlador);
                this.InicializarMenus();
                this.InicializarSelectoresDeFecha(this.ZonaDeFiltro, this.Navegador.Controlador);
                this.AplicarRestrictores();
                this.Buscar(atGrid.accion.buscar, 0);
            }
            catch (error) {
                Mensaje(TipoMensaje.Error, `Error al inicializar el crud ${this.IdCuerpoCabecera}`, error);
            }
        }
        PosicionarPanelesDelCuerpo() {
            if (this.ModoTrabajo === ModoTrabajo.mantenimiento) {
                this.PosicionarFiltro();
                this.PosicionarGrid();
            }
            if (this.ModoTrabajo === ModoTrabajo.editando || this.ModoTrabajo === ModoTrabajo.consultando) {
                if (!this.crudDeEdicion.EsModal)
                    this.crudDeEdicion.PosicionarEdicion();
                else
                    this.crudDeEdicion.AjustarModal();
            }
            if ((this.ModoTrabajo === ModoTrabajo.creando || this.ModoTrabajo === ModoTrabajo.copiando) && !this.crudDeCreacion.EsModal)
                this.crudDeCreacion.PosicionarCreacion();
        }
        PosicionarFiltro() {
            this.ZonaDeFiltro.style.position = 'fixed';
            let posicionFiltro = this.PosicionFiltro();
            this.ZonaDeFiltro.style.top = `${posicionFiltro}px`;
            let bloques = this.ZonaDeFiltro.getElementsByClassName('cuerpo-datos-filtro-bloque');
            let alturaDeBloques = 0;
            for (let i = 0; i < bloques.length; i++) {
                alturaDeBloques = alturaDeBloques + bloques[i].getBoundingClientRect().height;
            }
            let alturaCalculada = AlturaFormulario() * 20 / 100;
            this.ZonaDeFiltro.style.height = alturaDeBloques < alturaCalculada
                ? `${alturaDeBloques}px`
                : `${alturaCalculada}px`;
        }
        PosicionFiltro() {
            let alturaCabeceraPnlControl = AlturaCabeceraPnlControl();
            let alturaCabeceraMnt = this.CuerpoCabecera.getBoundingClientRect().height;
            return alturaCabeceraPnlControl + alturaCabeceraMnt;
        }
        AplicarRestrictores() {
            if (this.Estado.Contiene(Sesion.restrictor)) {
                let restrictor = this.Estado.Obtener(Sesion.restrictor);
                this.MapearRestrictorDeFiltro(restrictor.Propiedad, restrictor.Valor, restrictor.Texto);
                this.crudDeCreacion.MaperaRestrictorDeCreacion(restrictor.Propiedad, restrictor.Valor, restrictor.Texto);
                this.crudDeEdicion.MaperaRestrictorDeEdicion(restrictor.Propiedad, restrictor.Valor, restrictor.Texto);
            }
        }
        InicializarSelectores() {
            let selectores = this.ZonaDeFiltro.querySelectorAll(`input[tipo="${TipoControl.Selector}"]`);
            selectores.forEach((selector) => {
                let idModal = selector.getAttribute(atSelector.idModal);
                let modal = new Crud.ModalSeleccion(idModal);
                this.ModalesDeSeleccion.push(modal);
            });
        }
        InicializarMenus() {
            this.DeshabilitarOpcionesDeMenuDeElemento();
            let url = this.DefinirPeticionDeLeerModoDeAccesoAlNegocio();
            let datosDeEntrada = `{"negocio":"${this.Negocio}"}`;
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.EndPoint.LeerModoDeAccesoAlNegocio, datosDeEntrada, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, this.AplicarModoDeAccesoAlNegocio, this.SiHayErrorTrasPeticionAjax);
            a.Ejecutar();
        }
        AplicarModoDeAccesoAlNegocio(peticion) {
            let mantenimiento = peticion.llamador;
            let modoDeAccesoDelUsuario = peticion.resultado.modoDeAcceso;
            let opcionesGenerales = mantenimiento.ZonaDeMenu.querySelectorAll(`input[${atOpcionDeMenu.clase}="${ClaseDeOpcioDeMenu.DeVista}"]`);
            for (var i = 0; i < opcionesGenerales.length; i++) {
                let opcion = opcionesGenerales[i];
                let permisosNecesarios = opcion.getAttribute(atOpcionDeMenu.permisosNecesarios);
                if (permisosNecesarios === ModoDeAccesoDeDatos.Administrador && modoDeAccesoDelUsuario !== ModoDeAccesoDeDatos.Administrador)
                    opcion.disabled = true;
                else if (permisosNecesarios === ModoDeAccesoDeDatos.Gestor && (modoDeAccesoDelUsuario === ModoDeAccesoDeDatos.Consultor || modoDeAccesoDelUsuario === ModoDeAccesoDeDatos.SinPermiso))
                    opcion.disabled = true;
                else if (permisosNecesarios === ModoDeAccesoDeDatos.Consultor && modoDeAccesoDelUsuario === ModoDeAccesoDeDatos.SinPermiso)
                    opcion.disabled = true;
                else
                    opcion.disabled = false;
            }
        }
        DefinirPeticionDeLeerModoDeAccesoAlNegocio() {
            let url = `/${this.Controlador}/${Ajax.EndPoint.LeerModoDeAccesoAlNegocio}`;
            let parametros = `${Ajax.Param.negocio}=${this.Negocio}`;
            let peticion = url + '?' + parametros;
            return peticion;
        }
        ObtenerModalDeSeleccion(idModal) {
            for (let i = 0; i < this.ModalesDeSeleccion.length; i++) {
                let modal = this.ModalesDeSeleccion[i];
                if (modal.IdModal === idModal)
                    return modal;
            }
            return undefined;
        }
        ObtenerModalParaRelacionar(idModal) {
            for (let i = 0; i < this.ModalesParaRelacionar.length; i++) {
                let modal = this.ModalesParaRelacionar[i];
                if (modal.IdModal === idModal)
                    return modal;
            }
            let modal = new Crud.ModalParaRelacionar(this, idModal);
            this.ModalesParaRelacionar.push(modal);
            return modal;
        }
        ObtenerModalParaConsultarRelaciones(idModal) {
            for (let i = 0; i < this.ModalesParaConsultarRelaciones.length; i++) {
                let modal = this.ModalesParaConsultarRelaciones[i];
                if (modal.IdModal === idModal)
                    return modal;
            }
            let modal = new Crud.ModalParaConsultarRelaciones(this, idModal);
            this.ModalesParaConsultarRelaciones.push(modal);
            return modal;
        }
        AbrirModalParaRelacionar(idModalParaRelacionar) {
            let modal = this.ObtenerModalParaRelacionar(idModalParaRelacionar);
            if (modal === undefined)
                throw new Error(`Modal ${idModalParaRelacionar} no definida`);
            modal.AbrirModalDeRelacion();
        }
        AbrirModalParaConsultarRelaciones(idModalParaConsultar) {
            if (this.InfoSelector.Cantidad != 1)
                throw new Error("Debe seleccionar un elemento para poder consultar las relaciones");
            let modal = this.ObtenerModalParaConsultarRelaciones(idModalParaConsultar);
            if (modal === undefined)
                throw new Error(`Modal ${idModalParaConsultar} no definida`);
            modal.AbrirModalParaConsultarRelaciones(this.InfoSelector.LeerElemento(0));
        }
        AbrirModalBorrarElemento() {
            if (this.InfoSelector.Cantidad == 0)
                throw new Error(`Debe seleccionar el elemento a borrar, ha seleccionado ${this.InfoSelector.Cantidad}`);
            this.AbrirModalDeBorrar();
        }
        AbrirModalDeBorrar() {
            this.ModoTrabajo = ModoTrabajo.borrando;
            this.ModalDeBorrado.style.display = 'block';
            let mensaje = document.getElementById(`${this._idModalBorrar}-mensaje`);
            if (this.InfoSelector.Cantidad > 1) {
                mensaje.value = "Seguro desea borrar todos los elementos seleccionados";
            }
            else {
                mensaje.value = "Seguro desea borrar el elemento seleccionado";
            }
        }
        BorrarElemento() {
            let url = this.DefinirPeticionDeBorrado();
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.EndPoint.Borrar, null, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, this.DespuesDeBorrar, this.SiHayErrorTrasPeticionDeBorrar);
            a.Ejecutar();
        }
        DespuesDeBorrar(peticion) {
            let mantenimiento = peticion.llamador;
            mantenimiento.CerrarModalDeBorrado();
            mantenimiento.InfoSelector.QuitarTodos();
            mantenimiento.Buscar(atGrid.accion.buscar, 0);
        }
        SiHayErrorTrasPeticionDeBorrar(peticion) {
            let mantenimiento = peticion.llamador;
            mantenimiento.CerrarModalDeBorrado();
            mantenimiento.BlanquearTodosLosCheck();
            mantenimiento.SiHayErrorTrasPeticionAjax(peticion);
        }
        CerrarModalDeBorrado() {
            this.ModoTrabajo = ModoTrabajo.mantenimiento;
            ApiCrud.CerrarModal(this.ModalDeBorrado);
        }
        IraEditar() {
            if (this.InfoSelector.Cantidad == 0) {
                Mensaje(TipoMensaje.Info, "Debe marcar el elemento a editar");
                return;
            }
            this.crudDeEdicion.ComenzarEdicion(this.InfoSelector);
        }
        CerrarModalDeEdicion() {
            this.crudDeEdicion.EjecutarAcciones(Evento.Edicion.Cerrar);
        }
        ModificarElemento() {
            this.crudDeEdicion.EjecutarAcciones(Evento.ModalEdicion.Modificar);
        }
        IraCrear() {
            this.crudDeCreacion.ComenzarCreacion();
        }
        CrearElemento() {
            this.crudDeCreacion.EjecutarAcciones(Evento.Creacion.Crear);
        }
        CerrarModalDeCreacion() {
            this.crudDeCreacion.EjecutarAcciones(Evento.Creacion.Cerrar);
        }
        RestaurarPagina() {
            this.Navegador.EsRestauracion = false;
            let cantidad = this.Navegador.Cantidad;
            let pagina = this.Navegador.Pagina;
            if (pagina <= 1)
                this.CargarGrid(atGrid.accion.buscar, 0);
            else {
                let posicion = (pagina - 1) * cantidad;
                if (posicion < 0)
                    posicion = 0;
                this.Buscar(atGrid.accion.restaurar, posicion);
            }
        }
        DefinirPeticionDeBorrado() {
            let idsJson = JSON.stringify(this.InfoSelector.Seleccionados);
            var controlador = this.Navegador.Controlador;
            let url = `/${controlador}/${Ajax.EndPoint.Borrar}`;
            let parametros = `${Ajax.Param.idsJson}=${idsJson}`;
            let peticion = url + '?' + parametros;
            return peticion;
        }
        Buscar(accion, posicion) {
            if (this.Navegador.EsRestauracion) {
                this.RestaurarPagina();
            }
            else {
                this.CargarGrid(accion, posicion);
            }
        }
        CambiarSelector(idSelector) {
            var htmlSelector = document.getElementById(idSelector);
            let modal = Crud.crudMnt.ObtenerModalDeSeleccion(htmlSelector.getAttribute(atSelector.idModal));
            if (IsNullOrEmpty(htmlSelector.value))
                modal.InicializarModalDeSeleccion();
            else
                modal.TextoSelectorCambiado();
        }
        MapearRestrictorDeFiltro(porpiedadRestrictora, valorRestrictor, valorMostrar) {
            let restrictoresDeFiltro = this.ZonaDeFiltro.querySelectorAll(`input[${atControl.tipo}="${TipoControl.restrictorDeFiltro}"]`);
            if (restrictoresDeFiltro.length == 0)
                throw new Error("No se ha definido un Editor del tipo Restrictor en la zona de filtrado");
            this.MapearRestrictor(restrictoresDeFiltro, porpiedadRestrictora, valorMostrar, valorRestrictor);
        }
        OcultarMostrarFiltro() {
            if (NumeroMayorDeCero(this.ExpandirFiltro.value)) {
                ApiCrud.OcultarPanel(this.ZonaDeFiltro);
                this.ExpandirFiltro.value = "0";
                this.EtiquetaMostrarOcultarFiltro.innerText = "Mostrar filtro";
            }
            else {
                this.ExpandirFiltro.value = "1";
                ApiCrud.MostrarPanel(this.ZonaDeFiltro);
                this.EtiquetaMostrarOcultarFiltro.innerText = "Ocultar filtro";
            }
            this.PosicionarPanelesDelCuerpo();
        }
        OcultarMostrarBloque(idHtmlBloque) {
            let extensor = document.getElementById(`expandir.${idHtmlBloque}.input`);
            if (NumeroMayorDeCero(extensor.value)) {
                extensor.value = "0";
                ApiCrud.OcultarPanel(document.getElementById(`${idHtmlBloque}`));
            }
            else {
                extensor.value = "1";
                ApiCrud.MostrarPanel(document.getElementById(`${idHtmlBloque}`));
            }
            this.PosicionarPanelesDelCuerpo();
        }
    }
    Crud.CrudMnt = CrudMnt;
})(Crud || (Crud = {}));
//# sourceMappingURL=CrudMantenimiento.js.map