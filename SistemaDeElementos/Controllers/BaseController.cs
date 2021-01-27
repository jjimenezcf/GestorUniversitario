using Gestor.Errores;
using GestorDeElementos;
using GestoresDeNegocio.Entorno;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModeloDeDto.Entorno;
using ServicioDeDatos;
using ServicioDeDatos.Seguridad;

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

        protected string ObtenerUsuarioDeLaRequest()
        {
            if (HttpContext == null)
                return null;

            if (HttpContext.User == null)
                GestorDeErrores.Emitir("Conexión no establecidad");

            var caracter = HttpContext.User.FindFirst(nameof(UsuarioDto.Login));
            if (caracter == null)
                GestorDeErrores.Emitir("Usuario no definido");

            return HttpContext.User.FindFirst(nameof(UsuarioDto.Login)).Value;
        }


        protected ViewResult VistaNoDefinida(string vista)
        {
            return View("VistaNoDefinida");
        }
    }
}
