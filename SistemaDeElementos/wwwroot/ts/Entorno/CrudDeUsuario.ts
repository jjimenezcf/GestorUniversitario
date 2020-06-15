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
            let relacionarCon = partes[0].split('==')[1];
            let url = partes[1].split('==')[1];

            try {
                switch (relacionarCon) {
                    case Relaciones.puestos: {
                        var filtro = this.DefinirFiltroPorRestrictor("idusuario", 1);
                        url = url.replace("filtroJson", filtro);
                        break;
                    }
                }
            }
            catch (error) {
                Mensaje(TipoMensaje.Error, error);
            }
            super.IrARelacionar(url);
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