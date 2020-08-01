namespace Entorno {

    export function CrearCrudDeUsuarios(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
        Crud.crudMnt = new Entorno.CrudDeUsuarios(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        Crud.crudMnt.RenderGrid = false;
        Crud.crudMnt.LeerDatosParaElGrid(atGrid.accion.buscar, 0);
    }

    export class CrudDeUsuarios extends Crud.CrudMnt {

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt);
            this.crudDeCreacion = new CrudCreacionUsuario(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionUsuario(this, idPanelEdicion);
            this.idModalBorrar = idModalBorrar;
        }

        //permite relacionar un usuario con diferentes entidades
        // parametros de entrada:
        // idForm --> control Html que almacena los parámetros y la acción para someter
        // relacionarCon --> entidad con la que se relaciona
        public RelacionarCon(parametrosDeEntrada: string): Crud.DatosParaRelacionar {
            let datos = super.RelacionarCon(parametrosDeEntrada)
            try {
                if (this.InfoSelector.Cantidad != 1) {
                    throw new Error("Debe seleccionar solo un usuario");
                }

                switch (datos.RelacionarCon) {
                    case Relaciones.puestos: {
                        let idUsuario: number = this.InfoSelector.LeerElemento(0).Id;
                        datos.FiltroRestrictor = new Crud.DatosRestrictor(Variables.Usuario.restrictor, idUsuario, this.InfoSelector.LeerElemento(0).Texto)
                        break;
                    }
                }

                super.NavegarARelacionar(datos.IdFormHtml, datos.FiltroRestrictor);
            }
            catch (error) {
                Mensaje(TipoMensaje.Error, error);
                return;
            }
        }
    }

    export class CrudCreacionUsuario extends Crud.CrudCreacion {

        constructor(crud: Crud.CrudMnt, idPanelCreacion: string) {
            super(crud, idPanelCreacion);
        }

    }

    export class CrudEdicionUsuario extends Crud.CrudEdicion {

        constructor(crud: Crud.CrudMnt, idPanelEdicion: string) {
            super(crud, idPanelEdicion);
        }
    }


}