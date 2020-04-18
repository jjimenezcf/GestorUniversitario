namespace Crud {

    export class CrudEdicion extends CrudBase {

        constructor(idPanelMnt: string, idPaneEdicion: string) {
            super(idPanelMnt, null, idPaneEdicion);
        }

        InicializarValores(infSel: InfoSelector) {
            let id: number = infSel.Seleccionados[0] as number;

            let control: HTMLElement = this.BuscarControl(this.PanelDeEditar, Literal.id);
            if (control == null) {
                Mensaje(TipoMensaje.Error, "No está definido el control para mostrar el id del elemento");
                return;
            }
            (control as HTMLInputElement).value = id.toString();

            this.LeerElemento(id);
        }

        private LeerElemento(id: number) {
            let idJson: JSON = JSON.parse(`[${id}]`);

            let controlador = this.PanelDeEditar.getAttribute(Literal.controlador);
            let url: string = `/${controlador}/${Ajax.EndPoint.LeerPorIds}?${Ajax.Param.idsJson}=${JSON.stringify(idJson)}`;

            let req: XMLHttpRequest = new XMLHttpRequest();
            req.open('GET', url, false);
            this.PeticionSincrona(req, Ajax.EndPoint.LeerPorIds);
        }

        protected DespuesDeLaPeticion(req): ResultadoJson {
            let resultado = super.DespuesDeLaPeticion(req);
            this.MapearElemento(resultado.datos);
            return resultado;
        }

    }

    export function EjecutarMenuEdt(accion: string, idDivMostrarHtml: string, idDivOcultarHtml: string, gestor: Crud.CrudBase): void {

        if (accion === LiteralEdt.cancelaredicion)
            CancelarEdicion(idDivMostrarHtml, idDivOcultarHtml, gestor as CrudEdicion);
        else
            Mensaje(TipoMensaje.Info, `la opción ${accion} no está definida`);
    }

    function CancelarEdicion(idDivMnt: string, idDivEdt: string, gestorDeEdicion: CrudEdicion) {
        let panelMnt: HTMLDivElement = document.getElementById(`${idDivMnt}`) as HTMLDivElement;
        let panelEdt: HTMLDivElement = document.getElementById(`${idDivEdt}`) as HTMLDivElement;
        try {
            gestorDeEdicion.Cerrar(panelMnt, panelEdt);
        }
        catch (error) {
            Mensaje(TipoMensaje.Error, error.menssage);
        }
    }
}