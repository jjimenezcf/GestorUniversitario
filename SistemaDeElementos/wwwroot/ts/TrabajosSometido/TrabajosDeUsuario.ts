namespace TrabajosSometido {

    export let crudTu: CrudDeTrabajosDeUsuario = null;

    export function CrearCrudDeTrabajosDeUsuario(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
        Crud.crudMnt = new TrabajosSometido.CrudDeTrabajosDeUsuario(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        crudTu = Crud.crudMnt as TrabajosSometido.CrudDeTrabajosDeUsuario;
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
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

        public AjustarOpcionesDeMenu(elemento: TrabajoDeUsuario, modoAcceso: string): void {
            super.AjustarOpcionesDeMenu(elemento, modoAcceso);
            let estado: string = elemento.Estado.toLowerCase();
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


        public ActivarOpciones(opcionesDeMenu: string[]) {
        }

        public DesactivarOpciones(opcionesDeMenu: string[]) {
        }

        public IniciarTrabajo(): boolean {

            if (this.InfoSelector.Cantidad != 1) {
                Notificar(TipoMensaje.Info, "Solo se puede iniciar un trabajo");
                return;
            }

            this.EjecutarTrabajoDeUsuario();
        }

        public BloquearTrabajo(): void {
            for (let i: number = 0; i < this.InfoSelector.Cantidad; i++)
                this.BloquearTrabajoDeUsuario(this.InfoSelector.LeerId(i));
        }

        public DesbloquearTrabajo(): void {
            for (let i: number = 0; i < this.InfoSelector.Cantidad; i++)
                this.DesbloquearTrabajoDeUsuario(this.InfoSelector.LeerId(i));
        }

        private EjecutarTrabajoDeUsuario(): void {
            let idTrabajoDeUsuario: number = this.InfoSelector.LeerId(0);

            let url: string = `/${Ajax.TrabajosSometidos.rutaTu}/${Ajax.TrabajosSometidos.accion.iniciar}?idTrabajoUsuario=${idTrabajoDeUsuario}`;

            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.TrabajosSometidos.accion.iniciar
                , idTrabajoDeUsuario
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.TrasEjecutarTrabajo
                , this.SiHayErrorDeEjecucion
            );
            a.Ejecutar();
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

        public SiHayErrorDeEjecucion(peticion: ApiDeAjax.DescriptorAjax): void {
            MensajesSe.Error(peticion.nombre, peticion.resultado.mensaje, peticion.resultado.consola);
            let crudTu: CrudDeTrabajosDeUsuario = peticion.llamador as CrudDeTrabajosDeUsuario;
            crudTu.CargarGrid(atGrid.accion.buscar, 0);
        }

        public TrasEjecutarTrabajo(peticion: ApiDeAjax.DescriptorAjax) {
            Notificar(TipoMensaje.Info, peticion.resultado.mensaje);
            let crudTu: CrudDeTrabajosDeUsuario = peticion.llamador as CrudDeTrabajosDeUsuario;
            crudTu.CargarGrid(atGrid.accion.buscar, 0);
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

}