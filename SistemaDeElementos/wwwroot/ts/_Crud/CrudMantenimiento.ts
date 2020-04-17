namespace Crud {

    export class CrudMnt extends CrudBase {

        constructor(idPanelMnt: string, idPanelCreacion: string, idPanelEdicion: string) {
            super(idPanelMnt, null, idPanelCreacion);
        }

        InicializarValores() {
            
        }

    }

    export function EjecutarMenuMnt(accion: string, idDivMostrarHtml: string, idDivOcultarHtml: string, gestor: Crud.CrudBase): void {

        if (accion === LiteralMnt.crearelemento)
            IraCrear(gestor as Crud.CrudCreacion, idDivMostrarHtml, idDivOcultarHtml);
        else if (accion === LiteralMnt.editarelemento)
            IraEditar(gestor as Crud.CrudEdicion, idDivMostrarHtml, idDivOcultarHtml);
        else
            Mensaje(TipoMensaje.Info, `la opción ${accion} no está definida`);
    }

    export function AlPulsarUnCheckDeSeleccion(idGrid, idCheck) {
        BlanquearMensaje();
        var check = <HTMLInputElement>document.getElementById(idCheck);
        if (check.checked)
            AnadirAlInfoSelector(idGrid, idCheck);
        else
            QuitarDelSelector(idGrid, idCheck);
    }

    function IraEditar(gestorDeEdicion: Crud.CrudEdicion, idDivMostrarHtml: string, idDivOcultarHtml: string) {

        //obtener los elementos del grid seleccionado
        // si no los hay indicarlo y salir
        // Si los hay pasar el selector al inicializador

        let htmlDivMostrar: HTMLDivElement = document.getElementById(`${idDivMostrarHtml}`) as HTMLDivElement;
        let htmlDivOcultar: HTMLDivElement = document.getElementById(`${idDivOcultarHtml}`) as HTMLDivElement;

        htmlDivMostrar.classList.add(ClaseCss.divVisible);
        htmlDivMostrar.classList.remove(ClaseCss.divNoVisible);

        htmlDivOcultar.classList.add(ClaseCss.divNoVisible);
        htmlDivOcultar.classList.remove(ClaseCss.divVisible);

        gestorDeEdicion.InicializarValores();
    }

    function IraCrear(gestorDeCreacion: Crud.CrudCreacion, idDivMostrarHtml: string, idDivOcultarHtml: string) {
        let htmlDivMostrar: HTMLDivElement = document.getElementById(`${idDivMostrarHtml}`) as HTMLDivElement;
        let htmlDivOcultar: HTMLDivElement = document.getElementById(`${idDivOcultarHtml}`) as HTMLDivElement;

        htmlDivMostrar.classList.add(ClaseCss.divVisible);
        htmlDivMostrar.classList.remove(ClaseCss.divNoVisible);

        htmlDivOcultar.classList.add(ClaseCss.divNoVisible);
        htmlDivOcultar.classList.remove(ClaseCss.divVisible);

        gestorDeCreacion.InicializarValores();
    }

}