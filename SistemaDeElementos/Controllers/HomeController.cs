using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Models;
using Gestor.Errores;
using System;
using ServicioDeDatos;
using Microsoft.AspNetCore.Authorization;
using ModeloDeDto.Entorno;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace MVCSistemaDeElementos.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController(ContextoSe contexto, GestorDeErrores gestorDeErrores):
        base(gestorDeErrores, contexto.DatosDeConexion)
        {
        }

        [Authorize]
        public IActionResult Index()
        {

            DatosDeConexion.Login = ObtenerUsuarioDeLaRequest(); 
            ViewBag.DatosDeConexion = DatosDeConexion;
            return View();
        }


        protected IActionResult PanelDeControl(UsuarioDto usuario)
        {
            DatosDeConexion.Login = usuario.Login;

            var claimsDeUsuario = HttpContext.User; //new ClaimsPrincipal(claimsIdentity);

            var login =  claimsDeUsuario.FindFirstValue(nameof(UsuarioDto.Login));
            DatosDeConexion.Login = login;
            ViewBag.DatosDeConexion = DatosDeConexion;
            return View("PanelDeControl");
        }

        public IActionResult About()
        {
            try
            {
                int[] a = { 2, 4 };
                var b = 0;
                b = a[5];
                ViewData["Message"] = "Your application description page.";
            }
            catch(Exception e)
            {
                
                return Error(e);
            }

            return View();
        }

        public IActionResult Contact()
        {
            int[] a = { 2, 4 };
            var b = 0;
            b = a[5];

            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(Exception e)
        {
            Gestor.Errores.GestorDeErrores.EnviarExcepcionPorCorreo($"Error al ejecutar la petición",e);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
