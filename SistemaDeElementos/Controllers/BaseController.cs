using System;
using System.Collections.Generic;
using System.IO;
using AutoMapper;
using Enumerados;
using Gestor.Errores;
using GestorDeElementos;
using GestoresDeNegocio.Archivos;
using GestoresDeNegocio.Entorno;
using GestoresDeNegocio.Negocio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModeloDeDto;
using ModeloDeDto.Entorno;
using MVCSistemaDeElementos.Descriptores;
using Newtonsoft.Json;
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

    public class ResultadoDeLectura
    {
        public List<Dictionary<string, object>> registros { get; set; }
        public int posicion;
        public int cantidad;
        public int total { get; set; }
    }


    public class BaseController<TElemento> : Controller
        where TElemento : ElementoDto
    {
        protected GestorDeErrores GestorDeErrores { get; }
        public ILogger Logger { get; set; }
        protected DatosDeConexion DatosDeConexion => Contexto.DatosDeConexion;
        public IMapper Mapeador => Contexto.Mapeador;

        protected ContextoSe Contexto { get; private set; }

        protected DescriptorDeCrud<TElemento> Descriptor { get; set; }

        public BaseController(GestorDeErrores gestorDeErrores, ContextoSe contexto, IMapper mapeador)
        {
            GestorDeErrores = gestorDeErrores;
            Contexto = contexto;
            Contexto.Mapeador = mapeador;
            Contexto.IniciarTraza();
        }

        protected override void Dispose(bool disposing)
        {
            Contexto.CerrarTraza();
            base.Dispose(disposing);
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
                var rutaConFichero = $@"{GestorDeVariables.RutaDeDescarga}\{fichero.FileName}";

                using (var stream = new FileStream(rutaConFichero, FileMode.Create))
                {
                    fichero.CopyTo(stream);
                }

                if (rutaDestino.IsNullOrEmpty())
                {
                    r.Datos = GestorDocumental.SubirArchivo(contexto, rutaConFichero, mapeador);
                }
                else
                {
                    rutaDestino = $@"{GestorDeVariables.RutaBase}{rutaDestino.Replace("/", @"\")}";

                    if (!Directory.Exists(rutaDestino))
                        Directory.CreateDirectory(rutaDestino);
                    int numero = 1;
                    var ficheroSinExtension = Path.GetFileNameWithoutExtension(fichero.FileName).Replace(" ", "_");
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
                r.consola = GestorDeErrores.Detalle(e);
                if (e.Data.Contains(GestorDeErrores.Datos.Mostrar) && (bool)e.Data[GestorDeErrores.Datos.Mostrar] == true)
                    r.Mensaje = e.Message;
                else
                    r.Mensaje = $"No se ha podido subir el fichero. {(e.Data.Contains(GestorDeErrores.Datos.Mostrar) && (bool)e.Data[GestorDeErrores.Datos.Mostrar] == true ? e.Message : "")}";
            }


            return new JsonResult(r);

        }


        //END-POINT: Desde CrudMantenimiento.ts
        /// <summary>
        /// Devuelve el modo de acceso a los datos del negocio del usuario conectado
        /// </summary>
        /// <param name="negocio">negocio del que se quiere saber el modo de acceso del usuario conectado</param>
        /// <returns>modo de acceso a los datos del negocio</returns>
        public JsonResult epLeerModoDeAccesoAlNegocio(string negocio)
        {
            var r = new Resultado();
            try
            {
                ApiController.CumplimentarDatosDeUsuarioDeConexion(Contexto, Mapeador, HttpContext);
                var modoDeAcceso = LeerModoAccesoAlNegocio(DatosDeConexion.IdUsuario, NegociosDeSe.Negocio(negocio));
                r.ModoDeAcceso = modoDeAcceso.Render();
                r.consola = $"El usuario {DatosDeConexion.Login} tiene permisos de {modoDeAcceso}";
                r.Estado = enumEstadoPeticion.Ok;
            }
            catch (Exception e)
            {
                ApiController.PrepararError(e, r, $"Error al obtener los permisos sobre el negocio {negocio} para el usuario {DatosDeConexion.Login}.");
            }
            return new JsonResult(r);
        }

        //END-POINT: Desde CrudEdicion.ts
        public JsonResult epLeerPorId(int id, string parametrosJson = null)
        {
            var r = new Resultado();
            var parametros = new Dictionary<string, object>();
            if (!parametrosJson.IsNullOrEmpty())
            {
                var parametrosIn = JsonConvert.DeserializeObject<List<Parametro>>(parametrosJson);
                foreach (var p in parametrosIn)
                    parametros.Add(p.parametro, p.valor);
            }

            try
            {
                ApiController.CumplimentarDatosDeUsuarioDeConexion(Contexto, Mapeador, HttpContext);

                var elemento = LeerPorId(id, parametros);
                var modoDeAcceso = LeerModoDeAccesoAlElemento(elemento);
                if (modoDeAcceso == enumModoDeAccesoDeDatos.SinPermiso)
                    GestorDeErrores.Emitir("El usuario conectado no tiene acceso al elemento solicitado");

                r.Datos = elemento;
                r.ModoDeAcceso = modoDeAcceso.Render();
                r.Estado = enumEstadoPeticion.Ok;
                r.Mensaje = $"registro leido";
            }
            catch (Exception e)
            {
                ApiController.PrepararError(e, r, "Error al leer.");
            }
            return new JsonResult(r);
        }


        protected virtual enumModoDeAccesoDeDatos LeerModoAccesoAlNegocio(int idUsuario, enumNegocio negocio)
        {
            return GestorDeNegocios.LeerModoDeAcceso(Contexto, negocio);
        }

        protected virtual TElemento LeerPorId(int id, Dictionary<string, object> parametros)
        {
            return null;
        }
        protected virtual enumModoDeAccesoDeDatos LeerModoDeAccesoAlElemento(TElemento elemento)
        {
            return GestorDeNegocios.LeerModoDeAccesoAlElemento(Contexto, NegociosDeSe.ParsearDto(elemento.GetType().Name), elemento.Id);
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
