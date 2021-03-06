var TrabajosSometido;
(function (TrabajosSometido) {
    TrabajosSometido.crudTu = null;
    function CrearCrudDeTrabajosDeUsuario(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new TrabajosSometido.CrudDeTrabajosDeUsuario(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        TrabajosSometido.crudTu = Crud.crudMnt;
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
    }
    TrabajosSometido.CrearCrudDeTrabajosDeUsuario = CrearCrudDeTrabajosDeUsuario;
    const idsometedor = 'idsometedor';
    class CrudDeTrabajosDeUsuario extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionTrabajoDeUsuario(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionTrabajoDeUsuario(this, idPanelEdicion);
        }
        Inicializar(idPanelMnt) {
            super.Inicializar(idPanelMnt);
            this.MapearUsuarioConectado();
        }
        MapearUsuarioConectado() {
            function usuarioNoLeido(llamador) {
                let zonaDeMenu = llamador.CrudDeMnt.ZonaDeMenu;
                ApiControl.BloquearMenu(zonaDeMenu);
                console.error("no se ha podido leer");
            }
            let usuarioConectado = Registro.UsuarioConectado();
            if (usuarioConectado == null) {
                usuarioNoLeido(this.crudDeCreacion);
            }
            else {
                let idUsuario = usuarioConectado['id'];
                let usuario = usuarioConectado['login'];
                ApiControl.MapearPropiedadRestrictoraAlControl(this.crudDeCreacion.PanelDeCrear, idsometedor, idUsuario, usuario);
            }
        }
        AjustarOpcionesDeMenuDelElemento(elemento, modoAcceso) {
            super.AjustarOpcionesDeMenuDelElemento(elemento, modoAcceso);
            let estado = elemento['estado'];
            switch (estado) {
                case 'erroneo': {
                    this.ActivarOpciones([]);
                    this.DesactivarOpciones([]);
                    break;
                }
                default: {
                    MensajesSe.Error('AjustarOpcionesDeMenuDelElemento', `No está definido que hacer con el estado ${estado} de un trabajo`);
                    this.DeshabilitarOpcionesDeMenuDeElemento();
                    break;
                }
            }
        }
        ActivarOpciones(opcionesDeMenu) {
        }
        DesactivarOpciones(opcionesDeMenu) {
        }
        IniciarTrabajo() {
            if (this.InfoSelector.Cantidad != 1) {
                Notificar(TipoMensaje.Info, "Solo se puede iniciar un trabajo");
                return;
            }
            this.EjecutarTrabajoDeUsuario();
        }
        BloquearTrabajo() {
            for (let i = 0; i < this.InfoSelector.Cantidad; i++)
                this.BloquearTrabajoDeUsuario(this.InfoSelector.LeerId(i));
        }
        DesbloquearTrabajo() {
            for (let i = 0; i < this.InfoSelector.Cantidad; i++)
                this.DesbloquearTrabajoDeUsuario(this.InfoSelector.LeerId(i));
        }
        EjecutarTrabajoDeUsuario() {
            let idTrabajoDeUsuario = this.InfoSelector.LeerId(0);
            let url = `/${Ajax.TrabajosSometidos.rutaTu}/${Ajax.TrabajosSometidos.accion.iniciar}?idTrabajoUsuario=${idTrabajoDeUsuario}`;
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.TrabajosSometidos.accion.iniciar, idTrabajoDeUsuario, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, this.TrasEjecutarTrabajo, this.SiHayErrorDeEjecucion);
            a.Ejecutar();
        }
        BloquearTrabajoDeUsuario(idTrabajoDeUsuario) {
            let url = `/${Ajax.TrabajosSometidos.rutaTu}/${Ajax.TrabajosSometidos.accion.bloquear}?idTrabajoUsuario=${idTrabajoDeUsuario}`;
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.TrabajosSometidos.accion.bloquear, idTrabajoDeUsuario, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, this.TrasEjecutarTrabajo, this.SiHayErrorDeEjecucion);
            a.Ejecutar();
        }
        DesbloquearTrabajoDeUsuario(idTrabajoDeUsuario) {
            let url = `/${Ajax.TrabajosSometidos.rutaTu}/${Ajax.TrabajosSometidos.accion.desbloquear}?idTrabajoUsuario=${idTrabajoDeUsuario}`;
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.TrabajosSometidos.accion.desbloquear, idTrabajoDeUsuario, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, this.TrasEjecutarTrabajo, this.SiHayErrorDeEjecucion);
            a.Ejecutar();
        }
        SiHayErrorDeEjecucion(peticion) {
            MensajesSe.Error(peticion.nombre, peticion.resultado.mensaje, peticion.resultado.consola);
            let crudTu = peticion.llamador;
            crudTu.CargarGrid(atGrid.accion.buscar, 0);
        }
        TrasEjecutarTrabajo(peticion) {
            Notificar(TipoMensaje.Info, peticion.resultado.mensaje);
            let crudTu = peticion.llamador;
            crudTu.CargarGrid(atGrid.accion.buscar, 0);
        }
    }
    TrabajosSometido.CrudDeTrabajosDeUsuario = CrudDeTrabajosDeUsuario;
    class CrudCreacionTrabajoDeUsuario extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    TrabajosSometido.CrudCreacionTrabajoDeUsuario = CrudCreacionTrabajoDeUsuario;
    class CrudEdicionTrabajoDeUsuario extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    TrabajosSometido.CrudEdicionTrabajoDeUsuario = CrudEdicionTrabajoDeUsuario;
    function Eventos(accion) {
        try {
            switch (accion) {
                case Evento.TrabajoDeUsuario.iniciar: {
                    TrabajosSometido.crudTu.IniciarTrabajo();
                    break;
                }
                case Evento.TrabajoDeUsuario.bloquear: {
                    TrabajosSometido.crudTu.BloquearTrabajo();
                    break;
                }
                case Evento.TrabajoDeUsuario.desbloquear: {
                    TrabajosSometido.crudTu.DesbloquearTrabajo();
                    break;
                }
                default: {
                    Notificar(TipoMensaje.Error, `la opción ${accion} no está definida`);
                    break;
                }
            }
        }
        catch (error) {
            Notificar(TipoMensaje.Error, error);
        }
    }
    TrabajosSometido.Eventos = Eventos;
})(TrabajosSometido || (TrabajosSometido = {}));
//# sourceMappingURL=TrabajosDeUsuario.js.map