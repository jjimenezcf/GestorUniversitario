namespace Seguridad {

    export class CrudMntPuestoDeUnUsuario extends Crud.CrudMnt {
        private _IdUsuario: string
        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string, idUsuario: string) {
            super(idPanelMnt);
            this.crudDeCreacion = new CrudCreacionPuestoDeUnUsuario(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionPuestoDeUnUsuario(this, idPanelEdicion);
            this.idModalBorrar = idModalBorrar;
            this._IdUsuario = idUsuario;

            this._IdUsuario = sessionStorage["idusuario"];

            if (this._IdUsuario === undefined || this._IdUsuario.Numero() == 0) {
                document.location.href = document.referrer;
            }
            else {

                let restrictores: NodeListOf<HTMLInputElement> = this.PanelDeMnt.querySelectorAll(`input[${Atributo.tipo}="${TipoControl.restrictor}"]`) as NodeListOf<HTMLInputElement>;
                for (let i = 0; i < restrictores.length; i++) {
                    if (restrictores[i].getAttribute(Atributo.propiedad) === "idusuario")
                        restrictores[i].setAttribute("value", sessionStorage["nombreUsuario"]);
                }

                sessionStorage.removeItem("idusuario");
                sessionStorage.removeItem("nombreUsuario");
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