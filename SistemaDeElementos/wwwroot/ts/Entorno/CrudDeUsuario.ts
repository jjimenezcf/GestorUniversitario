namespace Entorno {

    const Relaciones = {
        puestos: 'PuestoDeUnUsuario'
    };


    export class CrudMntUsuario extends Crud.CrudMnt {

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt);
            this.crudDeCreacion = new CrudCreacionUsuario(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionUsuario(this, idPanelEdicion);
            this.idModalBorrar = idModalBorrar;
        }

        public IrARelacionar(crudDeRelacion: string) {
            super.IrARelacionar(crudDeRelacion);
            switch (crudDeRelacion) {
                case Relaciones.puestos: {
                    var filtro = this.DefinirFiltroPorRestrictor("idusuario", 1);
                    document.location.href = `/PuestoDeUnUsuario/CrudPuestoDeUnUsuario?filtroUsuario=${filtro}&orden=permiso`;
                    break;
                }

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