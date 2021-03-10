var ApiDeAjax;
(function (ApiDeAjax) {
    class ResultadoJson {
    }
    ApiDeAjax.ResultadoJson = ResultadoJson;
    class ResultadoHtml extends ResultadoJson {
    }
    ApiDeAjax.ResultadoHtml = ResultadoHtml;
    let TipoPeticion;
    (function (TipoPeticion) {
        TipoPeticion[TipoPeticion["Sincrona"] = 0] = "Sincrona";
        TipoPeticion[TipoPeticion["Asincrona"] = 1] = "Asincrona";
    })(TipoPeticion = ApiDeAjax.TipoPeticion || (ApiDeAjax.TipoPeticion = {}));
    function EsAsincrona(valor) {
        if (valor === TipoPeticion.Sincrona)
            return false;
        return true;
    }
    let ModoPeticion;
    (function (ModoPeticion) {
        ModoPeticion[ModoPeticion["Get"] = 0] = "Get";
        ModoPeticion[ModoPeticion["Post"] = 1] = "Post";
    })(ModoPeticion = ApiDeAjax.ModoPeticion || (ApiDeAjax.ModoPeticion = {}));
    function ParsearModo(modo) {
        if (modo === ModoPeticion.Get)
            return 'get';
        return 'post';
    }
    class DescriptorAjax {
        constructor(llamante, peticion, datos, url, tipo, modo, trasLaPeticion, siHayError) {
            this.Error = false;
            this.llamador = llamante;
            this.nombre = peticion;
            this.DatosDeEntrada = datos;
            this.resultado = undefined;
            this._tipoPeticion = tipo;
            this._modoPeticion = modo;
            this.Inicializar(url, trasLaPeticion, siHayError);
        }
        get Tipo() { return this._tipoPeticion; }
        get Request() { return this._req; }
        get Url() { return this._url; }
        get Modo() { return this._modoPeticion; }
        set DatosPost(datos) { this._datosPost = datos; }
        ParsearRespuesta() {
            try {
                this.resultado = JSON.parse(this.Request.response);
            }
            catch {
                MensajesSe.Error("ParsearRespuesta", `Error al procesar la respuesta de ${this.nombre}`);
            }
        }
        Inicializar(url, trasLaPeticion, siHayError) {
            this._req = new XMLHttpRequest();
            this._url = url;
            this.TrasLaPeticion = trasLaPeticion;
            this.ProcesarError = siHayError;
        }
        Ejecutar() {
            BlanquearMensaje();
            this.PeticionAjax();
            if (this.Error)
                throw `${this.resultado.mensaje}`;
        }
        PeticionAjax() {
            function RespuestaCorrecta(descriptor) {
                try {
                    if (IsNullOrEmpty(descriptor.Request.response))
                        descriptor.ErrorEnPeticion();
                    else {
                        descriptor.ParsearRespuesta();
                        if (descriptor.resultado === undefined || descriptor.resultado.estado === Ajax.jsonResultError)
                            descriptor.ErrorEnPeticion();
                        else
                            descriptor.DespuesDeLaPeticion();
                    }
                }
                //catch (error) {
                //   Mensaje(TipoMensaje.Error, `Error al procesar la peticion ${this.nombre}`, error);
                //}
                finally {
                    QuitarCapa();
                }
            }
            function RespuestaErronea() {
                try {
                    this.ErrorEnPeticion();
                }
                finally {
                    QuitarCapa();
                }
            }
            this.Request.addEventListener(Ajax.eventoLoad, () => RespuestaCorrecta(this));
            this.Request.addEventListener(Ajax.eventoError, () => RespuestaErronea());
            this.Request.open(ParsearModo(this.Modo), this.Url, EsAsincrona(this.Tipo));
            if (EsAsincrona(this.Tipo)) {
                PonerCapa();
            }
            if (this._datosPost != undefined)
                this.Request.send(this._datosPost);
            else
                this.Request.send();
        }
        ErrorEnPeticion() {
            this.Error = true;
            if (this.Request.status === 404) {
                this.resultado = new ResultadoJson();
                this.resultado.mensaje = `Error al acceder al servidor`;
                console.error(`Error al ejecutar la peticion '${this.nombre}'. Petición no definida. No está definida la petición con los parámetros indicados: ${this.Url}`);
            }
            else if (this.Request.status === 500) {
                this.resultado = new ResultadoJson();
                this.resultado.mensaje = `Error al ejecutar la peticion '${this.nombre}'. Petición ambigüa`;
                console.error(`Petición mal definida: ${this.Url}. ${this.Request.response}`);
            }
            else {
                this.resultado = JSON.parse(this.Request.response);
                if (IsNullOrEmpty(this.resultado.consola)) {
                    this.resultado.consola = `Error al ejecutar la peticion '${this.nombre}'. ${this.resultado.mensaje}`;
                }
                console.error(this.resultado.consola);
            }
            if (this.ProcesarError)
                this.ProcesarError(this);
        }
        DespuesDeLaPeticion() {
            this.resultado = JSON.parse(this.Request.response);
            if (!IsNullOrEmpty(this.resultado.consola))
                console.log(this.resultado.consola);
            if (!IsNullOrEmpty(this.resultado.mensaje))
                console.log(TipoMensaje.Info, this.resultado.mensaje);
            if (this.TrasLaPeticion)
                try {
                    this.TrasLaPeticion(this);
                }
                catch (error) {
                    MensajesSe.Error("DespuesDeLaPeticion", `Error al procesar la peticion ${this.nombre}`, error);
                }
        }
    }
    ApiDeAjax.DescriptorAjax = DescriptorAjax;
    function ErrorTrasPeticion(origen, peticion) {
        MensajesSe.Error(origen, peticion.resultado.mensaje, peticion.resultado.consola);
    }
    ApiDeAjax.ErrorTrasPeticion = ErrorTrasPeticion;
})(ApiDeAjax || (ApiDeAjax = {}));
//# sourceMappingURL=ApiDeAjax.js.map