namespace Crud {

    export class CrudEdicion extends CrudBase {

        protected PanelDeEditar: HTMLDivElement;
        protected PanelDeMnt: HTMLDivElement;
        protected CrudDeMnt: CrudMnt;

        private get Controlador(): string {
            return this.PanelDeEditar.getAttribute(Literal.controlador);
        }

        constructor(crud: CrudMnt, idPanelEdicion: string) {
            super();

            if (EsNula(idPanelEdicion))
                throw Error("No se puede construir un objeto del tipo CrudEdicion sin indica el panel de edición");

            this.PanelDeEditar = document.getElementById(idPanelEdicion) as HTMLDivElement;
            this.PanelDeMnt = crud.PanelDeMnt;
            this.CrudDeMnt = crud;
        }

        public EjecutarAcciones(accion: string) {
            let hayError: boolean = false;
            try {
                if (accion === LiteralEdt.Accion.ModificarElemento)
                    this.Modificar();
                else
                    if (accion === LiteralEdt.Accion.CancelarEdicion)
                        hayError = false;
                    else
                        throw `la opción ${accion} no está definida`;
            }
            catch (error) {
                hayError = true;
                Mensaje(TipoMensaje.Error, error);
            }

            if (!hayError) this.CerrarEdicion();
        }

        public ComenzarEdicion(panelAnterior: HTMLDivElement, infSel: InfoSelector) {
            this.ModoTrabajo = ModoTrabajo.editando;
            this.OcultarPanel(panelAnterior);
            this.MostrarPanel(this.PanelDeEditar);
            this.InicializarSlectoresDeElementos(this.PanelDeEditar, this.Controlador);
            this.InicializarValores(infSel);
        }

        protected CerrarEdicion() {
            this.Cerrar(this.PanelDeMnt, this.PanelDeEditar);
            this.ModoTrabajo = ModoTrabajo.mantenimiento;
            this.CrudDeMnt.Buscar(0);
        }

        protected InicializarValores(infSel: InfoSelector) {
            let id: number = infSel.Seleccionados[0] as number;

            let control: HTMLElement = this.BuscarInput(this.PanelDeEditar, Literal.id);
            if (control == null) {
                Mensaje(TipoMensaje.Error, "No está definido el control para mostrar el id del elemento");
                return;
            }
            (control as HTMLInputElement).value = id.toString();

            this.LeerElemento(id);
        }

        private LeerElemento(id: number) {
            let idJson: string = this.DefinirFiltroPorId(id);
            let url: string = `/${this.Controlador}/${Ajax.EndPoint.LeerPorIds}?${Ajax.Param.idsJson}=${idJson}`;
            let req: XMLHttpRequest = new XMLHttpRequest();
            let peticion: PeticionAjax = new PeticionAjax(Ajax.EndPoint.LeerPorIds, "{}");
            this.PeticionSincrona(req, url, peticion);
        }

        private DefinirFiltroPorId(id: number): string {
            var clausulas = new Array<ClausulaDeFiltrado>();
            var clausula: ClausulaDeFiltrado = new ClausulaDeFiltrado('id','igual', `${id}`);
            clausulas.push(clausula);
            return JSON.stringify(clausulas);
        }

        protected Modificar() {
            let json: JSON = this.MapearControlesDeIU(this.PanelDeEditar);
            this.ModificarElemento(json);
        }

        private ModificarElemento(json: JSON) {
            let controlador = this.PanelDeEditar.getAttribute(Literal.controlador);
            let url: string = `/${controlador}/${Ajax.EndPoint.Modificar}?${Ajax.Param.elementoJson}=${JSON.stringify(json)}`;
            let req: XMLHttpRequest = new XMLHttpRequest();
            let peticion: PeticionAjax = new PeticionAjax(Ajax.EndPoint.Modificar, "{}");
            this.PeticionSincrona(req, url, peticion);
        }

        protected DespuesDeLaPeticion(req: XMLHttpRequest, peticion: PeticionAjax): ResultadoJson {

            let resultado: ResultadoJson = super.DespuesDeLaPeticion(req, peticion) as ResultadoJson;

            if (peticion.nombre === Ajax.EndPoint.LeerTodos) {
                let datos: DatosPeticionSelector = JSON.parse(peticion.datos);
                let idSelector = datos.IdSelector;
                let selector = new SelectorDeElementos(idSelector);
                for (var i = 0; i < resultado.datos.length; i++) {
                    selector.AgregarOpcion(resultado.datos[i].id, resultado.datos[i].nombre);
                }
            }

            if (peticion.nombre === Ajax.EndPoint.LeerPorIds) {
                    this.MapearElementoLeido(this.PanelDeEditar, resultado.datos[0]);
            }

            return resultado;
        }

    }

    export function EjecutarMenuEdt(accion: string): void {
        crudMnt.crudDeEdicion.EjecutarAcciones(accion);
    }

}