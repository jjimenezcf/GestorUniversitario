class CrudEdicion extends CrudBase {

    constructor(idPanelMnt: string, idPanelCreacion: string) {
        super(idPanelMnt, null, idPanelCreacion);
    }

    InicializarValores() {
        Mensaje(TipoMensaje.Info,"He de leer el registro con ID pasado")
    }

}