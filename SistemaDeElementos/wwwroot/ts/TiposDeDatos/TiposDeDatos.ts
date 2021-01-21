namespace Tipos {

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

}