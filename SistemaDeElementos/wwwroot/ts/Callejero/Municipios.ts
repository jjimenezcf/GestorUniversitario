namespace Callejero {

    export function CrearCrudDeMunicipios(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
        Crud.crudMnt = new Callejero.CrudDeMunicipios(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);

        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }

    export class CrudDeMunicipios extends Crud.CrudMnt {


        protected get EditorDePais(): HTMLInputElement {
            let editor: HTMLInputElement = ApiControl.BuscarListaDinamicaPorPropiedad(this.ZonaDeFiltro,"idpais") as HTMLInputElement;
            if (NoDefinida(editor))
                MensajesSe.EmitirExcepcion("Propiedad EditorDePais", "No se lo caliza el editor de Pais en el filtro de Municipio");
            return editor;
        };


        protected get EditorDeProvincia(): HTMLInputElement {
            let editor: HTMLInputElement = ApiControl.BuscarListaDinamicaPorPropiedad(this.ZonaDeFiltro, "idprovincia") as HTMLInputElement;
            if (NoDefinida(editor))
                MensajesSe.EmitirExcepcion("Propiedad EditorDeProvincia", "No se lo caliza el editor de Provincia en el filtro de Municipio");
            return editor;
        };

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionMunicipio(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionMunicipio(this, idPanelEdicion);
        }

        public DespuesDeAplicarUnRestrictor(restrictor: Tipos.DatosRestrictor) {
            super.DespuesDeAplicarUnRestrictor(restrictor);


            if (restrictor.Propiedad === "idcp") {
                ApiControl.BloquearEditor(this.EditorDePais);
                ApiControl.BloquearEditor(this.EditorDeProvincia);
            }

            if (restrictor.Propiedad === "idprovincia") {
                let idProvincia: number = restrictor.Valor;
                ApiDePeticiones.LeerElementoPorId(this, "Provincias", idProvincia, new Array<Parametro>())
                    .then((peticion: ApiDeAjax.DescriptorAjax) => this.MapearPais(peticion))
                    .catch((peticion: ApiDeAjax.DescriptorAjax) => MensajesSe.Error("DespuesDeAplicarUnRestrictor", peticion.resultado.mensaje, peticion.resultado.consola))

            }
        }

        public MapearPais(peticion: ApiDeAjax.DescriptorAjax): void {
            let idPais: number = this.BuscarValorEnJson("idpais", peticion.resultado.datos) as number;
            let pais: string = this.BuscarValorEnJson("pais", peticion.resultado.datos) as string;
            let listaDeFiltro: HTMLInputElement = ApiControl.BuscarListaDinamicaPorPropiedad(this.ZonaDeFiltro, "idpais");
            MapearAlControl.FijarValorEnListaDinamica(listaDeFiltro, idPais, pais);

            let listaDeCreacion: HTMLInputElement = ApiControl.BuscarListaDinamicaPorGuardarEn(this.crudDeCreacion.PanelDeCrear, "idpais");
            MapearAlControl.FijarValorEnListaDinamica(listaDeCreacion, idPais, pais);

            let listaDeEdicion: HTMLInputElement = ApiControl.BuscarListaDinamicaPorGuardarEn(this.crudDeEdicion.PanelDeEditar, "idpais");
            MapearAlControl.FijarValorEnListaDinamica(listaDeEdicion, idPais, pais);
        }
    }

    export class CrudCreacionMunicipio extends Crud.CrudCreacion {

        constructor(crud: Crud.CrudMnt, idPanelCreacion: string) {
            super(crud, idPanelCreacion);
        }

    }

    export class CrudEdicionMunicipio extends Crud.CrudEdicion {

        constructor(crud: Crud.CrudMnt, idPanelEdicion: string) {
            super(crud, idPanelEdicion);
        }
    }
}