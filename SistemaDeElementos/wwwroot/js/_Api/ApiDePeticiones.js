var ApiDePeticiones;
(function (ApiDePeticiones) {
    class DatosPeticionSubirArchivo {
        constructor(idArchivo) {
            this._idArchivo = idArchivo;
        }
        Archivo() {
            return document.getElementById(this._idArchivo);
        }
    }
    ApiDePeticiones.DatosPeticionSubirArchivo = DatosPeticionSubirArchivo;
    function LeerModoDeAccesoAlNegocio(llamador, controlador, negocio) {
        return new Promise((resolve, reject) => {
            let url = DefinirPeticionDeLeerModoDeAccesoAlNegocio(controlador, negocio);
            let datosEntrada = { "cotrolador": controlador, "negocio": negocio };
            let a = new ApiDeAjax.DescriptorAjax(llamador, Ajax.EndPoint.LeerModoDeAccesoAlNegocio, datosEntrada, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, (peticion) => {
                resolve(peticion);
            }, (peticion) => {
                reject(peticion);
            });
            a.Ejecutar();
        });
    }
    ApiDePeticiones.LeerModoDeAccesoAlNegocio = LeerModoDeAccesoAlNegocio;
    function DefinirPeticionDeLeerModoDeAccesoAlNegocio(controlador, negocio) {
        let url = `/${controlador}/${Ajax.EndPoint.LeerModoDeAccesoAlNegocio}`;
        let parametros = `${Ajax.Param.negocio}=${negocio}`;
        let peticion = url + '?' + parametros;
        return peticion;
    }
    function LeerElementoPorId(llamador, controlador, id, parametros) {
        return new Promise((resolve, reject) => {
            let url = `/${controlador}/${Ajax.EndPoint.LeerPorId}?${Ajax.Param.id}=${id}&${Ajax.Param.parametros}=${JSON.stringify(parametros)}`;
            let a = new ApiDeAjax.DescriptorAjax(llamador, Ajax.EndPoint.LeerPorId, null, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, (peticion) => {
                resolve(peticion);
            }, (peticion) => {
                reject(peticion);
            });
            a.Ejecutar();
        });
    }
    ApiDePeticiones.LeerElementoPorId = LeerElementoPorId;
    function Exportar(llamador, controlador, parametros) {
        return new Promise((resolve, reject) => {
            let url = `/${controlador}/${Ajax.EndPoint.Exportar}?${Ajax.Param.parametros}=${JSON.stringify(parametros)}`;
            let a = new ApiDeAjax.DescriptorAjax(llamador, Ajax.EndPoint.Exportar, parametros, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, (peticion) => {
                resolve(peticion);
            }, (peticion) => {
                reject(peticion);
            });
            a.Ejecutar();
        });
    }
    ApiDePeticiones.Exportar = Exportar;
})(ApiDePeticiones || (ApiDePeticiones = {}));
//# sourceMappingURL=ApiDePeticiones.js.map