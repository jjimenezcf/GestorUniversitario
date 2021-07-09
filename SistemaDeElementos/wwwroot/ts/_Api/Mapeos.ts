namespace MapearAlJson {

    export function ListaDinamicas(panel: HTMLDivElement, elementoJson: JSON): void {
        let lista: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.ListaDinamica}"]`) as NodeListOf<HTMLInputElement>;
        for (let i = 0; i < lista.length; i++) {
            ListaDinamica(lista[i], elementoJson);
        }
    }

    function ListaDinamica(input: HTMLInputElement, elementoJson: JSON): void {
        let propiedadDto = input.getAttribute(atControl.propiedad);
        let guardarEn: string = input.getAttribute(atListasDinamicasDto.guardarEn);
        let obligatorio: string = input.getAttribute(atControl.obligatorio);
        let valor: number = Numero(input.getAttribute(atListasDinamicas.idSeleccionado));

        if (obligatorio === "S" && Number(valor) === 0) {
            input.classList.remove(ClaseCss.crtlValido);
            input.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`Debe seleccionar un elemento de la lista ${propiedadDto}`);
        }

        input.classList.remove(ClaseCss.crtlNoValido);
        input.classList.add(ClaseCss.crtlValido);
        elementoJson[guardarEn] = valor === 0 ? '' : valor.toString();
    }

    export function ListasDeElementos(panel: HTMLDivElement, elementoJson: JSON): void {
        let selectores: NodeListOf<HTMLSelectElement> = panel.querySelectorAll(`select[tipo="${TipoControl.ListaDeElementos}"]`) as NodeListOf<HTMLSelectElement>;
        for (let i = 0; i < selectores.length; i++) {
            ListaDeElemento(selectores[i], elementoJson);
        }
    }

    function ListaDeElemento(selector: HTMLSelectElement, elementoJson: JSON) {
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

    export function Restrictores(panel: HTMLDivElement, elementoJson: JSON): void {
        let restrictores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.restrictorDeEdicion}"]`) as NodeListOf<HTMLInputElement>;
        for (let i = 0; i < restrictores.length; i++) {
            Restrictor(restrictores[i], elementoJson);
        }
    }

    function Restrictor(input: HTMLInputElement, elementoJson: JSON): void {
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

    export function Fechas(panel: HTMLDivElement, elementoJson: JSON): void {
        let fechas: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.SelectorDeFecha}"]`) as NodeListOf<HTMLInputElement>;
        for (var i = 0; i < fechas.length; i++) {
            let fecha: HTMLInputElement = fechas[i] as HTMLInputElement;
            Fecha(fecha, elementoJson);
        }

        let fechasHoras: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.SelectorDeFechaHora}"]`) as NodeListOf<HTMLInputElement>;
        for (var i = 0; i < fechasHoras.length; i++) {
            let fecha: HTMLInputElement = fechasHoras[i] as HTMLInputElement;
            Fecha(fecha, elementoJson);
        }
    }

    function Fecha(controlDeFecha: HTMLInputElement, elementoJson: JSON): void {
        let propiedadDto: string = controlDeFecha.getAttribute(atControl.propiedad);
        let obligatorio: string = controlDeFecha.getAttribute(atControl.obligatorio);
        let valorDeFecha: string = controlDeFecha.value; //.replace(/\n/g, "\r\n");
        let fechaHoraFijada = false;
        if (obligatorio === "S" && NoDefinida(valorDeFecha)) {
            if (controlDeFecha.readOnly) {
                valorDeFecha = new Date(Date.now()).toISOString();
                fechaHoraFijada = true;
            }
            else {
                controlDeFecha.classList.remove(ClaseCss.crtlValido);
                controlDeFecha.classList.add(ClaseCss.crtlNoValido);
                throw new Error(`El campo: ${propiedadDto}, es obligatorio`);
            }
        }

        let fecha: Date = new Date(valorDeFecha);
        if (FechaValida(fecha)) {
            let idHora = controlDeFecha.getAttribute(atSelectorDeFecha.hora);
            if (!IsNullOrEmpty(idHora)) {
                let controlDeHora: HTMLInputElement = document.getElementById(idHora) as HTMLInputElement;

                if (!fechaHoraFijada) {
                    let valorDeHora = controlDeHora.value.split(':');
                    let hora: number = Numero(valorDeHora[0]);
                    let minuto: number = Numero(valorDeHora[1]);
                    let segundos: number = Numero(valorDeHora[2]);
                    let milisegundos: number = Numero(controlDeHora.getAttribute(atSelectorDeFecha.milisegundos));
                    fecha.setHours(hora);
                    fecha.setMinutes(minuto);
                    fecha.setSeconds(segundos);
                    fecha.setMilliseconds(milisegundos);
                }
            }
            var utcFecha = new Date(Date.UTC(fecha.getFullYear(), fecha.getMonth(), fecha.getDate(), fecha.getHours(), fecha.getMinutes(), fecha.getSeconds(), fecha.getMilliseconds()));
            elementoJson[propiedadDto] = utcFecha;
        }
        else
            elementoJson[propiedadDto] = '';
    }

    export function Textos(panel: HTMLDivElement, elementoJson: JSON): void {
        let areas: NodeListOf<HTMLTextAreaElement> = panel.querySelectorAll(`textarea[tipo="${TipoControl.AreaDeTexto}"]`) as NodeListOf<HTMLTextAreaElement>;
        for (let i = 0; i < areas.length; i++) {
            Texto(areas[i], elementoJson);
        }
    }

    function Texto(area: HTMLTextAreaElement, elementoJson: JSON): void {
        let propiedadDto: string = area.getAttribute(atControl.propiedad);
        let obligatorio: string = area.getAttribute(atControl.obligatorio);
        let valor: string = area.value; //.replace(/\n/g, "\r\n");
        if (obligatorio === "S" && NoDefinida(valor)) {
            area.classList.remove(ClaseCss.crtlValido);
            area.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`El campo ${propiedadDto} es obligatorio`);
        }
        elementoJson[propiedadDto] = valor;
    }

    export function Editores(panel: HTMLDivElement, elementoJson: JSON): void {
        let editores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.Editor}"]`) as NodeListOf<HTMLInputElement>;
        for (let i = 0; i < editores.length; i++) {
            Editor(editores[i], elementoJson);
        }
    }

    function Editor(input: HTMLInputElement, elementoJson: JSON): void {
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

    export function Archivos(panel: HTMLDivElement, elementoJson: JSON): void {
        let archivos: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.Archivo}"]`) as NodeListOf<HTMLInputElement>;
        for (let i = 0; i < archivos.length; i++) {
            Archivo(archivos[i], elementoJson);
        }
    }

    function Archivo(archivo: HTMLInputElement, elementoJson: JSON): void {
        var propiedadDto = archivo.getAttribute(atControl.propiedad);
        let valor: string = archivo.getAttribute(atArchivo.idArchivo);
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

    export function Urls(panel: HTMLDivElement, elementoJson: JSON): void {
        let urlsDeArchivos: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.UrlDeArchivo}"]`) as NodeListOf<HTMLInputElement>;
        for (let i = 0; i < urlsDeArchivos.length; i++) {
            Url(urlsDeArchivos[i], elementoJson);
        }
    }

    function Url(urlDeArchivo: HTMLInputElement, elementoJson: JSON): void {
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

    export function Checks(panel: HTMLDivElement, elementoJson: JSON): void {
        let checkes: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[tipo="${TipoControl.Check}"]`) as NodeListOf<HTMLInputElement>;
        for (let i = 0; i < checkes.length; i++) {
            Check(checkes[i], elementoJson);
        }
    }

    function Check(check: HTMLInputElement, elementoJson: JSON): void {
        var propiedadDto = check.getAttribute(atControl.propiedad);
        elementoJson[propiedadDto] = check.checked;
    }

};

namespace MapearPanelDeFiltro {

    export function MapearRestrictores(zonaDeFiltro: HTMLDivElement, propiedad: string, id: number, texto: string) {
        let mapeado: boolean = RestrictoresDeFiltrado(zonaDeFiltro, propiedad, id, texto);
        if (!mapeado) MapearAlControl.Propiedad(zonaDeFiltro, propiedad, id, texto);
    }

    function RestrictoresDeFiltrado(panel: HTMLDivElement, propiedadRestrictora: string, id: number, texto: string): boolean {
        let restrictores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.restrictorDeFiltro}"]`) as NodeListOf<HTMLInputElement>;

        for (let i = 0; i < restrictores.length; i++) {
            if (restrictores[i].getAttribute(atControl.propiedad) === propiedadRestrictora) {
                MapearAlControl.Restrictor(restrictores[i], id, texto);
                return true;
            }
        }
        return false;
    }

}

namespace MapearPanelDeCreacion {

    export function MapearRestrictores(zonaDeCreacion: HTMLDivElement, propiedad: string, id: number, texto: string) {
        let mapeado: boolean = RestrictoresDeCreacion(zonaDeCreacion, propiedad, id, texto);
        if (!mapeado) {
            let lista: HTMLInputElement = ApiControl.BuscarListaDinamicaPorGuardarEn(zonaDeCreacion, propiedad);
            if (Definida(lista))
                MapearAlControl.FijarValorEnListaDinamica(lista, id, texto);
            else
               MapearAlControl.Propiedad(zonaDeCreacion, propiedad, id, texto);
        }
    }

    function RestrictoresDeCreacion(panel: HTMLDivElement, propiedad: string, id: number, texto: string): boolean {
        let restrictores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.restrictorDeEdicion}"]`) as NodeListOf<HTMLInputElement>;

        for (let i = 0; i < restrictores.length; i++) {
            if (restrictores[i].getAttribute(atControl.propiedad) === propiedad) {
                MapearAlControl.Restrictor(restrictores[i], id, texto);
                return true;
            }
        }
        return false;
    }

}

namespace MapearPanelDeEdicion {

    export function MapearRestrictores(zonaDeEdicion: HTMLDivElement, propiedad: string, id: number, texto: string) {
        let mapeado: boolean = RestrictoresDeEdicion(zonaDeEdicion, propiedad, id, texto);
        if (!mapeado) {
            let lista: HTMLInputElement = ApiControl.BuscarListaDinamicaPorGuardarEn(zonaDeEdicion, propiedad);
            if (Definida(lista)) 
                MapearAlControl.FijarValorEnListaDinamica(lista, id, texto);
            else
                MapearAlControl.Propiedad(zonaDeEdicion, propiedad, id, texto);
        }
    }

    function RestrictoresDeEdicion(panel: HTMLDivElement, propiedad: string, id: number, texto: string): boolean {
        let restrictores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.restrictorDeEdicion}"]`) as NodeListOf<HTMLInputElement>;

        for (let i = 0; i < restrictores.length; i++) {
            if (restrictores[i].getAttribute(atControl.propiedad) === propiedad) {
                MapearAlControl.Restrictor(restrictores[i], id, texto);
                return true;
            }
        }
        return false;
    }
}

namespace MapearAlControl {

    export function Url(visor: HTMLImageElement, url: any) {
        visor.setAttribute('src', url);
        let idCanva: string = visor.getAttribute(atControl.id).replace('img', 'canvas');
        let htmlCanvas: HTMLCanvasElement = document.getElementById(idCanva) as HTMLCanvasElement;
        htmlCanvas.width = 100;
        htmlCanvas.height = 100;
        var canvas = htmlCanvas.getContext('2d');
        var img = new Image();
        var err = new Image();
        err.src = "/images/menu/not-found.svg";
        img.src = url;
        img.onload = function () {
            canvas.drawImage(img, 0, 0, 100, 100);
        };
        img.onerror = cargarImagenPorDefecto;
    }

    function cargarImagenPorDefecto(e) {
        e.target.src = '/images/menu/not-found.svg';
    }

    export function Fecha(control: HTMLInputElement, fecha: string) {
        var fechaLeida = new Date(fecha);
        if (FechaValida(fechaLeida)) {
            FechaDate(control, fechaLeida);
        }
        else {
            var propiedad: string = control.getAttribute(atControl.propiedad);
            MensajesSe.Error("MapearFechaAlControl", `Fecha leida para la propiedad ${propiedad} es no válida, valor ${fecha}`);
        }
    }

    export function FechaDate(control: HTMLInputElement, fecha: Date) {
        var fechaLeida = new Date(fecha);
        if (FechaValida(fechaLeida)) {
            let dia: number = fechaLeida.getDate();
            let mes: number = fechaLeida.getMonth() + 1;
            let ano: number = fechaLeida.getFullYear();
            control.value = `${ano}-${PadLeft(mes.toString(), "00")}-${PadLeft(dia.toString(), "00")}`;
        }
        else {
            var propiedad: string = control.getAttribute(atControl.propiedad);
            MensajesSe.Error("MapearFechaAlControl", `Fecha leida para la propiedad ${propiedad} es no válida, valor ${fecha}`);
        }
    }

    export function Texto(area: HTMLTextAreaElement, texto: string): void {
        area.textContent = texto;
    }



    export function RestrictoresDeEdicion(panel: HTMLDivElement, propiedad: string, id: number, texto: string) {
        let restrictores: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.restrictorDeEdicion}"]`) as NodeListOf<HTMLInputElement>;

        for (let i = 0; i < restrictores.length; i++) {
            if (restrictores[i].getAttribute(atControl.propiedad) === propiedad) {
                Restrictor(restrictores[i], id, texto);
            }
        }
    }

    export function Propiedad(panel: HTMLDivElement, propiedad: string, id: number, texto: string) {
        let controles: NodeListOf<HTMLInputElement> = panel.querySelectorAll(`input[${atControl.propiedad}="${propiedad}"]`) as NodeListOf<HTMLInputElement>;

        if (controles.length > 1)
            MensajesSe.EmitirExcepcion("Mapeo de propiedad", `Hay más de un control con la propiedad ${propiedad} en el panel ${panel.id}`);

        if (controles.length === 0)
            MensajesSe.EmitirExcepcion("Mapeo de propiedad", `No existe la propiedad ${propiedad} en el panel ${panel.id}`);

        let tipo: string = controles[0].getAttribute(atControl.tipo);

        if (tipo === TipoControl.ListaDinamica)
            FijarValorEnListaDinamica(controles[0], id, texto);

        if (tipo === TipoControl.Editor)
            FijarValorEnEditor(controles[0], id, texto);

    }


    export function Restrictor(restrictor: HTMLInputElement, id: number, texto: string): void {
        restrictor.setAttribute(atControl.valorInput, texto);
        restrictor.setAttribute(atControl.restrictor, id.toString());
        ApiControl.BlanquearDependientes(restrictor);
    }

    export function Lista(lista: HTMLSelectElement, id: number): void {
        if (!NumeroMayorDeCero(id))
            lista.selectedIndex = 0;
        else
            for (var j = 0; j < lista.options.length; j++) {
                if (Numero(lista.options[j].value) == id) {
                    lista.selectedIndex = j;
                    break;
                }
            }
    }

    export function Hora(control: HTMLInputElement, fechaHora: string) {
        var fechaLeida = new Date(fechaHora);
        if (FechaValida(fechaLeida)) {
            HoraDate(control, fechaLeida);
        }
        else {
            var propiedad: string = control.getAttribute(atControl.propiedad);
            MensajesSe.Error("MapearHoraAlControl", `Fecha leida para la propiedad ${propiedad} es no válida, valor ${fechaHora}`);
        }
    }

    export function HoraDate(control: HTMLInputElement, fechaLeida: Date): boolean {
        let hora: number = fechaLeida.getHours();
        let minuto: number = fechaLeida.getMinutes();
        let segundos: number = fechaLeida.getSeconds();
        let milisegundos: number = fechaLeida.getMilliseconds();
        let idHora: string = control.getAttribute(atSelectorDeFecha.hora);
        if (!IsNullOrEmpty(idHora)) {
            let controlHora: HTMLInputElement = document.getElementById(idHora) as HTMLInputElement;
            controlHora.value = `${PadLeft(hora.toString(), "00")}:${PadLeft(minuto.toString(), "00")}:${PadLeft(segundos.toString(), "00")}`;
            controlHora.setAttribute(atSelectorDeFecha.milisegundos, milisegundos.toString());
            return true;
        }
        return false;
    }

    export function ProponerValorEnListaDinamica(input: HTMLInputElement, id: number, texto: string) {
        if (Numero(id) > 0) {
            let listaDinamica = new Tipos.ListaDinamica(input);
            listaDinamica.AgregarOpcion(id, texto);
        }
        ListaDinamica(input, id, texto);
    }

    export function FijarValorEnListaDinamica(input: HTMLInputElement, id: number, texto: string) {
        ProponerValorEnListaDinamica(input, id, texto);
        ApiControl.BloquearListaDinamica(input, true);
    }


    export function ListaDinamica(input: HTMLInputElement, valor: number, texto: string) {
        try {
            input.setAttribute(atListasDinamicas.idSeleccionado, Numero(valor).toString());
            input.value = Numero(valor) === 0 ? "" : texto;
            ApiControl.BlanquearDependientes(input);
        }
        finally {
            input.setAttribute(atListasDinamicas.cargando, 'N');
        }
    }

    export function FijarValorEnEditor(input: HTMLInputElement, id: number, texto: string) {
        Restrictor(input, id, texto)
        ApiControl.BloquearEditor(input);
    }


}

