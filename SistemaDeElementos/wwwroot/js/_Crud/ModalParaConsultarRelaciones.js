var Crud;
(function (Crud) {
    class ModalParaConsultarRelaciones extends Crud.ModalConGrid {
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
        get Restrictor() {
            let propiedadRestrictora = this.PropiedadRestrictora;
            if (IsNullOrEmpty(propiedadRestrictora))
                throw new Error(`la modal ${this.IdModal} no tiene definida la ${propiedadRestrictora}`);
            let input = this.ZonaDeFiltro.querySelector(`input[${atControl.propiedad}="${propiedadRestrictora}"]`);
            if (input === null)
                throw new Error(`No se ha definido el control input asociado a la ${propiedadRestrictora}`);
            return input;
        }
        AbrirModalParaConsultarRelaciones(seleccionado) {
            this.InicializarModalConGrid();
            this.Restrictor.value = seleccionado.Texto;
            this.Restrictor.setAttribute(atControl.restrictor, seleccionado.Id.toString());
            this.AbrirModalConGrid();
        }
        ;
        CerrarModalParaConsultarRelaciones() {
            this.CerrarModalConGrid();
        }
    }
    Crud.ModalParaConsultarRelaciones = ModalParaConsultarRelaciones;
})(Crud || (Crud = {}));
//# sourceMappingURL=ModalParaConsultarRelaciones.js.map