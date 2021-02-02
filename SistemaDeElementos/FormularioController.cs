using Microsoft.AspNetCore.Mvc;
using System;
using Gestor.Errores;
using Utilidades;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos;
using GestoresDeNegocio.Entorno;
using Microsoft.AspNetCore.Authorization;
using ServicioDeDatos.Entorno;
using AutoMapper;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace MVCSistemaDeElementos.Controllers
{
    [Authorize]
    public class FormularioController<TContexto>: BaseController
        where TContexto : ContextoSe
    {
        public TContexto Contexto { get; }
        public IMapper Mapeador { get; }
        public DescriptorDeFormulario Descriptor { get; }

        public FormularioController(TContexto contexto, IMapper mapeador, DescriptorDeFormulario descriptor, GestorDeErrores gestorErrores)
        : base(gestorErrores, contexto.DatosDeConexion)
        {
            Contexto = contexto;
            Mapeador = mapeador;
            Descriptor = descriptor;
        }


        public IActionResult Index()
        {
            return View();
        }


        public ViewResult ViewFormulario()
        {
            CumplimentarDatosDeUsuarioDeConexion();
            Descriptor.GestorDeUsuario = GestorDeUsuarios.Gestor(Contexto, Mapeador);
            Descriptor.UsuarioConectado = Descriptor.GestorDeUsuario.LeerRegistroCacheado(nameof(UsuarioDtm.Login), DatosDeConexion.Login);

            if (!Descriptor.UsuarioConectado.EsAdministrador)
            {
                string nombreDeLaVista = ControllerContext.RouteData.Values["action"].ToString();
                string nombreDelControlador = ControllerContext.RouteData.Values["controller"].ToString();
                var hayPermisos = Descriptor.GestorDeUsuario.TienePermisoFuncional(Descriptor.UsuarioConectado, $"{nombreDelControlador}.{nombreDeLaVista}");
                if (!hayPermisos)
                    throw new Exception($"El usuario {Descriptor.UsuarioConectado.Login} no tiene permisos de acceso a la vista {nombreDelControlador}.{nombreDeLaVista}");
            }

            ViewBag.DatosDeConexion = DatosDeConexion;

            var destino = $"{(Descriptor.RutaVista.IsNullOrEmpty() ? "" : $"../{Descriptor.RutaVista}/")}{Descriptor.Vista}";

            if (!this.ExisteLaVista(destino))
            {
                ViewBag.Vista = destino;
                return VistaNoDefinida(destino);
            }


            return base.View(destino, Descriptor);
        }

        /// <summary>
        /// END-POIN: desde el ApiDeArchivos. Sube un fichero al gestor documental o a la ruta indicada
        /// </summary>
        /// <param name="fichero">fichero a subir</param>
        /// <param name="rutaDestino">si no se sube al gestor documenta, nombre de la ruta donde se almacenará</param>
        /// <param name="extensionesValidas">extensiones que ha de tener el archivo a subir</param>
        /// <returns>0 si no ha subido al gestor documental, o id del archivo subido al gestor documental</returns>
        [HttpPost]
        public JsonResult epSubirArchivo(IFormFile fichero, string rutaDestino, string extensionesValidas)
        {
            var r = new Resultado();

            try
            {
                if (fichero == null)
                    GestorDeErrores.Emitir("No se ha identificado el fichero");

                CumplimentarDatosDeUsuarioDeConexion();
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
                    r.Datos = GestoresDeNegocio.Archivos.GestorDocumental.SubirArchivo(Contexto, rutaConFichero, Mapeador);
                }
                else
                {
                    rutaDestino = $@"{rutaBase}{rutaDestino.Replace("/", @"\")}";

                    if (!Directory.Exists(rutaDestino))
                        Directory.CreateDirectory(rutaDestino);

                    if (!System.IO.File.Exists($@"{rutaDestino}\{fichero.FileName}"))
                        System.IO.File.Move(rutaConFichero, $@"{rutaDestino}\{fichero.FileName}");

                    r.Datos = fichero.FileName;
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


        private void ValidarExtension(IFormFile fichero, string extensiones)
        {
            if (extensiones.IsNullOrEmpty() || extensiones.EndsWith("*"))
                return;

            if (EsImagen(fichero) && (extensiones.Contains("png")
                                   || extensiones.Contains("jpg")
                                   || extensiones.Contains("svg")
                                   ))
                return;

            throw new Exception($"Para el tipo de fichero {fichero.ContentType} sólo se aceptan '{extensiones}'");

        }
        private bool EsImagen(IFormFile fichero)
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


        protected void CumplimentarDatosDeUsuarioDeConexion()
        {
            DatosDeConexion.Login = ObtenerUsuarioDeLaRequest();
            var gestorDeUsuario = GestorDeUsuarios.Gestor(Contexto, Mapeador);
            var usuario = gestorDeUsuario.LeerRegistroCacheado(nameof(UsuarioDtm.Login), DatosDeConexion.Login);
            DatosDeConexion.IdUsuario = usuario.Id;
            DatosDeConexion.EsAdministrador = usuario.EsAdministrador;
        }


    }
}
