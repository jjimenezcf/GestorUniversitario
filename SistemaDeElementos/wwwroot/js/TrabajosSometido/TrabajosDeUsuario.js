var TrabajosSometido;
(function (TrabajosSometido) {
    TrabajosSometido.crudTu = null;
    function CrearCrudDeTrabajosDeUsuario(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new TrabajosSometido.CrudDeTrabajosDeUsuario(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        TrabajosSometido.crudTu = Crud.crudMnt;
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    TrabajosSometido.CrearCrudDeTrabajosDeUsuario = CrearCrudDeTrabajosDeUsuario;
    const idsometedor = 'idsometedor';
    class TrabajoDeUsuario {
    }
    let EstadoTrabajo;
    (function (EstadoTrabajo) {
        EstadoTrabajo[EstadoTrabajo["erroneo"] = 0] = "erroneo";
        EstadoTrabajo[EstadoTrabajo["pendiente"] = 1] = "pendiente";
        EstadoTrabajo[EstadoTrabajo["bloqueado"] = 2] = "bloqueado";
        EstadoTrabajo[EstadoTrabajo["iniciado"] = 3] = "iniciado";
        EstadoTrabajo[EstadoTrabajo["terminado"] = 4] = "terminado";
        EstadoTrabajo[EstadoTrabajo["conerrores"] = 5] = "conerrores";
    })(EstadoTrabajo || (EstadoTrabajo = {}));
    function ParsearEstado(estado) {
        if (estado.toLowerCase() === 'erroneo')
            return EstadoTrabajo.erroneo;
        if (estado.toLowerCase() === 'pendiente')
            return EstadoTrabajo.pendiente;
        if (estado.toLowerCase() === 'bloqueado')
            return EstadoTrabajo.bloqueado;
        if (estado.toLowerCase() === 'iniciado')
            return EstadoTrabajo.iniciado;
        if (estado.toLowerCase() === 'terminado')
            return EstadoTrabajo.terminado;
        if (estado.toLowerCase() === 'con errores')
            return EstadoTrabajo.conerrores;
        throw Error(`No está definido el parseo para el estado ${estado}`);
    }
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
                MapearAlControl.RestrictoresDeEdicion(this.crudDeCreacion.PanelDeCrear, idsometedor, idUsuario, usuario);
            }
        }
        AplicarModoAccesoAlElemento(elemento) {
            super.AplicarModoAccesoAlElemento(elemento);
            let trabajo = elemento.Registro;
            let estado = trabajo.Estado.toLowerCase();
            let opcionesDeElemento = this.ZonaDeMenu.querySelectorAll(`input[${atOpcionDeMenu.clase}="${ClaseDeOpcioDeMenu.DeElemento}"]`);
            switch (estado) {
                case 'erroneo': {
                    ApiCrud.ActivarOpciones(opcionesDeElemento, ['errores', 'traza', 'editar', 'resometer'], this.InfoSelector.Cantidad);
                    ApiCrud.DesactivarOpciones(opcionesDeElemento, ['bloquear', 'desbloquear', 'ejecutar']);
                    break;
                }
                case 'pendiente': {
                    ApiCrud.ActivarOpciones(opcionesDeElemento, ['errores', 'traza', 'editar', 'bloquear', 'ejecutar'], this.InfoSelector.Cantidad);
                    ApiCrud.DesactivarOpciones(opcionesDeElemento, ['desbloquear', 'resometer']);
                    break;
                }
                case 'bloqueado': {
                    ApiCrud.ActivarOpciones(opcionesDeElemento, ['errores', 'traza', 'editar', 'desbloquear'], this.InfoSelector.Cantidad);
                    ApiCrud.DesactivarOpciones(opcionesDeElemento, ['bloquear', 'borrar', 'ejecutar', 'resometer']);
                    break;
                }
                case 'iniciado': {
                    ApiCrud.ActivarOpciones(opcionesDeElemento, ['errores', 'traza', 'editar', 'resometer'], this.InfoSelector.Cantidad);
                    ApiCrud.DesactivarOpciones(opcionesDeElemento, ['bloquear', 'desbloquear', 'ejecutar']);
                    break;
                }
                case 'terminado': {
                    ApiCrud.ActivarOpciones(opcionesDeElemento, ['errores', 'traza', 'editar', 'resometer'], this.InfoSelector.Cantidad);
                    ApiCrud.DesactivarOpciones(opcionesDeElemento, ['bloquear', 'desbloquear', 'ejecutar']);
                    break;
                }
                case 'con errores': {
                    ApiCrud.ActivarOpciones(opcionesDeElemento, ['errores', 'traza', 'editar', 'resometer'], this.InfoSelector.Cantidad);
                    ApiCrud.DesactivarOpciones(opcionesDeElemento, ['bloquear', 'desbloquear', 'ejecutar']);
                    break;
                }
                default: {
                    MensajesSe.Error('AjustarOpcionesDeMenuDelElemento', `No está definido que hacer con el estado ${estado} de un trabajo`);
                    this.DeshabilitarOpcionesDeMenuDeElemento();
                    break;
                }
            }
            ApiCrud.DesactivarConMultiSeleccion(opcionesDeElemento, this.InfoSelector.Cantidad);
        }
        IniciarTrabajo() {
            if (this.InfoSelector.Cantidad != 1) {
                MensajesSe.Apilar(MensajesSe.enumTipoMensaje.informativo, "Solo se puede iniciar un trabajo");
                return;
            }
            let idTrabajoDeUsuario = this.InfoSelector.LeerId(0);
            this.EjecutarTrabajoDeUsuario(idTrabajoDeUsuario)
                .then((peticion) => this.TrasEjecutarTrabajo(peticion))
                .catch((peticion) => this.SiHayErrorDeEjecucion(peticion));
        }
        BloquearTrabajo() {
            for (let i = 0; i < this.InfoSelector.Cantidad; i++)
                this.BloquearTrabajoDeUsuario(this.InfoSelector.LeerId(i));
        }
        DesbloquearTrabajo() {
            for (let i = 0; i < this.InfoSelector.Cantidad; i++)
                this.DesbloquearTrabajoDeUsuario(this.InfoSelector.LeerId(i));
        }
        ResometerTrabajo() {
            for (let i = 0; i < this.InfoSelector.Cantidad; i++)
                this.ResometerTrabajoDeUsuario(this.InfoSelector.LeerId(i));
        }
        EjecutarTrabajoDeUsuario(idTrabajoDeUsuario) {
            return new Promise((resolve, reject) => {
                let url = `/${Ajax.TrabajosSometidos.rutaTu}/${Ajax.TrabajosSometidos.accion.iniciar}?idTrabajoUsuario=${idTrabajoDeUsuario}`;
                let a = new ApiDeAjax.DescriptorAjax(this, Ajax.TrabajosSometidos.accion.iniciar, idTrabajoDeUsuario, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, (peticion) => {
                    resolve(peticion);
                }, (peticion) => {
                    reject(peticion);
                });
                a.Ejecutar();
            });
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
        ResometerTrabajoDeUsuario(idTrabajoDeUsuario) {
            let url = `/${Ajax.TrabajosSometidos.rutaTu}/${Ajax.TrabajosSometidos.accion.resometer}?idTrabajoUsuario=${idTrabajoDeUsuario}`;
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.TrabajosSometidos.accion.resometer, idTrabajoDeUsuario, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, this.TrasEjecutarTrabajo, this.SiHayErrorDeEjecucion);
            a.Ejecutar();
        }
        SiHayErrorDeEjecucion(peticion) {
            MensajesSe.Error(peticion.nombre, peticion.resultado.mensaje, peticion.resultado.consola);
            let crudTu = peticion.llamador;
            crudTu.RestaurarPagina();
        }
        TrasEjecutarTrabajo(peticion) {
            MensajesSe.Apilar(MensajesSe.enumTipoMensaje.informativo, peticion.resultado.mensaje);
            let crudTu = peticion.llamador;
            crudTu.RestaurarPagina();
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
        AntesDeMapearElementoDevuelto(peticion) {
            super.AntesDeMapearElementoDevuelto(peticion);
            let estado = ParsearEstado(peticion.resultado.datos['estado']);
            if (estado !== EstadoTrabajo.pendiente && estado !== EstadoTrabajo.bloqueado)
                peticion.resultado.modoDeAcceso = ModoAcceso.ModoDeAccesoDeDatos.Consultor;
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
                case Evento.TrabajoDeUsuario.resometer: {
                    TrabajosSometido.crudTu.ResometerTrabajo();
                    break;
                }
                default: {
                    MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, `la opción ${accion} no está definida`);
                    break;
                }
            }
        }
        catch (error) {
            MensajesSe.Apilar(MensajesSe.enumTipoMensaje.error, error.message);
        }
    }
    TrabajosSometido.Eventos = Eventos;
})(TrabajosSometido || (TrabajosSometido = {}));
//# sourceMappingURL=TrabajosDeUsuario.js.map