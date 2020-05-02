
//************************************************************************************************************************************************************************************/

/// Gestión de los info selectores

//************************************************************************************************************************************************************************************/
class Elemento {
    public Id: number;
    public Texto: string;

    public static get ElementoVacio(): Elemento { return new Elemento(0, ''); }

    constructor(id: number, texto: string) {
        this.Id = id;
        this.Texto = texto;
    }

    EsVacio(): boolean {
        if (this.Id === 0 || this.Texto === '')
            return true;
    }
}

class InfoSelector {

    private idGrid: string;
    private seleccionados: Array<number>;
    private paraMostrarEnSelector: Array<string>;
    private seleccionables: number;
    private htmlGrid: HTMLElement;

    public get Id() { return this.idGrid; }
    public get Cantidad() { return this.seleccionados.length; }
    public get Seleccionables() { return this.Seleccionables == NaN ? 0 : this.seleccionables; }
    public get Seleccionados() { return this.seleccionados; }

    iniciarClase(idGrid) {
        this.idGrid = idGrid;
        this.htmlGrid = document.getElementById(idGrid);
        this.seleccionables = this.htmlGrid.getAttribute("seleccionables").Numero();
        this.seleccionados = new Array();
        this.paraMostrarEnSelector = new Array();
    }

    constructor(idGrid) {
        this.iniciarClase(idGrid);
        console.log(`Ha creado el infoselector ${idGrid}`);
    }

    deshabilitarCheck(deshabilitar) {

        var ejecutar = false;
        //si has desmarcado checks y los seleccionados son menos que los seleccionables --> ok a habilitar
        if (deshabilitar === false && this.Cantidad < this.seleccionables)
            ejecutar = true;

        //Si has marcado y los seleccionados son más o igual que los seleccionables --> ok a deshabilitar
        if (deshabilitar === true && this.Cantidad >= this.seleccionables)
            ejecutar = true;

        if (ejecutar) {
            document.getElementsByName(`chksel.${this.idGrid}`).forEach(c => {
                let check = <HTMLInputElement>c;
                if (!check.checked) {
                    check.disabled = deshabilitar;
                }
                else {
                    check.disabled = false;
                }
            }
            );
        }
    }

    LeerId(pos): number {
        if (pos >= 0 && pos < this.Cantidad) {
            return this.seleccionados[pos];
        }
        console.log(`Ha intentado leer la posición ${pos} en una lista de longitud ${this.Cantidad}`);
        return 0;
    }

    LeerElemento(pos: number): Elemento {
            var id = this.LeerId(pos);
            if (id > 0) {
                var texto = this.paraMostrarEnSelector[pos];
                return new Elemento(id, texto);
            }
        return Elemento.ElementoVacio;
    }

    InsertarId(id) {
        if (!id || isNaN(parseInt(id))) {
            console.error(`Ha intentado insertar en la lista un id no válido ${id}`);
            return -1;
        }

        if (this.seleccionados.length < this.seleccionables) {
            this.seleccionados.push(id);
            console.log(`Ha insertar en la lista el id ${id}`);
        }

        this.deshabilitarCheck(true);
        return this.Cantidad;
    }

    InsertarElemento(id, textoMostrar) {
        var pos = this.InsertarId(id);
        if (pos === this.seleccionados.length) {
            this.paraMostrarEnSelector.push(textoMostrar);
        }
        return pos;
    }

    InsertarElementos(elementos) {

        if (!elementos || elementos.length > 0) {
            for (var i = 0; i < elementos.length; i++) {
                var e = elementos[i];
                if (this.seleccionados.indexOf(e.id) < 0)
                    this.InsertarElemento(e.id, e.valor);
            }
        }
        else {
            console.log(`Ha intentado insertar en la lista un array de elementos vacío`);
        }

        return this.Cantidad;
    }

    InsertarIds(ids) {

        if (!ids || ids.length === 1 && isNaN(parseInt(ids))) {
            console.log(`Ha intentado insertar en la lista un array de ids no válidos ${ids}`);
            return -2;
        }

        for (var i = 0; i < ids.length; i++) {
            var idSeleccionado = ids[i];
            this.InsertarId(idSeleccionado);
        }

        return this.Cantidad;
    }

    Buscar(id): number {
        return this.seleccionados.indexOf(id);
    }

    Quitar(idSeleccionado) {
        var pos = this.seleccionados.indexOf(idSeleccionado);
        if (pos >= 0) {
            this.seleccionados.splice(pos, 1);
            this.paraMostrarEnSelector.splice(pos, 1);
            console.log(`Ha quitado de la lista el ${idSeleccionado}`);
            this.deshabilitarCheck(false);
        }
        else
            console.error(`No se ha localizado el elemento con id  ${idSeleccionado}`);
    }

    QuitarTodos() {
        this.seleccionados.splice(0, this.seleccionados.length);
        this.paraMostrarEnSelector.splice(0, this.paraMostrarEnSelector.length);

    }

    ToString() {
        var ids = "";
        for (var i = 0; i < this.seleccionados.length; i++) {
            ids = ids + this.seleccionados[i];
            if (i < this.seleccionados.length - 1)
                ids = ids + ';';
        }
        return ids;
    }

    SincronizarCheck() {
        this.deshabilitarCheck(true);
    }

}


class InfoSelectores {

    _infoSelectores = new Array<InfoSelector>();

    constructor() {
        console.log("Aray de Infoselectores construido");
    }

    get Cantidad(): number {
        if (!this._infoSelectores)
            return 0;
        else
            return this._infoSelectores.length;
    }

    Obtener(id: string): InfoSelector {
        if (!this._infoSelectores || this._infoSelectores.length === 0)
            return undefined;

        for (var i = 0; i < this.Cantidad; i++) {
            if (this._infoSelectores[i].Id === id)
                return this._infoSelectores[i];
        }
        return undefined;
    }


    Insertar(infoSelector: InfoSelector): number {
        var infSel = this.Obtener(infoSelector.Id);
        if (!infSel)
            this._infoSelectores.push(infoSelector);

        return this._infoSelectores.length;
    }

    Borrar(id: string): number {
        for (var i = 0; i < this.Cantidad; i++) {
            if (this._infoSelectores[i].Id === id)
                this._infoSelectores.splice(i, 1);
        }
        return this._infoSelectores.length;
    }

    Crear(id: string): InfoSelector {
        this.Borrar(id);
        let infSel: InfoSelector = new InfoSelector(id);
        infoSelectores.Insertar(infSel);
        return infSel;
    }
}


var infoSelectores = new InfoSelectores();




