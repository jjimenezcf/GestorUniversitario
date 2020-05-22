using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Models;
using Gestor.Errores;
using System;
using ServicioDeDatos;

namespace MVCSistemaDeElementos.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController(ContextoSe contexto, GestorDeErrores gestorDeErrores):
        base(gestorDeErrores)
        {
            DatosDeConexion = contexto.DatosDeConexion;
        }


        public IActionResult Index()
        {
            ViewBag.DatosDeConexion = DatosDeConexion;
            return View();
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
            GestorDeErrores.Enviar($"Error al ejecutar la petición",e);
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
