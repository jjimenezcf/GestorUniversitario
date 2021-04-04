namespace TrabajosSometido {

    export let crudTu: CrudDeTrabajosDeUsuario = null;

    export function CrearCrudDeTrabajosDeUsuario(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
        Crud.crudMnt = new TrabajosSometido.CrudDeTrabajosDeUsuario(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        crudTu = Crud.crudMnt as TrabajosSometido.CrudDeTrabajosDeUsuario;
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);

        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }

    const idsometedor: string = 'idsometedor';

    class TrabajoDeUsuario {
        Ejecutor: string;
        Encolado: Date;
        Estado: string;
        Id: number;
        IdEjecutor: number;
        IdSometedor: number;
        IdTrabajo: number;
        Iniciado: Date;
        ModoDeAcceso: string;
        Parametros: string;
        Periodicidad: number;
        Planificado: Date;
        Sometedor: string;
        Terminado: Date;
        Trabajo: string;
    }


    enum EstadoTrabajo { erroneo, pendiente, bloqueado, iniciado, terminado, conerrores }

    function ParsearEstado(estado: string): EstadoTrabajo {
        if (estado.toLowerCase() === 'erroneo') return EstadoTrabajo.erroneo;
        if (estado.toLowerCase() === 'pendiente') return EstadoTrabajo.pendiente;
        if (estado.toLowerCase() === 'bloqueado') return EstadoTrabajo.bloqueado;
        if (estado.toLowerCase() === 'iniciado') return EstadoTrabajo.iniciado;
        if (estado.toLowerCase() === 'terminado') return EstadoTrabajo.terminado;
        if (estado.toLowerCase() === 'con errores') return EstadoTrabajo.conerrores;

        throw Error(`No está definido el parseo para el estado ${estado}`);
    }

    export class CrudDeTrabajosDeUsuario extends Crud.CrudMnt {

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionTrabajoDeUsuario(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionTrabajoDeUsuario(this, idPanelEdicion);
        }

        public Inicializar(idPanelMnt: string) {
            super.Inicializar(idPanelMnt);
            this.MapearUsuarioConectado();
        }

        private MapearUsuarioConectado(): void {

            function usuarioNoLeido(llamador: CrudCreacionTrabajoDeUsuario): void {
                let zonaDeMenu: HTMLDivElement = llamador.CrudDeMnt.ZonaDeMenu;
                ApiControl.BloquearMenu(zonaDeMenu);
                console.error("no se ha podido leer");
            }


            let usuarioConectado = Registro.UsuarioConectado();
            if (usuarioConectado == null) {
                usuarioNoLeido(this.crudDeCreacion);
            }
            else {
                let idUsuario: number = usuarioConectado['id'] as number;
                let usuario: string = usuarioConectado['login'] as string;
                ApiControl.MapearPropiedadRestrictoraAlControl(this.crudDeCreacion.PanelDeCrear, idsometedor, idUsuario, usuario);
            }
        }

        public AplicarModoAccesoAlElemento(elemento: Elemento): void {
            super.AplicarModoAccesoAlElemento(elemento);
            let trabajo: TrabajoDeUsuario = elemento.Registro;
            let estado: string = trabajo.Estado.toLowerCase();
            let opcionesDeElemento: NodeListOf<HTMLButtonElement> = this.ZonaDeMenu.querySelectorAll(`input[${atOpcionDeMenu.clase}="${ClaseDeOpcioDeMenu.DeElemento}"]`) as NodeListOf<HTMLButtonElement>;
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

        public IniciarTrabajo(): boolean {

            if (this.InfoSelector.Cantidad != 1) {
                MensajesSe.Apilar(MensajesSe.enumTipoMensaje.informativo, "Solo se puede iniciar un trabajo");
                return;
            }

            let idTrabajoDeUsuario: number = this.InfoSelector.LeerId(0);

            this.EjecutarTrabajoDeUsuario(idTrabajoDeUsuario)
                .then((peticion) => this.TrasEjecutarTrabajo(peticion))
                .catch((peticion) => this.SiHayErrorDeEjecucion(peticion));
        }

        public BloquearTrabajo(): void {
            for (let i: number = 0; i < this.InfoSelector.Cantidad; i++)
                this.BloquearTrabajoDeUsuario(this.InfoSelector.LeerId(i));
        }

        public DesbloquearTrabajo(): void {
            for (let i: number = 0; i < this.InfoSelector.Cantidad; i++)
                this.DesbloquearTrabajoDeUsuario(this.InfoSelector.LeerId(i));
        }

        public ResometerTrabajo(): void {
            for (let i: number = 0; i < this.InfoSelector.Cantidad; i++)
                this.ResometerTrabajoDeUsuario(this.InfoSelector.LeerId(i));
        }

        private EjecutarTrabajoDeUsuario(idTrabajoDeUsuario: number): Promise<ApiDeAjax.DescriptorAjax> {

            return new Promise((resolve, reject) => {

                let url: string = `/${Ajax.TrabajosSometidos.rutaTu}/${Ajax.TrabajosSometidos.accion.iniciar}?idTrabajoUsuario=${idTrabajoDeUsuario}`;

                let a = new ApiDeAjax.DescriptorAjax(this
                    , Ajax.TrabajosSometidos.accion.iniciar
                    , idTrabajoDeUsuario
                    , url
                    , ApiDeAjax.TipoPeticion.Asincrona
                    , ApiDeAjax.ModoPeticion.Get
                    , (peticion) => {
                        resolve(peticion);
                    }
                    , (peticion) => {
                        reject(peticion);
                    }
                );

                a.Ejecutar();
            });
        }

        private BloquearTrabajoDeUsuario(idTrabajoDeUsuario: number): void {
            let url: string = `/${Ajax.TrabajosSometidos.rutaTu}/${Ajax.TrabajosSometidos.accion.bloquear}?idTrabajoUsuario=${idTrabajoDeUsuario}`;

            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.TrabajosSometidos.accion.bloquear
                , idTrabajoDeUsuario
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.TrasEjecutarTrabajo
                , this.SiHayErrorDeEjecucion
            );
            a.Ejecutar();
        }

        private DesbloquearTrabajoDeUsuario(idTrabajoDeUsuario: number): void {
            let url: string = `/${Ajax.TrabajosSometidos.rutaTu}/${Ajax.TrabajosSometidos.accion.desbloquear}?idTrabajoUsuario=${idTrabajoDeUsuario}`;

            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.TrabajosSometidos.accion.desbloquear
                , idTrabajoDeUsuario
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.TrasEjecutarTrabajo
                , this.SiHayErrorDeEjecucion
            );
            a.Ejecutar();
        }

        private ResometerTrabajoDeUsuario(idTrabajoDeUsuario: number): void {
            let url: string = `/${Ajax.TrabajosSometidos.rutaTu}/${Ajax.TrabajosSometidos.accion.resometer}?idTrabajoUsuario=${idTrabajoDeUsuario}`;

            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.TrabajosSometidos.accion.resometer
                , idTrabajoDeUsuario
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.TrasEjecutarTrabajo
                , this.SiHayErrorDeEjecucion
            );
            a.Ejecutar();
        }

        public SiHayErrorDeEjecucion(peticion: ApiDeAjax.DescriptorAjax): void {
            MensajesSe.Error(peticion.nombre, peticion.resultado.mensaje, peticion.resultado.consola);
            let crudTu: CrudDeTrabajosDeUsuario = peticion.llamador as CrudDeTrabajosDeUsuario;
            crudTu.RestaurarPagina();
        }

        public TrasEjecutarTrabajo(peticion: ApiDeAjax.DescriptorAjax) {
            MensajesSe.Apilar(MensajesSe.enumTipoMensaje.informativo, peticion.resultado.mensaje);
            let crudTu: CrudDeTrabajosDeUsuario = peticion.llamador as CrudDeTrabajosDeUsuario;
            crudTu.RestaurarPagina();
        }
    }

    export class CrudCreacionTrabajoDeUsuario extends Crud.CrudCreacion {

        constructor(crud: Crud.CrudMnt, idPanelCreacion: string) {
            super(crud, idPanelCreacion);
        }

    }

    export class CrudEdicionTrabajoDeUsuario extends Crud.CrudEdicion {

        constructor(crud: Crud.CrudMnt, idPanelEdicion: string) {
            super(crud, idPanelEdicion);
        }

        protected AntesDeMapearElementoDevuelto(peticion: ApiDeAjax.DescriptorAjax): void {
            super.AntesDeMapearElementoDevuelto(peticion);
            let estado: EstadoTrabajo = ParsearEstado(peticion.resultado.datos['estado']);
            if (estado !== EstadoTrabajo.pendiente && estado !== EstadoTrabajo.bloqueado)
                peticion.resultado.modoDeAcceso = ModoAcceso.ModoDeAccesoDeDatos.Consultor;
        }
    }


    export function Eventos(accion: string) {

        try {
            switch (accion) {
                case Evento.TrabajoDeUsuario.iniciar: {
                    crudTu.IniciarTrabajo();
                    break;
                }
                case Evento.TrabajoDeUsuario.bloquear: {
                    crudTu.BloquearTrabajo();
                    break;
                }
                case Evento.TrabajoDeUsuario.desbloquear: {
                    crudTu.DesbloquearTrabajo();
                    break;
                }
                case Evento.TrabajoDeUsuario.resometer: {
                    crudTu.ResometerTrabajo();
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

}