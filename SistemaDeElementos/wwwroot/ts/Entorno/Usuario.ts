﻿namespace Entorno {

    export function CrearCrudDeUsuarios(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
        Crud.crudMnt = new Entorno.CrudDeUsuarios(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);

        window.addEventListener("load", function () {
            Crud.crudMnt.Inicializar(idPanelMnt);
        }, false);

        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    export class CrudDeUsuarios extends Crud.CrudMnt {

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionUsuario(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionUsuario(this, idPanelEdicion);
        }
    }

    export class CrudCreacionUsuario extends Crud.CrudCreacion {

        constructor(crud: Crud.CrudMnt, idPanelCreacion: string) {
            super(crud, idPanelCreacion);
        }

        public DespuesDeMapearDatosDeIU(crud: Crud.CrudBase, panel: HTMLDivElement, elementoJson: JSON, modoDeTrabajo: string): JSON {
            super.DespuesDeMapearDatosDeIU(crud, panel, elementoJson, modoDeTrabajo);
            return elementoJson;
        }

    }

    export class CrudEdicionUsuario extends Crud.CrudEdicion {

        constructor(crud: Crud.CrudMnt, idPanelEdicion: string) {
            super(crud, idPanelEdicion);
        }

        public AntesDeMapearDatosDeIU(crud: Crud.CrudBase, panel: HTMLDivElement, modoDeTrabajo: string): JSON {
            let json: JSON = super.AntesDeMapearDatosDeIU(crud, panel, modoDeTrabajo);
            console.log('funciona');
            return json;
        }

    }


}