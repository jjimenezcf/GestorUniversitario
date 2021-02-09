using System;
using System.IO;
using AutoMapper;
using Gestor.Errores;
using GestorDeElementos;
using GestoresDeNegocio.Entorno;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModeloDeDto.Entorno;
using ServicioDeDatos;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.Seguridad;
using Utilidades;

namespace MVCSistemaDeElementos.Controllers
{
    public enum enumEstadoPeticion { Ok, Error }

    public class Resultado
    {
        public enumEstadoPeticion Estado { get; set; }
        public string Mensaje { get; set; }
        public string consola { get; set; }
        public int Total { get; set; } = 0;
        public dynamic Datos { get; set; }
        public string ModoDeAcceso { get; set; }
    }

    public class ResultadoHtml : Resultado
    {
        public string Html { get; set; }
    }

    public class BaseController : Controller
    {
        protected GestorDeErrores GestorDeErrores { get; }
        public ILogger Logger { get; set; }
        protected DatosDeConexion DatosDeConexion { get; private set; }

        public BaseController(GestorDeErrores gestorDeErrores, DatosDeConexion datosDeConexion)
        {
            GestorDeErrores = gestorDeErrores;
            DatosDeConexion = datosDeConexion;
        }

        protected ViewResult RenderMensaje(string mensaje)
        {
            ViewBag.Mensaje = mensaje;
            ViewBag.DatosDeConexion = DatosDeConexion;
            return View(nameof(RenderMensaje));
        }


        public static JsonResult SubirArchivo(ContextoSe contexto, IMapper mapeador, HttpContext httpContext, IFormFile fichero, string rutaDestino, string extensionesValidas)
        {
            var r = new Resultado();

            try
            {
                if (fichero == null)
                    GestorDeErrores.Emitir("No se ha identificado el fichero");

                ApiController.CumplimentarDatosDeUsuarioDeConexion(contexto, mapeador, httpContext);
                ValidarExtension(fichero, extensionesValidas);
                var rutaBase = @"..\SistemaDeElementos\wwwroot";
                var rutaDeDescarga = $@"{rutaBase}\Archivos";
                var rutaConFichero = $@"{rutaDeDescarga}\{fichero.FileName}";

                using (var stream = new FileStream(rutaConFichero, FileMode.Create))
                {
                    fichero.CopyTo(stream);
                }

                if (rutaDestino.IsNullOrEmpty())
                {
                    r.Datos = GestoresDeNegocio.Archivos.GestorDocumental.SubirArchivo(contexto, rutaConFichero, mapeador);
                }
                else
                {
                    rutaDestino = $@"{rutaBase}{rutaDestino.Replace("/", @"\")}";

                    if (!Directory.Exists(rutaDestino))
                        Directory.CreateDirectory(rutaDestino);
                    int numero = 1;
                    var ficheroSinExtension = Path.GetFileNameWithoutExtension(fichero.FileName).Replace(" ","_");
                    var extension = Path.GetExtension(fichero.FileName);
                    while (System.IO.File.Exists($@"{rutaDestino}\{ficheroSinExtension}{extension}"))
                    {
                        if (numero == 1)
                            ficheroSinExtension = $"{ficheroSinExtension}_{numero}";
                        else
                            ficheroSinExtension = ficheroSinExtension.Replace($"_{numero - 1}", $"_{numero}");
                        numero++;
                    }

                    System.IO.File.Move(rutaConFichero, $@"{rutaDestino}\{ficheroSinExtension}{extension}");

                    r.Datos = $@"{ficheroSinExtension}{extension}";
                }

                r.Estado = enumEstadoPeticion.Ok;
                r.Mensaje = "fichero subido";
            }
            catch (Exception e)
            {
                r.Estado = enumEstadoPeticion.Error;
                r.consola = GestorDeErrores.Concatenar(e);
                r.Mensaje = $"No se ha podido subir el fichero. {(e.Data.Contains(GestorDeErrores.Datos.Mostrar) && (bool)e.Data[GestorDeErrores.Datos.Mostrar] == true ? e.Message : "")}";
            }


            return new JsonResult(r);

        }


        private static void ValidarExtension(IFormFile fichero, string extensiones)
        {
            if (extensiones.IsNullOrEmpty() || extensiones.EndsWith("*"))
                return;

            if (EsImagen(fichero) && (extensiones.Contains("png")
                                   || extensiones.Contains("jpg")
                                   || extensiones.Contains("svg")
                                   ))
                return;

            if (EsCsv(fichero) && extensiones.Contains("csv"))
                return;

            throw new Exception($"Para el tipo de fichero {fichero.ContentType} sólo se aceptan '{extensiones}'");

        }

        private static bool EsCsv(IFormFile fichero)
        {
            return fichero.ContentType.Contains("csv")
                || fichero.ContentType == "application/vnd.ms-excel";
        }

        private static bool EsImagen(IFormFile fichero)
        {
            return fichero.ContentType == "image/jpeg"
                || fichero.ContentType == "image/png"
                || fichero.ContentType == "image/gif"
                || fichero.ContentType == "image/jpg"
                || fichero.ContentType == "image/vnd.Microsoft.icon"
                || fichero.ContentType == "image/x-icon"
                || fichero.ContentType == "image/vnd.djvu"
                || fichero.ContentType == "image/svg+xml";
        }

    }
}
