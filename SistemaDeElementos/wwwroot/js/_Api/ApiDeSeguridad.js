var ApiDeSeguridad;
(function (ApiDeSeguridad) {
    class DatosPeticionSubirArchivo {
        constructor(idArchivo) {
            this._idArchivo = idArchivo;
        }
        Archivo() {
            return document.getElementById(this._idArchivo);
        }
    }
    ApiDeSeguridad.DatosPeticionSubirArchivo = DatosPeticionSubirArchivo;
    function LeerModoDeAccesoAlNegocio(llamador, controlador, negocio) {
        return new Promise((resolve, reject) => {
            let url = DefinirPeticionDeLeerModoDeAccesoAlNegocio(controlador, negocio);
            let datosEntrada = { "cotrolador": controlador, "negocio": negocio };
            let a = new ApiDeAjax.DescriptorAjax(llamador, Ajax.EndPoint.SubirArchivo, datosEntrada, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Post, (peticion) => {
                resolve(peticion);
            }, (peticion) => {
                reject(peticion);
            });
            a.Ejecutar();
        });
    }
    ApiDeSeguridad.LeerModoDeAccesoAlNegocio = LeerModoDeAccesoAlNegocio;
    function DefinirPeticionDeLeerModoDeAccesoAlNegocio(controlador, negocio) {
        let url = `/${controlador}/${Ajax.EndPoint.LeerModoDeAccesoAlNegocio}`;
        let parametros = `${Ajax.Param.negocio}=${negocio}`;
        let peticion = url + '?' + parametros;
        return peticion;
    }
})(ApiDeSeguridad || (ApiDeSeguridad = {}));
//# sourceMappingURL=ApiDeSeguridad.js.map