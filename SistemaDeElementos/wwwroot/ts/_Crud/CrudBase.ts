namespace Crud {

    export class HTMLSelector extends HTMLInputElement {
    }

    export class CrudBase {

        protected get Pagina(): string {
            return this.Estado.Obtener(Sesion.paginaActual);
        }

        private _estado: HistorialSe.EstadoPagina = undefined;

        public get Estado(): HistorialSe.EstadoPagina {
            if (this._estado === undefined) {
                throw new Error("Debe definir la variable estado");
            }
            return this._estado;
        }

        public set Estado(valor: HistorialSe.EstadoPagina) {
            this._estado = valor;
        }

        protected _controlador: string;

        public get Controlador() {
            return this._controlador;
        }

        constructor() {
        }

        public Inicializar(pagina: string): void {
            if (EntornoSe.Historial.HayHistorial(pagina))
                this._estado = EntornoSe.Historial.ObtenerEstadoDePagina(pagina);
            else
                this._estado = new HistorialSe.EstadoPagina(pagina);
        }

        //funciones de ayuda para la herencia


        protected InicializarListasDeElementos(panel: HTMLDivElement, controlador: string): void {
            let listas: NodeListOf<HTMLSelectElement> = panel.querySelectorAll(`select[${atControl.tipo}="${TipoControl.ListaDeElementos}"]`) as NodeListOf<HTMLSelectElement>;
            for (let i = 0; i < listas.length; i++) {
                if (listas[i].getAttribute(atListas.yaCargado) === "S")
                    continue;

                let claseElemento: string = listas[i].getAttribute(atListas.claseElemento);
                this.CargarListaDeElementos(controlador, claseElemento, listas[i].getAttribute(atControl.id));
            }
        }

        protected InicializarListasDinamicas(panel: HTMLDivElement): void {
            let listas: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.ListaDinamica}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < listas.length; i++) {
                let lista: Tipos.ListaDinamica = new Tipos.ListaDinamica(listas[i]);
                lista.Borrar();
            }
        }


        protected InicializarSelectoresDeFecha(panel: HTMLDivElement): void {
            let selectoresDeFecha: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`select[${atControl.tipo}="${TipoControl.SelectorDeFecha}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < selectoresDeFecha.length; i++) {
                this.InicializarFecha(selectoresDeFecha[i]);
            }
        }

        protected InicializarFecha(fecha: HTMLInputElement): void {
            let hora = fecha.getAttribute(atSelectorDeFecha.hora);
            if (!IsNullOrEmpty(hora)) {

            }
        }

        protected InicializarArchivos(panel: HTMLDivElement) {
            let archivos: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.Archivo}"]`) as NodeListOf<HTMLInputElement>;
            archivos.forEach((archivo) => { ApiDeArchivos.BlanquearArchivo(archivo, true); });
        }


        public AntesDeNavegar(valores: Diccionario<any>) {
        }

        protected SiHayErrorTrasPeticionAjax(peticion: ApiDeAjax.DescriptorAjax) {
            MensajesSe.Error("SiHayErrorTrasPeticionAjax", peticion.resultado.mensaje);
        }



        // funciones para mapear un elemento Json a los controles de un panel

        protected MapearElementoLeido(panel: HTMLDivElement, elementoJson: JSON) {
            this.MapearPropiedadesDelElemento(panel, "elementoJson", elementoJson);
            this.MapearRestrictoresDelElemento(panel, elementoJson);
            this.MaperaPropiedadesDeListasDeElementos(panel, elementoJson);
            this.MaperaOpcionesListasDinamicas(panel, elementoJson);
            this.MapearSelectoresDeArchivo(panel, elementoJson);
            this.MapearAreasDeTexto(panel, elementoJson);
            this.MapearFechas(panel, elementoJson);
        }

        private MapearRestrictoresDelElemento(panel: HTMLDivElement, elementoJson: JSON) {

            let restrictores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.restrictorDeEdicion}"]`) as NodeListOf<HTMLInputElement>;
            for (var i = 0; i < restrictores.length; i++) {
                let restrictor: HTMLInputElement = restrictores[i] as HTMLInputElement;
                this.MapearJsonAlRestrictor(restrictor, elementoJson);
            }

        }

        private MapearJsonAlRestrictor(restrictor: HTMLInputElement, elementoJson: JSON): void {
            let propiedad: string = restrictor.getAttribute(atControl.propiedad);
            let mostrar: string = restrictor.getAttribute(atRestrictor.mostrarExpresion);
            if (!IsNullOrEmpty(propiedad)) {
                let idRestrictor: number = this.BuscarValorEnJson(propiedad, elementoJson) as number;
                let texto: string = this.BuscarValorEnJson(mostrar, elementoJson) as string;
                if (!IsNullOrEmpty(texto)) {
                    MapearAlControl.Restrictor(restrictor, idRestrictor, texto);
                }
            }
        }

        private MaperaPropiedadesDeListasDeElementos(panel: HTMLDivElement, elementoJson: JSON) {
            let listas: HTMLCollectionOf<HTMLSelectElement> = panel.getElementsByTagName('select') as HTMLCollectionOf<HTMLSelectElement>;
            for (var i = 0; i < listas.length; i++) {
                let lista: HTMLSelectElement = listas[i] as HTMLSelectElement;
                let guardarEn: string = lista.getAttribute(atListasDinamicasDto.guardarEn);
                let id: number = this.BuscarValorEnJson(guardarEn, elementoJson) as number;
                MapearAlControl.Lista(lista, id);
            }
        }

        private MaperaOpcionesListasDinamicas(panel: HTMLDivElement, elementoJson: JSON) {

            let listas: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.ListaDinamica}"]`) as NodeListOf<HTMLInputElement>;

            for (var i = 0; i < listas.length; i++) {
                let input: HTMLInputElement = listas[i] as HTMLInputElement;
                let propiedad: string = input.getAttribute(atControl.propiedad);
                let guardarEn: string = input.getAttribute(atListasDinamicasDto.guardarEn);
                let id: number = this.BuscarValorEnJson(guardarEn, elementoJson) as number;
                let valor: string = this.BuscarValorEnJson(propiedad, elementoJson) as string;

                if (Numero(id) > 0) {
                    let listaDinamica = new Tipos.ListaDinamica(input);
                    listaDinamica.AgregarOpcion(id, valor);
                    input.value = valor;
                }

                MapearAlControl.ListaDinamica(input, id);
            }
        }

        private MapearPropiedadesDelElemento(panel: HTMLDivElement, propiedad: string, valorPropiedadJson: any) {

            if (valorPropiedadJson === undefined || valorPropiedadJson === null) {
                this.MapearPropiedad(panel, propiedad, "");
                return;
            }

            var tipoDeObjeto = typeof valorPropiedadJson;
            if (tipoDeObjeto === "object") {
                for (var propiedad in valorPropiedadJson) {
                    this.MapearPropiedadesDelElemento(panel, propiedad.toLowerCase(), valorPropiedadJson[propiedad]);
                }
            } else {
                this.MapearPropiedad(panel, propiedad, valorPropiedadJson);
            }
        }

        private BuscarValorEnJson(propiedad: string, valorPropiedadJson: any) {
            var tipoDeObjeto = typeof valorPropiedadJson;
            if (tipoDeObjeto === "object") {
                for (var p in valorPropiedadJson) {
                    if (propiedad.toLowerCase() === p.toLowerCase())
                        return valorPropiedadJson[p];
                }
            }
            return null;
        }


        private MapearPropiedad(panel: HTMLDivElement, propiedad: string, valor: any) {

            if (this.MapearPropiedaAlEditor(panel, propiedad, valor))
                return;

            if (this.MapearPropiedadAlSelectorDeUrlDelArchivo(panel, propiedad, valor))
                return;

            if (this.MapearPropiedadAlCheck(panel, propiedad, valor))
                return;
        }

        private MapearPropiedaAlEditor(panel: HTMLDivElement, propiedad: string, valor: any): boolean {
            let editor: HTMLInputElement = this.BuscarEditor(panel, propiedad);

            if (editor === null)
                return false;

            editor.classList.remove(ClaseCss.crtlNoValido);
            editor.classList.add(ClaseCss.crtlValido);

            editor.value = valor;
            return true;
        }

        private MapearPropiedadAlCheck(panel: HTMLDivElement, propiedad: string, valor: any): boolean {
            let check: HTMLInputElement = this.BuscarCheck(panel, propiedad);

            if (check === null)
                return false;

            check.classList.remove(ClaseCss.crtlNoValido);
            check.classList.add(ClaseCss.crtlValido);

            if (IsBool(valor))
                check.checked = valor === true;
            else
                if (IsString(valor))
                    check.checked = valor.toLowerCase() === 'true';
            return true;
        }


        private MapearSelectoresDeArchivo(panel: HTMLDivElement, elementoJson: JSON) {

            let selectores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.Archivo}"]`) as NodeListOf<HTMLInputElement>;

            for (var i = 0; i < selectores.length; i++) {
                let selector: HTMLInputElement = selectores[i] as HTMLInputElement;
                let propiedad: string = selector.getAttribute(atControl.propiedad);
                let valor: number = this.BuscarValorEnJson(propiedad, elementoJson) as number;
                if (valor !== null) {
                    let visorVinculado: string = selector.getAttribute(atArchivo.imagen);
                    selector.setAttribute(atArchivo.idArchivo, valor.toString());
                    this.MapearImagenes(elementoJson, visorVinculado);
                }
            }
        }
        private MapearAreasDeTexto(panel: HTMLDivElement, elementoJson: JSON): void {

            let areas: NodeListOf<HTMLTextAreaElement> = panel.querySelectorAll(`textarea[tipo="${TipoControl.AreaDeTexto}"]`) as NodeListOf<HTMLTextAreaElement>;
            for (var i = 0; i < areas.length; i++) {
                let area: HTMLTextAreaElement = areas[i] as HTMLTextAreaElement;
                this.MapearAreaDeTexto(area, elementoJson);
            }
        }

        private MapearFechas(panel: HTMLDivElement, elementoJson: JSON ): void {

            let fechas: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.SelectorDeFecha}"]`) as NodeListOf<HTMLInputElement>;
            for (var i = 0; i < fechas.length; i++) {
                let fecha: HTMLInputElement = fechas[i] as HTMLInputElement;
                this.MapearSelectorDeFecha(fecha, elementoJson);
            }

            let fechasHoras: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.SelectorDeFechaHora}"]`) as NodeListOf<HTMLInputElement>;
            for (var i = 0; i < fechasHoras.length; i++) {
                let fecha: HTMLInputElement = fechasHoras[i] as HTMLInputElement;
                this.MapearSelectorDeFecha(fecha, elementoJson);
            }
        }

        private MapearAreaDeTexto(area: HTMLTextAreaElement, elementoJson: JSON): void {
            let propiedad: string = area.getAttribute(atControl.propiedad);
            if (!IsNullOrEmpty(propiedad)) {
                let texto: string = this.BuscarValorEnJson(propiedad, elementoJson) as string;
                if (!IsNullOrEmpty(texto)) {
                    MapearAlControl.Texto(area, texto);
                }
            }
        }

        private MapearSelectorDeFecha(fecha: HTMLInputElement, elementoJson: JSON): void {
            let propiedad: string = fecha.getAttribute(atControl.propiedad);
            if (!IsNullOrEmpty(propiedad)) {
                let valor: string = this.BuscarValorEnJson(propiedad, elementoJson) as string;
                if (!IsNullOrEmpty(valor)) {
                    MapearAlControl.Fecha(fecha, valor);
                    let tipo: string = fecha.getAttribute(atControl.tipo);
                    if (tipo === TipoControl.SelectorDeFechaHora) {
                        MapearAlControl.Hora(fecha, valor);
                    }
                }
                else 
                    ApiControl.BlanquearFecha(fecha)
            }
        }

        private MapearImagenes(elementoJson: JSON, visorVinculado: string): void {
            let visor: HTMLImageElement = document.getElementById(visorVinculado) as HTMLImageElement;
            let propiedadDelVisor: string = visor.getAttribute(atControl.propiedad);
            let url: string = this.BuscarValorEnJson(propiedadDelVisor, elementoJson) as string;
            MapearAlControl.Url(visor, url);
        }

        private MapearPropiedadAlSelectorDeUrlDelArchivo(panel: HTMLDivElement, propiedad: string, valor: any): boolean {
            let selector: HTMLInputElement = this.BuscarUrlDelArchivo(panel, propiedad);

            if (selector === null)
                return false;
            let ruta: string = selector.getAttribute(atArchivo.rutaDestino);
            selector.classList.remove(ClaseCss.crtlNoValido);
            selector.classList.add(ClaseCss.crtlValido);
            selector.setAttribute(atArchivo.nombre, valor);
            this.MapearPropiedadAlVisorDeImagen(panel, propiedad, `${ruta}/${valor}`);

            return true;
        }

        private MapearPropiedadAlVisorDeImagen(panel: HTMLDivElement, propiedad: string, valor: any) {
            let visor: HTMLImageElement = this.BuscarVisorDeImagen(panel, propiedad);
            if (visor === null)
                return;
            MapearAlControl.Url(visor, valor);
        }


        // funciones para la gestión de los mapeos de controles a un json  ****************************************************************************

        protected BuscarEditor(controlPadre: HTMLDivElement, propiedadDto: string): HTMLInputElement {
            let editores: NodeListOf<HTMLInputElement> = controlPadre.querySelectorAll(`input[${atControl.tipo}='${TipoControl.Editor}']`) as NodeListOf<HTMLInputElement>;
            for (var i = 0; i < editores.length; i++) {
                var control = editores[i] as HTMLInputElement;
                var dto = control.getAttribute(atControl.propiedad);
                if (dto === propiedadDto.toLowerCase())
                    return control;
            }
            return null;
        }

        protected BuscarCheck(controlPadre: HTMLDivElement, propiedadDto: string): HTMLInputElement {
            let checkes: NodeListOf<HTMLInputElement> = controlPadre.querySelectorAll(`input[${atControl.tipo}='${TipoControl.Check}']`) as NodeListOf<HTMLInputElement>;
            for (var i = 0; i < checkes.length; i++) {
                var control = checkes[i] as HTMLInputElement;
                var dto = control.getAttribute(atControl.propiedad);
                if (dto === propiedadDto.toLowerCase())
                    return control;
            }
            return null;
        }

        protected BuscarSelectorDeArchivo(controlPadre: HTMLDivElement, propiedadDto: string): HTMLInputElement {
            let selectores: NodeListOf<HTMLInputElement> = controlPadre.querySelectorAll(`input[tipo="${TipoControl.Archivo}"]`) as NodeListOf<HTMLInputElement>;
            for (var i = 0; i < selectores.length; i++) {
                var control = selectores[i] as HTMLInputElement;
                var dto = control.getAttribute(atControl.propiedad);
                if (dto === propiedadDto.toLowerCase())
                    return control;
            }
            return null;
        }

        protected BuscarVisorDeImagen(controlPadre: HTMLDivElement, propiedadDto: string): HTMLImageElement {
            let visor: NodeListOf<HTMLImageElement> = controlPadre.querySelectorAll(`img[${atControl.tipo}='${TipoControl.VisorDeArchivo}']`) as NodeListOf<HTMLImageElement>;
            for (var i = 0; i < visor.length; i++) {
                var control = visor[i] as HTMLImageElement;
                var dto = control.getAttribute(atControl.propiedad);
                if (dto === propiedadDto.toLowerCase())
                    return control;
            }
            return null;
        }

        protected BuscarUrlDelArchivo(controlPadre: HTMLDivElement, propiedadDto: string): HTMLInputElement {
            let selectores: NodeListOf<HTMLInputElement> = controlPadre.querySelectorAll(`input[tipo="${TipoControl.UrlDeArchivo}"]`) as NodeListOf<HTMLInputElement>;
            for (var i = 0; i < selectores.length; i++) {
                var control = selectores[i] as HTMLInputElement;
                var dto = control.getAttribute(atControl.propiedad);
                if (dto === propiedadDto.toLowerCase())
                    return control;
            }
            return null;
        }

        protected BuscarSelect(controlPadre: HTMLDivElement, propiedadDto: string): HTMLSelectElement {
            let select: HTMLCollectionOf<HTMLSelectElement> = controlPadre.getElementsByTagName('select') as HTMLCollectionOf<HTMLSelectElement>;
            for (var i = 0; i < select.length; i++) {
                var control = select[i] as HTMLSelectElement;
                var dto = control.getAttribute(atControl.propiedad);
                if (dto === propiedadDto)
                    return control;
            }
            return null;
        }

        protected BuscarListaDinamica(controlPadre: HTMLDivElement, propiedadDto: string): HTMLInputElement {

            let inputs: NodeListOf<HTMLInputElement> = controlPadre.querySelectorAll(`input[${atControl.tipo}="${TipoControl.ListaDinamica}"]`) as NodeListOf<HTMLInputElement>;

            for (var i = 0; i < inputs.length; i++) {
                var control = inputs[i] as HTMLInputElement;
                var dto = control.getAttribute(atControl.propiedad);
                if (dto === propiedadDto)
                    return control;
            }
            return null;
        }

        public AntesDeMapearDatosDeIU(crud: CrudBase, panel: HTMLDivElement, modoDeTrabajo: string): JSON {
            if (modoDeTrabajo === ModoTrabajo.creando)
                return JSON.parse(`{"${literal.id}":"0"}`);

            if (modoDeTrabajo === ModoTrabajo.editando) {
                let input: HTMLInputElement = crud.BuscarEditor(panel, literal.id);
                if (Number(input.value) <= 0)
                    throw new Error(`El valor del id ${Number(input.value)} debe ser mayor a 0`);
                return JSON.parse(`{"${literal.id}":"${Number(input.value)}"}`);
            }

            throw new Error(`No se ha indicado que hacer para el modo de trabajo ${modoDeTrabajo} antes de mapear los datos de la IU`);
        }


        public DespuesDeMapearDatosDeIU(crud: CrudBase, panel: HTMLDivElement, elementoJson: JSON, modoDeTrabajo: string): JSON {
            return elementoJson;
        }

        public SeleccionarListaDinamica(input: HTMLInputElement) {
            let lista: Tipos.ListaDinamica = new Tipos.ListaDinamica(input);
            let valor: number = lista.BuscarSeleccionado(input.value);
            MapearAlControl.ListaDinamica(input, valor);
        }


        // funciones de carga de elementos para los selectores   ************************************************************************************
        public CargarListaDinamica(input: HTMLInputElement) {
            if (input.getAttribute(atListasDinamicas.cargando) == 'S' || IsNullOrEmpty(input.value)) {
                return;
            }

            let ultimaBuscada: string = input.getAttribute(atListasDinamicas.ultimaCadenaBuscada);
            let criterio: string = input.getAttribute(atListasDinamicas.criterio);
            if (!IsNullOrEmpty(ultimaBuscada)) {
                if (criterio === atCriterio.contiene && ultimaBuscada.includes(input.value))
                    return;
                if (criterio === atCriterio.comienza && ultimaBuscada.startsWith(input.value))
                    return;
            }

            let clase: string = input.getAttribute(atListasDinamicas.claseElemento);
            let idInput: string = input.getAttribute('id');
            let filtro: ClausulaDeFiltrado = ApiFiltro.DefinirFiltroListaDinamica(input, criterio);
            if (filtro === null)
                return;

            let cantidad: string = input.getAttribute(atListasDinamicas.cantidad);
            let url: string = this.DefinirPeticionDeCargarDinamica(this.Controlador, clase, Numero(cantidad), filtro);
            let datosDeEntrada = `{"ClaseDeElemento":"${clase}", "IdInput":"${idInput}", "buscada":"${filtro.valor}" , "criterio":"${filtro.criterio}"}`;
            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EndPoint.CargaDinamica
                , datosDeEntrada
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.AnadirOpcionesListaDinamica
                , this.SiHayErrorAlCargarListasDinamicas
            );
            input.setAttribute(atListasDinamicas.cargando, 'S');
            a.Ejecutar();
        }

        private AnadirOpcionesListaDinamica(peticion: ApiDeAjax.DescriptorAjax) {
            let datosDeEntrada: Tipos.DatosPeticionDinamica = JSON.parse(peticion.DatosDeEntrada);
            let input: HTMLInputElement = document.getElementById(datosDeEntrada.IdInput) as HTMLInputElement;
            try {
                let listaDinamica: Tipos.ListaDinamica = new Tipos.ListaDinamica(input);

                let expresionPorDefecto = atListasDinamicas.expresionPorDefecto;
                let mostrarExpresion = input.getAttribute(atListasDinamicas.mostrarExpresion);
                let expresion: string = "";
                for (var i = 0; i < peticion.resultado.datos.length; i++) {
                    if (expresionPorDefecto.toLowerCase() !== mostrarExpresion.toLowerCase()) {
                        expresion = ParsearExpresion(peticion.resultado.datos[i], mostrarExpresion.toLowerCase());
                    }
                    else
                        expresion = peticion.resultado.datos[i][expresionPorDefecto];

                    listaDinamica.AgregarOpcion(peticion.resultado.datos[i].id, expresion);
                }

                listaDinamica.Lista.click();
            }
            finally {
                input.setAttribute(atListasDinamicas.cargando, 'N');
                input.setAttribute(atListasDinamicas.ultimaCadenaBuscada, datosDeEntrada.buscada);
            }
        }

        private SiHayErrorAlCargarListasDinamicas(peticion: ApiDeAjax.DescriptorAjax) {
            let datosDeEntrada: Tipos.DatosPeticionDinamica = JSON.parse(peticion.DatosDeEntrada);
            let input: HTMLDivElement = document.getElementById(datosDeEntrada.IdInput) as HTMLInputElement;
            try {
                MensajesSe.Error("SiHayErrorAlCargarListasDinamicas", peticion.resultado.mensaje);
            }
            finally {
                input.setAttribute(atListasDinamicas.ultimaCadenaBuscada, '');
                input.setAttribute(atListasDinamicas.cargando, 'N');
            }
        }

        public CargarListaDeElementos(controlador: string, claseDeElementoDto: string, idLista: string) {

            let url: string = this.DefinirPeticionDeCargarElementos(controlador, claseDeElementoDto);
            let datosDeEntrada = `{"ClaseDeElemento":"${claseDeElementoDto}", "IdLista":"${idLista}"}`;
            let a = new ApiDeAjax.DescriptorAjax(this
                , Ajax.EndPoint.CargarLista
                , datosDeEntrada
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , this.MapearElementosEnLista
                , this.SiHayErrorTrasPeticionAjax
            );

            a.Ejecutar();
        }

        private MapearElementosEnLista(peticion: ApiDeAjax.DescriptorAjax) {
            let datosDeEntrada: Tipos.DatosPeticionLista = JSON.parse(peticion.DatosDeEntrada);
            let idLista = datosDeEntrada.IdLista;
            let lista = new Tipos.ListaDeElemento(idLista);
            let input: HTMLInputElement = document.getElementById(idLista) as HTMLInputElement;
            let expresion: string = "";
            let mostrarExpresion = input.getAttribute(atListasDeElemento.mostrarExpresion);

            for (var i = 0; i < peticion.resultado.datos.length; i++) {
                if (atListasDeElemento.expresionPorDefecto !== mostrarExpresion)
                    expresion = ParsearExpresion(peticion.resultado.datos[i], mostrarExpresion);
                else
                    expresion = peticion.resultado.datos[i][mostrarExpresion];
                lista.AgregarOpcion(peticion.resultado.datos[i].id, expresion);
            }

            lista.Lista.setAttribute(atListasDeElemento.yaCargado, "S");
        }

        private DefinirPeticionDeCargarElementos(controlador: string, claseElemento: string): string {
            let url: string = `/${controlador}/${Ajax.EndPoint.CargarLista}?${Ajax.Param.claseElemento}=${claseElemento}`;
            return url;
        }




        private DefinirPeticionDeCargarDinamica(controlador: string, claseElemento: string, cantidad: number, filtro: ClausulaDeFiltrado): string {
            let url: string = `/${controlador}/${Ajax.EndPoint.CargaDinamica}?${Ajax.Param.claseElemento}=${claseElemento}&posicion=0&cantidad=${cantidad}&filtro=${JSON.stringify(filtro)}`;
            return url;
        }

    }

}
