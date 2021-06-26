var MapearAlJson;
(function (MapearAlJson) {
    function ListaDinamicas(panel, elementoJson) {
        let lista = panel.querySelectorAll(`input[tipo="${TipoControl.ListaDinamica}"]`);
        for (let i = 0; i < lista.length; i++) {
            ListaDinamica(lista[i], elementoJson);
        }
    }
    MapearAlJson.ListaDinamicas = ListaDinamicas;
    function ListaDinamica(input, elementoJson) {
        let propiedadDto = input.getAttribute(atControl.propiedad);
        let guardarEn = input.getAttribute(atListasDinamicasDto.guardarEn);
        let obligatorio = input.getAttribute(atControl.obligatorio);
        let valor = Numero(input.getAttribute(atListasDinamicas.idSeleccionado));
        if (obligatorio === "S" && Number(valor) === 0) {
            input.classList.remove(ClaseCss.crtlValido);
            input.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`Debe seleccionar un elemento de la lista ${propiedadDto}`);
        }
        input.classList.remove(ClaseCss.crtlNoValido);
        input.classList.add(ClaseCss.crtlValido);
        elementoJson[guardarEn] = valor === 0 ? '' : valor.toString();
    }
    function ListasDeElementos(panel, elementoJson) {
        let selectores = panel.querySelectorAll(`select[tipo="${TipoControl.ListaDeElementos}"]`);
        for (let i = 0; i < selectores.length; i++) {
            ListaDeElemento(selectores[i], elementoJson);
        }
    }
    MapearAlJson.ListasDeElementos = ListasDeElementos;
    function ListaDeElemento(selector, elementoJson) {
        let propiedadDto = selector.getAttribute(atControl.propiedad);
        let guardarEn = selector.getAttribute(atListasDinamicasDto.guardarEn);
        let obligatorio = selector.getAttribute(atControl.obligatorio);
        if (obligatorio === "S" && Number(selector.value) === 0) {
            selector.classList.remove(ClaseCss.crtlValido);
            selector.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`Debe seleccionar un elemento de la lista ${propiedadDto}`);
        }
        selector.classList.remove(ClaseCss.crtlNoValido);
        selector.classList.add(ClaseCss.crtlValido);
        elementoJson[guardarEn] = selector.value;
    }
    function Restrictores(panel, elementoJson) {
        let restrictores = panel.querySelectorAll(`input[tipo="${TipoControl.restrictorDeEdicion}"]`);
        for (let i = 0; i < restrictores.length; i++) {
            Restrictor(restrictores[i], elementoJson);
        }
    }
    MapearAlJson.Restrictores = Restrictores;
    function Restrictor(input, elementoJson) {
        let propiedadDto = input.getAttribute(atControl.propiedad);
        let idRestrictor = input.getAttribute(atControl.restrictor);
        if (!NumeroMayorDeCero(idRestrictor)) {
            input.classList.remove(ClaseCss.crtlValido);
            input.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`El campo ${propiedadDto} es obligatorio`);
        }
        input.classList.remove(ClaseCss.crtlNoValido);
        input.classList.add(ClaseCss.crtlValido);
        elementoJson[propiedadDto] = idRestrictor;
    }
    function Fechas(panel, elementoJson) {
        let fechas = panel.querySelectorAll(`input[tipo="${TipoControl.SelectorDeFecha}"]`);
        for (var i = 0; i < fechas.length; i++) {
            let fecha = fechas[i];
            Fecha(fecha, elementoJson);
        }
        let fechasHoras = panel.querySelectorAll(`input[tipo="${TipoControl.SelectorDeFechaHora}"]`);
        for (var i = 0; i < fechasHoras.length; i++) {
            let fecha = fechasHoras[i];
            Fecha(fecha, elementoJson);
        }
    }
    MapearAlJson.Fechas = Fechas;
    function Fecha(controlDeFecha, elementoJson) {
        let propiedadDto = controlDeFecha.getAttribute(atControl.propiedad);
        let obligatorio = controlDeFecha.getAttribute(atControl.obligatorio);
        let valorDeFecha = controlDeFecha.value; //.replace(/\n/g, "\r\n");
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
        let fecha = new Date(valorDeFecha);
        if (FechaValida(fecha)) {
            let idHora = controlDeFecha.getAttribute(atSelectorDeFecha.hora);
            if (!IsNullOrEmpty(idHora)) {
                let controlDeHora = document.getElementById(idHora);
                if (!fechaHoraFijada) {
                    let valorDeHora = controlDeHora.value.split(':');
                    let hora = Numero(valorDeHora[0]);
                    let minuto = Numero(valorDeHora[1]);
                    let segundos = Numero(valorDeHora[2]);
                    let milisegundos = Numero(controlDeHora.getAttribute(atSelectorDeFecha.milisegundos));
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
    function Textos(panel, elementoJson) {
        let areas = panel.querySelectorAll(`textarea[tipo="${TipoControl.AreaDeTexto}"]`);
        for (let i = 0; i < areas.length; i++) {
            Texto(areas[i], elementoJson);
        }
    }
    MapearAlJson.Textos = Textos;
    function Texto(area, elementoJson) {
        let propiedadDto = area.getAttribute(atControl.propiedad);
        let obligatorio = area.getAttribute(atControl.obligatorio);
        let valor = area.value; //.replace(/\n/g, "\r\n");
        if (obligatorio === "S" && NoDefinida(valor)) {
            area.classList.remove(ClaseCss.crtlValido);
            area.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`El campo ${propiedadDto} es obligatorio`);
        }
        elementoJson[propiedadDto] = valor;
    }
    function Editores(panel, elementoJson) {
        let editores = panel.querySelectorAll(`input[tipo="${TipoControl.Editor}"]`);
        for (let i = 0; i < editores.length; i++) {
            Editor(editores[i], elementoJson);
        }
    }
    MapearAlJson.Editores = Editores;
    function Editor(input, elementoJson) {
        var propiedadDto = input.getAttribute(atControl.propiedad);
        let valor = input.value;
        let obligatorio = input.getAttribute(atControl.obligatorio);
        if (obligatorio === "S" && NoDefinida(valor)) {
            input.classList.remove(ClaseCss.crtlValido);
            input.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`El campo ${propiedadDto} es obligatorio`);
        }
        input.classList.remove(ClaseCss.crtlNoValido);
        input.classList.add(ClaseCss.crtlValido);
        elementoJson[propiedadDto] = valor;
    }
    function Archivos(panel, elementoJson) {
        let archivos = panel.querySelectorAll(`input[tipo="${TipoControl.Archivo}"]`);
        for (let i = 0; i < archivos.length; i++) {
            Archivo(archivos[i], elementoJson);
        }
    }
    MapearAlJson.Archivos = Archivos;
    function Archivo(archivo, elementoJson) {
        var propiedadDto = archivo.getAttribute(atControl.propiedad);
        let valor = archivo.getAttribute(atArchivo.idArchivo);
        let obligatorio = archivo.getAttribute(atControl.obligatorio);
        if (obligatorio === "S" && IsNullOrEmpty(valor)) {
            archivo.classList.remove(ClaseCss.crtlValido);
            archivo.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`El campo ${propiedadDto} es obligatorio`);
        }
        archivo.classList.remove(ClaseCss.crtlNoValido);
        archivo.classList.add(ClaseCss.crtlValido);
        elementoJson[propiedadDto] = valor;
    }
    function Urls(panel, elementoJson) {
        let urlsDeArchivos = panel.querySelectorAll(`input[tipo="${TipoControl.UrlDeArchivo}"]`);
        for (let i = 0; i < urlsDeArchivos.length; i++) {
            Url(urlsDeArchivos[i], elementoJson);
        }
    }
    MapearAlJson.Urls = Urls;
    function Url(urlDeArchivo, elementoJson) {
        var propiedadDto = urlDeArchivo.getAttribute(atControl.propiedad);
        let valor = urlDeArchivo.getAttribute(atArchivo.nombre);
        let obligatorio = urlDeArchivo.getAttribute(atControl.obligatorio);
        if (obligatorio === "S" && IsNullOrEmpty(valor)) {
            urlDeArchivo.classList.remove(ClaseCss.crtlValido);
            urlDeArchivo.classList.add(ClaseCss.crtlNoValido);
            throw new Error(`El campo ${propiedadDto} es obligatorio`);
        }
        urlDeArchivo.classList.remove(ClaseCss.crtlNoValido);
        urlDeArchivo.classList.add(ClaseCss.crtlValido);
        elementoJson[propiedadDto] = valor;
    }
    function Checks(panel, elementoJson) {
        let checkes = panel.querySelectorAll(`input[tipo="${TipoControl.Check}"]`);
        for (let i = 0; i < checkes.length; i++) {
            Check(checkes[i], elementoJson);
        }
    }
    MapearAlJson.Checks = Checks;
    function Check(check, elementoJson) {
        var propiedadDto = check.getAttribute(atControl.propiedad);
        elementoJson[propiedadDto] = check.checked;
    }
})(MapearAlJson || (MapearAlJson = {}));
;
var MapearPanelDeFiltro;
(function (MapearPanelDeFiltro) {
    function MapearRestrictores(zonaDeFiltro, propiedad, id, texto) {
        let mapeado = RestrictoresDeFiltrado(zonaDeFiltro, propiedad, id, texto);
        if (!mapeado)
            MapearAlControl.Propiedad(zonaDeFiltro, propiedad, id, texto);
    }
    MapearPanelDeFiltro.MapearRestrictores = MapearRestrictores;
    function RestrictoresDeFiltrado(panel, propiedadRestrictora, id, texto) {
        let restrictores = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.restrictorDeFiltro}"]`);
        for (let i = 0; i < restrictores.length; i++) {
            if (restrictores[i].getAttribute(atControl.propiedad) === propiedadRestrictora) {
                MapearAlControl.Restrictor(restrictores[i], id, texto);
                return true;
            }
        }
        return false;
    }
})(MapearPanelDeFiltro || (MapearPanelDeFiltro = {}));
var MapearPanelDeCreacion;
(function (MapearPanelDeCreacion) {
    function MapearRestrictores(zonaDeCreacion, propiedad, id, texto) {
        let mapeado = RestrictoresDeCreacion(zonaDeCreacion, propiedad, id, texto);
        if (!mapeado) {
            let lista = ApiControl.BuscarListaDinamicaPorGuardarEn(zonaDeCreacion, propiedad);
            if (Definida(lista))
                MapearAlControl.FijarValorEnListaDinamica(lista, id, texto);
            else
                MapearAlControl.Propiedad(zonaDeCreacion, propiedad, id, texto);
        }
    }
    MapearPanelDeCreacion.MapearRestrictores = MapearRestrictores;
    function RestrictoresDeCreacion(panel, propiedad, id, texto) {
        let restrictores = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.restrictorDeEdicion}"]`);
        for (let i = 0; i < restrictores.length; i++) {
            if (restrictores[i].getAttribute(atControl.propiedad) === propiedad) {
                MapearAlControl.Restrictor(restrictores[i], id, texto);
                return true;
            }
        }
        return false;
    }
})(MapearPanelDeCreacion || (MapearPanelDeCreacion = {}));
var MapearPanelDeEdicion;
(function (MapearPanelDeEdicion) {
    function MapearRestrictores(zonaDeEdicion, propiedad, id, texto) {
        let mapeado = RestrictoresDeEdicion(zonaDeEdicion, propiedad, id, texto);
        if (!mapeado) {
            let lista = ApiControl.BuscarListaDinamicaPorGuardarEn(zonaDeEdicion, propiedad);
            if (Definida(lista))
                MapearAlControl.FijarValorEnListaDinamica(lista, id, texto);
            else
                MapearAlControl.Propiedad(zonaDeEdicion, propiedad, id, texto);
        }
    }
    MapearPanelDeEdicion.MapearRestrictores = MapearRestrictores;
    function RestrictoresDeEdicion(panel, propiedad, id, texto) {
        let restrictores = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.restrictorDeEdicion}"]`);
        for (let i = 0; i < restrictores.length; i++) {
            if (restrictores[i].getAttribute(atControl.propiedad) === propiedad) {
                MapearAlControl.Restrictor(restrictores[i], id, texto);
                return true;
            }
        }
        return false;
    }
})(MapearPanelDeEdicion || (MapearPanelDeEdicion = {}));
var MapearAlControl;
(function (MapearAlControl) {
    function Url(visor, url) {
        visor.setAttribute('src', url);
        let idCanva = visor.getAttribute(atControl.id).replace('img', 'canvas');
        let htmlCanvas = document.getElementById(idCanva);
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
    MapearAlControl.Url = Url;
    function cargarImagenPorDefecto(e) {
        e.target.src = '/images/menu/not-found.svg';
    }
    function Fecha(control, fecha) {
        var fechaLeida = new Date(fecha);
        if (FechaValida(fechaLeida)) {
            FechaDate(control, fechaLeida);
        }
        else {
            var propiedad = control.getAttribute(atControl.propiedad);
            MensajesSe.Error("MapearFechaAlControl", `Fecha leida para la propiedad ${propiedad} es no válida, valor ${fecha}`);
        }
    }
    MapearAlControl.Fecha = Fecha;
    function FechaDate(control, fecha) {
        var fechaLeida = new Date(fecha);
        if (FechaValida(fechaLeida)) {
            let dia = fechaLeida.getDate();
            let mes = fechaLeida.getMonth() + 1;
            let ano = fechaLeida.getFullYear();
            control.value = `${ano}-${PadLeft(mes.toString(), "00")}-${PadLeft(dia.toString(), "00")}`;
        }
        else {
            var propiedad = control.getAttribute(atControl.propiedad);
            MensajesSe.Error("MapearFechaAlControl", `Fecha leida para la propiedad ${propiedad} es no válida, valor ${fecha}`);
        }
    }
    MapearAlControl.FechaDate = FechaDate;
    function Texto(area, texto) {
        area.textContent = texto;
    }
    MapearAlControl.Texto = Texto;
    function RestrictoresDeEdicion(panel, propiedad, id, texto) {
        let restrictores = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.restrictorDeEdicion}"]`);
        for (let i = 0; i < restrictores.length; i++) {
            if (restrictores[i].getAttribute(atControl.propiedad) === propiedad) {
                Restrictor(restrictores[i], id, texto);
            }
        }
    }
    MapearAlControl.RestrictoresDeEdicion = RestrictoresDeEdicion;
    function Propiedad(panel, propiedad, id, texto) {
        let controles = panel.querySelectorAll(`input[${atControl.propiedad}="${propiedad}"]`);
        if (controles.length > 1)
            MensajesSe.EmitirExcepcion("Mapeo de propiedad", `Hay más de un control con la propiedad ${propiedad} en el panel ${panel.id}`);
        if (controles.length === 0)
            MensajesSe.EmitirExcepcion("Mapeo de propiedad", `No existe la propiedad ${propiedad} en el panel ${panel.id}`);
        let tipo = controles[0].getAttribute(atControl.tipo);
        if (tipo === TipoControl.ListaDinamica)
            FijarValorEnListaDinamica(controles[0], id, texto);
        if (tipo === TipoControl.Editor)
            FijarValorEnEditor(controles[0], id, texto);
    }
    MapearAlControl.Propiedad = Propiedad;
    function Restrictor(restrictor, id, texto) {
        restrictor.setAttribute(atControl.valorInput, texto);
        restrictor.setAttribute(atControl.restrictor, id.toString());
    }
    MapearAlControl.Restrictor = Restrictor;
    function Lista(lista, id) {
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
    MapearAlControl.Lista = Lista;
    function Hora(control, fechaHora) {
        var fechaLeida = new Date(fechaHora);
        if (FechaValida(fechaLeida)) {
            HoraDate(control, fechaLeida);
        }
        else {
            var propiedad = control.getAttribute(atControl.propiedad);
            MensajesSe.Error("MapearHoraAlControl", `Fecha leida para la propiedad ${propiedad} es no válida, valor ${fechaHora}`);
        }
    }
    MapearAlControl.Hora = Hora;
    function HoraDate(control, fechaLeida) {
        let hora = fechaLeida.getHours();
        let minuto = fechaLeida.getMinutes();
        let segundos = fechaLeida.getSeconds();
        let milisegundos = fechaLeida.getMilliseconds();
        let idHora = control.getAttribute(atSelectorDeFecha.hora);
        if (!IsNullOrEmpty(idHora)) {
            let controlHora = document.getElementById(idHora);
            controlHora.value = `${PadLeft(hora.toString(), "00")}:${PadLeft(minuto.toString(), "00")}:${PadLeft(segundos.toString(), "00")}`;
            controlHora.setAttribute(atSelectorDeFecha.milisegundos, milisegundos.toString());
            return true;
        }
        return false;
    }
    MapearAlControl.HoraDate = HoraDate;
    function ProponerValorEnListaDinamica(input, id, texto) {
        if (Numero(id) > 0) {
            let listaDinamica = new Tipos.ListaDinamica(input);
            listaDinamica.AgregarOpcion(id, texto);
        }
        ListaDinamica(input, id, texto);
    }
    MapearAlControl.ProponerValorEnListaDinamica = ProponerValorEnListaDinamica;
    function FijarValorEnListaDinamica(input, id, texto) {
        ProponerValorEnListaDinamica(input, id, texto);
        ApiControl.BloquearListaDinamica(input, true);
    }
    MapearAlControl.FijarValorEnListaDinamica = FijarValorEnListaDinamica;
    function ListaDinamica(input, valor, texto) {
        input.setAttribute(atListasDinamicas.idSeleccionado, Numero(valor).toString());
        input.value = Numero(valor) === 0 ? "" : texto;
    }
    MapearAlControl.ListaDinamica = ListaDinamica;
    function FijarValorEnEditor(input, id, texto) {
        Restrictor(input, id, texto);
        ApiControl.BloquearEditor(input);
    }
    MapearAlControl.FijarValorEnEditor = FijarValorEnEditor;
})(MapearAlControl || (MapearAlControl = {}));
//# sourceMappingURL=Mapeos.js.map