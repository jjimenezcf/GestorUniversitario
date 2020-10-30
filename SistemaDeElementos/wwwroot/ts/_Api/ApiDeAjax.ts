namespace ApiDeAjax {

    export enum TipoPeticion {
        Sincrona,
        Asincrona
    }

    function EsAsincrona(valor: TipoPeticion): boolean {
        if (valor === TipoPeticion.Sincrona)
            return false;
        return true;
    }

    export enum ModoPeticion {
        Get,
        Post
    }

    function ParsearModo(modo: ModoPeticion) {
        if (modo === ModoPeticion.Get)
            return 'get';
        return 'post';
    }

    export class DescriptorAjax {
        private _tipoPeticion: TipoPeticion;
        private _modoPeticion: ModoPeticion;
        private _req: XMLHttpRequest;
        private _url: string;
        private _idBarraDeProceso: string;
        private _divBarra: HTMLDivElement;
        private _span: Element;
        private _datosPost: FormData;

        public llamador: any;
        public nombre: string;
        public DatosDeEntrada: any;
        public resultado: ResultadoJson;
        public Error: boolean = false;

        public get IdBarraDeProceso(): string { return this._idBarraDeProceso; };
        public get Tipo(): TipoPeticion { return this._tipoPeticion; }
        public get Request(): XMLHttpRequest { return this._req; }
        public get Url(): string { return this._url; }
        public get Modo(): ModoPeticion { return this._modoPeticion; }

        public set IdBarraDeProceso(id: string) { this._idBarraDeProceso = id; }
        public set DatosPost(datos: FormData) { this._datosPost = datos; }

        public TrasLaPeticion: Function;
        public ProcesarError: Function;
        constructor(llamante: any, peticion: string, datos: any, url: string, tipo: TipoPeticion, modo: ModoPeticion, trasLaPeticion: Function, siHayError: Function) {
            this.llamador = llamante;
            this.nombre = peticion;
            this.DatosDeEntrada = datos;
            this.resultado = undefined;
            this._tipoPeticion = tipo;
            this._modoPeticion = modo;
            this.Inicializar(url, trasLaPeticion, siHayError);
        }

        private ParsearRespuesta() {
            try {
                this.resultado = JSON.parse(this.Request.response);
            }
            catch
            {
                Mensaje(TipoMensaje.Error, `Error al procesar la respuesta de ${this.nombre}`);
            }
        }

        public Inicializar(url: string, trasLaPeticion: Function, siHayError: Function) {
            this._req = new XMLHttpRequest();
            this._url = url;
            this.TrasLaPeticion = trasLaPeticion;
            this.ProcesarError = siHayError;
        }

        public Ejecutar() {
            BlanquearMensaje();
            this.PeticionAjax();
            if (this.Error) throw `${this.resultado.mensaje}`;
        }

        public DefinirBarraDeProceso() {
            if (!IsNullOrEmpty(this.IdBarraDeProceso)) {
                this._divBarra = document.getElementById(this.IdBarraDeProceso) as HTMLDivElement;
                this._span = this._divBarra.children[0];
                this._divBarra.classList.remove(ClaseCss.barraVerde, ClaseCss.barraRoja);
                this._divBarra.classList.add(ClaseCss.barraAzul);
                this.Request.upload.addEventListener("progress", (event) => {
                    let porcentaje = Math.round((event.loaded / event.total) * 100);
                    this._divBarra.style.width = porcentaje + '%';
                    this._span.innerHTML = porcentaje + '%';
                });
            }
        }

        private PeticionAjax() {

            function RespuestaCorrecta(descriptor: DescriptorAjax) {
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

            this.DefinirBarraDeProceso();
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

        private ErrorEnPeticion() {
                this.Error = true;
                if (this._divBarra != undefined) {
                    this._divBarra.classList.remove(ClaseCss.barraVerde);
                    this._divBarra.classList.remove(ClaseCss.barraAzul);
                    this._divBarra.classList.add(ClaseCss.barraRoja);
                    this._span.innerHTML = "Error al subir el fichero";
                }

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

        private DespuesDeLaPeticion() {
            this.resultado = JSON.parse(this.Request.response);

            if (!IsNullOrEmpty(this.resultado.consola))
                console.log(this.resultado.consola);

            if (!IsNullOrEmpty(this.resultado.mensaje))
                Mensaje(TipoMensaje.Info, this.resultado.mensaje);

            if (this._divBarra != undefined) {
                this._divBarra.classList.remove(ClaseCss.barraRoja);
                this._divBarra.classList.add(ClaseCss.barraVerde);
                this._span.innerHTML = "Proceso completado";
            }

            if (this.TrasLaPeticion)
                this.TrasLaPeticion(this);
        }
    }




}