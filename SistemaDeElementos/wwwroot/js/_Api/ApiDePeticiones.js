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
    function LeerElemento(llamador, controlador, filtros, parametros) {
        return new Promise((resolve, reject) => {
            let url = `/${controlador}/${Ajax.EndPoint.LeerElemento}?${Ajax.Param.filtros}=${JSON.stringify(filtros)}&${Ajax.Param.parametros}=${JSON.stringify(parametros)}`;
            let a = new ApiDeAjax.DescriptorAjax(llamador, Ajax.EndPoint.LeerElemento, null, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, (peticion) => {
                resolve(peticion);
            }, (peticion) => {
                reject(peticion);
            });
            a.Ejecutar();
        });
    }
    ApiDePeticiones.LeerElemento = LeerElemento;
    function LeerElementos(llamador, controlador, filtros, parametros, datosDeEntrada) {
        return new Promise((resolve, reject) => {
            let url = `/${controlador}/${Ajax.EndPoint.LeerElementos}?${Ajax.Param.filtros}=${JSON.stringify(filtros)}&${Ajax.Param.parametros}=${JSON.stringify(parametros)}`;
            let a = new ApiDeAjax.DescriptorAjax(llamador, Ajax.EndPoint.LeerElementos, datosDeEntrada, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, (peticion) => {
                resolve(peticion);
            }, (peticion) => {
                reject(peticion);
            });
            a.Ejecutar();
        });
    }
    ApiDePeticiones.LeerElementos = LeerElementos;
    function CargaDinamica(llamador, input, filtros) {
        return new Promise((resolve, reject) => {
            let clase = input.getAttribute(atListasDinamicas.claseElemento);
            let idInput = input.getAttribute('id');
            let cantidad = input.getAttribute(atListasDinamicas.cantidad);
            let url = DefinirPeticionDeCargarDinamica(llamador.Controlador, clase, Numero(cantidad), filtros);
            let datosDeEntrada = `{"ClaseDeElemento":"${clase}", "IdInput":"${idInput}", "buscada":"${input.value}"}`;
            let a = new ApiDeAjax.DescriptorAjax(llamador, Ajax.EndPoint.CargaDinamica, datosDeEntrada, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, (peticion) => {
                resolve(peticion);
            }, (peticion) => {
                reject(peticion);
            });
            input.setAttribute(atListasDinamicas.cargando, 'S');
            a.Ejecutar();
        });
    }
    ApiDePeticiones.CargaDinamica = CargaDinamica;
    function DefinirPeticionDeCargarDinamica(controlador, claseElemento, cantidad, filtros) {
        let url = `/${controlador}/${Ajax.EndPoint.CargaDinamica}?${Ajax.Param.claseElemento}=${claseElemento}&posicion=0&cantidad=${cantidad}&filtrosJson=${JSON.stringify(filtros)}`;
        return url;
    }
    function Exportar(llamador, controlador, parametros) {
        return new Promise((resolve, reject) => {
            let url = `/${controlador}/${Ajax.EndPoint.Exportar}?${Ajax.Param.parametros}=${Encriptar(literal.ClaveDeEncriptacion, JSON.stringify(parametros))}`;
            let a = new ApiDeAjax.DescriptorAjax(llamador, Ajax.EndPoint.Exportar, parametros, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, (peticion) => {
                resolve(peticion);
            }, (peticion) => {
                reject(peticion);
            });
            a.Ejecutar();
        });
    }
    ApiDePeticiones.Exportar = Exportar;
    function EnviarCorreo(llamador, controlador, parametros) {
        return new Promise((resolve, reject) => {
            let url = `/${controlador}/${Ajax.EndPoint.EnviarCorreo}?${Ajax.Param.parametros}=${Encriptar(literal.ClaveDeEncriptacion, JSON.stringify(parametros))}`;
            let a = new ApiDeAjax.DescriptorAjax(llamador, Ajax.EndPoint.EnviarCorreo, parametros, url, ApiDeAjax.TipoPeticion.Asincrona, ApiDeAjax.ModoPeticion.Get, (peticion) => {
                resolve(peticion);
            }, (peticion) => {
                reject(peticion);
            });
            a.Ejecutar();
        });
    }
    ApiDePeticiones.EnviarCorreo = EnviarCorreo;
})(ApiDePeticiones || (ApiDePeticiones = {}));
//# sourceMappingURL=ApiDePeticiones.js.map