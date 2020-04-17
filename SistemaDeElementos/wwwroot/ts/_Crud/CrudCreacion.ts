namespace Crud {

    export class CrudCreacion extends CrudBase {

        constructor(idPanelMnt: string, idPanelCreacion: string) {
            super(idPanelMnt, idPanelCreacion, null);
        }

        public InicializarValores(): void {

        }

        public Aceptar(htmlDivMostrar: HTMLDivElement, htmlDivOcultar: HTMLDivElement) {
            let json: JSON = null;
            try {
                json = this.MapearControlesDeIU();
            }
            catch (error) {
                this.ResultadoPeticion = error.message;
                return;
            }
            this.CrearElemento(json, htmlDivMostrar, htmlDivOcultar);
        }

        protected AntesDeMapearDatosDeIU(): JSON {
            return JSON.parse(`{"${Literal.id}":"0"}`);
        }

        private MapearControlesDeIU(): JSON {
            let json: JSON = this.AntesDeMapearDatosDeIU();

            let propiedades: HTMLCollectionOf<Element> = this.PanelCreacion.getElementsByClassName("propiedad");
            for (var i = 0; i < propiedades.length; i++) {
                var propiedad = propiedades[i] as HTMLElement;
                if (propiedad instanceof HTMLInputElement) {
                    var propiedadDto = propiedad.getAttribute(Atributo.propiedadDto);
                    json[propiedadDto] = this.MapearInput(propiedad, propiedadDto);
                }
            }

            return this.DespuesDeMapearDatosDeIU(json);
        }

        private MapearInput(propiedad: HTMLInputElement, propiedadDto: string): string {
            let valor: string = (propiedad as HTMLInputElement).value;
            let obligatorio: string = propiedad.getAttribute(Atributo.obligatorio);

            if (obligatorio === "S" && valor.NoDefinida()) {
                let cssNoValida: string = propiedad.getAttribute(Atributo.classNoValido);
                propiedad.className = `${ClaseCss.classPropiedad} ${cssNoValida}`;
                throw new Error(`El campo ${propiedadDto} es obligatorio`);
            }

            let cssValida: string = propiedad.getAttribute(Atributo.classValido);
            propiedad.className = `${ClaseCss.classPropiedad} ${cssValida}`;
            return valor;
        }

        protected DespuesDeMapearDatosDeIU(json: JSON): JSON {
            return json;
        }

        private CrearElemento(json: JSON, htmlDivMostrar: HTMLDivElement, htmlDivOcultar: HTMLDivElement) {
            let controlador = this.PanelCreacion.getAttribute(Literal.controlador);
            let url: string = `/${controlador}/${Ajax.EndPoint.Crear}?${Ajax.Param.elementoJson}=${JSON.stringify(json)}`;
            let req: XMLHttpRequest = new XMLHttpRequest();
            req.open('GET', url, false);
            this.PeticionSincrona(req, Ajax.EndPoint.Crear);
        }
    }

    export function EjecutarMenuCrt(accion: string, idDivMostrarHtml: string, idDivOcultarHtml: string, gestor: Crud.CrudBase): void {

        if (accion === LiteralCrt.nuevoelemento)
            NuevoElemento(gestor as CrudCreacion, idDivMostrarHtml, idDivOcultarHtml);
        else if (accion === LiteralCrt.cancelarnuevo)
            CancelarNuevo(idDivMostrarHtml, idDivOcultarHtml, gestor as CrudCreacion);
        else
            Mensaje(TipoMensaje.Info, `la opción ${accion} no está definida`);
    }

    function NuevoElemento(gestorDeCreacion: CrudCreacion, idDivMostrarHtml: string, idDivOcultarHtml: string) {
        let htmlDivMostrar: HTMLDivElement = document.getElementById(`${idDivMostrarHtml}`) as HTMLDivElement;
        let htmlDivOcultar: HTMLDivElement = document.getElementById(`${idDivOcultarHtml}`) as HTMLDivElement;
        gestorDeCreacion.Aceptar(htmlDivMostrar, htmlDivOcultar);
        if (gestorDeCreacion.PeticioCorrecta) {
            gestorDeCreacion.Cerrar(htmlDivMostrar, htmlDivOcultar);
        }
        Mensaje(gestorDeCreacion.PeticioCorrecta ? TipoMensaje.Info : TipoMensaje.Error, gestorDeCreacion.ResultadoPeticion);
    }

    function CancelarNuevo(idDivMostrarHtml: string, idDivOcultarHtml: string, gestorDeCreacion: CrudCreacion) {
        let htmlDivMostrar: HTMLDivElement = document.getElementById(`${idDivMostrarHtml}`) as HTMLDivElement;
        let htmlDivOcultar: HTMLDivElement = document.getElementById(`${idDivOcultarHtml}`) as HTMLDivElement;
        try {
            gestorDeCreacion.Cerrar(htmlDivMostrar, htmlDivOcultar);
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error.menssage);
        }
    }

}
