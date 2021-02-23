namespace TrabajosSometido {

    export let crudTu: CrudDeTrabajosDeUsuario = null;

    export function CrearCrudDeTrabajosDeUsuario(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
        Crud.crudMnt = new TrabajosSometido.CrudDeTrabajosDeUsuario(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        crudTu = Crud.crudMnt as TrabajosSometido.CrudDeTrabajosDeUsuario;
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
    }

    export class CrudDeTrabajosDeUsuario extends Crud.CrudMnt {

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionTrabajoDeUsuario(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionTrabajoDeUsuario(this, idPanelEdicion);
        }

        public IniciarTrabajo(): boolean {

            if (this.InfoSelector.Cantidad != 1) {
                Mensaje(TipoMensaje.Info, "Solo se puede iniciar un trabajo");
                return;
            }

            this.EjecutarTrabajoDeUsuario();
        }

        public BloquearTrabajo(): void {
            for (let i: number = 0; i < this.InfoSelector.Cantidad; i++) 
                this.BloquearTrabajoDeUsuario(this.InfoSelector.LeerId(i));
        }

        private EjecutarTrabajoDeUsuario(): void {
            let idTrabajoDeUsuario: number = this.InfoSelector.LeerId(0);

            let url: string = `/${Ajax.TrabajosSometidos.TrabajosDeUsuario}/${Ajax.TrabajosSometidos.accion.iniciar}?idTrabajoUsuario=${idTrabajoDeUsuario}`;

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
            let url: string = `/${Ajax.TrabajosSometidos.TrabajosDeUsuario}/${Ajax.TrabajosSometidos.accion.bloquear}?idTrabajoUsuario=${idTrabajoDeUsuario}`;

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

        public SiHayErrorDeEjecucion(peticion: ApiDeAjax.DescriptorAjax): void {
            Mensaje(TipoMensaje.Error, peticion.resultado.mensaje);
            let crudTu: CrudDeTrabajosDeUsuario = peticion.llamador as CrudDeTrabajosDeUsuario;
            crudTu.CargarGrid(atGrid.accion.buscar, 0);
        }

        public TrasEjecutarTrabajo(peticion: ApiDeAjax.DescriptorAjax) {
            Mensaje(TipoMensaje.Info, peticion.resultado.mensaje);
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

}