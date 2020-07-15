namespace Seguridad {

    export class CrudMntPuestoDeTrabajo extends Crud.CrudMnt {

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt);
            this.crudDeCreacion = new CrudCreacionPuestoDeTrabajo(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionPuestoDeTrabajo(this, idPanelEdicion);
            this.idModalBorrar = idModalBorrar;
        }

        public RelacionarCon(parametrosDeEntrada: string): Crud.DatosParaRelacionar {
            let datos = super.RelacionarCon(parametrosDeEntrada)
            try {
                if (this.InfoSelector.Cantidad != 1) {
                    throw new Error("Debe seleccionar solo un usuario");
                }

                switch (datos.RelacionarCon) {
                    case Relaciones.roles: {
                        let id: number = this.InfoSelector.LeerElemento(0).Id;
                        datos.FiltroRestrictor = new Crud.DatosRestrictor(Restrictor.idPuesto, id, this.InfoSelector.LeerElemento(0).Texto)
                        break;
                    }
                }

                this.NavegarARelacionar(datos.IdFormHtml, datos.FiltroRestrictor);
            }
            catch (error) {
                Mensaje(TipoMensaje.Error, error);
                return;
            }
        }
    }

    export class CrudCreacionPuestoDeTrabajo extends Crud.CrudCreacion {

        constructor(crud: Crud.CrudMnt, idPanelCreacion: string) {
            super(crud, idPanelCreacion);
        }

    }

    export class CrudEdicionPuestoDeTrabajo extends Crud.CrudEdicion {

        constructor(crud: Crud.CrudMnt, idPanelEdicion: string) {
            super(crud, idPanelEdicion);
        }
    }
}