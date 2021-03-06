var ApiGrid;
(function (ApiGrid) {
    class PropiedadesDeLaFila {
        constructor() {
        }
    }
    ApiGrid.PropiedadesDeLaFila = PropiedadesDeLaFila;
    function ObtenerDescriptorDeLaCabecera(tabla) {
        let filaCabecera = new Array();
        var cabecera = tabla.rows[0];
        var ths = cabecera.querySelectorAll('th');
        for (let i = 0; i < ths.length; i++) {
            let p = new PropiedadesDeLaFila();
            p.id = ths[i].id;
            p.visible = !ths[i].hidden;
            p.claseCss = ths[i].className;
            p.estilo = ths[i].style;
            p.anchoEnPixel = ths[i].getBoundingClientRect().width;
            p.editable = false;
            p.propiedad = ths[i].getAttribute('propiedad');
            filaCabecera.push(p);
        }
        return filaCabecera;
    }
    ApiGrid.ObtenerDescriptorDeLaCabecera = ObtenerDescriptorDeLaCabecera;
    function ColumnaVisible(tabla, idColumna) {
        let columna = document.getElementById(idColumna);
        hacerVisible(tabla, columna);
    }
    ApiGrid.ColumnaVisible = ColumnaVisible;
    function ColumnaInvisible(tabla, idColumna) {
        let columna = document.getElementById(idColumna);
        hacerInvisible(tabla, columna);
    }
    ApiGrid.ColumnaInvisible = ColumnaInvisible;
    function OcultarMostrarColumna(tabla, propiedad) {
        var cabecera = tabla.rows[0];
        var ths = cabecera.querySelectorAll('th');
        for (let i = 0; i < ths.length; i++) {
            if (ths[i].getAttribute(atControl.propiedad) === propiedad.toLocaleLowerCase()) {
                let columna = ths[i];
                if (estaOculta(columna))
                    hacerVisible(tabla, columna);
                else
                    hacerInvisible(tabla, columna);
            }
        }
    }
    ApiGrid.OcultarMostrarColumna = OcultarMostrarColumna;
    function RecalcularAnchoColumnas(tabla) {
        recalcularPorcentajes(tabla);
        let cuerpo = tabla.tBodies[0];
        aplicarPorcentajes(cuerpo);
    }
    ApiGrid.RecalcularAnchoColumnas = RecalcularAnchoColumnas;
    function estaOculta(columna) {
        return columna.classList.contains('columna-oculta');
    }
    function hacerVisible(tabla, columna) {
        columna.classList.remove('columna-oculta');
        columna.classList.add('columna-cabecera');
        let cuerpoDeLaTabla = tabla.tBodies[0];
        var tds = cuerpoDeLaTabla.querySelectorAll('td');
        for (let i = 0; i < tds.length; i++) {
            if (tds[i].headers === columna.id) {
                tds[i].classList.remove('columna-oculta');
                tds[i].classList.add('columna-cabecera');
            }
        }
    }
    function hacerInvisible(tabla, columna) {
        columna.classList.add('columna-oculta');
        columna.classList.remove('columna-cabecera');
        let cuerpoDeLaTabla = tabla.tBodies[0];
        var tds = cuerpoDeLaTabla.querySelectorAll('td');
        for (let i = 0; i < tds.length; i++) {
            if (tds[i].headers === columna.id) {
                tds[i].classList.add('columna-oculta');
                tds[i].classList.remove('columna-cabecera');
            }
        }
    }
    function aplicarPorcentajes(cuerpoDeLaTabla) {
        var tds = cuerpoDeLaTabla.querySelectorAll('td');
        for (let i = 0; i < tds.length; i++) {
            if (!tds[i].classList.contains('columna-oculta')) {
                let idCabecera = tds[i].headers;
                let cabecera = document.getElementById(idCabecera);
                tds[i].style.width = cabecera.style.width;
            }
        }
    }
    function recalcularPorcentajes(tabla) {
        var cabecera = tabla.rows[0];
        var ths = cabecera.querySelectorAll('th');
        let sumaDeLoQueHay = 0;
        for (let i = 0; i < ths.length; i++) {
            if (!ths[i].classList.contains('columna-oculta')) {
                sumaDeLoQueHay = sumaDeLoQueHay + Numero(ths[i].style.width.replace('%', ''));
            }
        }
        let loQueHay = 0;
        let loQueDebeSer = 0;
        for (let i = 0; i < ths.length; i++) {
            if (!ths[i].hidden) {
                loQueHay = Numero(ths[i].style.width.replace('%', ''));
                loQueDebeSer = (loQueHay * 99) / sumaDeLoQueHay;
                ths[i].style.width = `${loQueDebeSer}%`;
            }
        }
    }
})(ApiGrid || (ApiGrid = {}));
//# sourceMappingURL=ApiDeGrid.js.map