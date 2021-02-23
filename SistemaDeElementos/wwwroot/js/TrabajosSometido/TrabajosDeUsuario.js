var TrabajosSometido;
(function (TrabajosSometido) {
    TrabajosSometido.crudTu = null;
    function CrearCrudDeTrabajosDeUsuario(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new TrabajosSometido.CrudDeTrabajosDeUsuario(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        TrabajosSometido.crudTu = Crud.crudMnt;
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
    }
    TrabajosSometido.CrearCrudDeTrabajosDeUsuario = CrearCrudDeTrabajosDeUsuario;
    class CrudDeTrabajosDeUsuario extends Crud.CrudMnt {
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionTrabajoDeUsuario(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionTrabajoDeUsuario(this, idPanelEdicion);
        }
        IniciarTrabajo() {
            function promesaNoResuelta(motivo) {
                Mensaje(TipoMensaje.Error, motivo);
                return false;
            }
            function promesaResuelta(mensaje) {
                Mensaje(TipoMensaje.Info, mensaje);
                return true;
            }
            if (this.InfoSelector.Cantidad != 1) {
                Mensaje(TipoMensaje.Info, "Solo se puede iniciar un trabajo");
                return;
            }
            let ejecutado;
            PonerCapa();
            this.EjecutarTrabajoDeUsuario()
                .then(resultado => ejecutado = promesaResuelta(resultado))
                .catch(error => ejecutado = promesaNoResuelta(error))
                .finally(() => this.TrasEjecutarElTrabajo(ejecutado));
        }
        BloquearTrabajo() {
            Mensaje(TipoMensaje.Info, "bloquear Trabajo");
        }
        TrasEjecutarElTrabajo(ejecutado) {
            var promesa = this.PromesaDeCargarGrid(atGrid.accion.buscar, 0);
            if (promesa != null)
                promesa.then(() => {
                    if (ejecutado)
                        Mensaje(TipoMensaje.Info, 'trabajo finalizado');
                    else
                        Mensaje(TipoMensaje.Error, 'trabajo con errores, consulte el log de ejecución');
                }).catch(() => {
                    Mensaje(TipoMensaje.Error, 'Error al recargar el grid, consulte la consola');
                }).finally(() => {
                    QuitarCapa();
                });
        }
        EjecutarTrabajoDeUsuario() {
            let promesa = new Promise((resolve, reject) => {
                let idTrabajoUsuario = this.InfoSelector.LeerId(0);
                let url = `/${Ajax.TrabajosSometidos.TrabajosDeUsuario}/${Ajax.TrabajosSometidos.accion.iniciar}?idTrabajoUsuario=${idTrabajoUsuario}`;
                let a = new ApiDeAjax.DescriptorAjax(this, 'epIniciarTrabajoDeUsuario', idTrabajoUsuario, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, (peticion) => {
                    resolve(this.TrasEjecutarTrabajo(peticion));
                }, (peticion) => {
                    reject(this.SiHayErrorDeEjecucion(peticion));
                });
                a.Ejecutar();
            });
            return promesa;
        }
        SiHayErrorDeEjecucion(peticion) {
            return peticion.resultado.mensaje;
        }
        TrasEjecutarTrabajo(peticion) {
            return peticion.resultado.mensaje;
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
                default: {
                    Mensaje(TipoMensaje.Error, `la opción ${accion} no está definida`);
                    break;
                }
            }
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error);
        }
    }
    TrabajosSometido.Eventos = Eventos;
})(TrabajosSometido || (TrabajosSometido = {}));
//# sourceMappingURL=TrabajosDeUsuario.js.map