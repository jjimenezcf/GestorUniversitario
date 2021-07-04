namespace Tipos {


    export class Orden {
        public IdColumna: string;
        public Propiedad: string;
        private _modo: string;
        private _ordenarPor: string;

        get ccsClase(): string {
            if (this._modo === ModoOrdenacion.ascedente)
                return ClaseCss.ordenAscendente;
            if (this._modo === ModoOrdenacion.descendente)
                return ClaseCss.ordenDescendente;
            return ClaseCss.sinOrden;
        }

        get OrdenarPor(): string {
            if (IsNullOrEmpty(this._ordenarPor))
                return this.Propiedad;
            return this._ordenarPor;
        }

        get Modo(): string {
            return this._modo;
        }

        set Modo(modo: string) {
            this._modo = modo;
        }

        constructor(idcolumna: string, propiedad: string, modo: string, ordenarPor: string) {
            this.Modo = modo;
            this.Propiedad = propiedad;
            this.IdColumna = idcolumna;
            this._ordenarPor = ordenarPor;
        }
    }


    export class Ordenacion {
        private lista: Array<Tipos.Orden>;

        public Count(): number {
            return this.lista.length;
        }

        constructor() {
            this.lista = new Array<Tipos.Orden>();
        }

        private Anadir(idcolumna: string, propiedad: string, modo: string, ordenarPor: string): boolean {
            for (let i = 0; i < this.lista.length; i++) {
                if (this.lista[i].Propiedad === propiedad) {
                    this.lista[i].Modo = modo;
                    //if (modo = ModoOrdenacion.ascedente)
                    //    this.lista[i].ccsClase = ClaseCss.ordenAscendente;
                    //else
                    //    if (modo = ModoOrdenacion.descendente)
                    //        this.lista[i].ccsClase = ClaseCss.ordenDescendente;
                    //    else
                    //        this.lista[i].ccsClase = ClaseCss.sinOrden;
                    return ApiControl.AjustarColumnaDelGrid(this.lista[i]);
                }
            }
            let orden: Tipos.Orden = new Tipos.Orden(idcolumna, propiedad, modo, ordenarPor);

            if (ApiControl.AjustarColumnaDelGrid(orden)) {
                this.lista.push(orden);
                return true;
            }
            return false;
        }

        private Quitar(propiedad: string): boolean {
            for (let i = 0; i < this.lista.length; i++) {
                if (this.lista[i].Propiedad == propiedad) {
                    let orden: Tipos.Orden = this.lista[i] as Tipos.Orden;
                    orden.Modo = ModoOrdenacion.sinOrden;
                    this.lista.splice(i, 1);
                    return ApiControl.AjustarColumnaDelGrid(orden);
                }
            }
        }

        public AnularOrdenacion(): void {
            for (let i = this.lista.length-1; i>= 0  ; i--) {
                let orden: Tipos.Orden = this.lista[i] as Tipos.Orden;
                orden.Modo = ModoOrdenacion.sinOrden;
                ApiControl.AjustarColumnaDelGrid(orden);
                this.lista.splice(i, 1);
            }

        }

        public Actualizar(idcolumna: string, propiedad: string, modo: string, ordenarPor: string): boolean {
            if (modo === ModoOrdenacion.sinOrden)
                return this.Quitar(propiedad);
            else
                return this.Anadir(idcolumna, propiedad, modo, ordenarPor);
        }

        public Leer(i: number): Tipos.Orden {
            return this.lista[i];
        }

        public LeerPorPropiedad(propiedad: string): Tipos.Orden {
            for (let i = 0; i < this.lista.length; i++) {
                if (this.lista[i].Propiedad == propiedad)
                    return this.lista[i];
            }
        }
    }



    export class DatosRestrictor {
        public Propiedad: string;
        public Valor: number;
        public Texto: string;

        constructor(propiedad: string, valor: number, texto: string) {
            this.Propiedad = propiedad;
            this.Valor = valor;
            this.Texto = texto;
        }
    }

    export class ListaDeElemento {
        private lista: HTMLSelectElement;

        get Lista(): HTMLSelectElement {
            return this.lista;
        }

        constructor(idLista: string) {
            this.lista = document.getElementById(idLista) as HTMLSelectElement;
        }

        public AgregarOpcion(valor: number, texto: string): void {

            var opcion = document.createElement("option");
            opcion.setAttribute("value", valor.toString());
            opcion.setAttribute("label", texto);
            this.Lista.appendChild(opcion);
        }
    }

    export class DatosPeticionLista {
        ClaseDeElemento: string;
        IdLista: string;

        get Selector(): ListaDeElemento {
            return new ListaDeElemento(this.IdLista);
        }
    }

    export class ListaDinamica {
        private _IdLista: string;

        get Input(): HTMLInputElement {
            return document.querySelector(`input[list="${this._IdLista}"]`);
        }

        get Lista(): HTMLDataListElement {
            return document.getElementById(this._IdLista) as HTMLDataListElement;
        }

        constructor(input: HTMLInputElement) {
            this._IdLista = input.getAttribute(atListas.idDeLaLista);
        }

        public AgregarOpcion(valor: number, texto: string): void {

            for (var i = 0; i < this.Lista.children.length; i++)
                if (Numero(this.Lista.children[i].getAttribute(atListas.identificador)) === valor)
                    return;

            let opcion: HTMLOptionElement = document.createElement("option");
            opcion.setAttribute(atListas.identificador, valor.toString());
            opcion.value = texto;

            this.Lista.appendChild(opcion);
        }

        public BuscarSeleccionado(valor: string): number {
            for (var i = 0; i < this.Lista.children.length; i++) {
                if (this.Lista.children[i] instanceof HTMLOptionElement) {
                    let opcion: HTMLOptionElement = this.Lista.children[i] as HTMLOptionElement;
                    if (opcion.value === valor)
                        return Numero(opcion.getAttribute(atListas.identificador));
                }
            }
            return 0;
        }

        public Borrar(): void {
            this.Input.value = "";
            this.Lista.innerHTML = "";
        }

    }

    export class DatosPeticionDinamica {
        public ClaseDeElemento: string;
        public IdInput: string;
        public buscada: string;
        public criterio: string;
    }

    export class DatosParaRelacionar {
        public idOpcionDeMenu: string;
        public RelacionarCon: string;
        public idSeleccionado: number;
        public PropiedadQueRestringe: string;
        public PropiedadRestrictora: string;
        public MostrarEnElRestrictor: string;
        public FiltroRestrictor: Tipos.DatosRestrictor;

        constructor() {
            this.FiltroRestrictor = null;
        }
    }

    export class DatosParaDependencias {
        public idOpcionDeMenu: string;
        public DatosDependientes: string;
        public idSeleccionado: number;
        public PropiedadQueRestringe: string;
        public PropiedadRestrictora: string;
        public MostrarEnElRestrictor: string;
        public FiltroRestrictor: Array<Tipos.DatosRestrictor>;

        constructor() {
            this.FiltroRestrictor = new Array<Tipos.DatosRestrictor>();
        }
    }

    export class TipoDtoElmento {
        TipoDto: string;
        IdElemento: number;
        Referencia: string;

        constructor(dto: string, id: number, texto: string) {
            this.TipoDto = dto;
            this.IdElemento = id;
            this.Referencia = texto;
        }
    }

}