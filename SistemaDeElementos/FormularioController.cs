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

        private void CumplimentarDatosDeUsuarioDeConexion()
        {
            DatosDeConexion.Login = ObtenerUsuarioDeLaRequest();
            var gestorDeUsuario = GestorDeUsuarios.Gestor(Contexto, Mapeador);
            var usuario = gestorDeUsuario.LeerRegistroCacheado(nameof(UsuarioDtm.Login), DatosDeConexion.Login);
            DatosDeConexion.IdUsuario = usuario.Id;
            DatosDeConexion.EsAdministrador = usuario.EsAdministrador;
        }


    }
}
