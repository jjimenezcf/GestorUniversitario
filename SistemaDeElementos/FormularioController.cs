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
        public IMapper Mapeador => Contexto.Mapeador;
        public DescriptorDeFormulario Descriptor { get; }

        public FormularioController(TContexto contexto, IMapper mapeador, DescriptorDeFormulario descriptor, GestorDeErrores gestorErrores)
        : base(gestorErrores, contexto.DatosDeConexion)
        {
            Contexto = contexto;
            Contexto.Mapeador = mapeador;
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
            ViewBag.DatosDeConexion = DatosDeConexion;

            var destino = $"{(Descriptor.RutaVista.IsNullOrEmpty() ? "" : $"../{Descriptor.RutaVista}/")}{Descriptor.Vista}";
            if (!this.ExisteLaVista(destino))
                return RenderMensaje($"La vista {destino} no está definida");

            if (!Descriptor.UsuarioConectado.EsAdministrador)
            {
                string nombreDeLaVista = ControllerContext.RouteData.Values["action"].ToString();
                string nombreDelControlador = ControllerContext.RouteData.Values["controller"].ToString();
                var hayPermisos = Descriptor.GestorDeUsuario.TienePermisoFuncional(Descriptor.UsuarioConectado, $"{nombreDelControlador}.{nombreDeLaVista}");
                if (!hayPermisos)
                    return RenderMensaje($"Solicite permisos de acceso a {destino}");
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
