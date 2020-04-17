namespace Crud {

    export class CrudEdicion extends CrudBase {

        constructor(idPanelMnt: string, idPaneEdicion: string) {
            super(idPanelMnt, null, idPaneEdicion);
        }

        InicializarValores(infSel: InfoSelector) {
            let id: number = infSel.Seleccionados[0] as number;

            let control: HTMLElement = this.BuscarControl(this.PanelEdicion, Literal.id);
            if (control == null) {
                Mensaje(TipoMensaje.Error, "No está definido el control para mostrar el id del elemento");
                return;
            }
            (control as HTMLInputElement).value = id.toString();

            this.LeerElemento(id);
        }


        private LeerElemento(id: number) {
            let json: JSON = JSON.parse(`[${id}]`);
            let url: string = this.urlPeticionLeer(json);
            let req: XMLHttpRequest = new XMLHttpRequest();
            req.open('GET', url, false);
            this.PeticionSincrona(req, Ajax.EndPoint.LeerPorIds);
        }

        private urlPeticionLeer(json: JSON): string {
            let controlador = this.PanelEdicion.getAttribute(Literal.controlador);
            let url: string = `/${controlador}/${Ajax.EndPoint.LeerPorIds}?${Ajax.Param.idsJson}=${JSON.stringify(json)}`;
            return url;
        }

        protected DespuesDeLaPeticion(req): ResultadoJson {
            let resultado = super.DespuesDeLaPeticion(req);
            this.MapearElemento(resultado.datos)
            return resultado;
        }

        protected MapearElemento(datos: any) {
            throw new Error("Method not implemented.");
        }

    }

    export function EjecutarMenuEdt(accion: string, idDivMostrarHtml: string, idDivOcultarHtml: string, gestor: Crud.CrudBase): void {

        if (accion === LiteralEdt.cancelaredicion)
            CancelarEdicion(idDivMostrarHtml, idDivOcultarHtml, gestor as CrudEdicion);
        else
            Mensaje(TipoMensaje.Info, `la opción ${accion} no está definida`);
    }

    function CancelarEdicion(idDivMostrarHtml: string, idDivOcultarHtml: string, gestorDeEdicion: CrudEdicion) {
        let htmlDivMostrar: HTMLDivElement = document.getElementById(`${idDivMostrarHtml}`) as HTMLDivElement;
        let htmlDivOcultar: HTMLDivElement = document.getElementById(`${idDivOcultarHtml}`) as HTMLDivElement;
        try {
            gestorDeEdicion.Cerrar(htmlDivMostrar, htmlDivOcultar);
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error.menssage);
        }
    }
}