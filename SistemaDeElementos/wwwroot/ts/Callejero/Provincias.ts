namespace Callejero {

    export function CrearCrudDeProvincias(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
        Crud.crudMnt = new Callejero.CrudDeProvincias(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);

        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }

    export class CrudDeProvincias extends Crud.CrudMnt {


        protected get EditorDePais(): HTMLInputElement {
            let editor: HTMLInputElement = ApiControl.BuscarListaDinamicaPorPropiedad(this.ZonaDeFiltro, Callejero.atributo.propiedad.idpais) as HTMLInputElement;
            if (NoDefinida(editor))
                MensajesSe.EmitirExcepcion("Propiedad EditorDePais", "No se lo caliza el editor de Pais en el filtro de provincia");
            return editor;
        };

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionProvincia(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionProvincia(this, idPanelEdicion);
        }

        public DespuesDeAplicarUnRestrictor(restrictor: Tipos.DatosRestrictor) {
            super.DespuesDeAplicarUnRestrictor(restrictor);

            if (restrictor.Propiedad === Callejero.restrictor.codigoPostal) {
                ApiControl.BloquearEditor(this.EditorDePais);
            }

        }

    }

    export class CrudCreacionProvincia extends Crud.CrudCreacion {

        constructor(crud: Crud.CrudMnt, idPanelCreacion: string) {
            super(crud, idPanelCreacion);
        }

    }

    export class CrudEdicionProvincia extends Crud.CrudEdicion {

        constructor(crud: Crud.CrudMnt, idPanelEdicion: string) {
            super(crud, idPanelEdicion);
        }
    }
}