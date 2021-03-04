using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Callejero;
using GestoresDeNegocio.Callejero;
using ModeloDeDto.Callejero;
using AutoMapper;
using System.IO;
using Utilidades;
using System;
using Microsoft.AspNetCore.Http;

namespace MVCSistemaDeElementos.Controllers
{
    public class ImportarCallejeroController : FormularioController<ContextoSe>
    {

        public ImportarCallejeroController(ContextoSe contexto, IMapper mapeador, GestorDeErrores gestorDeErrores)
         : base(contexto
               , mapeador
               , new DescriptorImportarCallejero()
               , gestorDeErrores)
        {
        }


        public IActionResult ImportarCallejero()
        {
            return ViewFormulario();
        }

        // END-POIN: desde el Callejero.ImportarCallejero.ts. somete la importación de ficheros csv
        public JsonResult epImportarCallejero(string parametros)
        {
            var r = new Resultado();

            try
            {
                ApiController.CumplimentarDatosDeUsuarioDeConexion(Contexto, Mapeador, HttpContext);
                GestorDePaises.SometerImportarCallejero(Contexto, parametros);
                r.Estado = enumEstadoPeticion.Ok;
                r.Mensaje = "Trabajo de importación sometido correctamente";
            }
            catch(Exception e)
            {
                r.Estado = enumEstadoPeticion.Error;
                r.consola = e.Message;
                r.Mensaje = "Error al someter el trabajo de importación del callejero";
            }

            return new JsonResult(r);
        }

    }
}
