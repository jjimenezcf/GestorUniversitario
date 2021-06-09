namespace Callejero {

    export function CrearCrudDeMunicipios(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
        Crud.crudMnt = new Callejero.CrudDeMunicipios(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);

        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }

    export class CrudDeMunicipios extends Crud.CrudMnt {

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string, idModalBorrar: string) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionMunicipio(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionMunicipio(this, idPanelEdicion);
        }

        public DespuesDeAplicarUnRestrictor(restrictor: Tipos.DatosRestrictor) {
            super.DespuesDeAplicarUnRestrictor(restrictor);
            let idProvincia: number = restrictor.Valor;
            ApiDePeticiones.LeerElementoPorId(this, "Provincias", idProvincia, new Array<Parametro>())
                .then((peticion: ApiDeAjax.DescriptorAjax) => this.MapearPais(peticion))
                .catch((peticion: ApiDeAjax.DescriptorAjax) => MensajesSe.Error("DespuesDeAplicarUnRestrictor", peticion.resultado.mensaje, peticion.resultado.consola))
        }

        public MapearPais(peticion: ApiDeAjax.DescriptorAjax): void {
            let idPais: number = this.BuscarValorEnJson("idpais", peticion.resultado.datos) as number;
            let pais: string = this.BuscarValorEnJson("pais", peticion.resultado.datos) as string;
            let lista: HTMLInputElement = this.BuscarListaDinamica(this.ZonaDeFiltro, "idpais");
            MapearAlControl.FijarValorEnListaDinamica(lista, idPais, pais);
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