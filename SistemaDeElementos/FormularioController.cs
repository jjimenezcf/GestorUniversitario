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
            ApiController.CumplimentarDatosDeUsuarioDeConexion(Contexto, Mapeador, HttpContext);
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
            return SubirArchivo(Contexto, Mapeador, HttpContext, fichero, rutaDestino, extensionesValidas);
        }
    }
}
