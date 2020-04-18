namespace Crud {

    export class CrudCreacion extends CrudBase {

        constructor(idPanelMnt: string, idPanelCreacion: string) {
            super(idPanelMnt, idPanelCreacion, null);
        }

        public InicializarValores(): void {

        }

        public Aceptar() {
            let json: JSON = null;
            try {
                json = this.MapearControlesDeIU(this.PanelDeCrear);
            }
            catch (error) {
                this.ResultadoPeticion = error.message;
                return;
            }
            this.CrearElemento(json);
        }

        private CrearElemento(json: JSON) {
            let controlador = this.PanelDeCrear.getAttribute(Literal.controlador);
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
        gestorDeCreacion.Aceptar();
        if (gestorDeCreacion.PeticioCorrecta) {
            gestorDeCreacion.Cerrar(htmlDivMostrar, htmlDivOcultar);
        }
        Mensaje(gestorDeCreacion.PeticioCorrecta ? TipoMensaje.Info : TipoMensaje.Error, gestorDeCreacion.ResultadoPeticion);
    }

    function CancelarNuevo(idDivMnt: string, idDivNuevo: string, gestorDeCreacion: CrudCreacion) {
        let panelDeMnt: HTMLDivElement = document.getElementById(`${idDivMnt}`) as HTMLDivElement;
        let panelDeCrear: HTMLDivElement = document.getElementById(`${idDivNuevo}`) as HTMLDivElement;
        try {
            gestorDeCreacion.Cerrar(panelDeMnt, panelDeCrear);
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error.menssage);
        }
    }

}
