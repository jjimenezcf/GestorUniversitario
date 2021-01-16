
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
    private _Seleccionables: number;
    private htmlGrid: HTMLElement;

    private get Seleccionables(): number {
        return this._Seleccionables == NaN
            ? 0
            : this._Seleccionables;
    }
    private get PuedeSeleccionarMas(): boolean {
        if (this.Seleccionables < 0)
            return true;

        return this.Cantidad < this.Seleccionables;
    }

    private set Seleccionables(seleccionables: number) {
        this._Seleccionables = seleccionables;
    }

    public get Id(): string { return this.idGrid; }
    public get Cantidad(): number { return this.seleccionados.length; }
    public get Seleccionados(): number[] { return this.seleccionados; }
   

    iniciarClase(idGrid) {
        this.idGrid = idGrid;
        this.htmlGrid = document.getElementById(idGrid);
        this.Seleccionables = Numero(this.htmlGrid.getAttribute("seleccionables"));
        this.seleccionados = new Array();
        this.paraMostrarEnSelector = new Array();
    }

    constructor(idGrid) {
        this.iniciarClase(idGrid);
        console.log(`Ha creado el infoselector ${idGrid}`);
    }

    private deshabilitarCheck(deshabilitar: boolean) {

        var ejecutar = false;
        //si has desmarcado checks y los seleccionados son menos que los seleccionables --> ok a habilitar
        if (deshabilitar === false && this.PuedeSeleccionarMas)
            ejecutar = true;

        //Si has marcado y los seleccionados son más o igual que los seleccionables --> ok a deshabilitar
        if (deshabilitar === true && !this.PuedeSeleccionarMas)
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

    public Contiene(id: number): boolean
    {
        if (this.Buscar(id) < 0)
            return false;
        else
            return true;
    }

    public LeerId(pos: number): number {
        if (pos >= 0 && pos < this.Cantidad) {
            return this.seleccionados[pos];
        }
        console.log(`Ha intentado leer la posición ${pos} en una lista de longitud ${this.Cantidad}`);
        return 0;
    }

    public LeerElemento(pos: number): Elemento {
        var id = this.LeerId(pos);
        if (id > 0) {
            var texto = this.paraMostrarEnSelector[pos];
            return new Elemento(id, texto);
        }
        return Elemento.ElementoVacio;
    }

    private InsertarId(id: number) {
        if (this.PuedeSeleccionarMas) {
            this.seleccionados.push(id);
        }

        this.deshabilitarCheck(true);
        return this.Cantidad;
    }

    public InsertarElemento(id: number, textoMostrar: string): number {
        if (!id || isNaN(id)) {
            Mensaje(TipoMensaje.Error,`Ha intentado insertar en la lista un id no válido ${id}`);
            return -1;
        }
        var pos = this.Buscar(id);
        if (pos < 0) {
            pos = this.InsertarId(id);
            if (pos === this.seleccionados.length) {
                this.paraMostrarEnSelector.push(textoMostrar);
            }
        }
        return pos;
    }

    public InsertarElementos(elementos: Array<Elemento>): number {
        if (!elementos || elementos.length > 0) {
            for (var i = 0; i < elementos.length; i++) {
                let e: Elemento = elementos[i];
                if (this.seleccionados.indexOf(e.Id) < 0)
                    this.InsertarElemento(e.Id, e.Texto);
            }
        }
        else {
            console.log(`Ha intentado insertar en la lista un array de elementos vacío`);
        }

        return this.Cantidad;
    }

    public Buscar(id: number): number {
        return this.seleccionados.indexOf(id);
    }

    public Quitar(idSeleccionado: number) {
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

    public QuitarTodos(): void {
        this.seleccionados.splice(0, this.seleccionados.length);
        this.paraMostrarEnSelector.splice(0, this.paraMostrarEnSelector.length);
    }

    public ToString(): string {
        let ids: string = "";
        for (var i = 0; i < this.seleccionados.length; i++) {
            ids = ids + this.seleccionados[i];
            if (i < this.seleccionados.length - 1)
                ids = ids + ';';
        }
        return ids;
    }

    public SincronizarCheck(): void {
        this.deshabilitarCheck(true);
    }

}

