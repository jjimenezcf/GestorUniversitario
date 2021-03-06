var Crud;
(function (Crud) {
    Crud.crudMnt = null;
    class CrudMnt extends Crud.GridDeDatos {
        constructor(idPanelMnt, idModalDeBorrado) {
            super(idPanelMnt);
            this.ModalesDeSeleccion = new Array();
            this.ModalesParaRelacionar = new Array();
            this.ModalesParaConsultarRelaciones = new Array();
            this.ModalesParaSeleccionar = new Array();
            this._idModalBorrar = idModalDeBorrado;
        }
        get ModoAccesoDelUsuario() {
            return this._modoAccesoDelUsuario;
        }
        set ModoAccesoDelUsuario(modoDeAcceso) {
            this._modoAccesoDelUsuario = modoDeAcceso;
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
        get ModalDeExportacion() {
            let idModal = this.IdCuerpoCabecera;
            idModal = idModal.replace('mantenimiento', '');
            idModal = idModal + 'panel-exportacion';
            return document.getElementById(idModal);
        }
        ;
        get ModalDeEnviarCorreo() {
            let idModal = this.IdCuerpoCabecera;
            idModal = idModal.replace('mantenimiento', '');
            idModal = idModal + 'panel-enviar-correo';
            return document.getElementById(idModal);
        }
        ;
        get ModalDeEnviarCorreo_DivDeElementos() {
            return document.getElementById(`${this.ModalDeEnviarCorreo.id}_elementos_ref`);
        }
        ;
        get OpcionesGenerales() {
            return this.ZonaDeMenu.querySelectorAll(`input[${atOpcionDeMenu.clase}="${ClaseDeOpcioDeMenu.DeVista}"]`);
        }
        NavegarDesdeElBrowser() {
            //MensajesSe.Info('Ha llamado al método navegar');
        }
        Inicializar(idPanelMnt) {
            const querystring = window.location.search;
            const params = new URLSearchParams(querystring);
            try {
                if (IsNullOrEmpty(idPanelMnt))
                    idPanelMnt = this.IdCuerpoCabecera;
                super.Inicializar(this.IdCuerpoCabecera);
                this.ModoTrabajo = ModoTrabajo.mantenimiento;
                this.InicializarSelectores();
                this.InicializarListasDeElementos(this.ZonaDeFiltro, this.Navegador.Controlador);
                this.InicializarMenus();
                this.InicializarSelectoresDeFecha(this.ZonaDeFiltro);
                if (params.has(atParametrosUrl.origen))
                    this.AplicarRestrictores(params.get(atParametrosUrl.origen));
                if (this.Navegador.EsRestauracion) {
                    this.RestaurarPagina()
                        .then((valor) => this.TrasRestaurar(valor));
                }
                else {
                    if (params.has(atParametrosUrl.id))
                        this.EditarRegistro(Numero(params.get(atParametrosUrl.id)));
                    this.InicializarOrdenacion();
                    this.Buscar(atGrid.accion.buscar, 0);
                }
            }
            catch (error) {
                MensajesSe.Error("Inicializando el crud", `Error al inicializar el crud ${this.IdCuerpoCabecera}`, error.message);
            }
        }
        EditarRegistro(id) {
            this.FiltrarPorId(id)
                .then(() => this.IraEditar());
        }
        TrasRestaurar(valor) {
            if (valor && this.Estado.Obtener("EditarAlVolver")) {
                this.Estado.Quitar("EditarAlVolver");
                EntornoSe.Historial.GuardarEstadoDePagina(this.Estado);
                EntornoSe.Historial.Persistir();
                this.IraEditar();
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
                else {
                    this.PosicionarFiltro();
                    this.PosicionarGrid();
                }
            }
            if (this.ModoTrabajo === ModoTrabajo.creando || this.ModoTrabajo === ModoTrabajo.copiando) {
                if (!this.crudDeCreacion.EsModal)
                    this.crudDeCreacion.PosicionarCreacion();
                else {
                    this.PosicionarFiltro();
                    this.PosicionarGrid();
                }
            }
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
        AplicarRestrictores(origen) {
            if (this.Estado.Contiene(Sesion.restrictores)) {
                let restrictores = this.Estado.Obtener(Sesion.restrictores);
                for (let i = 0; i < restrictores.length; i++) {
                    this.AplicarRestrictor(restrictores[i]);
                }
            }
        }
        AplicarRestrictor(restrictor) {
            MapearPanelDeFiltro.MapearRestrictores(this.ZonaDeFiltro, restrictor.Propiedad, restrictor.Valor, restrictor.Texto);
            if (!EsTrue(this.Estado.Obtener(Sesion.SoloMapearEnElFiltro))) {
                if (EsTrue(this.CuerpoCabecera.getAttribute(atMantenimniento.permiteCrear)))
                    MapearPanelDeCreacion.MapearRestrictores(this.crudDeCreacion.PanelDeCrear, restrictor.Propiedad, restrictor.Valor, restrictor.Texto);
                if (EsTrue(this.CuerpoCabecera.getAttribute(atMantenimniento.permiteEditar)))
                    MapearPanelDeEdicion.MapearRestrictores(this.crudDeEdicion.PanelDeEditar, restrictor.Propiedad, restrictor.Valor, restrictor.Texto);
            }
            this.DespuesDeAplicarUnRestrictor(restrictor);
        }
        DespuesDeAplicarUnRestrictor(restrictor) {
            //método para sobrecargar que analiza el tipo de restrictor aplicado y obtirnr información de si se ha de restringir o mapear más información
            //P.eje: Al mapear una provincia al filtro de municipios, mapear el país
        }
        InicializarSelectores() {
            let selectores = this.ZonaDeFiltro.querySelectorAll(`input[tipo="${TipoControl.Selector}"]`);
            selectores.forEach((selector) => {
                let idModal = selector.getAttribute(atSelectorDeFiltro.idModal);
                let modal = new Crud.ModalSeleccion(idModal);
                modal.InicializarModalDeSeleccion();
                this.ModalesDeSeleccion.push(modal);
            });
        }
        InicializarMenus() {
            this.DeshabilitarOpcionesDeMenuDeElemento();
            if (this.ModoAccesoDelUsuario === undefined) {
                ApiDePeticiones.LeerModoDeAccesoAlNegocio(this, this.Controlador, this.Negocio)
                    .then((peticion) => this.AplicarModoDeAccesoAlNegocio(peticion))
                    .catch((peticion) => this.ErrorAlLeerModoAccesoAlNegocio(peticion));
            }
            else {
                ModoAcceso.AplicarModoDeAccesoAlNegocio(this.OpcionesGenerales, this.ModoAccesoDelUsuario);
            }
        }
        ErrorAlLeerModoAccesoAlNegocio(peticion) {
            ApiDeAjax.ErrorTrasPeticion("Leer modo de acceso al negocio", peticion);
            ApiControl.BloquearMenu(this.Cuerpo);
        }
        AplicarModoDeAccesoAlNegocio(peticion) {
            let mantenimiento = peticion.llamador;
            let modoDeAccesoDelUsuario = ModoAcceso.Parsear(peticion.resultado.modoDeAcceso);
            mantenimiento.ModoAccesoDelUsuario = modoDeAccesoDelUsuario;
            ModoAcceso.AplicarModoDeAccesoAlNegocio(mantenimiento.OpcionesGenerales, modoDeAccesoDelUsuario);
        }
        ObtenerGrid(idModal) {
            if (IsNullOrEmpty(idModal))
                return this;
            let grid = this.ModalDeSeleccionAsociada(idModal);
            if (Definida(grid))
                return grid;
            grid = this.ModalParaSeleccionAsociada(idModal);
            if (Definida(grid))
                return grid;
            grid = this.ModalParaConsultarRelacionesAsociada(idModal);
            if (Definida(grid))
                return grid;
            grid = this.ModalParaRelacionesAsociada(idModal);
            if (Definida(grid))
                return grid;
            MensajesSe.EmitirExcepcion("ObtenerGrid", `Se busca la modal con id ${idModal} y no se ha encontrado`);
        }
        ModalParaRelacionesAsociada(idModal) {
            for (let i = 0; i < this.ModalesParaRelacionar.length; i++) {
                let modal = this.ModalesParaRelacionar[i];
                if (modal.IdModal === idModal)
                    return modal;
            }
            return undefined;
        }
        ModalParaConsultarRelacionesAsociada(idModal) {
            for (let i = 0; i < this.ModalesParaConsultarRelaciones.length; i++) {
                let modal = this.ModalesParaConsultarRelaciones[i];
                if (modal.IdModal === idModal)
                    return modal;
            }
            return undefined;
        }
        ModalParaSeleccionAsociada(idModal) {
            for (let i = 0; i < this.ModalesParaSeleccionar.length; i++) {
                let modal = this.ModalesParaSeleccionar[i];
                if (modal.IdModal === idModal)
                    return modal;
            }
            return undefined;
        }
        ModalDeSeleccionAsociada(idModal) {
            for (let i = 0; i < this.ModalesDeSeleccion.length; i++) {
                let modal = this.ModalesDeSeleccion[i];
                if (modal.IdModal === idModal)
                    return modal;
            }
            return undefined;
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
        ModalDeBorrado_Abrir() {
            if (this.InfoSelector.Cantidad == 0)
                throw new Error(`Debe seleccionar el elemento a borrar, ha seleccionado ${this.InfoSelector.Cantidad}`);
            this.modalDeBorrardo_Abrir();
        }
        modalDeBorrardo_Abrir() {
            this.ModoTrabajo = ModoTrabajo.borrando;
            this.ModalDeBorrado.style.display = 'block';
            EntornoSe.AjustarModalesAbiertas();
            let mensaje = document.getElementById(`${this._idModalBorrar}-mensaje`);
            if (this.InfoSelector.Cantidad > 1) {
                mensaje.value = "Seguro desea borrar todos los elementos seleccionados";
            }
            else {
                mensaje.value = "Seguro desea borrar el elemento seleccionado";
            }
        }
        ModalDeBorrado_Borrar() {
            let url = this.DefinirPeticionDeBorrado();
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.EndPoint.Borrar, null, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, this.DespuesDeBorrar, this.SiHayErrorTrasPeticionDeBorrar);
            a.Ejecutar();
        }
        DespuesDeBorrar(peticion) {
            let mantenimiento = peticion.llamador;
            mantenimiento.ModalDeBorrado_Cerrar();
            mantenimiento.InfoSelector.QuitarTodos();
            mantenimiento.Buscar(atGrid.accion.buscar, 0);
        }
        SiHayErrorTrasPeticionDeBorrar(peticion) {
            let mantenimiento = peticion.llamador;
            mantenimiento.ModalDeBorrado_Cerrar();
            mantenimiento.BlanquearTodosLosCheck();
            mantenimiento.SiHayErrorTrasPeticionAjax(peticion);
        }
        ModalDeBorrado_Cerrar() {
            this.ModoTrabajo = ModoTrabajo.mantenimiento;
            ApiCrud.CerrarModal(this.ModalDeBorrado);
        }
        IraEditar() {
            if (this.InfoSelector.Cantidad == 0) {
                MensajesSe.Error("IraEditar", "Debe marcar el elemento a editar");
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
            this.DatosDelGrid.InicializarCache();
            this.Navegador.EsRestauracion = false;
            let cantidad = this.Navegador.Cantidad;
            let pagina = this.Navegador.Pagina;
            let posicion = 0;
            let accion = atGrid.accion.buscar;
            if (pagina > 1) {
                posicion = (pagina - 1) * cantidad;
                accion = atGrid.accion.restaurar;
            }
            return this.PromesaDeCargarGrid(accion, posicion);
        }
        DefinirPeticionDeBorrado() {
            let idsJson = JSON.stringify(this.InfoSelector.IdsSeleccionados);
            var controlador = this.Navegador.Controlador;
            let url = `/${controlador}/${Ajax.EndPoint.Borrar}`;
            let parametros = `${Ajax.Param.idsJson}=${idsJson}`;
            let peticion = url + '?' + parametros;
            return peticion;
        }
        Buscar(accion, posicion) {
            this.DatosDelGrid.InicializarCache();
            if (accion !== atGrid.accion.restaurar) {
                this.Navegador.Pagina = 1;
                this.Navegador.Posicion = 0;
            }
            this.CargarGrid(accion, posicion);
        }
        CambiarSelector(idSelector) {
            var htmlSelector = document.getElementById(idSelector);
            let modal = Crud.crudMnt.ObtenerModalDeSeleccion(htmlSelector.getAttribute(atSelectorDeFiltro.idModal));
            if (IsNullOrEmpty(htmlSelector.value))
                modal.InicializarModalDeSeleccion();
            else
                modal.TextoSelectorCambiado();
        }
        ValidarRestrictorDeFiltrado() {
            let restrictoresDeFiltro = this.ZonaDeFiltro.querySelectorAll(`input[${atControl.tipo}="${TipoControl.restrictorDeFiltro}"]`);
            if (restrictoresDeFiltro.length == 0)
                return false;
            return true;
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
        ModalEnviarCorreo_Abrir() {
            this.ModoTrabajo = ModoTrabajo.enviandoCorreo;
            this.ModalDeEnviarCorreo.style.display = 'block';
            for (let i = 0; i < this.InfoSelector.Cantidad; i++)
                ApiCrud.CrearEnlaceAlElemento(this.ModalDeEnviarCorreo_DivDeElementos, this.InfoSelector.LeerElemento(i));
            EntornoSe.AjustarModalesAbiertas();
        }
        ModalEnviarCorreo_Cerrar() {
            this.ModoTrabajo = ModoTrabajo.mantenimiento;
            this.ModalDeEnviarCorreo_DivDeElementos.innerHTML = "";
            ApiCrud.CerrarModal(this.ModalDeEnviarCorreo);
        }
        ModalEnviarCorreo_Enviar() {
            let parametros = this.ParametrosDeEnviarCorreos();
            ApiDePeticiones.EnviarCorreo(this, this.Controlador, parametros)
                .then((peticion) => this.DespuesDeEnviarCorreo(peticion))
                .catch((peticion) => this.ErrorAlEnviarCorreo(peticion));
        }
        DespuesDeEnviarCorreo(peticion) {
            MensajesSe.Info(peticion.resultado.mensaje);
        }
        ErrorAlEnviarCorreo(peticion) {
            MensajesSe.Error(peticion.nombre, peticion.resultado.mensaje, peticion.resultado.consola);
        }
        ParametrosDeEnviarCorreos() {
            let parametros = new Array();
            let idPuestos = this.ModalDeEnviarCorreo.id + '_selector-puestos_editor';
            let idUsuarios = this.ModalDeEnviarCorreo.id + '_selector-usuario_editor';
            let idAsunto = this.ModalDeEnviarCorreo.id + '_asunto';
            let idCuerpo = this.ModalDeEnviarCorreo.id + '_mensaje';
            let idAjuntos = this.ModalDeEnviarCorreo.id + '_elementos_ref';
            let idsDeUsuarios = document.getElementById(idUsuarios).getAttribute(atSelectorDeElementos.Seleccionados);
            let usuarios = ToLista(idsDeUsuarios, ',');
            let idsDePuestos = document.getElementById(idPuestos).getAttribute(atSelectorDeElementos.Seleccionados);
            let puestos = ToLista(idsDePuestos, ',');
            if (puestos.length == 0 && usuarios.length == 0)
                throw new Error("Al menos debe definir un receptor");
            let asunto = document.getElementById(idAsunto).value;
            let cuerpo = document.getElementById(idCuerpo).value;
            if (IsNullOrEmpty(asunto))
                throw new Error("Debe indicar el asunto");
            let divAdjuntos = document.getElementById(idAjuntos);
            let adjuntos = [];
            let refAdjuntos = divAdjuntos.querySelectorAll("a");
            for (let i = 0; i < refAdjuntos.length; i++) {
                let adjunto = new Tipos.TipoDtoElmento(this.Dto, Numero(refAdjuntos[i].getAttribute(atControl.idElemento)), refAdjuntos[i].text);
                adjuntos.push(adjunto);
            }
            parametros.push(new Parametro(Ajax.Param.negocio, this.Negocio));
            parametros.push(new Parametro('usuarios', usuarios));
            parametros.push(new Parametro('puestos', puestos));
            parametros.push(new Parametro('asunto', asunto));
            parametros.push(new Parametro('cuerpo', cuerpo));
            parametros.push(new Parametro('adjuntos', adjuntos));
            return parametros;
        }
        ObtenerModalParaSeleccionar(idModal) {
            for (let i = 0; i < this.ModalesParaSeleccionar.length; i++) {
                let modal = this.ModalesParaSeleccionar[i];
                if (modal.IdModal === idModal)
                    return modal;
            }
            let modal = new Crud.ModalParaSeleccionar(this, idModal);
            this.ModalesParaSeleccionar.push(modal);
            return modal;
        }
        ObtenerFocoEnSelector(idSelector) {
            let selector = ApiCrud.ObtenerSelector(idSelector);
            let editor = ApiCrud.ObtenerEditorAsociadoAlSelector(selector);
            editor.setAttribute(atSelectorDeElementos.ValorAlEntrar, editor.value);
        }
        PerderElFocoEnUnSelectorDesdeUnaModal(idModalQueSeAbre, idModalQueSeCierra, idSelector) {
            let selector = ApiCrud.ObtenerSelector(idSelector);
            let editor = ApiCrud.ObtenerEditorAsociadoAlSelector(selector);
            let valorAlEntrar = editor.getAttribute(atSelectorDeElementos.ValorAlEntrar);
            if (editor.value === valorAlEntrar || IsNullOrEmpty(editor.value))
                return;
            selector.setAttribute(atSelectorDeElementos.CerrarAutomaticamente, 'S');
            this.AbrirModalParaSeleccionarDesdeUnaModal(idModalQueSeAbre, idModalQueSeCierra, idSelector);
        }
        AbrirModalParaSeleccionarDesdeUnaModal(idModalQueSeAbre, idModalQueSeCierra, idSelector) {
            ApiCrud.OcultarModalPorId(idModalQueSeCierra);
            let modal = this.ObtenerModalParaSeleccionar(idModalQueSeAbre);
            if (NoDefinida(modal))
                throw new Error(`Modal ${idModalQueSeAbre} no definida`);
            let selector = ApiCrud.ObtenerSelector(idSelector);
            selector.setAttribute(atSelectorDeElementos.ModalPadre, idModalQueSeCierra);
            modal.AbrirModalParaSeleccionar(selector);
        }
        ModalExportacion_Abrir() {
            this.ModoTrabajo = ModoTrabajo.exportando;
            this.ModalDeExportacion.style.display = 'block';
            EntornoSe.AjustarModalesAbiertas();
        }
        ModalExportacion_Cerrar() {
            this.ModoTrabajo = ModoTrabajo.mantenimiento;
            ApiCrud.CerrarModal(this.ModalDeExportacion);
        }
        ModalExportacion_SalirDeListaDeCorreos() {
            let idCorreos = this.ModalDeExportacion.id + '_correos';
            let correos = document.getElementById(idCorreos);
            if (!IsNullOrEmpty(correos.value)) {
                ApiControl.AnularError(correos);
                let lista = correos.value.split(';');
                let correoMalo = this.ValidarListaDeCorreos(lista);
                if (!IsNullOrEmpty(correoMalo)) {
                    ApiControl.MarcarError(correos);
                    throw Error(`El correo ${correoMalo} no es válido`);
                }
            }
        }
        ValidarListaDeCorreos(lista) {
            for (let i = 0; i < lista.length; i++) {
                if (!EsCorreoValido(lista[i].trim())) {
                    return lista[i].trim();
                }
            }
            return '';
        }
        ModalExportacion_CheckSometerPulsado() {
            let idCheck = this.ModalDeExportacion.id + '_sometido';
            let idCorreos = this.ModalDeExportacion.id + '_correos';
            let check = document.getElementById(idCheck);
            let correos = document.getElementById(idCorreos);
            if (check.checked) {
                ApiControl.DesbloquearEditor(correos);
                correos.value = Registro.UsuarioConectado().mail;
            }
            else {
                ApiControl.BlanquearEditor(correos);
                ApiControl.BloquearEditor(correos);
            }
        }
        ModalExportacion_Exportar() {
            let parametros = this.ParametrosDeExportacion();
            ApiDePeticiones.Exportar(this, this.Controlador, parametros)
                .then((peticion) => this.DespuesDeExportar(peticion))
                .catch((peticion) => this.ErrorAlExportar(peticion));
        }
        ParametrosDeExportacion() {
            let parametros = new Array();
            let idMostradas = this.ModalDeExportacion.id + '_mostradas';
            let idSometido = this.ModalDeExportacion.id + '_sometido';
            let idCorreo = this.ModalDeExportacion.id + '_correos';
            let mostradas = document.getElementById(idMostradas).checked;
            let controlSometido = document.getElementById(idSometido);
            let parametroSometido = controlSometido.getAttribute(atControl.propiedad);
            let sometido = controlSometido.checked;
            let controlReceptor = document.getElementById(idCorreo);
            let parametroReceptor = controlReceptor.getAttribute(atControl.propiedad);
            let receptores = controlReceptor.value;
            if (sometido && IsNullOrEmpty(receptores))
                throw Error(`Debe indicar al menos un correo`);
            let lista = receptores.split(';');
            let correoMalo = this.ValidarListaDeCorreos(lista);
            if (!IsNullOrEmpty(correoMalo)) {
                ApiControl.MarcarError(document.getElementById(idCorreo));
                throw Error(`El correo ${correoMalo} no es válido`);
            }
            let posicion = 0;
            let cantidad = -1;
            if (mostradas) {
                cantidad = this.Navegador.Cantidad;
                posicion = this.Navegador.Posicion;
                posicion = posicion - cantidad;
                if (posicion < 0)
                    posicion = 0;
            }
            parametros.push(new Parametro(Ajax.Param.negocio, this.Negocio));
            parametros.push(new Parametro(Ajax.Param.posicion, posicion));
            parametros.push(new Parametro(Ajax.Param.cantidad, cantidad));
            parametros.push(new Parametro(parametroSometido, sometido));
            parametros.push(new Parametro(parametroReceptor, receptores));
            parametros.push(new Parametro(Ajax.Param.filtro, this.ObtenerFiltros()));
            parametros.push(new Parametro(Ajax.Param.orden, this.ObtenerOrdenacion()));
            return parametros;
        }
        DespuesDeExportar(peticion) {
            let crud = peticion.llamador;
            let parametros = peticion.DatosDeEntrada;
            for (let i = 0; i < parametros.length; i++) {
                if (parametros[i].Parametro === 'sometido' && (parametros[i].valor)) {
                    MensajesSe.Info(peticion.resultado.mensaje);
                    return;
                }
            }
            crud.DescargarArchivo(peticion);
        }
        DescargarArchivo(peticion) {
            var downloadLink = document.createElement("a");
            document.body.appendChild(downloadLink);
            downloadLink.href = peticion.resultado.datos;
            downloadLink.click();
            document.body.removeChild(downloadLink);
        }
        ErrorAlExportar(peticion) {
            MensajesSe.Error(peticion.nombre, peticion.resultado.mensaje, peticion.resultado.consola);
        }
    }
    Crud.CrudMnt = CrudMnt;
})(Crud || (Crud = {}));
//# sourceMappingURL=CrudMantenimiento.js.map