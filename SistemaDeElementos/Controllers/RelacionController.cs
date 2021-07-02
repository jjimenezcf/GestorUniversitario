using Microsoft.AspNetCore.Mvc;
using System;
using Gestor.Errores;
using GestorDeElementos;
using System.Collections.Generic;
using Newtonsoft.Json;
using ServicioDeDatos;
using ServicioDeDatos.Elemento;
using ModeloDeDto;

namespace MVCSistemaDeElementos.Controllers
{
    public class RelacionController<TContexto, TRelacion, TElemento> : EntidadController<TContexto, TRelacion, TElemento>
        where TContexto : ContextoSe
        where TRelacion : RegistroDeRelacion
        where TElemento : ElementoDto
    {

        protected GestorDeRelaciones<TContexto, TRelacion, TElemento> GestorDeRelaciones => (GestorDeRelaciones<TContexto,TRelacion,TElemento>)GestorDeElementos;

        public RelacionController(GestorDeRelaciones<TContexto, TRelacion, TElemento> gestorDeRelaciones, GestorDeErrores gestorErrores)
        : base(gestorDeRelaciones, gestorErrores)
        {
        }

        //END-POINT: Desde ModalParaRelacionar.ts
        /// <summary>
        /// crea las relaciones entre el id de un elemento pasado y la lista de id's de otros elementos
        /// </summary>
        /// <param name="id">id del elemento pasado</param>
        /// <param name="idsJson">lista de ids en formato json de los ids con los que relacionar</param>
        /// <returns></returns>
        public JsonResult epCrearRelaciones(string propiedadId, int id, string idsJson)
        {
            var r = new Resultado();
            try
            {
                ApiController.CumplimentarDatosDeUsuarioDeConexion(GestorDeElementos.Contexto, GestorDeElementos.Mapeador, HttpContext);
                List<int> listaIds = JsonConvert.DeserializeObject<List<int>>(idsJson);
                var relacionados = 0;
                var mensajeInformativo = "";
                foreach (var idParaRelacionar in listaIds)
                {
                    if (!GestorDeRelaciones.CrearRelacion(propiedadId, id, idParaRelacionar, false).existe)
                        relacionados++;
                    else
                        mensajeInformativo = mensajeInformativo + Environment.NewLine + $"Existe la relación {id} con {idParaRelacionar}";
                }
                r.Total = relacionados;
                r.consola = mensajeInformativo;
                r.Mensaje = $"Se han relacionado {relacionados} de los {listaIds.Count} marcados";
                r.Estado = enumEstadoPeticion.Ok;
            }
            catch (Exception e)
            {
                ApiController.PrepararError(e, r, "Error en el proceso de relación.");
            }

            return new JsonResult(r);
        }

    }
}
