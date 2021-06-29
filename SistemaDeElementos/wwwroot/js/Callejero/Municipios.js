var Callejero;
(function (Callejero) {
    function CrearCrudDeMunicipios(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
        Crud.crudMnt = new Callejero.CrudDeMunicipios(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar);
        window.addEventListener("load", function () { Crud.crudMnt.Inicializar(idPanelMnt); }, false);
        window.onbeforeunload = function () {
            Crud.crudMnt.NavegarDesdeElBrowser();
        };
    }
    Callejero.CrearCrudDeMunicipios = CrearCrudDeMunicipios;
    class CrudDeMunicipios extends Crud.CrudMnt {
        get EditorDePais() {
            let editor = ApiControl.BuscarListaDinamicaPorPropiedad(this.ZonaDeFiltro, Callejero.atributo.propiedad.idpais);
            if (NoDefinida(editor))
                MensajesSe.EmitirExcepcion("Propiedad EditorDePais", "No se lo caliza el editor de Pais en el filtro de Municipio");
            return editor;
        }
        ;
        get EditorDeProvincia() {
            let editor = ApiControl.BuscarListaDinamicaPorPropiedad(this.ZonaDeFiltro, Callejero.atributo.propiedad.idprovincia);
            if (NoDefinida(editor))
                MensajesSe.EmitirExcepcion("Propiedad EditorDeProvincia", "No se lo caliza el editor de Provincia en el filtro de Municipio");
            return editor;
        }
        ;
        constructor(idPanelMnt, idPanelCreacion, idPanelEdicion, idModalBorrar) {
            super(idPanelMnt, idModalBorrar);
            this.crudDeCreacion = new CrudCreacionMunicipio(this, idPanelCreacion);
            this.crudDeEdicion = new CrudEdicionMunicipio(this, idPanelEdicion);
        }
        DespuesDeAplicarUnRestrictor(restrictor) {
            super.DespuesDeAplicarUnRestrictor(restrictor);
            if (restrictor.Propiedad === Callejero.restrictor.codigoPostal) {
                ApiControl.BloquearEditor(this.EditorDePais);
                ApiControl.BloquearEditor(this.EditorDeProvincia);
            }
            if (restrictor.Propiedad === Callejero.restrictor.provincia) {
                let idProvincia = restrictor.Valor;
                ApiDePeticiones.LeerElementoPorId(this, Callejero.controlador.provincia, idProvincia, new Array())
                    .then((peticion) => this.MapearPais(peticion))
                    .catch((peticion) => MensajesSe.Error("DespuesDeAplicarUnRestrictor", peticion.resultado.mensaje, peticion.resultado.consola));
            }
        }
        MapearPais(peticion) {
            let idPais = this.BuscarValorEnJson(Callejero.objeto.municipioDto.idpais, peticion.resultado.datos);
            let pais = this.BuscarValorEnJson(Callejero.objeto.municipioDto.pais, peticion.resultado.datos);
            let listaDeFiltro = ApiControl.BuscarListaDinamicaPorPropiedad(this.ZonaDeFiltro, Callejero.atributo.propiedad.idpais);
            MapearAlControl.FijarValorEnListaDinamica(listaDeFiltro, idPais, pais);
            let listaDeCreacion = ApiControl.BuscarListaDinamicaPorGuardarEn(this.crudDeCreacion.PanelDeCrear, Callejero.atributo.guardarEn.idpais);
            MapearAlControl.FijarValorEnListaDinamica(listaDeCreacion, idPais, pais);
            let listaDeEdicion = ApiControl.BuscarListaDinamicaPorGuardarEn(this.crudDeEdicion.PanelDeEditar, Callejero.atributo.guardarEn.idpais);
            MapearAlControl.FijarValorEnListaDinamica(listaDeEdicion, idPais, pais);
        }
    }
    Callejero.CrudDeMunicipios = CrudDeMunicipios;
    class CrudCreacionMunicipio extends Crud.CrudCreacion {
        constructor(crud, idPanelCreacion) {
            super(crud, idPanelCreacion);
        }
    }
    Callejero.CrudCreacionMunicipio = CrudCreacionMunicipio;
    class CrudEdicionMunicipio extends Crud.CrudEdicion {
        constructor(crud, idPanelEdicion) {
            super(crud, idPanelEdicion);
        }
    }
    Callejero.CrudEdicionMunicipio = CrudEdicionMunicipio;
})(Callejero || (Callejero = {}));
//# sourceMappingURL=Municipios.js.map