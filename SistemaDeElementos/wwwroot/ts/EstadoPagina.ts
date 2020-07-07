const Estado = {
    Pagina: 'pagina'
};

class EstadoPagina extends Diccionario<any> implements IDiccionario<any> {
    constructor(pagina: string) {
        super();
        this.anadir(Estado.Pagina, pagina);
        GuardarEstado(this);
    }
}

function CrearEstado(pagina: string): EstadoPagina {
    return new EstadoPagina(pagina);
}

function LeerHistorialDeNavegacion(): Diccionario<EstadoPagina> {
    let _historialJson:string = sessionStorage.getItem('historial');

    if (IsNullOrEmpty(_historialJson)) {
        var a = new Diccionario<EstadoPagina>();
        GuardarHistorialDeNavegacion(a);
        _historialJson = sessionStorage.getItem('historial');
    }

    let diccionario: Diccionario<EstadoPagina> = JsonToDiccionario(_historialJson);

    return diccionario ;
};

function GuardarHistorialDeNavegacion(historial: Diccionario<EstadoPagina>) {
    sessionStorage.setItem('historial',JSON.stringify(historial));
}

function GuardarEstado(estado: EstadoPagina) {
    let historial: Diccionario<EstadoPagina> = LeerHistorialDeNavegacion();
    historial.anadir(estado.obtener(Estado.Pagina), estado);
    GuardarHistorialDeNavegacion(historial);
}
