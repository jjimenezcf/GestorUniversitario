
//************************************************************************************************************************************************************************************/

/// Gestión de los info selectores

//************************************************************************************************************************************************************************************/
class Elemento {
    private _datos: any = null;
    private _modoDeAcceso: string;
    public expresionMostrar: string;

    public get Id(): number { return this._datos["id"]; }
    public get Texto(): string { return this.mostrar(); }
    public get ModoDeAcceso(): string { return this._modoDeAcceso; }

    public static get ElementoVacio(): Elemento { return new Elemento(null); }

    constructor(elemento: Elemento) {
        if (elemento !== null) {
            this._datos = elemento._datos;
            this._modoDeAcceso = (elemento._datos["ModoDeAcceso"] as string).toLowerCase();
            this.expresionMostrar = elemento._datos;
        }
    }

    EsVacio(): boolean {
        return this._datos === null;
    }

    private mostrar(): string {
        // recorre el datos y compone la expresione
        return "pendiente";
    }
}

class InfoSelector {

    private idGrid: string;
    private _Seleccionables: number;
    private htmlGrid: HTMLElement;
    private elementos: Elemento[] = [];

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
    public get Cantidad(): number { return this.elementos.length; }
    public get Seleccionados(): Elemento[] { return this.elementos; }


    iniciarClase(idGrid) {
        this.idGrid = idGrid;
        this.htmlGrid = document.getElementById(idGrid);
        this.Seleccionables = Numero(this.htmlGrid.getAttribute("seleccionables"));
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

    public Contiene(id: number): boolean {
        if (this.Buscar(id) < 0)
            return false;
        else
            return true;
    }

    public LeerId(pos: number): number {
        if (pos >= 0 && pos < this.Cantidad) {
            return this.elementos[pos].Id as number;
        }
        console.log(`Ha intentado leer la posición ${pos} en una lista de longitud ${this.Cantidad}`);
        return 0;
    }

    public LeerElemento(pos: number): Elemento {
        if (pos >= 0 && pos < this.Cantidad) {
            return this.elementos[pos] as Elemento;
        }
        console.log(`Ha intentado leer la posición ${pos} en una lista de longitud ${this.Cantidad}`);
        return Elemento.ElementoVacio;
    }

    public InsertarElemento(elemento: Elemento): number {
        var pos = this.Buscar(elemento.Id);
        if (pos < 0) {
            this.elementos.push(elemento);
        }
        return pos;
    }

    public InsertarElementos(elementos: Array<Elemento>): number {
        if (!elementos || elementos.length > 0) {
            for (var i = 0; i < elementos.length; i++) {
                let e: Elemento = elementos[i];
                    this.InsertarElemento(e);
            }
        }
        else {
            console.log(`Ha intentado insertar en la lista un array de elementos vacío`);
        }

        return this.Cantidad;
    }

    public Buscar(id: number): number {
        for (let i: number = 0; i < this.elementos.length; i++)
            if (Numero(this.elementos[i].Id) === id)
                return (i);
        return -1;
    }

    public Quitar(idSeleccionado: number) {
        var pos = this.Buscar(idSeleccionado);
        if (pos >= 0) {
            this.elementos.splice(pos, 1);
            console.log(`Ha quitado de la lista el ${idSeleccionado}`);
            this.deshabilitarCheck(false);
        }
        else
            console.error(`No se ha localizado el elemento con id  ${idSeleccionado}`);
    }

    public QuitarTodos(): void {
        this.elementos.splice(0, this.elementos.length);
    }

    public ToString(): string {
        let ids: string = "";
        for (var i = 0; i < this.elementos.length; i++) {
            ids = ids + this.elementos[i].Id;
            if (i < this.elementos.length - 1)
                ids = ids + ';';
        }
        return ids;
    }

    public SincronizarCheck(): void {
        this.deshabilitarCheck(true);
    }

}

