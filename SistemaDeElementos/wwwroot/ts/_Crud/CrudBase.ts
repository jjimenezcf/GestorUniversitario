﻿namespace Crud {

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

        constructor() {
        }

        public Inicializar(pagina: string) {
            if (EntornoSe.Historial.HayHistorial(pagina))
                this._estado = EntornoSe.Historial.ObtenerEstadoDePagina(pagina);
            else
                this._estado = new HistorialSe.EstadoPagina(pagina);
        }

        //funciones de ayuda para la herencia

        
        protected InicializarListasDeElementos(panel: HTMLDivElement, controlador: string) {
        let listas: NodeListOf<HTMLSelectElement> = panel.querySelectorAll(`select[${atControl.tipo}="${TipoControl.ListaDeElementos}"]`) as NodeListOf<HTMLSelectElement>;
        for (let i = 0; i < listas.length; i++) {
            if (listas[i].getAttribute(atListas.yaCargado) === "S")
                continue;

            let claseElemento: string = listas[i].getAttribute(atListas.claseElemento);
            this.CargarListaDeElementos(controlador, claseElemento, listas[i].getAttribute(atControl.id));
        }
    }

        protected InicializarListasDinamicas(panel: HTMLDivElement) {
            let listas: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.ListaDinamica}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < listas.length; i++) {
                let lista: Tipos.ListaDinamica = new Tipos.ListaDinamica(listas[i]);
                lista.Borrar();
            }
        }

        protected InicializarCanvases(panel: HTMLDivElement) {
            let canvases: NodeListOf<HTMLCanvasElement> = panel.querySelectorAll("canvas") as NodeListOf<HTMLCanvasElement>;
            canvases.forEach((canvas) => { canvas.width = canvas.width; });
        }


        public AntesDeNavegar(valores: Diccionario<any>) {
        }

        protected SiHayErrorTrasPeticionAjax(peticion: ApiDeAjax.DescriptorAjax) {
            Mensaje(TipoMensaje.Error, peticion.resultado.mensaje);
        }

        // funciones para mapear un elemento Json a los controles de un panel

        protected MapearElementoLeido(panel: HTMLDivElement, elementoJson: JSON, modoDeAcceso: string) {
            this.MapearPropiedadesDelElemento(panel, "elementoJson", elementoJson, modoDeAcceso);
            this.MaperaPropiedadesDeListasDeElementos(panel, elementoJson, modoDeAcceso);
            this.MaperaOpcionesListasDinamicas(panel, elementoJson, modoDeAcceso);
            this.MapearSelectoresDeArchivo(panel, elementoJson);
        }

        private MaperaPropiedadesDeListasDeElementos(panel: HTMLDivElement, elementoJson: JSON, modoDeAcceso: string) {
            let select: HTMLCollectionOf<HTMLSelectElement> = panel.getElementsByTagName('select') as HTMLCollectionOf<HTMLSelectElement>;
            for (var i = 0; i < select.length; i++) {
                let selector: HTMLSelectElement = select[i] as HTMLSelectElement;
                let guardarEn: string = selector.getAttribute(atListasDinamicasDto.guardarEn);
                let id: number = this.BuscarValorEnJson(guardarEn, elementoJson) as number;
                if (id === null || id == 0)
                    selector.selectedIndex = 0;
                else
                    for (var j = 0; j < selector.options.length; j++) {
                        if (Numero(selector.options[j].value) == id) {
                            selector.selectedIndex = j;
                            break;
                        }
                    }

                selector.classList.remove(ClaseCss.soloLectura);
                if (modoDeAcceso === ModoDeAccesoDeDatos.Consultor) {
                    selector.disabled = true;
                    selector.classList.add(ClaseCss.soloLectura);
                }
            }
        }

        private MaperaOpcionesListasDinamicas(panel: HTMLDivElement, elementoJson: JSON, modoDeAcceso: string) {

            let listas: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.ListaDinamica}"]`) as NodeListOf<HTMLInputElement>;

            for (var i = 0; i < listas.length; i++) {
                let input: HTMLInputElement = listas[i] as HTMLInputElement;
                let propiedad: string = input.getAttribute(atControl.propiedad);
                let guardarEn: string = input.getAttribute(atListasDinamicasDto.guardarEn);
                let id: number = this.BuscarValorEnJson(guardarEn, elementoJson) as number;
                let valor: string = this.BuscarValorEnJson(propiedad, elementoJson) as string;

                if (id === null || id == 0)
                    input.value = "";
                else {
                    let listaDinamica = new Tipos.ListaDinamica(input);
                    listaDinamica.AgregarOpcion(id, valor);
                    input.value = valor;
                }

                input.classList.remove(ClaseCss.soloLectura);
                if (modoDeAcceso === ModoDeAccesoDeDatos.Consultor) {
                    input.disabled = true;
                    input.classList.add(ClaseCss.soloLectura);
                }

            }
        }

        private MapearPropiedadesDelElemento(panel: HTMLDivElement, propiedad: string, valorPropiedadJson: any, modoDeAcceso: string) {

            if (valorPropiedadJson === undefined || valorPropiedadJson === null) {
                this.MapearPropiedad(panel, propiedad, "", modoDeAcceso);
                return;
            }

            var tipoDeObjeto = typeof valorPropiedadJson;
            if (tipoDeObjeto === "object") {
                for (var propiedad in valorPropiedadJson) {
                    this.MapearPropiedadesDelElemento(panel, propiedad.toLowerCase(), valorPropiedadJson[propiedad], modoDeAcceso);
                }
            } else {
                this.MapearPropiedad(panel, propiedad, valorPropiedadJson, modoDeAcceso);
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


        private MapearPropiedad(panel: HTMLDivElement, propiedad: string, valor: any, modoDeAcceso: string) {

            if (this.MapearPropiedaAlEditor(panel, propiedad, valor, modoDeAcceso))
                return;

            //if (this.MapearPropiedadAlSelectorDeArchivo(panel, propiedad, valor))
            //    return;

            if (this.MapearPropiedadAlSelectorDeUrlDelArchivo(panel, propiedad, valor, modoDeAcceso))
                return;

            if (this.MapearPropiedadAlCheck(panel, propiedad, valor, modoDeAcceso))
                return;
        }

        private MapearPropiedaAlEditor(panel: HTMLDivElement, propiedad: string, valor: any, modoDeAcceso: string): boolean {
            let editor: HTMLInputElement = this.BuscarEditor(panel, propiedad);

            if (editor === null)
                return false;

            editor.classList.remove(ClaseCss.crtlNoValido);
            editor.classList.remove(ClaseCss.soloLectura);

            if (modoDeAcceso === ModoDeAccesoDeDatos.Consultor) {
                editor.readOnly = true;
                editor.classList.add(ClaseCss.soloLectura);
            }
            else
                editor.classList.add(ClaseCss.crtlValido);

            editor.value = valor;
            return true;
        }

        private MapearPropiedadAlCheck(panel: HTMLDivElement, propiedad: string, valor: any, modoDeAcceso: string): boolean {
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

            if (modoDeAcceso === ModoDeAccesoDeDatos.Consultor) {
                check.disabled = true;
            }

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
                    selector.setAttribute(atArchivo.id, valor.toString());
                    this.MapearImagenes(elementoJson, visorVinculado);
                }
            }

        }

        private MapearImagenes(elementoJson: JSON, visorVinculado: string) {
            let visor: HTMLImageElement = document.getElementById(visorVinculado) as HTMLImageElement;
            let propiedadDelVisor: string = visor.getAttribute(atControl.propiedad);
            let url: string = this.BuscarValorEnJson(propiedadDelVisor, elementoJson) as string;
            this.MostrarImagenUrl(visor, url);
        }


        private MapearPropiedadAlSelectorDeUrlDelArchivo(panel: HTMLDivElement, propiedad: string, valor: any, modoDeAcceso: string): boolean {
            let selector: HTMLInputElement = this.BuscarUrlDelArchivo(panel, propiedad);

            if (selector === null)
                return false;
            let ruta: string = selector.getAttribute(atArchivo.rutaDestino);
            if (modoDeAcceso === ModoDeAccesoDeDatos.Consultor) {
                let ref = document.getElementById(`${selector.id}.ref`);
                ref.style.visibility = "hidden";
            }

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

            this.MostrarImagenUrl(visor, valor);
        }

        protected MapearRestrictor(restrictores: NodeListOf<HTMLInputElement>, porpiedadRestrictora: string, valorMostrar: string, valorRestrictor: number) {
            for (let i = 0; i < restrictores.length; i++) {
                if (restrictores[i].getAttribute(atControl.propiedad) === porpiedadRestrictora) {
                    restrictores[i].setAttribute(atControl.valorInput, valorMostrar);
                    restrictores[i].setAttribute(atControl.restrictor, valorRestrictor.toString());
                }
            }
        }

        private MostrarImagenUrl(visor: HTMLImageElement, url: any) {
            visor.setAttribute('src', url);
            let idCanva: string = visor.getAttribute(atControl.id).replace('img', 'canvas');
            let htmlCanvas: HTMLCanvasElement = document.getElementById(idCanva) as HTMLCanvasElement;
            htmlCanvas.width = 100;
            htmlCanvas.height = 100;
            var canvas = htmlCanvas.getContext('2d');
            var img = new Image();
            img.src = url;
            img.onload = function () {
                canvas.drawImage(img, 0, 0, 100, 100);
            };
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

        protected AntesDeMapearDatosDeIU(panel: HTMLDivElement, modoDeTrabajo: string): JSON {
            if (modoDeTrabajo === ModoTrabajo.creando)
                return JSON.parse(`{"${literal.id}":"0"}`);

            if (modoDeTrabajo === ModoTrabajo.editando) {
                let input: HTMLInputElement = this.BuscarEditor(panel, literal.id);
                if (Number(input.value) <= 0)
                    throw new Error(`El valor del id ${Number(input.value)} debe ser mayor a 0`);
                return JSON.parse(`{"${literal.id}":"${Number(input.value)}"}`);
            }

            throw new Error(`No se ha indicado que hacer para el modo de trabajo ${modoDeTrabajo} antes de mapear los datos de la IU`);
        }

        protected MapearControlesDeIU(panel: HTMLDivElement, modoDeTrabajo: string): JSON {
            let elementoJson: JSON = this.AntesDeMapearDatosDeIU(panel, modoDeTrabajo);

            this.MapearSelectoresDeElementosAlJson(panel, elementoJson);
            this.MapearSelectoresDinamicosAlJson(panel, elementoJson);
            this.MapearRestrictoresAlJson(panel, elementoJson);
            this.MapearEditoresAlJson(panel, elementoJson);
            this.MapearArchivosAlJson(panel, elementoJson);
            this.MapearUrlArchivosAlJson(panel, elementoJson);
            this.MapearCheckesAlJson(panel, elementoJson);

            return this.DespuesDeMapearDatosDeIU(panel, elementoJson, modoDeTrabajo);
        }

        protected MapearEditoresAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
            let editores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.Editor}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < editores.length; i++) {
                this.MapearEditorAlJson(editores[i], elementoJson);
            }
        }

        private MapearEditorAlJson(input: HTMLInputElement, elementoJson: JSON): void {
            var propiedadDto = input.getAttribute(atControl.propiedad);
            let valor: string = (input as HTMLInputElement).value;
            let obligatorio: string = input.getAttribute(atControl.obligatorio);

            if (obligatorio === "S" && NoDefinida(valor)) {
                input.classList.remove(ClaseCss.crtlValido);
                input.classList.add(ClaseCss.crtlNoValido);
                throw new Error(`El campo ${propiedadDto} es obligatorio`);
            }

            input.classList.remove(ClaseCss.crtlNoValido);
            input.classList.add(ClaseCss.crtlValido);
            elementoJson[propiedadDto] = valor;
        }


        protected MapearRestrictoresAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
            let restrictores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.restrictorDeEdicion}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < restrictores.length; i++) {
                this.MapearRestrictorAlJson(restrictores[i], elementoJson);
            }
        }

        private MapearRestrictorAlJson(input: HTMLInputElement, elementoJson: JSON): void {
            let propiedadDto: string = input.getAttribute(atControl.propiedad);
            let idRestrictor: string = input.getAttribute(atControl.restrictor);

            if (!NumeroMayorDeCero(idRestrictor)) {
                input.classList.remove(ClaseCss.crtlValido);
                input.classList.add(ClaseCss.crtlNoValido);
                throw new Error(`El campo ${propiedadDto} es obligatorio`);
            }

            input.classList.remove(ClaseCss.crtlNoValido);
            input.classList.add(ClaseCss.crtlValido);
            elementoJson[propiedadDto] = idRestrictor;
        }

        protected MapearCheckesAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
            let checkes: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.Check}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < checkes.length; i++) {
                this.MapearCheckAlJson(checkes[i], elementoJson);
            }
        }


        private MapearCheckAlJson(check: HTMLInputElement, elementoJson: JSON): void {
            var propiedadDto = check.getAttribute(atControl.propiedad);
            elementoJson[propiedadDto] = check.checked;
        }
        protected MapearSelectoresDinamicosAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
            let selectores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.ListaDinamica}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < selectores.length; i++) {
                this.MapearSelectorDinamico(selectores[i], elementoJson);
            }
        }

        private MapearSelectorDinamico(input: HTMLInputElement, elementoJson: JSON) {
            let propiedadDto = input.getAttribute(atControl.propiedad);
            let guardarEn: string = input.getAttribute(atListasDinamicasDto.guardarEn);
            let obligatorio: string = input.getAttribute(atControl.obligatorio);
            let lista: Tipos.ListaDinamica = new Tipos.ListaDinamica(input);
            let valor: number = lista.BuscarSeleccionado(input.value);

            if (obligatorio === "S" && (IsNullOrEmpty(input.value) || Number(valor) === 0)) {
                input.classList.remove(ClaseCss.crtlValido);
                input.classList.add(ClaseCss.crtlNoValido);
                throw new Error(`Debe seleccionar un elemento de la lista ${propiedadDto}`);
            }

            input.classList.remove(ClaseCss.crtlNoValido);
            input.classList.add(ClaseCss.crtlValido);
            elementoJson[guardarEn] = valor.toString();
        }

        protected MapearSelectoresDeElementosAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
            let selectores: NodeListOf<HTMLSelectElement> = panel.querySelectorAll(`select[tipo="${TipoControl.ListaDeElementos}"]`) as NodeListOf<HTMLSelectElement>;
            for (let i = 0; i < selectores.length; i++) {
                this.MapearSelectorDeElementosAlJson(selectores[i], elementoJson);
            }
        }

        private MapearSelectorDeElementosAlJson(selector: HTMLSelectElement, elementoJson: JSON) {
            let propiedadDto = selector.getAttribute(atControl.propiedad);
            let guardarEn: string = selector.getAttribute(atListasDinamicasDto.guardarEn);
            let obligatorio: string = selector.getAttribute(atControl.obligatorio);

            if (obligatorio === "S" && Number(selector.value) === 0) {
                selector.classList.remove(ClaseCss.crtlValido);
                selector.classList.add(ClaseCss.crtlNoValido);
                throw new Error(`Debe seleccionar un elemento de la lista ${propiedadDto}`);
            }

            selector.classList.remove(ClaseCss.crtlNoValido);
            selector.classList.add(ClaseCss.crtlValido);
            elementoJson[guardarEn] = selector.value;
        }

        protected MapearArchivosAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
            let archivos: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.Archivo}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < archivos.length; i++) {
                this.MapearArchivoAlJson(archivos[i], elementoJson);
            }
        }


        private MapearArchivoAlJson(archivo: HTMLInputElement, elementoJson: JSON): void {
            var propiedadDto = archivo.getAttribute(atControl.propiedad);
            let valor: string = archivo.getAttribute(atArchivo.id);
            let obligatorio: string = archivo.getAttribute(atControl.obligatorio);

            if (obligatorio === "S" && IsNullOrEmpty(valor)) {
                archivo.classList.remove(ClaseCss.crtlValido);
                archivo.classList.add(ClaseCss.crtlNoValido);
                throw new Error(`El campo ${propiedadDto} es obligatorio`);
            }

            archivo.classList.remove(ClaseCss.crtlNoValido);
            archivo.classList.add(ClaseCss.crtlValido);
            elementoJson[propiedadDto] = valor;
        }

        private MapearUrlArchivosAlJson(panel: HTMLDivElement, elementoJson: JSON): void {
            let urlsDeArchivos: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.UrlDeArchivo}"]`) as NodeListOf<HTMLInputElement>;
            for (let i = 0; i < urlsDeArchivos.length; i++) {
                this.MapearUrlArchivoAlJson(urlsDeArchivos[i], elementoJson);
            }
        }

        private MapearUrlArchivoAlJson(urlDeArchivo: HTMLInputElement, elementoJson: JSON): void {
            var propiedadDto = urlDeArchivo.getAttribute(atControl.propiedad);
            let valor: string = urlDeArchivo.getAttribute(atArchivo.nombre);
            let obligatorio: string = urlDeArchivo.getAttribute(atControl.obligatorio);

            if (obligatorio === "S" && IsNullOrEmpty(valor)) {
                urlDeArchivo.classList.remove(ClaseCss.crtlValido);
                urlDeArchivo.classList.add(ClaseCss.crtlNoValido);
                throw new Error(`El campo ${propiedadDto} es obligatorio`);
            }

            urlDeArchivo.classList.remove(ClaseCss.crtlNoValido);
            urlDeArchivo.classList.add(ClaseCss.crtlValido);
            elementoJson[propiedadDto] = valor;
        }

        protected DespuesDeMapearDatosDeIU(panel: HTMLDivElement, elementoJson: JSON, modoDeTrabajo: string): JSON {
            return elementoJson;
        }


        // funciones de carga de elementos para los selectores   ************************************************************************************

        protected CargarListaDinamica(input: HTMLInputElement, controlador: string) {
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
            let url: string = this.DefinirPeticionDeCargarDinamica(controlador, clase, Numero(cantidad), filtro);
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
                    if (expresionPorDefecto.toLowerCase() !== mostrarExpresion.toLowerCase())
                        expresion = ParsearExpresion(peticion.resultado.datos[i], mostrarExpresion.toLowerCase());
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
                Mensaje(TipoMensaje.Error, peticion.resultado.mensaje);
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