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
        get EditorDeFiltro() {
            var idEditorDeFiltro = this._selector.getAttribute(atSelectorDeElementos.IdEditorDeFiltro);
            let editorDeFiltro = document.getElementById(idEditorDeFiltro);
            if (NoDefinida(editorDeFiltro))
                throw new Error(`el editor ${idEditorDeFiltro} no está definido en la zona de filtro de la modal asociada al selector ${this._selector.id}`);
            return editorDeFiltro;
        }
        get EditorAsociado() {
            return ApiCrud.ObtenerEditorAsociadoAlSelector(this._selector);
        }
        InicializarModalParaSeleccionar(selector) {
            this.InicializarModalConGrid();
            this._selector = selector;
            this.EditorDeFiltro.value = this.EditorAsociado.value;
        }
        AbrirModalParaSeleccionar(selector) {
            this.InicializarModalParaSeleccionar(selector);
            this.RecargarGrid()
                .then((valor) => {
                if (!valor) {
                    ApiCrud.CerrarModal(this.Modal);
                    let idModal = selector.getAttribute(atSelectorDeElementos.ModalPadre);
                    if (!NoDefinida(idModal))
                        ApiCrud.AbrirModalPorId(idModal);
                }
            })
                .catch((valor) => {
                ApiCrud.CerrarModal(this.Modal);
                let idModal = selector.getAttribute(atSelectorDeElementos.ModalPadre);
                if (!NoDefinida(idModal))
                    ApiCrud.AbrirModalPorId(idModal);
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