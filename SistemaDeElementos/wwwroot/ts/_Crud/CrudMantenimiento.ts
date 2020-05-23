namespace Crud {

    export let crudMnt: CrudMnt = null;



    export class CrudMnt extends GridMnt {

        public crudDeCreacion: CrudCreacion;
        public crudDeEdicion: CrudEdicion;
        public idModalBorrar: string;

        public PanelDeMnt: HTMLDivElement;

        public Modales: Array<ModalSeleccion> = new Array<ModalSeleccion>();

        constructor(idPanelMnt: string) {
            super(`${idPanelMnt}_grid`);

            if (EsNula(idPanelMnt))
                throw Error("No se puede construir un objeto del tipo CrudMantenimiento sin indica el panel de mantenimiento");

            this.InicializarInformacionPaneles(idPanelMnt);
            this.InicializarNavegador();
            this.InicializarSelectores();
            this.InicializarSlectoresDeElementos(this.ZonaDeFiltro, this.Controlador);
        }

        private InicializarInformacionPaneles(idPanelMnt: string) {
            this.PanelDeMnt = document.getElementById(idPanelMnt) as HTMLDivElement;
        }

        private InicializarSelectores() {
            let selectores: NodeListOf<HTMLSelector> = this.ZonaDeFiltro.querySelectorAll(`input[tipo="${TipoControl.Selector}"]`) as NodeListOf<HTMLSelector>;;
            selectores.forEach((selector) => {

                let idGridModal: string = selector.getAttribute(AtributoSelector.idGridModal);

                let idModal: string = selector.getAttribute(AtributoSelector.idModal);
                let modal: ModalSeleccion = new ModalSeleccion(idModal, idGridModal);
                this.Modales.push(modal);

            });
        }

        public ObtenerModal(idModal: string): ModalSeleccion {
            for (let i: number = 0; i < this.Modales.length; i++) {
                let modal: ModalSeleccion = this.Modales[i];
                if (modal.IdModal === idModal)
                    return modal;
            }
            return undefined;
        }

        public AbrirModalBorrarElemento() {
            if (this.InfoSelector.Cantidad == 0) {
                Mensaje(TipoMensaje.Info, "Debe marcar el elemento a borrar");
                return;
            }
            this.AbrirModalDeBorrar();
        }

        public CerrarModalDeBorrado() {
            this.ModoTrabajo = ModoTrabajo.mantenimiento;
            this.CerrarModal(this.idModalBorrar);
        }


        private AbrirModalDeBorrar() {
            this.ModoTrabajo = ModoTrabajo.borrando;
            var ventana = document.getElementById(this.idModalBorrar);
            ventana.style.display = 'block';
        }

        public IraEditar() {
            if (this.InfoSelector.Cantidad == 0) {
                Mensaje(TipoMensaje.Info, "Debe marcar el elemento a editar");
                return;
            }

            this.crudDeEdicion.ComenzarEdicion(crudMnt.PanelDeMnt, this.InfoSelector);
        }

        public IraCrear() {
            this.crudDeCreacion.ComenzarCreacion(crudMnt.PanelDeMnt);
        }

        public FilaPulsada(idCheck: string, idDelInput: string) {

            let check: HTMLInputElement = document.getElementById(idCheck) as HTMLInputElement;
            //Se hace porque antes ha pasado por aquí por haber pulsado en la fila
            if (idCheck === idDelInput) {
                check.checked = !check.checked;
                return;
            }

            check.checked = !check.checked;

            if (check.checked) {
                let expresionElemento: string = this.ObtenerExpresionMostrar(idCheck);
                this.AnadirAlInfoSelector(idCheck, expresionElemento);
            }
            else
                this.QuitarDelSelector(idCheck);
        }

        public OrdenarPor(columna: string) {
            this.EstablecerOrdenacion(columna);
            this.Buscar(0);
        }

        public ObtenerUltimos() {
            this.Buscar(-1);
        }

        public ObtenerAnteriores() {
            let cantidad: number = this.Navegador.value.Numero();
            let posicion: number = this.Navegador.getAttribute(Atributo.posicion).Numero();
            posicion = posicion - (cantidad * 2);
            if (posicion < 0)
                posicion = 0;
            this.Buscar(posicion);
        }

        public ObtenerSiguientes() {
            let posicion: number = this.Navegador.getAttribute(Atributo.posicion).Numero();
            this.Buscar(posicion);
        }

        public BorrarElemento() {
            this.CerrarModalDeBorrado();
            let id: number = this.InfoSelector.Seleccionados[0] as number;
            let url: string = this.DefinirPeticionDeBorrado(id);

            let a = new ApiDeAjax.DescriptorAjax(Ajax.EndPoint.Borrar
                , this
                , url
                , ApiDeAjax.TipoPeticion.Sincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.RecargarGrid
                , null
            );

            try {
                a.Ejecutar();
            }
            catch (error) {
                Mensaje(TipoMensaje.Error, error);
                return;
            }
        }

        private DefinirPeticionDeBorrado(id: number): string {
            let idJson: JSON = JSON.parse(`[${id}]`);
            var controlador = this.Navegador.getAttribute(Atributo.controlador);
            let url: string = `/${controlador}/${Ajax.EndPoint.Borrar}`;
            let parametros: string = `${Ajax.Param.idsJson}=${JSON.stringify(idJson)}`;
            let peticion: string = url + '?' + parametros;
            return peticion;
        }

        private RecargarGrid(peticion: ApiDeAjax.DescriptorAjax) {
            let mnt: CrudMnt = (peticion.DatosDeEntrada as CrudMnt);
            mnt.InfoSelector.QuitarTodos();
            mnt.Buscar(0);
        }

        public Buscar(posicion: number) {
            if (this.Navegador === null)
                Mensaje(TipoMensaje.Error, `No está definido el control de la cantidad de elementos a obtener`);
            else {
                let url: string = this.DefinirPeticionDeBusqueda(posicion);

                let a = new ApiDeAjax.DescriptorAjax(Ajax.EndPoint.LeerGridEnHtml
                    , this
                    , url
                    , ApiDeAjax.TipoPeticion.Sincrona
                    , ApiDeAjax.ModoPeticion.Get
                    , this.ActualizarGrid
                    , null
                );

                a.Ejecutar();
            }
        }

        private ActualizarGrid(peticion: ApiDeAjax.DescriptorAjax) {
            let mnt: CrudMnt = (peticion.DatosDeEntrada as CrudMnt);
            let resultado = peticion.resultado as ResultadoHtml;

            if (mnt.IdGrid === mnt.Grid.getAttribute(Atributo.id)) {
                mnt.ActualizarGridHtml(mnt, resultado.html);
            }
        }

        private DefinirPeticionDeBusqueda(posicion: number): string {
            var cantidad = this.Navegador.value.Numero();
            var controlador = this.Navegador.getAttribute(Atributo.controlador);
            var filtroJson = this.ObtenerFiltros();
            var ordenJson = this.ObtenerOrdenacion();

            let url: string = `/${controlador}/${Ajax.EndPoint.LeerGridEnHtml}`;
            let parametros: string = `${Ajax.Param.modo}=Mantenimiento` +
                `&${Ajax.Param.posicion}=${posicion}` +
                `&${Ajax.Param.cantidad}=${cantidad}` +
                `&${Ajax.Param.filtro}=${filtroJson}` +
                `&${Ajax.Param.orden}=${ordenJson}`;
            let peticion: string = url + '?' + parametros;
            return peticion;
        }

        public CambiarSelector(idSelector: string) {
            var htmlSelector: HTMLSelector = <HTMLSelector>document.getElementById(idSelector);

            let modal: ModalSeleccion = crudMnt.ObtenerModal(htmlSelector.getAttribute(AtributoSelector.idModal));
            if (EsNula(htmlSelector.value))
                modal.InicializarModal();
            else
               modal.TextoSelectorCambiado(htmlSelector.value);
        }
    }
}