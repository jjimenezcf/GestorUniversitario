var Tipos;
(function (Tipos) {
    class Orden {
        constructor(idcolumna, propiedad, modo, ordenarPor) {
            this.Modo = modo;
            this.Propiedad = propiedad;
            this.IdColumna = idcolumna;
            this._ordenarPor = ordenarPor;
        }
        get ccsClase() {
            if (this._modo === ModoOrdenacion.ascedente)
                return ClaseCss.ordenAscendente;
            if (this._modo === ModoOrdenacion.descendente)
                return ClaseCss.ordenDescendente;
            return ClaseCss.sinOrden;
        }
        get OrdenarPor() {
            if (IsNullOrEmpty(this._ordenarPor))
                return this.Propiedad;
            return this._ordenarPor;
        }
        get Modo() {
            return this._modo;
        }
        set Modo(modo) {
            this._modo = modo;
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
            let orden = new Tipos.Orden(idcolumna, propiedad, modo, ordenarPor);
            if (ApiControl.AjustarColumnaDelGrid(orden)) {
                this.lista.push(orden);
                return true;
            }
            return false;
        }
        Quitar(propiedad) {
            for (let i = 0; i < this.lista.length; i++) {
                if (this.lista[i].Propiedad == propiedad) {
                    let orden = this.lista[i];
                    orden.Modo = ModoOrdenacion.sinOrden;
                    this.lista.splice(i, 1);
                    return ApiControl.AjustarColumnaDelGrid(orden);
                }
            }
        }
        AnularOrdenacion() {
            for (let i = this.lista.length - 1; i >= 0; i--) {
                let orden = this.lista[i];
                orden.Modo = ModoOrdenacion.sinOrden;
                ApiControl.AjustarColumnaDelGrid(orden);
                this.lista.splice(i, 1);
            }
        }
        Actualizar(idcolumna, propiedad, modo, ordenarPor) {
            if (modo === ModoOrdenacion.sinOrden)
                return this.Quitar(propiedad);
            else
                return this.Anadir(idcolumna, propiedad, modo, ordenarPor);
        }
        Leer(i) {
            return this.lista[i];
        }
        LeerPorPropiedad(propiedad) {
            for (let i = 0; i < this.lista.length; i++) {
                if (this.lista[i].Propiedad == propiedad)
                    return this.lista[i];
            }
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
            this.FiltroRestrictor = new Array();
        }
    }
    Tipos.DatosParaDependencias = DatosParaDependencias;
})(Tipos || (Tipos = {}));
//# sourceMappingURL=TiposDeDatos.js.map