var Crud;
(function (Crud) {
    class ModalParaSeleccionar extends Crud.ModalConGrid {
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
        AbrirModalParaSeleccionar(filtro) {
            this.InicializarModalConGrid();
            this.RecargarGrid()
                .then((valor) => {
                if (!valor)
                    ApiCrud.CerrarModal(this.Modal);
            })
                .catch((valor) => {
                ApiCrud.CerrarModal(this.Modal);
            });
        }
        ;
        CerrarModalParaSeleccionar() {
            this.CerrarModalConGrid();
            this._crud.ModalEnviarCorreo_Abrir();
        }
    }
    Crud.ModalParaSeleccionar = ModalParaSeleccionar;
})(Crud || (Crud = {}));
//# sourceMappingURL=ModalParaSeleccionar.js.map