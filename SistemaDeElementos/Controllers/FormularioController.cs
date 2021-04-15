﻿using Microsoft.AspNetCore.Mvc;
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
using ModeloDeDto.Entorno;

namespace MVCSistemaDeElementos.Controllers
{
    [Authorize]
    public class FormularioController<TContexto> : BaseController<UsuarioDto>
    where TContexto : ContextoSe
    {
        public DescriptorDeFormulario Formulario { get; }

        public FormularioController(TContexto contexto, IMapper mapeador, DescriptorDeFormulario descriptor, GestorDeErrores gestorErrores)
        : base(gestorErrores, contexto, mapeador)
        {
            Formulario = descriptor;
        }

        public IActionResult Index()
        {
            return View();
        }


        public ViewResult ViewFormulario()
        {
            ApiController.CumplimentarDatosDeUsuarioDeConexion(Contexto, Mapeador, HttpContext);
            Formulario.GestorDeUsuario = GestorDeUsuarios.Gestor(Contexto, Mapeador);
            Formulario.UsuarioConectado = Formulario.GestorDeUsuario.LeerRegistroCacheado(nameof(UsuarioDtm.Login), DatosDeConexion.Login);
            ViewBag.DatosDeConexion = DatosDeConexion;

            var destino = $"{(Formulario.RutaVista.IsNullOrEmpty() ? "" : $"../{Formulario.RutaVista}/")}{Formulario.Vista}";
            if (!this.ExisteLaVista(destino))
                return RenderMensaje($"La vista {destino} no está definida");

            if (!Formulario.UsuarioConectado.EsAdministrador)
            {
                string nombreDeLaVista = ControllerContext.RouteData.Values["action"].ToString();
                string nombreDelControlador = ControllerContext.RouteData.Values["controller"].ToString();
                var hayPermisos = Formulario.GestorDeUsuario.TienePermisoFuncional(Formulario.UsuarioConectado, $"{nombreDelControlador}.{nombreDeLaVista}");
                if (!hayPermisos)
                    return RenderMensaje($"Solicite permisos de acceso a {destino}");
            }

            return base.View(destino, Formulario);
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