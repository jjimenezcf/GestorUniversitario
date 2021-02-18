var Crud;
(function (Crud) {
    class ModalParaRelacionar extends Crud.ModalConGrid {
        constructor(crudPadre, idModal) {
            super(idModal, document.getElementById(idModal).getAttribute(atControl.crudModal));
            this._crud = crudPadre;
        }
        get Crud() {
            return this._crud;
        }
        get PropiedadRestrictora() {
            return this.Modal.getAttribute(atControl.propiedadRestrictora);
        }
        get IdRestrictor() {
            let propiedadRestrictora = this.PropiedadRestrictora;
            if (IsNullOrEmpty(propiedadRestrictora))
                throw new Error(`la modal ${this.IdModal} no tiene definida la ${propiedadRestrictora}`);
            let input = this.Crud.ZonaDeFiltro.querySelector(`input[${atControl.propiedad}="${propiedadRestrictora}"]`);
            if (input === null)
                throw new Error(`No se ha definido el control input asociado a la ${propiedadRestrictora}`);
            let idRestrictor = input.getAttribute(atControl.restrictor);
            if (IsNullOrEmpty(idRestrictor))
                throw new Error(`No se ha pasado el id del retrictor a la propiedad restrictora ${propiedadRestrictora}`);
            return Numero(idRestrictor);
        }
        InicializarModalParaRelacionar() {
            super.InicializarModalConGrid();
        }
        ;
        CerrarModalParaRelacionar() {
            this.CerrarModalConGrid();
        }
        AbrirModalDeRelacion() {
            if (this.IdRestrictor == 0)
                throw new Error(`Debe seleccionar el elemento a con el que relacionar los elementos`);
            super.AbrirModalConGrid();
        }
        CrearRelaciones() {
            if (this.InfoSelector.Seleccionados.length == 0)
                throw new Error("Debe seleccionar algún registro con los que relacionar el elemento");
            let url = this.DefinirPeticionDeCrearRelaciones();
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.EndPoint.CrearRelaciones, "{}", url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, this.DespuesDeCrearRelaciones, null);
            a.Ejecutar();
        }
        DefinirPeticionDeCrearRelaciones() {
            let idsJson = JSON.stringify(this.InfoSelector.Seleccionados);
            let url = `/${this.Crud.Controlador}/${Ajax.EndPoint.CrearRelaciones}`;
            let parametros = `&${Ajax.Param.id}=${this.IdRestrictor}` +
                `&${Ajax.Param.idsJson}=${idsJson}`;
            let peticion = url + '?' + parametros;
            return peticion;
        }
        DespuesDeCrearRelaciones(peticion) {
            let modlParaRelacionar = peticion.llamador;
            modlParaRelacionar.InfoSelector.QuitarTodos();
            modlParaRelacionar.RecargarGrid();
            modlParaRelacionar.Crud.Buscar(atGrid.accion.buscar, 0);
        }
        FiltrosExcluyentes(clausulas) {
            clausulas = super.FiltrosExcluyentes(clausulas);
            let propiedad = this.PropiedadRestrictora;
            let criterio = literal.filtro.criterio.diferente;
            let valor = this.IdRestrictor;
            let clausula = new ClausulaDeFiltrado(propiedad, criterio, valor.toString());
            clausulas.push(clausula);
            return clausulas;
        }
    }
    Crud.ModalParaRelacionar = ModalParaRelacionar;
})(Crud || (Crud = {}));
//# sourceMappingURL=ModalParaRelacionar.js.map