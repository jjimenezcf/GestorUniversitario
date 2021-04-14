﻿namespace ApiDePeticiones {


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
                , Ajax.EndPoint.SubirArchivo
                , datosEntrada
                , url
                , ApiDeAjax.TipoPeticion.Asincrona
                , ApiDeAjax.ModoPeticion.Post
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

}