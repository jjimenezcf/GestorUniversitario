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

            function promesaNoResuelta(motivo: string): boolean {
                Mensaje(TipoMensaje.Error, motivo);
                return false;
            }

            function promesaResuelta(mensaje: string): boolean {
                Mensaje(TipoMensaje.Info, mensaje);
                return true;
            }

            if (this.InfoSelector.Cantidad != 1) {
                Mensaje(TipoMensaje.Info, "Solo se puede iniciar un trabajo");
                return;
            }

            let ejecutado: boolean;

            PonerCapa();
            this.EjecutarTrabajoDeUsuario()
                .then(resultado => ejecutado = promesaResuelta(resultado))
                .catch(error => ejecutado = promesaNoResuelta(error))
                .finally(() => this.TrasEjecutarElTrabajo(ejecutado));
        }

        public BloquearTrabajo(): void {
            Mensaje(TipoMensaje.Info, "bloquear Trabajo");
        }


        private TrasEjecutarElTrabajo(ejecutado: boolean) {
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

        public EjecutarTrabajoDeUsuario(): Promise<string> {
            let promesa: Promise<string> = new Promise((resolve, reject) => {
                let idTrabajoUsuario: number = this.InfoSelector.LeerId(0);

                let url: string = `/${Ajax.TrabajosSometidos.TrabajosDeUsuario}/${Ajax.TrabajosSometidos.accion.iniciar}?idTrabajoUsuario=${idTrabajoUsuario}`;

                let a = new ApiDeAjax.DescriptorAjax(this
                    , 'epIniciarTrabajoDeUsuario'
                    , idTrabajoUsuario
                    , url
                    , ApiDeAjax.TipoPeticion.Asincrona
                    , ApiDeAjax.ModoPeticion.Get
                    , (peticion) => {
                        resolve(this.TrasEjecutarTrabajo(peticion));
                    }
                    , (peticion) => {
                        reject(this.SiHayErrorDeEjecucion(peticion));
                    }
                );
                a.Ejecutar();
            });

            return promesa;
        }

        public SiHayErrorDeEjecucion(peticion: ApiDeAjax.DescriptorAjax) {
            return peticion.resultado.mensaje;
        }

        public TrasEjecutarTrabajo(peticion: ApiDeAjax.DescriptorAjax) {
            return peticion.resultado.mensaje;
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