namespace Entorno {

    const Relaciones = {
        puestos: 'PuestoDto'
    };


    export class CrudMntUsuario extends Crud.CrudMnt {

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt);
            this.crudDeCreacion = new CrudCreacionUsuario(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionUsuario(this, idPanelEdicion);
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
                    case Relaciones.puestos: {
                        let id: number = this.InfoSelector.LeerElemento(0).Id;     
                        let filtro: string = this.DefinirFiltroPorRestrictor("idusuario", id);
                        let orden: string = "puesto";

                        sessionStorage["idusuario"] = id
                        sessionStorage["nombreUsuario"] = this.InfoSelector.LeerElemento(0).Texto;

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