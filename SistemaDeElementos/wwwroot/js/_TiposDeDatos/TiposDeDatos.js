var Tipos;
(function (Tipos) {
    class Orden {
        constructor(idcolumna, propiedad, modo, ordenarPor) {
            this.Modo = modo;
            this.Propiedad = propiedad;
            this.IdColumna = idcolumna;
            this.ccsClase = modo;
            this._ordenarPor = ordenarPor;
        }
        get ccsClase() {
            return this._cssClase;
        }
        get OrdenarPor() {
            if (IsNullOrEmpty(this._ordenarPor))
                return this.Propiedad;
            return this._ordenarPor;
        }
        set ccsClase(modo) {
            if (modo === ModoOrdenacion.ascedente)
                this._cssClase = ClaseCss.ordenAscendente;
            else if (modo === ModoOrdenacion.descendente)
                this._cssClase = ClaseCss.ordenDescendente;
            else if (modo === ModoOrdenacion.sinOrden)
                this._cssClase = ClaseCss.sinOrden;
        }
    }
    Tipos.Orden = Orden;
    class Ordenacion {
        constructor() {
            this.lista = new Array();
        }
        Count() {
            return this.lista.length;
        }
        Anadir(idcolumna, propiedad, modo, ordenarPor) {
            for (let i = 0; i < this.lista.length; i++) {
                if (this.lista[i].Propiedad === propiedad) {
                    this.lista[i].Modo = modo;
                    this.lista[i].ccsClase = modo;
                    ApiControl.AjustarColumnaDelGrid(this.lista[i]);
                    return;
                }
            }
            let orden = new Tipos.Orden(idcolumna, propiedad, modo, ordenarPor);
            this.lista.push(orden);
            ApiControl.AjustarColumnaDelGrid(orden);
        }
        Quitar(propiedad) {
            for (let i = 0; i < this.lista.length; i++) {
                if (this.lista[i].Propiedad == propiedad) {
                    this.lista[i].Modo = ModoOrdenacion.sinOrden;
                    this.lista[i].ccsClase = ModoOrdenacion.sinOrden;
                    let orden = this.lista[i];
                    ApiControl.AjustarColumnaDelGrid(orden);
                    this.lista.splice(i, 1);
                    return;
                }
            }
        }
        Actualizar(idcolumna, propiedad, modo, ordenarPor) {
            if (modo === ModoOrdenacion.sinOrden)
                this.Quitar(propiedad);
            else
                this.Anadir(idcolumna, propiedad, modo, ordenarPor);
        }
        Leer(i) {
            return this.lista[i];
        }
    }
    Tipos.Ordenacion = Ordenacion;
    class DatosRestrictor {
        constructor(propiedad, valor, texto) {
            this.Propiedad = propiedad;
            this.Valor = valor;
            this.Texto = texto;
        }
    }
    Tipos.DatosRestrictor = DatosRestrictor;
    class ListaDeElemento {
        constructor(idLista) {
            this.lista = document.getElementById(idLista);
        }
        get Lista() {
            return this.lista;
        }
        AgregarOpcion(valor, texto) {
            var opcion = document.createElement("option");
            opcion.setAttribute("value", valor.toString());
            opcion.setAttribute("label", texto);
            this.Lista.appendChild(opcion);
        }
    }
    Tipos.ListaDeElemento = ListaDeElemento;
    class DatosPeticionLista {
        get Selector() {
            return new ListaDeElemento(this.IdLista);
        }
    }
    Tipos.DatosPeticionLista = DatosPeticionLista;
    class ListaDinamica {
        constructor(input) {
            this._IdLista = input.getAttribute(atListas.idDeLaLista);
        }
        get Input() {
            return document.querySelector(`input[list="${this._IdLista}"]`);
        }
        get Lista() {
            return document.getElementById(this._IdLista);
        }
        AgregarOpcion(valor, texto) {
            for (var i = 0; i < this.Lista.children.length; i++)
                if (Numero(this.Lista.children[i].getAttribute(atListas.identificador)) === valor)
                    return;
            let opcion = document.createElement("option");
            opcion.setAttribute(atListas.identificador, valor.toString());
            opcion.value = texto;
            this.Lista.appendChild(opcion);
        }
        BuscarSeleccionado(valor) {
            for (var i = 0; i < this.Lista.children.length; i++) {
                if (this.Lista.children[i] instanceof HTMLOptionElement) {
                    let opcion = this.Lista.children[i];
                    if (opcion.value === valor)
                        return Numero(opcion.getAttribute(atListas.identificador));
                }
            }
            return 0;
        }
        Borrar() {
            this.Input.value = "";
            this.Lista.innerHTML = "";
        }
    }
    Tipos.ListaDinamica = ListaDinamica;
    class DatosPeticionDinamica {
    }
    Tipos.DatosPeticionDinamica = DatosPeticionDinamica;
    class DatosParaRelacionar {
        constructor() {
            this.FiltroRestrictor = null;
        }
    }
    Tipos.DatosParaRelacionar = DatosParaRelacionar;
    class DatosParaDependencias {
        constructor() {
            this.FiltroRestrictor = null;
        }
    }
    Tipos.DatosParaDependencias = DatosParaDependencias;
})(Tipos || (Tipos = {}));
//# sourceMappingURL=TiposDeDatos.js.map