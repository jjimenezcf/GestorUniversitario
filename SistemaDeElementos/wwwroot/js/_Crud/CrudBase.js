var Crud;
(function (Crud) {
    class HTMLSelector extends HTMLInputElement {
    }
    Crud.HTMLSelector = HTMLSelector;
    class CrudBase {
        constructor() {
            this._estado = undefined;
        }
        get Pagina() {
            return this.Estado.Obtener(Sesion.paginaActual);
        }
        get Estado() {
            if (this._estado === undefined) {
                throw new Error("Debe definir la variable estado");
            }
            return this._estado;
        }
        set Estado(valor) {
            this._estado = valor;
        }
        get Controlador() {
            return this._controlador;
        }
        Inicializar(pagina) {
            if (EntornoSe.Historial.HayHistorial(pagina))
                this._estado = EntornoSe.Historial.ObtenerEstadoDePagina(pagina);
            else
                this._estado = new HistorialSe.EstadoPagina(pagina);
        }
        //funciones de ayuda para la herencia
        InicializarListasDeElementos(panel, controlador) {
            let listas = panel.querySelectorAll(`select[${atControl.tipo}="${TipoControl.ListaDeElementos}"]`);
            for (let i = 0; i < listas.length; i++) {
                if (listas[i].getAttribute(atListas.yaCargado) === "S")
                    continue;
                let claseElemento = listas[i].getAttribute(atListas.claseElemento);
                this.CargarListaDeElementos(controlador, claseElemento, listas[i].getAttribute(atControl.id));
            }
        }
        InicializarListasDinamicas(panel) {
            let listas = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.ListaDinamica}"]`);
            for (let i = 0; i < listas.length; i++) {
                let lista = new Tipos.ListaDinamica(listas[i]);
                lista.Borrar();
            }
        }
        InicializarSelectoresDeFecha(panel) {
            let selectoresDeFecha = panel.querySelectorAll(`select[${atControl.tipo}="${TipoControl.SelectorDeFecha}"]`);
            for (let i = 0; i < selectoresDeFecha.length; i++) {
                this.InicializarFecha(selectoresDeFecha[i]);
            }
        }
        InicializarFecha(fecha) {
            let hora = fecha.getAttribute(atSelectorDeFecha.hora);
            if (!IsNullOrEmpty(hora)) {
            }
        }
        InicializarArchivos(panel) {
            let archivos = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.Archivo}"]`);
            archivos.forEach((archivo) => { ApiDeArchivos.BlanquearArchivo(archivo, true); });
        }
        AntesDeNavegar(valores) {
        }
        SiHayErrorTrasPeticionAjax(peticion) {
            MensajesSe.Error("SiHayErrorTrasPeticionAjax", peticion.resultado.mensaje);
        }
        // funciones para mapear un elemento Json a los controles de un panel
        MapearElementoLeido(panel, elementoJson, modoDeAcceso) {
            this.MapearPropiedadesDelElemento(panel, "elementoJson", elementoJson, modoDeAcceso);
            this.MapearRestrictoresDelElemento(panel, elementoJson, modoDeAcceso);
            this.MaperaPropiedadesDeListasDeElementos(panel, elementoJson, modoDeAcceso);
            this.MaperaOpcionesListasDinamicas(panel, elementoJson, modoDeAcceso);
            this.MapearSelectoresDeArchivo(panel, elementoJson, modoDeAcceso);
            this.MapearAreasDeTexto(panel, elementoJson, modoDeAcceso);
            this.MapearFechas(panel, elementoJson, modoDeAcceso);
        }
        MapearRestrictoresDelElemento(panel, elementoJson, modoDeAcceso) {
            let restrictores = panel.querySelectorAll(`input[tipo="${TipoControl.restrictorDeEdicion}"]`);
            for (var i = 0; i < restrictores.length; i++) {
                let restrictor = restrictores[i];
                this.MapearJsonAlRestrictor(restrictor, elementoJson);
            }
        }
        MapearJsonAlRestrictor(restrictor, elementoJson) {
            let propiedad = restrictor.getAttribute(atControl.propiedad);
            let mostrar = restrictor.getAttribute(atRestrictor.mostrarExpresion);
            if (!IsNullOrEmpty(propiedad)) {
                let idRestrictor = this.BuscarValorEnJson(propiedad, elementoJson);
                let texto = this.BuscarValorEnJson(mostrar, elementoJson);
                if (!IsNullOrEmpty(texto)) {
                    ApiControl.MapearRestrictorAlControl(restrictor, idRestrictor, texto);
                }
            }
        }
        MaperaPropiedadesDeListasDeElementos(panel, elementoJson, modoDeAcceso) {
            let select = panel.getElementsByTagName('select');
            for (var i = 0; i < select.length; i++) {
                let selector = select[i];
                let guardarEn = selector.getAttribute(atListasDinamicasDto.guardarEn);
                let id = this.BuscarValorEnJson(guardarEn, elementoJson);
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
                if (!ModoAcceso.HayPermisos(ModoAcceso.ModoDeAccesoDeDatos.Gestor, modoDeAcceso)) {
                    selector.disabled = true;
                    selector.classList.add(ClaseCss.soloLectura);
                }
            }
        }
        MaperaOpcionesListasDinamicas(panel, elementoJson, modoDeAcceso) {
            let listas = panel.querySelectorAll(`input[${atControl.tipo}="${TipoControl.ListaDinamica}"]`);
            for (var i = 0; i < listas.length; i++) {
                let input = listas[i];
                let propiedad = input.getAttribute(atControl.propiedad);
                let guardarEn = input.getAttribute(atListasDinamicasDto.guardarEn);
                let id = this.BuscarValorEnJson(guardarEn, elementoJson);
                let valor = this.BuscarValorEnJson(propiedad, elementoJson);
                if (Numero(id) > 0) {
                    let listaDinamica = new Tipos.ListaDinamica(input);
                    listaDinamica.AgregarOpcion(id, valor);
                    input.value = valor;
                }
                ApiControl.AlmacenarValorDeListaDinamica(input, id);
                input.classList.remove(ClaseCss.soloLectura);
                if (!ModoAcceso.HayPermisos(ModoAcceso.ModoDeAccesoDeDatos.Gestor, modoDeAcceso)) {
                    input.disabled = true;
                    input.classList.add(ClaseCss.soloLectura);
                }
            }
        }
        MapearPropiedadesDelElemento(panel, propiedad, valorPropiedadJson, modoDeAcceso) {
            if (valorPropiedadJson === undefined || valorPropiedadJson === null) {
                this.MapearPropiedad(panel, propiedad, "", modoDeAcceso);
                return;
            }
            var tipoDeObjeto = typeof valorPropiedadJson;
            if (tipoDeObjeto === "object") {
                for (var propiedad in valorPropiedadJson) {
                    this.MapearPropiedadesDelElemento(panel, propiedad.toLowerCase(), valorPropiedadJson[propiedad], modoDeAcceso);
                }
            }
            else {
                this.MapearPropiedad(panel, propiedad, valorPropiedadJson, modoDeAcceso);
            }
        }
        BuscarValorEnJson(propiedad, valorPropiedadJson) {
            var tipoDeObjeto = typeof valorPropiedadJson;
            if (tipoDeObjeto === "object") {
                for (var p in valorPropiedadJson) {
                    if (propiedad.toLowerCase() === p.toLowerCase())
                        return valorPropiedadJson[p];
                }
            }
            return null;
        }
        MapearPropiedad(panel, propiedad, valor, modoDeAcceso) {
            if (this.MapearPropiedaAlEditor(panel, propiedad, valor, modoDeAcceso))
                return;
            //if (this.MapearPropiedadAlSelectorDeArchivo(panel, propiedad, valor))
            //    return;
            if (this.MapearPropiedadAlSelectorDeUrlDelArchivo(panel, propiedad, valor, modoDeAcceso))
                return;
            if (this.MapearPropiedadAlCheck(panel, propiedad, valor, modoDeAcceso))
                return;
        }
        MapearPropiedaAlEditor(panel, propiedad, valor, modoDeAcceso) {
            let editor = this.BuscarEditor(panel, propiedad);
            if (editor === null)
                return false;
            editor.classList.remove(ClaseCss.crtlNoValido);
            editor.classList.remove(ClaseCss.soloLectura);
            if (!ModoAcceso.HayPermisos(ModoAcceso.ModoDeAccesoDeDatos.Gestor, modoDeAcceso)) {
                editor.readOnly = true;
                editor.classList.add(ClaseCss.soloLectura);
            }
            else
                editor.classList.add(ClaseCss.crtlValido);
            editor.value = valor;
            return true;
        }
        MapearPropiedadAlCheck(panel, propiedad, valor, modoDeAcceso) {
            let check = this.BuscarCheck(panel, propiedad);
            if (check === null)
                return false;
            check.classList.remove(ClaseCss.crtlNoValido);
            check.classList.add(ClaseCss.crtlValido);
            if (IsBool(valor))
                check.checked = valor === true;
            else if (IsString(valor))
                check.checked = valor.toLowerCase() === 'true';
            if (!ModoAcceso.HayPermisos(ModoAcceso.ModoDeAccesoDeDatos.Gestor, modoDeAcceso)) {
                check.disabled = true;
            }
            return true;
        }
        MapearSelectoresDeArchivo(panel, elementoJson, modoDeAcceso) {
            let selectores = panel.querySelectorAll(`input[tipo="${TipoControl.Archivo}"]`);
            for (var i = 0; i < selectores.length; i++) {
                let selector = selectores[i];
                let propiedad = selector.getAttribute(atControl.propiedad);
                let valor = this.BuscarValorEnJson(propiedad, elementoJson);
                if (valor !== null) {
                    let visorVinculado = selector.getAttribute(atArchivo.imagen);
                    selector.setAttribute(atArchivo.idArchivo, valor.toString());
                    this.MapearImagenes(elementoJson, visorVinculado);
                }
            }
        }
        MapearAreasDeTexto(panel, elementoJson, modoDeAcceso) {
            let areas = panel.querySelectorAll(`textarea[tipo="${TipoControl.AreaDeTexto}"]`);
            for (var i = 0; i < areas.length; i++) {
                let area = areas[i];
                this.MapearAreaDeTexto(area, elementoJson, modoDeAcceso);
            }
        }
        MapearFechas(panel, elementoJson, modoDeAcceso) {
            let fechas = panel.querySelectorAll(`input[tipo="${TipoControl.SelectorDeFecha}"]`);
            for (var i = 0; i < fechas.length; i++) {
                let fecha = fechas[i];
                this.MapearSelectorDeFecha(fecha, elementoJson, modoDeAcceso);
            }
            let fechasHoras = panel.querySelectorAll(`input[tipo="${TipoControl.SelectorDeFechaHora}"]`);
            for (var i = 0; i < fechasHoras.length; i++) {
                let fecha = fechasHoras[i];
                this.MapearSelectorDeFecha(fecha, elementoJson, modoDeAcceso);
            }
        }
        MapearAreaDeTexto(area, elementoJson, modoDeAcceso) {
            let propiedad = area.getAttribute(atControl.propiedad);
            if (!IsNullOrEmpty(propiedad)) {
                let texto = this.BuscarValorEnJson(propiedad, elementoJson);
                if (!IsNullOrEmpty(texto)) {
                    ApiControl.MapearTextoAlControl(area, texto);
                    if (!ModoAcceso.HayPermisos(ModoAcceso.ModoDeAccesoDeDatos.Gestor, modoDeAcceso)) {
                        area.classList.add(ClaseCss.soloLectura);
                        area.readOnly = true;
                    }
                }
            }
        }
        MapearSelectorDeFecha(fecha, elementoJson, modoDeAcceso) {
            let propiedad = fecha.getAttribute(atControl.propiedad);
            if (!IsNullOrEmpty(propiedad)) {
                let valor = this.BuscarValorEnJson(propiedad, elementoJson);
                if (!IsNullOrEmpty(valor)) {
                    ApiControl.MapearFechaAlControl(fecha, valor);
                    let tipo = fecha.getAttribute(atControl.tipo);
                    if (tipo === TipoControl.SelectorDeFechaHora) {
                        ApiControl.MapearHoraAlControl(fecha, valor, modoDeAcceso);
                    }
                    if (!fecha.readOnly && !ModoAcceso.HayPermisos(ModoAcceso.ModoDeAccesoDeDatos.Gestor, modoDeAcceso)) {
                        fecha.classList.add(ClaseCss.soloLectura);
                        fecha.readOnly = true;
                    }
                }
                else
                    ApiControl.BlanquearFecha(fecha);
            }
        }
        MapearImagenes(elementoJson, visorVinculado) {
            let visor = document.getElementById(visorVinculado);
            let propiedadDelVisor = visor.getAttribute(atControl.propiedad);
            let url = this.BuscarValorEnJson(propiedadDelVisor, elementoJson);
            this.MostrarImagenUrl(visor, url);
        }
        MapearPropiedadAlSelectorDeUrlDelArchivo(panel, propiedad, valor, modoDeAcceso) {
            let selector = this.BuscarUrlDelArchivo(panel, propiedad);
            if (selector === null)
                return false;
            let ruta = selector.getAttribute(atArchivo.rutaDestino);
            if (!ModoAcceso.HayPermisos(ModoAcceso.ModoDeAccesoDeDatos.Gestor, modoDeAcceso)) {
                let ref = document.getElementById(`${selector.id}.ref`);
                ref.style.visibility = "hidden";
            }
            selector.classList.remove(ClaseCss.crtlNoValido);
            selector.classList.add(ClaseCss.crtlValido);
            selector.setAttribute(atArchivo.nombre, valor);
            this.MapearPropiedadAlVisorDeImagen(panel, propiedad, `${ruta}/${valor}`);
            return true;
        }
        MapearPropiedadAlVisorDeImagen(panel, propiedad, valor) {
            let visor = this.BuscarVisorDeImagen(panel, propiedad);
            if (visor === null)
                return;
            this.MostrarImagenUrl(visor, valor);
        }
        MostrarImagenUrl(visor, url) {
            visor.setAttribute('src', url);
            let idCanva = visor.getAttribute(atControl.id).replace('img', 'canvas');
            let htmlCanvas = document.getElementById(idCanva);
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
        BuscarEditor(controlPadre, propiedadDto) {
            let editores = controlPadre.querySelectorAll(`input[${atControl.tipo}='${TipoControl.Editor}']`);
            for (var i = 0; i < editores.length; i++) {
                var control = editores[i];
                var dto = control.getAttribute(atControl.propiedad);
                if (dto === propiedadDto.toLowerCase())
                    return control;
            }
            return null;
        }
        BuscarCheck(controlPadre, propiedadDto) {
            let checkes = controlPadre.querySelectorAll(`input[${atControl.tipo}='${TipoControl.Check}']`);
            for (var i = 0; i < checkes.length; i++) {
                var control = checkes[i];
                var dto = control.getAttribute(atControl.propiedad);
                if (dto === propiedadDto.toLowerCase())
                    return control;
            }
            return null;
        }
        BuscarSelectorDeArchivo(controlPadre, propiedadDto) {
            let selectores = controlPadre.querySelectorAll(`input[tipo="${TipoControl.Archivo}"]`);
            for (var i = 0; i < selectores.length; i++) {
                var control = selectores[i];
                var dto = control.getAttribute(atControl.propiedad);
                if (dto === propiedadDto.toLowerCase())
                    return control;
            }
            return null;
        }
        BuscarVisorDeImagen(controlPadre, propiedadDto) {
            let visor = controlPadre.querySelectorAll(`img[${atControl.tipo}='${TipoControl.VisorDeArchivo}']`);
            for (var i = 0; i < visor.length; i++) {
                var control = visor[i];
                var dto = control.getAttribute(atControl.propiedad);
                if (dto === propiedadDto.toLowerCase())
                    return control;
            }
            return null;
        }
        BuscarUrlDelArchivo(controlPadre, propiedadDto) {
            let selectores = controlPadre.querySelectorAll(`input[tipo="${TipoControl.UrlDeArchivo}"]`);
            for (var i = 0; i < selectores.length; i++) {
                var control = selectores[i];
                var dto = control.getAttribute(atControl.propiedad);
                if (dto === propiedadDto.toLowerCase())
                    return control;
            }
            return null;
        }
        BuscarSelect(controlPadre, propiedadDto) {
            let select = controlPadre.getElementsByTagName('select');
            for (var i = 0; i < select.length; i++) {
                var control = select[i];
                var dto = control.getAttribute(atControl.propiedad);
                if (dto === propiedadDto)
                    return control;
            }
            return null;
        }
        BuscarListaDinamica(controlPadre, propiedadDto) {
            let inputs = controlPadre.querySelectorAll(`input[${atControl.tipo}="${TipoControl.ListaDinamica}"]`);
            for (var i = 0; i < inputs.length; i++) {
                var control = inputs[i];
                var dto = control.getAttribute(atControl.propiedad);
                if (dto === propiedadDto)
                    return control;
            }
            return null;
        }
        AntesDeMapearDatosDeIU(crud, panel, modoDeTrabajo) {
            if (modoDeTrabajo === ModoTrabajo.creando)
                return JSON.parse(`{"${literal.id}":"0"}`);
            if (modoDeTrabajo === ModoTrabajo.editando) {
                let input = crud.BuscarEditor(panel, literal.id);
                if (Number(input.value) <= 0)
                    throw new Error(`El valor del id ${Number(input.value)} debe ser mayor a 0`);
                return JSON.parse(`{"${literal.id}":"${Number(input.value)}"}`);
            }
            throw new Error(`No se ha indicado que hacer para el modo de trabajo ${modoDeTrabajo} antes de mapear los datos de la IU`);
        }
        DespuesDeMapearDatosDeIU(crud, panel, elementoJson, modoDeTrabajo) {
            return elementoJson;
        }
        SeleccionarListaDinamica(input) {
            let lista = new Tipos.ListaDinamica(input);
            let valor = lista.BuscarSeleccionado(input.value);
            ApiControl.AlmacenarValorDeListaDinamica(input, valor);
        }
        // funciones de carga de elementos para los selectores   ************************************************************************************
        CargarListaDinamica(input) {
            if (input.getAttribute(atListasDinamicas.cargando) == 'S' || IsNullOrEmpty(input.value)) {
                return;
            }
            let ultimaBuscada = input.getAttribute(atListasDinamicas.ultimaCadenaBuscada);
            let criterio = input.getAttribute(atListasDinamicas.criterio);
            if (!IsNullOrEmpty(ultimaBuscada)) {
                if (criterio === atCriterio.contiene && ultimaBuscada.includes(input.value))
                    return;
                if (criterio === atCriterio.comienza && ultimaBuscada.startsWith(input.value))
                    return;
            }
            let clase = input.getAttribute(atListasDinamicas.claseElemento);
            let idInput = input.getAttribute('id');
            let filtro = ApiFiltro.DefinirFiltroListaDinamica(input, criterio);
            if (filtro === null)
                return;
            let cantidad = input.getAttribute(atListasDinamicas.cantidad);
            let url = this.DefinirPeticionDeCargarDinamica(this.Controlador, clase, Numero(cantidad), filtro);
            let datosDeEntrada = `{"ClaseDeElemento":"${clase}", "IdInput":"${idInput}", "buscada":"${filtro.valor}" , "criterio":"${filtro.criterio}"}`;
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.EndPoint.CargaDinamica, datosDeEntrada, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, this.AnadirOpcionesListaDinamica, this.SiHayErrorAlCargarListasDinamicas);
            input.setAttribute(atListasDinamicas.cargando, 'S');
            a.Ejecutar();
        }
        AnadirOpcionesListaDinamica(peticion) {
            let datosDeEntrada = JSON.parse(peticion.DatosDeEntrada);
            let input = document.getElementById(datosDeEntrada.IdInput);
            try {
                let listaDinamica = new Tipos.ListaDinamica(input);
                let expresionPorDefecto = atListasDinamicas.expresionPorDefecto;
                let mostrarExpresion = input.getAttribute(atListasDinamicas.mostrarExpresion);
                let expresion = "";
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
        SiHayErrorAlCargarListasDinamicas(peticion) {
            let datosDeEntrada = JSON.parse(peticion.DatosDeEntrada);
            let input = document.getElementById(datosDeEntrada.IdInput);
            try {
                MensajesSe.Error("SiHayErrorAlCargarListasDinamicas", peticion.resultado.mensaje);
            }
            finally {
                input.setAttribute(atListasDinamicas.ultimaCadenaBuscada, '');
                input.setAttribute(atListasDinamicas.cargando, 'N');
            }
        }
        CargarListaDeElementos(controlador, claseDeElementoDto, idLista) {
            let url = this.DefinirPeticionDeCargarElementos(controlador, claseDeElementoDto);
            let datosDeEntrada = `{"ClaseDeElemento":"${claseDeElementoDto}", "IdLista":"${idLista}"}`;
            let a = new ApiDeAjax.DescriptorAjax(this, Ajax.EndPoint.CargarLista, datosDeEntrada, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, this.MapearElementosEnLista, this.SiHayErrorTrasPeticionAjax);
            a.Ejecutar();
        }
        MapearElementosEnLista(peticion) {
            let datosDeEntrada = JSON.parse(peticion.DatosDeEntrada);
            let idLista = datosDeEntrada.IdLista;
            let lista = new Tipos.ListaDeElemento(idLista);
            let input = document.getElementById(idLista);
            let expresion = "";
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
        DefinirPeticionDeCargarElementos(controlador, claseElemento) {
            let url = `/${controlador}/${Ajax.EndPoint.CargarLista}?${Ajax.Param.claseElemento}=${claseElemento}`;
            return url;
        }
        DefinirPeticionDeCargarDinamica(controlador, claseElemento, cantidad, filtro) {
            let url = `/${controlador}/${Ajax.EndPoint.CargaDinamica}?${Ajax.Param.claseElemento}=${claseElemento}&posicion=0&cantidad=${cantidad}&filtro=${JSON.stringify(filtro)}`;
            return url;
        }
    }
    Crud.CrudBase = CrudBase;
})(Crud || (Crud = {}));
//# sourceMappingURL=CrudBase.js.map