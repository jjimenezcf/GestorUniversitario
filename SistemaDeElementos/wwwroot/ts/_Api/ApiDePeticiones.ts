namespace ApiDePeticiones {


    export class DatosPeticionSubirArchivo {
        private _idArchivo: string;

        public Archivo(): HTMLInputElement {
            return document.getElementById(this._idArchivo) as HTMLInputElement;
        }

        constructor(idArchivo: string) {
            this._idArchivo = idArchivo;
        }
    }


    export function LeerModoDeAccesoAlNegocio(llamador: any, controlador: string, negocio: string): Promise<ApiDeAjax.DescriptorAjax> {

        return new Promise((resolve, reject) => {

            let url: string = DefinirPeticionDeLeerModoDeAccesoAlNegocio(controlador, negocio);
            let datosEntrada: any = { "cotrolador": controlador, "negocio": negocio };
            let a = new ApiDeAjax.DescriptorAjax(llamador
                , Ajax.EndPoint.LeerModoDeAccesoAlNegocio
                , datosEntrada
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , (peticion) => {
                    resolve(peticion);
                }
                , (peticion) => {
                    reject(peticion);
                }
            );
            a.Ejecutar();
        });
    }

    function DefinirPeticionDeLeerModoDeAccesoAlNegocio(controlador: string, negocio: string): string {
        let url: string = `/${controlador}/${Ajax.EndPoint.LeerModoDeAccesoAlNegocio}`;
        let parametros: string = `${Ajax.Param.negocio}=${negocio}`;
        let peticion: string = url + '?' + parametros;
        return peticion;
    }


    export function LeerElementoPorId(llamador: any, controlador: string, id: number, parametros: Array<Parametro>): Promise<ApiDeAjax.DescriptorAjax> {

        return new Promise((resolve, reject) => {

            let url: string = `/${controlador}/${Ajax.EndPoint.LeerPorId}?${Ajax.Param.id}=${id}&${Ajax.Param.parametros}=${JSON.stringify(parametros)}`;

            let a = new ApiDeAjax.DescriptorAjax(llamador
                , Ajax.EndPoint.LeerPorId
                , null
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , (peticion) => {
                    resolve(peticion);
                }
                , (peticion) => {
                    reject(peticion);
                }
            );

            a.Ejecutar();
        });
    }

    export function LeerElemento(llamador: any, controlador: string, filtros: Array<ClausulaDeFiltrado>, parametros: Array<Parametro>): Promise<ApiDeAjax.DescriptorAjax> {

        return new Promise((resolve, reject) => {

            let url: string = `/${controlador}/${Ajax.EndPoint.LeerElemento}?${Ajax.Param.filtros}=${JSON.stringify(filtros)}&${Ajax.Param.parametros}=${JSON.stringify(parametros)}`;

            let a = new ApiDeAjax.DescriptorAjax(llamador
                , Ajax.EndPoint.LeerElemento
                , null
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , (peticion) => {
                    resolve(peticion);
                }
                , (peticion) => {
                    reject(peticion);
                }
            );

            a.Ejecutar();
        });
    }

    export function LeerElementos(llamador: any, controlador: string, filtros: Array<ClausulaDeFiltrado>, parametros: Array<Parametro>, datosDeEntrada: string): Promise<ApiDeAjax.DescriptorAjax> {

        return new Promise((resolve, reject) => {

            let url: string = `/${controlador}/${Ajax.EndPoint.LeerElementos}?${Ajax.Param.filtros}=${JSON.stringify(filtros)}&${Ajax.Param.parametros}=${JSON.stringify(parametros)}`;

            let a = new ApiDeAjax.DescriptorAjax(llamador
                , Ajax.EndPoint.LeerElementos
                , datosDeEntrada
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , (peticion) => {
                    resolve(peticion);
                }
                , (peticion) => {
                    reject(peticion);
                }
            );

            a.Ejecutar();
        });
    }



    export function CargaDinamica(llamador: any, input: HTMLInputElement, filtros: Array<ClausulaDeFiltrado>): Promise<ApiDeAjax.DescriptorAjax> {

        return new Promise((resolve, reject) => {

            let clase: string = input.getAttribute(atListasDinamicas.claseElemento);
            let idInput: string = input.getAttribute('id');
            let cantidad: string = input.getAttribute(atListasDinamicas.cantidad);
            let url: string = DefinirPeticionDeCargarDinamica(llamador.Controlador, clase, Numero(cantidad), filtros);
            let datosDeEntrada = `{"ClaseDeElemento":"${clase}", "IdInput":"${idInput}", "buscada":"${input.value}"}`;
            let a = new ApiDeAjax.DescriptorAjax(llamador
                , Ajax.EndPoint.CargaDinamica
                , datosDeEntrada
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , (peticion) => {
                    resolve(peticion);
                }
                , (peticion) => {
                    reject(peticion);
                }
            );
            input.setAttribute(atListasDinamicas.cargando, 'S');
            a.Ejecutar();
        });
    }

    function DefinirPeticionDeCargarDinamica(controlador: string, claseElemento: string, cantidad: number, filtros: Array<ClausulaDeFiltrado>): string {
        let url: string = `/${controlador}/${Ajax.EndPoint.CargaDinamica}?${Ajax.Param.claseElemento}=${claseElemento}&posicion=0&cantidad=${cantidad}&filtrosJson=${JSON.stringify(filtros)}`;
        return url;
    }


    export function Exportar(llamador: any, controlador: string, parametros: Array<Parametro>): Promise<ApiDeAjax.DescriptorAjax> {

        return new Promise((resolve, reject) => {

            let url: string = `/${controlador}/${Ajax.EndPoint.Exportar}?${Ajax.Param.parametros}=${Encriptar(literal.ClaveDeEncriptacion, JSON.stringify(parametros))}`;

            let a = new ApiDeAjax.DescriptorAjax(llamador
                , Ajax.EndPoint.Exportar
                , parametros
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , (peticion) => {
                    resolve(peticion);
                }
                , (peticion) => {
                    reject(peticion);
                }
            );

            a.Ejecutar();
        });
    }

    export function EnviarCorreo(llamador: any, controlador: string, parametros: Array<Parametro>): Promise<ApiDeAjax.DescriptorAjax> {

        return new Promise((resolve, reject) => {

            let url: string = `/${controlador}/${Ajax.EndPoint.EnviarCorreo}?${Ajax.Param.parametros}=${Encriptar(literal.ClaveDeEncriptacion, JSON.stringify(parametros))}`;

            let a = new ApiDeAjax.DescriptorAjax(llamador
                , Ajax.EndPoint.EnviarCorreo
                , parametros
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Get
                , (peticion) => {
                    resolve(peticion);
                }
                , (peticion) => {
                    reject(peticion);
                }
            );

            a.Ejecutar();
        });
    }

}