namespace Seguridad {

    export class CrudMntPuestoDeTrabajo extends Crud.CrudMnt {

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt);
            this.crudDeCreacion = new CrudCreacionPuestoDeTrabajo(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionPuestoDeTrabajo(this, idPanelEdicion);
            this.idModalBorrar = idModalBorrar;
        }

        public IrARelacionar(parametrosDeEntrada: string) {
            let partes = parametrosDeEntrada.split('#');
            let idForm = partes[0].split('==')[1];
            let relacionarCon = partes[1].split('==')[1];

            try {
                if (this.InfoSelector.Cantidad != 1) {
                    throw new Error("Debe seleccionar solo un usuario");
                }

                switch (relacionarCon) {
                    case Relaciones.roles: {
                        let id: number = this.InfoSelector.LeerElemento(0).Id;
                        let filtro: string = this.DefinirFiltroPorRestrictor(Restrictor.idPuesto, id);
                        let orden: string = "rol";

                        sessionStorage[Restrictor.idPuesto] = id;
                        sessionStorage[Parametros.Puesto] = this.InfoSelector.LeerElemento(0).Texto;

                        parametrosDeEntrada = `${idForm}#${filtro}#${orden}`;

                        break;
                    }
                }

                super.IrARelacionar(parametrosDeEntrada);
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