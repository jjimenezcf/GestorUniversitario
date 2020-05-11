namespace Crud {

    export class CrudCreacion extends CrudBase {

        protected PanelDeCrear: HTMLDivElement;
        protected PanelDeMnt: HTMLDivElement;
        protected CrudDeMnt: CrudMnt;

        private get Controlador(): string {
            return this.PanelDeCrear.getAttribute(Literal.controlador);
        }
        constructor(crud: CrudMnt, idPanelCreacion: string) {
            super();

            if (EsNula(idPanelCreacion))
                throw Error("No se puede construir un objeto del tipo CrudCreacion sin indica el panel de creación");

            this.PanelDeCrear = document.getElementById(idPanelCreacion) as HTMLDivElement;
            this.PanelDeMnt = crud.PanelDeMnt;
            this.CrudDeMnt = crud;
        }


        public EjecutarAcciones(accion: string) {
            let hayError: boolean = false;
            try {
                if (accion === LiteralCrt.Accion.NuevoElemento)
                    this.Crear();
                else
                    if (accion === LiteralCrt.Accion.CancelarNuevo)
                        hayError = false;
                    else
                        throw `la opción ${accion} no está definida`;
            }
            catch (error) {
                hayError = true;
                Mensaje(TipoMensaje.Error, error);
            }

            if (!hayError) this.CerrarCreacion();
        }

        public ComenzarCreacion(panelAnterior: HTMLDivElement) {
            this.OcultarPanel(panelAnterior);
            this.MostrarPanel(this.PanelDeCrear);
            this.InicializarSlectoresDeElementos(this.PanelDeCrear, this.Controlador);
        }

        public Crear() {
            let json: JSON = this.MapearControlesDeIU(this.PanelDeCrear);
            this.CrearElemento(json);
        }

        public CerrarCreacion() {
            this.Cerrar(this.PanelDeMnt, this.PanelDeCrear);
            this.CrudDeMnt.Buscar(0);
        }

        private CrearElemento(json: JSON) {
            let url: string = `/${this.Controlador}/${Ajax.EndPoint.Crear}?${Ajax.Param.elementoJson}=${JSON.stringify(json)}`;
            let req: XMLHttpRequest = new XMLHttpRequest();
            let peticion: PeticionAjax = new PeticionAjax(Ajax.EndPoint.Crear, "{}")
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

            return resultado;
        }
    }

    export function EjecutarMenuCrt(accion: string): void {
        crudMnt.crudDeCreacion.EjecutarAcciones(accion);
    }

}
