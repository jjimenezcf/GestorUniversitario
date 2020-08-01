namespace Seguridad {

    export function CrearCrudDePuestosDeUnUsuario(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
        Crud.crudMnt = new Seguridad.CrudDePuestosDeUnUsuario(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        Crud.crudMnt.RenderGrid = false;
        Crud.crudMnt.LeerDatosParaElGrid(atGrid.accion.buscar, 0);
    }

    export class CrudDePuestosDeUnUsuario extends Crud.CrudMnt {

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt);
            this.crudDeCreacion = new CrudCreacionPuestoDeUnUsuario(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionPuestoDeUnUsuario(this, idPanelEdicion);
            this.idModalBorrar = idModalBorrar;

            if (this.HayHistorial)
                this.RecuperarFiltros();
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
                        datos.FiltroRestrictor = new Crud.DatosRestrictor(Variables.Puesto.restrictor, id, this.InfoSelector.LeerElemento(0).Texto)
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

    export class CrudCreacionPuestoDeUnUsuario extends Crud.CrudCreacion {

        constructor(crud: Crud.CrudMnt, idPanelCreacion: string) {
            super(crud, idPanelCreacion);
        }

    }

    export class CrudEdicionPuestoDeUnUsuario extends Crud.CrudEdicion {

        constructor(crud: Crud.CrudMnt, idPanelEdicion: string) {
            super(crud, idPanelEdicion);
        }
    }
}