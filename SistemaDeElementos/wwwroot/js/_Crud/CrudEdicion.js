var Crud;
(function (Crud) {
    class CrudEdicion extends Crud.CrudBase {
        //private get Controlador(): string {
        //    return this.PanelDeEditar.getAttribute(literal.controlador);
        //}
        constructor(crud, idPanelEdicion) {
            super();
            if (IsNullOrEmpty(idPanelEdicion))
                throw Error("No se puede construir un objeto del tipo CrudEdicion sin indica el panel de edición");
            this._idPanelEdicion = idPanelEdicion;
            this.PanelDeMnt = crud.CuerpoCabecera;
            this._controlador = this.PanelDeEditar.getAttribute(literal.controlador);
            this.CrudDeMnt = crud;
        }
        get PanelDeEditar() {
            return document.getElementById(this._idPanelEdicion);
        }
        get EsModal() {
            return this.PanelDeEditar.className === ClaseCss.contenedorModal;
        }
        get PanelDeContenidoModal() {
            return document.getElementById(`${this._idPanelEdicion}_contenido`);
        }
        get InfoSelectorEdicion() {
            return this._infoSelectorEdicion;
        }
        set InfoSelectorEdicion(info) {
            this._infoSelectorEdicion = info;
            this.TotalSeleccionados = info.Cantidad;
            this.Posicionador = 1;
        }
        get Posicionador() {
            let control = document.getElementById(`${this._idPanelEdicion}-posicionador`);
            return Numero(control.value);
        }
        set Posicionador(posicionador) {
            let control = document.getElementById(`${this._idPanelEdicion}-posicionador`);
            control.value = posicionador.toString();
        }
        get TotalSeleccionados() {
            return this.InfoSelectorEdicion.Cantidad;
        }
        set TotalSeleccionados(cantidad) {
            let control = document.getElementById(`${this._idPanelEdicion}-total-seleccionados`);
            control.value = cantidad.toString();
        }
        get Elemento() {
            if (this.Posicionador === 0)
                this.Posicionador = 1;
            return this.InfoSelectorEdicion.Seleccionados[this.Posicionador - 1];
        }
        get IdEditor() {
            var control = this.BuscarEditor(this.PanelDeEditar, literal.id);
            if (control == null) {
                MensajesSe.Error("IdEditor", "No está definido el control para mostrar el id del elemento");
                this.CerrarEdicion();
            }
            return control;
        }
        EjecutarAcciones(accion) {
            let cerrarEdicion = false;
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
                MensajesSe.Error("EjecutarAcciones", error.message);
            }
            if (cerrarEdicion)
                this.CerrarEdicion();
        }
        ComenzarEdicion(infSel) {
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
        PosicionarEdicion() {
            this.PanelDeEditar.style.position = 'fixed';
            this.PanelDeEditar.style.top = `${AlturaCabeceraPnlControl()}px`;
            this.PanelDeEditar.style.height = `${AlturaFormulario() - AlturaPiePnlControl() - AlturaCabeceraPnlControl()}px`;
        }
        EditarSeleccionado(seleccionado) {
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
                this.InicializarValores(this.Elemento.Id);
            }
        }
        CerrarEdicion() {
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
            }
            this.CrudDeMnt.ModoTrabajo = ModoTrabajo.mantenimiento;
            this.CrudDeMnt.Buscar(atGrid.accion.buscar, 0);
        }
        InicializarValores(id) {
            this.IdEditor.value = id.toString();
            let parametros = this.ParametrosOpcionalesLeerPorId();
            ApiDePeticiones.LeerElementoPorId(this, this.Controlador, id, parametros)
                .then((peticion) => this.MapearElementoDevuelto(peticion))
                .catch((peticion) => this.SiHayErrorAlLeerElemento(peticion));
        }
        LeerElementoPorId(id) {
            let parametros = this.ParametrosOpcionalesLeerPorId();
            let url = `/${this.Controlador}/${Ajax.EndPoint.LeerPorId}?${Ajax.Param.id}=${id}&${Ajax.Param.parametros}=${JSON.stringify(parametros)}`;
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.EndPoint.LeerPorId, null, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, this.MapearElementoDevuelto, this.SiHayErrorAlLeerElemento);
            a.Ejecutar();
        }
        ParametrosOpcionalesLeerPorId() {
            return new Array();
        }
        MapearElementoDevuelto(peticion) {
            let edicion = peticion.llamador;
            edicion.AntesDeMapearElementoDevuelto(peticion);
            let panel = edicion.PanelDeEditar;
            edicion.MapearElementoLeido(panel, peticion.resultado.datos, peticion.resultado.modoDeAcceso);
            edicion.AjustarOpcionesDeMenuDeEdicion(peticion.resultado.modoDeAcceso);
        }
        AntesDeMapearElementoDevuelto(peticion) {
        }
        AjustarOpcionesDeMenuDeEdicion(modoDeAcceso) {
            let opcionesDeElemento = this.PanelDeEditar.querySelectorAll(`input[${atOpcionDeMenu.clase}="${ClaseDeOpcioDeMenu.DeElemento}"]`);
            let permisosDelUsuario = modoDeAcceso;
            for (var i = 0; i < opcionesDeElemento.length; i++) {
                let opcion = opcionesDeElemento[i];
                if (ApiControl.EstaBloqueada(opcion))
                    continue;
                let permisosNecesarios = opcion.getAttribute(atOpcionDeMenu.permisosNecesarios);
                opcion.disabled = !ModoAcceso.HayPermisos(permisosNecesarios, permisosDelUsuario);
            }
        }
        SiHayErrorAlLeerElemento(peticion) {
            let edicion = peticion.llamador;
            edicion.CerrarEdicion();
            edicion.CrudDeMnt.BlanquearTodosLosCheck();
            edicion.SiHayErrorTrasPeticionAjax(peticion);
        }
        Modificar() {
            let json = ApiCrud.MapearControlesDesdeLaIuAlJson(this, this.PanelDeEditar, ModoTrabajo.editando);
            this.ModificarElemento(json);
        }
        ModificarElemento(json) {
            let controlador = this.PanelDeEditar.getAttribute(literal.controlador);
            let url = `/${controlador}/${Ajax.EndPoint.Modificar}?${Ajax.Param.elementoJson}=${JSON.stringify(json)}`;
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.EndPoint.Modificar, this, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, this.DespuesDeModificar, this.SiHayErrorTrasPeticionAjax);
            a.Ejecutar();
        }
        NavegarDesdeEdicion(url) {
            this.CrudDeMnt.Estado.Agregar("EditarAlVolver", true);
            this.CrudDeMnt.Estado.Agregar(atGrid.id, this.CrudDeMnt.Navegador.Datos);
            this.CrudDeMnt.Estado.Agregar("elementos_seleccionados", this.CrudDeMnt.InfoSelector.Seleccionados);
            EntornoSe.Historial.GuardarEstadoDePagina(this.CrudDeMnt.Estado);
            let datos = [];
            let negocio = new Tipos.DatosRestrictor('idnegocio', this.CrudDeMnt.IdNegocio, this.CrudDeMnt.Negocio);
            let elemento = new Tipos.DatosRestrictor('idelemento', this.Elemento.Id, this.Elemento.Texto);
            datos.push(negocio);
            datos.push(elemento);
            this.PrepararValoresAntesDeNavegar('crud_auditoriadto_mantenimiento', datos);
            EntornoSe.NavegarAUrl(url);
        }
        PrepararValoresAntesDeNavegar(paginaDestino, datos) {
            let estadoPaginaDestino = EntornoSe.Historial.ObtenerEstadoDePagina(paginaDestino);
            estadoPaginaDestino.Agregar(Sesion.restrictores, datos);
            EntornoSe.Historial.GuardarEstadoDePagina(estadoPaginaDestino);
        }
        DespuesDeModificar(peticion) {
            let crudEdicion = peticion.llamador;
            if (crudEdicion.TotalSeleccionados === 1) {
                crudEdicion.CerrarEdicion();
            }
        }
    }
    Crud.CrudEdicion = CrudEdicion;
})(Crud || (Crud = {}));
//# sourceMappingURL=CrudEdicion.js.map