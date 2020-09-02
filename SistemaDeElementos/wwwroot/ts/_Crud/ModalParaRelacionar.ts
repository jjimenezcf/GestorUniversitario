namespace Crud {

    export class ModalParaRelacionar extends ModalConGrid {

        private _crud: CrudMnt;
        protected get Crud(): CrudMnt {
            return this._crud;
        }

        protected get PropiedadRestrictora(): string {
            return this.Modal.getAttribute(atControl.propiedadRestrictora);
        }

        protected get IdRestrictor(): number {
            let propiedadRestrictora: string = this.PropiedadRestrictora;
            if (IsNullOrEmpty(propiedadRestrictora))
                throw new Error(`la modal ${this.IdModal} no tiene definida la ${propiedadRestrictora}`);

            let input: HTMLInputElement = this.Crud.ZonaDeFiltro.querySelector(`input[${atControl.propiedad}="${propiedadRestrictora}"]`);
            if (input === null)
                throw new Error(`No se ha definido el control input asociado a la ${propiedadRestrictora}`);


            let idRestrictor: string = input.getAttribute(atControl.restrictor);
            if (IsNullOrEmpty(idRestrictor))
                throw new Error(`No se ha pasado el id del retrictor a la propiedad restrictora ${propiedadRestrictora}`);

            return Numero(idRestrictor);
        }

        constructor(crudPadre: CrudMnt, idModal: string) {
            super(idModal, document.getElementById(idModal).getAttribute(atControl.crudModal));
            this._crud = crudPadre;
        }

        public InicializarModalParaRelacionar() {
            super.InicializarModalConGrid();
        };

        public CerrarModalParaRelacionar() {
            this.CerrarModalConGrid();
        }

        public AbrirModalDeRelacion() {
            if (this.Crud.InfoSelector.Cantidad != 1)
                throw new Error(`Debe seleccionar el elemento a relacionar, ha seleccionado ${this.InfoSelector.Cantidad}`);

            super.AbrirModalConGrid();
        }

        public CrearRelaciones() {
            if (this.InfoSelector.Seleccionados.length == 0) 
                throw new Error("Debe seleccionar algún registro con los que relacionar el elemento");
            
            let url: string = this.DefinirPeticionDeCrearRelaciones(Ajax.EndPoint.CrearRelaciones);
            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EndPoint.CrearRelaciones
                , "{}"
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.DespuesDeCrearRelaciones
                , null
            );

            a.Ejecutar();
        }


        private DefinirPeticionDeCrearRelaciones(endPoint: string): string {

            let idsJson: string = JSON.stringify(this.InfoSelector.Seleccionados);
            let url: string = `/${this.Crud.Controlador}/${endPoint}`;
            let parametros: string =
                `&${Ajax.Param.id}=${this.IdRestrictor}` +
                `&${Ajax.Param.idsJson}=${idsJson}`;
            let peticion: string = url + '?' + parametros;
            return peticion;
        }

        private DespuesDeCrearRelaciones(peticion: ApiDeAjax.DescriptorAjax) {
            let modlParaRelacionar: ModalParaRelacionar = peticion.llamador as ModalParaRelacionar;
            modlParaRelacionar.RecargarGrid();
        }

        protected FiltrosExcluyentes(clausulas: ClausulaDeFiltrado[]): ClausulaDeFiltrado[] {
            clausulas = super.FiltrosExcluyentes(clausulas);
            let propiedad: string = this.PropiedadRestrictora;
            let criterio: string = Literal.filtro.criterio.diferente;
            let valor = this.IdRestrictor;
            let clausula: ClausulaDeFiltrado = new ClausulaDeFiltrado(propiedad, criterio, valor.toString());
            clausulas.push(clausula);
            return clausulas
        }
    }
}
